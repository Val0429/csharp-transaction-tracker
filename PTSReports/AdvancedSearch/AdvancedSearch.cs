using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using PanelBase;
using PTSReports.Base;
using PTSReportsGenerator;
using SetupBase;
using Manager = SetupBase.Manager;
using ApplicationForms = PanelBase.ApplicationForms;

namespace PTSReports.AdvancedSearch
{
    public sealed partial class AdvancedSearch : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
        public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.advancedSearchIcon, Properties.Resources.IMGAdvancedSearchIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.advancedSearchIcon_activate, Properties.Resources.IMGAdvancedSearchActivate);
        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

		public IApp App { get; set; }
		private IPTS _pts;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
				if (value is IPTS)
				{
					_pts = value as IPTS;
					_pts.OnPOSModify += POSOnPOSModify;
				}
			}
		}
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public POS_Exception.AdvancedSearchCriteria SearchCriteria;

		public CriteriaPanel CriteriaPanel;
		public DateTimePanel DateTimePanel;
		public SearchPanel SearchPanel;
		public POSPanel POSPanel;
		public CashierIdPanel CashierIdPanel;
		public CashierPanel CashierPanel;
		public ExceptionPanel ExceptionPanel;
		public ExceptionAmountPanel ExceptionAmountPanel;
		public TagPanel TagPanel;
		public KeywordPanel KeywordPanel;
        public TimeIntervalPanel TimeIntervalPanel;
        public CountingPanel CountingPanel;

		private Control _focusControl;
		public String TitleName { get; set; }

		public AdvancedSearch()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_AdvancedSearch", "Advance Search"},

								   {"MessageBox_Information", "Information"},

								   {"PTSReports_POS", "POS"},
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_CashierId", "Cashier Id"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_ExceptionAmount", "ExceptionAmount"},
								   {"PTSReports_Tag", "Tag"},
								   {"PTSReports_Keyword", "Keyword"},
                                   {"PTSReports_TimeIntervel", "Time Interval"},
                                   {"PTSReports_CountingDiscrepancies", "Counting Discrepancies"},

								   {"PTSReports_NotSelectPOS", "Not select POS"},
								   {"PTSReports_NotSelectExceptions", "Not select exceptions"},

								   {"PTSReports_SearchResult", "Search Result"},
								   {"PTSReports_SaveReport", "Save Report"},
							   };
			Localizations.Update(Localization);

			Name = "AdvanceSearch";
			TitleName = Localization["Control_AdvancedSearch"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);

			SearchCriteria = new POS_Exception.AdvancedSearchCriteria
			{
				DateTimeSet = DateTimeSet.Today
			};

            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_AdvancedSearch"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
		}

		private void SetDefaultCondition()
		{
			SearchCriteria.DateTimeSet = DateTimeSet.Today;
			var range = DateTimes.UpdateStartAndEndDateTime(_pts.Server.DateTime, _pts.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			SearchCriteria.POSCriterias.Clear();

			foreach (IPOS obj in _pts.POS.POSServer)
			{
				SearchCriteria.POSCriterias.Add(new POS_Exception.POSCriteria
				{
					POSId = obj.Id,
					Equation = POS_Exception.Comparative.Equal
				});
			}

			SearchCriteria.ExceptionCriterias.Clear();
			SearchCriteria.ExceptionAmountCriterias.Clear();
			SearchCriteria.CashierIdCriterias.Clear();
			SearchCriteria.CashierCriterias.Clear();
			SearchCriteria.TagCriterias.Clear();
			SearchCriteria.KeywordCriterias.Clear();
			SearchCriteria.PageIndex = 1;

			CashierIdPanel.Initialize();
			CashierPanel.Initialize();
			ExceptionAmountPanel.Initialize();
			ExceptionPanel.Initialize();
			KeywordPanel.Initialize();
			TagPanel.Initialize();
            TimeIntervalPanel.Initialize();
            CountingPanel.Initialize();
		}

		public Boolean SetDefaultPOSConfig = true;
		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			var range = DateTimes.UpdateStartAndEndDateTime(_pts.Server.DateTime, _pts.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			if (SetDefaultPOSConfig)
			{
				foreach (IPOS obj in _pts.POS.POSServer)
				{
					SearchCriteria.POSCriterias.Add(new POS_Exception.POSCriteria
					{
						POSId = obj.Id,
						Equation = POS_Exception.Comparative.Equal
					});
				}
			}

			CriteriaPanel = new CriteriaPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria
			};
			CriteriaPanel.Initialize();

			CashierIdPanel = new CashierIdPanel
			{
				PTS = _pts,
				CashierIdCriterias = SearchCriteria.CashierIdCriterias
			};
			CashierIdPanel.Initialize();

			CashierPanel = new CashierPanel
			{
				PTS = _pts,
				CashierCriterias = SearchCriteria.CashierCriterias
			};
			CashierPanel.Initialize();

			POSPanel = new POSPanel
			{
				PTS = _pts,
				POSCriterias = SearchCriteria.POSCriterias
			};

			DateTimePanel = new DateTimePanel
			{
				PTS = _pts,
				DateTimeCriteria = SearchCriteria,
				DateSetArray = new[] { DateTimeSet.Today, DateTimeSet.Yesterday, DateTimeSet.DayBeforeYesterday, DateTimeSet.ThisWeek,
					DateTimeSet.ThisMonth, DateTimeSet.LastMonth, DateTimeSet.TheMonthBeforeLast},
			};
			DateTimePanel.ShowDateTimeSelectionRange();
			DateTimePanel.Initialize();

			ExceptionPanel = new ExceptionPanel
			{
				PTS = _pts,
				ExceptionCriterias = SearchCriteria.ExceptionCriterias
			};
			ExceptionPanel.Initialize();

			ExceptionAmountPanel = new ExceptionAmountPanel
			{
				PTS = _pts,
				ExceptionAmountCriterias = SearchCriteria.ExceptionAmountCriterias
			};
			ExceptionAmountPanel.Initialize();

			TagPanel = new TagPanel
			{
				PTS = _pts,
				TagCriterias = SearchCriteria.TagCriterias
			};
			TagPanel.Initialize();

			KeywordPanel = new KeywordPanel
			{
				PTS = _pts,
				KeywordCriterias = SearchCriteria.KeywordCriterias
			};
			KeywordPanel.Initialize();

            TimeIntervalPanel = new TimeIntervalPanel
            {
                PTS = _pts,
                TimeIntervalCriteria = SearchCriteria.TimeIntervalCriteria
            };
            TimeIntervalPanel.Initialize();

            CountingPanel = new CountingPanel()
            {
                PTS = _pts,
                CountingCriteria = SearchCriteria.CountingCriteria
            };
            TimeIntervalPanel.Initialize();

			SearchPanel = new SearchPanel
			{
				PTS = _pts,
				App = App,
				SearchCriteria = SearchCriteria
			};

			CriteriaPanel.OnDateTimeEdit += CriteriaPanelOnDateTimeEdit;
			CriteriaPanel.OnPOSEdit += CriteriaPanelOnPOSEdit;
			CriteriaPanel.OnCashierIdEdit += CriteriaPanelOnCashierIdEdit;
			CriteriaPanel.OnCashierEdit += CriteriaPanelOnCashierEdit;
			CriteriaPanel.OnExceptionEdit += CriteriaPanelOnExceptionEdit;
			CriteriaPanel.OnExceptionAmountEdit += CriteriaPanelOnExceptionAmountEdit;
			CriteriaPanel.OnTagEdit += CriteriaPanelOnTagEdit;
			CriteriaPanel.OnKeywordEdit += CriteriaPanelOnKeywordEdit;
            CriteriaPanel.OnTimeIntervalEdit += CriteriaPanelOnTimeIntervalEdit;
            CriteriaPanel.OnCountingEdit += CriteriaPanelOnCountingEdit;

			contentPanel.Controls.Contains(CriteriaPanel);
		}

		private Boolean _isActivate;
		public void Activate()
		{
			if (!BlockPanel.IsFocusedControl(this)) return;

			_isActivate = true;
		}

		public void Deactivate()
		{
			_isActivate = false;
		}

		private void CriteriaPanelOnDateTimeEdit(Object sender, EventArgs e)
		{
			_focusControl = DateTimePanel;

			DateTimePanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnPOSEdit(Object sender, EventArgs e)
		{
			_focusControl = POSPanel;

			POSPanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnCashierIdEdit(Object sender, EventArgs e)
		{
			_focusControl = CashierIdPanel;

			CashierIdPanel.ScrollTop();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnCashierEdit(Object sender, EventArgs e)
		{
			_focusControl = CashierPanel;

			CashierPanel.ScrollTop();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnExceptionEdit(Object sender, EventArgs e)
		{
			_focusControl = ExceptionPanel;

			ExceptionPanel.ScrollTop();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnExceptionAmountEdit(Object sender, EventArgs e)
		{
			_focusControl = ExceptionAmountPanel;

			//ExceptionAmountPanel.CheckResetException();
			ExceptionAmountPanel.ScrollTop();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnTagEdit(Object sender, EventArgs e)
		{
			_focusControl = TagPanel;

			TagPanel.ScrollTop();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnKeywordEdit(Object sender, EventArgs e)
		{
			_focusControl = KeywordPanel;

			KeywordPanel.ScrollTop();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}


        private void CriteriaPanelOnTimeIntervalEdit(object sender, EventArgs e)
        {
            _focusControl = TimeIntervalPanel;

            TimeIntervalPanel.ScrollTop();

            Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
        }

        private void CriteriaPanelOnCountingEdit(object sender, EventArgs e)
        {
            _focusControl = CountingPanel;

            CountingPanel.ScrollTop();

            Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
        }

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowCriteriaPanel();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			if (BlockPanel != null)
				BlockPanel.ShowThisControlPanel(this);

			//keep search result, even change to other report page
			if (_focusControl == SearchPanel)
			{
				if (OnSelectionChange != null)
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						Localization["PTSReports_SearchResult"], "Back", "SaveReport")));

				return;
			}
			ShowCriteriaPanel();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "SearchException":
					if (SearchCriteria.POSCriterias.Count == 0)
					{
						TopMostMessageBox.Show(Localization["PTSReports_NotSelectPOS"], Localization["MessageBox_Information"], MessageBoxButtons.OK);
						return;
					}
					if (!_isSearching)
						SearchExceptions();
					break;

				case "ClearConditional":
					SetDefaultCondition();
					CriteriaPanel.Invalidate();
					break;

				case "SaveReport":
					SaveReport();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (_previousSettingControl != null && _previousSettingControl != CriteriaPanel)
						{
							_focusControl = _previousSettingControl;
							Manager.ReplaceControl(SearchPanel, _previousSettingControl, contentPanel, ManagerMoveToSettingComplete);
						}
						else
						{
							Manager.ReplaceControl(_focusControl, CriteriaPanel, contentPanel, ShowCriteriaPanel);
						}
					}
					break;
			}
		}

		private Boolean _isSearching;
		private Control _previousSettingControl;
		private void SearchExceptions()
		{
			_isSearching = true;
			_previousSettingControl = _focusControl;
			_focusControl = SearchPanel;

			SearchCriteria.PageIndex = 1;
			SearchPanel.ClearResult();

			Manager.ReplaceControl(CriteriaPanel, SearchPanel, contentPanel, ManagerMoveToSearchComplete);
		}

		public void ShowCriteriaPanel()
		{
			_isSearching = false;

			_focusControl = CriteriaPanel;

			if (!contentPanel.Controls.Contains(CriteriaPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(CriteriaPanel);
			}

			CriteriaPanel.ShowCriteria();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "SearchException,ClearConditional")));
		}

		private void ManagerMoveToSettingComplete()
		{
			_isSearching = false;
			_previousSettingControl = null;
			_focusControl.Focus();

			if (_focusControl == ExceptionPanel)
				ExceptionPanel.ScrollTop();
			else if (_focusControl == ExceptionAmountPanel)
				ExceptionAmountPanel.ScrollTop();
			else if (_focusControl == TagPanel)
				TagPanel.ScrollTop();
			else if (_focusControl == KeywordPanel)
				KeywordPanel.ScrollTop();
			else if (_focusControl == POSPanel)
				POSPanel.ScrollTop();
			else if (_focusControl == CashierIdPanel)
				CashierIdPanel.ScrollTop();
			else if (_focusControl == CashierPanel)
				CashierPanel.ScrollTop();
            else if (_focusControl == TimeIntervalPanel)
                TimeIntervalPanel.ScrollTop();
            else if (_focusControl == CountingPanel)
                CountingPanel.ScrollTop();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					(Localization.ContainsKey("PTSReports_" + _focusControl.Name)) ? Localization["PTSReports_" + _focusControl.Name] : _focusControl.Name
					, "Back", "SearchException")));
		}

		private void ManagerMoveToSearchComplete()
		{
			SearchPanel.Focus();

			SearchPanel.SearchExceptions(1);//Page 1

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					Localization["PTSReports_SearchResult"], "Back", "SaveReport")));
		}

		public void SaveReport(Object sender, EventArgs e)
		{
			if (!_isActivate) return;

			SaveReport();
		}

		public void SaveReport()
		{
			if (_focusControl != SearchPanel || SearchPanel.ReportXml == null) return;

			ApplicationForms.ShowLoadingIcon(Server.Form);
			Application.RaiseIdle(null);

			//SearchReportListDelegate searchReportListDelegate = SearchReportList;
			//searchReportListDelegate.BeginInvoke(SearchReportListCallback, searchReportListDelegate);

			ShowReportForm(SearchReportList());
			ApplicationForms.HideLoadingIcon();
		}

		public XmlDocument SearchReportList()
		{
			//use previous resilt
			return SearchPanel.ReportXml;

			//query AGAIN
			//var conditionXml = new XmlDocument();
			//return _pts.POS.ReadTransactionByCondition(SearchCriteria, SearchCriteria.StartDateTime, SearchCriteria.EndDateTime, ref conditionXml);
		}

		private delegate XmlDocument SearchReportListDelegate();
		private delegate void SearchReportListCallbackDelegate(IAsyncResult result);
		private void SearchReportListCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new SearchReportListCallbackDelegate(SearchReportListCallback), result);
				}
				catch (System.Exception)
				{
				}
				return;
			}

			ApplicationForms.HideLoadingIcon();

			var resultXml = ((SearchReportListDelegate)result.AsyncState).EndInvoke(result);

			if (resultXml == null) return;

			ShowReportForm(resultXml);
		}

		private void ShowReportForm(XmlDocument resultXml)
		{

			var saveReportForm = new SaveReportForm
			{
				Icon = Server.Form.Icon,
				Text = Localization["PTSReports_SaveReport"],
				Size = new Size(850, 550)
			};

			var posDic = _pts.POS.POSServer.ToDictionary(pos => pos.Id, pos => pos.Name);

			var reportViewer = new PTSReportViewer
			{
				POS = posDic,
				TimeZone = Server.Server.TimeZone,
				ReportXmlDoc = resultXml,
				ReportType = "TransactionList",
			};
			saveReportForm.Controls.Add(reportViewer);

			//要呼叫 RefreshReport 才會繪製報表
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}

		public Control RootPanel
		{
			get { return contentPanel; }
		}

		public Boolean IsAtIndexPage
		{
			get { return _focusControl == CriteriaPanel; }
		}

		public Boolean IsAtSearchPanel
		{
			get { return _focusControl == SearchPanel; }
		}

		private void POSOnPOSModify(Object sender, EventArgs<IPOS> e)
		{
			if (e.Value.ReadyState == ReadyState.Delete)
			{
				var pos = SearchCriteria.POSCriterias.ToArray();
				foreach (var criteria in pos)
				{
					if (criteria.POSId == e.Value.Id)
						SearchCriteria.POSCriterias.Remove(criteria);
				}
			}
		}
	}
}
