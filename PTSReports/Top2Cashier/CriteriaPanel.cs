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
using PTSReportsGenerator.C1Report;
using SetupBase;

namespace PTSReports.Top2Cashier
{
	public sealed partial class CriteriaPanel : UserControl
	{
		public event EventHandler<EventArgs<CashierExceptionReportParameter>> OnExceptionSearchCriteriaChange;
		public event EventHandler OnExceptionEdit;

		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public POS_Exception.SearchCriteria SearchCriteria;

		public CriteriaPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Common_UsedSeconds", "(%1 seconds)"},
								   
								   {"PTSReports_SaveReport", "Save Report"},

								   {"PTSReports_Period", "Period"},
								   {"PTSReports_Daily", "Daily"},
								   {"PTSReports_Weekly", "Weekly"},
								   {"PTSReports_Monthly", "Monthly"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_Count", "Count"},

								   {"PTSReports_SearchResult", "Search Result"},
                                   {"PTSReports_SearchNoResult", "No Result Found"}
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

            BackgroundImage = Manager.BackgroundNoBorder;

			//searchResultLabel.Visible = false;
			resultLabel.Text = Localization["PTSReports_SearchResult"];
            resultLabel.Visible = false;
			foundLabel.Text = "";

			//summaryTableLayoutPanel.Paint += new PaintEventHandler(SummaryTableLayoutPanelPaint);
		}

		//private readonly Pen _pen = new Pen(Color.DimGray);
		//private void SummaryTableLayoutPanelPaint(object sender, PaintEventArgs e)
		//{
		//    Graphics g = summaryTableLayoutPanel.CreateGraphics();
		//    foreach (Control c in summaryTableLayoutPanel.Controls)
		//    {
		//        Rectangle rec = c.Bounds; rec.Inflate(2, 2);
		//        g.DrawRectangle(_pen, rec);
		//    }
		//}

		public void Initialize()
		{
			titleTableLayoutPanel.Paint += TitleTableLayoutPanelPaint;

			periodDoubleBufferPanel.Paint += PeriodDoubleBufferPanelPaint;

			periodComboBox.Items.Add(Localization["PTSReports_Daily"]);
			periodComboBox.Items.Add(Localization["PTSReports_Weekly"]);
			periodComboBox.Items.Add(Localization["PTSReports_Monthly"]);
			periodComboBox.SelectedIndex = 1;

			periodComboBox.SelectedIndexChanged += PeriodComboBoxSelectedIndexChanged;

			exceptionDoubleBufferPanel.Paint += ExceptionDoubleBufferPanelPaint;
			exceptionDoubleBufferPanel.MouseClick += ExceptionDoubleBufferPanelMouseClick;
		}

		public void ResetPeriod()
		{
			periodComboBox.SelectedIndexChanged -= PeriodComboBoxSelectedIndexChanged;
			periodComboBox.SelectedIndex = 1;
			periodComboBox.SelectedIndexChanged += PeriodComboBoxSelectedIndexChanged;
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

			if (SearchCriteria.Exceptions.Count == 0)
				return;

			//SearchCashierExceptionSummaryReport();
		}

		private Boolean _isSearch;
		private readonly Stopwatch _watch = new Stopwatch();
		public void SearchCashierExceptionSummaryReport()
		{
			if (_isSearch) return;

            resultLabel.Visible = true;
            foundLabel.Text = "";

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
			ReadDailyReportExceptionGroupByCashierDelegate loadReportDelegate = ReadDailyReportExceptionGroupByCashier;
			loadReportDelegate.BeginInvoke(periods, null, null);
		}

		private String _dateTime1;
		private String _dateTime2;
		private String _dateTime3;
		private readonly List<List<String>> _rowData = new List<List<String>>();
		private const UInt16 TopCashier = 2;
		private delegate void ReadDailyReportExceptionGroupByCashierDelegate(List<UInt64[]> periods);
		private void ReadDailyReportExceptionGroupByCashier(List<UInt64[]> periods)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new ReadDailyReportExceptionGroupByCashierDelegate(ReadDailyReportExceptionGroupByCashier), periods);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			titleTableLayoutPanel.Controls.Clear();
			//titleTableLayoutPanel.RowStyles.Clear();

			//no pos, dont search
			if (PTS.POS.POSServer.Count == 0)
			{
				SearchCompleted();
				return;
			}

			tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = true;
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Exception"], true), 0, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Cashier"] + " (" + Localization["PTSReports_Count"] + ")", true), 1, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Cashier"] + " (" + Localization["PTSReports_Count"] + ")", true), 2, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Cashier"] + " (" + Localization["PTSReports_Count"] + ")", true), 3, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel("", true), 0, 0);

			var summaryExceptionCount = new Dictionary<String, POS_Exception.ExceptionCount>();

			var index = 1;
			_dateTime1 = _dateTime2 = _dateTime3 = "";
			var resultList = new Dictionary<UInt64[], List<POS_Exception.CashierExceptionCountList>>();
			foreach (var period in periods)
			{
				var result = PTS.POS.ReadDailyReportExceptionGroupByCashier(period[0], period[1]);

				//----------------------Filter-------------------------------------------------
				foreach (var cashierExceptionCountList in result)
				{
					var filterResult = new List<POS_Exception.ExceptionCount>();
					filterResult.AddRange(cashierExceptionCountList.ExceptionList);
					foreach (var exceptionCount in filterResult)
					{
						var include = false;
						foreach (var exception in SearchCriteria.Exceptions)
						{
							if (exceptionCount.Exception == exception)
							{
								include = true;
								break;
							}
						}
						if (!include)
							cashierExceptionCountList.ExceptionList.Remove(exceptionCount);
					}
				}
				//-------------------------------------------------------------------------------
				resultList.Add(period, result);

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

				titleTableLayoutPanel.Controls.Add(GetTableLabel(range, true), index++, 0);
				
				foreach (var cashierExceptionCountList in result)
				{
					foreach (var exceptionCount in cashierExceptionCountList.ExceptionList)
					{
						if (!summaryExceptionCount.ContainsKey(exceptionCount.Exception))
							summaryExceptionCount.Add(exceptionCount.Exception, new POS_Exception.ExceptionCount
																					{
																						Exception = exceptionCount.Exception,
																						Count = 0,
																					});

						summaryExceptionCount[exceptionCount.Exception].Count += exceptionCount.Count;
					}
				}
			}


			//sort exception by eash user
			var orderByCount = summaryExceptionCount.Values.ToList();
			orderByCount.Sort(SortByCountThenException);
			//---------------------------------------------------------------------------------------------

			//add table style
			summaryTableLayoutPanel.Controls.Clear();
			summaryTableLayoutPanel.RowStyles.Clear();
			_rowData.Clear();

			if (orderByCount.Count == 0)
			{
				tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = false;
				summaryTableLayoutPanel.RowCount = 0;
				summaryTableLayoutPanel.Height = 0;
                foundLabel.Text = Localization["PTSReports_SearchNoResult"];
				SearchCompleted();
				return;
			}

			var rows = orderByCount.Count * TopCashier; //top "2"

			//var height = (float)100.0 / rows;
			summaryTableLayoutPanel.RowCount = rows;
			summaryTableLayoutPanel.Height = 30 * summaryTableLayoutPanel.RowCount + (summaryTableLayoutPanel.RowCount + 1); // height + border

			for (int i = 0; i < rows; i++)
			{
				summaryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			}
			//----------------------------------------------------------------------------------------------

			var rowindex = 0;
			foreach (var targetException in orderByCount)
			{

                var exceptionLabel = GetTableLabel(POS_Exception.FindExceptionValueByKey(targetException.Exception));
				summaryTableLayoutPanel.Controls.Add(exceptionLabel, 0, rowindex);
				summaryTableLayoutPanel.SetRowSpan(exceptionLabel, 2);

				//top 1

				var row1 = new List<String>();
				var row2 = new List<String>();
				_rowData.Add(row1);
				_rowData.Add(row2);
                row1.Add(POS_Exception.FindExceptionValueByKey(targetException.Exception));
				row2.Add("");

				//three time range
				var x = 1;
				foreach (var result in resultList)
				{
					var period = result.Key;
					var top2List = new List<POS_Exception.CashierCount>();

					foreach (var cashierExceptionCountList in result.Value)
					{
						foreach (var exceptionCount in cashierExceptionCountList.ExceptionList)
						{
							if(exceptionCount.Exception != targetException.Exception) continue;

							top2List.Add(new POS_Exception.CashierCount
													  {
														  CashierId = cashierExceptionCountList.CashierId,
														  Cashier = cashierExceptionCountList.Cashier,
														  Count =  exceptionCount.Count
													  });
							break;
						}
					}

					top2List.Sort(SortByCountThenCashier);

					//keep only "2"
					while (top2List.Count > TopCashier)
					{
						top2List.Remove(top2List.Last());
					}

					//add N/A when only users less than 2
					while (top2List.Count < TopCashier)
					{
						top2List.Add(new POS_Exception.CashierCount
						{
							Count = -1,
						});
					}

					foreach (var exceptionCount in top2List)
					{
						//String exception, String cashierId, String cashier, Int32 count, UInt64 start, UInt64 end
						var label = GetCashierExceptionTableLabel(
							(exceptionCount.Count > 0) ? targetException.Exception : "",
							exceptionCount.CashierId,
							exceptionCount.Cashier,
							exceptionCount.Count,
							period[0],
							period[1]
							);
						summaryTableLayoutPanel.Controls.Add(label, x, rowindex);
						
						if (top2List.IndexOf(exceptionCount) == 0)
							row1.Add(label.Text);
						else
							row2.Add(label.Text);
						rowindex++;
					}
					x++;
					rowindex -= top2List.Count;
				}

				rowindex += TopCashier;
			}
			SearchCompleted();
		}

		private void SearchCompleted()
		{
			_watch.Stop();

		    //foundLabel.Text = "";//Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

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
				Size = new Size(825, 450)
			};

			var reportViewer = new PTSReportViewer {ShowZoomControl = false};
			var parser = new C1ReportParser { LocalReport = reportViewer.LocalReport };
			parser.ParseC1Report();
			saveReportForm.Controls.Add(reportViewer);

			reportViewer.LocalReport.SetParameters(new[]
			{
				new Microsoft.Reporting.WinForms.ReportParameter("Exception", Localization["PTSReports_Exception"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Localization["PTSReports_Cashier"] + " (" + Localization["PTSReports_Count"] + ")"),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime1", _dateTime1),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime2", _dateTime2),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime3", _dateTime3),
			});
			parser.SetC1Report(_rowData);
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}

		private static Int32 SortByCountThenException(POS_Exception.ExceptionCount x, POS_Exception.ExceptionCount y)
		{
			if (x.Count != y.Count)
				return (y.Count - x.Count);

			return x.Exception.CompareTo(y.Exception);
		}

		private static Int32 SortByCountThenCashier(POS_Exception.CashierCount x, POS_Exception.CashierCount y)
		{
			if (x.Count != y.Count)
				return (y.Count - x.Count);

			return x.Cashier.CompareTo(y.Cashier);
		}
		
		private void TitleTableLayoutPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintTop(g, titleTableLayoutPanel);
		}

		private static Label GetTableLabel(String weekday, Boolean transparent = false)
		{
			return new Label
			{
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Text = weekday,
				BackColor = (transparent) ? Color.Transparent : Color.White,
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Fill,
				Margin = new Padding(0),
				Padding = new Padding(0, 0, 0, 0),
				Cursor = Cursors.Default
			};
		}

		private Label GetCashierExceptionTableLabel(String exception, String cashierId, String cashier, Int32 count, UInt64 start, UInt64 end)
		{
			var label = new CashierExceptionLabel
			{
				CashierId = cashierId,
				Cashier = cashier,
				Exception = exception,
				StartDateTime = start,
				EndDateTime = end,
			};

			if (count > 0)
			{
				label.Text = cashierId + @" " + cashier + @" (" + ((count > 0) ? count.ToString() : "") + @")";
				label.MouseClick += LabelMouseClick;
			}
			else
			{
				label.Text = @"N/A";
				label.Cursor = Cursors.Default;
			}


			return label;
		}

		private void LabelMouseClick(Object sender, MouseEventArgs e)
		{
			var label = sender as CashierExceptionLabel;
			if (label == null) return;

			var exceptions = new List<String>();
			if (!String.IsNullOrEmpty(label.Exception))
				exceptions.Add(label.Exception);
			else
				exceptions.AddRange(SearchCriteria.Exceptions.ToArray());

			if (OnExceptionSearchCriteriaChange != null)
				OnExceptionSearchCriteriaChange(this, new EventArgs<CashierExceptionReportParameter>(new CashierExceptionReportParameter
				{
					CashierId = label.CashierId,
					Cashier = label.Cashier,
					StartDateTime = label.StartDateTime,
					EndDateTime = label.EndDateTime,
					Exceptions = exceptions,
					DateTimeSet = DateTimeSet.None,
				}));

			//Console.WriteLine(label.Text + " " + label.Exception + " " + label.StartDateTime + " " + label.EndDateTime);
		}

		private void ExceptionDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnExceptionEdit != null)
				OnExceptionEdit(this, e);
		}

		public sealed class CashierExceptionLabel : Label
		{
			public String Exception;
			public String CashierId;
			public String Cashier;
			public UInt64 StartDateTime;
			public UInt64 EndDateTime;

			public CashierExceptionLabel()
			{
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
				BackColor = Color.White;// Color.FromArgb(141, 180, 227),
				TextAlign = ContentAlignment.MiddleCenter;
				Dock = DockStyle.Fill;
				Margin = new Padding(0);
				Padding = new Padding(0, 0, 0, 0);
				Cursor = Cursors.Hand;
			}
		}
	} 
}