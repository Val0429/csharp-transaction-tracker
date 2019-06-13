using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;

namespace SetupEvent
{
    public sealed class GotoPresetSettingPanel : Panel
    {
        public HandlePanel HandlePanel;
        public Dictionary<String, String> Localization;

        private GotoPresetEventHandle _eventHandle;
        public GotoPresetEventHandle EventHandle
        {
            set
            {
                _eventHandle = value;

                if (HandlePanel.Server == null || _eventHandle == null || _eventHandle.Device == null || !(_eventHandle.Device is ICamera)) return;

                _isEditing = false;

                UpdateDeviceList();
                UpdatePersetPointList();

                _isEditing = true;
            }
        }

        private readonly Label _deviceLabel;
        private readonly Label _nvrLabel;
        private readonly ComboBox _nvrComboBox;
        private readonly ComboBox _deviceComboBox;
        private readonly Label _presetLabel;
        private readonly ComboBox _presetComboBox;
        public GotoPresetSettingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Handler_Device", "Device"},
                                   {"Handler_PresetPoint", "Preset Point"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.None;
            AutoSize = true;
            Height = 24;
            Location = new Point(200, 7);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            _deviceLabel = new Label
            {
                Padding = new Padding(0, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_Device"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _nvrLabel = new Label
            {
                AutoSize = true,
                MinimumSize = new Size(10, 24),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };

            _nvrComboBox = new ComboBox
            {
                Width = 110,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
                Sorted = true,
                Visible = false
            };

            _deviceComboBox = new ComboBox
            {
                Width = 120,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
                Sorted = true,
            };

            _presetLabel = new Label
            {
                Padding = new Padding(15, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_PresetPoint"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _presetComboBox = new ComboBox
            {
                Width = 120,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
                Sorted = true,
            };

            Controls.Add(_presetComboBox);
            Controls.Add(_presetLabel);
            Controls.Add(_deviceComboBox);
            Controls.Add(_nvrLabel);
            Controls.Add(_nvrComboBox);
            Controls.Add(_deviceLabel);

            _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            _presetComboBox.SelectedIndexChanged += PresetComboBoxSelectedIndexChanged;
        }

        private Boolean _isEditing;

        private void NVRComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;
            var nvr = _nvrComboBox.SelectedItem as INVR;
            if (nvr == null) return;

            _isEditing = false;

            _deviceComboBox.Items.Clear();
            foreach (KeyValuePair<UInt16, IDevice> device in nvr.Device.Devices)
            {
                _deviceComboBox.Items.Add(device.Value);
            }
            Manager.DropDownWidth(_deviceComboBox);
            _deviceComboBox.Enabled = (_deviceComboBox.Items.Count > 1);
            _deviceComboBox.SelectedIndex = 0;
            _eventHandle.Device = _deviceComboBox.SelectedItem as IDevice;
            HandlePanel.HandleChange();
            _isEditing = true;
        }

        private void UpdateDeviceList()
        {
            _deviceComboBox.Items.Clear();

            if (HandlePanel.Server is ICMS)
            {
                _nvrLabel.Visible = _nvrComboBox.Visible = true;
                _nvrComboBox.SelectedIndexChanged += NVRComboBoxSelectedIndexChanged;

                _nvrComboBox.Items.Clear();
                var cms = HandlePanel.Server as ICMS;
                foreach (KeyValuePair<UInt16, INVR> nvr in cms.NVRManager.NVRs)
                {
                    if (nvr.Value.Device.Devices.Count == 0) continue;
                    _nvrComboBox.Items.Add(nvr.Value);
                }

                Manager.DropDownWidth(_nvrComboBox);
                _nvrComboBox.SelectedItem = _eventHandle.Device.Server;
                foreach (KeyValuePair<UInt16, IDevice> device in _eventHandle.Device.Server.Device.Devices)
                {
                    _deviceComboBox.Items.Add(device.Value);
                }
            }
            else
            {
                var devices = new List<IDevice>(HandlePanel.Server.Device.Devices.Values);

                devices.Sort((x, y) => (x.Id - y.Id));
                foreach (var device in devices)
                {
                    if (!(device is ICamera)) continue;
                    if (((ICamera)device).PresetPoints.Count == 0) continue;

                    _deviceComboBox.Items.Add(device);
                }
            }

            if (_deviceComboBox.Items.Count > 0)
            {
                if (_deviceComboBox.Items.Contains(_eventHandle.Device))
                {
                    _deviceComboBox.SelectedItem = _eventHandle.Device;
                }
                else
                {
                    _isEditing = true;
                    _deviceComboBox.SelectedIndex = 0;
                }
                _deviceComboBox.Enabled = true;
            }
            else
            {
                _deviceComboBox.Enabled = false;
            }
        }

        private void UpdatePersetPointList()
        {
            _presetComboBox.Items.Clear();
            if (!(_eventHandle.Device is ICamera))
            {
                _presetComboBox.Enabled = false;
                return;
            }

            var camera = _eventHandle.Device as ICamera;

            var points = camera.PresetPoints.Values.OrderBy(g => g.Id);

            foreach (var point in points)
            {
                _presetComboBox.Items.Add(point);
            }

            if (_presetComboBox.Items.Count > 0)
            {
                if (camera.PresetPoints.Keys.Contains(_eventHandle.PresetPoint))
                {
                    _presetComboBox.SelectedItem = camera.PresetPoints[_eventHandle.PresetPoint];
                }
                else
                {
                    _isEditing = true;
                    _presetComboBox.SelectedIndex = 0;
                }
                _presetComboBox.Enabled = true;
            }
            else
            {
                _presetComboBox.Enabled = false;
            }

        }

        private void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            _eventHandle.Device = (IDevice)_deviceComboBox.SelectedItem;
            UpdatePersetPointList();

            HandlePanel.HandleChange();
        }

        private void PresetComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;
            if(!(_eventHandle.Device is ICamera)) return;

            var point = _presetComboBox.SelectedItem as PresetPoint;
            _eventHandle.PresetPoint = (point != null)
                ? point.Id : (UInt16)0;

            HandlePanel.HandleChange();
        }
    }
}
