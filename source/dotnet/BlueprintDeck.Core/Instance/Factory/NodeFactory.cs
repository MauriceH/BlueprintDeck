using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public CreateNodeResult<NodeRegistration> CreateNode(IServiceScope scope, string nodeTypeKey, Design.Node designNode)
        {
            if (!_nodeRegistrations.TryGetValue(nodeTypeKey, out var nodeRegistration))
            {
                throw new Exception("Node not found");
            }

            var nodeType = nodeRegistration.NodeType;

            if (nodeType.IsGenericType)
            {
                var genericParameter = nodeType.GetTypeInfo().GenericTypeParameters;
                if (designNode.GenericTypes?.Count != genericParameter.Length)
                {
                    throw new Exception("Invalid generic parameters");
                }
                
                nodeType = nodeType.MakeGenericType();
            }

            var constructor = nodeType.GetConstructors().First();
            var parameters = new List<object>();
            foreach (var parameter in constructor.GetParameters())
            {
                var parameterType = parameter.ParameterType;
                if (parameterType == typeof(Design.Node))
                {
                    parameters.Add(designNode);
                    continue;
                }
                parameters.Add(scope.ServiceProvider.GetRequiredService(parameterType));
            }


            var node = (INode)constructor.Invoke(parameters.ToArray());
            return new CreateNodeResult<NodeRegistration>(nodeRegistration, node);
        }

        public CreateNodeResult<ConstantValueRegistration> CreateConstantValueNode(IServiceScope scope, string constantValueNodeTypeKey)
        {
            if (!_constantValueRegistrations.TryGetValue(constantValueNodeTypeKey, out var registration))
            {
                throw new Exception("ConstantValue not found");
            }
            var constantValueNode = (INode) new ConstantValueNode(registration.ActivationCall);
            return new CreateNodeResult<ConstantValueRegistration>(registration, constantValueNode);
        }
    }
}