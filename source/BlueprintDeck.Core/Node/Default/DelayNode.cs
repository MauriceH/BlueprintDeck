using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Properties;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Node.Default
{
    [Node("Delay", "Delay")]
    public class DelayNode : INode
    {
        private readonly ILogger<DelayNode> _logger;

        public IInput? Input { get; set; }
        
        [PortOptional]
        public IInput<TimeSpan>? DelayDuration { get; set; }
        
        public IOutput? Output { get; set; }

        [PropertyTitle("Default Delay")]
        public TimeSpan? DefaultDelay { get; set; }

        public DelayNode(ILogger<DelayNode> logger)
        {
            _logger = logger;
        }

        public Task Activate()
        {
            _logger.LogDebug("Start initializing delay node with default delay {defaultDelay}", DefaultDelay);
            Input?.Subscribe(async () =>
            {
                var valueTimeSpan = DelayDuration?.LastValue;
                valueTimeSpan ??= DefaultDelay;
                if (valueTimeSpan == null)
                {
                    _logger.LogWarning("Delay node has no delay configured");
                    return;
                }
                var delay = valueTimeSpan.Value;
                _logger.LogTrace("Delay node triggered. Waiting {delay} to emit output",delay);
                await Task.Delay(delay);
                Output?.Emit();
            });
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}