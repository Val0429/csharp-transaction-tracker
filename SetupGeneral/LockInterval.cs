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
    public sealed partial class LockIntervalControl : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public LockIntervalControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "Sec"},

                                   {"SetupGeneral_Custom", "Custom"},
                                   {"SetupGeneral_LockIntervalValue", "Lock Interval should between %1 secs to %2 hour"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "AutoLockApplicationTimer";
            BackgroundImage = Manager.BackgroundNoBorder;
            customPanel.Paint += DurationInputPanelPaint;
            infoLabel.Text = Localization["SetupGeneral_LockIntervalValue"].Replace("%1", MiniumDuration.ToString()).Replace("%2", (MaximumDuration / 3600).ToString());

            durationTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
        }

        private const UInt16 MiniumDuration = 30;
        private const UInt16 MaximumDuration = 3600;
        public void ParseSetting()
        {
            durationTextBox.TextChanged -= DurationTextBoxTextChanged;

            durationTextBox.Text = Server.Configure.AutoLockApplicationTimer.ToString();
            
            durationTextBox.TextChanged += DurationTextBoxTextChanged;
        }

        private readonly UInt16[] _durationArray = new UInt16[] { 0, 30, 60, 120, 180, 300, 600, 900, 1800, 3600};
        public void Initialize()
        {
            foreach (UInt16 duration in _durationArray.Reverse())
            {
                LockIntervalPanel intervalPanel = new LockIntervalPanel
                {
                    Server = Server,
                    Tag = duration.ToString(),
                };
                intervalPanel.MouseClick += LockIntervalPanelMouseClick;

                containerPanel.Controls.Add(intervalPanel);
            }
        }

        private void LockIntervalPanelMouseClick(Object sender, MouseEventArgs e)
        {
            Server.Configure.AutoLockApplicationTimer = Convert.ToUInt16(((Control)sender).Tag);

            durationTextBox.TextChanged -= DurationTextBoxTextChanged;

            durationTextBox.Text = Server.Configure.AutoLockApplicationTimer.ToString();

            durationTextBox.TextChanged += DurationTextBoxTextChanged;

            ((Control)sender).Focus();
            Invalidate();
        }

        private void DurationInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (!_durationArray.Contains(Server.Configure.AutoLockApplicationTimer))
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

        private void DurationTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 duration = (durationTextBox.Text != "") ? Convert.ToUInt32(durationTextBox.Text) : 0;

            if(duration == 0)
            {
                Server.Configure.AutoLockApplicationTimer = 0;
            }
            else
            {
                Server.Configure.AutoLockApplicationTimer = Convert.ToUInt16(Math.Min(Math.Max(duration, MiniumDuration), MaximumDuration));
            }

            Invalidate();
        }
    }

    public sealed class LockIntervalPanel : Panel
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public LockIntervalPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "Sec"},
                                   {"Common_Min", "Min"},
                                   {"Common_Hr", "Hr"},
                                   {"SetupGeneral_Disabled", "Disabled"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += LockIntervalPanelPaint;
        }

        private String _duration;
        private void LockIntervalPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            if (_duration == null)
            {
                UInt16 duration = Convert.ToUInt16(Tag);
                if (duration == 0)
                    _duration = Localization["SetupGeneral_Disabled"];
                else if (duration < 60)
                    _duration = duration + " " + Localization["Common_Sec"];
                else if (duration < 3600)
                    _duration = (duration / 60) + Localization["Common_Min"];
                else
                    _duration = (duration / 3600) + " " + Localization["Common_Hr"];
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
            if (Tag.ToString() == Server.Configure.AutoLockApplicationTimer.ToString())
            {
                Manager.PaintText(g, _duration, Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, _duration);
        }
    }
}
