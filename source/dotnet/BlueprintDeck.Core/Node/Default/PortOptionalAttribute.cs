using System;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node.Default
{
    public class PortOptionalAttribute : PortAttribute
    {
        public override void Setup(NodePortDefinition definition)
        {
            definition.Mandatory = false;
        }
    }

    public abstract class PortAttribute : Attribute
    {
        public abstract void Setup(NodePortDefinition definition);
    }
}