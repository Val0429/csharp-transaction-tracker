using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;

namespace SetupEvent
{
    public sealed class TriggerDigitalOutSettingPanel : Panel
    {
        public HandlePanel HandlePanel;
        public Dictionary<String, String> Localization;

        private TriggerDigitalOutEventHandle _eventHandle;
        public TriggerDigitalOutEventHandle EventHandle
        {
            set
            {
                _eventHandle = value;

                if (HandlePanel.Server == null || _eventHandle == null || _eventHandle.Device == null || !(_eventHandle.Device is ICamera)) return;

                _isEditing = false;

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
                    foreach (KeyValuePair<UInt16, IDevice> obj in HandlePanel.Server.Device.Devices)
                    {
                        var camera = obj.Value as ICamera;
                        if (camera == null) continue;

                        if (camera.Model.IOPortSupport != null)
                        {
                            var dosupport = false;
                            foreach (var ioPort in camera.IOPort)
                            {
                                if (ioPort.Value == IOPort.Output)
                                {
                                    dosupport = true;
                                    break;
                                }
                            }
                            if (!dosupport)
                                continue;
                        }
                        else
                        {
                            if (camera.Model.NumberOfDo == 0)
                                continue;
                        }

                        _deviceComboBox.Items.Add(obj.Value);
                    }
                }

                if (_deviceComboBox.Items.Count > 0)
                {
                    if (_deviceComboBox.Items.Contains(_eventHandle.Device))
                    {
                        _deviceComboBox.SelectedItem = _eventHandle.Device;
                        UpdateDigitalOutList();
                        _doComboBox.SelectedItem = _eventHandle.DigitalOutId.ToString();
                        _statusComboBox.SelectedIndex = ((_eventHandle.DigitalOutStatus) ? 0 : 1);
                    }
                    else
                    {
                        _isEditing = true;
                        _deviceComboBox.SelectedIndex = 0;
                    }

                    _deviceComboBox.Enabled = (_deviceComboBox.Items.Count > 1);
                }
                else
                {
                    _deviceComboBox.Enabled = _doComboBox.Enabled = _statusComboBox.Enabled = false;
                }

                _isEditing = true;
            }
        }

        private readonly Label _deviceLabel;
        private readonly Label _nvrLabel;
        private readonly ComboBox _nvrComboBox;
        private readonly ComboBox _deviceComboBox;
        private readonly Label _doLabel;
        private readonly ComboBox _doComboBox;
        private readonly Label _statusLabel;
        private readonly ComboBox _statusComboBox;
        public TriggerDigitalOutSettingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Handler_Device", "Device"},
                                   {"Handler_ID", "ID"},
                                   {"Handler_Status", "Status"},
                                   {"Handler_On", "On"},
                                   {"Handler_Off", "Off"},
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
                Width = 110,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
                Sorted = true,
            };

            _doLabel = new Label
            {
                Padding = new Padding(15, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_ID"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _doComboBox = new ComboBox
            {
                Width = 60,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
                Sorted = true,
            };

            _statusLabel = new Label
            {
                Padding = new Padding(15, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_Status"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _statusComboBox = new ComboBox
            {
                Width = 110,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
            };
            _statusComboBox.Items.Add(Localization["Handler_On"]);
            _statusComboBox.Items.Add(Localization["Handler_Off"]);

            Controls.Add(_statusComboBox);
            Controls.Add(_statusLabel);
            Controls.Add(_doComboBox);
            Controls.Add(_doLabel);
            Controls.Add(_deviceComboBox);
            Controls.Add(_nvrLabel);
            Controls.Add(_nvrComboBox);
            Controls.Add(_deviceLabel);

            _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            _doComboBox.SelectedIndexChanged += DoComboBoxSelectedIndexChanged;
            _statusComboBox.SelectedIndexChanged += StatusComboBoxSelectedIndexChanged;
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

        private void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            _eventHandle.Device = _deviceComboBox.SelectedItem as IDevice;
            UpdateDigitalOutList();
            if (_doComboBox.Items.Count == 0)
            {
                _doComboBox.Enabled = false;
                HandlePanel.HandleChange();
                return;
            }
            _doComboBox.SelectedIndex = 0;
            if (_statusComboBox.SelectedIndex == -1)
                _statusComboBox.SelectedIndex = 0;
            HandlePanel.HandleChange();
        }

        private void UpdateDigitalOutList()
        {
            _doComboBox.Items.Clear();
            var camera = _eventHandle.Device as ICamera;
            if (camera == null)
            {
                _doComboBox.Enabled = false;
                return;
            }

            if (camera.Model.IOPortSupport != null)
            {
                foreach (var ioPort in camera.IOPort)
                {
                    if (ioPort.Value == IOPort.Output)
                        _doComboBox.Items.Add(ioPort.Key.ToString());
                }
            }
            else
            {
                for (UInt16 i = 1; i <= camera.Model.NumberOfDo; i++)
                {
                    _doComboBox.Items.Add(i.ToString());
                }
            }
            
            if (_doComboBox.Items.Count == 0)
            {
                _statusComboBox.Enabled = _doComboBox.Enabled = false;
            }
            else if (_doComboBox.Items.Count == 1)
            {
                _doComboBox.Enabled = false;
                _statusComboBox.Enabled = true;
            }
            else
            {
                _doComboBox.Enabled = _statusComboBox.Enabled = true;
            }
        }

        private void DoComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;
            if(!(_eventHandle.Device is ICamera)) return;

            _eventHandle.DigitalOutId = Convert.ToUInt16(_doComboBox.SelectedItem);
            HandlePanel.HandleChange();
        }

        private void StatusComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;
            if (!(_eventHandle.Device is ICamera)) return;

            _eventHandle.DigitalOutStatus = (_statusComboBox.SelectedIndex == 0);
            HandlePanel.HandleChange();
        }
    }
}
