using System;
using System.Collections.Generic;

namespace BlueprintDeck.ValueSerializer.Registration
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
            if (TryLoadSerializer(type, out var serializer)) return serializer!;
            throw  new ValueSerializerNotFoundException(type);
        }
        
        public bool TryLoadSerializer(Type type, out IRawValueSerializer? serializer)
        {
            lock (_serializers)
            {
                if (_serializers.TryGetValue(type, out var loadedSerializer))
                {
                    serializer = loadedSerializer;
                    return true;
                }

                var serializerType = typeof(IValueSerializer<>).MakeGenericType(type);
                loadedSerializer = (IRawValueSerializer?)_serviceProvider.GetService(serializerType);
                if (loadedSerializer == null)
                {
                    serializer = null;
                    return false;
                }

                _serializers[type] = loadedSerializer;
                serializer = loadedSerializer;
                return true;
            }
        }
    }
}