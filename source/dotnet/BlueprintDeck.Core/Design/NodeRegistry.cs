using System.Collections.Generic;

namespace BlueprintDeck.Design
{
    public class NodeRegistry
    {
        private INodeRegistrationResolver _nodeRegistrationResolver;
        
        private readonly Dictionary<string, NodeRegistration> _nodes = new Dictionary<string, NodeRegistration>();
        
        public NodeRegistry(INodeRegistrationResolver nodeRegistrationResolver)
        {
            _nodeRegistrationResolver = nodeRegistrationResolver;
        }

        public void Initialize()
        {
            foreach (var registration in _nodeRegistrationResolver.ResolveNodeRegistrations())
            {
                _nodes.Add(registration.Id, registration);
            }
        }
        
        
    }
}