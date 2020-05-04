using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BlueprintDeck.Instance;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Registration;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck
{
    public class BluePrintFactory
    {
        private readonly ILifetimeScope _parentScope;
        private readonly INodeRepository _nodeRepository;
        private readonly Func<ILogger<BluePrint>> _loggerFactory;
        private readonly IConstantValueSerializerRepository _serializerRepository;

        public BluePrintFactory(ILifetimeScope parentScope, INodeRepository nodeRepository, Func<ILogger<BluePrint>> loggerFactory,
            IConstantValueSerializerRepository serializerRepository)
        {
            _parentScope = parentScope;
            _nodeRepository = nodeRepository;
            _loggerFactory = loggerFactory;
            _serializerRepository = serializerRepository;
        }


        public BluePrint CreateBluePrint(Design.BluePrint design)
        {
            if (design == null) throw new ArgumentNullException(nameof(design));

            var lifeTimeId = Guid.NewGuid().ToString();
            var scope = _parentScope.BeginLifetimeScope(lifeTimeId);
            var nodeOrder = new List<NodeInstance>();
            var nodes = new List<NodeInstance>();
            var values = new List<ConstantValueInstance>();


            if ((design?.Nodes?.Count ?? 0) == 0)
            {
                throw new InvalidBluePrintException("Blueprint nodes cannot be empty");
            }

            if ((design?.Connections?.Count ?? 0) == 0)
            {
                throw new InvalidBluePrintException("Blueprint has no connections");
            }


            // Load all nodes, lookup input or output type of ports for constant value connections and initialize constant values

            foreach (var designNode in design!.Nodes!)
            {
                if (designNode == null) throw new InvalidBluePrintException("Blueprint node is null");
                if (string.IsNullOrWhiteSpace(designNode.Key)) throw new InvalidBluePrintException("Blueprint node key is null or empty");
                var nodeKey = designNode.Key!;
                if (string.IsNullOrWhiteSpace(designNode.NodeTypeKey))
                    throw new InvalidBluePrintException($"Blueprint type-key is null or empty for node \"{nodeKey}\"");

                var nodeCreateResult = _nodeRepository.CreateNode(scope, designNode.NodeTypeKey!);
                var nodeLifeTimeId = Guid.NewGuid().ToString();
                var nodeInstance = new NodeInstance(nodeLifeTimeId, designNode, nodeCreateResult.Node, nodeCreateResult.Descriptor);
                var portDescriptor = _nodeRepository.CreatePortDescriptor(nodeInstance.Descriptor.Id);
                foreach (var definition in portDescriptor.PortDefinitions)
                {
                    nodeInstance.Ports.Add(new PortInstance(definition));
                }

                nodes.Add(nodeInstance);
            }

            var openConnections = design.Connections.ToList();

            foreach (var connection in design!.Connections!.Where(con => con.IsConstantConnection))
            {
                var connectionKey = connection.ConstantKey ?? throw new Exception("Constant value connection without key found");
                var constantKey = connection.ConstantKey ?? throw new Exception($"Constant value connection \"{connectionKey}\" has no constant-key");
                var nodeKey = connection.NodeTo ?? throw new Exception($"Constant value connection \"{connectionKey}\" has no node-key");
                var portKey = connection.NodePortTo ?? throw new Exception($"Constant value connection \"{connectionKey}\" has no port-key");

                var node = nodes.FirstOrDefault(x => x.Design.Key! == nodeKey) ??
                           throw new Exception($"Node key \"{nodeKey}\" not found for constant value connection \"{connectionKey}\"");

                var port = node.Ports.FirstOrDefault(x => x.Definition.Key == portKey)??
                           throw new Exception($"Node port key \"{portKey}\" not found for constant value connection \"{connectionKey}\"");
                
                
                var portType = port.Definition.PortDataType ??
                    throw new Exception($"Node port with key \"{portKey}\" for constant value connection \"{connectionKey}\" has no type");

                
                var constantValue = design.ConstantValues.FirstOrDefault(x=>x.Key == constantKey) ??
                    throw new Exception($"constant value with key \"{constantKey}\" not found for constant value connection \"{connectionKey}\"");

                var dataValue = constantValue.Value ??
                                    throw new Exception($"constant value with key \"{constantKey}\" for constant value connection \"{connectionKey}\" has no data");
                
                var serializer = _serializerRepository.LoadSerializer(portType);
                
                var instance = new ConstantValueInstance(constantValue, portType)
                {
                    CurrentValue = serializer.Deserialize(dataValue)
                };
                
                port.InitializeAsInputToConstantValue(instance);
                
                values.Add(instance);
                openConnections.Remove(connection);
            }


            var toConnectNodes = nodes.ToList();
            var lastCount = -1;
            while (toConnectNodes.Count > 0 && lastCount != toConnectNodes.Count)
            {
                lastCount = toConnectNodes.Count;
                var nodesReady = toConnectNodes.Where(x => x.AllRequiredInputsConnected).ToList();

                foreach (var node in nodesReady)
                {
                    toConnectNodes.Remove(node);
                    nodeOrder.Add(node);

                    var outputPorts = node.Ports.Where(x => x.Definition.InputOutputType == InputOutputType.Output);
                    foreach (var outputPort in outputPorts)
                    {
                        outputPort.InitializeAsOutput();
                        if (outputPort == null)
                        {
                            throw new Exception("output not initialized");
                        }
                        var portConnections = openConnections.Where(x => x.NodeFrom == node.Design.Key && x.NodePortFrom == outputPort.Definition.Key)
                            .ToList();

                        foreach (var connection in portConnections)
                        {
                            openConnections.Remove(connection);
                            var toNode = nodes.FirstOrDefault(x => x.Design.Key == connection.NodeTo);
                            var toPort = toNode?.Ports.FirstOrDefault(x => x.Definition.Key == connection.NodePortTo);
                            toPort?.InitializeAsInput(outputPort.InputOutput!);
                        }
                    }
                }
            }

            return new BluePrint(_loggerFactory.Invoke(), scope, nodeOrder, values);
        }
    }
}