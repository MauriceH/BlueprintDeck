namespace BlueprintDeck.ConstantValue.Serializer
{
    // ReSharper disable once UnusedTypeParameter
    public interface IConstantValueSerializer<TDataType> : IRawConstantValueSerializer
    {
    }

    public interface IRawConstantValueSerializer
    {
        string? Serialize(object? value);
        object? Deserialize(string? serializedValue);
    }
    
}