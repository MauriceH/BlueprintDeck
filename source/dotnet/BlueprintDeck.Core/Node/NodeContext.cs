using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    public interface INodeContext
    {

         T GetPort<T>(NodePortDefinition definition);

    }
}