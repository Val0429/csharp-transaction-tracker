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
using ApplicationForms = PanelBase.ApplicationForms;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class SearchPanel : UserControl
	{
		public IApp App;
		public IPTS PTS;
		public POS_Exception.AdvancedSearchCriteria SearchCriteria;
		public Dictionary<String, String> Localization;

		private readonly PageSelector _pageSelector;
		public SearchPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"Common_UsedSeconds", "(%1 seconds)"},
								   
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

			resultLabel.Text = Localization["PTSReports_SearchResult"];
		    resultLabel.Visible = false;
			foundLabel.Text = "";

			SearchCriteria = new POS_Exception.AdvancedSearchCriteria();
		}

		private readonly List<TransactionResultPanel> _recycleTransactionResult = new List<TransactionResultPanel>();
		private TransactionResultPanel GetExceptionResultPanel()
		{
			TransactionResultPanel searchResultPanel = null;
			if (_recycleTransactionResult.Count > 0)
			{
				foreach (TransactionResultPanel resultPanel in _recycleTransactionResult)
				{
					if (resultPanel.IsLoadingImage) continue;

					searchResultPanel = resultPanel;
					break;
				}

				if (searchResultPanel != null)
				{
					_recycleTransactionResult.Remove(searchResultPanel);
					return searchResultPanel;
				}
			}

			
			var exceptionResultPanel = new TransactionResultPanel
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

		private Boolean _clearModel;
		private XmlDocument _conditionXml;
		public void SearchExceptions(UInt16 page)
		{
			if (_isSearching) return;
            resultLabel.Visible = true;

			_clearModel = true;
			_page = page;

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

			_conditionXml = new XmlDocument();
			SearchTransactionByConditionDelegate searchDelegate = PTS.POS.ReadTransactionByCondition;
			searchDelegate.BeginInvoke(SearchCriteria, SearchCriteria.StartDateTime, SearchCriteria.EndDateTime, ref _conditionXml, SearchReportCallback, searchDelegate);
		}

		private delegate XmlDocument SearchTransactionByConditionDelegate(POS_Exception.AdvancedSearchCriteria criteria, UInt64 startutc, UInt64 endutc, ref XmlDocument conditionXml);
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
			
			var resultList = new POS_Exception.TransactionList();
			var reportXml = ((SearchTransactionByConditionDelegate)result.AsyncState).EndInvoke(ref _conditionXml, result);

			if (reportXml != null)
				resultList = Report.ReadTransactionByCondition.Parse(reportXml, _conditionXml, PTS.Server.TimeZone);
				
			UpdateResult(resultList);
		}

		private delegate XmlDocument SearchTransactionByPageIndexDelegate(XmlDocument xmlDoc);
		private delegate void SearchReportByPageCallbackDelegate(IAsyncResult result);
		private void SearchReportByPageCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new SearchReportByPageCallbackDelegate(SearchReportByPageCallback), result);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			_watch.Stop();

			var reportXml = ((SearchTransactionByPageIndexDelegate)result.AsyncState).EndInvoke(result);
			var list = Report.ReadTransactionByCondition.Parse(reportXml, _conditionXml, PTS.Server.TimeZone);
			
			UpdateResult(list);
		}

		public XmlDocument ReportXml
		{
			get { return (_searchResult == null)
				? null
				: _searchResult.RawXml; }
		}

		private POS_Exception.TransactionList _searchResult;
		private void UpdateResult(POS_Exception.TransactionList searchResult)
		{
			_searchResult = searchResult;
			if (_searchResult.Count == 0)
			{
				if (!_clearModel)
				{
					ClearViewModel();
				}

			    foundLabel.Text = Localization["PTSReports_SearchNoResult"];
					 //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));
			}
			else
			{
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
				foreach (var transaction in _searchResult.Results)
				{
					var resultPanel = GetExceptionResultPanel();
					resultPanel.Id = count + ((_page - 1) * CountPerPage);
					resultPanel.Transaction = transaction;
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
			if (_isSearching) return;

			_clearModel = false;
			_page = _pageSelector.SelectPage;

			ApplicationForms.ShowLoadingIcon(PTS.Form);

			_isSearching = true;
			_watch.Reset();
			_watch.Start();

			SearchCriteria.PageIndex = _page;
			ConvertStringToXml();

			SearchTransactionByPageIndexDelegate searchDelegate = PTS.POS.ReadTransactionByCondition;
			searchDelegate.BeginInvoke(_conditionXml, SearchReportByPageCallback, searchDelegate);
		}

		public void ConvertStringToXml()
		{
			if (_searchResult == null) return;

			_conditionXml = new XmlDocument();
			_conditionXml.LoadXml(_searchResult.SearchCondition);

			var pageNode = Xml.GetFirstElementByTagName(_conditionXml, "Page");
			if (pageNode != null)
				pageNode.SetAttribute("index", SearchCriteria.PageIndex.ToString());
		}

		private void ExceptionResultPanelOnPlayback(Object sender, EventArgs e)
		{
			var panel = sender as TransactionResultPanel;
			if(panel == null) return;

			var pos = PTS.POS.FindPOSById(panel.Transaction.POSId);
			if (pos == null) return;

			var allPos = true;
			var posIds = SearchCriteria.POSCriterias.Select(posCriteria => posCriteria.POSId).ToList();
			foreach (IPOS obj in PTS.POS.POSServer)
			{
				if (!posIds.Contains(obj.Id))
				{
					allPos = false;
					break;
				}
			}

			App.SwitchPage("Playback", new TransactionListParameter
			{
				Index  = panel.Id,
				Transaction = panel.Transaction,
				TransactionList = _searchResult,
                POS = (allPos) ? posIds : new List<String> { panel.Transaction.POSId },
				StartDateTime = SearchCriteria.StartDateTime,
				EndDateTime = Math.Max(SearchCriteria.EndDateTime, SearchCriteria.StartDateTime + 2000),//SearchCriteria.EndDateTime,
				Exceptions = SearchCriteria.ExceptionAmountCriterias.Select(exceptionCriteria => exceptionCriteria.Exception).ToList(),
				ExceptionDateTime = panel.Transaction.UTC,
                SearchCriteria = SearchCriteria
			});
		}

		private const UInt16 MaximumConnection = 3;
		private UInt16 _connection;
		public List<TransactionResultPanel> QueueSearchResultPanel = new List<TransactionResultPanel>();
		public void QueueLoadSnapshot(TransactionResultPanel searchResultPanel)
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
		
		public void ClearResult()
		{
			_pageSelector.ClearViewModel();
			foundLabel.Text = "";

			ClearViewModel();
		}

		public void ClearViewModel()
		{
			if (containerPanel.Controls.Count == 0) return;

			foreach (TransactionResultPanel control in containerPanel.Controls)
			{
				control.Reset();

				if (!_recycleTransactionResult.Contains(control))
					_recycleTransactionResult.Add(control);
			}

			containerPanel.Controls.Clear();
		}
	}
}
