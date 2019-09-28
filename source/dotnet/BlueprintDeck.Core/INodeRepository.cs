using Autofac;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck
{
    public interface INodeRepository
    {
        CreateNodeResult CreateNode(ILifetimeScope scope, string nodeTypeKey);
        INodeDescriptor CreatePortDescriptor(string nodeTypeKey);
    }
}