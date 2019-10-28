namespace BlueprintDeck.Design
{
    public class Connection
    {
        public string? Id { get; set; }
        public string? NodeFrom { get; set; }
        public string? NodePortFrom { get; set; }
        public bool IsConstantConnection { get; set; }
        public string? ConstantKey { get; set; }
        public string? NodeTo { get; set; }
        public string? NodePortTo { get; set; }

        public override string ToString()
        {
            if (IsConstantConnection)
            {
                return $"Constant {ConstantKey} => {NodeTo}.{NodePortTo}";
            }
            return $"{NodeFrom}.{NodePortFrom} => {NodeTo}.{NodePortTo}";
        }
    }
}