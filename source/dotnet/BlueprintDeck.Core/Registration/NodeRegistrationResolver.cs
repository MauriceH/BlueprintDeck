using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    public class NodeRegistrationResolver
    {
        private SHA1? _sha1;

        public NodeRegistrationResolver()
        {
            _sha1 = SHA1.Create();
        }

        internal IList<NodeRegistration> ResolveNodeRegistrations(Assembly assembly)
        {
            
            var registrations = new List<NodeRegistration>();
            
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types;
            }

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


        public NodeRegistration? CreateNodeRegistration<T>()
        {
            return CreateNodeRegistration(typeof(T));
        }
        
        
        public NodeRegistration? CreateNodeRegistration(Type? type)
        {
            var attribute = type.GetCustomAttribute<NodeDescriptorAttribute>(true);
            if (attribute == null) return null;

            if (!typeof(INode).IsAssignableFrom(type)) return null;


            var id = attribute.Id ?? Encoding.UTF8.GetString(_sha1.ComputeHash(Encoding.UTF8.GetBytes(type.FullName ?? type.Name)));

            var controller = (INodeDescriptor) Activator.CreateInstance(attribute.PortDescriptor);

            return new NodeRegistration(id, attribute.Title, type, controller.PortDefinitions);
        }
    }
}