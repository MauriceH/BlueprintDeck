using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueprintDeck.Design;
using BlueprintDeck.Misc;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Instance.Factory
{
    internal class BlueprintFactory : IBlueprintFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INodeFactory _nodeFactory;
        private readonly IPortInstanceFactory _portInstanceFactory;

        public BlueprintFactory(IServiceProvider serviceProvider, INodeFactory nodeFactory, IPortInstanceFactory portInstanceFactory)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));
            _portInstanceFactory = portInstanceFactory ?? throw new ArgumentNullException(nameof(portInstanceFactory));
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

                

                var nodeInstance = new NodeInstance(nodeLifeTimeId, designNode, nodeCreateResult.Node, nodeCreateResult.Registration, nodeCreateResult.GenericTypes);

                foreach (var portRegistration in nodeCreateResult.Registration.Ports)
                {
                    var portInstance = _portInstanceFactory.Create(portRegistration);
                    nodeInstance.Ports.Add(portInstance);
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
                        _portInstanceFactory.InitializeAsOutput(node, outputPort);
                        //outputPort.Registration.Property.SetValue(node.Node,outputPort.InputOutput);
                        node.Node.GetType().GetProperty(outputPort.Registration.Property.Name)!.SetValue(node.Node,outputPort.InputOutput);
                        
                        if (outputPort == null)
                        {
                            throw new Exception("output not initialized");
                        }

                        var portConnections = openConnections.Where(x => x.NodeFrom == node.Design.Id && x.NodePortFrom == outputPort.Registration.Key)
                            .ToList();

                        foreach (var connection in portConnections)
                        {
                            openConnections.Remove(connection);
                            var toNode = nodes.FirstOrDefault(x => x.Design.Id == connection.NodeTo);
                            var toPort = toNode?.Ports.FirstOrDefault(x => x.Registration.Key == connection.NodePortTo);
                            if(toPort == null) throw new Exception("invalid connection");
                            _portInstanceFactory.InitializeAsInput(toNode!, toPort, outputPort.InputOutput!);
                            toNode.Node.GetType().GetProperty(toPort.Registration.Property.Name)!.SetValue(toNode!.Node,toPort.InputOutput);
                        }
                    }
                }
            }

            return new Blueprint(scope.ServiceProvider.GetRequiredService<ILogger<Blueprint>>(), scope, nodeOrder);
        }
    }
}