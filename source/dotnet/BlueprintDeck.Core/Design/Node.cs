using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BlueprintDeck.Design
{
    public class Node
    {
        public NodeLocation? Location { get; set; }
        public string? Title { get; set; }
        public string? Id { get; set; }
        
        public string? NodeTypeKey { get; set; }
        
        public JToken? Data { get; set; }
        
        public List<NodeGenericType>? GenericTypes { get; set; } 

    }
}