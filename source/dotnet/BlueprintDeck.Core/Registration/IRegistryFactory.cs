using BlueprintDeck.Design.Registry;

namespace BlueprintDeck.Registration
{
    public interface IRegistryFactory
    {
        NodeRegistry CreateNodeRegistry();
    }
}