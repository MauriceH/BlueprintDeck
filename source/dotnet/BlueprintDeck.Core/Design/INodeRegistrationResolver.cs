using System.Collections.Generic;

namespace BlueprintDeck.Design
{
    public interface INodeRegistrationResolver
    {
        IList<NodeRegistration> ResolveNodeRegistrations();
    }
}