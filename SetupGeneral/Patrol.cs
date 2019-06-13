using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupGeneral
{
    public sealed partial class PatrolControl : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public PatrolControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "Sec"},

                                   {"SetupGeneral_Custom", "Custom"},
                                   {"SetupGeneral_PatrolIntervaValue", "Patrol interval should between %1 secs to %2 hour"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "LivePatrolInterval";
            BackgroundImage = Manager.BackgroundNoBorder;
            infoLabel.Text = Localization["SetupGeneral_PatrolIntervaValue"].Replace("%1", MiniumInterval.ToString()).Replace("%2", (MaximumInterval / 3600).ToString());
            
            customPanel.Paint += PatrolInputPanelPaint;

            intervalTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
        }

        private const UInt16 MiniumInterval = 5;
        private const UInt16 MaximumInterval = 3600;
        public void ParseSetting()
        {
            intervalTextBox.TextChanged -= IntervalTextBoxTextChanged;

            intervalTextBox.Text = Server.Configure.PatrolInterval.ToString();
            
            intervalTextBox.TextChanged += IntervalTextBoxTextChanged;
        }

        private readonly UInt16[] _intervalArray = new UInt16[] { /*5, 10, */15, 30, 45, 60, 120, 180, 300, 600/*, 1800, 3600 */};
        public void Initialize()
        {
            foreach (UInt16 interval in _intervalArray.Reverse())
            {
                IntervalPanel intervalPanel = new IntervalPanel
                {
                    Server = Server,
                    Tag = interval.ToString(),
                };

                intervalPanel.MouseClick += IntervalPanelMouseClick;

                containerPanel.Controls.Add(intervalPanel);
            }
        }

        private void IntervalPanelMouseClick(Object sender, MouseEventArgs e)
        {
            Server.Configure.PatrolInterval = Convert.ToUInt16(((Control)sender).Tag);

            intervalTextBox.TextChanged -= IntervalTextBoxTextChanged;

            intervalTextBox.Text = Server.Configure.PatrolInterval.ToString();

            intervalTextBox.TextChanged += IntervalTextBoxTextChanged;

            ((Control)sender).Focus();
            Invalidate();
        }

        private void PatrolInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (!_intervalArray.Contains(Server.Configure.PatrolInterval))
            {
                Manager.PaintText(g, Localization["SetupGeneral_Custom"], Manager.SelectedTextColor);

                Manager.PaintTextRight(g, customPanel, Localization["Common_Sec"], Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
            {
                Manager.PaintText(g, Localization["SetupGeneral_Custom"]);

                Manager.PaintTextRight(g, customPanel, Localization["Common_Sec"]);
            }
        }

        private void IntervalTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 interval = (intervalTextBox.Text != "") ? Convert.ToUInt32(intervalTextBox.Text) : 0;

            Server.Configure.PatrolInterval = Convert.ToUInt16(Math.Min(Math.Max(interval, MiniumInterval), MaximumInterval));

            Invalidate();
        }
    }

    public sealed class IntervalPanel : Panel
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public IntervalPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "Sec"},
                                   {"Common_Min", "Min"},
                                   {"Common_Hr", "Hr"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += IntervalPanelPaint;
        }

        private String _interval;
        private void IntervalPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            if(_interval == null)
            {
                UInt16 interval = Convert.ToUInt16(Tag);
                if (interval < 60)
                    _interval = interval + " " + Localization["Common_Sec"];
                else if (interval < 3600)
                    _interval = (interval / 60) + Localization["Common_Min"];
                else
                    _interval = (interval / 3600) + " " + Localization["Common_Hr"];
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
            if (Tag.ToString() == Server.Configure.PatrolInterval.ToString())
            {
                Manager.PaintText(g, _interval, Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, _interval);
        }
    }
}
