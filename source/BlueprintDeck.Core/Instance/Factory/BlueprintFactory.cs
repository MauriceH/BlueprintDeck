using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Design;
using BlueprintDeck.Misc;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.ValueSerializer.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlueprintDeck.Instance.Factory
{
    internal class BlueprintFactory : IBlueprintFactory
    {
        private readonly INodeFactory _nodeFactory;
        private readonly IPortConnectionManager _portConnectionManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IValueSerializerRepository _valueSerializerRepository;

        public BlueprintFactory(IServiceProvider serviceProvider, INodeFactory nodeFactory, IPortConnectionManager portConnectionManager, IValueSerializerRepository valueSerializerRepository)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
            _portConnectionManager = portConnectionManager ?? throw new ArgumentNullException(nameof(portConnectionManager));
            _valueSerializerRepository = valueSerializerRepository ?? throw new ArgumentNullException(nameof(valueSerializerRepository));
        }


        public IBlueprintInstance CreateBlueprint(Design.Blueprint design)
        {
            if (design == null) throw new ArgumentNullException(nameof(design));

            var scope = _serviceProvider.CreateScope();
            var nodeOrder = new List<NodeInstance>();
            var nodes = new List<NodeInstance>();


            design.Nodes ??= new List<Design.Node>();
            design.Connections ??= new List<Connection>();

            // Load all nodes, lookup input or output type of ports for constant value connections and initialize constant values

            foreach (var designNode in design.Nodes)
            {
                if (designNode == null) throw new InvalidBlueprintException("Blueprint node is null");
                if (string.IsNullOrWhiteSpace(designNode.Id)) throw new InvalidBlueprintException("Blueprint node key is null or empty");

                if (string.IsNullOrWhiteSpace(designNode.NodeTypeKey))
                    throw new InvalidBlueprintException($"Blueprint type-key is null or empty for node \"{designNode.Id!}\"");

                var nodeCreateResult = _nodeFactory.CreateNode(scope, designNode.NodeTypeKey!, designNode);
                var nodeLifeTimeId = Guid.NewGuid().ToString();


                var nodeInstance = new NodeInstance(nodeLifeTimeId, designNode, nodeCreateResult.Node, nodeCreateResult.Registration,
                    nodeCreateResult.GenericTypes);

                foreach (var portRegistration in nodeCreateResult.Registration.Ports)
                {
                    var portInstance = new PortInstance(portRegistration);
                    nodeInstance.Ports.Add(portInstance);
                }

                foreach (var propertyRegistration in nodeCreateResult.Registration.Properties)
                {
                    var props = designNode?.Properties?.Keys.ToDictionary(x=>x.ToLower(),x=> designNode.Properties[x]);
                    if (!(props?.TryGetValue(propertyRegistration.Name.ToLower(), out var rawValue) ?? false))
                    {
                        continue;
                    }

                    object? value = null;
                    if (propertyRegistration.Type == typeof(string))
                    {
                        value = rawValue;
                    }
                    else
                    {
                        if (_valueSerializerRepository.TryLoadSerializer(propertyRegistration.Type, out var serializer))
                        {
                            value = serializer?.Deserialize(rawValue);
                        }

                        value ??= JsonConvert.DeserializeObject(rawValue, propertyRegistration.Type);
                    }

                    if (value == null) continue;
                    var prop = nodeInstance.Node.GetType().GetProperty(propertyRegistration.Name) ?? throw new Exception($"Property {propertyRegistration.Name} not found on type {nodeInstance.Node.GetType().Name}");
                    prop.SetValue(nodeInstance.Node, value);
                }

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

                    var outputPorts = node.Ports.Where(x => x.Registration.Direction == Direction.Output);
                    foreach (var outputPort in outputPorts)
                    {
                        if (outputPort == null) throw new Exception("output not initialized");

                        _portConnectionManager.InitializePortAsOutput(node, outputPort);
                        //outputPort.Registration.Property.SetValue(node.Node,outputPort.InputOutput);
                        node.Node.GetType().GetProperty(outputPort.Registration.Property.Name)!.SetValue(node.Node, outputPort.InputOutput);


                        var portConnections = openConnections
                            .Where(x => x.NodeFrom == node.Design.Id && x.NodePortFrom == outputPort.Registration.Key)
                            .ToList();

                        foreach (var connection in portConnections)
                        {
                            openConnections.Remove(connection);
                            var toNode = nodes.FirstOrDefault(x => x.Design.Id == connection.NodeTo);
                            var toPort = toNode?.Ports.FirstOrDefault(x => x.Registration.Key == connection.NodePortTo);
                            if (toPort == null) throw new Exception("invalid connection");
                            _portConnectionManager.InitializePortAsInput(toNode!, toPort, outputPort.InputOutput!);
                            toNode!.Node.GetType().GetProperty(toPort.Registration.Property.Name)!.SetValue(toNode.Node, toPort.InputOutput);
                        }
                    }
                }
            }

            return new Blueprint(scope.ServiceProvider.GetRequiredService<ILogger<Blueprint>>(), scope, nodeOrder);
        }
    }
}