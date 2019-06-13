using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGeneral
{
    public sealed partial class AutoSwitchDecodeIFrame : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        private UInt16 _minCount = 1;
        private UInt16 _maxCount = 64;

        public AutoSwitchDecodeIFrame()
        {
            Localization = new Dictionary<String, String>
                               {
                                    {"SetupGeneral_Enabled", "Enabled"},
                                    {"AutoSwitchDecodeIFrame_AutoSwitch", "Auto switch"},
                                    {"AutoSwitchDecodeIFrame_SimultaneousViewingLiveStreamingCountMoreThan", "Simultaneous viewing live streaming count more than"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Name = "AutoSwitchDecodeIFrame";
            Dock = DockStyle.None;
            BackgroundImage = Manager.BackgroundNoBorder;

            enabledCheckBox.Text = Localization["SetupGeneral_Enabled"];
            enabledCheckBox.Click += EnabledCheckBoxClick;
        }

        public void Initialize()
        {
            autoSwitchPanel.Paint += InputPanelPaint;
            countPanel.Paint += InputPanelPaint;

            var count = new List<object>();
            for (int i = _minCount; i <= _maxCount; i++)
                count.Add((ushort)i);
            countComboBox.Items.AddRange(count.ToArray());
            countComboBox.KeyPress += KeyAccept.AcceptNumberOnly;
        }

        public void ParseSetting()
        {
            enabledCheckBox.Checked = Server.Configure.EnableAutoSwitchDecodeIFrame;

            countComboBox.SelectedIndexChanged -= SecurityComboBoxSelectedIndexChanged;
            countComboBox.TextChanged -= CountTextChanged;

            countComboBox.SelectedItem = Server.Configure.AutoSwitchDecodeIFrameCount;

            countComboBox.SelectedIndexChanged += SecurityComboBoxSelectedIndexChanged;
            countComboBox.TextChanged += CountTextChanged;
        }

        private void EnabledCheckBoxClick(object sender, EventArgs e)
        {
            Server.Configure.EnableAutoSwitchDecodeIFrame = enabledCheckBox.Checked;
        }

        private void SecurityComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            Server.Configure.AutoSwitchDecodeIFrameCount = (ushort)countComboBox.SelectedItem;
        }

        private void CountTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(countComboBox.Text)) return;
            var count = Convert.ToInt32(countComboBox.Text);
            Server.Configure.AutoSwitchDecodeIFrameCount = Convert.ToUInt16(Math.Min(Math.Max(count, _minCount), _maxCount));
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("AutoSwitchDecodeIFrame_" + control.Tag))
                Manager.PaintText(g, Localization["AutoSwitchDecodeIFrame_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }
    }
}
