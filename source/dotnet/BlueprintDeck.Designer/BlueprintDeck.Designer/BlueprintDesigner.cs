using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Designer
{
    public partial class BlueprintDesigner : Panel
    {

        private Pen GridPen { get; set; } = new Pen(new SolidBrush(Color.FromArgb(35,49,63)),2);
        private double zoom = 1;
        private int difX;
        private int difY;
        private Point onDown;
        private Dictionary<string, NodeDisplay> _nodes = new Dictionary<string, NodeDisplay>();
        

        public BlueprintDesigner()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void AddNode(NodeDisplay node)
        {
            Controls.Add(node);
            _nodes.Add(node.Id, node);
        }

        public void Initialize(BlueprintData data)
        {
            _nodes.Clear();
            Controls.Clear();

            var nodes = new List<NodeDisplay>();
            foreach (var designNode in data.Blueprint.Nodes)
            {
                var tp = data.Registry.Nodes.FirstOrDefault(x => x.Id == designNode.NodeTypeKey);

                var disp = new NodeDisplay
                {
                    Id = designNode.NodeInstanceId,
                    Title = designNode.Title,
                    TypeName = tp.Title,
                    DesignLocation = designNode.Location
                };
                foreach (var port in tp.PortDefinitions)
                {
                    var prt = new Port()
                    {
                        Key = port.Key,
                        Title = port.Title,
                        IsData = port.DataMode == DataMode.WithData,
                        IsMandatory = port.Mandatory,
                    };
                    if (port.InputOutputType == InputOutputType.Input)
                    {
                        disp.InputPorts.Add(prt);
                        continue;
                    }
                    disp.OutputPorts.Add(prt);

                    var con = data.Blueprint.Connections.FirstOrDefault(x => x.NodeFrom == designNode.NodeInstanceId && x.NodePortFrom == port.Key);
                    if (con == null) continue;
                    prt.ConnectionNodeId = con.NodeTo;
                    prt.ConnectionNodePortKey = con.NodePortTo;

                }
                AddNode(disp);
                nodes.Add(disp);
            }

            foreach (var nd in nodes)
            {
                foreach (var output in nd.OutputPorts)
                {
                    if (output.ConnectionNodeId == null || output.ConnectionNodePortKey == null) continue;
                    var inputNode = nodes.FirstOrDefault(x => x.Id == output.ConnectionNodeId);
                    var inputPort = inputNode?.InputPorts.FirstOrDefault(x => x.Key == output.ConnectionNodePortKey);
                    if (inputPort == null) continue;
                    inputPort.IsConnected = true;
                    output.IsConnected = true;
                }
            }

            SetItems();
            foreach (Control control in Controls)
            {
                if (control is NodeDisplay nd)
                {
                    nd.Initialize();
                }
            }
            SetItems();
            Refresh();
            Refresh();
        }

        


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            //var zoomChange = 0.0;
            //if (numberOfTextLinesToMove > 0)
            //{
            //    if (zoom >= 2.5) return;
            //    zoomChange = 0.1;
            //}
            //else
            //{
            //    if (zoom <= 0.4) return;
            //    zoomChange = -0.1;
            //}
            //var difBeforeX = (int)(e.Location.X );
            ////var difBeforeY = (int)(e.Location.Y * zoom);
            //zoom += zoomChange;
            //var difAfterX = (int)(e.Location.X * zoom);
            ////var difAfterY = (int)(e.Location.Y * zoom);
            ////difX -= e.Location.X; //difAfterX - difBeforeX;
            ////difY -= difAfterY - difBeforeY;

            //SetItems();
            //this.Refresh();
        }

        private void SetItems()
        {
            foreach (Control control in this.Controls)
            {
                if (!(control is NodeDisplay nd)) continue;
                //nd.Zoom = zoom;
                nd.Left = ((int) (nd.DesignLocation.X * zoom) + difX);
                nd.Top = ((int) (nd.DesignLocation.Y * zoom) + difY);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            onDown = e.Location;
            this.Cursor = Cursors.SizeAll;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                difX += e.X - onDown.X;
                difY += e.Y - onDown.Y;
                SetItems();
                //Debug.WriteLine($"X: {difX:000} Y: {difY:000}");
                this.Refresh();
            }
            
            onDown = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var width = ClientRectangle.Width;
            var height = ClientRectangle.Height;
            var size = (int)(zoom * 50);

            var xCount = (int)Math.Ceiling(width / (double) size);
            var yCount = (int)Math.Ceiling(height / (double) size);

            var g = e.Graphics;

            for (var x = 0; x < xCount+1; x++)
            {
                var xx = (x * size) + (difX % size);
                g.DrawLine(GridPen, xx,0,xx,height);
            }

            for (var y = 0; y < yCount+1; y++)
            {
                var yy = (y * size) + (difY % size);
                g.DrawLine(GridPen, 0, yy, width, yy);
            }

            var blackPen = new Pen(Color.White, 2);

            if (Controls.Count < 0) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var node in _nodes.Values)
            {
                foreach (var output in node.OutputPorts)
                {
                    if(!output.IsConnected) continue;
                    
                    var n1 = node;

                    if(!_nodes.TryGetValue(output.ConnectionNodeId,out var n2)) continue;
                    var n2Input = n2.InputPorts.FirstOrDefault(x=> x.Key == output.ConnectionNodePortKey);
                    if(n2Input == null) continue;

                    var n1PosY = output.PositionY;
                    var n2PosY = n2Input.PositionY;

                    // Create points for curve.
                    var start = new PointF(n1.Right - 1, n1.Top + n1PosY);
                    var control1 = new PointF(n1.Right + 150, n1.Top + n1PosY);
                    var control2 = new PointF(n2.Left - 150, n2.Top + n2PosY);
                    var end = new PointF(n2.Left, n2.Top + n2PosY);

                    // Draw arc to screen.
                    e.Graphics.DrawBezier(blackPen, start, control1, control2, end);
                }
                
            }

            


        }
    }
}
