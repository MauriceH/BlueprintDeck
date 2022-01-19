using System;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.ConstantValue.Registration
{
    internal interface IValueSerializerRepository
    {
        IRawValueSerializer? LoadSerializer(Type type);
        bool TryLoadSerializer(Type type, out IRawValueSerializer? serializer);
    }
}