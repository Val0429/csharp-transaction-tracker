using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public sealed partial class PIPControl : UserControl
    {
        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;

        private Dictionary<UInt16, String> _streams = new Dictionary<UInt16, String>();

        public PIPControl()
        {
            Localization = new Dictionary<String, String>
            {
                {"DevicePanel_PIP", "PIP"},
                {"DevicePanel_Device", "Device"},
                {"DevicePanel_StreamId", "Stream"},
                {"DevicePanel_Position", "Position"},
                {"DevicePanel_PositionRightTop", "Right Top"},
                {"DevicePanel_PositionRightBottom", "Right Bottom"},
                {"DevicePanel_PositionLeftTop", "Left Top"},
                {"DevicePanel_PositionLeftBottom", "Left Bottom"},
                {"DevicePanel_VideoStreamID", "Video Stream %1"},
                {"SetupGeneral_Disabled", "Disabled"},
            };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += MultiStreamingControlPaint;
            devicePanel.Paint += PaintInput;
            streamPanel.Paint += PaintInput;
            positionPanel.Paint += PaintInput;

            positionComboBox.Items.Add(Localization["DevicePanel_PositionRightTop"]);
            positionComboBox.Items.Add(Localization["DevicePanel_PositionRightBottom"]);
            positionComboBox.Items.Add(Localization["DevicePanel_PositionLeftTop"]);
            positionComboBox.Items.Add(Localization["DevicePanel_PositionLeftBottom"]);

            deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            streamComboBox.SelectedIndexChanged += StreamComboBoxSelectedIndexChanged;
            positionComboBox.SelectedIndexChanged += PositionComboBoxSelectedIndexChanged;
        }

        private void MultiStreamingControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_PIP"], Manager.Font, Brushes.DimGray, 8, 10);
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

        private void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.PIPDevice = deviceComboBox.SelectedItem as ICamera;
            streamComboBox.Enabled = positionComboBox.Enabled = true;
            positionComboBox.SelectedIndex = 0;
            ParserStreamConfig();
            EditPanel.CameraIsModify();
        }

        private void StreamComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.PIPStreamId = ReadStreamIdByValue(streamComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void PositionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.PIPPosition = Positions.ToIndex(positionComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
        }

        private void ParserStreamConfig()
        {
            streamComboBox.SelectedIndexChanged -= StreamComboBoxSelectedIndexChanged;
            streamComboBox.Items.Clear();
            _streams.Clear();

            if (EditPanel.Camera.PIPDevice == null) return;

            foreach (KeyValuePair<UInt16, StreamConfig> streamConfig in EditPanel.Camera.PIPDevice.Profile.StreamConfigs)
                _streams.Add(streamConfig.Key, Localization["DevicePanel_VideoStreamID"].Replace("%1", streamConfig.Key.ToString()));

            for (int i = 0; i <= _streams.Count; i++)//because stream configs are not in sequency.
            {
                var id = (UInt16)i;
                if (_streams.ContainsKey(id))
                {
                    streamComboBox.Items.Add(_streams[id]);
                }
            }
            streamComboBox.SelectedIndexChanged += StreamComboBoxSelectedIndexChanged;
            streamComboBox.SelectedIndex = 0;
        }

        public void ParseDevice()
        {
            deviceComboBox.Items.Clear();
            streamComboBox.Items.Clear();
            _streams.Clear();

            deviceComboBox.Items.Add(Localization["SetupGeneral_Disabled"]);
            foreach (KeyValuePair<ushort, IDevice> device in EditPanel.Server.Device.Devices)
            {
                if(device.Value.ReadyState == ReadyState.Delete) continue;
                if (device.Value == EditPanel.Camera) continue;
                deviceComboBox.Items.Add(device.Value);
            }

            if (deviceComboBox.Items.Count > 0 && EditPanel.Camera.PIPDevice != null && EditPanel.Camera.PIPDevice.ReadyState != ReadyState.Delete)
            {
                deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;
                deviceComboBox.SelectedItem = EditPanel.Camera.PIPDevice;
                deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;

                foreach (KeyValuePair<UInt16, StreamConfig> streamConfig in EditPanel.Camera.PIPDevice.Profile.StreamConfigs)
                    _streams.Add(streamConfig.Key, Localization["DevicePanel_VideoStreamID"].Replace("%1", streamConfig.Key.ToString()));

                for (int i = 0; i <= _streams.Count; i++)//because stream configs are not in sequency.
                {
                    var id = (UInt16)i;
                    if (_streams.ContainsKey(id))
                    {
                        streamComboBox.Items.Add(_streams[id]);
                    }
                }

                streamComboBox.SelectedIndexChanged -= StreamComboBoxSelectedIndexChanged;
                var defaultStream = Localization["DevicePanel_VideoStreamID"].Replace("%1", "1");
                streamComboBox.SelectedItem = _streams.ContainsKey(EditPanel.Camera.PIPStreamId) ? Localization["DevicePanel_VideoStreamID"].Replace("%1", EditPanel.Camera.PIPStreamId.ToString()) : defaultStream;
                streamComboBox.SelectedIndexChanged += StreamComboBoxSelectedIndexChanged;

                positionComboBox.SelectedIndexChanged -= PositionComboBoxSelectedIndexChanged;
                positionComboBox.SelectedItem = Positions.ToString(EditPanel.Camera.PIPPosition);
                positionComboBox.SelectedIndexChanged += PositionComboBoxSelectedIndexChanged;

                streamComboBox.Enabled = positionComboBox.Enabled = true;
            }
            else
            {
                deviceComboBox.SelectedIndex = 0;
                streamComboBox.Enabled = positionComboBox.Enabled = false;
            }

            
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

    }
}
