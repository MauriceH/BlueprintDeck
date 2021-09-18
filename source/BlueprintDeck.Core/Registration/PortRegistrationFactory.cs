using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueprintDeck.Misc;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal class PortRegistrationFactory : IPortRegistrationFactory
    {
        public List<PortRegistration> CreatePortRegistrations(Type nodeType)
        {
            var portProperties = nodeType.GetProperties();
            var result = new List<PortRegistration>();
            foreach (var property in portProperties)
            {
                var propertyType = property.PropertyType;
               
                if (!propertyType.IsInput() && !propertyType.IsOutput()) continue;

                var inputOutputType = propertyType.IsInput() ? Direction.Input : Direction.Output;


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

                var definition = new PortRegistration(property.Name, inputOutputType, portDataType, portGenericType);

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
    }
}