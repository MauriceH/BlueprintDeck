using System;

namespace BlueprintDeck.ValueSerializer.Serializer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StringValueSerializer : IValueSerializer<string>
    {
        public string? Serialize(object? value)
        {
            return value switch
            {
                null => null,
                string str => str,
                _ => throw new ArgumentException($"Invalid value type {value.GetType().Name}")
            };
        }
        

        public object? Deserialize(string? serializedValue)
        {
            return serializedValue;
        }
    }
}