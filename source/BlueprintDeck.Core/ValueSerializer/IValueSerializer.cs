namespace BlueprintDeck.ConstantValue.Serializer
{
    // ReSharper disable once UnusedTypeParameter
    public interface IValueSerializer<TDataType> : IRawValueSerializer
    {
        
    }

    public interface IRawValueSerializer
    {
        string? Serialize(object? value);
        object? Deserialize(string? serializedValue);
    }
    
}