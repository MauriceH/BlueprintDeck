using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    public interface INodeRegistration
    {
        string Key { get; }
        string Title { get; }
        Type NodeType { get; }
        Type NodeDescriptorType { get; }
        IList<NodePortDefinition> PortDefinitions { get; }
    }
}