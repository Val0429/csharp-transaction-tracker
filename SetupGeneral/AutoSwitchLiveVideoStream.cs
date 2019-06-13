using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupGeneral
{
    public sealed partial class AutoSwitchLiveVideoStream : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        private UInt16 _minCount = 1;
        private UInt16 _maxCount = 64;

        public AutoSwitchLiveVideoStream()
        {
            Localization = new Dictionary<String, String>
                               {
                                    {"SetupGeneral_Enabled", "Enabled"},
                                    {"AutoSwitchLiveVideoStream_AutoSwitch", "Auto switch"},
                                    {"AutoSwitchLiveVideoStream_HighProfileStartLessThen", "High profile starts when streaming count less than"},
                                    {"AutoSwitchLiveVideoStream_MediumProfileStartMoreThen", "Medium profile starts when streaming count between"},
                                    {"AutoSwitchLiveVideoStream_LowProfileStartMoreThen", "Low profile starts when streaming count more than"},
                                    {"AutoSwitchLiveVideoStream_Information", "High profile streaming count must be less then low profile streaming count."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Name = "AutoSwitchLiveVideoStream";
            Dock = DockStyle.None;
            BackgroundImage = Manager.BackgroundNoBorder;

            enabledCheckBox.Text = Localization["SetupGeneral_Enabled"];
            enabledCheckBox.Click += EnabledCheckBoxClick;
        }

        public void Initialize()
        {
            autoSwitchPanel.Paint += InputPanelPaint;
            highProfilePanel.Paint += InputPanelPaint;
            mediumProfilePanel.Paint += InputPanelPaint;
            lowProfilePanel.Paint += InputPanelPaint;

            var count = new List<object>();
            for (int i = _minCount; i <= _maxCount; i++)
                count.Add((ushort)i);

            highProfileComboBox.Items.AddRange(count.ToArray());
            highProfileComboBox.KeyPress += KeyAccept.AcceptNumberOnly;

            lowProfileComboBox.Items.AddRange(count.ToArray());
            lowProfileComboBox.KeyPress += KeyAccept.AcceptNumberOnly;

            infoLabel.Text = Localization["AutoSwitchLiveVideoStream_Information"];
        }

        public void ParseSetting()
        {
            enabledCheckBox.Checked = Server.Configure.EnableAutoSwitchLiveStream;

            highProfileComboBox.SelectedIndexChanged -= SecurityComboBoxSelectedIndexChanged;
            highProfileComboBox.TextChanged -= CountTextChanged;

            highProfileComboBox.SelectedItem = Server.Configure.AutoSwitchLiveHighProfileCount;

            highProfileComboBox.SelectedIndexChanged += SecurityComboBoxSelectedIndexChanged;
            highProfileComboBox.TextChanged += CountTextChanged;

            lowProfileComboBox.SelectedIndexChanged -= SecurityComboBoxSelectedIndexChanged;
            lowProfileComboBox.TextChanged -= CountTextChanged;

            lowProfileComboBox.SelectedItem = Server.Configure.AutoSwitchLiveLowProfileCount;

            lowProfileComboBox.SelectedIndexChanged += SecurityComboBoxSelectedIndexChanged;
            lowProfileComboBox.TextChanged += CountTextChanged;

            ChangeMediumProfileLabel();
        }

        private void EnabledCheckBoxClick(object sender, EventArgs e)
        {
            Server.Configure.EnableAutoSwitchLiveStream = enabledCheckBox.Checked;
        }

        private void SecurityComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            var highCount = Convert.ToInt32(highProfileComboBox.SelectedItem);
            var lowCount = Convert.ToInt32(lowProfileComboBox.SelectedItem);

            if (highCount > lowCount)
            {
                var comboBox = sender as ComboBox;
                if(comboBox == highProfileComboBox)
                    Server.Configure.AutoSwitchLiveHighProfileCount = Server.Configure.AutoSwitchLiveLowProfileCount;

                if (comboBox == lowProfileComboBox)
                    Server.Configure.AutoSwitchLiveLowProfileCount = Server.Configure.AutoSwitchLiveHighProfileCount;

                ParseSetting();
                return;
            }

            Server.Configure.AutoSwitchLiveHighProfileCount = (ushort) highProfileComboBox.SelectedItem;
            Server.Configure.AutoSwitchLiveLowProfileCount = (ushort)lowProfileComboBox.SelectedItem;
            ChangeMediumProfileLabel();
        }

        private void CountTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(highProfileComboBox.Text)) return;
            if (String.IsNullOrEmpty(lowProfileComboBox.Text)) return;

            var highCount = Convert.ToInt32(highProfileComboBox.Text);
            var lowCount = Convert.ToInt32(lowProfileComboBox.Text);

            if(highCount > lowCount) return;

            Server.Configure.AutoSwitchLiveHighProfileCount = Convert.ToUInt16(Math.Min(Math.Max(highCount, _minCount), _maxCount));
            Server.Configure.AutoSwitchLiveLowProfileCount = Convert.ToUInt16(Math.Min(Math.Max(lowCount, _minCount), _maxCount));
            ChangeMediumProfileLabel();
        }

        private void ChangeMediumProfileLabel()
        {
            highLabel.Text = Server.Configure.AutoSwitchLiveHighProfileCount.ToString();
            lowLabel.Text = Server.Configure.AutoSwitchLiveLowProfileCount.ToString();
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("AutoSwitchLiveVideoStream_" + control.Tag))
                Manager.PaintText(g, Localization["AutoSwitchLiveVideoStream_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }
    }
}
