using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.Exception
{
	public sealed partial class DateTimePanel : UserControl
	{
		public IPTS PTS;
		public Dictionary<String, String> Localization;
		public Exception ExceptionReport;

		public DateTimePanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Custom", "Custom"},
								   
								   {"Common_Today", "Today"},
								   {"Common_Yesterday", "Yesterday"},
								   {"Common_DayBeforeYesterday", "The day before yesterday"},
								   {"Common_ThisWeek", "This week"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
			Name = "DateTime";

			customPanel.Paint += CustomPanelPaint;

			startDatePicker.ValueChanged += StartDatePickerValueChanged;
			startTimePicker.ValueChanged += StartDatePickerValueChanged;

			endDatePicker.ValueChanged += EndDatePickerValueChanged;
			endTimePicker.ValueChanged += EndDatePickerValueChanged;

			BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
		}

		private readonly String[] _dateSetArray = new[] { "Today", "Yesterday", "DayBeforeYesterday", "ThisWeek" };
		public void Initialize()
		{
            Array.Reverse(_dateSetArray);
			foreach (String set in _dateSetArray)
			{
				if(!Localization.ContainsKey("Common_" + set)) continue;

				var dateSetPanel = new DateSetPanel
				{
					ExceptionReport = ExceptionReport,
					Tag = set,
					Name = Localization["Common_" + set]
				};

				dateSetPanel.MouseClick += DateSetPanelMouseClick;

				containerPanel.Controls.Add(dateSetPanel);
			}
		}

		private void DateSetPanelMouseClick(Object sender, MouseEventArgs e)
		{
		    _isEditing = false;

		    UpdateStartAndEndDateTime(((Control) sender).Tag.ToString());

		    startDatePicker.Value = startTimePicker.Value = DateTimes.ToDateTime(ExceptionReport.StartDateTime, PTS.Server.TimeZone);
            endDatePicker.Value = endTimePicker.Value = DateTimes.ToDateTime(ExceptionReport.EndDateTime, PTS.Server.TimeZone);

			((Control)sender).Focus();
            Invalidate();

            _isEditing = true;
		}

		private void CustomPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, customPanel);

			if (customPanel.Width <= 100) return;

			if (ExceptionReport.DateTimeSet == "")
			{
				Manager.PaintText(g, Localization["PTSReports_Custom"], Brushes.RoyalBlue);

				Manager.PaintSelected(g);
			}
			else
			{
				Manager.PaintText(g, Localization["PTSReports_Custom"]);
			}
		}

		private Boolean _isEditing;
		public void ParseSetting()
		{
			_isEditing = false;

            if (ExceptionReport.DateTimeSet != "")
                UpdateStartAndEndDateTime(ExceptionReport.DateTimeSet);

                startDatePicker.Value = startTimePicker.Value = DateTimes.ToDateTime(ExceptionReport.StartDateTime, PTS.Server.TimeZone);
            endDatePicker.Value = endTimePicker.Value = DateTimes.ToDateTime(ExceptionReport.EndDateTime, PTS.Server.TimeZone);

			_isEditing = true;
		}

        private void UpdateStartAndEndDateTime(String set)
        {
            var today = PTS.Server.DateTime;
            DateTime start = today;
            DateTime end = today;
            switch (set)
            {
                case "Today":
                    ExceptionReport.DateTimeSet = "Today";
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24).AddSeconds(-1);
                    break;

                case "Yesterday":
                    ExceptionReport.DateTimeSet = "Yesterday";
                    today = today.AddHours(-24);
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24).AddSeconds(-1);
                    break;

                case "DayBeforeYesterday":
                    ExceptionReport.DateTimeSet = "DayBeforeYesterday";
                    today = today.AddHours(-48);
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24).AddSeconds(-1);
                    break;

                case "ThisWeek":
                    ExceptionReport.DateTimeSet = "ThisWeek";
                    var dayofWeek = today.DayOfWeek;
                    var days = 0;
                    switch (dayofWeek)
                    {
                        case DayOfWeek.Monday:
                            days = 1;
                            break;

                        case DayOfWeek.Tuesday:
                            days = 2;
                            break;

                        case DayOfWeek.Wednesday:
                            days = 3;
                            break;

                        case DayOfWeek.Thursday:
                            days = 4;
                            break;

                        case DayOfWeek.Friday:
                            days = 5;
                            break;

                        case DayOfWeek.Saturday:
                            days = 6;
                            break;
                    }

                    today = today.AddHours(-24 * days);
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24 * (days + 1)).AddSeconds(-1);
                    break;
            }

            ExceptionReport.StartDateTime = DateTimes.ToUtc(start, PTS.Server.TimeZone);
            ExceptionReport.EndDateTime = DateTimes.ToUtc(end, PTS.Server.TimeZone);
        }

	    private void StartDatePickerValueChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			ExceptionReport.DateTimeSet = "";

			ExceptionReport.StartDateTime = DateTimes.ToUtc(
				new DateTime(startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day,
							 startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second),
				PTS.Server.TimeZone);

	        Invalidate();
		}

		private void EndDatePickerValueChanged(Object sender, EventArgs e)
		{
			if (!_isEditing) return;

			ExceptionReport.DateTimeSet = "";

			ExceptionReport.EndDateTime = DateTimes.ToUtc(
				new DateTime(endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day,
							 endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second),
                PTS.Server.TimeZone);

            Invalidate();
		}
	}

	public sealed class DateSetPanel : Panel
	{
		public Exception ExceptionReport;
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

			Manager.Paint(g, this);

			if (Width <= 100) return;
			if (Tag.ToString() == ExceptionReport.DateTimeSet)
			{
				Manager.PaintText(g, Name, Brushes.RoyalBlue);
				Manager.PaintSelected(g);
			}
			else
				Manager.PaintText(g, Name);
		}
	}
}
