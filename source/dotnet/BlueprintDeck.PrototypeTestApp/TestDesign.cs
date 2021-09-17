using System.Collections.Generic;
using BlueprintDeck.Design;
using Newtonsoft.Json.Linq;

namespace BlueprintDeck.PrototypeTestApp
{
    public class TestDesign
    {
        public static Blueprint CreateDesign()
        {
            return new Blueprint
            {
                Nodes = new List<Design.Node>()
                {
                    new Design.Node
                    {
                        Location = new NodeLocation(500,100),
                        NodeTypeKey = "TestNode",
                        Id = "TestNode1",
                        Title = "TestNode1"
                    },
                    new Design.Node
                    {
                        Location = new NodeLocation(500,300),
                        NodeTypeKey = "TestNode",
                        Id = "TestNode2",
                        Title = "TestNode2"
                    },
                    new Design.Node
                    {
                        Location = new NodeLocation(100,200),
                        NodeTypeKey = "Activate",
                        Id = "Activate1",
                    },
                    new Design.Node
                    {
                        Location = new NodeLocation(250,200),
                        NodeTypeKey = "Delay",
                        Id = "Delay1",
                        Data = JToken.Parse("{\"DefaultMilliseconds\":10000}")
                    }
                },
                ConstantValues = new List<ConstantValueNode>()
                {
                    new ConstantValueNode()
                    {
                        Id = "Duration1",
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