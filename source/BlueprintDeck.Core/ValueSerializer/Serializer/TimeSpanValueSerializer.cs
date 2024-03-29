using System;
using System.Globalization;

namespace BlueprintDeck.ValueSerializer.Serializer
{
    public class TimeSpanValueSerializer : IValueSerializer<TimeSpan>
    {
        public string? Serialize(object? value)
        {
            return value switch
            {
                null => null,
                TimeSpan ts => (ts.TotalMilliseconds).ToString(CultureInfo.CurrentCulture),
                _ => throw new ArgumentException($"Invalid value type {value.GetType().Name}")
            };
        }

        public object? Deserialize(string? serializedValue)
        {
            if (serializedValue == null) return null;
            if (double.TryParse(serializedValue, out var result))
            {
                return TimeSpan.FromMilliseconds(result);
            }
            throw new ArgumentException($"Cannot parse \"{result}\" to TimeSpan");
        }
    }
}