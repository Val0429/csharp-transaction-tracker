using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using ServerProfile;

namespace SetupEvent
{
    public sealed class SendMailNoAttachSettingPanel : Panel
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

                UpdateMailList();
                _toComboBox.Text= _eventHandle.MailReceiver;

                _isEditing = true;
            }
        }

        private readonly Label _toLabel;
        private readonly ComboBox _toComboBox;
        public SendMailNoAttachSettingPanel()
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

            Controls.Add(_toComboBox);
            Controls.Add(_toLabel);

            _toComboBox.TextChanged += ToComboBoxTextChanged;
        }

        private Boolean _isEditing;

        private void UpdateMailList()
        {
            _toComboBox.Items.Clear();

            foreach (KeyValuePair<UInt16, IUser> obj in HandlePanel.Server.User.Users)
            {
                if(obj.Value.Email == "") continue;
                _toComboBox.Items.Add(obj.Value.Email);
            }
        }

        private void ToComboBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            _eventHandle.MailReceiver = _toComboBox.Text;

            HandlePanel.HandleChange();
        }
    }
}
