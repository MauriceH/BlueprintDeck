using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Design
{
    public class NodeRegistrationAssemblyResolver : INodeRegistrationResolver
    {
        public IList<NodeRegistration> ResolveNodeRegistrations()
        {
            var sha1 = SHA1.Create();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var registrations = new List<NodeRegistration>();
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    var name = assembly.GetName().Name;
                    if (!name.StartsWith("HomeCon")) continue;
                    if (name.StartsWith("System")) continue;
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

                        var interfaces = type.GetInterfaces();

                        var nodeInterface = interfaces.FirstOrDefault(x => x.IsGenericType);
                        if (nodeInterface == null) continue;

                        var genericTypes = nodeInterface.GetGenericArguments();

                        var genericType = genericTypes.FirstOrDefault();
                        if (genericType == null) continue;

                        var id = attribute.Id ?? Encoding.UTF8.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(type.FullName ?? type.Name)));

                        var controller = (INodeDescriptor) Activator.CreateInstance(genericType);
                        registrations.Add(new NodeRegistration(id, attribute.Title, type, controller.PortDefinitions));
                    }
                    catch (Exception)
                    {
                        //ignored
                    }
                }
            }

            return registrations;
        }
    }
}