using System;
using System.Threading.Tasks;

namespace BlueprintDeck.Node.Default
{
    public class ConstantValueNode : INode
    {
        private Action<Func<object>> ActivationCall { get; }
        
        public object? Value { get; set; }
        
        public ConstantValueNode(Action<Func<object>> activationCall)
        {
            ActivationCall = activationCall;
        }

        public Task Activate()
        {
            if (Value == null) return Task.CompletedTask;
            var val = Value!;
            ActivationCall(() => val);
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}