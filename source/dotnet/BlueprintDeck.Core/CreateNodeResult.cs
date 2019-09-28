using BlueprintDeck.Node;

namespace BlueprintDeck
{
    public class CreateNodeResult
    {
        public CreateNodeResult(NodeDescriptorAttribute descriptor, INode node)
        {
            Descriptor = descriptor;
            Node = node;
        }

        public NodeDescriptorAttribute Descriptor { get;  }
        public INode Node { get; }
    }
}