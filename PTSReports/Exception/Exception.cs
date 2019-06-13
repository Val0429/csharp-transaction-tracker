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

namespace PTSReports.Exception
{
    public sealed partial class Exception : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
        public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.exceptionIcon, Properties.Resources.IMGExceptionIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.exceptionIcon_activate, Properties.Resources.IMGExceptionActivate);
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

		public POS_Exception.SearchCriteria SearchCriteria;

		public CriteriaPanel CriteriaPanel;
		public POSPanel POSPanel;
		public DateTimePanel DateTimePanel;
		public ExceptionPanel ExceptionPanel;
		public SearchPanel SearchPanel;

		private Control _focusControl;
		public String TitleName { get; set; }

		public Exception()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportException", "Exception Search"},

								   {"MessageBox_Information", "Information"},

								   {"PTSReports_POS", "POS"},
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_NotSelectPOS", "Not select POS"},
								   {"PTSReports_NotSelectExceptions", "Not select exceptions"},

								   {"PTSReports_SearchResult", "Search Result"},
							   };
			Localizations.Update(Localization);

			Name = "ReportException";
			TitleName = Localization["Control_ReportException"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);

			SearchCriteria = new POS_Exception.SearchCriteria
			{
				DateTimeSet = DateTimeSet.Today
			};

            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_ReportException"] };
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

			SearchCriteria.POS.Clear();
			//SearchCriteria.Cashiers.Clear();
			//SearchCriteria.CashierIds.Clear();
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

			var range = DateTimes.UpdateStartAndEndDateTime(_pts.Server.DateTime, _pts.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			var sortResult = _pts.POS.POSServer.Select(pos => pos.Id).ToList();
            sortResult.Sort((x, y) => (x.CompareTo(y)));

			foreach (var pos in sortResult)
			{
				SearchCriteria.POS.Add(pos);
			}

			CriteriaPanel = new CriteriaPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria
			};
			CriteriaPanel.Initialize();

			POSPanel = new POSPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria
			};

			DateTimePanel = new DateTimePanel
			{
				PTS = _pts,
				DateTimeCriteria = SearchCriteria,
				DateSetArray = new[] { DateTimeSet.Today, DateTimeSet.Yesterday, DateTimeSet.DayBeforeYesterday, DateTimeSet.ThisWeek },
			};
			DateTimePanel.ShowDateTimeSelectionRange();
			DateTimePanel.Initialize();

			ExceptionPanel = new ExceptionPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria
			};

			SearchPanel = new SearchPanel
			{
				PTS = _pts,
				App = App,
				SearchCriteria = SearchCriteria
			};

			CriteriaPanel.OnPOSEdit += CriteriaPanelOnPOSEdit;
			CriteriaPanel.OnDateTimeEdit += CriteriaPanelOnDateTimeEdit;
			CriteriaPanel.OnExceptionEdit += CriteriaPanelOnExceptionEdit;

			contentPanel.Controls.Contains(CriteriaPanel);
		}

		public void ApplySwitchPageParameter()
		{
			App.OnSwitchPage += SearchCriteriaChange;
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

		public void SearchCriteriaChange(Object sender, EventArgs<String, Object> e)
		{
			if (!String.Equals(e.Value1, "Report")) return;

			var exceptionsReportParameter = e.Value2 as ExceptionReportParameter;
			if (exceptionsReportParameter == null) return;

			SearchCriteria.POS.Clear();
			SearchCriteria.POS.AddRange(exceptionsReportParameter.POS.ToArray());

			SearchCriteria.Exceptions.Clear();
			SearchCriteria.Exceptions.AddRange(exceptionsReportParameter.Exceptions.ToArray());

			SearchCriteria.DateTimeSet = exceptionsReportParameter.DateTimeSet;
			if (SearchCriteria.DateTimeSet == DateTimeSet.None)
			{
				SearchCriteria.StartDateTime = exceptionsReportParameter.StartDateTime;
				SearchCriteria.EndDateTime = exceptionsReportParameter.EndDateTime;
			}
			else
			{
				var range = DateTimes.UpdateStartAndEndDateTime(_pts.Server.DateTime, _pts.Server.TimeZone, SearchCriteria.DateTimeSet);
				SearchCriteria.StartDateTime = range[0];
				SearchCriteria.EndDateTime = range[1];
			}

			//update  condition

			ExceptionPanel.ParseSetting();
			POSPanel.ParseSetting();
			DateTimePanel.ParseSetting();

			//ShowCriteriaPanel();
			SearchExceptions();

		    //DockIconClick(this, null);
            if (IsMinimize)
                Maximize();
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
							ShowCriteriaPanel();
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

			if (_focusControl != SearchPanel)
				_previousSettingControl = _focusControl;

			_focusControl = SearchPanel;

			SearchPanel.ClearResult();

			Manager.ReplaceControl(CriteriaPanel, SearchPanel, contentPanel, ManagerMoveToSearchComplete);
		}

		private void ShowCriteriaPanel()
		{
			_isSearching = false;

			_previousSettingControl = null;
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
			else if (_focusControl == POSPanel)
				POSPanel.ScrollTop();

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
			if (_focusControl != SearchPanel) return;

			SearchPanel.SaveReport();
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
