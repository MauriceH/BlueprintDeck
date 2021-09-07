using System.Collections.Generic;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BlueprintDeck.Design.Registry
{
    public class NodeType
    {
        public string? Key { get; set; }
        public string? Title { get; set; }
        public IList<NodePort>? Ports { get; set; }
        public IList<string>? GenericTypes { get; set; }
    }
}