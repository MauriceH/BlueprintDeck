using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlueprintDeck.Node.Ports.Definitions
{
    public class NodePortDefinition 
    {
        
       public NodePortDefinition(string key, Direction direction, Type? portDataType = null, string? genericTypeParameterName = null)
        {
            Key = key;
            Direction = direction;
            PortDataType = portDataType;
            GenericTypeParameterName = genericTypeParameterName;
        }


        public string Key { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Direction Direction { get; }

        [JsonIgnore]
        public Type? PortDataType { get; }

        public string? GenericTypeParameterName { get; }
        
        public string? Title { get; set; }
        public bool Mandatory { get; set; } = true;

        public bool WithData => PortDataType != null || GenericTypeParameterName != null;

    }
}