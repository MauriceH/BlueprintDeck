using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    public class PortTitleAttribute : PortAttribute
    {
        public string Title { get; }

        public PortTitleAttribute(string title)
        {
            Title = title;
        }

        public override void Setup(NodePortDefinition definition)
        {
            definition.Title = Title;
        }
    }
}