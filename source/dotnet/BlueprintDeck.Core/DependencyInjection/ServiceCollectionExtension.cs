using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.Instance.Factory;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static void AddBlueprintDeck(this IServiceCollection services, Action<IBlueprintDeckRegistryBuilder> config)
        {
            
            services.AddSingleton<ConstantValueSerializerRegistry>();
            services.AddSingleton<IConstantValueSerializerRepository>(provider => provider.GetRequiredService<ConstantValueSerializerRegistry>());

            services.AddSingleton<RegistryFactory>();
            services.AddSingleton<IRegistryFactory>(provider => provider.GetRequiredService<RegistryFactory>());
            
            services.AddSingleton<BluePrintFactory>();
            services.AddSingleton<IBluePrintFactory>(provider => provider.GetRequiredService<BluePrintFactory>());

            services.AddSingleton<NodeFactory>();
            services.AddSingleton<INodeFactory>(provider => provider.GetRequiredService<NodeFactory>());

            var blueprintDeckBuilder = new RegistryBuilder(services);
            blueprintDeckBuilder.RegisterConstantValue<DoubleConstantValueSerializer, double>("double", "Double value");
            blueprintDeckBuilder.RegisterConstantValue<Int32ConstantValueSerializer, int>("int32", "Int32 value");
            blueprintDeckBuilder.RegisterConstantValue<TimeSpanConstantValueSerializer, TimeSpan>("timespan", "TimeSpan value");
            blueprintDeckBuilder.RegisterConstantValue<StringConstantValueSerializer, string>("string", "String value");

            blueprintDeckBuilder.RegisterAssemblyNodes(Assembly.GetExecutingAssembly());

            config(blueprintDeckBuilder);
        }


        private class RegistryBuilder : IBlueprintDeckRegistryBuilder
        {
            private readonly IServiceCollection _services;
            private readonly NodeRegistrationResolver _resolver;
            private readonly Dictionary<Type, DataTypeRegistration> _dataTypes = new Dictionary<Type, DataTypeRegistration>();
            private readonly SHA1 _sha1 = SHA1.Create();

            public RegistryBuilder(IServiceCollection services)
            {
                _services = services;
                _resolver = new NodeRegistrationResolver();
            }

            public void RegisterNode<T>() where T : INode
            {
                var nodeRegistration = _resolver.CreateNodeRegistration<T>() ?? throw new ArgumentException("Type not configured");
                RegisterRegistration(nodeRegistration);
            }

            public void RegisterAssemblyNodes(Assembly assembly)
            {
                var registrations = _resolver.ResolveNodeRegistrations(assembly);
                foreach (var registration in registrations)
                {
                    RegisterRegistration(registration);
                }
            }

            private void RegisterRegistration(NodeRegistration? registration)
            {
                foreach (var portDef in registration.PortDefinitions.Where(x => x.DataMode == DataMode.WithData))
                {
                    var type = portDef.PortDataType ??
                               throw new Exception($"Port {portDef.Key} of node type {registration.Key} registered as WithData without datatype");
                    if (_dataTypes.ContainsKey(type)) continue;
                    RegisterDataType(type);
                }

                _services.AddSingleton(registration);
                _services.AddTransient(registration.NodeType);
                _services.AddTransient(provider => (INode) provider.GetRequiredService(registration.NodeType));
                //_services.AddSingleton(registration.NodeDescriptorType);
            }

            public void RegisterConstantValue<TSerializer, TDataType>(string key, string title) where TSerializer : class, IConstantValueSerializer<TDataType>
            {
                _services.AddSingleton<TSerializer>();
                _services.AddSingleton<IConstantValueSerializer<TDataType>>(provider => provider.GetRequiredService<TSerializer>());

                RegisterDataType<TDataType>();

                var nodePortDefinition = new NodePortDefinition("value",title,InputOutputType.Output,typeof(TDataType),false);
                var constantValueRegistration = new ConstantValueRegistration(key, title, typeof(TDataType), nodePortDefinition, (context,valueReceiver) =>
                {
                    var output = context.GetPort<IOutput<TDataType>>(nodePortDefinition);
                    output?.Emit((TDataType)valueReceiver());
                });
                _services.AddSingleton(constantValueRegistration);
            }

            public void RegisterDataType<TDataType>()
            {
                var type = typeof(TDataType);
                RegisterDataType(type, type.Name);
            }

            public void RegisterDataType<TDataType>(string title)
            {
                RegisterDataType(typeof(TDataType), title);
            }

            public void RegisterDataType(Type type)
            {
                RegisterDataType(type, type.Name);
            }

            public void RegisterDataType(Type type, string title)
            {
                if (_dataTypes.ContainsKey(type)) throw new Exception($"Data type {type.FullName} already registered");
                var typeHash = _sha1.ComputeHash(type.FullName!);
                var dataTypeRegistration = new DataTypeRegistration($"{type.Name}-{typeHash}", type, title);
                _dataTypes[type] = dataTypeRegistration;
                _services.AddSingleton(dataTypeRegistration);
            }
        }
    }
}