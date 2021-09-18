using System;
using System.Collections.Generic;

namespace BlueprintDeck.Node.Ports.Registration
{
    internal interface IPortRegistrationFactory
    {
        List<PortRegistration> CreatePortRegistrations(Type nodeType);
    }
}