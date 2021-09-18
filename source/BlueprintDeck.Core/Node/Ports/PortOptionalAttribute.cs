using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.Node.Ports
{
    public class PortOptionalAttribute : PortAttribute
    {
        public override void Setup(PortRegistration definition)
        {
            definition.Mandatory = false;
        }
    }
}