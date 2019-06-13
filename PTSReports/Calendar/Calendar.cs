using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using PanelBase;
using PTSReports.Base;
using SetupBase;
using Manager = SetupBase.Manager;

namespace PTSReports.Calendar
{
    public sealed partial class Calendar : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
        public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.calendarIcon, Properties.Resources.IMGCalendarIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.calendarIcon_activate, Properties.Resources.IMGCalendarIconActivate);
        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

		private IPTS _pts;
		public IApp App { get; set; }
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

		public POS_Exception.SearchCriteria SearchCriteria;

		public CriteriaPanel CriteriaPanel;
		public POSPanel POSPanel;
		public DateTimePanel DateTimePanel;
		public ExceptionPanel ExceptionPanel;
		public SearchPanel SearchPanel;
		private Exception.Exception _exceptionReport;

		private Control _focusControl;
		private Control _focusReport;
		public String TitleName { get; set; }

		public Calendar()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportCalendar", "Calendar"},

								   {"MessageBox_Information", "Information"},

								   {"PTSReports_POS", "POS"},
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_NotSelectPOS", "Not select POS"},
								   {"PTSReports_NotSelectExceptions", "Not select exceptions"},

								   {"PTSReports_SearchResult", "Search Result"},
							   };
			Localizations.Update(Localization);

			Name = "ReportCalendar";
			TitleName = Localization["Control_ReportCalendar"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);

			SearchCriteria = new POS_Exception.SearchCriteria
			{
				DateTimeSet = DateTimeSet.ThisMonth
			};

            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_ReportCalendar"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
		}

		private void SetDefaultCondition()
		{
			SearchCriteria.DateTimeSet = DateTimeSet.ThisMonth;
			var range = DateTimes.UpdateStartAndEndDateTime(_pts.Server.DateTime, _pts.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			SearchCriteria.POS.Clear();
			SearchCriteria.Exceptions.Clear();

			var sortResult = _pts.POS.POSServer.Select(pos => pos.Id).ToList();
            sortResult.Sort((x, y) => (x.CompareTo(y)));

			foreach (var pos in sortResult)
			{
				SearchCriteria.POS.Add(pos);
			}

			ExceptionPanel.ResetManufacture();
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			var sortResult = _pts.POS.POSServer.Select(pos => pos.Id).ToList();
            sortResult.Sort((x, y) => (x.CompareTo(y)));

			foreach (var pos in sortResult)
			{
				SearchCriteria.POS.Add(pos);
			}

			CriteriaPanel = new CriteriaPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria,
			};
			CriteriaPanel.Initialize();

			POSPanel = new POSPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria,
			};

			DateTimePanel = new DateTimePanel
			{
				PTS = _pts,
				DateSetArray = new[] { DateTimeSet.ThisMonth, DateTimeSet.LastMonth, DateTimeSet.TheMonthBeforeLast },
				DateTimeCriteria = SearchCriteria,
			};
			DateTimePanel.ShowMonthSelection();
			DateTimePanel.Initialize();

			ExceptionPanel = new ExceptionPanel
			{
				PTS = _pts,
				ShowExceptionColor = true,
				SearchCriteria = SearchCriteria,
			};

			SearchPanel = new SearchPanel
			{
				App = App,
				PTS = _pts,
				SearchCriteria = SearchCriteria,
			};
			SearchPanel.Initialize();

			_exceptionReport = new Exception.Exception
			{
				Server = _pts,
				App = App,
			};
			_exceptionReport.Initialize();

			_focusReport = this;

			CriteriaPanel.OnPOSEdit += CriteriaPanelOnPOSEdit;
			CriteriaPanel.OnDateTimeEdit += CriteriaPanelOnDateTimeEdit;
			CriteriaPanel.OnExceptionEdit += CriteriaPanelOnExceptionEdit;

			SearchPanel.OnExceptionSearchCriteriaChange += SearchPanelOnExceptionSearchCriteriaChange;
			
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

		private void CriteriaPanelOnPOSEdit(Object sender, EventArgs e)
		{
			_focusControl = POSPanel;

			POSPanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnDateTimeEdit(Object sender, EventArgs e)
		{
			_focusControl = DateTimePanel;

			DateTimePanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnExceptionEdit(Object sender, EventArgs e)
		{
			_focusControl = ExceptionPanel;

			ExceptionPanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void SearchPanelOnExceptionSearchCriteriaChange(Object sender, EventArgs<ExceptionReportParameter> e)
		{
			var parameter = e.Value;
			_exceptionReport.SearchCriteria.DateTimeSet = parameter.DateTimeSet;
			_exceptionReport.SearchCriteria.StartDateTime = parameter.StartDateTime;
			_exceptionReport.SearchCriteria.EndDateTime = parameter.EndDateTime;
			_exceptionReport.SearchCriteria.Exceptions.Clear();
			_exceptionReport.SearchCriteria.Exceptions.AddRange(parameter.Exceptions);
			_exceptionReport.SearchCriteria.POS.Clear();
			_exceptionReport.SearchCriteria.POS.AddRange(parameter.POS);

			SearchExceptionList();
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
						Localization["PTSReports_SearchResult"], "Back", "")));

				return;
			}

			if ( _focusControl == _exceptionReport.SearchPanel)
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
					if (_focusReport == this)
					{
						if (SearchCriteria.POS.Count == 0)
						{
							TopMostMessageBox.Show(Localization["PTSReports_NotSelectPOS"], Localization["MessageBox_Information"], MessageBoxButtons.OK);
							return;
						}

						if (SearchCriteria.Exceptions.Count == 0)
						{
							TopMostMessageBox.Show(Localization["PTSReports_NotSelectExceptions"], Localization["MessageBox_Information"], MessageBoxButtons.OK);
							return;
						}
						if (!_isSearching)
							SearchExceptions();
					}
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
						if (_focusControl == _exceptionReport.SearchPanel)
						{
							_focusControl = SearchPanel;
							_focusReport = this;

							Manager.ReplaceControl(_exceptionReport, SearchPanel, contentPanel, ManagerMoveBackToCalendarComplete);
							return;
						}

						Manager.ReplaceControl(_focusControl, CriteriaPanel, contentPanel, ShowCriteriaPanel);
					}
					break;
			}
		}

		private Boolean _isSearching;
		private void SearchExceptions()
		{
			_isSearching = true;
			_focusControl = SearchPanel;

			SearchPanel.ClearViewModel();

			Manager.ReplaceControl(CriteriaPanel, SearchPanel, contentPanel, ManagerMoveToCalendarSearchComplete);
		}
		
		private void SearchExceptionList()
		{
			_isSearching = true;
			_focusControl = _exceptionReport.SearchPanel;
			_focusReport = _exceptionReport;

			_exceptionReport.SearchPanel.ClearResult();

			Manager.ReplaceControl(SearchPanel, _exceptionReport.SearchPanel, contentPanel, ManagerMoveToSearchListComplete);
		}

		private void ShowCriteriaPanel()
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
			_focusControl.Focus();

			var text = TitleName + "  /  ";
			text += (Localization.ContainsKey("PTSReports_" + _focusControl.Name))
				? Localization["PTSReports_" + _focusControl.Name]
				: _focusControl.Name;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					text, "Back", "SearchException")));
		}

		private void ManagerMoveToCalendarSearchComplete()
		{
			_focusControl.Focus();

			SearchPanel.SearchExceptions(SearchCriteria.StartDateTime, SearchCriteria.EndDateTime);

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					Localization["PTSReports_SearchResult"], "Back", "")));
		}

		private void ManagerMoveBackToCalendarComplete()
		{
			_focusControl.Focus();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					Localization["PTSReports_SearchResult"], "Back", "")));
		}

		private void ManagerMoveToSearchListComplete()
		{
			_focusControl.Focus();

			_exceptionReport.SearchPanel.SearchExceptions(1);//Page 1

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
			if (_focusControl != _exceptionReport.SearchPanel) return;

			_exceptionReport.SearchPanel.SaveReport();
		}

		private void POSOnPOSModify(Object sender, EventArgs<IPOS> e)
		{
			if (e.Value.ReadyState == ReadyState.Delete)
			{
				SearchCriteria.POS.Remove(e.Value.Id);
			}
		}
	}
}
