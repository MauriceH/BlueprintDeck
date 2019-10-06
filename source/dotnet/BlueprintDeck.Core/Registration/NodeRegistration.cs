using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;
using Newtonsoft.Json;

namespace BlueprintDeck.Registration
{
    public class NodeRegistration
    {
        public NodeRegistration(string id, string title, Type nodeType, Type nodeDescriptorType, IList<NodePortDefinition> portDefinitions, bool isConstantNode)
        {
            Id = id;
            Title = title;
            NodeType = nodeType;
            NodeDescriptorType = nodeDescriptorType;
            PortDefinitions = portDefinitions;
            IsConstantNode = isConstantNode;
        }

        public NodeRegistration()
        {
        }

        public string Id { get; set; }
        public string Title { get; set;}
        
        [JsonIgnore]
        public Type NodeType { get; set;}
        
        [JsonIgnore]
        public Type NodeDescriptorType { get; set;}
        
        public bool IsConstantNode { get; set;}
        
        public IList<NodePortDefinition> PortDefinitions { get; set;}
    }
}