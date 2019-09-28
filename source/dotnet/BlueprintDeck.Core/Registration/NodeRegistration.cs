using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;
using Newtonsoft.Json;

namespace BlueprintDeck.Design
{
    public class NodeRegistration
    {
        public NodeRegistration(string id, string title, Type nodeType, Type nodeDescriptorType, IList<NodePortDefinition> portDefinitions)
        {
            Id = id;
            Title = title;
            NodeType = nodeType;
            NodeDescriptorType = nodeDescriptorType;
            PortDefinitions = portDefinitions;
        }
        public string Id { get; }
        public string Title { get; }
        
        [JsonIgnore]
        public Type NodeType { get; }
        public Type NodeDescriptorType { get; }
        public IList<NodePortDefinition> PortDefinitions { get; }
    }
}