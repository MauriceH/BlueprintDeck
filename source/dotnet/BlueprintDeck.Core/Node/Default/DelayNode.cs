using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Node.Default
{
    
    [NodeDescriptor(DelayNodeDescriptor.NodeKey,"Delay", typeof(DelayNodeDescriptor) )]
    public class DelayNode : INode
    {
        private readonly ILogger<DelayNode> _logger;
        private IInput? _triggerInput;
        private IInput<TimeSpan>? _durationInput;
        private IOutput? _output;
        private DelayNodeData? _data;

        public Design.Node DesignValues { get;}
        
        public DelayNode(ILogger<DelayNode> logger, Design.Node designValues)
        {
            _logger = logger;
            DesignValues = designValues;
        }
       

        public Task Activate(INodeContext nodeContext)
        {
            _logger.LogDebug("Start initializing delay node");
            _triggerInput = nodeContext.GetPort<IInput>(DelayNodeDescriptor.Input);
            _durationInput = nodeContext.GetPort<IInput<TimeSpan>>(DelayNodeDescriptor.DelayDuration);
            _output = nodeContext.GetPort<IOutput>(DelayNodeDescriptor.Output);
            _triggerInput?.Register(OnInput);

            _data = DesignValues.Data?.ToObject<DelayNodeData>();
            if (_data?.DefaultMilliseconds == null)
            {
                throw new Exception("Invalid Configuration");
            }
            
            return Task.CompletedTask;
        }
       

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
        
        private async Task OnInput()
        {
            var valueTimeSpan = _durationInput?.Value;
            valueTimeSpan ??= TimeSpan.FromMilliseconds(_data!.DefaultMilliseconds.GetValueOrDefault());
            await Task.Delay(valueTimeSpan.Value);          
            _output?.Emit();
        }

        public class DelayNodeData
        {
            public long? DefaultMilliseconds { get; set; }
        }
        
    }
}