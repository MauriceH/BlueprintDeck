using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    public class PortOptionalAttribute : PortAttribute
    {
        public override void Setup(NodePortDefinition definition)
        {
            definition.Mandatory = false;
        }
    }
}