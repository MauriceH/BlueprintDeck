using System.Collections.Generic;

namespace BlueprintDeck.Design
{
    public class BluePrint
    {
        
        public List<Node>? Nodes { get; set; }

        public List<Connection>? Connections { get; set; }
        
        public List<ConstantValueNode>? ConstantValues { get; set; }
        
    }
}