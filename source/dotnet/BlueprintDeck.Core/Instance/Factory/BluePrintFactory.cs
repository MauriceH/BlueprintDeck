using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Design;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConstantValueNode = BlueprintDeck.Node.Default.ConstantValueNode;

namespace BlueprintDeck.Instance.Factory
{
    internal class BluePrintFactory : IBluePrintFactory
    {
        private readonly IServiceProvider _parentScope;
        private readonly INodeFactory _nodeFactory;
        private readonly IConstantValueSerializerRepository _serializerRepository;

        public BluePrintFactory(IServiceProvider parentScope, INodeFactory nodeFactory, IConstantValueSerializerRepository serializerRepository)
        {
            _parentScope = parentScope;
            _nodeFactory = nodeFactory;
            _serializerRepository = serializerRepository;
        }


        public IBluePrintInstance CreateBluePrint(Design.BluePrint design)
        {
            if (design == null) throw new ArgumentNullException(nameof(design));

            var scope = _parentScope.CreateScope();
            var nodeOrder = new List<NodeInstance>();
            var nodes = new List<NodeInstance>();
            var values = new List<ConstantValueInstance>();


            if ((design.Nodes?.Count ?? 0) == 0)
            {
                //throw new InvalidBluePrintException("Blueprint nodes cannot be empty");
                design.Nodes = new List<Design.Node>();
            }

            if ((design.Connections?.Count ?? 0) == 0)
            {
                design.Connections = new List<Connection>();
                //throw new InvalidBluePrintException("Blueprint has no connections");
            }

            if ((design.ConstantValues?.Count ?? 0) == 0)
            {
                //throw new InvalidBluePrintException("Blueprint nodes cannot be empty");
                design.ConstantValues = new List<Design.ConstantValueNode>();
            }

            // Load all nodes, lookup input or output type of ports for constant value connections and initialize constant values

            foreach (var designNode in design.Nodes!)
            {
                if (designNode == null) throw new InvalidBluePrintException("Blueprint node is null");
                if (string.IsNullOrWhiteSpace(designNode.Key)) throw new InvalidBluePrintException("Blueprint node key is null or empty");

                if (string.IsNullOrWhiteSpace(designNode.NodeTypeKey))
                    throw new InvalidBluePrintException($"Blueprint type-key is null or empty for node \"{designNode.Key!}\"");

                var nodeCreateResult = _nodeFactory.CreateNode(scope, designNode.NodeTypeKey!, designNode);
                var nodeLifeTimeId = Guid.NewGuid().ToString();


                var nodeInstance = new NodeInstance(nodeLifeTimeId, designNode, nodeCreateResult.Node, nodeCreateResult.Registration);

                foreach (var definition in nodeCreateResult.Registration.PortDefinitions)
                {
                    nodeInstance.Ports.Add(new PortInstance(definition));
                }

                nodes.Add(nodeInstance);
            }

            foreach (var designValueNode in design.ConstantValues!)
            {
                if (designValueNode == null) throw new InvalidBluePrintException("Blueprint node is null");
                if (string.IsNullOrWhiteSpace(designValueNode.Key)) throw new InvalidBluePrintException("Blueprint node key is null or empty");

                if (string.IsNullOrWhiteSpace(designValueNode.NodeTypeKey))
                    throw new InvalidBluePrintException($"Blueprint type-key is null or empty for constant value node \"{designValueNode.Key!}\"");

                var nodeCreateResult = _nodeFactory.CreateConstantValueNode(scope, designValueNode.NodeTypeKey!);
                var nodeLifeTimeId = Guid.NewGuid().ToString();
                var registration = nodeCreateResult.Registration;
                var nodeRegistration = new NodeRegistration(registration.Key, registration.Title, typeof(ConstantValueNode),
                    new List<NodePortDefinition> { registration.PortDefinition }, new List<string>());


                var nodeInstance = new NodeInstance(nodeLifeTimeId, designValueNode, nodeCreateResult.Node, nodeRegistration);

                //Ugly implementation
                var constantValueNode = (ConstantValueNode)nodeCreateResult.Node;


                nodeInstance.Ports.Add(new PortInstance(registration.PortDefinition));

                var serializer = _serializerRepository.LoadSerializer(registration.DataType);
                if (serializer == null) throw new Exception("No serializer found");

                constantValueNode.Value = serializer.Deserialize(designValueNode.Value);

                nodes.Add(nodeInstance);
            }


            var openConnections = design.Connections?.ToList() ?? new List<Connection>();

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

            return new BluePrint(scope.ServiceProvider.GetRequiredService<ILogger<BluePrint>>(), scope, nodeOrder, values);
        }
    }
}