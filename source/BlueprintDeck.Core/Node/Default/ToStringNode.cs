using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;

namespace BlueprintDeck.Node.Default
{
    [Node("ToString", "ToString")]
    public class ToStringNode<TInput> : INode
    {
        public IInput<TInput>? Input { get; set; }
        
        public IOutput<string>? Output { get; set; }

        public Task Activate(INodeContext nodeContext)
        {
            Input?.OnData(value =>
            {
                if (value == null) throw new Exception("value cannot be null");
                Output?.Emit(value.ToString()!);
            });
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}