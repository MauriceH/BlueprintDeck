using BlueprintDeck.Node.Properties.Registration;

namespace BlueprintDeck.Node.Properties
{
    public class PropertyTitleAttribute : PropertyAttribute
    {
        public PropertyTitleAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; }
        
        public override void Setup(PropertyRegistration definition)
        {
            definition.Title = Title;
        }
    }
}