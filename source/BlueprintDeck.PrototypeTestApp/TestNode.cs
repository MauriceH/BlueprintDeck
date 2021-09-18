using System.Threading.Tasks;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.PrototypeTestApp
{
    [Node("TestNode","Test node")]
    public class TestNode : INode
    {
        private readonly ILogger<TestNode> _logger;
        
        public IInput? Trigger { get; set; }

        public TestNode(ILogger<TestNode> logger, Design.Node designValues)
        {
            _logger = logger;
            DesignValues = designValues;
        }

        public Design.Node DesignValues { get; }

        public Task Activate(INodeContext nodeContext)
        {
            _logger.LogDebug("Start initializing test node");
            Trigger?.Register(() =>
            {
                _logger.LogInformation("TestNode {ShortTitle} trigger event received", DesignValues.Title);
                return Task.CompletedTask;
            });

            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            _logger.LogInformation("TestNode deactivated");
            return Task.CompletedTask;
        }
    }
}