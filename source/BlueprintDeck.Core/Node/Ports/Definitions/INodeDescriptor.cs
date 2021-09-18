using System.Collections.Generic;

namespace BlueprintDeck.Node.Ports.Definitions
{
    public interface INodeDescriptor
    {
        IList<PortRegistration> PortDefinitions { get; }
    }
}