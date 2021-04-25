using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node.Default
{
    public class ConstantValueNode : INode
    {
        public Design.Node DesignValues { get; set; }
        
        public NodePortDefinition PortDefinition { get; }

        public Action<INodeContext, Func<object?>> ActivationCall { get; }
        
        public object? Value { get; set; }
        
        public ConstantValueNode(NodePortDefinition portDefinition, Action<INodeContext, Func<object?>> activationCall)
        {
            PortDefinition = portDefinition;
            ActivationCall = activationCall;
        }

        

        public Task Activate(INodeContext nodeContext)
        {
            ActivationCall(nodeContext, () => Value);
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}