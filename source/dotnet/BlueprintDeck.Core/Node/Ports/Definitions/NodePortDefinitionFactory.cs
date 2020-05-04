namespace BlueprintDeck.Node.Ports.Definitions
{
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

        public static NodePortDefinition CreateDataOutput<TDataType>(string key, string title)
        {
            return new NodePortDefinition(key, title, InputOutputType.Output,  typeof(TDataType), false);
        }
        
        public static NodePortDefinition CreateDataInput<TDataType>(string key, string title, TDataType defaultValue, bool isMandatory = true) where TDataType : notnull
        {
            return new NodePortDefinition(key, title, InputOutputType.Input,  defaultValue, isMandatory);
        }
        
    }
}