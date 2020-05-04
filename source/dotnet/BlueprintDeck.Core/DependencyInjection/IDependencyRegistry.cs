using System;

namespace BlueprintDeck.DependencyInjection
{
    public interface IDependencyRegistry
    {
        T Resolve<T>();
        object Resolve(Type type);
        object ResolveOptional(Type type);
    }
}