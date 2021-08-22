using BlueprintDeck.Node.Ports.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BlueprintDeck.Design.Registry
{
    public class NodePortTypeBase
    {
        public string? Key { get; set; }
        public string? Title { get; set;}
        public string? TypeId  { get;  set;}
        public string? DefaultValue { get;  set;}
    }
    
    public class NodePort : NodePortTypeBase
    {
        public bool Mandatory { get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public InputOutputType? InputOutputType { get;  set;}
        
        [JsonConverter(typeof(StringEnumConverter))]
        public DataMode? DataMode { get; set;}
    }

    public class ConstantValueNodePortType : NodePortTypeBase
    {
        
    }
}