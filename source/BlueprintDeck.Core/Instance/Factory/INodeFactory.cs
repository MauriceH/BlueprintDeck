using BlueprintDeck.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.Instance.Factory
{
    internal interface INodeFactory
    {
        CreateNodeResult<NodeRegistration> CreateNode(IServiceScope scope, string nodeKey, Design.Node designNode);
        CreateNodeResult<ConstantValueRegistration> CreateConstantValueNode(IServiceScope scope, string constantValueNodeTypeKey);
    }
}