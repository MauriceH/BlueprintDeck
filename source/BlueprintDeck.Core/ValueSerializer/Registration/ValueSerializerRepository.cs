using System;
using System.Collections.Generic;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.ConstantValue.Registration
{
    internal class ValueSerializerRepository : IValueSerializerRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, IRawValueSerializer> _serializers = new();

        public ValueSerializerRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRawValueSerializer LoadSerializer(Type type)
        {
            lock (_serializers)
            {
                if (_serializers.TryGetValue(type, out var serializer)) return serializer;
                var serializerType = typeof(IValueSerializer<>).MakeGenericType(type);
                var newSerializer = (IRawValueSerializer?)_serviceProvider.GetService(serializerType);
                _serializers[type] = newSerializer ?? throw new ValueSerializerNotFoundException(type);
                return newSerializer;
            }
        }
    }
}