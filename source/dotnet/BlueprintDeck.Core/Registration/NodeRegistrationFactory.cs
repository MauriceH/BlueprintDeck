using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Default;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal class NodeRegistrationFactory
    {
        private readonly SHA1 _sha1;

        public NodeRegistrationFactory()
        {
            _sha1 = SHA1.Create();
        }

        internal IEnumerable<NodeRegistration> CreateNodeRegistrationsByAssembly(Assembly assembly)
        {
            var registrations = new List<NodeRegistration>();
            Type?[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                if (e.Types.Length == 0) return registrations;
                types = e.Types;
            }

            foreach (var type in types)
            {
                if (type == null) continue;
                try
                {
                    var nodeRegistration = CreateNodeRegistration(type);
                    if (nodeRegistration == null) continue;
                    registrations.Add(nodeRegistration);
                }
                catch (Exception)
                {
                    //ignored
                }
            }

            return registrations;
        }


        internal NodeRegistration? CreateNodeRegistration<T>()
        {
            return CreateNodeRegistration(typeof(T));
        }


        internal NodeRegistration? CreateNodeRegistration(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var attribute = type.GetCustomAttribute<NodeDescriptorAttribute>(true);
            if (attribute == null) return null;

            if (typeof(INode).IsAssignableFrom(type))
            {
                return NodeRegistrationOld(type, attribute);
            }

            var interfaceType = type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(INode<>));
            if (interfaceType == null) return null;

            var portDescriptorType = interfaceType.GenericTypeArguments.First();


            var portDefinitions = CreatePortDefinitions(portDescriptorType);


            // ReSharper disable once ConstantNullCoalescingCondition
            var id = attribute.Id ?? Encoding.UTF8.GetString(_sha1.ComputeHash(Encoding.UTF8.GetBytes(type.FullName ?? type.Name)));

            var genericTypes = new List<string>();

            if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                genericTypes.AddRange(typeInfo.GenericTypeParameters.Select(x => x.Name));
            }

            return new NodeRegistration(id, attribute.Title, type, portDefinitions.ToList(), genericTypes);
        }

        private IEnumerable<NodePortDefinition> CreatePortDefinitions(Type portDescriptorType)
        {
            var portProperties = portDescriptorType.GetProperties();
            var result = new List<NodePortDefinition>();
            foreach (var property in portProperties)
            {
                var propertyType = property.PropertyType;
                var isInput = propertyType.IsAssignableFrom(typeof(IInput))
                              || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IInput<>));
                var isOutput = propertyType.IsAssignableFrom(typeof(IOutput))
                               || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IOutput<>));

                if (!isInput && !isOutput) continue;

                var inputOutputType = isInput ? InputOutputType.Input : InputOutputType.Output;


                string? portGenericType = null;
                Type? portDataType = null;
                if (propertyType.IsGenericType) // With Data
                {
                    var typeInfo = propertyType.GetTypeInfo();
                    if (typeInfo.ContainsGenericParameters)
                    {
                        portGenericType = typeInfo.GetGenericArguments().First().Name;
                    }
                    else
                    {
                        portDataType = typeInfo.GetGenericArguments().First();
                    }
                }

                var definition = new NodePortDefinition(property.Name, inputOutputType, portDataType, portGenericType);

                var portAttributes = property.GetCustomAttributes();
                foreach (var portAttribute in portAttributes)
                {
                    if (portAttribute is not PortAttribute pa) continue;
                    pa.Setup(definition);
                }
                result.Add(definition);
                //yield return definition;
            }

            return result;
        }

        private NodeRegistration? NodeRegistrationOld(Type type, NodeDescriptorAttribute? attribute)
        {
            // ReSharper disable once ConstantNullCoalescingCondition
            var id = attribute.Id ?? Encoding.UTF8.GetString(_sha1.ComputeHash(Encoding.UTF8.GetBytes(type.FullName ?? type.Name)));

            var descriptor = (INodeDescriptor)Activator.CreateInstance(attribute.NodeDescriptor)!;
            if (descriptor == null) throw new Exception($"Cannot create node descriptor instance for node type {type.Name}");
            var genericTypes = new List<string>();

            if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                genericTypes.AddRange(typeInfo.GenericTypeParameters.Select(x => x.Name));
            }

            return new NodeRegistration(id, attribute.Title, type, descriptor.PortDefinitions, genericTypes);
        }
    }
}