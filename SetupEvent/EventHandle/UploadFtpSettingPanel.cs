using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using ServerProfile;

namespace SetupEvent
{
    public sealed class UploadFtpSettingPanel : Panel
    {
        public HandlePanel HandlePanel;
        public Dictionary<String, String> Localization;

        private UploadFtpEventHandle _eventHandle;
        public UploadFtpEventHandle EventHandle
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
                
                Manager.DropDownWidth(_deviceComboBox);
                _deviceComboBox.Enabled = (_deviceComboBox.Items.Count > 1);

                _deviceComboBox.SelectedItem = _eventHandle.Device;
                String name = Regex.Replace(_eventHandle.FileName, "[^a-zA-Z0-9]", "");
                _fileTextBox.Text = _eventHandle.FileName = name;

                _isEditing = true;
            }
        }

        private readonly Label _deviceLabel;
        private readonly Label _nvrLabel;
        private readonly ComboBox _nvrComboBox;
        private readonly ComboBox _deviceComboBox;
        private readonly Label _fileLabel;
        private readonly TextBox _fileTextBox;
        public UploadFtpSettingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Handler_Snapshot", "Snapshot"},
                                   {"Handler_FileName", "File name"},
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
                Text = Localization["Handler_Snapshot"],
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

            _fileLabel = new Label
            {
                Padding = new Padding(15, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_FileName"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _fileTextBox = new PanelBase.HotKeyTextBox
            {
                Width = 150,
                Dock = DockStyle.Left,
                ImeMode = ImeMode.Disable
            };
            _fileTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            Controls.Add(_fileTextBox);
            Controls.Add(_fileLabel);
            Controls.Add(_deviceComboBox);
            Controls.Add(_nvrLabel);
            Controls.Add(_nvrComboBox);
            Controls.Add(_deviceLabel);

            _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            _fileTextBox.TextChanged += FileTextBoxTextChanged;
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

            //String deviceName = (_eventHandle.Device != null)
            //    ? _eventHandle.Device.ToString() : "";

            _eventHandle.Device = _deviceComboBox.SelectedItem as IDevice;

            HandlePanel.HandleChange();
            
            if(_eventHandle.Device == null) return;

            //if (deviceName != "")
            //    _fileTextBox.Text = _fileTextBox.Text.Replace(deviceName, _eventHandle.Device.ToString());
            
            _fileTextBox.Text = Regex.Replace(_eventHandle.Device.ToString(), "[^a-zA-Z0-9]", "");
        }

        private void FileTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            _eventHandle.FileName = _fileTextBox.Text;

            HandlePanel.HandleChange();
        }
    }
}
