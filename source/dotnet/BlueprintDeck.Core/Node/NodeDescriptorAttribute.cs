using System;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeDescriptorAttribute : Attribute
    {
        public string Title { get; }
        public string Id { get; }
        public Type PortDescriptor { get; }

        public NodeDescriptorAttribute(string id, string title, Type portDescriptor)
        {
            Id = id;
            Title = title;
            PortDescriptor = portDescriptor;
            if (!typeof(INodeDescriptor).IsAssignableFrom(portDescriptor))
            {
                throw new ArgumentException("portDescriptor does not implement INodeDescriptor", nameof(portDescriptor));
            }
        }
    }
}