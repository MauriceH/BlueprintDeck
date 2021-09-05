using System;
using System.Collections.Generic;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.Registration
{
    public class ConstantValueSerializerRepository : IConstantValueSerializerRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, IRawConstantValueSerializer> _serializers = new();

        public ConstantValueSerializerRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRawConstantValueSerializer LoadSerializer(Type type)
        {
            lock (_serializers)
            {
                if (_serializers.TryGetValue(type, out var serializer)) return serializer;
                var serializerType = typeof(IConstantValueSerializer<>).MakeGenericType(type);
                var newSerializer = (IRawConstantValueSerializer?)_serviceProvider.GetService(serializerType);
                _serializers[type] = newSerializer ?? throw new ConstantValueSerializerNotFoundException(type);
                return newSerializer;
            }
        }
    }
}