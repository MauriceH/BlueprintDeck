using System;
using System.Threading.Tasks;

namespace BlueprintDeck.Node.Default
{
    public class ConstantValueNode : INode
    {
        private Action<INodeContext, Func<object?>> ActivationCall { get; }
        
        public object? Value { get; set; }
        
        public ConstantValueNode(Action<INodeContext, Func<object?>> activationCall)
        {
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