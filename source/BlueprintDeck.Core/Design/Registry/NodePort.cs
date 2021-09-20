using BlueprintDeck.Node.Ports;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BlueprintDeck.Design.Registry
{
    public class NodePort
    {
        public string? Key { get; set; }
        public string? Title { get; set;}
        public string? TypeId  { get;  set;}
        
        public bool Mandatory { get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public Direction? Direction { get;  set;}
        
        public string? GenericTypeParameter { get; set; }
    }

}