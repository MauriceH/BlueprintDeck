using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.Instance.Factory
{
    internal interface IPortInstanceFactory
    {
        PortInstance Create(PortRegistration portRegistration);
        void InitializeAsOutput(PortInstance portInstance);
        void InitializeAsInput(PortInstance portInstance,object connectedOutput);
    }
}