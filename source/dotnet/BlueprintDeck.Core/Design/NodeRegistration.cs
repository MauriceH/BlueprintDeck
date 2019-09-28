using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;
using Newtonsoft.Json;

namespace BlueprintDeck.Design
{
    public class NodeRegistration
    {
        public NodeRegistration(string id, string title, Type nodeType, IList<NodePortDefinition> portDefinitions)
        {
            Id = id;
            Title = title;
            NodeType = nodeType;
            PortDefinitions = portDefinitions;
        }
        public string Id { get; }
        public string Title { get; }
        
        [JsonIgnore]
        public Type NodeType { get; }
        public IList<NodePortDefinition> PortDefinitions { get; }
    }
}