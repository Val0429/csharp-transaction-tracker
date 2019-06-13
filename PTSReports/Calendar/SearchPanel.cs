using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using PanelBase;
using ApplicationForms = PanelBase.ApplicationForms;

namespace PTSReports.Calendar
{
	public sealed partial class SearchPanel : UserControl
	{
		public event EventHandler<EventArgs<ExceptionReportParameter>> OnExceptionSearchCriteriaChange;
		
		public IApp App;
		public IPTS PTS;
		public POS_Exception.SearchCriteria SearchCriteria;
		public Dictionary<String, String> Localization;

		public SearchPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"Common_UsedSeconds", "(%1 seconds)"},

								   {"Common_Monday", "Monday"},
								   {"Common_Tuesday", "Tuesday"},
								   {"Common_Wednesday", "Wednesday"},
								   {"Common_Thursday", "Thursday"},
								   {"Common_Friday", "Friday"},
								   {"Common_Saturday", "Saturday"},
								   {"Common_Sunday", "Sunday"},

								   {"Common_January", "January"},
								   {"Common_February", "February"},
								   {"Common_March", "March"},
								   {"Common_April", "April"},
								   {"Common_May", "May"},
								   {"Common_June", "June"},
								   {"Common_July", "July"},
								   {"Common_August", "August"},
								   {"Common_September", "September"},
								   {"Common_October", "October"},
								   {"Common_November", "November"},
								   {"Common_December", "December"},
								   
								   {"Common_NextMonth", "Next month"},
								   {"Common_PreviousMonth", "Previous month"},
								   {"PTSReports_SearchResult", "Search Result:"},
								   {"PTSReports_SearchNoResult", "No Result Found"},
							   };
			Localizations.Update(Localization);
			
			InitializeComponent();
            BackgroundImage = Manager.BackgroundNoBorder;
			DoubleBuffered = true;
			Dock = DockStyle.None;

			SharedToolTips.SharedToolTip.SetToolTip(nextPictureBox, Localization["Common_NextMonth"]);
			SharedToolTips.SharedToolTip.SetToolTip(previousPictureBox, Localization["Common_PreviousMonth"]);
			monthPanel.Paint += MonthPanelPaint;

			resultLabel.Text = Localization["PTSReports_SearchResult"];
		    resultLabel.Visible = false;
			foundLabel.Text = "";
			//tableLayoutPanel.CellPaint += TableLayoutPanelCellPaint;
		}

		private readonly Dictionary<UInt16, CalendarCell> _calendarDays = new Dictionary<UInt16, CalendarCell>();
		public void Initialize()
		{
			var range = DateTimes.UpdateStartAndEndDateTime(PTS.Server.DateTime, PTS.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Sunday"]), 0, 0);
			weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Monday"]), 1, 0);
			weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Tuesday"]), 2, 0);
			weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Wednesday"]), 3, 0);
			weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Thursday"]), 4, 0);
			weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Friday"]), 5, 0);
			weekTableLayoutPanel.Controls.Add(GetWeekDayLabel(Localization["Common_Saturday"]), 6, 0);
		}

		private Label GetWeekDayLabel(String weekday)
		{
			return new Label
			{
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Text = weekday,
				BackColor = Color.FromArgb(50, 51, 53),// Color.FromArgb(141, 180, 227),
				ForeColor = Color.FromArgb(224, 225, 227),
				TextAlign = ContentAlignment.MiddleLeft,
				Dock = DockStyle.Fill,
				Margin = new Padding(0),
				Padding = new Padding(8, 0, 0, 0)
			};
		}

		//private readonly Color _lightBlue = Color.FromArgb(216, 222, 233);
		//private void TableLayoutPanelCellPaint(Object sender, TableLayoutCellPaintEventArgs e)
		//{
		//    var g = e.Graphics;
		//    if (e.Row != 0)
		//    {
		//        g.DrawRectangle(new Pen(_lightBlue), e.CellBounds);
		//    }
		//}

		private static readonly Queue<Label> _recycleExceptionLabel = new Queue<Label>();
		private static Label GetExceptionLabel(String exception, Color color)
		{
			if (_recycleExceptionLabel.Count > 0)
			{
				var label = _recycleExceptionLabel.Dequeue();
                label.Text = POS_Exception.FindExceptionValueByKey(exception);
				label.BackColor = color;

				return label;
			}

			return new Label
			{
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Text = POS_Exception.FindExceptionValueByKey(exception),
				ForeColor = Color.White,
				BackColor = color,
				TextAlign = ContentAlignment.MiddleCenter,
				//Dock = DockStyle.Left,
				//BorderStyle = BorderStyle.FixedSingle,
				Margin = new Padding(3),
				Padding = new Padding(0),
				MinimumSize = new Size(125, 25),
				AutoSize = true
			};
		}

		private void MonthPanelPaint(object sender, PaintEventArgs e)
		{
			Manager.PaintTop(e.Graphics, monthPanel);
		}

		private Boolean _isSearching;

		private readonly Stopwatch _watch = new Stopwatch();
		
		private UInt64 _startTime;
		private UInt64 _endTime;
		public void SearchExceptions(UInt64 start, UInt64 end)
		{
			if (_isSearching) return;
            resultLabel.Visible = true;
			if (SearchCriteria.POS.Count == 0) return;
			if (SearchCriteria.Exceptions.Count == 0) return;

			_startTime = start;
			_endTime = end;
			ClearViewModel();

			ApplicationForms.ShowLoadingIcon(PTS.Form);

			_isSearching = true;
			_watch.Reset();
			_watch.Start();

			SearchExceptionByConditionDelegate searchDelegate = PTS.POS.ReadExceptionCalculationByMonth;
			searchDelegate.BeginInvoke(SearchCriteria.POS.ToArray(), _startTime, _endTime, SearchCriteria.Exceptions.ToArray(), SearchReportCallback, searchDelegate);
		}

        private delegate List<POS_Exception.ExceptionCount> SearchExceptionByConditionDelegate(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions);        
		private delegate void SearchReportCallbackDelegate(IAsyncResult result);
		private void SearchReportCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new SearchReportCallbackDelegate(SearchReportCallback), result);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			_watch.Stop();

			UpdateResult(((SearchExceptionByConditionDelegate)result.AsyncState).EndInvoke(result));
		}

		private List<POS_Exception.ExceptionCount> _searchResult;
		private void UpdateResult(List<POS_Exception.ExceptionCount> searchResult)
		{
			_searchResult = searchResult;

			ClearViewModel();
			if (_searchResult.Count == 0)
			{
				foundLabel.Text = Localization["PTSReports_SearchNoResult"] + @" ";
			}
			else
			{
				foundLabel.Text = "";
			}
			var exceptions = new List<String> ();
			exceptions.AddRange(SearchCriteria.Exceptions);
			exceptions.Sort((x, y) => (x.CompareTo(y)));
			
			foreach (var exception in exceptions)
			{
				if(PTS.POS.ExceptionThreshold.ContainsKey(exception))
					exceptionFlowLayoutPanel.Controls.Add(GetExceptionLabel(exception, PTS.POS.ExceptionThreshold[exception].Color));
			}

			//foundLabel.Text += @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));
			var dateTime = DateTimes.ToDateTime(_startTime, PTS.Server.TimeZone);
			var month = "";
			switch (dateTime.Month)
			{
				case 1:
					month = Localization["Common_January"];
					break;

				case 2:
					month = Localization["Common_February"];
					break;

				case 3:
					month = Localization["Common_March"];
					break;

				case 4:
					month = Localization["Common_April"];
					break;

				case 5:
					month = Localization["Common_May"];
					break;

				case 6:
					month = Localization["Common_June"];
					break;

				case 7:
					month = Localization["Common_July"];
					break;

				case 8:
					month = Localization["Common_August"];
					break;

				case 9:
					month = Localization["Common_September"];
					break;

				case 10:
					month = Localization["Common_October"];
					break;

				case 11:
					month = Localization["Common_November"];
					break;

				case 12:
					month = Localization["Common_December"];
					break;

			}
			monthLabel.Text = dateTime.Year + @" " + month;

			_isSearching = false;

			UpdateCell(dateTime);

			ApplicationForms.HideLoadingIcon();
		}

		public void UpdateCell(DateTime dateTime)
		{
			_calendarDays.Clear();

			var firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
			var offset = 0;
			switch (firstDayOfTheMonth.DayOfWeek)
			{
				case DayOfWeek.Sunday:
					offset = 0;
					break;

				case DayOfWeek.Monday:
					offset = 1;
					break;

				case DayOfWeek.Tuesday:
					offset = 2;
					break;

				case DayOfWeek.Wednesday:
					offset = 3;
					break;

				case DayOfWeek.Thursday:
					offset = 4;
					break;

				case DayOfWeek.Friday:
					offset = 5;
					break;

				case DayOfWeek.Saturday:
					offset = 6;
					break;
			}

			var days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
			//UInt16 start = 1;

			var rows = Convert.ToUInt16(Math.Ceiling((days - (7 - offset))/7.0) + 1);
			var height = (float) 100.0/rows;
			tableLayoutPanel.RowCount = rows;
			tableLayoutPanel.RowStyles.Clear();
			for (int i = 0; i < rows; i++)
			{
				tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, height));
			}

			var thisMonth = dateTime.Month;

			//var dateTimeUtc = DateTimes.ToUtc(dateTime, PTS.Server.TimeZone);
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < 7; x++)
				{
					var control = GetCalendarCell();
					tableLayoutPanel.Controls.Add(control, x, y);

					if (thisMonth != dateTime.Month)
					{
						control.Day = 0;
						control.IsToday = false;
						control.Exceptions = null;
						continue;
					}

					if (offset > 0)
					{
						control.Day = 0;
						control.IsToday = false;
						control.Exceptions = null;
						offset--;
						continue;
					}

					if (dateTime.Day > days)
					{
						control.Day = 0;
						control.IsToday = false;
						control.Exceptions = null;
						continue;
					}

					var exceptions = new List<POS_Exception.ExceptionCount>();
					foreach (var exceptionCount in _searchResult)
					{
						if (exceptionCount.DateTime == dateTime.ToString("yyyy-MM-dd"))
						{
							exceptions.Add(exceptionCount);
						}
					}

					//sort by count
					//exceptions.Sort((a, b) => ((b.Count != a.Count) ? b.Count - a.Count : b.Exception.CompareTo(a.Exception)));

					//default is sort by search exception order
					control.Exceptions = exceptions;

					control.Day = Convert.ToUInt16(dateTime.Day);
					control.IsToday = (dateTime.Year == PTS.Server.DateTime.Year && dateTime.Month == PTS.Server.DateTime.Month &&
									   dateTime.Date == PTS.Server.DateTime.Date);

					control.StartDateTime = DateTimes.ToUtc(dateTime, PTS.Server.TimeZone);//dateTimeUtc + (Convert.ToUInt64((dateTime.Day - 1) * 86400) * 1000);
					control.EndDateTime = DateTimes.ToUtc(dateTime.AddHours(24).AddMilliseconds(-1), PTS.Server.TimeZone);//control.StartDateTime + 86399000;
					dateTime = dateTime.AddDays(1);
				}
			}
		}

		public void ClearViewModel()
		{
			monthLabel.Text = "";
			foundLabel.Text = "";

			foreach (CalendarCell control in tableLayoutPanel.Controls)
			{
				if (!_recycleCalendarCell.Contains(control))
					_recycleCalendarCell.Enqueue(control);
			}

			tableLayoutPanel.Controls.Clear();

			foreach (Label label in exceptionFlowLayoutPanel.Controls)
			{
				if (!_recycleExceptionLabel.Contains(label))
					_recycleExceptionLabel.Enqueue(label);
			}
			exceptionFlowLayoutPanel.Controls.Clear();
		}

		private void PreviousPictureBoxClick(Object sender, EventArgs e)
		{
			var month = DateTimes.ToDateTime(_startTime, PTS.Server.TimeZone);
			month = month.AddMonths(-1);
			_startTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, 1), PTS.Server.TimeZone);
			_endTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), 23, 59, 59, 999), PTS.Server.TimeZone);

			SearchExceptions(_startTime, _endTime);
		}

		private void NextPictureBoxClick(Object sender, EventArgs e)
		{
			var month = DateTimes.ToDateTime(_startTime, PTS.Server.TimeZone);
			month = month.AddMonths(1);
			_startTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, 1), PTS.Server.TimeZone);
			_endTime = DateTimes.ToUtc(new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month), 23, 59, 59, 999), PTS.Server.TimeZone);

			SearchExceptions(_startTime, _endTime);
		}

		private readonly Queue<CalendarCell> _recycleCalendarCell = new Queue<CalendarCell>();
		private CalendarCell GetCalendarCell()
		{
			if (_recycleCalendarCell.Count > 0)
				return _recycleCalendarCell.Dequeue();

			var cell = new CalendarCell
			{
				ExceptionThreshold = PTS.POS.ExceptionThreshold,
			};
			
			//cell.OnSelectionChange += CellOnSelectionChange;
			cell.OnExceptionSearch += CellOnExceptionSearch;

			return cell;
		}

		//private void CellOnSelectionChange(Object sender, EventArgs e)
		//{
		//    foreach (CalendarCell control in tableLayoutPanel.Controls)
		//    {
		//        if (control == sender) continue;

		//        control.ClearSelected();
		//    }
		//}

		private void CellOnExceptionSearch(Object sender, EventArgs<String, UInt64, UInt64> e)
		{
			if (OnExceptionSearchCriteriaChange != null)
				OnExceptionSearchCriteriaChange(this, new EventArgs<ExceptionReportParameter>(new ExceptionReportParameter
				{
					POS = SearchCriteria.POS,
					StartDateTime = e.Value2,
					EndDateTime = e.Value3,
					DateTimeSet = DateTimeSet.None,
					Exceptions = new List<String> { e.Value1 }
				}));
		}
	}

	public sealed class CalendarCell : Panel
	{
		//public event EventHandler OnSelectionChange;
		public event EventHandler<EventArgs<String, UInt64, UInt64>> OnExceptionSearch;
		private readonly Label _dayLabel;
		//private readonly ListBox _listBox;
		private readonly DoubleBufferFlowLayoutPanel _resultPanel;

		public UInt64 StartDateTime;
		public UInt64 EndDateTime;

		public Dictionary<String, POS_Exception.ExceptionThreshold> ExceptionThreshold;
		public UInt16 Day
		{
			set
			{
				if (value == 0)
				{
					_dayLabel.Text = "";
					//_dayLabel.BackColor = Color.FromArgb(229, 229, 229);
					return;
				}
				_dayLabel.Text = value.ToString();

				//keep white
				//_dayLabel.BackColor = Color.FromArgb(219, 229, 241);
			}
		}

		public Boolean IsToday
		{
			set
			{
				if (value)
				{
					BackColor = Color.FromArgb(219, 241, 251);
				}
				else
				{
					if (String.IsNullOrEmpty(_dayLabel.Text))
						BackColor = Color.FromArgb(238, 239, 242);
					else
						BackColor = Color.White;
				}
			}
		}

		public List<POS_Exception.ExceptionCount> Exceptions
		{
			set
			{
				foreach (Label label in _resultPanel.Controls)
				{
					label.Click -= LabelClick;
					if (!_recycleExceptionLabel.Contains(label))
						_recycleExceptionLabel.Enqueue(label);
				}
				_resultPanel.Controls.Clear();
				//_listBox.Items.Clear();
				if (value == null) return;

				if(ExceptionThreshold != null)
				{
					foreach (var exceptionCount in value)
					{
						_resultPanel.Controls.Add(GetExceptionLabel(exceptionCount.Count,
							ExceptionThreshold[exceptionCount.Exception].Color, exceptionCount.Exception));
						//_listBox.Items.Add(exceptionCount.Exception.Key + ' ' + exceptionCount.Count);
					}
				}
			}
		}
		
		private static readonly Queue<Label> _recycleExceptionLabel = new Queue<Label>();
		private Label GetExceptionLabel(Int32 count, Color color, String exception)
		{
			Label label = null;
			if (_recycleExceptionLabel.Count > 0)
			{
				label = _recycleExceptionLabel.Dequeue();
				label.Text = count.ToString();//"99999"
				label.Tag = exception;//"99999"
				label.BackColor = color;
				label.Click += LabelClick;
                SharedToolTips.SharedToolTip.SetToolTip(label, POS_Exception.FindExceptionValueByKey(exception) + " : " + count);
				return label;
			}

			label =  new Label
			{
				Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
				Text = count.ToString(),//"99999"
				ForeColor = Color.White,
				BackColor = color,
				Tag = exception,
				TextAlign = ContentAlignment.MiddleCenter,
				BorderStyle = BorderStyle.FixedSingle,
				Margin = new Padding(0),
				Padding = new Padding(0),
				Size = new Size(44, 25),
				Cursor = Cursors.Hand,
			};

            SharedToolTips.SharedToolTip.SetToolTip(label, POS_Exception.FindExceptionValueByKey(exception) + " : " + count);
			label.Click += LabelClick; 

			return label;
		}

		public CalendarCell()
		{
			DoubleBuffered = true;
			Margin = new Padding(0);
			Dock = DockStyle.Fill;
			//BorderStyle = BorderStyle.FixedSingle;
			
			_dayLabel = new Label
			{
				BackColor = Color.Transparent,
				Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
				TextAlign = ContentAlignment.MiddleLeft,
				Margin = new Padding(0),
				Padding = new Padding(8, 0, 0, 0),
				Height = 20,
				Dock = DockStyle.Top,
				BorderStyle = BorderStyle.None,
			};

			_resultPanel = new DoubleBufferFlowLayoutPanel
			{
				BackColor = Color.Transparent,
				Dock = DockStyle.Fill,
				AutoScroll = true
								   
			};
			//_listBox = new ListBox
			//{
			//    BackColor = Color.White,
			//    Dock = DockStyle.Fill,
			//    Font = new Font("Lucida Console", 10F, FontStyle.Regular, GraphicsUnit.Point, 0),
			//    ForeColor = Color.Black,
			//    FormattingEnabled = true,
			//    BorderStyle = BorderStyle.None,
			//};

			//_listBox.Click += ListBoxClick;
			//_listBox.DoubleClick += ListBoxDoubleClick;

			//Controls.Add(_listBox);
			Controls.Add(_resultPanel);
			Controls.Add(_dayLabel);
		}

		private void LabelClick(Object sender, EventArgs e)
		{
			var label = sender as Label;
			if(label == null) return;

			if (OnExceptionSearch != null)
				OnExceptionSearch(this, new EventArgs<String, UInt64, UInt64>(label.Tag.ToString(), StartDateTime, EndDateTime));
		}

		//public void ClearSelected()
		//{
		//    _listBox.ClearSelected();
		//}

		//private void ListBoxClick(Object sender, EventArgs e)
		//{
		//    if (_listBox.SelectedItem == null) return;

		//    if (OnSelectionChange != null)
		//        OnSelectionChange(this, null);
		//}

		//private void ListBoxDoubleClick(Object sender, EventArgs e)
		//{
		//    if (_listBox.SelectedItem == null) return;

		//    if (OnExceptionSearch != null)
		//        OnExceptionSearch(this, new EventArgs<String, UInt64, UInt64>(_listBox.SelectedItem.ToString(), StartDateTime, EndDateTime));
		//}
	}
}
