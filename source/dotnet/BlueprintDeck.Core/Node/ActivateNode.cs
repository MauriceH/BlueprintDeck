using System;
using System.Threading.Tasks;
using BlueprintDeck.Node.Ports;

namespace BlueprintDeck.Node
{
    [NodeDescriptor("Activate", "Activate Node", typeof(ActivateNodeDescriptor))]
    public class ActivateNode : INode
    {
        public string ShortTitle { get; set; }

        public Task Activate(INodeContext nodeContext)
        {
            Console.WriteLine("Initializing activate node ...");
            nodeContext.GetPort<IOutput>(ActivateNodeDescriptor.Definition).Emit();
            Console.WriteLine("activate node emitted and initialized");
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            return Task.CompletedTask;
        }
    }
}