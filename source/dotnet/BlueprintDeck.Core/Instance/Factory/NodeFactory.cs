using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.Instance.Factory
{
    internal class NodeFactory : INodeFactory
    {
        private readonly Dictionary<string, NodeRegistration> _nodeRegistrations;
        private readonly Dictionary<string, ConstantValueRegistration> _constantValueRegistrations;

        public NodeFactory(IServiceProvider serviceProvider)
        {
            _nodeRegistrations = serviceProvider.GetServices<NodeRegistration>().ToDictionary(x => x.Key, _ => _);
            _constantValueRegistrations = serviceProvider.GetServices<ConstantValueRegistration>().ToDictionary(x => x.Key, _ => _);
        }

        public CreateNodeResult<NodeRegistration> CreateNode(IServiceScope scope, string nodeTypeKey)
        {
            if (!_nodeRegistrations.TryGetValue(nodeTypeKey, out var nodeRegistration))
            {
                throw new Exception("Node not found");
            }
            
            var node = (INode) scope.ServiceProvider.GetRequiredService(nodeRegistration.NodeType);
            return new CreateNodeResult<NodeRegistration>(nodeRegistration, node);
        }

        public CreateNodeResult<ConstantValueRegistration> CreateConstantValueNode(IServiceScope scope, string constantValueNodeTypeKey)
        {
            if (!_constantValueRegistrations.TryGetValue(constantValueNodeTypeKey, out var registration))
            {
                throw new Exception("ConstantValue not found");
            }
            var constantValueNode = (INode) new ConstantValueNode(registration.PortDefinition, registration.ActivationCall);
            return new CreateNodeResult<ConstantValueRegistration>(registration, constantValueNode);
        }
    }
}