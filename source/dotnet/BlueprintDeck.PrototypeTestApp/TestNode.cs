using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;
using Microsoft.Extensions.Logging;

namespace BlueprintDeck.PrototypeTestApp
{
    [NodeDescriptor("TestNode","Test node",typeof(Descriptor) )]
    public class TestNode : INode
    {
        private readonly ILogger<TestNode> _logger;

        public TestNode(ILogger<TestNode> logger, Design.Node designValues)
        {
            _logger = logger;
            DesignValues = designValues;
        }

        public Design.Node DesignValues { get; }

        public Task Activate(INodeContext nodeContext)
        {
            _logger.LogDebug("Start initializing test node");
            var inputPort = nodeContext.GetPort<IInput>(Descriptor.TriggerInput);
            inputPort?.Register(() =>
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

        public class Descriptor : INodeDescriptor
        {
            public static readonly NodePortDefinition TriggerInput  = NodePortDefinitionFactory.CreateInput("Trigger","Trigger");
            public IList<NodePortDefinition> PortDefinitions => new List<NodePortDefinition>() { TriggerInput };
        }
    }
}