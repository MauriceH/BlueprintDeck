using System;

namespace BlueprintDeck.Node
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeAttribute : Attribute
    {
        public string? Title { get; }
        public string? Id { get; }

        public NodeAttribute(string? id = null, string? title = null)
        {
            Id = id;
            Title = title ?? id;
        }
    }
}