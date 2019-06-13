using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public sealed partial class MultiStreamingControl : UserControl
    {
        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;

        private Dictionary<UInt16, String> _streams = new Dictionary<UInt16, String>();

        public MultiStreamingControl()
        {
            Localization = new Dictionary<String, String>
            {
                {"DevicePanel_MultiStreaming", "Multi-Streaming"},
                {"DevicePanel_HighProfile", "High Profile"},
                {"DevicePanel_MediumProfile", "Medium Profile"},
                {"DevicePanel_LowProfile", "Low Profile"},
                {"DevicePanel_VideoStreamID", "Video Stream %1"},
            };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += MultiStreamingControlPaint;
            highProfilePanel.Paint += PaintInput;
            mediumProfilePanel.Paint += PaintInput;
            lowProfilePanel.Paint += PaintInput;

            highProfileComboBox.SelectedIndexChanged += HighProfileComboBoxSelectedIndexChanged;
            mediumProfileComboBox.SelectedIndexChanged += MediumProfileComboBoxSelectedIndexChanged;
            lowProfileComboBox.SelectedIndexChanged += LowProfileComboBoxSelectedIndexChanged;
        }

        private void MultiStreamingControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_MultiStreaming"], Manager.Font, Brushes.DimGray, 8, 10);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            var control = (Control)sender;

            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, control);

            if (Localization.ContainsKey("DevicePanel_" + control.Tag))
                Manager.PaintText(g, Localization["DevicePanel_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private void HighProfileComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.HighProfile = ReadStreamIdByValue(highProfileComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void MediumProfileComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.MediumProfile = ReadStreamIdByValue(mediumProfileComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void LowProfileComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.LowProfile = ReadStreamIdByValue(lowProfileComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        public void ParseDevice()
        {
            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "iSapSolution":
                    Visible = (EditPanel.Camera.Model.Type != "SmartPatrolService");
                    break;

                case "Certis":
                    Visible = (EditPanel.Camera.Model.Type != "Badge");
                    break;

                case "Kedacom":
                    Visible =false;
                    break;

                default:
                    Visible = true;
                    break;
            }

            highProfileComboBox.Items.Clear();
            mediumProfileComboBox.Items.Clear();
            lowProfileComboBox.Items.Clear();
            _streams.Clear();
            foreach (KeyValuePair<UInt16, StreamConfig> streamConfig in EditPanel.Camera.Profile.StreamConfigs)
                _streams.Add(streamConfig.Key, Localization["DevicePanel_VideoStreamID"].Replace("%1", streamConfig.Key.ToString()));

            for (int i = 0; i <= _streams.Count; i++)//because stream configs are not in sequency.
            {
                var id = (UInt16)i;
                if (_streams.ContainsKey(id))
                {
                    highProfileComboBox.Items.Add(_streams[id]);
                    mediumProfileComboBox.Items.Add(_streams[id]);
                    lowProfileComboBox.Items.Add(_streams[id]);
                }   
            }

            var defaultStream = Localization["DevicePanel_VideoStreamID"].Replace("%1", "1");
            highProfileComboBox.SelectedItem = _streams.ContainsKey(EditPanel.Camera.Profile.HighProfile) ? Localization["DevicePanel_VideoStreamID"].Replace("%1", EditPanel.Camera.Profile.HighProfile.ToString()) : defaultStream;
            mediumProfileComboBox.SelectedItem = _streams.ContainsKey(EditPanel.Camera.Profile.MediumProfile) ? Localization["DevicePanel_VideoStreamID"].Replace("%1", EditPanel.Camera.Profile.MediumProfile.ToString()) : defaultStream;
            lowProfileComboBox.SelectedItem = _streams.ContainsKey(EditPanel.Camera.Profile.LowProfile) ? Localization["DevicePanel_VideoStreamID"].Replace("%1", EditPanel.Camera.Profile.LowProfile.ToString()) : defaultStream;
        }

        private UInt16 ReadStreamIdByValue(String streamString)
        {
            foreach (KeyValuePair<ushort, string> stream in _streams)
            {
                if (String.Equals(stream.Value, streamString))
                    return stream.Key;
            }

            return 1;
        }

        //------------------------------------------------------------------------------------

        public void UpdateStreamConfigList()
        {
            ParseDevice();

            Invalidate();
        }

    }
}
