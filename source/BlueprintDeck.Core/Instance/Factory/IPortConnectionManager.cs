using BlueprintDeck.Node.Ports;

namespace BlueprintDeck.Instance.Factory
{
    internal interface IPortConnectionManager
    {
        void InitializePortAsOutput(NodeInstance nodeInstance, PortInstance portInstance);
        void InitializePortAsInput(NodeInstance nodeInstance, PortInstance portInstance, IPort connectedOutput);
    }
}