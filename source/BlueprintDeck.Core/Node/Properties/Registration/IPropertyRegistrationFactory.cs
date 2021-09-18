using System;
using System.Collections.Generic;

namespace BlueprintDeck.Node.Properties.Registration
{
    internal interface IPropertyRegistrationFactory
    {
        IList<PropertyRegistration> CreatePropertyRegistrations(Type nodeType);
    }
}