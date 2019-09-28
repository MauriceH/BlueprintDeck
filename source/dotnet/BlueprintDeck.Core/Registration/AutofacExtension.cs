using System;
using System.Reflection;
using Autofac;
using BlueprintDeck.Node;

namespace BlueprintDeck.Design
{
    public static  class AutofacExtension
    {

        public static void RegisterBlueprintDeck(this ContainerBuilder builder, Action<IBlueprintDeckAutofacBuilder> config)
        {
            builder.RegisterModule<BluePrintDependencyModule>();
            var blueprintDeckBuilder = new BlueprintDeckAutofacBuilder(builder);
            blueprintDeckBuilder.RegisterAssemblyNodes(Assembly.GetExecutingAssembly());
            config(blueprintDeckBuilder);
        }


        private class BlueprintDeckAutofacBuilder : IBlueprintDeckAutofacBuilder
        {
            private readonly ContainerBuilder _builder;
            private readonly NodeRegistrationAssemblyResolver _resolver;
            public BlueprintDeckAutofacBuilder(ContainerBuilder builder)
            {
                _builder = builder;
                _resolver = new NodeRegistrationAssemblyResolver();
            }

            public void RegisterAssemblyNodes(Assembly assembly)
            {
                var registrations = _resolver.ResolveNodeRegistrations(assembly);
                foreach (var registration in registrations)
                {
                    _builder.RegisterType(registration.NodeType).AsSelf().As<INode>().InstancePerDependency();
                    _builder.RegisterType(registration.NodeDescriptorType).AsSelf().InstancePerDependency();
                }
            }
        }
    }

    public interface IBlueprintDeckAutofacBuilder
    {
        void RegisterAssemblyNodes(Assembly assembly);
    }
}