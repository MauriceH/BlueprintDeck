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

        public BluePrintFactory(ILifetimeScope parentScope, INodeRepository nodeRepository, Func<ILogger<BluePrint>> loggerFactory, IConstantValueSerializerRepository serializerRepository)
        {
            _parentScope = parentScope;
            _nodeRepository = nodeRepository;
            _loggerFactory = loggerFactory;
            _serializerRepository = serializerRepository;
        }

       
        public BluePrint CreateBluePrint(Design.BluePrint design)
        {
            var lifeTimeId = Guid.NewGuid().ToString();
            var scope = _parentScope.BeginLifetimeScope(lifeTimeId);
            var nodeOrder = new List<NodeInstance>();
            var nodes = new List<NodeInstance>();
            var values = new List<ConstantValueInstance>();
            
            foreach (var constantValue in design.ConstantValues)
            {
                var serializer = _serializerRepository.LoadSerializer(constantValue.Type);
                var dataType = serializer.GetDataType();
                var instance = new ConstantValueInstance(constantValue, dataType)
                {
                    Value = serializer.Deserialize(constantValue.Value)
                };
                values.Add(instance);
            }
            
            
            foreach (var designNode in design.Nodes)
            {
                var nodeCreateResult = _nodeRepository.CreateNode(scope, designNode.NodeTypeKey);
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
            
            foreach (var constantValue in design.ConstantValues)
            {
                var instance = values.FirstOrDefault(x => x.Description.Key == constantValue.Key);
                if(instance == null) throw new Exception("Invalid blueprint state");
                var connection = openConnections.FirstOrDefault(x => x.IsConstantConnection && x.ConstantKey == constantValue.Key);
                if(connection == null) continue;
                var toNode = nodes.FirstOrDefault(x => x.Design.Key == connection.NodeTo);
                var toPort = toNode?.Ports.FirstOrDefault(x => x.Definition.Key == connection.NodePortTo);
                toPort?.InitializeAsInputToConstantValue(instance);
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

                        var portConnections = openConnections.Where(x => x.NodeFrom == node.Design.Key && x.NodePortFrom == outputPort.Definition.Key).ToList();

                        foreach (var connection in portConnections)
                        {
                            openConnections.Remove(connection);
                            var toNode = nodes.FirstOrDefault(x => x.Design.Key == connection.NodeTo);
                            var toPort = toNode?.Ports.FirstOrDefault(x => x.Definition.Key == connection.NodePortTo);
                            toPort?.InitializeAsInput(outputPort.InputOutput);
                        }
                    }
                }
            }

            return new BluePrint(scope, nodeOrder, values, _loggerFactory.Invoke());
        }
    }
}