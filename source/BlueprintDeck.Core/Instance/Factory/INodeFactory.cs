using BlueprintDeck.ConstantValue.Registration;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.Instance.Factory
{
    internal interface INodeFactory
    {
        CreateNodeResult<NodeRegistration> CreateNode(IServiceScope scope, string nodeKey, Design.Node designNode);
    }
}