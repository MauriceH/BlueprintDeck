namespace BlueprintDeck.Instance.Factory
{
    public interface IBluePrintFactory
    {
        IBluePrintInstance CreateBluePrint(Design.BluePrint design);
    }
}