using System;
using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.ConstantValue.Registration
{
    internal record ConstantValueRegistration(
        string Key,
        string Title,
        Type DataType,
        PortRegistration Port,
        Action<Func<object>> ActivationCall);
}