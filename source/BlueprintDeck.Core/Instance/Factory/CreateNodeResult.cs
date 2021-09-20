using System.Collections.Generic;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Registration;

namespace BlueprintDeck.Instance.Factory
{
    internal class CreateNodeResult
    {
        public CreateNodeResult(NodeRegistration registration, INode node, List<GenericTypeParameterInstance> genericTypes)
        {
            Registration = registration;
            Node = node;
            GenericTypes = genericTypes;
        }
        public NodeRegistration Registration { get;  }
        public INode Node { get; }
        public List<GenericTypeParameterInstance> GenericTypes { get; }
    }
}