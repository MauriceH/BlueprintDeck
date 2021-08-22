using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Int32ConstantValueSerializer : IConstantValueSerializer<int>
    {
        public string? Serialize(object? value)
        {
            return value?.ToString();
        }
        
        public object? Deserialize(string? serializedValue)
        {
            if (serializedValue == null) return null;
            if (int.TryParse(serializedValue, out var result)) return result;
            throw new Exception($"Cannot parse \"{result}\" to int32");
        }
    }
}