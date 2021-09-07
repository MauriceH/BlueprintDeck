using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlueprintDeck.Node.Ports.Definitions
{
    public class NodePortDefinition 
    {
        public NodePortDefinition(string key, string title, InputOutputType inputOutputType,  Type dataType,  bool isMandatory)
        {
            Key = key;
            Title = title;
            InputOutputType = inputOutputType;
            DataMode = DataMode.WithData;
            PortDataType = dataType;
            DefaultValue = null;
            Mandatory = isMandatory;
        }
        
        public NodePortDefinition(string key, string title, InputOutputType inputOutputType,  object defaultValue, bool isMandatory)
        {
            Key = key;
            Title = title;
            InputOutputType = inputOutputType;
            DataMode = DataMode.WithData;
            PortDataType = defaultValue.GetType();
            DefaultValue = defaultValue;
            Mandatory = isMandatory;
        }

        public NodePortDefinition(string key, string title,  InputOutputType inputOutputType, bool isMandatory)
        {
            Key = key;
            Title = title;
            InputOutputType = inputOutputType;
            DataMode = DataMode.Simple;
            PortDataType = null;
            DefaultValue = null;
            Mandatory = isMandatory;
        }
        
        public NodePortDefinition(string key, string title, string genericTypeParameterName, InputOutputType inputOutputType, bool isMandatory)
        {
            Key = key;
            Title = title;
            InputOutputType = inputOutputType;
            DataMode = DataMode.WithData;
            PortDataType = null;
            DefaultValue = null;
            GenericTypeParameterName = genericTypeParameterName;
            Mandatory = isMandatory;
        }
        

        public string Key { get; }
        public string Title { get;}
        public bool Mandatory { get;}
        public string? GenericTypeParameterName { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InputOutputType InputOutputType { get; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public DataMode DataMode { get;}
        
        [JsonIgnore]
        public Type? PortDataType { get; }
        
        public object? DefaultValue { get; }
    }
}