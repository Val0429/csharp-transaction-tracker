using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGeneral
{
    public sealed partial class CPULoadingControl : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public CPULoadingControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupGeneral_Custom", "Custom"},
                                   {"SetupGeneral_CPULoadingUpperBoundaryValue", "Application will start adjusting frame rate when CPU usage reaches the user defined threshold.  System allowed threshold range is %1% ~ %2%."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "CPULoadingUpperBoundary";
            BackgroundImage = Manager.BackgroundNoBorder;
            infoLabel.Text = Localization["SetupGeneral_CPULoadingUpperBoundaryValue"].Replace("%1", MiniumInterval.ToString()).Replace("%2", MaximumInterval.ToString());

            customPanel.Paint += CustomPanelPaint;

            boundaryTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
        }

        private const UInt16 MiniumInterval = 50;
        private const UInt16 MaximumInterval = 95;
        public void ParseSetting()
        {
            boundaryTextBox.TextChanged -= CPUTextBoxTextChanged;

            boundaryTextBox.Text = Server.Configure.CPULoadingUpperBoundary.ToString();
            
            boundaryTextBox.TextChanged += CPUTextBoxTextChanged;
        }

        private readonly UInt16[] _boundaryArray = new UInt16[] {50, 60, 70, 80, 90, 95};
        public void Initialize()
        {
            foreach (UInt16 boundary in _boundaryArray.Reverse())
            {
                BoundaryPanel boundaryPanel = new BoundaryPanel
                {
                    Server = Server,
                    Tag = boundary.ToString(),
                };

                boundaryPanel.MouseClick += CPUPanelMouseClick;

                containerPanel.Controls.Add(boundaryPanel);
            }
        }

        private void CPUPanelMouseClick(Object sender, MouseEventArgs e)
        {
            Server.Configure.CPULoadingUpperBoundary = Convert.ToUInt16(((Control)sender).Tag);

            boundaryTextBox.TextChanged -= CPUTextBoxTextChanged;

            boundaryTextBox.Text = Server.Configure.CPULoadingUpperBoundary.ToString();

            boundaryTextBox.TextChanged += CPUTextBoxTextChanged;

            ((Control)sender).Focus();
            Invalidate();
        }

        private void CustomPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (!_boundaryArray.Contains(Server.Configure.CPULoadingUpperBoundary))
            {
                Manager.PaintText(g, Localization["SetupGeneral_Custom"], Manager.SelectedTextColor);

                Manager.PaintTextRight(g, customPanel, "%", Manager.SelectedTextColor);
                    
                Manager.PaintSelected(g);
            }
            else
            {
                Manager.PaintText(g, Localization["SetupGeneral_Custom"]);

                Manager.PaintTextRight(g, customPanel, "%");
            }
        }

        private void CPUTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 boundary = (boundaryTextBox.Text != "") ? Convert.ToUInt32(boundaryTextBox.Text) : 0;

            Server.Configure.CPULoadingUpperBoundary = Convert.ToUInt16(Math.Min(Math.Max(boundary, MiniumInterval), MaximumInterval));

            Invalidate();
        }
    }

    public sealed class BoundaryPanel : Panel
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public BoundaryPanel()
        {
            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += BoundaryPanelPaint;
        }

        private String _boundary;
        private void BoundaryPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            if(_boundary == null)
            {
                UInt16 boundary = Convert.ToUInt16(Tag);
                _boundary = String.Format("{0}%", boundary); 
            }

            Graphics g = e.Graphics;

            if (Parent.Controls[0] == this)
            {
                Manager.PaintBottom(g, this);
            }
            else
            {
                Manager.PaintMiddle(g, this);
            }

            if (Width <= 100) return;
            if (Tag.ToString() == Server.Configure.CPULoadingUpperBoundary.ToString())
            {
                Manager.PaintText(g, _boundary, Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, _boundary);
        }
    }
}
