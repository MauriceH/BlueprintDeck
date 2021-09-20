using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.Instance.Factory
{
    internal class NodeFactory : INodeFactory
    {
        private readonly Dictionary<string, NodeRegistration> _nodeRegistrations;
        private readonly Dictionary<string, DataTypeRegistration> _dataTypeRegistrations;

        public NodeFactory(IServiceProvider serviceProvider)
        {
            _nodeRegistrations = serviceProvider.GetServices<NodeRegistration>().ToDictionary(x => x.Id, _ => _);
            _dataTypeRegistrations = serviceProvider.GetServices<DataTypeRegistration>().ToDictionary(x => x.Id, _ => _);
        }

        public CreateNodeResult CreateNode(IServiceScope scope, string nodeTypeKey, Design.Node designNode)
        {
            if (!_nodeRegistrations.TryGetValue(nodeTypeKey, out var nodeRegistration))
            {
                throw new Exception("Node not found");
            }

            var nodeType = nodeRegistration.NodeType;
            var genericTypeInstances = new List<GenericTypeParameterInstance>();
            
            if (nodeType.IsGenericType)
            {
                var genericParameter = nodeType.GetTypeInfo().GenericTypeParameters;
                if (designNode.GenericTypes?.Count != genericParameter.Length)
                {
                    throw new Exception("Invalid generic parameters");
                }

                foreach (var type in genericParameter)
                {
                    var nodeGenericType = designNode.GenericTypes?.FirstOrDefault(x => x.GenericParameter == type.Name);
                    if (nodeGenericType == null) throw new Exception($"Cannot create node {nodeTypeKey}, generic parameter {type.Name} not set");
                    
                    if(!_dataTypeRegistrations.TryGetValue(nodeGenericType.TypeId ?? "", out var typeRegistration)) 
                        if (nodeGenericType == null) throw new Exception($"Cannot create node {nodeTypeKey}, generic parameter {type.Name} not registered");
                    
                    genericTypeInstances.Add(new GenericTypeParameterInstance(nodeGenericType.GenericParameter!,typeRegistration!.DataType));
                    
                }
                
                nodeType = nodeType.MakeGenericType(genericTypeInstances.Select(x=>x.DataType).ToArray());
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
            return new CreateNodeResult(nodeRegistration, node, genericTypeInstances);
        }
    }
}