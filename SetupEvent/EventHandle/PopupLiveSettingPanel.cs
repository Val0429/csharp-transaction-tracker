using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using ServerProfile;

namespace SetupEvent
{
    public sealed class PopupLiveSettingPanel : Panel
    {
        public HandlePanel HandlePanel;
        public Dictionary<String, String> Localization;

        private PopupLiveEventHandle _eventHandle;
        public PopupLiveEventHandle EventHandle
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
                        if (!(obj.Value is ICamera)) continue;

                        _deviceComboBox.Items.Add(obj.Value);
                    }
                }

                _deviceComboBox.Enabled = (_deviceComboBox.Items.Count > 1);
                
                _deviceComboBox.SelectedItem = _eventHandle.Device;                
                _isEditing = true;
            }
        }

        private readonly Label _deviceLabel;
        private readonly Label _nvrLabel;
        private readonly ComboBox _nvrComboBox;
        private readonly ComboBox _deviceComboBox;
        public PopupLiveSettingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Handler_Device", "Device"},
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
            
            _deviceLabel.Text = Localization["Handler_Device"];

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

            Controls.Add(_deviceComboBox);
            Controls.Add(_nvrLabel);
            Controls.Add(_nvrComboBox);
            Controls.Add(_deviceLabel);

            _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
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

            HandlePanel.HandleChange();
        }
    }
}
