using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StringConstantValueSerializer : IConstantValueSerializer<string>
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