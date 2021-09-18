using System;
using System.Reflection;

namespace BlueprintDeck.Node.Ports.Registration
{
    public class PortRegistration 
    {
        
        internal PropertyInfo Property { get; }

        public string Key => Property.Name;
        
        internal PortRegistration(PropertyInfo property, Direction direction, Type? dataType = null, string? genericTypeParameterName = null)
        {
            Property = property;
            Direction = direction;
            DataType = dataType;
            GenericTypeParameterName = genericTypeParameterName;
        }


        public Direction Direction { get; }

        public Type? DataType { get; }

        public string? GenericTypeParameterName { get; }

        public bool WithData => DataType != null || GenericTypeParameterName != null;
        
        
        public string? Title { get; set; }

        public bool Mandatory { get; set; } = true;
    }
}