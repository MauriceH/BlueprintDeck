using System;
using System.Reflection;
using Autofac;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.Node;

namespace BlueprintDeck.Registration
{
    public static  class AutofacExtension
    {

        public static void RegisterBlueprintDeck(this ContainerBuilder builder, Action<IBlueprintDeckAutofacBuilder> config)
        {
            builder.RegisterModule<BluePrintDependencyModule>();

            builder.RegisterType<ConstantValueSerializerRegistry>()
                .AsSelf()
                .As<IConstantValueSerializerRepository>()
                .SingleInstance();
            
            var blueprintDeckBuilder = new BlueprintDeckAutofacBuilder(builder);
            blueprintDeckBuilder.RegisterAssemblyNodes(Assembly.GetExecutingAssembly());
            blueprintDeckBuilder.RegisterConstantValueSerializer<DoubleConstantValueSerializer>("Double");
            blueprintDeckBuilder.RegisterConstantValueSerializer<Int32ConstantValueSerializer>("int");
            blueprintDeckBuilder.RegisterConstantValueSerializer<TimeSpanConstantValueSerializer>("timespan");
            
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
                    _builder.RegisterInstance(registration).As<NodeRegistration>().SingleInstance();
                    _builder.RegisterType(registration.NodeType).AsSelf().As<INode>().InstancePerDependency();
                    _builder.RegisterType(registration.NodeDescriptorType).AsSelf().InstancePerDependency();
                }
            }

            public void RegisterConstantValueSerializer<T>(string typeName) where T : IConstantValueSerializer
            {
                ConstantValueSerializerRegistry.Register<T>(_builder, typeName);
            }
            
            
           
        }
    }

    public interface IBlueprintDeckAutofacBuilder
    {
        void RegisterAssemblyNodes(Assembly assembly);
    }
}