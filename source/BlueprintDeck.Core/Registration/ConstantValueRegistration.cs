using System;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal record ConstantValueRegistration(
        string Key,
        string Title,
        Type DataType,
        NodePortDefinition PortDefinition,
        Action<INodeContext, Func<object>> ActivationCall);
}