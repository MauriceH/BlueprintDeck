using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;

namespace BlueprintDeck.Node.Default
{
    public abstract class ConstantValueNode<TDataType> : INode
    {
        public TDataType? Value { get; set; }
        
        public IOutput<TDataType>? Output { get; set; }

        public Task Activate()
        {
            if (Value == null) return Task.CompletedTask;
            Output?.Emit(Value);
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}