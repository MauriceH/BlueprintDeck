using BlueprintDeck.Node.Ports.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BlueprintDeck.Design.Registry
{
    public class NodePort
    {
        public string? Key { get; set; }
        public string? Title { get; set;}
        public bool Mandatory { get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public InputOutputType? InputOutputType { get;  set;}
        
        [JsonConverter(typeof(StringEnumConverter))]
        public DataMode? DataMode { get; set;}
        
        public string? TypeId  { get;  set;}
        
        public string? DefaultValue { get;  set;}
    }
}