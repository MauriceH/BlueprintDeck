using System;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;
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
        
        public NodePortDefinition(string key, string title, InputOutputType inputOutputType,  IPortData defaultValue, bool isMandatory)
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

        public NodePortDefinition()
        {
        }

        public string Key { get; set; }
        public string Title { get; set;}
        public bool Mandatory { get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public InputOutputType InputOutputType { get; set;}
        
        [JsonConverter(typeof(StringEnumConverter))]
        public DataMode DataMode { get; set;}
        
        [JsonIgnore]
        public Type? PortDataType { get; set;}
        
        
        public IPortData? DefaultValue { get; set;}
    }

    public static class NodePortDefinitionFactory
    {
        
        
        public static NodePortDefinition CreateOutput(string key, string title)
        {
            return new NodePortDefinition(key, title, InputOutputType.Output,false);
        }
        
        public static NodePortDefinition CreateInput(string key, string title, bool isMandatory = true)
        {
            return new NodePortDefinition(key, title, InputOutputType.Input, isMandatory);
        }

        public static NodePortDefinition CreateDataOutput<TDataType>(string key, string title) where TDataType : IPortData
        {
            return new NodePortDefinition(key, title, InputOutputType.Output,  typeof(TDataType), false);
        }
        
        public static NodePortDefinition CreateDataInput<TDataType>(string key, string title, TDataType defaultValue, bool isMandatory = true)  where TDataType : IPortData
        {
            return new NodePortDefinition(key, title, InputOutputType.Input,  defaultValue, isMandatory);
        }
        
    }
}