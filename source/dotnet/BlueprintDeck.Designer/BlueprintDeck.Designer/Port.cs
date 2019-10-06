namespace BlueprintDeck.Designer
{
    public class Port
    {
        public bool IsConnected { get; set; }
        public string Title { get; set; }
        public int PositionY { get; set; }
        public string Key { get; set; }
        public string ConnectionNodeId { get; set; }
        public string ConnectionNodePortKey { get; set; }
        public bool IsData { get; set; }
        public bool IsMandatory { get; set; }
    }
}