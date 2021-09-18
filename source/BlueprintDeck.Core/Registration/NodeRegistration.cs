using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal record NodeRegistration(
        string Id, 
        string Title,
        Type NodeType,
        IList<PortRegistration> Ports,
        IList<string> GenericTypes,
        IList<PropertyRegistration> Properties);
}