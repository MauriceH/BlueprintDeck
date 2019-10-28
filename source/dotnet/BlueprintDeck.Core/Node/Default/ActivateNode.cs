using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.Node
{
    [NodeDescriptor("Activate", "Activate Node", typeof(ActivateNodeDescriptor))]
    public class ActivateNode : INode
    {
        private readonly ILogger<ActivateNode> _logger;

        public ActivateNode(ILogger<ActivateNode> logger)
        {
            _logger = logger;
        }

        public string? ShortTitle { get; set; }

        public Task Activate(INodeContext nodeContext)
        {
            _logger.LogDebug("Start initializing activate node");
            nodeContext.GetPort<IOutput>(ActivateNodeDescriptor.Definition).Emit();
            _logger.LogInformation("activate node emitted and initialized");
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}