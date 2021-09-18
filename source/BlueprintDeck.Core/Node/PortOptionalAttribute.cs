using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    public class PortOptionalAttribute : PortAttribute
    {
        public override void Setup(PortRegistration definition)
        {
            definition.Mandatory = false;
        }
    }
}