using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BlueprintDeck.Design;
using BlueprintDeck.Instance;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck
{
    public class BluePrintFactory
    {
        private readonly ILifetimeScope _parentScope;
        private readonly INodeRepository _nodeRepository;

        public BluePrintFactory(ILifetimeScope parentScope, INodeRepository nodeRepository)
        {
            _parentScope = parentScope;
            _nodeRepository = nodeRepository;
        }


        public BluePrint CreateBluePrint(BluePrintDesign design)
        {
            var lifeTimeId = Guid.NewGuid().ToString();
            var scope = _parentScope.BeginLifetimeScope(lifeTimeId);
            var nodeOrder = new List<NodeInstance>();
            var nodes = new List<NodeInstance>();
            foreach (var designNode in design.Nodes)
            {
                var nodeCreateResult = _nodeRepository.CreateNode(scope, designNode.NodeTypeKey);
                var nodeLifeTimeId = Guid.NewGuid().ToString();
                var nodeInstance = new NodeInstance(nodeLifeTimeId, designNode, nodeCreateResult.Node, nodeCreateResult.Descriptor);

                if (nodeInstance.IsValueNode)
                {
                    nodeInstance.SetValue(designNode.Value);
                }

                var portDescriptor = _nodeRepository.CreatePortDescriptor(nodeInstance.Descriptor.Id);
                foreach (var definition in portDescriptor.PortDefinitions)
                {
                    nodeInstance.Ports.Add(new PortInstance(definition));
                }

                nodes.Add(nodeInstance);
            }

            var toConnectNodes = nodes.ToList();
            var openConnections = design.Connections.ToList();
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

                        var portConnections = openConnections.Where(x => x.NodeFrom == node.Design.NodeInstanceId && x.NodePortFrom == outputPort.Definition.Key).ToList();

                        foreach (var connection in portConnections)
                        {
                            openConnections.Remove(connection);
                            var toNode = nodes.FirstOrDefault(x => x.Design.NodeInstanceId == connection.NodeTo);
                            var toPort = toNode?.Ports.FirstOrDefault(x => x.Definition.Key == connection.NodePortTo);
                            toPort?.InitializeAsInput(outputPort.InputOutput);
                        }
                    }
                }
            }

            return new BluePrint(scope, nodeOrder);
        }
    }
}