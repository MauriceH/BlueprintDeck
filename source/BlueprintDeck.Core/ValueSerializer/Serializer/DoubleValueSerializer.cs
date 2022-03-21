using System;

namespace BlueprintDeck.ValueSerializer.Serializer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DoubleValueSerializer : IValueSerializer<double>
    {
        public string? Serialize(object? value)
        {
            if (value == null) return null;
            if (value is not double dbl) throw new ArgumentException($"Invalid type {value.GetType().Name}");
            return $"{dbl}";
        }
        
        public object? Deserialize(string? serializedValue)
        {
            if (serializedValue == null) return null;
            if (!double.TryParse(serializedValue, out var dbl)) throw new ArgumentException("Value cannot be parsed to double");
            return dbl;
        }
    }
}