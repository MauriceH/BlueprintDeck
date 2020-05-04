using System;
using System.Collections.Generic;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Node.Default
{
    public class DelayNodeDescriptor : INodeDescriptor
    {
        public const string NodeKey = "Delay";
        public static  NodePortDefinition Input { get; } = NodePortDefinitionFactory.CreateInput("Input","Input");
        public static  NodePortDefinition DelayDuration { get; } = NodePortDefinitionFactory.CreateDataInput("Duration","Duration",TimeSpan.FromSeconds(10));
        public static  NodePortDefinition Output { get; } = NodePortDefinitionFactory.CreateOutput("Output","Output");

        public IList<NodePortDefinition> PortDefinitions => new List<NodePortDefinition>()
        {
            Input,
            DelayDuration,
            Output
        };
    }
}