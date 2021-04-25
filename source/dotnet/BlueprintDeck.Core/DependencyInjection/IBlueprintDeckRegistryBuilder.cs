using System;
using System.Reflection;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.DependencyInjection
{
    public interface IBlueprintDeckRegistryBuilder
    {
        void RegisterAssemblyNodes(Assembly assembly);
        void RegisterConstantValue<TSerializer, TDataType>(string key, string title) where TSerializer : class, IConstantValueSerializer<TDataType>;
        void RegisterDataType<TDataType>();
        void RegisterDataType<TDataType>(string title);
        void RegisterDataType(Type type);
        void RegisterDataType(Type type, string title);
    }
}