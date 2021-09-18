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
    internal class BlueprintFactory : IBlueprintFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INodeFactory _nodeFactory;
        private readonly IConstantValueSerializerRepository _serializerRepository;

        public BlueprintFactory(IServiceProvider serviceProvider, INodeFactory nodeFactory, IConstantValueSerializerRepository serializerRepository)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
            _serializerRepository = serializerRepository ?? throw new ArgumentNullException(nameof(serializerRepository));
        }


        public IBlueprintInstance CreateBlueprint(Design.Blueprint design)
        {
            if (design == null) throw new ArgumentNullException(nameof(design));

            var scope = _serviceProvider.CreateScope();
            var nodeOrder = new List<NodeInstance>();
            var nodes = new List<NodeInstance>();
            var values = new List<ConstantValueInstance>();


            design.Nodes ??= new List<Design.Node>();
            design.Connections ??= new List<Connection>();
            design.ConstantValues ??= new List<Design.ConstantValueNode>();

            // Load all nodes, lookup input or output type of ports for constant value connections and initialize constant values

            foreach (var designNode in design.Nodes)
            {
                if (designNode == null) throw new InvalidBlueprintException("Blueprint node is null");
                if (string.IsNullOrWhiteSpace(designNode.Id)) throw new InvalidBlueprintException("Blueprint node key is null or empty");

                if (string.IsNullOrWhiteSpace(designNode.NodeTypeKey))
                    throw new InvalidBlueprintException($"Blueprint type-key is null or empty for node \"{designNode.Id!}\"");

                var nodeCreateResult = _nodeFactory.CreateNode(scope, designNode.NodeTypeKey!, designNode);
                var nodeLifeTimeId = Guid.NewGuid().ToString();


                var nodeInstance = new NodeInstance(nodeLifeTimeId, designNode, nodeCreateResult.Node, nodeCreateResult.Registration);

                foreach (var definition in nodeCreateResult.Registration.Ports)
                {
                    nodeInstance.Ports.Add(new PortInstance(definition));
                }

                nodes.Add(nodeInstance);
            }

            foreach (var designValueNode in design.ConstantValues)
            {
                if (designValueNode == null) throw new InvalidBlueprintException("Blueprint node is null");
                if (string.IsNullOrWhiteSpace(designValueNode.Id)) throw new InvalidBlueprintException("Blueprint node key is null or empty");

                if (string.IsNullOrWhiteSpace(designValueNode.NodeTypeKey))
                    throw new InvalidBlueprintException($"Blueprint type-key is null or empty for constant value node \"{designValueNode.Id!}\"");

                var nodeCreateResult = _nodeFactory.CreateConstantValueNode(scope, designValueNode.NodeTypeKey!);
                var nodeLifeTimeId = Guid.NewGuid().ToString();
                var registration = nodeCreateResult.Registration;
                var nodeRegistration = new NodeRegistration(registration.Key, registration.Title, typeof(ConstantValueNode),
                    new List<PortRegistration> { registration.Port }, new List<string>(), new List<PropertyRegistration>());


                var nodeInstance = new NodeInstance(nodeLifeTimeId, designValueNode, nodeCreateResult.Node, nodeRegistration);

                //Ugly implementation
                var constantValueNode = (ConstantValueNode)nodeCreateResult.Node;


                nodeInstance.Ports.Add(new PortInstance(registration.Port));

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

                    var outputPorts = node.Ports.Where(x => x.Definition.Direction == Direction.Output);
                    foreach (var outputPort in outputPorts)
                    {
                        outputPort.InitializeAsOutput();
                        if (outputPort == null)
                        {
                            throw new Exception("output not initialized");
                        }

                        var portConnections = openConnections.Where(x => x.NodeFrom == node.Design.Id && x.NodePortFrom == outputPort.Definition.Key)
                            .ToList();

                        foreach (var connection in portConnections)
                        {
                            openConnections.Remove(connection);
                            var toNode = nodes.FirstOrDefault(x => x.Design.Id == connection.NodeTo);
                            var toPort = toNode?.Ports.FirstOrDefault(x => x.Definition.Key == connection.NodePortTo);
                            toPort?.InitializeAsInput(outputPort.InputOutput!);
                        }
                    }
                }
            }

            return new Blueprint(scope.ServiceProvider.GetRequiredService<ILogger<Blueprint>>(), scope, nodeOrder, values);
        }
    }
}