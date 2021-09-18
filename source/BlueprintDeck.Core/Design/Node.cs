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
        
        public Dictionary<string, string>? Properties { get; set; }

        public List<NodeGenericType>? GenericTypes { get; set; } 

    }
}