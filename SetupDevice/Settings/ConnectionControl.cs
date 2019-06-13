using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceCab;
using DeviceConstant;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public sealed partial class ConnectionControl : UserControl
    {
        public event EventHandler OnModeChange;
        public event EventHandler OnTvStandardChange;
        public event EventHandler OnSensorModeChange;
        public event EventHandler OnPowerFrequencyChange;
        public event EventHandler OnDeviceMountTypeChange;
        public event EventHandler OnAspectRatioChange;
        public event EventHandler OnAspectRatioCorrectionChange;
        public event EventHandler OnRecordStreamChange;

        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;

        private String _streamId1Str;
        private String _streamId2Str;
        private String _streamId3Str;
        private String _streamId4Str;
        private String _streamId5Str;
        private String _streamId6Str;

        private List<String> _lensType = new List<String> { "A0**V", "A8TRT", "A1UST" };

        public ConnectionControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePanel_VideoStreamID", "Video Stream %1"},
                                   {"DevicePanel_Connection", "Connection"},
                                   {"DevicePanel_NetworkAddress", "Network Address"},
                                   {"DevicePanel_HTTPPort", "HTTP Port"},
                                   {"DevicePanel_AudioOutPort", "Audio Out Port"},
                                   {"DevicePanel_Mode", "Mode"},
                                   {"DevicePanel_LiveStream", "Live Stream"},
                                   {"DevicePanel_RecordingStream", "Recording Stream"},
                                   {"DevicePanel_TVStandard", "TV Standard"},
                                   {"DevicePanel_SensorMode", "Sensor Mode"},
                                   {"DevicePanel_PowerFrequency", "Power Frequency"},
                                   {"DevicePanel_AspectRatioCorrection", "Aspect Ratio Correction"},
                                   {"DevicePanel_AspectRatio", "Aspect Ratio"},
                                   {"DevicePanel_URI", "URI"},
                                   {"DevicePanel_Enabled", "Enabled"},
                                   {"DevicePanel_DewarpType", "Dewarp Type"},
                                   {"DevicePanel_RemoteRecovery", "Remote Recovery"},
                                   {"DevicePanel_DeviceMountType", "Mount Type"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += ConnectionControlPaint;

            networkAddressPanel.Paint += PaintInput;
            httpPanel.Paint += PaintInput;
            audioPanel.Paint += PaintInput;
            modePanel.Paint += PaintInput;
            liveProfilePanel.Paint += PaintInput;
            recordProfilePanel.Paint += PaintInput;
            tvStandardPanel.Paint += PaintInput;
            sensorModePanel.Paint += PaintInput;
            powerFrequencyPanel.Paint += PaintInput;
            mountTypePanel.Paint += PaintInput;
            aspectRatioPanel.Paint += PaintInput;
            aspectRatioCorrectionPanel.Paint += PaintInput;
            dewarpTypePanel.Paint += PaintInput;
            uriPanel.Paint += PaintInput;
            remoteRecoveryPanel.Paint += PaintInput;

            aspectRatioCorrectionCheckBox.Text = Localization["DevicePanel_Enabled"];
            RemoteRecoveryCheckBox.Text = Localization["DevicePanel_Enabled"];

            httpPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            audioPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            var dewarpType = new List<String> { "A0**V", "A8TRT", "A1UST" };
            dewarpTypeComboBox.Items.Add("Off");
            foreach (var type in dewarpType)
            {
                dewarpTypeComboBox.Items.Add(type);
            }

            modeComboBox.SelectedIndexChanged += ModeComboBoxSelectedIndexChanged;
            streamComboBox.SelectedIndexChanged += StreamComboBoxSelectedIndexChanged;
            recordStreamComboBox.SelectedIndexChanged += RecordStreamComboBoxSelectedIndexChanged;
            tvStandardComboBox.SelectedIndexChanged += TvStandardComboBoxSelectedIndexChanged;
            sensorModeComboBox.SelectedIndexChanged += SensorModeComboBoxSelectedIndexChanged;
            powerFrequencyComboBox.SelectedIndexChanged += PowerFrequencyComboBoxSelectedIndexChanged;
            mountTypeComboBox.SelectedIndexChanged += MountTypeComboBoxSelectedIndexChanged;
            aspectRatioComboBox.SelectedIndexChanged += AspectRatioComboBoxSelectedIndexChanged;
            dewarpTypeComboBox.SelectedIndexChanged += DewarpTypeComboBoxSelectedIndexChanged;

            ipAddressTextBox.TextChanged += IpAddressTextBoxTextChanged;
            httpPortTextBox.TextChanged += HttpPortTextBoxTextChanged;
            audioPortTextBox.TextChanged += AudioPortTextBoxTextChanged;
            aspectRatioCorrectionCheckBox.CheckStateChanged += AspectRatioCorrectionCheckBoxCheckStateChanged;
            RemoteRecoveryCheckBox.CheckStateChanged += RemoteRecoveryCheckBoxCheckStateChanged;
            uriTextBox.TextChanged += URITextBoxTextChanged;

            _streamId1Str = Localization["DevicePanel_VideoStreamID"].Replace("%1", "1");
            _streamId2Str = Localization["DevicePanel_VideoStreamID"].Replace("%1", "2");
            _streamId3Str = Localization["DevicePanel_VideoStreamID"].Replace("%1", "3");
            _streamId4Str = Localization["DevicePanel_VideoStreamID"].Replace("%1", "4");
            _streamId5Str = Localization["DevicePanel_VideoStreamID"].Replace("%1", "5");
            _streamId6Str = Localization["DevicePanel_VideoStreamID"].Replace("%1", "6");
        }

        private void ConnectionControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_Connection"], Manager.Font, Brushes.DimGray, 8, 10);
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

        private void ModeComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing)
            {
                UpdateStreamConfigList();
                return;
            }

            EditPanel.Camera.Mode = CameraModes.DisplayStringToIndex(modeComboBox.SelectedItem.ToString());

            if (EditPanel.Camera.Profile.StreamConfigs.ContainsKey(1))
            {
                switch (EditPanel.Camera.Model.Manufacture)
                {
                    case "ACTi":
                        ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        break;

                    case "MegaSys":
                    case "Avigilon":
                    case "DivioTec":
                    case "SIEMENS":
                    case "SAMSUNG":
                    case "inskytec":
                    case "HIKVISION":
                    case "PULSE":
                    case "ZeroOne":
                        ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        break;

                    case "Brickcom":
                    case "VIGZUL":
                    case "Surveon":
                    case "Certis":
                    case "FINE":
                        if (EditPanel.Camera.Model.Series == "DynaColor")
                        {
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        }
                        break;

                    case "DLink":
                        if (EditPanel.Camera.Model.Type == "DynaColor")
                        {
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[1], 1);
                        }
                        break;
                }
            }

            switch (EditPanel.Camera.Mode)
            {
                case CameraMode.Single:
                    EditPanel.Camera.Profile.StreamConfigs.Remove(2);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(3);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(4);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(5);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(6);
                    break;

                case CameraMode.Dual:
                    EditPanel.AddStreamConfigFromDefault(2);

                    EditPanel.Camera.Profile.StreamConfigs.Remove(3);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(4);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(5);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(6);
                    break;

                case CameraMode.Triple:
                    EditPanel.AddStreamConfigFromDefault(2);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "MegaSys":
                        case "Avigilon":
                        case "DivioTec":
                        case "SIEMENS":
                        case "SAMSUNG":
                        case "inskytec":
                        case "HIKVISION":
                        case "PULSE":
                        case "ZeroOne":
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            break;

                        case "Brickcom":
                        case "VIGZUL":
                        case "Surveon":
                        case "Certis":
                        case "FINE":
                            if (EditPanel.Camera.Model.Series == "DynaColor")
                            {
                                ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            }
                            break;

                        case "DLink":
                            if (EditPanel.Camera.Model.Type == "DynaColor")
                            {
                                ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            }
                            break;
                    }
                    EditPanel.AddStreamConfigFromDefault(3);

                    EditPanel.Camera.Profile.StreamConfigs.Remove(4);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(5);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(6);
                    break;

                case CameraMode.Quad:
                    EditPanel.AddStreamConfigFromDefault(2);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "Axis":

                            break;
                    }
                    EditPanel.Camera.Profile.StreamConfigs.Remove(3);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(4);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(5);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(6);
                    break;

                case CameraMode.Multi:
                case CameraMode.FourVga:
                    EditPanel.AddStreamConfigFromDefault(2);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "MegaSys":
                        case "Avigilon":
                        case "DivioTec":
                        case "SIEMENS":
                        case "SAMSUNG":
                        case "inskytec":
                        case "HIKVISION":
                        case "PULSE":
                        case "ZeroOne":
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            break;

                        case "Brickcom":
                        case "VIGZUL":
                        case "Surveon":
                        case "Certis":
                        case "FINE":
                            if (EditPanel.Camera.Model.Series == "DynaColor")
                            {
                                ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            }
                            break;

                        case "DLink":
                            if (EditPanel.Camera.Model.Type == "DynaColor")
                            {
                                ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                                ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            }
                            break;
                    }
                    EditPanel.AddStreamConfigFromDefault(3);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "MegaSys":
                        case "Avigilon":
                        case "DivioTec":
                        case "SIEMENS":
                        case "SAMSUNG":
                        case "inskytec":
                        case "HIKVISION":
                        case "PULSE":
                        case "ZeroOne":
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            break;

                        case "Brickcom":
                        case "VIGZUL":
                        case "Surveon":
                        case "Certis":
                        case "FINE":
                            if (EditPanel.Camera.Model.Series == "DynaColor")
                            {
                                ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                                ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                                ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                                ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            }
                            break;

                        case "DLink":
                            if (EditPanel.Camera.Model.Type == "DynaColor")
                            {
                                ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                                ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                                ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                                ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            }
                            break;
                    }
                    EditPanel.AddStreamConfigFromDefault(4);
                    
                    EditPanel.Camera.Profile.StreamConfigs.Remove(5);
                    EditPanel.Camera.Profile.StreamConfigs.Remove(6);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "ACTi":
                            EditPanel.Camera.Profile.RecordStreamId = EditPanel.Camera.Profile.StreamId;
                            break;
                    }
                    break;

                case CameraMode.Five:
                    EditPanel.AddStreamConfigFromDefault(2);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "HIKVISION":
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[2], 2);
                            break;
                    }
                    EditPanel.AddStreamConfigFromDefault(3);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "HIKVISION":
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[3], 3);
                            break;
                    }
                    EditPanel.AddStreamConfigFromDefault(4);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "HIKVISION":
                            ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[4], 4);
                            ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[4], 4);
                            ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[4], 4);
                            ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, EditPanel.Camera.Profile.StreamConfigs[4], 4);
                            break;
                    }
                    EditPanel.AddStreamConfigFromDefault(5);
                    break;

                case CameraMode.SixVga:
                    EditPanel.AddStreamConfigFromDefault(2);
                    EditPanel.AddStreamConfigFromDefault(3);
                    EditPanel.AddStreamConfigFromDefault(4);
                    EditPanel.AddStreamConfigFromDefault(5);
                    EditPanel.AddStreamConfigFromDefault(6);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "ACTi":
                            EditPanel.Camera.Profile.RecordStreamId = EditPanel.Camera.Profile.StreamId;
                            break;
                    }
                    break;
            }

            if (OnModeChange != null)
                OnModeChange(this, null);

            UpdateStreamConfigList();

            EditPanel.CameraIsModify();
        }

        private void IpAddressTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.NetworkAddress = ipAddressTextBox.Text;

            EditPanel.CameraIsModify();
        }

        private void StreamComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;
            String streamId = streamComboBox.SelectedItem.ToString();

            UInt16 index = 1;
            if(streamId == _streamId1Str)
                index = 1;
            else if (streamId == _streamId2Str)
                index = 2;
            else if (streamId == _streamId3Str)
                index = 3;
            else if (streamId == _streamId4Str)
                index = 4;
            else if (streamId == _streamId5Str)
                index = 5;
            else if (streamId == _streamId6Str)
                index = 6;

            EditPanel.Camera.Profile.StreamId = index;

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "ACTi":
                    if (EditPanel.Camera.Mode == CameraMode.FourVga || EditPanel.Camera.Mode == CameraMode.SixVga)
                    {
                        EditPanel.Camera.Profile.RecordStreamId = index;
                        recordStreamComboBox.SelectedItem = streamId;
                        if (OnRecordStreamChange != null)
                            OnRecordStreamChange(this, null);
                    }
                    break;
            }

            EditPanel.CameraIsModify();
        }

        private void RecordStreamComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;
            String streamId = recordStreamComboBox.SelectedItem.ToString();

            UInt16 index = 1;
            if (streamId == _streamId1Str)
                index = 1;
            else if (streamId == _streamId2Str)
                index = 2;
            else if (streamId == _streamId3Str)
                index = 3;
            else if (streamId == _streamId4Str)
                index = 4;
            else if (streamId == _streamId5Str)
                index = 5;
            else if (streamId == _streamId6Str)
                index = 6;

            EditPanel.Camera.Profile.RecordStreamId = index;

            EditPanel.CameraIsModify();

            if (OnRecordStreamChange != null)
                OnRecordStreamChange(this, null);
        }

        private void MountTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.DeviceMountType = DeviceMountTypes.ToIndex(mountTypeComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();

            if (OnDeviceMountTypeChange != null)
                OnDeviceMountTypeChange(this, null);
        }

        private void PowerFrequencyComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.PowerFrequency = PowerFrequencies.DisplayStringToIndex(powerFrequencyComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();

            if (OnPowerFrequencyChange != null)
                OnPowerFrequencyChange(this, null);
        }

        private void AspectRatioComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.AspectRatio = AspectRatios.ToIndex(aspectRatioComboBox.SelectedItem.ToString());

            EditPanel.CameraIsModify();
            
            if (OnAspectRatioChange != null)
                OnAspectRatioChange(this, null);
        }

        private void DewarpTypeComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            if (dewarpTypeComboBox.SelectedIndex == 0)
                EditPanel.Camera.Profile.DewarpType = "";
            else
                EditPanel.Camera.Profile.DewarpType = dewarpTypeComboBox.SelectedItem.ToString();

            EditPanel.CameraIsModify();
        }
        
        private void TvStandardComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.TvStandard = TvStandards.ToIndex(tvStandardComboBox.SelectedItem.ToString());

            foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
            {
                ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, config.Value, config.Key);
                ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, config.Value, config.Key);
            }

            EditPanel.CameraIsModify();

            if (OnTvStandardChange != null)
                OnTvStandardChange(this, null);
        }

        private void SensorModeComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.SensorMode = SensorModes.DisplayStringToIndex(sensorModeComboBox.SelectedItem.ToString());
            
            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "Axis":
                    UpdatePowerFrequency();
                    break;

                case "VIVOTEK":
                    UpdateDeviceMountType();
                    break;
            }

            EditPanel.CameraIsModify();
            
            if (OnSensorModeChange != null)
                OnSensorModeChange(this, null);
        }

        private void HttpPortTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            UInt32 port = (httpPortTextBox.Text != "") ? Convert.ToUInt32(httpPortTextBox.Text) : 80;

            EditPanel.Camera.Profile.HttpPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

            EditPanel.CameraIsModify();
        }

        private void AudioPortTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            UInt32 port = (audioPortTextBox.Text != "") ? Convert.ToUInt32(audioPortTextBox.Text) : 0;

            EditPanel.Camera.Profile.AudioOutPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

            EditPanel.CameraIsModify();
        }

        private void URITextBoxTextChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.URI = uriTextBox.Text;

            EditPanel.CameraIsModify();
        }

        private void AspectRatioCorrectionCheckBoxCheckStateChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.AspectRatioCorrection = aspectRatioCorrectionCheckBox.Checked;

            EditPanel.CameraIsModify();

            if (OnAspectRatioCorrectionChange != null)
                OnAspectRatioCorrectionChange(this, null);
        }

        private void RemoteRecoveryCheckBoxCheckStateChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.RemoteRecovery = RemoteRecoveryCheckBox.Checked;

            EditPanel.CameraIsModify();
        }

        public void ParseDevice()
        {
            ipAddressTextBox.Text = EditPanel.Camera.Profile.NetworkAddress;
            modeComboBox.SelectedItem = CameraModes.ToDisplayString(EditPanel.Camera.Mode);
            httpPortTextBox.Text = EditPanel.Camera.Profile.HttpPort.ToString();
            audioPortTextBox.Text = EditPanel.Camera.Profile.AudioOutPort.ToString();
            aspectRatioCorrectionCheckBox.Checked = EditPanel.Camera.Profile.AspectRatioCorrection;
            RemoteRecoveryCheckBox.Checked = EditPanel.Camera.Profile.RemoteRecovery;
            tvStandardComboBox.SelectedItem = TvStandards.ToString(EditPanel.Camera.Profile.TvStandard);
            sensorModeComboBox.SelectedItem = SensorModes.ToDisplayString(EditPanel.Camera.Profile.SensorMode);
            powerFrequencyComboBox.SelectedItem = PowerFrequencies.ToDisplayString(EditPanel.Camera.Profile.PowerFrequency);
            aspectRatioComboBox.SelectedItem = AspectRatios.ToString(EditPanel.Camera.Profile.AspectRatio);
            if (String.IsNullOrEmpty(EditPanel.Camera.Profile.DewarpType))
                dewarpTypeComboBox.SelectedIndex = 0;
            else
                dewarpTypeComboBox.SelectedItem = EditPanel.Camera.Profile.DewarpType;

            AspectRatioPanelVisible();
            RemoteRecoveryPanelVisible();
            TvStandardVisible();
            SensorModePanelVisible();
            PowerFrequencyPanelVisible();
            MountTypePanelVisible();
            AudioPortPanelVisible();
            URIPanelVisible();
        }

        //------------------------------------------------------------------------------------

        public void UpdateStreamConfigList()
        {
            streamComboBox.Items.Clear();
            recordStreamComboBox.Items.Clear();

            recordProfilePanel.Enabled = true;
            var items = new Dictionary<UInt16, String>();
            switch (EditPanel.Camera.Mode)
            {
                case CameraMode.Single:
                    items.Add(1, _streamId1Str);
                    recordProfilePanel.Visible = liveProfilePanel.Visible = false;
                    break;

                case CameraMode.Dual:
                    items.Add(1, _streamId1Str);

                    EditPanel.AddStreamConfigFromDefault(2);

                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "Customization":
                            items.Add(2, _streamId2Str);
                            break;
                        default:
                            if (EditPanel.Camera.Profile.StreamConfigs[2].Compression != Compression.Off)
                                items.Add(2, _streamId2Str);
                            break;
                    }

                    recordProfilePanel.Visible = liveProfilePanel.Visible = true;
                    break;

                case CameraMode.Triple:
                    items.Add(1, _streamId1Str);

                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "Customization":
                            items.Add(2, _streamId2Str);
                            items.Add(3, _streamId3Str);
                            break;
                        default:
                            EditPanel.AddStreamConfigFromDefault(2);
                            if (EditPanel.Camera.Profile.StreamConfigs[2].Compression != Compression.Off)
                                items.Add(2, _streamId2Str);

                            EditPanel.AddStreamConfigFromDefault(3);
                            if (EditPanel.Camera.Profile.StreamConfigs[3].Compression != Compression.Off)
                                items.Add(3, _streamId3Str);
                            break;
                    }

                    recordProfilePanel.Visible = liveProfilePanel.Visible = true;
                    break;

                case CameraMode.Multi:
                case CameraMode.FourVga:
                    items.Add(1, _streamId1Str);
                    items.Add(2, _streamId2Str);
                    items.Add(3, _streamId3Str);
                    items.Add(4, _streamId4Str);

                    EditPanel.AddStreamConfigFromDefault(2);
                    EditPanel.AddStreamConfigFromDefault(3);
                    EditPanel.AddStreamConfigFromDefault(4);

                    recordProfilePanel.Visible = liveProfilePanel.Visible = true;

                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "ACTi":
                            recordProfilePanel.Enabled = false;
                            break;
                    }
                    break;

                case CameraMode.Five:
                    items.Add(1, _streamId1Str);
                    items.Add(2, _streamId2Str);
                    items.Add(3, _streamId3Str);
                    items.Add(4, _streamId4Str);
                    items.Add(5, _streamId5Str);

                    EditPanel.AddStreamConfigFromDefault(2);
                    EditPanel.AddStreamConfigFromDefault(3);
                    EditPanel.AddStreamConfigFromDefault(4);
                    EditPanel.AddStreamConfigFromDefault(5);

                    recordProfilePanel.Visible = liveProfilePanel.Visible = true;
                    break;

                case CameraMode.Quad:
                    items.Add(1, _streamId1Str);
                    items.Add(2, _streamId2Str);
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "Axis":
                            items.Add(3, _streamId3Str + " (QUAD) ");
                            break;
                        default:
                            items.Add(3, _streamId3Str);
                            break;
                    }

                    EditPanel.AddStreamConfigFromDefault(2);
                    recordProfilePanel.Visible = liveProfilePanel.Visible = true;
                    break;

                case CameraMode.SixVga:
                    items.Add(1, _streamId1Str);
                    items.Add(2, _streamId2Str);
                    items.Add(3, _streamId3Str);
                    items.Add(4, _streamId4Str);
                    items.Add(5, _streamId5Str);
                    items.Add(6, _streamId6Str);

                    EditPanel.AddStreamConfigFromDefault(2);
                    EditPanel.AddStreamConfigFromDefault(3);
                    EditPanel.AddStreamConfigFromDefault(4);
                    EditPanel.AddStreamConfigFromDefault(5);
                    EditPanel.AddStreamConfigFromDefault(6);

                    recordProfilePanel.Visible = liveProfilePanel.Visible = true;

                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "ACTi":
                            recordProfilePanel.Enabled = false;
                            break;
                    }
                    break;
            }

            if (!items.ContainsKey(EditPanel.Camera.Profile.StreamId))
                EditPanel.Camera.Profile.StreamId = 1;

            if (!items.ContainsKey(EditPanel.Camera.Profile.RecordStreamId))
                EditPanel.Camera.Profile.RecordStreamId = 1;

            foreach (var item in items)
            {
                streamComboBox.Items.Add(item.Value);
                recordStreamComboBox.Items.Add(item.Value);
            }

            if (items.ContainsKey(EditPanel.Camera.Profile.StreamId))
                streamComboBox.SelectedItem = items[EditPanel.Camera.Profile.StreamId];
            else
                streamComboBox.SelectedIndex = 0;

            if (items.ContainsKey(EditPanel.Camera.Profile.RecordStreamId))
                recordStreamComboBox.SelectedItem = items[EditPanel.Camera.Profile.RecordStreamId];
            else
                recordStreamComboBox.SelectedIndex = 0;

            streamComboBox.Enabled = recordStreamComboBox.Enabled = (items.Count > 1);

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "ONVIF":
                case "Kedacom":
                    modePanel.Visible = false;
                    break;

                default:
                    modePanel.Visible = true;
                    break;
            }

            Invalidate();
        }

        public void UpdateMode()
        {
            modeComboBox.Items.Clear();

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "Panasonic":
                    switch (EditPanel.Camera.Model.Series)
                    {
                        case "SW4x8":
                            if (EditPanel.Camera.Profile.AspectRatio == AspectRatio.Ratio43_VGA_QuadStreams_Ceiling)
                            {
                                modeComboBox.Items.Add(CameraModes.ToDisplayString(CameraMode.Single));
                                modeComboBox.SelectedItem = CameraModes.ToDisplayString(CameraMode.Single);
                            }
                            else
                            {
                                modeComboBox.Items.Add(CameraModes.ToDisplayString(CameraMode.Dual));
                                modeComboBox.SelectedItem = CameraModes.ToDisplayString(CameraMode.Dual);
                            }
                            break;

                        case "SFx631L":
                            if (EditPanel.Camera.Profile.AspectRatio == AspectRatio.Ratio43_3000K)
                            {
                                modeComboBox.Items.Add(CameraModes.ToDisplayString(CameraMode.Dual));
                                modeComboBox.SelectedItem = CameraModes.ToDisplayString(CameraMode.Dual);
                            }
                            else
                            {
                                foreach (CameraMode cameraMode in EditPanel.Camera.Model.CameraMode)
                                {
                                    modeComboBox.Items.Add(CameraModes.ToDisplayString(cameraMode));
                                }
                                modeComboBox.SelectedItem = CameraModes.ToDisplayString(EditPanel.Camera.Mode);
                            }
                            break;

                        default:
                            foreach (CameraMode cameraMode in EditPanel.Camera.Model.CameraMode)
                            {
                                modeComboBox.Items.Add(CameraModes.ToDisplayString(cameraMode));
                            }
                            modeComboBox.SelectedItem = CameraModes.ToDisplayString(EditPanel.Camera.Mode);
                            break;
                    }
                    
                    break;

                default:
                    foreach (CameraMode cameraMode in EditPanel.Camera.Model.CameraMode)
                    {
                        modeComboBox.Items.Add(CameraModes.ToDisplayString(cameraMode));
                    }
                    modeComboBox.SelectedItem = CameraModes.ToDisplayString(EditPanel.Camera.Mode);
                    break;
            }
            modeComboBox.Enabled = (modeComboBox.Items.Count > 1);
        }

        public void UpdateTvStandard()
        {
            tvStandardComboBox.Items.Clear();
            
            switch (EditPanel.Camera.Model .Manufacture)
            {
                case "NEXCOM":
                    foreach (TvStandard tvStandard in ((NexcomCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "YUAN":
                    foreach (TvStandard tvStandard in ((YuanCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "Stretch":
                    foreach (TvStandard tvStandard in ((StretchCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "Messoa":
                    foreach (TvStandard tvStandard in ((MessoaCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "MegaSys":
                    foreach (TvStandard tvStandard in ((MegaSysCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "Avigilon":
                    foreach (TvStandard tvStandard in ((AvigilonCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "DivioTec":
                    foreach (TvStandard tvStandard in ((DivioTecCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "SIEMENS":
                    foreach (TvStandard tvStandard in ((SIEMENSCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "SAMSUNG":
                    foreach (TvStandard tvStandard in ((SAMSUNGCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "inskytec":
                    foreach (TvStandard tvStandard in ((InskytecCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "VIVOTEK":
                    foreach (TvStandard tvStandard in ((VIVOTEKCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "Certis":
                    foreach (TvStandard tvStandard in ((CertisCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "Brickcom":
                    foreach (TvStandard tvStandard in ((BrickcomCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "VIGZUL":
                    foreach (TvStandard tvStandard in ((VIGZULCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "DLink":
                    foreach (TvStandard tvStandard in ((DLinkCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "Panasonic":
                    foreach (TvStandard tvStandard in ((PanasonicCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "FINE":
                    foreach (TvStandard tvStandard in ((FINECameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "GoodWill":
                    foreach (TvStandard tvStandard in ((GoodWillCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;

                case "MOBOTIX":
                    foreach (TvStandard tvStandard in ((MOBOTIXCameraModel)EditPanel.Camera.Model).TvStandard)
                        tvStandardComboBox.Items.Add(TvStandards.ToString(tvStandard));
                    break;
            }

            tvStandardComboBox.SelectedItem = TvStandards.ToString(EditPanel.Camera.Profile.TvStandard);

            tvStandardComboBox.Enabled = (tvStandardComboBox.Items.Count > 1);

            TvStandardVisible();
        }

        public void UpdateSensor()
        {
            sensorModeComboBox.Items.Clear();
            
            switch(EditPanel.Camera.Model.Manufacture)
            {
                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    foreach (SensorMode sensorMode in ((EtrovisionCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "Axis":
                    foreach (SensorMode sensorMode in ((AxisCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "ArecontVision":
                    foreach (SensorMode sensorMode in ((ArecontVisionCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "VIVOTEK":
                    foreach (SensorMode sensorMode in ((VIVOTEKCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "DLink":
                    foreach (SensorMode sensorMode in ((DLinkCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "Panasonic":
                    foreach (SensorMode sensorMode in ((PanasonicCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "A-MTK":
                    foreach (SensorMode sensorMode in ((AMTKCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "Certis":
                    foreach (SensorMode sensorMode in ((CertisCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;

                case "DivioTec":
                    foreach (SensorMode sensorMode in ((DivioTecCameraModel)EditPanel.Camera.Model).SensorMode)
                    {
                        sensorModeComboBox.Items.Add(SensorModes.ToDisplayString(sensorMode));
                    }
                    break;
            }

            sensorModeComboBox.SelectedItem = SensorModes.ToDisplayString(EditPanel.Camera.Profile.SensorMode);

            sensorModeComboBox.Enabled = (sensorModeComboBox.Items.Count > 1);

            SensorModePanelVisible();
        }

        public void UpdateAudioOutPort()
        {
            if (EditPanel.Camera.Model.Manufacture == "Messoa")
            {
                audioPortTextBox.Text = EditPanel.Camera.Profile.AudioOutPort.ToString();
            }

            AudioPortPanelVisible();
        }

        public void UpdateURI()
        {
            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "Customization":
                    uriTextBox.Text = EditPanel.Camera.Profile.URI;
                    break;
            }
         
            URIPanelVisible();
        }

        public void UpdateDeviceMountType()
        {
            mountTypeComboBox.Items.Clear();

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "ACTi":
                    if (((ACTiCameraModel)EditPanel.Camera.Model).DeviceMountType.Count > 0)
                    {
                        foreach (DeviceMountType deviceMountType in ((ACTiCameraModel)EditPanel.Camera.Model).DeviceMountType)
                            mountTypeComboBox.Items.Add(DeviceMountTypes.ToString(deviceMountType));
                    }
                    break;

                case "Axis":
                    if (((AxisCameraModel)EditPanel.Camera.Model).DeviceMountType.Count > 0)
                    {
                        foreach (DeviceMountType deviceMountType in ((AxisCameraModel)EditPanel.Camera.Model).DeviceMountType)
                            mountTypeComboBox.Items.Add(DeviceMountTypes.ToString(deviceMountType));
                    }
                    break;

                case "VIVOTEK":
                    if (EditPanel.Camera.Profile.SensorMode == SensorMode.Fisheye)
                    {
                        foreach (DeviceMountType deviceMountType in ((VIVOTEKCameraModel)EditPanel.Camera.Model).DeviceMountType)
                            mountTypeComboBox.Items.Add(DeviceMountTypes.ToString(deviceMountType));
                    }
                    break;
            }

            if (mountTypeComboBox.Items.Count > 0)
            {
                //mountTypeComboBox.SelectedIndexChanged -= MountTypeComboBoxSelectedIndexChanged;
                if (EditPanel.Camera.Profile.DeviceMountType == DeviceMountType.NonSpecific)
                    mountTypeComboBox.SelectedItem = mountTypeComboBox.Items[0];
                else
                    mountTypeComboBox.SelectedItem = DeviceMountTypes.ToString(EditPanel.Camera.Profile.DeviceMountType);
                //mountTypeComboBox.SelectedIndexChanged += MountTypeComboBoxSelectedIndexChanged;
            }
            else
            {
                EditPanel.Camera.Profile.DeviceMountType = DeviceMountType.NonSpecific;
            }

            mountTypeComboBox.Enabled = (mountTypeComboBox.Items.Count > 1);

            MountTypePanelVisible();
        }

        public void UpdatePowerFrequency()
        {
            powerFrequencyComboBox.Items.Clear();

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    foreach (PowerFrequency powerFrequency in ((EtrovisionCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;
                
                case "Messoa":
                    foreach (PowerFrequency powerFrequency in ((MessoaCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                case "PULSE":
                    foreach (PowerFrequency powerFrequency in ((PULSECameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                case "ZeroOne":
                    foreach (PowerFrequency powerFrequency in ((ZeroOneCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                case "DLink":
                    foreach (PowerFrequency powerFrequency in ((DLinkCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                case "Panasonic":
                    foreach (PowerFrequency powerFrequency in ((PanasonicCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                case "Axis":
                    foreach (PowerFrequency powerFrequency in ((AxisCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));

                    if (powerFrequencyComboBox.Items.Count <= 1)
                    {
                        switch (EditPanel.Camera.Profile.SensorMode)
                        {
                            case SensorMode.Progressive720P25:
                            case SensorMode.Progressive720P50:
                            case SensorMode.Progressive1080P25:
                            case SensorMode.Progressive1080P50:
                            case SensorMode.Progressive1080I25:
                            case SensorMode.Progressive1080I50:
                                powerFrequencyComboBox.Items.Clear();
                                powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(PowerFrequency.Hertz50));
                                break;

                            case SensorMode.Progressive720P30:
                            case SensorMode.Progressive720P60:
                            case SensorMode.Progressive1080P30:
                            case SensorMode.Progressive1080P60:
                            case SensorMode.Progressive1080I30:
                            case SensorMode.Progressive1080I60:
                                powerFrequencyComboBox.Items.Clear();
                                powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(PowerFrequency.Hertz50));
                                break;
                        }
                    }
                    break;

                case "VIVOTEK":
                    foreach (PowerFrequency powerFrequency in ((VIVOTEKCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                case "ArecontVision":
                    foreach (PowerFrequency powerFrequency in ((ArecontVisionCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                case "HIKVISION":
                    foreach (PowerFrequency powerFrequency in ((HIKVISIONCameraModel)EditPanel.Camera.Model).PowerFrequency)
                        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToDisplayString(powerFrequency));
                    break;

                //case "Messoa":
                //    foreach (PowerFrequency powerFrequency in ((MessoaCameraModel)EditPanel.Camera.Model).PowerFrequency)
                //        powerFrequencyComboBox.Items.Add(PowerFrequencies.ToString(powerFrequency));
                //break;
            }

            if (powerFrequencyComboBox.Items.Count > 0)
            {
                powerFrequencyComboBox.SelectedIndexChanged -= PowerFrequencyComboBoxSelectedIndexChanged;
                if(EditPanel.Camera.Profile.PowerFrequency == PowerFrequency.NonSpecific)
                    powerFrequencyComboBox.SelectedItem = powerFrequencyComboBox.Items[0];
                else
                    powerFrequencyComboBox.SelectedItem = PowerFrequencies.ToDisplayString(EditPanel.Camera.Profile.PowerFrequency);
                powerFrequencyComboBox.SelectedIndexChanged += PowerFrequencyComboBoxSelectedIndexChanged;
            }

            powerFrequencyComboBox.Enabled = (powerFrequencyComboBox.Items.Count > 1);

            PowerFrequencyPanelVisible();
        }

        public void UpdateAspectRatio()
        {
            aspectRatioComboBox.Items.Clear();
            aspectRatioComboBox.Width = 181;
            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "ACTi":
                    foreach (AspectRatio aspectRatio in ((ACTiCameraModel)EditPanel.Camera.Model).AspectRatio)
                        aspectRatioComboBox.Items.Add(AspectRatios.ToString(aspectRatio));
                    break;

                case "DLink":
                    foreach (AspectRatio aspectRatio in ((DLinkCameraModel)EditPanel.Camera.Model).AspectRatio)
                        aspectRatioComboBox.Items.Add(AspectRatios.ToString(aspectRatio));
                    break;

                case "Panasonic":
                    if (EditPanel.Camera.Model.Series == "SW4x8")
                    {
                        aspectRatioComboBox.Width = 251;
                    }

                    foreach (AspectRatio aspectRatio in ((PanasonicCameraModel)EditPanel.Camera.Model).AspectRatio)
                        aspectRatioComboBox.Items.Add(AspectRatios.ToString(aspectRatio));
                    break;
            }

            if (aspectRatioComboBox.Items.Count > 0)
                aspectRatioComboBox.SelectedItem = AspectRatios.ToString(EditPanel.Camera.Profile.AspectRatio);

            aspectRatioComboBox.Enabled = (aspectRatioComboBox.Items.Count > 1);

            aspectRatioCorrectionCheckBox.CheckStateChanged -= AspectRatioCorrectionCheckBoxCheckStateChanged;
            aspectRatioCorrectionCheckBox.Checked = EditPanel.Camera.Profile.AspectRatioCorrection;
            aspectRatioCorrectionCheckBox.CheckStateChanged += AspectRatioCorrectionCheckBoxCheckStateChanged;

            AspectRatioPanelVisible();
        }

        public void UpdateDewarpType()
        {
            dewarpTypeComboBox.Items.Clear();
            dewarpTypeComboBox.Items.Add("Off");
            foreach (var type in _lensType)
            {
                dewarpTypeComboBox.Items.Add(type);
            }

            foreach (var type in EditPanel.Camera.Model.LensType)
            {
                dewarpTypeComboBox.Items.Add(type);
            }

            if (String.IsNullOrEmpty(EditPanel.Camera.Profile.DewarpType))
                dewarpTypeComboBox.SelectedIndex = 0;
            else
                dewarpTypeComboBox.SelectedItem = EditPanel.Camera.Profile.DewarpType;

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "Kedacom":
                    dewarpTypePanel.Visible = false;
                    break;

                default:
                    dewarpTypePanel.Visible = true;
                    break;
            }
        }
        //------------------------------------------------------------------------------------

        public void AspectRatioPanelVisible()
        {
            if (EditPanel.Camera == null) return;

            aspectRatioCorrectionPanel.Visible = (EditPanel.Camera.Model.Series == "ARC");

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "ACTi":
                    aspectRatioPanel.Visible = (((ACTiCameraModel)EditPanel.Camera.Model).AspectRatio.Count > 0);
                    break;

                case "DLink":
                    aspectRatioPanel.Visible = (((DLinkCameraModel)EditPanel.Camera.Model).AspectRatio.Count > 0);
                    break;

                case "Panasonic":
                    aspectRatioPanel.Visible = (((PanasonicCameraModel)EditPanel.Camera.Model).AspectRatio.Count > 0);
                    break;

                default:
                    aspectRatioPanel.Visible = false;
                    break;
            }

            Invalidate();
        }

        public void RemoteRecoveryPanelVisible()
        {
            if (EditPanel.Camera == null) return;

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "VIVOTEK":
                case "Axis":
                case "A-MTK":
                    remoteRecoveryPanel.Visible = true;
                    break;

                case "Certis":
                    remoteRecoveryPanel.Visible = (EditPanel.Camera.Model.Series == "DynaColor" || EditPanel.Camera.Model.Series == "A-MTK");
                    break;

                default:
                    remoteRecoveryPanel.Visible = false;
                    break;
            }

            Invalidate();
        }

        public void TvStandardVisible()
        {
            if (EditPanel.Camera == null) return;

            tvStandardPanel.Visible = (EditPanel.Camera.Profile.TvStandard != TvStandard.NonSpecific);

            Invalidate();
        }
        
        public void SensorModePanelVisible()
        {
            if (EditPanel.Camera == null) return;

            sensorModePanel.Visible = (EditPanel.Camera.Profile.SensorMode != SensorMode.NonSpecific);

            Invalidate();
        }

        public void AudioPortPanelVisible()
        {
            if (EditPanel.Camera == null) return;

            audioPanel.Visible = (EditPanel.Camera.Profile.AudioOutPort != 0);

            Invalidate();
        }

        public void URIPanelVisible()
        {
            if (EditPanel.Camera == null) return;

            switch (EditPanel.Camera.Model.Manufacture)
            {
                //case "Customization":
                //    uriPanel.Visible = true;
                //    break;

                default:
                    uriPanel.Visible = false;
                    break;
            }

            Invalidate();
        }

        public void MountTypePanelVisible()
        {
            if (EditPanel.Camera == null) return;

            switch (EditPanel.Camera.Model.Manufacture)
            {
                case "ACTi":
                    mountTypePanel.Visible = (((ACTiCameraModel)EditPanel.Camera.Model).DeviceMountType.Count > 0);
                    break;

                case "Axis":
                    mountTypePanel.Visible = (((AxisCameraModel)EditPanel.Camera.Model).DeviceMountType.Count > 0);
                    break;

                case "VIVOTEK":
                    if (EditPanel.Camera.Model.Type == "fisheye" && EditPanel.Camera.Profile.SensorMode == SensorMode.Fisheye)
                    {
                        mountTypePanel.Visible = (((VIVOTEKCameraModel)EditPanel.Camera.Model).DeviceMountType.Count > 0);
                    }
                    else
                    {
                        mountTypePanel.Visible = false;
                    }
                    
                    break;

                default:
                    mountTypePanel.Visible = false;
                    break;
            }
        }

        public void PowerFrequencyPanelVisible()
        {
            if (EditPanel.Camera == null) return;

            switch (EditPanel.Camera.Profile.SensorMode)
            {
                case SensorMode.Progressive720P25:
                case SensorMode.Progressive720P30:
                case SensorMode.Progressive720P50:
                case SensorMode.Progressive720P60:
                case SensorMode.Progressive1080P25:
                case SensorMode.Progressive1080P30:
                case SensorMode.Progressive1080P50:
                case SensorMode.Progressive1080P60:
                case SensorMode.Progressive1080I25:
                case SensorMode.Progressive1080I30:
                case SensorMode.Progressive1080I50:
                case SensorMode.Progressive1080I60:
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "A-MTK":
                        case "Certis":
                            powerFrequencyPanel.Visible = (EditPanel.Camera.Profile.PowerFrequency != PowerFrequency.NonSpecific);
                            break;

                        default:
                            powerFrequencyPanel.Visible = true;
                            break;
                    }
                    
                    break;

                default:
                    switch (EditPanel.Camera.Model.Manufacture)
                    {
                        case "VIVOTEK":
                            powerFrequencyPanel.Visible = (((VIVOTEKCameraModel)EditPanel.Camera.Model).PowerFrequency.Count > 1);
                            break;

                        case "PULSE":
                            powerFrequencyPanel.Visible = (((PULSECameraModel)EditPanel.Camera.Model).PowerFrequency.Count > 1);
                            break;

                        case "ZeroOne":
                            powerFrequencyPanel.Visible = (((ZeroOneCameraModel)EditPanel.Camera.Model).PowerFrequency.Count > 1);
                            break;

                        case "DLink":
                            powerFrequencyPanel.Visible = (((DLinkCameraModel)EditPanel.Camera.Model).PowerFrequency.Count > 1);
                            break;

                        case "Panasonic":
                            powerFrequencyPanel.Visible = (((PanasonicCameraModel)EditPanel.Camera.Model).PowerFrequency.Count > 1);
                            break;

                        default:
                            powerFrequencyPanel.Visible = (EditPanel.Camera.Profile.PowerFrequency != PowerFrequency.NonSpecific);
                            break;
                    }
                    
                    break;
            }

            Invalidate();
        }

        public void NetwordAddressPanelVisible(Boolean visible)
        {
            networkAddressPanel.Visible = httpPanel.Visible = modePanel.Visible = visible;
        }
    }
}
