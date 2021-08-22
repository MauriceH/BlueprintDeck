using BlueprintDeck.Node;

namespace BlueprintDeck.Instance.Factory
{
    internal class CreateNodeResult<TRegistrationType>
    {
        public CreateNodeResult(TRegistrationType registration, INode node)
        {
            Registration = registration;
            Node = node;
        }
        public TRegistrationType Registration { get;  }
        public INode Node { get; }
    }
}