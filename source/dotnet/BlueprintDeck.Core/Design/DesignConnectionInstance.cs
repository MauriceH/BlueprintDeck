namespace BlueprintDeck.Design
{
    public class DesignConnectionInstance
    {
        public string? Id { get; set; }
        public string? NodeFrom { get; set; }
        public string? NodePortFrom { get; set; }
        public string? NodeTo { get; set; }
        public string? NodePortTo { get; set; }

        public override string ToString()
        {
            return $"{NodeFrom}.{NodePortFrom} => {NodeTo}.{NodePortTo}";
        }
    }
}