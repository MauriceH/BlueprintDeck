using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BlueprintDeck.Design;
using BlueprintDeck.Node.Ports.Definitions;

namespace BlueprintDeck.Designer
{
    public partial class NodeDisplay : UserControl
    {
        private const int TitleHeight = 45;
        private const int PortHeight = 50;
        private const int FooterHeight = 20;
        private readonly Color _backgroundColorBottom = Color.FromArgb(50, 136, 158);
        private readonly Color _backgroundColorTop = Color.FromArgb(10, 106, 108);
        private readonly Pen _border = new Pen(Color.Black, 2);
        private readonly Brush _footerBackgroundBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0));
        private readonly Brush _portShadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
        private readonly Color _titleColorBottom = Color.FromArgb(50, 136, 158);
        private readonly Color _titleColorTop = Color.FromArgb(50, 156, 158);
        private Size _defaultSize = new Size(300, 400);

        public string Id { get; set; }
        public string Title { get; set; }
        public string TypeName { get; set; }
        public List<Port> InputPorts { get; } = new List<Port>();
        public List<Port> OutputPorts { get; } = new List<Port>();


        private double _zoom;
        private int downX;
        private int downY;


        public NodeDisplay()
        {
            InitializeComponent();
            
        }

        public void Initialize()
        {
            Height = TitleHeight + FooterHeight + (Math.Max(InputPorts.Count, OutputPorts.Count) * PortHeight);
            Width = 350;
        }

        public Point DesignLocation { get; set; }


        public double Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                Width = (int) (_defaultSize.Width * _zoom);
                Height = (int) (_defaultSize.Height * _zoom);
                Refresh();
            }
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            downX = e.Location.X;
            downY = e.Location.Y;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = DefaultCursor;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var isOnTitleBar = e.Location.Y < TitleHeight;
            Cursor = isOnTitleBar ? Cursors.SizeAll : DefaultCursor;
            if (e.Button == MouseButtons.Left && isOnTitleBar)
            {
                var moveX = e.Location.X - downX;
                var moveY = e.Location.Y - downY;
                Left += moveX;
                Top += moveY;
                DesignLocation = new Point(DesignLocation.X + (moveX), DesignLocation.Y + moveY);
                this.Parent.Refresh();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            var rec = new Rectangle(1, 1, Width - 3, Height - 3);
            var back = new LinearGradientBrush(rec, _backgroundColorTop, _backgroundColorBottom, LinearGradientMode.Vertical);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.FillRectangle(back, rec);
            var rec2 = new Rectangle(1, 1, Width - 3, TitleHeight);
            var back2 = new LinearGradientBrush(rec2, _titleColorTop, _titleColorBottom, LinearGradientMode.Vertical);
            g.FillRectangle(back2, rec2);

            g.DrawLine(Pens.DarkSlateGray, 2, TitleHeight + 1, Width - 3, TitleHeight + 1);

            g.FillRectangle(_footerBackgroundBrush, 0, Height - FooterHeight - 1, Width, FooterHeight);
            g.DrawRectangle(_border, 0, 0, Width - 1, Height - 1);
            g.DrawString(Title, new Font("Arial", 14), Brushes.Black, new RectangleF(6, 6, Width - 13, TitleHeight - 6), new StringFormat {LineAlignment = StringAlignment.Center});
            g.DrawString(TypeName, new Font("Arial", 8), Brushes.Black, new RectangleF(2, Height - FooterHeight, Width - 5, FooterHeight), new StringFormat {LineAlignment = StringAlignment.Center});

            var circleSize = 22;
            var yy = TitleHeight;
            for (int i = 0; i < InputPorts.Count; i++)
            {
                var port = InputPorts[i];
                g.FillEllipse(_portShadowBrush, 18, yy - 2 + (int)((PortHeight / 2.0) - (circleSize / 2.0)), circleSize + 4, circleSize + 4);
                g.FillEllipse(new SolidBrush(this.Parent.BackColor), 20, yy + (int) ((PortHeight / 2.0) - (circleSize / 2.0)), circleSize, circleSize);
                
                g.FillRectangle(_portShadowBrush, 0, yy  + (int)((PortHeight / 2.0) - 6), 20,12);
                g.FillRectangle(new SolidBrush(this.Parent.BackColor), 0, yy  + (int)((PortHeight / 2.0) - 4), 22,8);

                var brConnection = port.IsData ? Brushes.PaleGreen : Brushes.White;
                if (port.IsConnected)
                {
                    g.FillRectangle(brConnection, -2, yy + 4 + (int)((PortHeight / 2.0) - 5), 30, 2);
                    
                }
                g.FillEllipse(brConnection, 24, yy + 4 + (int)((PortHeight / 2.0) - (circleSize / 2.0)), circleSize - 8, circleSize - 8);
                var layoutRectangle = new RectangleF(50+1,yy+1,((int)(Width / 2.0))- 50, PortHeight);

                var portTextColor = !port.IsConnected && port.IsMandatory ? Brushes.DarkRed : Brushes.Black;
                g.DrawString(port.Title, new Font("Arial",12),portTextColor,layoutRectangle,new StringFormat() {LineAlignment = StringAlignment.Center});

                port.PositionY = yy + 4 + (int) ((PortHeight / 2.0) - 5) + 1;

                yy += PortHeight;
            }

            yy = TitleHeight;
            for (int i = 0; i < OutputPorts.Count; i++)
            {
                var port = OutputPorts[i];
                g.FillEllipse(_portShadowBrush, Width - 1 - 18 - (circleSize + 4) , yy - 2 + (int)((PortHeight / 2.0) - (circleSize / 2.0)), circleSize + 4, circleSize + 4);
                g.FillEllipse(new SolidBrush(this.Parent.BackColor), Width - 1 - 20 - circleSize, yy + (int)((PortHeight / 2.0) - (circleSize / 2.0)), circleSize, circleSize);

                g.FillRectangle(_portShadowBrush, Width - 1 -20, yy + (int)((PortHeight / 2.0) - 6), 20, 12);
                g.FillRectangle(new SolidBrush(this.Parent.BackColor), Width - 1 - 22, yy + (int)((PortHeight / 2.0) - 4), 22, 8);

                if (port.IsConnected)
                {
                    g.FillRectangle(Brushes.White, Width - 1 - 30, yy + 4 + (int)((PortHeight / 2.0) - 5), 34, 2);
                }
                g.FillEllipse(Brushes.White, Width - 1 - 24 - (circleSize - 8), yy + 4 + (int)((PortHeight / 2.0) - (circleSize / 2.0)), circleSize - 8, circleSize - 8);
                var layoutRectangle = new RectangleF(Width - ((int)(Width / 2.0)) , yy+1, ((int)(Width / 2.0)) - 50, PortHeight);
                var portTextColor = !port.IsConnected && port.IsMandatory ? Brushes.DarkRed : Brushes.Black;
                g.DrawString(port.Title, new Font("Arial", 12), portTextColor, layoutRectangle, new StringFormat() { LineAlignment = StringAlignment.Center , Alignment = StringAlignment.Far});

                port.PositionY = yy + 4 + (int)((PortHeight / 2.0) - 5) + 1;

                yy += PortHeight;
            }


        }
    }
}