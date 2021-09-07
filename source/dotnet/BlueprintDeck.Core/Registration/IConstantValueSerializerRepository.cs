using System;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.Registration
{
    internal interface IConstantValueSerializerRepository
    {
        IRawConstantValueSerializer? LoadSerializer(Type type);
    }
}