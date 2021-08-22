using System;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal class ConstantValueRegistration
    {
        public ConstantValueRegistration(string key, string title, Type dataType, NodePortDefinition portDefinition, Action<INodeContext,Func<object>> activationCall)
        {
            Key = key;
            Title = title;
            DataType = dataType;
            PortDefinition = portDefinition;
            ActivationCall = activationCall;
        }

        public string Key { get; }
        public string Title { get; }
        public Type DataType { get; }
        public NodePortDefinition PortDefinition { get; }
        public Action<INodeContext,Func<object>> ActivationCall { get; }

    }
}