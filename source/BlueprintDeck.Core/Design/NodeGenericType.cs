using System.Diagnostics.CodeAnalysis;

namespace BlueprintDeck.Design
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class NodeGenericType
    {
        public string? GenericParameter { get; init; }
        public string? TypeId { get; init; }
    }
}