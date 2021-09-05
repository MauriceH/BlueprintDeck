using System;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeDescriptorAttribute : Attribute
    {
        public string Title { get; }
        public string Id { get; }
        public Type NodeDescriptor { get; }

        public NodeDescriptorAttribute(string id, string title, Type nodeDescriptor)
        {
            Id = id;
            Title = title;
            NodeDescriptor = nodeDescriptor;
            if (!typeof(INodeDescriptor).IsAssignableFrom(nodeDescriptor))
            {
                throw new ArgumentException("portDescriptor does not implement INodeDescriptor", nameof(nodeDescriptor));
            }
        }
    }
}