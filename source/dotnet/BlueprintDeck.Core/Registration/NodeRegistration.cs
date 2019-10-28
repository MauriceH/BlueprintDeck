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

        public string Id { get; }
        public string Title { get;}
        
        [JsonIgnore]
        public Type NodeType { get;}
        
        [JsonIgnore]
        public Type NodeDescriptorType { get;}
        
        public bool IsConstantNode { get; }
        
        public IList<NodePortDefinition> PortDefinitions { get; }
    }
}