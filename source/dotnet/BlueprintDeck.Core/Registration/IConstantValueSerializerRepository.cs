using System;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.Registration
{
    public interface IConstantValueSerializerRepository
    {
        IRawConstantValueSerializer LoadSerializer(Type type);
    }
}