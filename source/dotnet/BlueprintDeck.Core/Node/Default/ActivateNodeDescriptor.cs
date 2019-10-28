using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node
{
    public class ActivateNodeDescriptor : INodeDescriptor
    {
            
        public static readonly NodePortDefinition Definition = NodePortDefinitionFactory.CreateOutput("Event","OnActivate");

        public ActivateNodeDescriptor()
        {
            PortDefinitions = new List<NodePortDefinition>()
            {
                Definition
            };
        }

        public IList<NodePortDefinition> PortDefinitions { get; }
    }
}