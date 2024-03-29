using System;
using System.Reflection;
using BlueprintDeck.Node;
using BlueprintDeck.ValueSerializer;

namespace BlueprintDeck.DependencyInjection
{
    public interface IBlueprintRegistryBuilder
    {
        void RegisterNode<T>() where T : INode; 
        void RegisterNode(Type type); 
        void RegisterAssemblyNodes(Assembly assembly);
        void RegisterDataType<TDataType>();
        void RegisterDataType<TDataType>(string title);
        void RegisterDataType(Type type);
        void RegisterDataType(Type type, string title);
        void RegisterSerializer<T, TValueType>() where T : class, IValueSerializer<TValueType>;
    }
}