using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    [NodeDescriptor("TestNode","Test node",typeof(Descriptor) )]
    public class TestNode : INode
    {
        
        
        public string ShortTitle { get; set; }
        
        public Task Activate(INodeContext nodeContext)
        {
            Console.WriteLine("Initializing test node ...");
            var inputPort = nodeContext.GetPort<IInput>(Descriptor.TriggerInput);
            inputPort.Register(() =>
            {
                Console.WriteLine("Trigger event received");
                return Task.CompletedTask;
            });
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            Console.WriteLine("OnDeactivate");
            return Task.CompletedTask;
        }


        public class Descriptor : INodeDescriptor
        {
            public static readonly NodePortDefinition TriggerInput  = NodePortDefinitionFactory.CreateInput("Trigger","Trigger",true);
            public IList<NodePortDefinition> PortDefinitions => new List<NodePortDefinition>() { TriggerInput };
        }
    }
}