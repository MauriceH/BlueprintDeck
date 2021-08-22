using System;
using System.Collections.Generic;
using BlueprintDeck.ConstantValue.Serializer;
using Microsoft.Extensions.DependencyInjection;

namespace BlueprintDeck.Registration
{
    public class ConstantValueSerializerRegistry : IConstantValueSerializerRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, IRawConstantValueSerializer> _serializers = new Dictionary<Type, IRawConstantValueSerializer>();

        public ConstantValueSerializerRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRawConstantValueSerializer LoadSerializer(Type type)
        {
            lock (_serializers)
            {
                if (_serializers.TryGetValue(type, out var serializer)) return serializer;
                var serializerType = typeof(IConstantValueSerializer<>).MakeGenericType(type);
                serializer = (IRawConstantValueSerializer) _serviceProvider.GetRequiredService(serializerType);
                _serializers[type] = serializer ?? throw new ConstantValueSerializerNotFoundException(type);
                return serializer;
            }
        }

    }
}