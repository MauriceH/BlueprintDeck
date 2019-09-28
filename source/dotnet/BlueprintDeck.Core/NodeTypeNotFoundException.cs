using System;

namespace BlueprintDeck
{
    public class NodeTypeNotFoundException : Exception
    {
        public NodeTypeNotFoundException(string nodeTypeKey) : base($"Node type {nodeTypeKey} not found")
        {
            
        }
    }
}