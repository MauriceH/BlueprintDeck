using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Node.Default
{
    [Node("Activate", "Activate Node")]
    public class ActivateNode : INode
    {
        private readonly ILogger<ActivateNode> _logger;

        [PortTitle("OnActivate")]
        public IOutput? Event { get; set; }
        
        public ActivateNode(ILogger<ActivateNode> logger)
        {
            _logger = logger;
        }

        public Task Activate()
        {
            _logger.LogDebug("Start initializing activate node");
            if (Event == null)
            {
                _logger.LogInformation("activate node initialized without connection");
                return Task.CompletedTask;
            }

            Event.Emit();
            _logger.LogInformation("activate node emitted and initialized");
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}