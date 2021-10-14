using System;

namespace BlueprintDeck.Instance.Factory
{
    public class PortInitializationException : Exception
    {
        public PortInitializationException(string nodeId, string portId, string message) : base(message)
        {
            NodeId = nodeId;
            PortId = portId;
        }

        public string NodeId { get; }

        public string PortId { get; }
    }
}