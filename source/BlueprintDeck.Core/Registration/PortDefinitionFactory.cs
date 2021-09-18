using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal class PortDefinitionFactory
    {
        internal static List<NodePortDefinition> CreatePortDefinitions(Type nodeType)
        {
            var portProperties = nodeType.GetProperties();
            var result = new List<NodePortDefinition>();
            foreach (var property in portProperties)
            {
                var propertyType = property.PropertyType;
                var isInput = propertyType.IsAssignableFrom(typeof(IInput))
                              || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IInput<>));
                var isOutput = propertyType.IsAssignableFrom(typeof(IOutput))
                               || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IOutput<>));

                if (!isInput && !isOutput) continue;

                var inputOutputType = isInput ? Direction.Input : Direction.Output;


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
    }
}