using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.Instance.Factory
{
    internal interface IPortInstanceFactory
    {
        PortInstance Create(PortRegistration portRegistration);
        void InitializeAsOutput(NodeInstance nodeInstance, PortInstance portInstance);
        void InitializeAsInput(NodeInstance nodeInstance, PortInstance portInstance,IPort connectedOutput);
    }
}