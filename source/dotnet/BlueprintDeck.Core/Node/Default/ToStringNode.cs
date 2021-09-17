using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;

namespace BlueprintDeck.Node.Default
{
    [NodeDescriptor("ToString", "ToString")]
    public class ToStringNode<TInput> : INode<ToStringNodePorts<TInput>>
    {

        
        public Task Activate(INodeContext nodeContext, ToStringNodePorts<TInput> ports)
        {
            ports.Input.OnData((value) =>
            {
                if (value == null) throw new Exception("value cannot be null");
                ports.Output.Emit(value.ToString()!);
            });
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }

    public class ToStringNodePorts<TInput>
    {
        [PortTitle("Value")]
        [PortOptional]
        public IInput<TInput> Input { get; set; }
        
        [PortTitle("Value")]
        [PortOptional]
        public IInput<double> InputDouble { get; set; }

        [PortTitle("Value")]
        [PortOptional]
        public IInput InputSimple { get; set; }
        
        [PortTitle("Raus damit")]
        public IOutput<string> Output { get; set; }
    }
}