namespace BlueprintDeck.Instance.Factory
{
    public interface IBlueprintFactory
    {
        IBlueprintInstance CreateBlueprint(Design.Blueprint design);
    }
}