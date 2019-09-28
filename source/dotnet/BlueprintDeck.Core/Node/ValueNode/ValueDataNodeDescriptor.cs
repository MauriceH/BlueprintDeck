using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node.ValueNode
{
    public class ValueDataNodeDescriptor<T> : INodeDescriptor where T : IPortData
    {
        public static NodePortDefinition Definition { get; } =  NodePortDefinitionFactory.CreateDataOutput<T>("Value", "Value");
        
        
        public ValueDataNodeDescriptor()
        {
            PortDefinitions = new List<NodePortDefinition>()
            {
                Definition
            };
        }
        
        public IList<NodePortDefinition> PortDefinitions { get; }
    }
}