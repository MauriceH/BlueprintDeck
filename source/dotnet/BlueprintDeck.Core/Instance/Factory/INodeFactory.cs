using BlueprintDeck.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.Instance.Factory
{
    internal interface INodeFactory
    {
        CreateNodeResult<NodeRegistration> CreateNode(IServiceScope scope, string nodeKey);
        CreateNodeResult<ConstantValueRegistration> CreateConstantValueNode(IServiceScope scope, string constantValueNodeTypeKey);
    }
}