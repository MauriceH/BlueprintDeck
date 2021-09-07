using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace BlueprintDeck.Design
{
    public class Node
    {
        public NodeLocation? Location { get; set; }
        public string? Title { get; set; }
        public string? Key { get; set; }
        
        public string? NodeTypeKey { get; set; }
        
        public JToken? Data { get; set; }
        
        public List<NodeGenericType>? GenericTypes { get; set; } 

    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class NodeLocation
    {
        public int X { get; set; }
        public int Y { get; set; }

        public NodeLocation()
        {
        }

        public NodeLocation(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class NodeGenericType
    {
        public string? GenericParameter { get; set; }
        public string? TypeId { get; set; }
    }
}