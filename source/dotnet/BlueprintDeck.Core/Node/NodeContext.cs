using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    public interface INodeContext
    {

        T? GetPort<T>(NodePortDefinition definition) where T : class, IPortInputOutput;

    }
}