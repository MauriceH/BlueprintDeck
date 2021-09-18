using BlueprintDeck.Node.Ports.Registration;

namespace BlueprintDeck.Node.Ports
{
    public class PortTitleAttribute : PortAttribute
    {
        public string Title { get; }

        public PortTitleAttribute(string title)
        {
            Title = title;
        }

        public override void Setup(PortRegistration definition)
        {
            definition.Title = Title;
        }
    }
}