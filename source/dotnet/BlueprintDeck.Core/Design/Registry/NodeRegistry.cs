using System.Collections.Generic;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BlueprintDeck.Design.Registry
{
    public class NodeRegistry
    {
        
        public IEnumerable<NodeType>? NodeTypes { get; set; }
        
        public IEnumerable<DataType>? DataTypes { get; set; }
        
        public IEnumerable<ConstantValueNodeType>? ConstantValueNodeTypes { get; set; }
        
    }
}