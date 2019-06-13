using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace SetupDevice
{
    public sealed partial class CopySettingPanel : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public CopySettingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupDevice_CopyFrom", "Copy From"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            BackgroundImage = SetupBase.Manager.BackgroundNoBorder;
            
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
        }

        public void Initialize()
        {
            copyFromPanel.Paint += CopyFromPanelPaint;

            copyFromComboBox.SelectedIndexChanged += CopyFromComboBoxSelectedIndexChanged;
        }

        private Boolean _isEditing;
        private ICamera copyFrom;
        private void CopyFromComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if(!containerPanel.Enabled) return;

            if (!(copyFromComboBox.SelectedItem is ICamera)) return;
            copyFrom = (ICamera)copyFromComboBox.SelectedItem;

            foreach (DevicePanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }

            foreach (DevicePanel control in containerPanel.Controls)
            {
                if(control.IsTitle)continue;
                ICamera camera = control.Device as ICamera;
                if(camera == null) continue;

                control.Enabled = (camera != copyFrom && camera.Model.Model == copyFrom.Model.Model);
            }
        }

        private void CopyFromPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SetupBase.Manager.PaintSingleInput(g, copyFromPanel);

            if (copyFromPanel.Width <= 100) return;

            SetupBase.Manager.PaintText(g, Localization["SetupDevice_CopyFrom"]);
        }

        private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();

        public void GenerateViewModel()
        {
            _isEditing = false;
            ClearViewModel();

            if (Server == null) return;

            var sortResult = new List<IDevice>(Server.Device.Devices.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0) return;
            
            containerPanel.Enabled = true;
            containerPanel.Visible = false;
            foreach (IDevice device in sortResult)
            {
                if (device != null && device is ICamera)
                {
                    var devicePanel = GetDevicePanel();

                    devicePanel.Device = device;
                    devicePanel.SelectionVisible = true;
                    containerPanel.Controls.Add(devicePanel);
                }
            }

            var deviceTitlePanel = GetDevicePanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.EditVisible = false;
            containerPanel.Controls.Add(deviceTitlePanel);
            containerPanel.Visible = true;

            copyFromComboBox.Items.Clear();

            sortResult.Sort((x, y) => (x.Id - y.Id));
            foreach (IDevice device in sortResult)
            {
                copyFromComboBox.Items.Add(device);
            }

            SetupBase.Manager.DropDownWidth(copyFromComboBox);
            copyFromComboBox.SelectedIndex = 0;

            _isEditing = true;
        }

        private DevicePanel GetDevicePanel()
        {
            if (_recycleDevice.Count > 0)
            {
                return _recycleDevice.Dequeue();
            }

            var devicePanel = new DevicePanel
            {
                EditVisible = false,
                SelectionVisible = true,
                Cursor = Cursors.Hand,
                Server = Server,
            };
            devicePanel.OnSelectChange += DevicePanelOnSelectChange;

            return devicePanel;
        }

        private void DevicePanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (DevicePanel control in containerPanel.Controls)
            {
                if (!control.Enabled) continue;

                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (DevicePanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectChange(Object sender, EventArgs e)
        {
            if(!_isEditing) return;
            if (copyFrom == null) return;
            var panel = sender as DevicePanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                if (panel.Device != null)
                    CopyDeviceSetting(panel.Device as ICamera);

                selectAll = true;
                foreach (DevicePanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as DevicePanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= DevicePanelOnSelectAll;
                title.OnSelectNone -= DevicePanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += DevicePanelOnSelectAll;
                title.OnSelectNone += DevicePanelOnSelectNone;
            }
        }

        private void CopyDeviceSetting(ICamera camera)
        {
            if (camera == null) return;
            if (camera.Model.Model != copyFrom.Model.Model) return;

            camera.Profile.Authentication.Encryption = copyFrom.Profile.Authentication.Encryption;
            camera.Profile.Authentication.UserName = copyFrom.Profile.Authentication.UserName;
            camera.Profile.Authentication.Password = copyFrom.Profile.Authentication.Password;

            camera.Mode = copyFrom.Mode;

            camera.Profile.HttpPort = copyFrom.Profile.HttpPort;
            camera.Profile.TvStandard = copyFrom.Profile.TvStandard;
            camera.Profile.PowerFrequency = copyFrom.Profile.PowerFrequency;
            camera.Profile.SensorMode = copyFrom.Profile.SensorMode;
            camera.Profile.StreamId = copyFrom.Profile.StreamId;
            camera.Profile.RecordStreamId = copyFrom.Profile.RecordStreamId;
            camera.Profile.AspectRatio = copyFrom.Profile.AspectRatio;
            camera.Profile.AspectRatioCorrection = copyFrom.Profile.AspectRatioCorrection;
            camera.Profile.DewarpType = copyFrom.Profile.DewarpType;
            camera.Profile.DeviceMountType = copyFrom.Profile.DeviceMountType;
            camera.Profile.RemoteRecovery = copyFrom.Profile.RemoteRecovery;
            camera.Profile.HighProfile = copyFrom.Profile.HighProfile;
            camera.Profile.MediumProfile = copyFrom.Profile.MediumProfile;
            camera.Profile.LowProfile = copyFrom.Profile.LowProfile;

            if (camera.Model.Type == "CaptureCard" && copyFrom.Profile.CaptureCardConfig != null)
            {
                camera.Profile.CaptureCardConfig = new CaptureCardConfig
                {
                    Brightness = copyFrom.Profile.CaptureCardConfig.Brightness,
                    Contrast = copyFrom.Profile.CaptureCardConfig.Contrast,
                    Hue = copyFrom.Profile.CaptureCardConfig.Hue,
                    Saturation = copyFrom.Profile.CaptureCardConfig.Saturation,
                    Sharpness = copyFrom.Profile.CaptureCardConfig.Sharpness,
                    Gamma = copyFrom.Profile.CaptureCardConfig.Gamma,
                    ColorEnable = copyFrom.Profile.CaptureCardConfig.ColorEnable,
                    WhiteBalance = copyFrom.Profile.CaptureCardConfig.WhiteBalance,
                    BacklightCompensation = copyFrom.Profile.CaptureCardConfig.BacklightCompensation,
                    Gain = copyFrom.Profile.CaptureCardConfig.Gain,
                    TemporalSensitivity = copyFrom.Profile.CaptureCardConfig.TemporalSensitivity,
                    SpatialSensitivity = copyFrom.Profile.CaptureCardConfig.SpatialSensitivity,
                    LevelSensitivity = copyFrom.Profile.CaptureCardConfig.LevelSensitivity,
                    Speed = copyFrom.Profile.CaptureCardConfig.Speed
                };
            }

            camera.IOPort.Clear();
            foreach (var ioPort in copyFrom.IOPort)
                camera.IOPort.Add(ioPort.Key, ioPort.Value);

            //NEXCOM dont copy stream-id
            //UInt16 channelId = copyFrom.StreamConfig.Channel;
            var temp = new Dictionary<UInt16, StreamConfig>();
            foreach (var streamConfig in camera.Profile.StreamConfigs)
            {
                temp.Add(streamConfig.Key, streamConfig.Value);
            }
            camera.Profile.StreamConfigs.Clear();
            foreach (var obj in copyFrom.Profile.StreamConfigs)
            {
                var streamConfig = StreamConfigs.Clone(obj.Value);
                //keep channel id
                switch (camera.Model.Manufacture)
                {
                    case "ArecontVision":
                            streamConfig.Channel = temp.ContainsKey(obj.Key) ? temp[(obj.Key)].Channel : temp[(ushort)(obj.Key > 1 ? 1 : obj.Key)].Channel;
                        break;

                    default:
                        if (temp.ContainsKey(obj.Key))
                            streamConfig.Channel = temp[obj.Key].Channel;
                        break;
                }
                
                camera.Profile.StreamConfigs.Add(obj.Key, streamConfig);
            }

            if (copyFrom.Model.Manufacture == "Customization")
            {
                camera.Profile.LiveCheckURI = copyFrom.Profile.LiveCheckURI;
                camera.Profile.LiveCheckInterval = copyFrom.Profile.LiveCheckInterval;
                camera.Profile.LiveCheckRetryCount = copyFrom.Profile.LiveCheckRetryCount;

                camera.Profile.PtzCommand.Up = copyFrom.Profile.PtzCommand.Up;
                camera.Profile.PtzCommand.Down = copyFrom.Profile.PtzCommand.Down;
                camera.Profile.PtzCommand.Left = copyFrom.Profile.PtzCommand.Left;
                camera.Profile.PtzCommand.Right = copyFrom.Profile.PtzCommand.Right;
                camera.Profile.PtzCommand.UpLeft = copyFrom.Profile.PtzCommand.UpLeft;
                camera.Profile.PtzCommand.DownLeft = copyFrom.Profile.PtzCommand.DownLeft;
                camera.Profile.PtzCommand.UpRight = copyFrom.Profile.PtzCommand.UpRight;
                camera.Profile.PtzCommand.DownRight = copyFrom.Profile.PtzCommand.DownRight;
                camera.Profile.PtzCommand.Stop = copyFrom.Profile.PtzCommand.Stop;

                camera.Profile.PtzCommand.ZoomIn = copyFrom.Profile.PtzCommand.ZoomIn;
                camera.Profile.PtzCommand.ZoomOut = copyFrom.Profile.PtzCommand.ZoomOut;
                camera.Profile.PtzCommand.ZoomStop = copyFrom.Profile.PtzCommand.ZoomStop;
                camera.Profile.PtzCommand.FocusIn = copyFrom.Profile.PtzCommand.FocusIn;
                camera.Profile.PtzCommand.FocusOut = copyFrom.Profile.PtzCommand.FocusOut;
                camera.Profile.PtzCommand.FocusStop = copyFrom.Profile.PtzCommand.FocusStop;

                foreach (KeyValuePair<ushort, PtzCommandCgi> point in copyFrom.Profile.PtzCommand.PresetPoints)
                {
                    if (camera.Profile.PtzCommand.PresetPoints.ContainsKey(point.Key))
                        camera.Profile.PtzCommand.PresetPoints[point.Key] = point.Value;
                    else
                        camera.Profile.PtzCommand.PresetPoints.Add(point.Key, point.Value);
                }

                foreach (KeyValuePair<ushort, PtzCommandCgi> point in copyFrom.Profile.PtzCommand.GotoPresetPoints)
                {
                    if (camera.Profile.PtzCommand.GotoPresetPoints.ContainsKey(point.Key))
                        camera.Profile.PtzCommand.GotoPresetPoints[point.Key] = point.Value;
                    else
                        camera.Profile.PtzCommand.GotoPresetPoints.Add(point.Key, point.Value);
                }

                foreach (KeyValuePair<ushort, PtzCommandCgi> point in copyFrom.Profile.PtzCommand.DeletePresetPoints)
                {
                    if (camera.Profile.PtzCommand.DeletePresetPoints.ContainsKey(point.Key))
                        camera.Profile.PtzCommand.DeletePresetPoints[point.Key] = point.Value;
                    else
                        camera.Profile.PtzCommand.DeletePresetPoints.Add(point.Key, point.Value);
                }
            }

            Server.DeviceModify(camera);
        }

        private void ClearViewModel()
        {
            foreach (DevicePanel devicePanel in containerPanel.Controls)
            {
                devicePanel.SelectionVisible = true;

                devicePanel.OnSelectChange -= DevicePanelOnSelectChange;
                devicePanel.Checked = false;
                devicePanel.Device = null;
                devicePanel.Cursor = Cursors.Hand;
                devicePanel.EditVisible = false;
                devicePanel.Enabled = true;
                devicePanel.OnSelectChange += DevicePanelOnSelectChange;

                if (devicePanel.IsTitle)
                {
                    devicePanel.OnSelectAll -= DevicePanelOnSelectAll;
                    devicePanel.OnSelectNone -= DevicePanelOnSelectNone;
                    devicePanel.IsTitle = false;
                }

                if (!_recycleDevice.Contains(devicePanel))
                    _recycleDevice.Enqueue(devicePanel);
            }
            containerPanel.Controls.Clear();
        }
    } 
}