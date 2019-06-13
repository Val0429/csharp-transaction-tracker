using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
    public sealed class PointSelectPanel : Panel
    {
        public Dictionary<String, String> Localization;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        private readonly ComboBox _pointComboBox;
        private readonly HotKeyTextBox _durationTextBox;

        private readonly CheckBox _checkBox = new CheckBox();

        public IServer Server;
        private Boolean _isTitle;
        public Boolean IsTitle {
            get { return _isTitle; }
            set
            {
                _isTitle = value;
                _pointComboBox.Visible = _durationTextBox.Visible = !value;
            }
        }
        public ICamera Camera;
        private TourPoint _tourPoint;
        public TourPoint TourPoint
        {
            get { return _tourPoint; }
            set
            {
                _tourPoint = value;
                if(_tourPoint == null) return;

                _isEditing = false;
                
                    _durationTextBox.Text = _tourPoint.Duration.ToString();
                _pointComboBox.Items.Clear();
                var selection = -1;

                var points = Camera.PresetPoints.Values.OrderBy(g => g.Id);

                foreach (var point in points)
                {
                    var index = _pointComboBox.Items.Add(point.ToString());
                    if (point.Id == _tourPoint.Id)
                        selection = index;
                }
                _pointComboBox.SelectedIndex = selection;

                _isEditing = true;
            }
        }

        private Boolean _isEditing;
        public PointSelectPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"PointPanel_PresetPoint", "Preset Point"},
                                   {"PointPanel_Duration", "Duration (Sec)"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Default;
            Height = 40;
            BackColor = Color.Transparent;

            _pointComboBox = new ComboBox
            {
                Width = 120,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(44, 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.System,
            };

            _durationTextBox = new HotKeyTextBox
            {
                MaxLength = 5,
                ImeMode = ImeMode.Disable,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(180, 10),
                Size = new Size(40, 22),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };

            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;


            Controls.Add(_checkBox);
            Controls.Add(_pointComboBox);
            Controls.Add(_durationTextBox);

            _durationTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;
            _durationTextBox.TextChanged += DurationTextBoxTextChanged;
            _pointComboBox.SelectedIndexChanged += PointComboBoxSelectedIndexChanged;
            Paint += PointPanelPaint;
        }

        public Boolean Checked
        {
            get
            {
                return _checkBox.Checked;
            }
            set
            {
                _checkBox.Checked = value;
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (IsTitle)
            {
                if (Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }

            _checkBox.Focus();
            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        private void PointComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || Camera == null || _tourPoint == null) return;

            var pointId = _pointComboBox.SelectedItem.ToString().Split(' ')[0];

            _tourPoint.Id = Convert.ToUInt16(pointId);
        }

        private void DurationTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEditing || Camera == null || _tourPoint == null) return;

            UInt32 duraion = (_durationTextBox.Text != "") ? Convert.ToUInt32(_durationTextBox.Text) : 5;

            _tourPoint.Duration = Convert.ToUInt16(Math.Min(Math.Max(duraion, 1), 65535));

            //dont save device setting just because preset is change
            Camera.PresetTours.IsModify = true;
            Camera.ReadyState = ReadyState.Modify;
            Server.DeviceModify(Camera);
        }

        private void PointPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                return;
            }

            Manager.Paint(g, this);
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;
            Manager.PaintTitleText(g, Localization["PointPanel_PresetPoint"]);

            if (Width <= 260) return;
            g.DrawString(Localization["PointPanel_Duration"], Manager.Font, Manager.TitleTextColor, 180, 13);
        }
    }
}
