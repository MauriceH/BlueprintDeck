using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal interface IPortRegistrationFactory
    {
        List<PortRegistration> CreatePortRegistrations(Type nodeType);
    }
}