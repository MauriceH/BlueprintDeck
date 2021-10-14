using System.Collections.Generic;

namespace BlueprintDeck.Design.Registry
{
    public class BlueprintRegistry
    {
        public BlueprintRegistry(IEnumerable<NodeType> nodeTypes, IEnumerable<DataType> dataTypes)
        {
            NodeTypes = nodeTypes;
            DataTypes = dataTypes;
        }

        public IEnumerable<NodeType> NodeTypes { get; }
        
        public IEnumerable<DataType> DataTypes { get; }
        
    }
}