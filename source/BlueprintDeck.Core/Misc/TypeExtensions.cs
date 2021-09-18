using System;
using BlueprintDeck.Node.Ports;

namespace BlueprintDeck.Misc
{
    internal static class TypeExtensions
    {

        public static bool IsInput(this Type type)
        {
            return  type.IsAssignableFrom(typeof(IInput))
                    || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IInput<>));
        }
        
        public static bool IsOutput(this Type type)
        {
            return  type.IsAssignableFrom(typeof(IOutput))
                    || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IOutput<>));
        }
        
    }
}