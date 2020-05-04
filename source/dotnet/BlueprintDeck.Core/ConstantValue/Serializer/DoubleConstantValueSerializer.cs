using System;

namespace BlueprintDeck.ConstantValue.Serializer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DoubleConstantValueSerializer : IConstantValueSerializer<double>
    {
        public string? Serialize(object? value)
        {
            return value == null ? null : $"{value}";
        }
        
        public object? Deserialize(string? serializedValue)
        {
            if (serializedValue == null) return null;
            if (!double.TryParse(serializedValue, out var dbl)) throw new ArgumentException("Value cannot be parsed to double");
            return dbl;
        }
    }
}