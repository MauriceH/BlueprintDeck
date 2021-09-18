using System;
using System.Reflection;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.Node;

namespace BlueprintDeck.DependencyInjection
{
    public interface IBlueprintDeckRegistryBuilder
    {
        void RegisterNode<T>() where T : INode; 
        void RegisterNode(Type type); 
        void RegisterAssemblyNodes(Assembly assembly);
        void RegisterDataType<TDataType>();
        void RegisterDataType<TDataType>(string title);
        void RegisterDataType(Type type);
        void RegisterDataType(Type type, string title);
    }
}