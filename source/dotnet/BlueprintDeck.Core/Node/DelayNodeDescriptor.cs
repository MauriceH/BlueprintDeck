using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;

namespace BlueprintDeck.Node
{
    public class DelayNodeDescriptor : INodeDescriptor
    {
        public const string NodeKey = "Delay";
        public static  NodePortDefinition Input { get; } = NodePortDefinitionFactory.CreateInput("Input","Input",true);
        public static  NodePortDefinition DelayDuration { get; } = NodePortDefinitionFactory.CreateDataInput("Duration","Duration",new PdtDuration(TimeSpan.FromSeconds(10)),true);
        public static  NodePortDefinition Output { get; } = NodePortDefinitionFactory.CreateOutput("Output","Output");

        public IList<NodePortDefinition> PortDefinitions => new List<NodePortDefinition>()
        {
            Input,
            DelayDuration,
            Output
        };
    }
}