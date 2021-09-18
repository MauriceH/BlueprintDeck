using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;

namespace BlueprintDeck.Node.Registration
{
    internal record NodeRegistration(
        string Id, 
        string Title,
        Type NodeType,
        IList<PortRegistration> Ports,
        IList<string> GenericTypes,
        IList<PropertyRegistration> Properties);
}