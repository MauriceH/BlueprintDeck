using System;
using System.Collections.Generic;
using BlueprintDeck.ConstantValue.Serializer;
using BlueprintDeck.DependencyInjection;

namespace BlueprintDeck.Registration
{
    public class ConstantValueSerializerRegistry : IConstantValueSerializerRepository
    {
        private readonly IDependencyRegistry _dependencyRegistry;
        private readonly Dictionary<Type, IRawConstantValueSerializer> _serializers = new Dictionary<Type, IRawConstantValueSerializer>();

        public ConstantValueSerializerRegistry(IDependencyRegistry dependencyRegistry)
        {
            _dependencyRegistry = dependencyRegistry;
        }

        public IRawConstantValueSerializer LoadSerializer(Type type)
        {
            lock (_serializers)
            {
                if (_serializers.TryGetValue(type, out var serializer)) return serializer;
                var serializerType = typeof(IConstantValueSerializer<>).MakeGenericType(type);
                serializer = (IRawConstantValueSerializer) _dependencyRegistry.ResolveOptional(serializerType);
                _serializers[type] = serializer ?? throw new ConstantValueSerializerNotFoundException(type);
                return serializer;
            }
        }

    }
}