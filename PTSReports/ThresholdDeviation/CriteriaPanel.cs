using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using PTSReportsGenerator;
using PTSReportsGenerator.B3Report;
using SetupBase;

namespace PTSReports.ThresholdDeviation
{
	public sealed partial class CriteriaPanel : UserControl
	{
		public event EventHandler OnPOSEdit;
		public event EventHandler OnExceptionEdit;
		public event EventHandler<EventArgs<ExceptionReportParameter>> OnExceptionSearchCriteriaChange;

		public IPTS PTS;
		public IApp App;
		public Dictionary<String, String> Localization;
		public POS_Exception.SearchCriteria SearchCriteria;

		public CriteriaPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Common_UsedSeconds", "(%1 seconds)"},
								   
								   {"PTSReports_SaveReport", "Save Report"},

								   {"PTSReports_POS", "POS"},
								   {"PTSReports_AllPOS", "All POS"},

								   {"PTSReports_Period", "Period"},
								   {"PTSReports_Daily", "Daily"},
								   {"PTSReports_Weekly", "Weekly"},
								   {"PTSReports_Monthly", "Monthly"},
								   {"PTSReports_Rank", "Rank"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_Threshold", "Threshold"},
								   {"PTSReports_Difference", "Difference"},
								   {"PTSReports_Summary", "Summary"},
								   {"PTSReports_Count", "Count"},
								   
								   {"PTSReports_SearchResult", "Search Result"},
							   };
			Localizations.Update(Localization);

			Name = "ReportSummary";

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;

			resultLabel.Text = Localization["PTSReports_SearchResult"];
            resultLabel.Visible = false;
			foundLabel.Text = "";
		}
		
		public void Initialize()
		{
			summaryTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Rank"]), 0, 0);
			summaryTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Exception"]), 1, 0);

			titleTableLayoutPanel.Paint += TitleTableLayoutPanelPaint;

			posDoubleBufferPanel.Paint += POSDoubleBufferPanelPaint;
			posDoubleBufferPanel.MouseClick += POSDoubleBufferPanelMouseClick;

			periodDoubleBufferPanel.Paint += PeriodDoubleBufferPanelPaint;

			exceptionDoubleBufferPanel.Paint += ExceptionDoubleBufferPanelPaint;
			exceptionDoubleBufferPanel.MouseClick += ExceptionDoubleBufferPanelMouseClick;

			periodComboBox.Items.Add(Localization["PTSReports_Daily"]);
			periodComboBox.Items.Add(Localization["PTSReports_Weekly"]);
			periodComboBox.Items.Add(Localization["PTSReports_Monthly"]);
			periodComboBox.SelectedIndex = 1;

			periodComboBox.SelectedIndexChanged += PeriodComboBoxSelectedIndexChanged;
		}

		public void ResetPeriod()
		{
			periodComboBox.SelectedIndexChanged -= PeriodComboBoxSelectedIndexChanged;
			periodComboBox.SelectedIndex = 1;
			periodComboBox.SelectedIndexChanged += PeriodComboBoxSelectedIndexChanged;
		}

		private void PeriodComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			switch (periodComboBox.SelectedIndex)
			{
				case 0:
					SearchCriteria.DateTimeSet = DateTimeSet.Daily;
					break;

				case 1:
					SearchCriteria.DateTimeSet = DateTimeSet.Weekly;
					break;

				case 2:
					SearchCriteria.DateTimeSet = DateTimeSet.Monthly;
					break;
			}

			if (SearchCriteria.POS.Count == 0)
				return;

			if (SearchCriteria.Exceptions.Count == 0)
				return;

			//SearchExceptionThresholdDeviationReport();
		}

		private Boolean _isSearch;
		private readonly Stopwatch _watch = new Stopwatch();
		public void SearchExceptionThresholdDeviationReport()
		{
			if (_isSearch) return;

			//no pos select
			if (SearchCriteria.POS.Count == 0) return;

            resultLabel.Visible = true;

			var periods = new List<UInt64[]>();

			switch (SearchCriteria.DateTimeSet)
			{
				case DateTimeSet.Daily:
					periods = DateTimes.GetTimeDailyPeriods(PTS.Server.DateTime, PTS.Server.TimeZone);
					break;

				case DateTimeSet.Weekly:
					periods = DateTimes.GetTimeWeeklyPeriods(PTS.Server.DateTime, PTS.Server.TimeZone);
					break;

				case DateTimeSet.Monthly:
					periods = DateTimes.GetTimeMonthlyPeriods(PTS.Server.DateTime, PTS.Server.TimeZone);
					break;
			}

			if (periods.Count == 0) return;

			ApplicationForms.ShowLoadingIcon(PTS.Form);
			Application.RaiseIdle(null);

			_isSearch = true;
			tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = false;

			_watch.Reset();
			_watch.Start();
			ReadDailyReportByStationGroupByExceptionDelegate loadReportDelegate = ReadDailyReportByStationGroupByException;
			loadReportDelegate.BeginInvoke(
				SearchCriteria.POS.ToArray(),
				SearchCriteria.Exceptions.ToArray(), periods, null, null);
		}

		private String _dateTime1;
		private String _dateTime2;
		private String _dateTime3;
		private readonly List<List<String>> _rowData = new List<List<String>>();
        private delegate void ReadDailyReportByStationGroupByExceptionDelegate(String[] posIds, String[] exceptions, List<UInt64[]> periods);
        private void ReadDailyReportByStationGroupByException(String[] posIds, String[] exceptions, List<UInt64[]> periods)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new ReadDailyReportByStationGroupByExceptionDelegate(ReadDailyReportByStationGroupByException), posIds, exceptions, periods);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = true;
			titleTableLayoutPanel.Controls.Clear();
			//titleTableLayoutPanel.RowStyles.Clear();

			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Rank"]), 0, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Exception"]), 1, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Count"]),2, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Threshold"]),3, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Difference"]), 4, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Count"]), 5, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Threshold"]), 6, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Difference"]), 7, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Count"]), 8, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Threshold"]), 9, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Difference"]), 10, 1);
			
			var summaryExceptionCount = new Dictionary<String, Int32>();

			var index = 2;
			_dateTime1 = _dateTime2 = _dateTime3 = "";
			var resultList = new Dictionary<UInt64[], POS_Exception.ExceptionCountWithThresholdList>();
			foreach (var period in periods)
			{
				var result = PTS.POS.ReadDailyReportByStationGroupByException(posIds, period[0], period[1], exceptions);
				var exceptionList = new List<POS_Exception.ExceptionCountWithThreshold>();
				foreach (var exceptionCount in result.ExceptionList)
				{
					var threshold = PTS.POS.ExceptionThreshold.ContainsKey(exceptionCount.Exception) 
						? PTS.POS.ExceptionThreshold[exceptionCount.Exception]
						: new POS_Exception.ExceptionThreshold();

					exceptionList.Add(new POS_Exception.ExceptionCountWithThreshold
					{
						Exception = exceptionCount.Exception,
						Count = exceptionCount.Count,
						DateTime = exceptionCount.DateTime,
						Threshold = threshold
					});
				}

				resultList.Add(period, new POS_Exception.ExceptionCountWithThresholdList
				{
					POSIds = result.POSIds,
					StartDateTime = result.StartDateTime,
					EndDateTime = result.EndDateTime,
					ExceptionList = exceptionList
				});

				var start = DateTimes.ToDateTime(period[0], PTS.Server.TimeZone);
				var end = DateTimes.ToDateTime(period[1], PTS.Server.TimeZone);

				String range;
				switch (SearchCriteria.DateTimeSet)
				{
					case DateTimeSet.Daily:
						range = start.ToString("MM-dd-yyyy");
						break;

					default:
						range = (start.ToString("MM-dd-yyyy") + @" ~ " + end.ToString("MM-dd-yyyy"));
						break;
				}

				if (String.IsNullOrEmpty(_dateTime1))
					_dateTime1 = range;
				else if (String.IsNullOrEmpty(_dateTime2))
					_dateTime2 = range;
				else if (String.IsNullOrEmpty(_dateTime3))
					_dateTime3 = range;

				var rangeLabel = GetTableLabel(range);
				titleTableLayoutPanel.Controls.Add(rangeLabel, index, 0);
				titleTableLayoutPanel.SetColumnSpan(rangeLabel, 3);
				index += 3;

				//add each range's data to summary);
				foreach (var exceptionCount in result.ExceptionList)
				{
					if(!summaryExceptionCount.ContainsKey(exceptionCount.Exception))
						summaryExceptionCount.Add(exceptionCount.Exception, 0);

					summaryExceptionCount[exceptionCount.Exception] += exceptionCount.Count;
				}
			}

			var orderByCount = new List<POS_Exception.ExceptionCountWithThreshold>();
			foreach (var exception in summaryExceptionCount)
			{
				var threshold = PTS.POS.ExceptionThreshold.ContainsKey(exception.Key)
					? PTS.POS.ExceptionThreshold[exception.Key]
					: new POS_Exception.ExceptionThreshold();

				orderByCount.Add(new POS_Exception.ExceptionCountWithThreshold
				{
					Count = exception.Value,
					Exception = exception.Key,
					Threshold = threshold
				});
			}
			orderByCount.Sort(SortByThresholdThenCountThemException);

			//after sorting, create xml for report-viewer
			//---------------------------------------------------------------------------------------------

			//add table style
			summaryTableLayoutPanel.Controls.Clear();
			summaryTableLayoutPanel.RowStyles.Clear();
			_rowData.Clear();

			if(summaryExceptionCount.Count == 0)
			{
				tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = false;
				summaryTableLayoutPanel.RowCount = 0;
				summaryTableLayoutPanel.Height = 0;
				SearchCompleted();
				return;
			}

			var rows = summaryExceptionCount.Count;
			//var height = (float)100.0 / rows;
			summaryTableLayoutPanel.RowCount = rows + 1; //exception's count + total
			summaryTableLayoutPanel.Height = 30 * summaryTableLayoutPanel.RowCount + (summaryTableLayoutPanel.RowCount + 1); // height + border

			for (int i = 0; i < rows; i++)
			{
				summaryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			}

			//----------------------------------------------------------------------------------------------
			for (int y = 0; y <  rows; y++)
			{
				var currentException = orderByCount[y];

				var row = new List<String>();
				_rowData.Add(row);

				//NO.
				summaryTableLayoutPanel.Controls.Add(GetTableLabel((y + 1).ToString()), 0, y);
				//Exception Name
                summaryTableLayoutPanel.Controls.Add(GetTableLabel(POS_Exception.FindExceptionValueByKey(currentException.Exception)), 1, y);

				row.Add((y + 1).ToString());
                row.Add(POS_Exception.FindExceptionValueByKey(currentException.Exception));

				index = 2;
				foreach (var exceptionCountList in resultList)
				{
					var hasValue = false;
					foreach (var exceptionCount in exceptionCountList.Value.ExceptionList)
					{
						if (exceptionCount.Exception != currentException.Exception) continue;

						//var diff = exceptionCount.Count - exceptionCount.Threshold.ThresholdValue1;

						summaryTableLayoutPanel.Controls.Add(
							GetExceptionTableLabel(exceptionCount.Exception, exceptionCount.Count,
												   exceptionCount.Threshold.ThresholdValue1, exceptionCount.Threshold.ThresholdValue2,
												   exceptionCountList.Key[0], exceptionCountList.Key[1]), index, y);

						row.Add(exceptionCount.Count.ToString());

						var label1 = GetTableLabel(exceptionCount.Threshold.ThresholdValue1.ToString());
						if (exceptionCount.Count > exceptionCount.Threshold.ThresholdValue2)
						{
							label1.Text = exceptionCount.Threshold.ThresholdValue2.ToString();
							label1.BackColor = _pinkColor;
						}
						else if (exceptionCount.Count > exceptionCount.Threshold.ThresholdValue1)
						{
							label1.BackColor = _yellowColor;
						}
						summaryTableLayoutPanel.Controls.Add(label1, index + 1, y);
						row.Add(label1.Text);

						var label2 = GetTableLabel((exceptionCount.Count - exceptionCount.Threshold.ThresholdValue1).ToString());
						if (exceptionCount.Count > exceptionCount.Threshold.ThresholdValue2)
						{
							label2.Text = (exceptionCount.Count - exceptionCount.Threshold.ThresholdValue2).ToString();
							label2.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
							label2.ForeColor = Color.Red;
							label2.BackColor = _pinkColor;
						}
						else if (exceptionCount.Count > exceptionCount.Threshold.ThresholdValue1)
						{
							label2.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
							label2.ForeColor = Color.Red;
							label2.BackColor = _yellowColor;
						}

						summaryTableLayoutPanel.Controls.Add(label2, index + 2, y);
						row.Add(label2.Text);
						hasValue = true;
						break;
					}

					if (!hasValue)
					{
						summaryTableLayoutPanel.Controls.Add(GetTableLabel("0"), index + 1, y);

						row.Add("0");
					}

					index+=3;
				}
			}

			var summaryrow = new List<String>();
			_rowData.Add(summaryrow);
			summaryTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Summary"]), 1, rows);
			summaryrow.Add("");
			summaryrow.Add(Localization["PTSReports_Summary"]);

			index = 2;
			foreach (var exceptionCountList in resultList)
			{
				var count = exceptionCountList.Value.ExceptionList.Sum(exceptionCount => exceptionCount.Count);
				var threshold1 = exceptionCountList.Value.ExceptionList.Sum(exceptionCount => exceptionCount.Threshold.ThresholdValue1);
				var threshold2 = exceptionCountList.Value.ExceptionList.Sum(exceptionCount => exceptionCount.Threshold.ThresholdValue2);

				//var diff = count - threshold;

				summaryTableLayoutPanel.Controls.Add(
					GetExceptionTableLabel(null, count, threshold1, threshold2, exceptionCountList.Key[0], exceptionCountList.Key[1]), index, rows);

				summaryrow.Add(count.ToString());
				index++;

				var label1 = GetTableLabel(threshold1.ToString());
				if (count > threshold2)
				{
					label1.Text = threshold2.ToString();
					label1.BackColor = _pinkColor;
				}
				else if (count > threshold1)
				{
					label1.BackColor = _yellowColor;
				}

				summaryTableLayoutPanel.Controls.Add(label1, index, rows);
				summaryrow.Add(label1.Text);

				index++;

				var label2 = GetTableLabel((count - threshold1).ToString());
				if (count > threshold2)
				{
					label2.Text = (count - threshold2).ToString();
					label2.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
					label2.ForeColor = Color.Red;
					label2.BackColor = _pinkColor;
				}
				else if (count > threshold1)
				{
					label2.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
					label2.ForeColor = Color.Red;
					label2.BackColor = _yellowColor;
				}

				summaryTableLayoutPanel.Controls.Add(label2, index, rows);
				summaryrow.Add(label2.Text);
				index++;
			}

			SearchCompleted();
		}

		private void SearchCompleted()
		{
			_watch.Stop();

			foundLabel.Text = "";//Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

			ApplicationForms.HideLoadingIcon();

			_isSearch = false;
		}
		
		public void SaveReport()
		{
			//no data
			if (_rowData.Count == 0) return;

			var saveReportForm = new SaveReportForm
			{
				Icon = PTS.Form.Icon,
				Text = Localization["PTSReports_SaveReport"],
				Size = new Size(1095, 450)
			};


			var reportViewer = new PTSReportViewer {ShowZoomControl = false};
			var parser = new B3ReportParser { LocalReport = reportViewer.LocalReport };
			parser.ParseB3Report();
			saveReportForm.Controls.Add(reportViewer);

			reportViewer.LocalReport.SetParameters(new[]
			{
				new Microsoft.Reporting.WinForms.ReportParameter("Rank", Localization["PTSReports_Rank"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Exception", Localization["PTSReports_Exception"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Count", Localization["PTSReports_Count"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Threshold", Localization["PTSReports_Threshold"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Difference", Localization["PTSReports_Difference"]),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime1", _dateTime1),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime2", _dateTime2),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime3", _dateTime3),
			});
			parser.SetB3Report(_rowData);
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}

		private readonly Color _pinkColor = Color.FromArgb(242, 220, 219);
		private readonly Color _yellowColor = Color.FromArgb(240, 240, 108);
		
		private static Int32 SortByThresholdThenCountThemException(POS_Exception.ExceptionCountWithThreshold x, POS_Exception.ExceptionCountWithThreshold y)
		{
			var diffx =  Math.Max(x.Count - x.Threshold.ThresholdValue1, 0);
			var diffy = Math.Max(y.Count - y.Threshold.ThresholdValue1, 0);
			if (diffx != diffy)
				return (diffy - diffx);

			if (x.Count != y.Count)
				return (y.Count - x.Count);

			return x.Exception.CompareTo(y.Exception);
		}

		private void POSDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, posDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_POS"]);

			Manager.PaintEdit(g, posDoubleBufferPanel);

			var containesAll = true;
			foreach (var pos in PTS.POS.POSServer)
			{
				if (!SearchCriteria.POS.Contains(pos.Id))
				{
					containesAll = false;
					break;
				}
			}

			if (containesAll && PTS.POS.POSServer.Count > 3)
			{
				Manager.PaintTextRight(g, posDoubleBufferPanel, Localization["PTSReports_AllPOS"]);
			}
			else
			{
				var posSelecton = new List<String>();

				var list = new List<String>();

				foreach (var posId in SearchCriteria.POS)
				{
					var pos = PTS.POS.FindPOSById(posId);
					if (pos != null)
						list.Add(pos.ToString());
				}

				list.Sort();

				foreach (var pos in list)
				{
					if (posSelecton.Count >= 3)
					{
						posSelecton[2] += " ...";
						break;
					}

					if (String.IsNullOrEmpty(pos)) continue;

					posSelecton.Add(pos);
				}

				Manager.PaintTextRight(g, posDoubleBufferPanel, String.Join(", ", posSelecton.ToArray()));
			}
		}

		private void TitleTableLayoutPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintTop(g, titleTableLayoutPanel);
		}

		private void PeriodDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, periodDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_Period"]);
		}

		private void ExceptionDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, exceptionDoubleBufferPanel);

			Manager.PaintText(g, Localization["PTSReports_Exception"]);

			Manager.PaintEdit(g, exceptionDoubleBufferPanel);

			var exceptionSelecton = new List<String>();

			var list = new List<String>();

			foreach (var exception in SearchCriteria.Exceptions)
			{
				list.Add(exception);
			}

			list.Sort();

			foreach (var exception in list)
			{
				if (exceptionSelecton.Count >= 3)
				{
					exceptionSelecton[2] += " ...";
					break;
				}

				if (String.IsNullOrEmpty(exception)) continue;

                exceptionSelecton.Add(POS_Exception.FindExceptionValueByKey(exception));
			}

			Manager.PaintTextRight(g, exceptionDoubleBufferPanel, String.Join(", ", exceptionSelecton.ToArray()));
		}

		private void POSDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPOSEdit != null)
				OnPOSEdit(this, null);
		}

		private void ExceptionDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnExceptionEdit != null)
				OnExceptionEdit(this, null);
		}

		private static Label GetTableLabel(String value)
		{
			return new Label
			{
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Text = value,
				BackColor = Color.Transparent,// Color.FromArgb(141, 180, 227),
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Fill,
				Margin = new Padding(0),
				Padding = new Padding(0, 0, 0, 0),
				Cursor = Cursors.Default
			};
		}

		private Label GetExceptionTableLabel(String exception, Int32 count, Int32 threshold1, Int32 threshold2, UInt64 start, UInt64 end)
		{
			var label = new ExceptionLabel
			{
				Text = count.ToString(),
				Exception = exception,
				StartDateTime = start,
				EndDateTime = end,
			};

			if (count != 0)
			{
				if (count > threshold2)
					label.BackColor = _pinkColor;
				else if (count > threshold1)
					label.BackColor = _yellowColor;

				label.Cursor = Cursors.Hand;
				label.MouseClick += LabelMouseClick;
			}

			return label;
		}

		private void LabelMouseClick(Object sender, MouseEventArgs e)
		{
			var label = sender as ExceptionLabel;
			if (label == null) return;

			var exceptions = new List<String>();
			if (!String.IsNullOrEmpty(label.Exception))
				exceptions.Add(label.Exception);
			else
				exceptions.AddRange(SearchCriteria.Exceptions.ToArray());
			
			if (OnExceptionSearchCriteriaChange != null)
				OnExceptionSearchCriteriaChange(this, new EventArgs<ExceptionReportParameter>(new ExceptionReportParameter
				{
					POS = SearchCriteria.POS,
					StartDateTime = label.StartDateTime,
					EndDateTime = label.EndDateTime,
					DateTimeSet = DateTimeSet.None,
					Exceptions = exceptions
				}));

			//Console.WriteLine(label.Text + " " + label.Exception + " " + label.StartDateTime + " " + label.EndDateTime);
		}

		public sealed class ExceptionLabel : Label
		{
			public String Exception;
			public UInt64 StartDateTime;
			public UInt64 EndDateTime;

			public ExceptionLabel()
			{
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
				BackColor = Color.Transparent;// Color.FromArgb(141, 180, 227),
				TextAlign = ContentAlignment.MiddleCenter;
				Dock = DockStyle.Fill;
				Margin = new Padding(0);
				Padding = new Padding(0, 0, 0, 0);
				Cursor = Cursors.Default;
			}
		}
	}
}
