using System;
using System.Globalization;

namespace BlueprintDeck.ValueSerializer.Serializer;

public class NullableTimeSpanValueSerializer : IValueSerializer<TimeSpan?>
{
    public string? Serialize(object? value)
    {
        return value switch
        {
            null => null,
            TimeSpan ts => (ts.TotalMilliseconds).ToString(CultureInfo.CurrentCulture),
            _ => throw new Exception("Serialized value is not a TimeSpan")
        };
    }

    public object? Deserialize(string? serializedValue)
    {
        if (string.IsNullOrWhiteSpace(serializedValue)) return null;
        if (double.TryParse(serializedValue, out var result))
        {
            return TimeSpan.FromMilliseconds(result);
        }
        if (int.TryParse(serializedValue, out var resultInt))
        {
            return TimeSpan.FromMilliseconds(resultInt);
        }
        if (TimeSpan.TryParse(serializedValue, out var timeSpan))
        {
            return timeSpan;
        }
        throw new Exception($"Cannot parse TimeSpan value \"{serializedValue}\"");
    }
}