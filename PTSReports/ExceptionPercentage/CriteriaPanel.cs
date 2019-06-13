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
using PTSReportsGenerator.C2Report;
using SetupBase;

namespace PTSReports.ExceptionPercentage
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
								   {"PTSReports_Summary", "Summary"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_Percentage", "Percentage"},
								   
								   {"PTSReports_SearchResult", "Search Result"},
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
				list.Add(POS_Exception.FindExceptionValueByKey(exception));
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

			//SearchCashierExceptionPercentageReport();
		}

		private Boolean _isSearch;
		private readonly Stopwatch _watch = new Stopwatch();
		public void SearchCashierExceptionPercentageReport()
		{
			if (_isSearch) return;
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

			//no pos, dont search
			if (PTS.POS.POSServer.Count == 0)
			{
				SearchCompleted();
				return;
			}

			tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = true;
			titleTableLayoutPanel.Controls.Clear();
			//titleTableLayoutPanel.RowStyles.Clear();

			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Rank"], true), 0, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Cashier"], true), 1, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Exception"], true), 2, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Percentage"], true), 3, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Exception"], true), 4, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Percentage"], true), 5, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Exception"], true), 6, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Percentage"], true), 7, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Summary"], true), 8, 1);
			titleTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Percentage"], true), 9, 1);
			
			var periodStart = periods.First()[0];
			var periodEnd = periods.Last()[1];
			//                                                                            cashier             period   count
			var summaryExceptionCount = new Dictionary<String, Dictionary<UInt64[], Int32>>();

			//                                                                 period   count
			var totalExceptionCount = new Dictionary<UInt64[], Int32>();
			Int32 allSummary = 0;

			var index = 2;
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

				var rangeLabel = GetTableLabel(range, true);
				titleTableLayoutPanel.Controls.Add(rangeLabel, index, 0);
				titleTableLayoutPanel.SetColumnSpan(rangeLabel, 2);
				index += 2;

				totalExceptionCount.Add(period, 0);

				foreach (var cashierExceptionCountList in result)
				{
					var cashier = cashierExceptionCountList.CashierId + "@" + cashierExceptionCountList.Cashier;

					Dictionary<UInt64[], Int32> list;
					if (!summaryExceptionCount.ContainsKey(cashier))
					{
						list = new Dictionary<UInt64[], Int32>();
						summaryExceptionCount.Add(cashier, list);
					}
					else
					{
						list = summaryExceptionCount[cashier];
					}

					if (!list.ContainsKey(period))
						list.Add(period, 0);
					//add each range's data to summary
					foreach (var exceptionCount in cashierExceptionCountList.ExceptionList)
					{
						list[period] += exceptionCount.Count;
						totalExceptionCount[period] += exceptionCount.Count;
						allSummary += exceptionCount.Count;
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

			if (orderByCount.Count == 0)
			{
				tableTitlePanel.Visible = summaryTableLayoutPanel.Visible = false;
				summaryTableLayoutPanel.RowCount = 0;
				summaryTableLayoutPanel.Height = 0;
				SearchCompleted();
				return;
			}

			var rows = orderByCount.Count; //cashier's count + total
			
			//var height = (float)100.0 / rows;
			summaryTableLayoutPanel.RowCount = rows + 1;
			summaryTableLayoutPanel.Height = 30*summaryTableLayoutPanel.RowCount +(summaryTableLayoutPanel.RowCount + 1); // height + border


			for (int i = 0; i < rows; i++)
			{
				summaryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			}
			
			//summaryTableLayoutPanel.SetRowSpan();
			//----------------------------------------------------------------------------------------------
			var x = 2;
			var rowindex = 0;
			var cashierIndex = 1;
			foreach (var obj in orderByCount)
			{
				var row = new List<String>();
				_rowData.Add(row);

				var cashier = obj.Cashier;

				//NO.
				summaryTableLayoutPanel.Controls.Add(GetTableLabel(cashierIndex.ToString()), 0, rowindex);
				row.Add(cashierIndex.ToString());

				//Cashier Name
				summaryTableLayoutPanel.Controls.Add(GetTableLabel(cashier.Replace("@", " ")), 1, rowindex);
				row.Add(cashier.Replace("@", " "));

				var temp = cashier.Split('@');
				var cashierId = temp[0];
				var cashierName = cashier.Replace(cashierId+"@", "");

				x = 2;
				var total = 0;
				foreach (var exception in summaryExceptionCount)
				{
					if (exception.Key != cashier) continue;
					foreach (var timeperiod in totalExceptionCount)
					{
						var count = 0;
						foreach (var exceptionObj in exception.Value)
						{
							if (exceptionObj.Key == timeperiod.Key)
							{
								count = exceptionObj.Value;
								break;
							}
						}
						total += count;
						var label = GetCashierExceptionTableLabel(
							"", cashierId, cashierName, count, timeperiod.Key[0], timeperiod.Key[1]
							);
						summaryTableLayoutPanel.Controls.Add(label, x++, rowindex);
						row.Add(label.Text);

						var value = "0%";
						if (timeperiod.Value > 0)
							value = (count * 100.0 / timeperiod.Value).ToString("F2") + "%";
						label = GetCashierExceptionTableLabel(
							value, cashierId, cashierName, count, timeperiod.Key[0], timeperiod.Key[1]
							);
						summaryTableLayoutPanel.Controls.Add(label, x++, rowindex);
						row.Add(label.Text);
					}

					var label2 = GetCashierExceptionTableLabel(
						"", cashierId, cashierName, total, periodStart, periodEnd
						);
					summaryTableLayoutPanel.Controls.Add(label2, x++, rowindex);
					row.Add(label2.Text);
					
					var value2 = "0%";
					if (allSummary > 0)
						value2 = (total*100.0/allSummary).ToString("F2") + "%";
					label2 = GetCashierExceptionTableLabel(
						value2, cashierId, cashierName, total, periodStart, periodEnd
						);
					summaryTableLayoutPanel.Controls.Add(label2, x++, rowindex);
					row.Add(label2.Text);
					break;
				}

				cashierIndex++;
				rowindex++;
			}
			//----------------------------------------------------------------------------------------------
			summaryTableLayoutPanel.Controls.Add(GetTableLabel(Localization["PTSReports_Summary"]), 1, rowindex);

			var summaryrow = new List<String>();
			_rowData.Add(summaryrow);

			summaryrow.Add("");
			summaryrow.Add(Localization["PTSReports_Summary"]);

			x = 2;
			foreach (var exceptionCount in totalExceptionCount)
			{
				var label = GetCashierExceptionTableLabel(
					"", null, null, exceptionCount.Value, exceptionCount.Key[0], exceptionCount.Key[1]
					);
				summaryTableLayoutPanel.Controls.Add(label, x++, rowindex);
				summaryrow.Add(label.Text);

				var value = "0%";
				if (exceptionCount.Value > 0)
					value = "100.00%";
				label = GetCashierExceptionTableLabel(
					value, null, null, exceptionCount.Value, exceptionCount.Key[0], exceptionCount.Key[1]
					);
				summaryTableLayoutPanel.Controls.Add(label, x++, rowindex);
				summaryrow.Add(label.Text);
			}

			var label3 = GetCashierExceptionTableLabel(
				"", null, null, allSummary, periodStart, periodEnd
				);
			summaryTableLayoutPanel.Controls.Add(label3, x++, rowindex);
			summaryrow.Add(label3.Text);

			label3 = GetCashierExceptionTableLabel(
				"100.00%", null, null, allSummary, periodStart, periodEnd
				);
			summaryTableLayoutPanel.Controls.Add(label3, x++, rowindex);
			summaryrow.Add(label3.Text);
			
			SearchCompleted();
		}

		private void SearchCompleted()
		{
			_watch.Stop();

		    foundLabel.Text = "";//"Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

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
				Size = new Size(1000, 450)
			};

			var reportViewer = new PTSReportViewer {ShowZoomControl = false};
			var parser = new C2ReportParser { LocalReport = reportViewer.LocalReport };
			parser.ParseC2Report();
			saveReportForm.Controls.Add(reportViewer);

			reportViewer.LocalReport.SetParameters(new[]
			{
				new Microsoft.Reporting.WinForms.ReportParameter("Rank", Localization["PTSReports_Rank"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Localization["PTSReports_Cashier"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Exception", Localization["PTSReports_Exception"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Percentage", Localization["PTSReports_Percentage"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Summary", Localization["PTSReports_Summary"]),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime1", _dateTime1),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime2", _dateTime2),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime3", _dateTime3),
			});
			parser.SetC2Report(_rowData);
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
				Cursor = Cursors.Default,
			};
		}

		private Label GetCashierExceptionTableLabel(String percentage, String cashierId, String cashier, Int32 count, UInt64 start, UInt64 end)
		{
			var label = new CashierExceptionLabel
			{
				Text = String.IsNullOrEmpty(percentage) ? ((count > 0) ? count.ToString() : "") : percentage,
				CashierId = cashierId,
				Cashier = cashier,
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
			exceptions.AddRange(SearchCriteria.Exceptions);

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