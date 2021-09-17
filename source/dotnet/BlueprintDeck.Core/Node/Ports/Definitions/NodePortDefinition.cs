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

        public NodePortDefinition(string key, InputOutputType inputOutputType, Type? portDataType, string? genericTypeParameterName)
        {
            Key = key;
            InputOutputType = inputOutputType;
            PortDataType = portDataType;
            GenericTypeParameterName = genericTypeParameterName;
        }


        public string Key { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InputOutputType InputOutputType { get; }

        [JsonIgnore]
        public Type? PortDataType { get; }

        public string? GenericTypeParameterName { get; }
        
        public string Title { get; set; }
        public bool Mandatory { get; set; } = true;


        // Kann weg

        [JsonConverter(typeof(StringEnumConverter))]
        public DataMode DataMode { get;}

        public object? DefaultValue { get; }
    }
}