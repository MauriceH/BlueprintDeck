using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck
{
    public class AutofacNodeRepository : INodeRepository
    {
        private readonly ILifetimeScope _parentScope;
        private readonly Dictionary<string, NodeRegistration> _nodeTypes;

        public AutofacNodeRepository(ILifetimeScope parentScope)
        {
            _parentScope = parentScope;
            _nodeTypes = _parentScope.ComponentRegistry.Registrations
                .Where(r => typeof(INode).IsAssignableFrom(r.Activator.LimitType))
                .Select(r =>
                {
                    var type = r.Activator.LimitType;
                    var descriptor = type.GetCustomAttributes<NodeDescriptorAttribute>()?.FirstOrDefault();
                    return descriptor == null ? null : new NodeRegistration(type, descriptor);
                })
                .Where(x => x?.Descriptor != null)
                .ToDictionary(x => x.Descriptor.Id);
        }

        public INodeDescriptor CreatePortDescriptor(string nodeTypeKey)
        {
            var registration = ResolveRegistration(nodeTypeKey);
            return (INodeDescriptor)_parentScope.Resolve(registration.Descriptor.PortDescriptor);
        }

        public CreateNodeResult CreateNode(ILifetimeScope scope, string nodeTypeKey)
        {
            var registration = ResolveRegistration(nodeTypeKey);

            var node = (INode)scope.Resolve(registration.NodeType);
            
            return new CreateNodeResult(registration.Descriptor, node);
        }

        private NodeRegistration ResolveRegistration(string nodeTypeKey)
        {
            if (!_nodeTypes.TryGetValue(nodeTypeKey, out var registration))
            {
                throw new NodeTypeNotFoundException(nodeTypeKey);
            }
            return registration;
        }

        private class NodeRegistration
        {
            public NodeRegistration(Type nodeType, NodeDescriptorAttribute descriptor)
            {
                NodeType = nodeType;
                Descriptor = descriptor;
            }

            public Type NodeType { get; }
            public NodeDescriptorAttribute Descriptor { get; }
        }
    }
}