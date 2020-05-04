using BlueprintDeck.Design.Registry;

namespace BlueprintDeck.Registration
{
    public interface INodeRegistryFactory
    {
        NodeRegistry LoadNodeRegistry();
    }
}