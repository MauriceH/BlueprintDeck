using System;
using Autofac;
using BlueprintDeck.DependencyInjection;

namespace BlueprintDeck.AutoFac
{
    public class AutofacLifetimeDependencyRegistry : IDependencyRegistry
    {
        private readonly  ILifetimeScope _lifetimeScope;

        public AutofacLifetimeDependencyRegistry(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public T Resolve<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _lifetimeScope.Resolve(type);
        }

        public object ResolveOptional(Type type)
        {
            return _lifetimeScope.ResolveOptional(type);
        }
    }
}