using System;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.ConstantValue.Registration
{
    internal interface IConstantValueSerializerRepository
    {
        IRawConstantValueSerializer? LoadSerializer(Type type);
    }
}