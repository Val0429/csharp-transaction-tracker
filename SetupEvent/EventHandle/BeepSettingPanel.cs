using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using ServerProfile;

namespace SetupEvent
{
    public sealed class BeepSettingPanel : Panel
    {
        public HandlePanel HandlePanel;
        public Dictionary<String, String> Localization;

        private BeepEventHandle _eventHandle;
        public BeepEventHandle EventHandle
        {
            set
            {
                _eventHandle = value;

                if (_eventHandle == null) return;

                _isEditing = false;

                _timesComboBox.SelectedItem = _eventHandle.Times.ToString();
                _durationComboBox.SelectedItem = _eventHandle.Duration.ToString();

                _isEditing = true;
            }
        }

        private readonly Label _timesLabel;
        private readonly ComboBox _timesComboBox;
        private readonly Label _durationLabel;
        private readonly ComboBox _durationComboBox;
        private readonly Label _secLabel;
        
        public BeepSettingPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "Sec"},
                                   
                                   {"Handler_Duration", "Duration"},
                                   {"Handler_Repetition", "Repetition"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.None;
            AutoSize = true;
            Height = 24;
            Location = new Point(200, 7);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            _timesLabel = new Label
            {
                Padding = new Padding(0, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_Repetition"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _timesComboBox = new ComboBox
            {
                Width = 60,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
            };

            _durationLabel = new Label
            {
                Padding = new Padding(15, 0, 0, 4),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Handler_Duration"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            _durationComboBox = new ComboBox
            {
                Width = 60,
                Dock = DockStyle.Left,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
            };

            _secLabel = new Label
            {
                Padding = new Padding(4, 0, 0, 0),
                AutoSize = true,
                MinimumSize = new Size(20, 24),
                Dock = DockStyle.Left,
                Text = Localization["Common_Sec"],
                TextAlign = ContentAlignment.MiddleLeft,
            };

            for (UInt16 i = 1; i <= 10; i++)
            {
                _timesComboBox.Items.Add(i.ToString());
            }

            for (UInt16 i = 1; i <= 10; i++)
            {
                _durationComboBox.Items.Add(i.ToString());
            }

            Controls.Add(_secLabel);
            Controls.Add(_durationComboBox);
            Controls.Add(_durationLabel);
            Controls.Add(_timesComboBox);
            Controls.Add(_timesLabel);

            _timesComboBox.SelectedIndexChanged += TimesComboBoxSelectedIndexChanged;
            _durationComboBox.SelectedIndexChanged += DurationComboBoxSelectedIndexChanged;
        }

        private Boolean _isEditing;

        private void TimesComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            _eventHandle.Times = Convert.ToUInt16(_timesComboBox.SelectedItem);

            if (HandlePanel != null)
                HandlePanel.HandleChange();
        }

        private void DurationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || _eventHandle == null) return;

            _eventHandle.Duration = Convert.ToUInt16(_durationComboBox.SelectedItem);

            HandlePanel.HandleChange();
        }
    }
}
