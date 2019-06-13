using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using PanelBase;
using PTSReportsGenerator;
using ApplicationForms = PanelBase.ApplicationForms;
using Manager = SetupBase.Manager;

namespace PTSReports.Exception
{
	public sealed partial class SearchPanel : UserControl
	{
		public IApp App;
		public IPTS PTS;
		public POS_Exception.SearchCriteria SearchCriteria;
		public Dictionary<String, String> Localization;

		private readonly PageSelector _pageSelector;
		public SearchPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"Common_UsedSeconds", "(%1 seconds)"},
								   
								   {"PTSReports_SaveReport", "Save Report"},

								   {"PTSReports_Chart", "Chart"},
								   {"PTSReports_SearchResult", "Search Result:"},
								   {"PTSReports_SearchNoResult", "No Result Found"},
								   {"PTSReports_SearchResultFound", "%1 Result Found"},
								   {"PTSReports_SearchResultsFound", "%1 Results Found"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
            BackgroundImage = Manager.BackgroundNoBorder;
			DoubleBuffered = true;
			Dock = DockStyle.None;

			_pageSelector = new PageSelector();
			_pageSelector.OnSelectionChange += PageSelectorOnSelectionChange;
			pagePanel.Controls.Add(_pageSelector);

			chartPanel.Paint += ChartPanelPaint;

			chartPanel.MouseClick += ChartPanelMouseClick;

			resultLabel.Text = Localization["PTSReports_SearchResult"];
            resultLabel.Visible = false;
			foundLabel.Text = "";

			SearchCriteria = new POS_Exception.SearchCriteria();
		}

		private void ChartPanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, chartPanel);

			if (chartPanel.Height == 40)
				Manager.PaintExpand(g, chartPanel);
			else
				Manager.PaintCollapse(g, chartPanel);

			if (Width <= 200) return;
			Manager.PaintText(g, Localization["PTSReports_Chart"]);
		}

		private Boolean _isExpand;

		private void ChartPanelMouseClick(Object sender, MouseEventArgs e)
		{
			_isExpand = !_isExpand;
			if (_isExpand)
			{
				ShowChart();
				chartPanel.Height = 240;
				chartPanel.Cursor = Cursors.PanNorth;
			}
			else
			{
				chartPanel.Height = 40;
				chartPanel.Cursor = Cursors.PanSouth;
			}

			chartPanel.Invalidate();
		}
		private readonly List<ExceptionResultPanel> _recycleExceptionResult = new List<ExceptionResultPanel>();
		private ExceptionResultPanel GetExceptionResultPanel()
		{
			ExceptionResultPanel exceptionResultPanel = null;
			if (_recycleExceptionResult.Count > 0)
			{
				foreach (ExceptionResultPanel resultPanel in _recycleExceptionResult)
				{
					if (resultPanel.IsLoadingImage) continue;

					exceptionResultPanel = resultPanel;
					break;
				}

				if (exceptionResultPanel != null)
				{
					_recycleExceptionResult.Remove(exceptionResultPanel);
					return exceptionResultPanel;
				}
			}

			exceptionResultPanel = new ExceptionResultPanel
			{
				App = App,
				PTS = PTS,
				SearchPanel = this,
			};
			exceptionResultPanel.OnPlayback += ExceptionResultPanelOnPlayback;

			return exceptionResultPanel;
		}

		private Boolean _isSearching;

		private readonly Stopwatch _watch = new Stopwatch();
		private Int32 _page;
		private const UInt16 CountPerPage = 20;
		
		public void SearchExceptions(UInt16 page)
		{
			SearchExceptions(page, true);
		}

		private Boolean _clearModel;
		public void SearchExceptions(Int32 page, Boolean clearModel)
		{
			if (_isSearching) return;
            resultLabel.Visible = true;
			_clearModel = clearModel;
			_page = page;
			//if (SearchCriteria.POS.Count == 0) return;
			//if (SearchCriteria.Exceptions.Count == 0) return;

			if (clearModel)
				ClearViewModel();

			ApplicationForms.ShowLoadingIcon(PTS.Form);

			_isSearching = true;
			_watch.Reset();
			_watch.Start();

			//refresh start and end
			if (SearchCriteria.DateTimeSet != DateTimeSet.None)
			{
				var range = DateTimes.UpdateStartAndEndDateTime(PTS.Server.DateTime, PTS.Server.TimeZone, SearchCriteria.DateTimeSet);
				SearchCriteria.StartDateTime = range[0];
				SearchCriteria.EndDateTime = range[1];
			}

			SearchExceptionByConditionDelegate searchDelegate = PTS.POS.ReadExceptionByCondition;
			searchDelegate.BeginInvoke(
				SearchCriteria.POS.ToArray(),
				SearchCriteria.CashierIds.ToArray(),
				SearchCriteria.Cashiers.ToArray(),
				SearchCriteria.StartDateTime,
				SearchCriteria.EndDateTime,
				SearchCriteria.Exceptions.ToArray(),
				new String[0], _page, CountPerPage,
				SearchReportCallback, searchDelegate);
		}

        private delegate POS_Exception.ExceptionDetailList SearchExceptionByConditionDelegate(String[] posIds, String[] cashierIds, String[] cashiers, UInt64 startutc, UInt64 endutc, String[] exceptions, String[] keywords, Int32 pageIndex, UInt16 count);        
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

		private POS_Exception.ExceptionDetailList _searchResult;
		private void UpdateResult(POS_Exception.ExceptionDetailList searchResult)
		{
			_searchResult = searchResult;

			if (_searchResult.Count == 0)
			{
				if (!_clearModel)
				{
					ClearViewModel();
				}

				chartPanel.Visible = false;
			    foundLabel.Text = Localization["PTSReports_SearchNoResult"];
					 //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));
			}
			else
			{
				chartPanel.Visible = true;
				if (_clearModel)
				{
					_pageSelector.Pages = _searchResult.Pages;
					_pageSelector.SelectPage = _page;
					_pageSelector.ShowPages();
				}
				else
				{
					ClearViewModel();
				}

				foundLabel.Text = ((_searchResult.Count == 1)
							? Localization["PTSReports_SearchResultFound"]
							: Localization["PTSReports_SearchResultsFound"]).Replace("%1", _searchResult.Count.ToString());

				//foundLabel.Text += @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

				var count = 1;
				foreach (var exceptionDetail in _searchResult.Results)
				{
					var resultPanel = GetExceptionResultPanel();
					resultPanel.Id = Convert.ToInt32(count + ((_page - 1) * CountPerPage));
					resultPanel.ExceptionDetail = exceptionDetail;

					containerPanel.Controls.Add(resultPanel);
					resultPanel.BringToFront();
					count++;
				}

				var resultTitlePanel = GetExceptionResultPanel();
				resultTitlePanel.IsTitle = true;
				resultTitlePanel.Cursor = Cursors.Default;
				containerPanel.Controls.Add(resultTitlePanel);
			}

			_isSearching = false;

			ApplicationForms.HideLoadingIcon();
			containerPanel.AutoScroll = false;
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
			containerPanel.AutoScroll = true;

            pagePanel.Visible = _searchResult.Count > 0;
		}

		private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
		{
			SearchExceptions(_pageSelector.SelectPage, false);
		}

		private ExceptionResultPanel _selectedExceptionResultPanel;
		private void ExceptionResultPanelOnPlayback(Object sender, EventArgs e)
		{
			var panel = sender as ExceptionResultPanel;
			if (panel == null || panel.ExceptionDetail == null) return;

			var pos = PTS.POS.FindPOSById(panel.ExceptionDetail.POSId);
			if (pos == null) return;

			_selectedExceptionResultPanel = panel;

			//use exception search condition search transaction list
			ApplicationForms.ShowLoadingIcon(PTS.Form);
				
			ReadTransactionByIdDelegate readTransactionByIdDelegate = PTS.POS.ReadTransactionById;
			readTransactionByIdDelegate.BeginInvoke(
				panel.ExceptionDetail.TransactionId,
				SearchTransactionIdReportCallback,
				readTransactionByIdDelegate);

			//SearchTransactionListByConditionDelegate searchTransactionListByConditionDelegate = PTS.POS.ReadTransactionHeadByCondition;
			//searchTransactionListByConditionDelegate.BeginInvoke(
			//    SearchCriteria.POS.ToArray(),
			//    SearchCriteria.CashierIds.ToArray(),
			//    _searchResult.ExceptionDetails.First().UTC - 60000,
			//    _searchResult.ExceptionDetails.Last().UTC + 60000,
			//    SearchCriteria.Exceptions.ToArray(),
			//    new String[] { }, 1, 20,
			//    SearchTransactionListReportCallback,
			//    searchTransactionListByConditionDelegate);
		}

		private delegate POS_Exception.TransactionItemList ReadTransactionByIdDelegate(String transactionId);
		//private delegate POS_Exception.TransactionList SearchTransactionListByConditionDelegate(UInt16[] posIds, String[] cashierIds, UInt64 startutc, UInt64 endutc, String[] exceptions, String[] keywords, Int32 pageIndex, UInt16 count);
		private delegate void SearchTransactionIdReportCallbackDelegate(IAsyncResult result);
		private void SearchTransactionIdReportCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new SearchTransactionIdReportCallbackDelegate(SearchTransactionIdReportCallback), result);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			var transactionItemList = ((ReadTransactionByIdDelegate)result.AsyncState).EndInvoke(result);

			//Find the transaction's TOTAL exception amount
			var transactions = PTS.POS.ReadTransactionHeadByCondition(
				_selectedExceptionResultPanel.ExceptionDetail.TransactionId
				, transactionItemList.StartDateTime, transactionItemList.EndDateTime,
			SearchCriteria.Exceptions.ToArray());

			ApplicationForms.HideLoadingIcon();

			var targetTransaction = new POS_Exception.Transaction
			{
				Id = _selectedExceptionResultPanel.ExceptionDetail.TransactionId,
				POSId = _selectedExceptionResultPanel.ExceptionDetail.POSId,
				DateTime = DateTimes.ToDateTime(transactionItemList.StartDateTime, PTS.Server.TimeZone),
				UTC = transactionItemList.StartDateTime,
				CashierId = _selectedExceptionResultPanel.ExceptionDetail.CashierId,
				Cashier = _selectedExceptionResultPanel.ExceptionDetail.Cashier,
			};

			if (transactions.Results.Count > 0)
			{
				var transaction = transactions.Results.First();
				//transaction.Total = transaction.Total;  // <- this number is wrong
				targetTransaction.ExceptionAmount = transaction.ExceptionAmount; //sum: exception amount
			}
			else
			{
				//fake total & exception a,oint(it only single)
				targetTransaction.ExceptionAmount = _selectedExceptionResultPanel.ExceptionDetail.ExceptionAmount; //single exception amount
			}

			targetTransaction.Total = _selectedExceptionResultPanel.ExceptionDetail.TotalTransactionAmount;

			var transactionList = new POS_Exception.TransactionList{Count = 1};
			transactionList.Results.Add(targetTransaction);

			App.SwitchPage("Playback", new TransactionListParameter
			{
				Index = 1,//_selectedExceptionResultPanel.Id,
				Transaction = targetTransaction,
				TransactionList = transactionList,
                POS = new List<String> { targetTransaction.POSId },
				StartDateTime = transactionItemList.StartDateTime,
				EndDateTime = Math.Max(transactionItemList.EndDateTime, transactionItemList.StartDateTime + 2000),//transactionItemList.EndDateTime,
				Exceptions = SearchCriteria.Exceptions,
				ExceptionDateTime = _selectedExceptionResultPanel.ExceptionDetail.UTC
			});
		}
		public void ClearResult()
		{
			_isExpand = false;
			chartPanel.Height = 40;
			chartPanel.Cursor = Cursors.PanSouth;
			chartDoubleBufferPanel.Controls.Clear();

			_pageSelector.ClearViewModel();
			foundLabel.Text = ""; // Localization["PTSReports_SearchResult"];

			ClearViewModel();
		}

		public void ClearViewModel()
		{
			if (containerPanel.Controls.Count == 0) return;

			foreach (ExceptionResultPanel control in containerPanel.Controls)
			{
				control.Reset();

				if (!_recycleExceptionResult.Contains(control))
					_recycleExceptionResult.Add(control);
			}

			containerPanel.Controls.Clear();
		}

		//---------------------------------------------------------------------------------------------------------------------

		private const UInt16 MaximumConnection = 3;
		private UInt16 _connection;
		public List<ExceptionResultPanel> QueueSearchResultPanel = new List<ExceptionResultPanel>();
		public void QueueLoadSnapshot(ExceptionResultPanel searchResultPanel)
		{
			if (searchResultPanel.Camera == null) return;

			if (_connection < MaximumConnection)
			{
				if (QueueSearchResultPanel.Contains(searchResultPanel))
					QueueSearchResultPanel.Remove(searchResultPanel);

				switch (searchResultPanel.Camera.Server.Manufacture)
				{
					case "Salient":
						searchResultPanel.LoadSalientSnapshot();
						break;

					default:
						_connection++;
						LoadSnapshotDelegate loadSnapshotDelegate = searchResultPanel.LoadSnapshot;
						loadSnapshotDelegate.BeginInvoke(LoadSnapshotCallback, loadSnapshotDelegate);
						break;
				}

				return;
			}

			if (!QueueSearchResultPanel.Contains(searchResultPanel))
				QueueSearchResultPanel.Add(searchResultPanel);
		}

		private delegate void LoadSnapshotDelegate();
		private void LoadSnapshotCallback(IAsyncResult result)
		{
			((LoadSnapshotDelegate)result.AsyncState).EndInvoke(result);
			_connection--;

			if (QueueSearchResultPanel.Count > 0)
				QueueLoadSnapshot(QueueSearchResultPanel[0]);
		}
		
		//---------------------------------------------------------------------------------------------------------------------

		private void ShowChart()
		{
			if (chartDoubleBufferPanel.Controls.Count > 0) return;

			ApplicationForms.ShowLoadingIcon(PTS.Form);
			Application.RaiseIdle(null);

			ExceptionChartDelegate exceptionChartDelegate = ExceptionChart;
			exceptionChartDelegate.BeginInvoke(ExceptionChartCallback, exceptionChartDelegate);
		}

		private XmlDocument ExceptionChart()
		{
			var posIds = SearchCriteria.POS.ToArray();

			//if no pos, must set all pos id
			if (posIds.Length == 0)
				posIds = PTS.POS.POSServer.Select(pos => pos.Id).ToArray();

            var tempString = new List<String>();
            foreach (String posId in posIds)
            {
                tempString.Add(posId);
                if (PTS.POS.UsePTSId(posId))
                {
                    if (!tempString.Contains("PTS"))
                        tempString.Add("PTS");
                }
            }

		    posIds = tempString.ToArray();
			var conditionXml = Report.ReadExceptionCalculationByDateGroupByRegister.ObtainCondition(
				posIds,
				SearchCriteria.CashierIds.ToArray(),
				SearchCriteria.Cashiers.ToArray(),
				SearchCriteria.StartDateTime,
				SearchCriteria.EndDateTime,
				SearchCriteria.Exceptions.ToArray(),
				PTS.Server.TimeZone);

			return Report.ReadExceptionCalculationByDateGroupByRegister.Search(conditionXml, PTS.Credential);
		}

		private delegate XmlDocument ExceptionChartDelegate();
		private delegate void ExceptionChartCallbackDelegate(IAsyncResult result);
		private void ExceptionChartCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new ExceptionChartCallbackDelegate(ExceptionChartCallback), result);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			ApplicationForms.HideLoadingIcon();

			var resultXml = ((ExceptionChartDelegate)result.AsyncState).EndInvoke(result);

			if (resultXml == null) return;

			var posDic = PTS.POS.POSServer.ToDictionary(pos => pos.Id, pos => pos.Name);

			var reportViewer = new PTSReportViewer
			{
				POS = posDic,
				TimeZone = PTS.Server.TimeZone,
				ReportXmlDoc = resultXml,
				ReportType = "ExceptionChart",
				ShowZoomControl = false
			};

			chartDoubleBufferPanel.Controls.Add(reportViewer);

			//要呼叫 RefreshReport 才會繪製報表
			reportViewer.RefreshReport();
		}

		//---------------------------------------------------------------------------------------------------------------------
		public void SaveReport()
		{
			if (_searchResult == null || _searchResult.RawXml == null) return;

			ApplicationForms.ShowLoadingIcon(PTS.Form);
			Application.RaiseIdle(null);

			//SaveExceptionsXmlDelegate saveDelegate = SaveExceptionsXml;
			//saveDelegate.BeginInvoke(SaveReportCallback, saveDelegate);

			ShowReportForm(SaveExceptionsXml());
			ApplicationForms.HideLoadingIcon();
		}

		//---------------------------------------------------------------------------------------------------------------------
		private XmlDocument SaveExceptionsXml()
		{
			return _searchResult.RawXml;
		}

		//private delegate XmlDocument SaveExceptionsXmlDelegate();
		//private delegate void SaveReportCallbackDelegate(IAsyncResult result);
		//private void SaveReportCallback(IAsyncResult result)
		//{
		//    if (InvokeRequired)
		//    {
		//        try
		//        {
		//            Invoke(new SaveReportCallbackDelegate(SaveReportCallback), result);
		//        }
		//        catch (System.Exception)
		//        {
		//        }
		//        return;
		//    }

		//    ApplicationForms.HideLoadingIcon();

		//    var resultXml = ((SaveExceptionsXmlDelegate)result.AsyncState).EndInvoke(result);

		//    if (resultXml == null) return;

		//    ShowReportForm(resultXml);
		//}
		
		private void ShowReportForm(XmlDocument resultXml)
		{
			var saveReportForm = new SaveReportForm
			{
				Icon = PTS.Form.Icon,
				Text = Localization["PTSReports_SaveReport"],
				Size = new Size(965, 550)
			};

			var posDic = PTS.POS.POSServer.ToDictionary(pos => pos.Id, pos => pos.Name);

			var reportViewer = new PTSReportViewer
			{
				POS = posDic,
				TimeZone = PTS.Server.TimeZone,
				ReportXmlDoc = resultXml,
				ReportType = "ExceptionList",
				ShowZoomControl = false
			};
			saveReportForm.Controls.Add(reportViewer);

			//要呼叫 RefreshReport 才會繪製報表
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}
	}
}
