using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Node
{
    
    [NodeDescriptor(DelayNodeDescriptor.NodeKey,"Delay", typeof(DelayNodeDescriptor) )]
    public class DelayNode : INode
    {
        private readonly ILogger<DelayNode> _logger;
        private IInput? _triggerInput;
        private IInput<TimeSpan>? _durationInput;
        private IOutput? _output;

        public DelayNode(ILogger<DelayNode> logger)
        {
            _logger = logger;
        }

        public string? ShortTitle { get; set; }
        
        
        
        public Task Activate(INodeContext nodeContext)
        {
            _logger.LogDebug("Start initializing delay node");
            _triggerInput = nodeContext.GetPort<IInput>(DelayNodeDescriptor.Input);
            _durationInput = nodeContext.GetPort<IInput<TimeSpan>>(DelayNodeDescriptor.DelayDuration);
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
            var valueTimeSpan = _durationInput?.Value;
            if (valueTimeSpan == null)
            {
                throw new PortNotInitializedException(DelayNodeDescriptor.NodeKey,DelayNodeDescriptor.DelayDuration.Key);
            }
            await Task.Delay(valueTimeSpan.Value);
            _output?.Emit();
        }
    }
}