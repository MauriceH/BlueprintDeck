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
                        Location = new NodeLocation(500,100),
                        NodeTypeKey = "TestNode",
                        Key = "TestNode1",
                        Title = "TestNode1"
                    },
                    new Design.Node
                    {
                        Location = new NodeLocation(500,300),
                        NodeTypeKey = "TestNode",
                        Key = "TestNode2",
                        Title = "TestNode2"
                    },
                    new Design.Node
                    {
                        Location = new NodeLocation(100,200),
                        NodeTypeKey = "Activate",
                        Key = "Activate1",
                    },
                    new Design.Node
                    {
                        Location = new NodeLocation(250,200),
                        NodeTypeKey = "Delay",
                        Key = "Delay1",
                    }
                },
                ConstantValues = new List<ConstantValueNode>()
                {
                    new ConstantValueNode()
                    {
                        Key = "Duration1",
                        Title = "RainDuration",
                        Value = "15000",
                        NodeTypeKey = "timespan"
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
                        NodeFrom = "Duration1",
                        NodePortFrom = "value"
                    }
                }
            };
        }
    }
}