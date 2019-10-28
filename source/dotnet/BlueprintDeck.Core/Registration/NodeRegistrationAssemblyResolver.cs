using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    public class NodeRegistrationAssemblyResolver
    {
        public IList<NodeRegistration> ResolveNodeRegistrations(Assembly assembly)
        {
            var sha1 = SHA1.Create();
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
                    var attribute = type.GetCustomAttribute<NodeDescriptorAttribute>(true);
                    if (attribute == null) continue;

                    if(!typeof(INode).IsAssignableFrom(type)) continue;
                   
                    
                    var id = attribute.Id ?? Encoding.UTF8.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(type.FullName ?? type.Name)));

                    var controller = (INodeDescriptor) Activator.CreateInstance(attribute.PortDescriptor);
                    
                    registrations.Add(new NodeRegistration(id, attribute.Title, type,attribute.PortDescriptor ,controller.PortDefinitions, false));
                }
                catch (Exception)
                {
                    //ignored
                }
            }
            
            return registrations;
        }
    }
}