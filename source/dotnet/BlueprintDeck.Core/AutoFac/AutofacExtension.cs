using System;
using System.Reflection;
using Autofac;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.DependencyInjection;
using BlueprintDeck.Node;
using BlueprintDeck.Registration;

namespace BlueprintDeck.AutoFac
{
    public static  class AutofacExtension
    {

        public static void RegisterBlueprintDeck(this ContainerBuilder builder, Action<IBlueprintDeckAutofacBuilder> config)
        {
            builder.RegisterModule<BluePrintDependencyModule>();
            builder.RegisterType<AutofacLifetimeDependencyRegistry>().As<IDependencyRegistry>().InstancePerDependency();

            builder.RegisterType<ConstantValueSerializerRegistry>()
                .AsSelf()
                .As<IConstantValueSerializerRepository>()
                .SingleInstance();
            
            builder.RegisterType<NodeRegistryFactory>()
                .AsSelf()
                .As<INodeRegistryFactory>()
                .SingleInstance();
            
            var blueprintDeckBuilder = new BlueprintDeckAutofacBuilder(builder);
            blueprintDeckBuilder.RegisterAssemblyNodes(Assembly.GetExecutingAssembly());
            blueprintDeckBuilder.RegisterConstantValueSerializer<DoubleConstantValueSerializer,double>();
            blueprintDeckBuilder.RegisterConstantValueSerializer<Int32ConstantValueSerializer,int>();
            blueprintDeckBuilder.RegisterConstantValueSerializer<TimeSpanConstantValueSerializer,TimeSpan>();
            blueprintDeckBuilder.RegisterConstantValueSerializer<StringConstantValueSerializer,string>();
            
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
            
            public void RegisterConstantValueSerializer<TSerializer,TDataType>() where TSerializer : IConstantValueSerializer<TDataType>
            {
                _builder.RegisterType<TSerializer>().AsSelf().As<IConstantValueSerializer<TDataType>>().SingleInstance();
            }
        }
    }

    public interface IBlueprintDeckAutofacBuilder
    {
        void RegisterAssemblyNodes(Assembly assembly);
    }
}