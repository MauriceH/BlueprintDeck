namespace BlueprintDeck.Node
{
    public class PortNotInitializedException : InvalidNodeStateException
    {
        public PortNotInitializedException(string nodeId, string portKey) : base($"Port {portKey} of node {nodeId} not initialized")
        {
            
        }
    }
}