namespace BlueprintDeck.ConstantValue.Serializer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StringConstantValueSerializer : IConstantValueSerializer<string>
    {
        public string? Serialize(object? value)
        {
            return value == null ? null : $"{value}";
        }
        

        public object? Deserialize(string? serializedValue)
        {
            return serializedValue;
        }
    }
}