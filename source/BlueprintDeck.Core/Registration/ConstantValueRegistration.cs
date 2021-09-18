using System;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal record ConstantValueRegistration(
        string Key,
        string Title,
        Type DataType,
        PortRegistration Port,
        Action<INodeContext, Func<object>> ActivationCall);
}