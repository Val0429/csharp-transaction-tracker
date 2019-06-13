using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using DeviceConstant;
using Interface;
using PanelBase;

namespace Investigation.Base
{
    public sealed partial class DateTimePanel : UserControl
    {
        public INVR NVR;
        public Dictionary<String, String> Localization;
        public CameraEventSearchCriteria SearchCriteria;


        public DateTimePanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Investigation_Custom", "Custom"},

                                   {"Common_Today", "Today"},
                                   {"Common_Yesterday", "Yesterday"},
                                   {"Common_DayBeforeYesterday", "The day before yesterday"},
                                   {"Common_ThisWeek", "This week"},

                                   {"Common_ThisMonth", "This month"},
                                   {"Common_LastMonth", "Last month"},
                                   {"Common_TheMonthBeforeLast", "The month before last"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "DateTime";

            customPanel.Paint += CustomPanelPaint;

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public DateTimeSet[] DateSetArray = new DateTimeSet[] { 0 };

        public void Initialize()
        {
            Array.Reverse(DateSetArray);
            foreach (DateTimeSet set in DateSetArray)
            {
                if (!Localization.ContainsKey("Common_" + set)) continue;

                var dateSetPanel = new DateSetPanel
                {
                    Tag = set,
                    Name = Localization["Common_" + set]
                };

                dateSetPanel.MouseClick += DateSetPanelMouseClick;

                containerPanel.Controls.Add(dateSetPanel);
            }
        }

        private DateTimePicker _startTimePicker;
        private DateTimePicker _startDatePicker;
        private DateTimePicker _endTimePicker;
        private DateTimePicker _endDatePicker;
        private Label _splitLabel;
        public void ShowDateTimeSelectionRange()
        {
            _startTimePicker = new DateTimePicker
            {
                Anchor = AnchorStyles.Right,
                CustomFormat = DateTimeConverter.GetTimePattern(),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Format = DateTimePickerFormat.Custom,
                Location = new Point(170, 10),
                Margin = new Padding(0),
                ShowUpDown = true,
                Size = new Size(75, 21)
            };

            _startDatePicker = new DateTimePicker
            {
                Anchor = AnchorStyles.Right,
                CustomFormat = DateTimeConverter.GetDatePattern(),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Format = DateTimePickerFormat.Custom,
                Location = new Point(217, 10),
                Margin = new Padding(0),
                Size = new Size(103, 21)
            };

            _endTimePicker = new DateTimePicker
            {
                Anchor = AnchorStyles.Right,
                CustomFormat = DateTimeConverter.GetTimePattern(),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Format = DateTimePickerFormat.Custom,
                Location = new Point(374, 10),
                Margin = new Padding(0),
                ShowUpDown = true,
                Size = new Size(75, 21)
            };

            _endDatePicker = new DateTimePicker
            {
                Anchor = AnchorStyles.Right,
                CustomFormat = DateTimeConverter.GetDatePattern(),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Format = DateTimePickerFormat.Custom,
                Location = new Point(346, 10),
                Margin = new Padding(0),
                Size = new Size(103, 21)
            };

            _splitLabel = new Label
            {
                Anchor = AnchorStyles.Right,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.Black,
                Location = new Point(323, 10),
                Size = new Size(22, 21),
                TabIndex = 18,
                Text = @"~",
                TextAlign = ContentAlignment.MiddleCenter
            };

            customPanel.Controls.Add(_splitLabel);
            customPanel.Controls.Add(_endDatePicker);
            customPanel.Controls.Add(_startDatePicker);

            _startDatePicker.ValueChanged += StartDatePickerValueChanged;
            _endDatePicker.ValueChanged += EndDatePickerValueChanged;
        }

        public void ShowTmeSelection()
        {
            _startDatePicker.Location = new Point(62, 10);
            _endDatePicker.Location = new Point(266, 10);
            _splitLabel.Location = new Point(245, 10);

            customPanel.Controls.Add(_endTimePicker);
            customPanel.Controls.Add(_startTimePicker);

            _startTimePicker.ValueChanged += StartDatePickerValueChanged;
            _endTimePicker.ValueChanged += EndDatePickerValueChanged;
        }

        public void ShowOnlyMonthSelection()
        {
            _startDatePicker.CustomFormat = DateTimeConverter.GetShortYearMonthPattern();
            _startDatePicker.Size = new Size(80, 21);
            _startDatePicker.Location = new Point(370, 10);

            customPanel.Controls.Remove(_splitLabel);
            customPanel.Controls.Remove(_endDatePicker);

            _startDatePicker.ValueChanged -= StartDatePickerValueChanged;
            _endDatePicker.ValueChanged -= EndDatePickerValueChanged;

            _startDatePicker.ValueChanged += StartMonthPickerValueChanged;
        }

        private void DateSetPanelMouseClick(Object sender, MouseEventArgs e)
        {
            _isEditing = false;

            var set = (DateTimeSet)((Control)sender).Tag;
            SearchCriteria.DateTimeSet = set;
            var range = DateTimes.UpdateStartAndEndDateTime(NVR.Server.DateTime, NVR.Server.TimeZone, set);
            SearchCriteria.StartDateTime = range[0];
            SearchCriteria.EndDateTime = range[1];

            if (_startDatePicker != null)
                _startDatePicker.Value = DateTimes.ToDateTime(SearchCriteria.StartDateTime, NVR.Server.TimeZone);

            if (_endDatePicker != null)
                _endDatePicker.Value = DateTimes.ToDateTime(SearchCriteria.EndDateTime, NVR.Server.TimeZone);

            ((Control)sender).Focus();
            Invalidate();

            _isEditing = true;
        }

        private void CustomPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, customPanel);

            if (customPanel.Width <= 100) return;

            if (SearchCriteria.DateTimeSet == DateTimeSet.None)
            {
                Manager.PaintText(g, Localization["Investigation_Custom"], Manager.SelectedTextColor);

                Manager.PaintSelected(g);
            }
            else
            {
                Manager.PaintText(g, Localization["Investigation_Custom"]);
            }
        }

        private Boolean _isEditing;
        public void ParseSetting()
        {
            foreach (DateSetPanel dateTimeSet in containerPanel.Controls)
            {
                dateTimeSet.SearchCriteria = SearchCriteria;
            }

            _isEditing = false;

            if (SearchCriteria.DateTimeSet != DateTimeSet.None)
            {
                var range = DateTimes.UpdateStartAndEndDateTime(NVR.Server.DateTime, NVR.Server.TimeZone, SearchCriteria.DateTimeSet);
                SearchCriteria.StartDateTime = range[0];
                SearchCriteria.EndDateTime = range[1];
            }

            if (_startDatePicker != null)
            {
                _startDatePicker.Value = DateTimes.ToDateTime(SearchCriteria.StartDateTime, NVR.Server.TimeZone);
                if (_startTimePicker != null)
                {
                    _startTimePicker.Value = _startDatePicker.Value;
                }
            }

            if (_endDatePicker != null)
            {
                _endDatePicker.Value = DateTimes.ToDateTime(SearchCriteria.EndDateTime, NVR.Server.TimeZone);
                if (_endTimePicker != null)
                {
                    _endTimePicker.Value = _endDatePicker.Value;
                }
            }

            _isEditing = true;
        }

        private void StartDatePickerValueChanged(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            SearchCriteria.DateTimeSet = DateTimeSet.None;

            SearchCriteria.StartDateTime = DateTimes.ToUtc(new DateTime(_startDatePicker.Value.Year, _startDatePicker.Value.Month, _startDatePicker.Value.Day,
                _startTimePicker.Value.Hour, _startTimePicker.Value.Minute, _startTimePicker.Value.Second, 0),
                NVR.Server.TimeZone);

            Invalidate();
        }

        private void EndDatePickerValueChanged(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            SearchCriteria.DateTimeSet = DateTimeSet.None;

            SearchCriteria.EndDateTime = DateTimes.ToUtc(new DateTime(_endDatePicker.Value.Year, _endDatePicker.Value.Month, _endDatePicker.Value.Day,
                _endTimePicker.Value.Hour, _endTimePicker.Value.Minute, _endTimePicker.Value.Second, 999),
                NVR.Server.TimeZone);

            Invalidate();
        }

        private void StartMonthPickerValueChanged(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            SearchCriteria.DateTimeSet = DateTimeSet.None;

            var start = new DateTime(_startDatePicker.Value.Year, _startDatePicker.Value.Month, 1);
            var end = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));

            SearchCriteria.StartDateTime = DateTimes.ToUtc(start, NVR.Server.TimeZone);
            SearchCriteria.EndDateTime = DateTimes.ToUtc(end, NVR.Server.TimeZone);

            Invalidate();
        }
    }

    public sealed class DateSetPanel : Panel
    {
        public CameraEventSearchCriteria SearchCriteria;
        public DateSetPanel()
        {
            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += DateSetPanelPaint;
        }

        private void DateSetPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (Parent.Controls[0] == this)
            {
                Manager.PaintBottom(g, this);
            }
            else
            {
                Manager.PaintMiddle(g, this);
            }

            if (Width <= 100) return;

            if (SearchCriteria != null && (DateTimeSet)Tag == SearchCriteria.DateTimeSet)
            {
                Manager.PaintText(g, Name, Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, Name);
        }
    }
}
