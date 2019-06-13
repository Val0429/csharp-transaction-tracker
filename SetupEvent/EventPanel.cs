using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using PanelBase;

namespace SetupEvent
{
    public sealed class EventPanel : Panel
    {
        public event EventHandler OnEventEditClick;
        public event EventHandler OnIntervalEdit;

        private EventCondition _eventCondition;
        public EventCondition EventCondition
        {
            get
            {
                return _eventCondition;
            }
            set
            {
                _eventCondition = value;
                _intervalTextBox.TextChanged -= IntervalTextBoxTextChanged;
                if (_eventCondition == null) return;

                _intervalTextBox.Text = _eventCondition.Interval.ToString();
                _intervalTextBox.TextChanged += IntervalTextBoxTextChanged;
            }
        }

        public Dictionary<String, String> Localization;
        private readonly Label _intervalLabel;
        private readonly TextBox _intervalTextBox;
        public EventPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"EventPanel_TriggerInterval", "Trigger Interval(Sec)"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Size = new Size(350, 40);
            Padding = new Padding(200, 10, 0, 9);
            BackColor = Color.Transparent;

            _intervalLabel = new Label
            {
                Text = Localization["EventPanel_TriggerInterval"],
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                MinimumSize = new Size(80, 21),
                AutoSize = true,
            };
            _intervalTextBox = new PanelBase.HotKeyTextBox
            {
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Dock = DockStyle.Left,
                MaxLength = 4,
                Size = new Size(40, 21),
                ImeMode = ImeMode.Disable
            };
            _intervalTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            Controls.Add(_intervalTextBox);
            Controls.Add(_intervalLabel);

            MouseClick += EventPanelMouseClick;
            _intervalLabel.MouseClick += EventPanelMouseClick;

            Paint += EventPanelPaint;
        }

        private void IntervalTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt16 interval = (_intervalTextBox.Text != "") ? Convert.ToUInt16(_intervalTextBox.Text) : (UInt16)1;

            _eventCondition.Interval = Convert.ToUInt16(Math.Min(Math.Max(interval, (UInt16)0), (UInt16)3600));

            if (OnIntervalEdit != null)
                OnIntervalEdit(this, null);
        }

        private void EventPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, this, true);
            if (_editVisible){
                Manager.PaintEdit(g, this);
            }

            //Manager.PaintStatus(g, EventCondition.ReadyState);

            if (Width <= 300) return;

            Manager.PaintText(g, _eventCondition.CameraEvent.ToLocalizationString(), Brushes.Black);
        }

        private void EventPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (!_editVisible) return;

            if (OnEventEditClick != null)
                OnEventEditClick(this, e);
        }

        private Boolean _editVisible = true;
        public Boolean EditVisible { 
            set
            {
                _editVisible = value;
                Cursor = (value) ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        public Boolean IntervalVisible
        {
            set
            {
                if(value)
                {
                    Controls.Add(_intervalTextBox);
                    Controls.Add(_intervalLabel);
                }
                else
                {
                    Controls.Remove(_intervalLabel);
                    Controls.Remove(_intervalTextBox);
                }
            }
        }
    }
}
