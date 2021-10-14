using System;
using System.Reflection;

namespace BlueprintDeck.Node.Ports.Registration
{
    public class PortRegistration 
    {
        
        internal PropertyInfo Property { get; }

        public string Key => Property.Name;
        
        internal PortRegistration(PropertyInfo property, Direction direction, Type? dataType = null, string? genericTypeParameter = null)
        {
            Property = property;
            Direction = direction;
            DataType = dataType;
            GenericTypeParameter = genericTypeParameter;
        }


        public Direction Direction { get; }

        public Type? DataType { get; }

        public string? GenericTypeParameter { get; }

        public bool WithData => DataType != null || GenericTypeParameter != null;
        
        
        public string? Title { get; set; }

        public bool Mandatory { get; set; } = true;
    }
}