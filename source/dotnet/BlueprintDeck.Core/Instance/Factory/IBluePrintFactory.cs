namespace BlueprintDeck.Instance.Factory
{
    public interface IBluePrintFactory
    {
        BluePrint CreateBluePrint(Design.BluePrint design);
    }
}