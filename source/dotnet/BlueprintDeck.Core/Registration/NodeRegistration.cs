using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    public class NodeRegistration
    {
        
        public NodeRegistration(string key, string title, Type nodeType, IList<NodePortDefinition> portDefinitions)
        {
            Key = key;
            Title = title;
            NodeType = nodeType;
            PortDefinitions = portDefinitions;
        }

        public string Key { get; }
        public string Title { get;}
        
        public Type NodeType { get;}
        
        public IList<NodePortDefinition> PortDefinitions { get; }
    }
}