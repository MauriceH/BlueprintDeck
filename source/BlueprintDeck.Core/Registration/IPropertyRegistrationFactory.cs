using System;
using System.Collections.Generic;

namespace BlueprintDeck.Registration
{
    internal interface IPropertyRegistrationFactory
    {
        IList<PropertyRegistration> CreatePropertyRegistrations(Type nodeType);
    }
}