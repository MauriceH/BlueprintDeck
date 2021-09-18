using System;

namespace BlueprintDeck.Node.Ports.Registration
{
    public class PortRegistration 
    {
        
        internal PortRegistration(string key, Direction direction, Type? dataType = null, string? genericTypeParameterName = null)
        {
            Key = key;
            Direction = direction;
            DataType = dataType;
            GenericTypeParameterName = genericTypeParameterName;
        }


        public string Key { get; }

        public Direction Direction { get; }

        public Type? DataType { get; }

        public string? GenericTypeParameterName { get; }

        public bool WithData => DataType != null || GenericTypeParameterName != null;
        
        
        public string? Title { get; set; }

        public bool Mandatory { get; set; } = true;
    }
}