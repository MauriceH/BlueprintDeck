using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueprintDeck.Design;
using BlueprintDeck.Node.Ports.Definitions;
using BlueprintDeck.Node.Ports.Definitions.DataTypes;
using BlueprintDeck.Registration;
using Newtonsoft.Json;

namespace BlueprintDeck.Designer
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            rightPanel.Width = 0;
           
            var registrations = new List<NodeRegistration>();
            var node1 = new NodeRegistration("Switch", "Switch", null, null, new List<NodePortDefinition>()
            {
                new NodePortDefinition("SwitchRef", "Switch Ref", InputOutputType.Input, typeof(PdtDuration),true),
                new NodePortDefinition("ShortPressed", "Short pressed", InputOutputType.Output, true),
                new NodePortDefinition("LongPressed", "Long pressed", InputOutputType.Output, true),
            },false);
            var node2 = new NodeRegistration("WaterControl", "WaterControl", null, null, new List<NodePortDefinition>()
            {
                new NodePortDefinition("StartStopTrigger", "Start/Stop", InputOutputType.Input, true),
                new NodePortDefinition("ExtendTrigger", "Extend runtime", InputOutputType.Input, true),
                new NodePortDefinition("DefaultDuration", "Default Duration", InputOutputType.Input,typeof(PdtDuration),true),
                new NodePortDefinition("ExtendDuration", "Extend Duration", InputOutputType.Input, typeof(PdtDuration),false),
                new NodePortDefinition("WaterOn", "Water on", InputOutputType.Output, true),
                new NodePortDefinition("WaterOff", "Water off", InputOutputType.Output, true),
            }, false);

            var node3 = new NodeRegistration("WaterActor", "Actor", null, null, new List<NodePortDefinition>()
            {
                new NodePortDefinition("Toggle", "Toggle", InputOutputType.Input, false),
                new NodePortDefinition("On", "On", InputOutputType.Input, false),
                new NodePortDefinition("Off", "Off", InputOutputType.Input, false),
            }, false);
            registrations.Add(node1);
            registrations.Add(node2);
            registrations.Add(node3);

            txtNodes.Text = JsonConvert.SerializeObject(new {Nodes = registrations}, Formatting.Indented, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore});

            var design = new BluePrintDesign
            {
                Nodes = new List<NodeDesignInstance>(),
                Connections = new List<DesignConnectionInstance>()
            };
            var n1 = new NodeDesignInstance()
            {
                NodeTypeKey = "WaterActor",
                NodeInstanceId = "WaterActor1",
                Title = "Water-Relay",
                Location = new Point(1200, 220),
            };
            var n2 = new NodeDesignInstance()
            {
                NodeTypeKey = "WaterControl",
                NodeInstanceId = "WaterControl1",
                Title = "Water-controller",
                Location = new Point(700, 200)
            };
            var n3 = new NodeDesignInstance()
            {
                NodeTypeKey = "Switch",
                NodeInstanceId = "Switch1",
                Title = "Switch",
                Location = new Point(200, 170)
            };
            design.Nodes.Add(n1);
            design.Nodes.Add(n2);
            design.Nodes.Add(n3);

            design.Connections.Add(new DesignConnectionInstance()
            {
                Id = "SwitchToControl1",
                NodeFrom = "Switch1",
                NodePortFrom = "ShortPressed",
                NodeTo = "WaterControl1",
                NodePortTo = "StartStopTrigger"
            });
            design.Connections.Add(new DesignConnectionInstance()
            {
                Id = "SwitchToControl2",
                NodeFrom = "Switch1",
                NodePortFrom = "LongPressed",
                NodeTo = "WaterControl1",
                NodePortTo = "ExtendTrigger"
            });
            design.Connections.Add(new DesignConnectionInstance()
            {
                Id = "ControlToActor1",
                NodeFrom = "WaterControl1",
                NodePortFrom = "WaterOn",
                NodeTo = "WaterActor1",
                NodePortTo = "On"
            });
            design.Connections.Add(new DesignConnectionInstance()
            {
                Id = "ControlToActor2",
                NodeFrom = "WaterControl1",
                NodePortFrom = "WaterOff",
                NodeTo = "WaterActor1",
                NodePortTo = "Off"
            });

            txtBlueprint.Text = JsonConvert.SerializeObject(design, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            applyCodeButton_Click(null, null);



        }

        private void displayCodeButton_Click(object sender, EventArgs e)
        {
            if (rightPanel.Width != 0)
            {
                rightPanel.Width = 0;
                displayCodeButton.Text = "Display Code";
            }
            else
            {
                rightPanel.Width = 650;
                displayCodeButton.Text = "Hide Code";
            }
        }

        private void applyCodeButton_Click(object sender, EventArgs e)
        {
            var data = new BlueprintData
            {
                Registry = JsonConvert.DeserializeObject<BlueprintRegistry>(txtNodes.Text),
                Blueprint = JsonConvert.DeserializeObject<BluePrintDesign>(txtBlueprint.Text),
            };


            blueprintDesigner1.Initialize(data);
        }
    }
}
