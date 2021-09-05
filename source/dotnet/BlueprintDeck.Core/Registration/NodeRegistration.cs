using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    public record NodeRegistration(string Key, string Title, Type NodeType, IList<NodePortDefinition> PortDefinitions);
      
}