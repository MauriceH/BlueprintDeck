using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.Registration
{
    public interface IConstantValueSerializerRepository
    {
        IConstantValueSerializer LoadSerializer(string typeName);
    }
}