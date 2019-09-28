using BlueprintDeck.Node.ValueNode;

namespace BlueprintDeck.Node.Ports.Definitions.DataTypes
{
    [NodeDescriptor("PdtDuration","Duration", typeof(Descriptor))]
    public class PdtDurationDataNode : ValueDataNode<PdtDuration>
    {
        public class Descriptor : ValueDataNodeDescriptor<PdtDuration>
        {
           
        }
        
    }
}