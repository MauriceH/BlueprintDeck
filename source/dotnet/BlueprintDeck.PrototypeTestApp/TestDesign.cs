using System.Collections.Generic;
using BlueprintDeck.Design;

namespace BlueprintDeck.PrototypeTestApp
{
    public class TestDesign
    {
        public static BluePrintDesign CreateDesign()
        {
            return new BluePrintDesign
            {
                Nodes = new List<NodeDesignInstance>()
                {
                    new NodeDesignInstance
                    {
                        NodeTypeKey = "TestNode",
                        NodeInstanceId = "TestNode1",
                    },
                    new NodeDesignInstance
                    {
                        NodeTypeKey = "Activate",
                        NodeInstanceId = "Activate1",
                    },
                    new NodeDesignInstance
                    {
                        NodeTypeKey = "Delay",
                        NodeInstanceId = "Delay1",
                    },
                    new NodeDesignInstance
                    {
                        NodeTypeKey = "PdtDuration",
                        NodeInstanceId = "Duration1",
                        Value = "5000"
                    }
                },
                Connections = new List<DesignConnectionInstance>()
                {
                    new DesignConnectionInstance
                    {
                        Id = "ActivateToDelay",
                        NodeTo = "Delay1",
                        NodePortTo = "Input",
                        NodeFrom = "Activate1",
                        NodePortFrom = "Event"
                    },
                    new DesignConnectionInstance
                    {
                        Id = "DelayToTest",
                        NodeTo = "TestNode1",
                        NodePortTo = "Trigger",
                        NodeFrom = "Delay1",
                        NodePortFrom = "Output"
                    },
                    new DesignConnectionInstance
                    {
                        Id = "DurationToDelay",
                        NodeTo = "Delay1",
                        NodePortTo = "Duration",
                        NodeFrom = "Duration1",
                        NodePortFrom = "Value"
                    }
                }
            };
        }
    }
}