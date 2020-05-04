using System.Collections.Generic;
using BlueprintDeck.Design;

namespace BlueprintDeck.PrototypeTestApp
{
    public class TestDesign
    {
        public static BluePrint CreateDesign()
        {
            return new BluePrint
            {
                Nodes = new List<Design.Node>()
                {
                    new Design.Node
                    {
                        NodeTypeKey = "TestNode",
                        Key = "TestNode1",
                    },
                    new Design.Node
                    {
                        NodeTypeKey = "TestNode",
                        Key = "TestNode2",
                    },
                    new Design.Node
                    {
                        NodeTypeKey = "Activate",
                        Key = "Activate1",
                    },
                    new Design.Node
                    {
                        NodeTypeKey = "Delay",
                        Key = "Delay1",
                    }
                },
                ConstantValues = new List<Design.ConstantValue>()
                {
                    new Design.ConstantValue()
                    {
                        Key = "Duration1",
                        Title = "RainDuration",
                        Value = "15000",
                    }
                },
                Connections = new List<Connection>()
                {
                    new Connection
                    {
                        Key = "ActivateToDelay",
                        NodeTo = "Delay1",
                        NodePortTo = "Input",
                        NodeFrom = "Activate1",
                        NodePortFrom = "Event"
                    },
                    new Connection
                    {
                        Key = "DelayToTest1",
                        NodeTo = "TestNode1",
                        NodePortTo = "Trigger",
                        NodeFrom = "Delay1",
                        NodePortFrom = "Output"
                    },
                    new Connection
                    {
                        Key = "DelayToTest2",
                        NodeTo = "TestNode2",
                        NodePortTo = "Trigger",
                        NodeFrom = "Activate1",
                        NodePortFrom = "Event"
                    },
                    new Connection
                    {
                        Key = "DurationToDelay",
                        NodeTo = "Delay1",
                        NodePortTo = "Duration",
                        ConstantKey = "Duration1",
                        IsConstantConnection = true
                    }
                }
            };
        }
    }
}