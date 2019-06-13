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
using PTSReportsGenerator.B2Report;
using SetupBase;

namespace PTSReports.Cashier
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
								   {"PTSReports_Rank", "Rank"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_Summary", "Summary"},
								   {"PTSReports_Count", "Count"},

								   {"PTSReports_SearchResult", "Search Result:"},
                                   {"PTSReports_SearchNoResult", "No Result Found"}
							   };
			Localizations.Update(Localization);

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

			tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = true;
			titleTableLayoutPanel.Controls.Clear();
			//titleTableLayoutPanel.RowStyles.Clear();
			
			
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Rank"], true), 0, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Cashier"], true), 1, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Exception"], true), 2, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Count"], true), 3, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Count"], true), 4, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Count"], true), 5, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Summary"], true), 6, 1);
			
			titleTableLayoutPanel.Controls.Add(GetTableLabel("", true), 0, 0);
			titleTableLayoutPanel.Controls.Add(GetTableLabel("", true), 1, 0);
			titleTableLayoutPanel.Controls.Add(GetTableLabel("", true), 2, 0);
			titleTableLayoutPanel.Controls.Add(GetTableLabel("", true), 6, 0);
			//                                                                            cashier             exception   count
			var summaryExceptionCount = new Dictionary<String, Dictionary<String, Int32>>();

			var index = 3;
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
					var cashier = cashierExceptionCountList.CashierId + "@" + cashierExceptionCountList.Cashier;

					Dictionary<String, Int32> list;
					if (!summaryExceptionCount.ContainsKey(cashier))
					{
						list = new Dictionary<String, Int32>();
						summaryExceptionCount.Add(cashier, list);
					}
					else
					{
						list = summaryExceptionCount[cashier];
					}

					//add each range's data to summary
					foreach (var exceptionCount in cashierExceptionCountList.ExceptionList)
					{
						if (!list.ContainsKey(exceptionCount.Exception))
							list.Add(exceptionCount.Exception, 0);

						list[exceptionCount.Exception] += exceptionCount.Count;
					}
				}
			}

			var orderByCount = new List<POS_Exception.CashierCount>();
			foreach (var exception in summaryExceptionCount)
			{
				var cashierCount = new POS_Exception.CashierCount
				{
					Cashier = exception.Key,
					Count = 0
				};
				orderByCount.Add(cashierCount);
				foreach (var obj in exception.Value)
				{
					cashierCount.Count += obj.Value;
				}
			}
			orderByCount.Sort(SortByCountThenCashierName);

			//---------------------------------------------------------------------------------------------

			//add table style
			summaryTableLayoutPanel.Controls.Clear();
			summaryTableLayoutPanel.RowStyles.Clear();
			_rowData.Clear();
			var rows = 0;

			foreach (var exception in summaryExceptionCount)
			{
				if(exception.Value.Count > 0)
					rows += (exception.Value.Count + 1);//exception's count + total
			}

			if (rows == 0)
			{
			    foundLabel.Text = Localization["PTSReports_SearchNoResult"];
				tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = false;
				summaryTableLayoutPanel.RowCount = 0;
				summaryTableLayoutPanel.Height = 0;
				SearchCompleted();
				return;
			}
			
			summaryTableLayoutPanel.RowCount = rows;
			summaryTableLayoutPanel.Height = 30*summaryTableLayoutPanel.RowCount +(summaryTableLayoutPanel.RowCount + 1); // height + border

			for (int i = 0; i < rows; i++)
			{
				summaryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			}
			
			//summaryTableLayoutPanel.SetRowSpan();
			//----------------------------------------------------------------------------------------------
			var rowindex = 0;
			var cashierIndex = 1;
			foreach (var obj in orderByCount)
			{
				var cashier = obj.Cashier;
				//NO.
				var noLabel = GetTableLabel(cashierIndex.ToString());
				summaryTableLayoutPanel.Controls.Add(noLabel, 0, rowindex);
				
				//Cashier Name
				var nameLabel = GetTableLabel(cashier.Replace("@", " "));
				summaryTableLayoutPanel.Controls.Add(nameLabel, 1, rowindex);

				foreach (var exception in summaryExceptionCount)
				{
					if (exception.Key != cashier) continue;

					summaryTableLayoutPanel.SetRowSpan(noLabel, (exception.Value.Count + 1)); //+ total row
					summaryTableLayoutPanel.SetRowSpan(nameLabel, (exception.Value.Count + 1)); //+ total row
					break;
				}
				//-----------------------------------------------------------------

				var allExceptionCount = new Dictionary<String, POS_Exception.ExceptionCount>();
				foreach (var result in resultList)
				{
					foreach (var cashierExceptionCountList in result.Value)
					{
						if (cashierExceptionCountList.CashierId + "@" + cashierExceptionCountList.Cashier != cashier) continue;

						foreach (var exceptionCount in cashierExceptionCountList.ExceptionList)
						{
							if (!allExceptionCount.ContainsKey(exceptionCount.Exception))
							{
								allExceptionCount.Add(exceptionCount.Exception, new POS_Exception.ExceptionCount
																					{
																						Exception = exceptionCount.Exception
																					});
							}

							allExceptionCount[exceptionCount.Exception].Count += exceptionCount.Count;
						}
					}
				}

				//sort exception by eash user
				var list = allExceptionCount.Values.ToList();
				list.Sort(SortByCountThenException);

				var totalEachCashier = new List<Int32>{0, 0, 0};
				Int32 x;
				Int32 total;
				foreach (var exceptionObjCount in list)
				{
					var row = new List<String>();
					_rowData.Add(row);

					row.Add(noLabel.Text);
					row.Add(nameLabel.Text);
					//key
					var exception = exceptionObjCount.Exception;

					//Exception Name
					summaryTableLayoutPanel.Controls.Add(GetTableLabel(POS_Exception.FindExceptionValueByKey(exception)), 2, rowindex);
                    row.Add(POS_Exception.FindExceptionValueByKey(exception));

					//three time range
					x = 3;
					total = 0;
					foreach (var result in resultList)
					{
						var period = result.Key;
						var hasValue = false;
						foreach (var cashierExceptionCountList in result.Value)
						{
							if (cashierExceptionCountList.CashierId + "@" + cashierExceptionCountList.Cashier != cashier) continue;
							
							foreach (var exceptionCount in cashierExceptionCountList.ExceptionList)
							{
								if (exceptionCount.Exception != exception) continue;
								//String exception, String cashierId, String cashier, Int32 count, UInt64 start, UInt64 end
								hasValue = true;
								summaryTableLayoutPanel.Controls.Add(GetCashierExceptionTableLabel(
									exception,
									cashierExceptionCountList.CashierId,
									cashierExceptionCountList.Cashier,
									exceptionCount.Count,
									period[0],
									period[1]
									), x, rowindex);
								total += exceptionCount.Count;
								totalEachCashier[x - 3] += exceptionCount.Count;

								row.Add(exceptionCount.Count.ToString());
							}
						}
						if (!hasValue)
						{
							summaryTableLayoutPanel.Controls.Add(GetTableLabel("0"), x, rowindex);
							row.Add("0");
						}
						x++;
					}

					summaryTableLayoutPanel.Controls.Add(GetCashierExceptionTableLabel(
						exception,
						cashier.Split('@')[0],
						cashier.Split('@')[1],
						total,
						resultList.Keys.First()[0],
						resultList.Keys.Last()[1]
						), x, rowindex);

					row.Add(total.ToString());

					rowindex++;
				}
				summaryTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Summary"]), 2, rowindex);

				var summaryrow = new List<String>();
					_rowData.Add(summaryrow);

				summaryrow.Add(noLabel.Text);
				summaryrow.Add(nameLabel.Text);
				summaryrow.Add(Localization["PTSReports_Summary"]);

				x = 3;
				total = 0;
				foreach (var result in resultList)
				{
					summaryTableLayoutPanel.Controls.Add(GetCashierExceptionTableLabel(
						"",
						cashier.Split('@')[0],
						cashier.Split('@')[1],
						totalEachCashier[x - 3],
						result.Key[0],
						result.Key[1]
						), x, rowindex);
					
					summaryrow.Add(totalEachCashier[x - 3].ToString());

					total += totalEachCashier[x - 3];
					x++;
				}

				summaryTableLayoutPanel.Controls.Add(GetCashierExceptionTableLabel(
					"",
					cashier.Split('@')[0],
					cashier.Split('@')[1],
					total,
					resultList.Keys.First()[0],
					resultList.Keys.Last()[1]
					), x, rowindex);
				summaryrow.Add(total.ToString());

				x++;
				rowindex++;
				cashierIndex++;
			}

			SearchCompleted();
		}

		private void SearchCompleted()
		{
			_watch.Stop();

		   //"Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

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
				Size = new Size(1035, 550)
			};

			var reportViewer = new PTSReportViewer {ShowZoomControl = false};
			var parser = new B2ReportParser {LocalReport = reportViewer.LocalReport};
			parser.ParseB2Report();
			saveReportForm.Controls.Add(reportViewer);

			reportViewer.LocalReport.SetParameters(new[]
			{
				new Microsoft.Reporting.WinForms.ReportParameter("Rank", Localization["PTSReports_Rank"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Localization["PTSReports_Cashier"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Exception", Localization["PTSReports_Exception"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Count", Localization["PTSReports_Count"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Summary", Localization["PTSReports_Summary"]),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime1", _dateTime1),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime2", _dateTime2),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime3", _dateTime3),
			});
			parser.SetB2Report(_rowData);
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}

		private static Int32 SortByCountThenCashierName(POS_Exception.CashierCount x, POS_Exception.CashierCount y)
		{
			if (x.Count != y.Count)
				return (y.Count - x.Count);

			return x.Cashier.CompareTo(y.Cashier);
		}

		private static Int32 SortByCountThenException(POS_Exception.ExceptionCount x, POS_Exception.ExceptionCount y)
		{
			if (x.Count != y.Count)
				return (y.Count - x.Count);

			return x.Exception.CompareTo(y.Exception);
		}

		private void TitleTableLayoutPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintTop(g, titleTableLayoutPanel);
		}

		private static Label GetTableLabel(String text, Boolean transparent = false)
		{
			return new Label
			{
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Text = text,
				BackColor = (transparent) ? Color.Transparent : Color.White,
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Fill,
				Margin = new Padding(0),
				Padding = new Padding(0, 0, 0, 0),
				Cursor = Cursors.Default,
			};
		}

		private Label GetCashierExceptionTableLabel(String exception, String cashierId, String cashier, Int32 count, UInt64 start, UInt64 end)
		{
			var label = new CashierExceptionLabel
			{
				Text = (count > 0) ? count.ToString() : "0",
				CashierId = cashierId,
				Cashier = cashier,
				Exception = exception,
				StartDateTime = start,
				EndDateTime = end,
			};

			if (count != 0)
			{
				label.Cursor = Cursors.Hand;
				label.MouseClick += LabelMouseClick;
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
				Cursor = Cursors.Default;
			}
		}
	} 
}