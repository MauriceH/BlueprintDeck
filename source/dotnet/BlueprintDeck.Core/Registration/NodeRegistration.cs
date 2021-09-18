using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Registration
{
    internal record NodeRegistration(string Id, string Title, Type NodeType, IList<NodePortDefinition> PortDefinitions, IList<string> GenericTypes);
      
}