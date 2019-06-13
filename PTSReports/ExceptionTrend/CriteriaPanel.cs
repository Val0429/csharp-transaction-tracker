using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using PTSReportsGenerator;
using PTSReportsGenerator.C3Report;
using SetupBase;

namespace PTSReports.ExceptionTrend
{
	public sealed partial class CriteriaPanel : UserControl
	{
		//public event EventHandler<EventArgs<CashierExceptionReportParameter>> OnExceptionSearchCriteriaChange;
		public event EventHandler OnExceptionEdit;

		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public POS_Exception.SearchCriteria SearchCriteria;

		public CriteriaPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Common_UsedSeconds", "(%1 seconds)"},
								   {"PTSReports_Exception", "Exception"},
								   
								   {"PTSReports_SaveReport", "Save Report"},

								   {"PTSReports_Period", "Period"},
								   {"PTSReports_Daily", "Daily"},
								   {"PTSReports_Weekly", "Weekly"},
								   {"PTSReports_Monthly", "Monthly"},
								   {"PTSReports_Rank", "Rank"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_Summary", "Summary"},
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

			//SearchCashierExceptionTrendReport();
		}

		private Boolean _isSearch;
		private readonly Stopwatch _watch = new Stopwatch();
		public void SearchCashierExceptionTrendReport()
		{
			if (_isSearch) return;
            resultLabel.Visible = true;
			_rowData.Clear();
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
			chartDoubleBufferPanel.Visible = false;

			_watch.Reset();
			_watch.Start();
			ReadDailyReportExceptionGroupByCashierDelegate loadReportDelegate = ReadDailyReportExceptionGroupByCashier;
			loadReportDelegate.BeginInvoke(periods, null, null);
		}

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

			chartDoubleBufferPanel.Visible = true;

			//                                                                            cashier             range   count
			var summaryExceptionCount = new Dictionary<String, Dictionary<String, Int32>>();
			var rangeList = new List<String>();

			foreach (var period in periods)
			{
				var start = DateTimes.ToDateTime(period[0], PTS.Server.TimeZone);
				var end = DateTimes.ToDateTime(period[1], PTS.Server.TimeZone);

				switch (SearchCriteria.DateTimeSet)
				{
					case DateTimeSet.Daily:
						rangeList.Add(start.ToString("MM-dd-yyyy"));
						break;

					default:
						rangeList.Add((start.ToString("MM-dd-yyyy") + @" ~ " + end.ToString("MM-dd-yyyy")));
						break;
				}
			}

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

				foreach (var cashierExceptionCountList in result)
				{
					var cashier = cashierExceptionCountList.CashierId + " " + cashierExceptionCountList.Cashier;

					Dictionary<String, Int32> list;
					if (!summaryExceptionCount.ContainsKey(cashier))
					{
						list = new Dictionary<String, Int32>();
						summaryExceptionCount.Add(cashier, list);

						foreach (var rangeValue in rangeList)
						{
							list.Add(rangeValue, 0);
						}
					}
					else
					{
						list = summaryExceptionCount[cashier];
					}

					//add each range's data to summary
					foreach (var exceptionCount in cashierExceptionCountList.ExceptionList)
					{
						list[range] += exceptionCount.Count;
					}
				}
			}
			//----------------------------------------------------------------------------------------------
			chartDoubleBufferPanel.Controls.Clear();

			var keys = summaryExceptionCount.Keys.ToList();
			keys.Sort((x, y) => (x.CompareTo(y)));

			foreach (var key in keys)
			{
				var resultXml = new XmlDocument();
				var xmlRoot = resultXml.CreateElement("Result");
				xmlRoot.SetAttribute("cashier", key);
				resultXml.AppendChild(xmlRoot);

				var exceptions = summaryExceptionCount[key];
				foreach (var exception in exceptions)
				{
					var dataRoot = resultXml.CreateElement("Data");
					dataRoot.SetAttribute("date", exception.Key);
					xmlRoot.AppendChild(dataRoot);

					dataRoot.AppendChild(Xml.CreateXmlElementWithText(resultXml, "Exception", exception.Value));
				}
				AppendCashierExceptionTrendChart(resultXml);
			}
			//var resultXml = new XmlDocument();
			//resultXml.LoadXml("<Result cashier=\"1 Deray Hsueh\"><Data date=\"12-01 ~ 12-07\"><Exception>123</Exception></Data><Data date=\"12-08 ~ 12-15\"><Exception>333</Exception></Data><Data date=\"12-15 ~ 12-22\"><Exception>243</Exception></Data></Result>");
			//AppendCashierExceptionTrendChart(resultXml);

			//var resultXml2 = new XmlDocument();
			//resultXml2.LoadXml("<Result cashier=\"1 Mary Chris\"><Data date=\"12-01 ~ 12-07\"><Exception>311</Exception></Data><Data date=\"12-08 ~ 12-15\"><Exception>0</Exception></Data><Data date=\"12-15 ~ 12-22\"><Exception>218</Exception></Data></Result>");
			//AppendCashierExceptionTrendChart(resultXml2);

			SearchCompleted();
		}
		private void SearchCompleted()
		{
			_watch.Stop();

		    foundLabel.Text = "";//"Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

			ApplicationForms.HideLoadingIcon();

			_isSearch = false;
		}

		private readonly List<Image> _rowData = new List<Image>();
		public void SaveReport()
		{
			//no data
			if (_rowData.Count == 0) return;

			var saveReportForm = new SaveReportForm
			{
				Icon = PTS.Form.Icon,
				Text = Localization["PTSReports_SaveReport"],
				Size = new Size(480, 730)
			};

			var reportViewer = new PTSReportViewer {ShowZoomControl = false};
			var parser = new C3ReportParser { LocalReport = reportViewer.LocalReport };
			parser.ParseC3Report();
			saveReportForm.Controls.Add(reportViewer);

			parser.SetC3Report(_rowData);
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}

		private void AppendCashierExceptionTrendChart(XmlDocument resultXml)
		{
			var reportViewer = new PTSReportViewer
			{
				TimeZone = PTS.Server.TimeZone,
				ReportXmlDoc = resultXml,
				ReportType = "Trend",
				Size = new Size(415, 250),
				Dock = DockStyle.None,
				ShowZoomControl = false
			};
			//reportViewer.ReportViewer.Dock = DockStyle.Fill;
			//remove previous chart
			chartDoubleBufferPanel.Controls.Add(reportViewer);

			//要呼叫 RefreshReport 才會繪製報表
			reportViewer.RefreshReport();

			try
			{
				var image = Converter.ImageTrim(new Bitmap(Image.FromStream(new MemoryStream(reportViewer.SnapshotImage()))));
				if(image.Width > 0 && image .Height > 0)
					_rowData.Add(image);
			}
			catch (System.Exception)
			{
				
			}
		}

		private void ExceptionDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnExceptionEdit != null)
				OnExceptionEdit(this, e);
		}
	} 
}