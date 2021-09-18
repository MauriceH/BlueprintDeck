namespace BlueprintDeck.Registration
{
    public class PropertyDefinition
    {
        public string Name { get; }
        
        public string Title { get; set; }
        
        public PropertyDefinition(string name)
        {
            Name = name;
        }

        
    }
}