using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using BlueprintDeck.ConstantValue.Registration;
using BlueprintDeck.DataTypes.Registration;
using BlueprintDeck.Design.Registry;
using BlueprintDeck.Instance.Factory;
using BlueprintDeck.Misc;
using BlueprintDeck.Node;
using BlueprintDeck.Node.Ports.Registration;
using BlueprintDeck.Node.Properties.Registration;
using BlueprintDeck.Node.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static void AddBlueprintDeck(this IServiceCollection services)
        {
                services.AddBlueprintDeck(_ => { });
        }

        public static void AddBlueprintDeck(this IServiceCollection services, Action<IBlueprintRegistryBuilder> config)
        {
            services.AddSingleton<ValueSerializerRepository>();
            services.AddSingleton<IValueSerializerRepository>(provider => provider.GetRequiredService<ValueSerializerRepository>());

            services.AddSingleton<RegistryFactory>();
            services.AddSingleton<IRegistryFactory>(provider => provider.GetRequiredService<RegistryFactory>());

            services.AddSingleton<BlueprintFactory>();
            services.AddSingleton<IBlueprintFactory>(provider => provider.GetRequiredService<BlueprintFactory>());

            services.AddSingleton<NodeFactory>();
            services.AddSingleton<INodeFactory>(provider => provider.GetRequiredService<NodeFactory>());

            services.AddTransient(provider => provider.GetServices<NodeRegistration>().ToList());
            services.AddTransient(provider => provider.GetServices<DataTypeRegistration>().ToList());

            services.AddTransient<IPortConnectionManager, PortConnectionManager>();

            var blueprintDeckBuilder = new RegistryBuilder(services);

            blueprintDeckBuilder.RegisterAssemblyNodes(Assembly.GetExecutingAssembly());

            config(blueprintDeckBuilder);
        }


        private class RegistryBuilder : IBlueprintRegistryBuilder
        {
            private readonly Dictionary<Type, DataTypeRegistration> _dataTypes = new();
            private readonly NodeRegistrationFactory _factory;
            private readonly IServiceCollection _services;
            private readonly SHA1 _sha1 = SHA1.Create();

            public RegistryBuilder(IServiceCollection services)
            {
                _services = services;
                _factory = new NodeRegistrationFactory(new AssemblyTypesResolver(), new PortRegistrationFactory(), new GenericTypeParametersFactory(),
                    new PropertyRegistrationFactory());
            }

            public void RegisterNode<T>() where T : INode
            {
                RegisterNode(typeof(T));
            }

            public void RegisterNode(Type type)
            {
                var nodeRegistration = _factory.CreateNodeRegistration(type) ?? throw new ArgumentException("Type not configured");
                RegisterRegistration(nodeRegistration);
            }

            public void RegisterAssemblyNodes(Assembly assembly)
            {
                var registrations = _factory.CreateNodeRegistrationsByAssembly(assembly);
                foreach (var registration in registrations) RegisterRegistration(registration);
            }

            public void RegisterDataType<TDataType>()
            {
                var type = typeof(TDataType);
                RegisterDataType<TDataType>(type.Name);
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

            private void RegisterRegistration(NodeRegistration registration)
            {
                foreach (var portDef in registration.Ports.Where(x => x.DataType != null))
                {
                    var type = portDef.DataType ??
                               throw new Exception($"Port {portDef.Key} of node type {registration.Id} registered as WithData without datatype");
                    if (_dataTypes.ContainsKey(type)) continue;
                    RegisterDataType(type);
                }

                _services.AddSingleton(registration);
                _services.AddTransient(registration.NodeType);
            }
        }
    }
}