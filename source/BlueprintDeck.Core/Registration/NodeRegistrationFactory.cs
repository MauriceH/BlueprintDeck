using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BlueprintDeck.Node;

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

            if (!typeof(INode).IsAssignableFrom(type)) return null;

            // ReSharper disable once ConstantNullCoalescingCondition
            var id = attribute.Id ?? Encoding.UTF8.GetString(_sha1.ComputeHash(Encoding.UTF8.GetBytes(type.FullName ?? type.Name)));

            var portDefinitions = PortDefinitionFactory.CreatePortDefinitions(type);
            var genericTypes = GenericTypeParametersFactory.CreateGenericTypeList(type);

            return new NodeRegistration(id, attribute.Title, type, portDefinitions, genericTypes);
        }
    }
}