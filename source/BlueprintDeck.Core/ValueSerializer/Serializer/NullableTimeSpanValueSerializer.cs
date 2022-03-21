using System;

namespace BlueprintDeck.ValueSerializer.Serializer;

public class NullableTimeSpanValueSerializer : IValueSerializer<TimeSpan?>
{
    public string? Serialize(object? value)
    {
        return value switch
        {
            null => null,
            TimeSpan ts => ts.ToString(),
            _ => throw new Exception("Serialized value is not a TimeSpan")
        };
    }

    public object? Deserialize(string? serializedValue)
    {
        if (string.IsNullOrWhiteSpace(serializedValue)) return null;
        if (!TimeSpan.TryParse(serializedValue, out var timeSpan))
            throw new Exception($"Cannot parse TimeSpan value \"{serializedValue}\"");
        return timeSpan;
    }
}