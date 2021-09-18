using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Misc;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;

namespace BlueprintDeck.Node.Registration
{
    internal class NodeRegistrationFactory
    {
        private readonly IAssemblyTypesResolver _assemblyTypesResolver;
        private readonly IPortRegistrationFactory _portFactory;
        private readonly IGenericTypeParametersFactory _genericTypeFactory;
        private readonly IPropertyRegistrationFactory _propertyFactory;
        private readonly SHA1 _sha1;

        public NodeRegistrationFactory(IAssemblyTypesResolver assemblyTypesResolver, 
            IPortRegistrationFactory portFactory,
            IGenericTypeParametersFactory genericTypeFactory,
            IPropertyRegistrationFactory propertyFactory)
        {
            _assemblyTypesResolver = assemblyTypesResolver;
            _portFactory = portFactory;
            _genericTypeFactory = genericTypeFactory;
            _propertyFactory = propertyFactory;
            _sha1 = SHA1.Create();
        }

        internal IEnumerable<NodeRegistration> CreateNodeRegistrationsByAssembly(Assembly assembly)
        {
            var registrations = new List<NodeRegistration>();

            var types = _assemblyTypesResolver.ResolveTypes(assembly);

            foreach (var type in types)
            {
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
            var attribute = type.GetCustomAttribute<NodeAttribute>(true);
            if (attribute == null) return null;

            if (!typeof(INode).IsAssignableFrom(type)) return null;

            // ReSharper disable once ConstantNullCoalescingCondition
            var id = attribute.Id ?? Encoding.UTF8.GetString(_sha1.ComputeHash(Encoding.UTF8.GetBytes(type.FullName ?? type.Name)));

            var portDefinitions = _portFactory.CreatePortRegistrations(type);
            var genericTypes = _genericTypeFactory.CreateGenericTypeList(type);
            var properties = _propertyFactory.CreatePropertyRegistrations(type);

            return new NodeRegistration(id, attribute.Title, type, portDefinitions, genericTypes, properties);
        }
    }
}