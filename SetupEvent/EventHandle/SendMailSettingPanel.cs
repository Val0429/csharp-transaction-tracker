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
    public sealed class SendMailSettingPanel : Panel
    {
        public HandlePanel HandlePanel;
        public Dictionary<String, String> Localization;

        private SendMailEventHandle _eventHandle;
        public SendMailEventHandle EventHandle
        {
            set
            {
                _eventHandle = value;

                if (HandlePanel.Server == null || _eventHandle == null) return;

                _isEditing = false;

                _deviceComboBox.Items.Clear();
                _deviceComboBox.Items.Add(" " + Localization["Handler_DontAttach"]);
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
                    if (_eventHandle.Device == null)
                        _nvrComboBox.SelectedIndex = 0;
                    else
                    {
                        _nvrComboBox.SelectedItem = _eventHandle.Device.Server;
                        foreach (KeyValuePair<UInt16, IDevice> device in _eventHandle.Device.Server.Device.Devices)
                        {
                            _deviceComboBox.Items.Add(device.Value);
                        }
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
                if (_eventHandle.Device == null)
                {
                    _deviceComboBox.SelectedIndex = 0;

                    if (HandlePanel.Server is ICMS)
                        _nvrComboBox.SelectedIndex = -1;
                }
                else
                    _deviceComboBox.SelectedItem = _eventHandle.Device;

                UpdateMailList();
                _toComboBox.Text = _eventHandle.MailReceiver;
                //_subjectTextBox.Text = _eventHandle.Subject;

                _isEditing = true;
            }
        }

        private readonly Label _deviceLabel;
        private readonly Label _nvrLabel;
        private readonly ComboBox _nvrComboBox;
        private readonly ComboBox _deviceComboBox;
        private readonly Label _toLabel;
        private readonly ComboBox _toComboBox;
        //private readonly Label _subjectLabel;
        //private readonly TextBox _subjectTextBox;
        public SendMailSettingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Handler_Snapshot", "Snapshot"},
                                   {"Handler_To", "To"},
                                   {"Handler_DontAttach", "Don't Attach"},
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
                Sorted = true
            };

            _toLabel = new Label
            {
                Padding = new Padding(15, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_To"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _toComboBox = new ComboBox
            {
                Width = 130,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDown,
                //FlatStyle = FlatStyle.System,
                Sorted = true,
            };

            //_subjectLabel = new Label
            //{
            //    Padding = new Padding(15, 0, 0, 4),
            //    AutoSize = true,
            //    MinimumSize = new Size(20, 24),
            //    Dock = DockStyle.Left,
            //    Text = @"Subject",
            //    TextAlign = ContentAlignment.MiddleLeft,
            //};

            //_subjectTextBox = new TextBox
            //{
            //    Width = 200,
            //    Dock = DockStyle.Left,
            //};

            //Controls.Add(_subjectTextBox);
            //Controls.Add(_subjectLabel);
            Controls.Add(_toComboBox);
            Controls.Add(_toLabel);
            Controls.Add(_deviceComboBox);
            Controls.Add(_nvrLabel);
            Controls.Add(_nvrComboBox);
            Controls.Add(_deviceLabel);

            _deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            _toComboBox.TextChanged += ToComboBoxTextChanged;
            //_subjectTextBox.TextChanged += SubjectTextBoxTextChanged;
        }

        private Boolean _isEditing;

        private void NVRComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;
            var nvr = _nvrComboBox.SelectedItem as INVR;
            if (nvr == null) return;

            _isEditing = false;

            _deviceComboBox.Items.Clear();
            _deviceComboBox.Items.Add(" " + Localization["Handler_DontAttach"]);
            foreach (KeyValuePair<UInt16, IDevice> device in nvr.Device.Devices)
            {
                _deviceComboBox.Items.Add(device.Value);
            }
            Manager.DropDownWidth(_deviceComboBox);
            _deviceComboBox.SelectedIndex = 0;
            _eventHandle.Device = null;
            _eventHandle.AttachFile = false;

            HandlePanel.HandleChange();
            _isEditing = true;
        }

        private void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            //Dont Attach File
            if (_deviceComboBox.SelectedIndex == 0)
            {
                _eventHandle.Device = null;
                _eventHandle.AttachFile = false;

                HandlePanel.HandleChange();
                return;
            }

            //String deviceName = "";
            //if (_eventHandle.Device != null)
            //    deviceName = _eventHandle.Device.ToString();

            _eventHandle.Device = _deviceComboBox.SelectedItem as ICamera;

            if (_eventHandle.Device == null)
            {
                _eventHandle.AttachFile = false;

                HandlePanel.HandleChange();
                return;
            }

            _eventHandle.AttachFile = true;

            HandlePanel.HandleChange();
            //if (deviceName != "")
            //    _subjectTextBox.Text = _subjectTextBox.Text.Replace(deviceName, _eventHandle.Device.ToString());
        }

        private void UpdateMailList()
        {
            _toComboBox.Items.Clear();

            foreach (KeyValuePair<UInt16, IUser> obj in HandlePanel.Server.User.Users)
            {
                if (obj.Value.Email == "") continue;
                _toComboBox.Items.Add(obj.Value.Email);
            }
        }

        private void ToComboBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            _eventHandle.MailReceiver = _toComboBox.Text;

            HandlePanel.HandleChange();
        }

        //private void SubjectTextBoxTextChanged(Object sender, EventArgs e)
        //{
        //    if (!_isEditing || _eventHandle == null) return;

        //    _eventHandle.Subject = _subjectTextBox.Text;

        //    HandlePanel.HandleChange();
        //}
    }
}
