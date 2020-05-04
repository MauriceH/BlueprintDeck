using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    public class NodeRegistration
    {
        
        public NodeRegistration(string key, string title, Type nodeType, Type nodeDescriptorType, IList<NodePortDefinition> portDefinitions)
        {
            Key = key;
            Title = title;
            NodeType = nodeType;
            NodeDescriptorType = nodeDescriptorType;
            PortDefinitions = portDefinitions;
        }

        public string Key { get; }
        public string Title { get;}
        
        public Type NodeType { get;}

        public Type NodeDescriptorType { get;}
        
        public IList<NodePortDefinition> PortDefinitions { get; }
    }
}