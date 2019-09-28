using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node
{
    
    [NodeDescriptor("Delay","Delay", typeof(DelayNodeDescriptor) )]
    public class DelayNode : INode
    {
        private IInput _triggerInput;
        private IInput<PdtDuration> _durationInput;
        private IOutput _output;
        public string ShortTitle { get; set; }
        
        public Task Activate(INodeContext nodeContext)
        {
            Console.WriteLine("Initializing delay node ...");
            _triggerInput = nodeContext.GetPort<IInput>(DelayNodeDescriptor.Input);
            _durationInput = nodeContext.GetPort<IInput<PdtDuration>>(DelayNodeDescriptor.DelayDuration);
            _output = nodeContext.GetPort<IOutput>(DelayNodeDescriptor.Output);
            _triggerInput.Register(OnInput);
            return Task.CompletedTask;
        }
       

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
        
        private async Task OnInput()
        {
            Console.WriteLine("OnDelayTriggert");
            await Task.Delay(_durationInput.Value.TimeSpan);
            _output.Emit();
        }
    }
}