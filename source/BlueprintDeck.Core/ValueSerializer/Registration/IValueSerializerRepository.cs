using System;

namespace BlueprintDeck.ValueSerializer.Registration
{
    internal interface IValueSerializerRepository
    {
        IRawValueSerializer? LoadSerializer(Type type);
        bool TryLoadSerializer(Type type, out IRawValueSerializer? serializer);
    }
}