using Autofac;
using BlueprintDeck.ConstantValue.Serializer;

namespace BlueprintDeck.Registration
{
    public class ConstantValueSerializerRegistry : IConstantValueSerializerRepository
    {
        private readonly ILifetimeScope _lifetimeScope;

        public ConstantValueSerializerRegistry(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public static void Register<T>(ContainerBuilder builder, string typeName) where T: IConstantValueSerializer
        {
            builder.RegisterType(typeof(T))
                .Named<IConstantValueSerializer>(CreateTypeName(typeName));
        }

        private static string CreateTypeName(string typeName)
        {
            return $"serializer:{typeName.ToLower()}";
        }

        public IConstantValueSerializer LoadSerializer(string typeName)
        {
            var serializer = _lifetimeScope.ResolveNamed<IConstantValueSerializer>(CreateTypeName(typeName));
            return serializer;
        }

    }
}