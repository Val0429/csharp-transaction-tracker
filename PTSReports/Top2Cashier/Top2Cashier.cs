using System;
using System.Collections.Generic;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using PanelBase;
using PTSReports.Base;
using SetupBase;
using Manager = SetupBase.Manager;

namespace PTSReports.Top2Cashier
{
	public sealed partial class Top2Cashier : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse
	{
		public event EventHandler<EventArgs<String>> OnSelectionChange;

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
		public ExceptionPanel ExceptionPanel;
		private Exception.Exception _exceptionReport;

		private Control _focusControl;
		private Control _focusReport;
		public String TitleName { get; set; }

		public Top2Cashier()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_Top2ReportCashier", "Top 2 Cashier"},

								   {"MessageBox_Information", "Information"},
								   {"PTSReports_Exception", "Exception"},

								   {"PTSReports_SearchResult", "Search Result"},
								   {"PTSReports_NotSelectExceptions", "Not select exceptions"},
							   };
			Localizations.Update(Localization);

			Name = "ReportTop2Cashier";
			TitleName = Localization["Control_Top2ReportCashier"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);

			SearchCriteria = new POS_Exception.SearchCriteria
			{
				DateTimeSet = DateTimeSet.Weekly
			};
		}

		private void SetDefaultCondition()
		{
			SearchCriteria.DateTimeSet = DateTimeSet.Weekly;
			var range = DateTimes.UpdateStartAndEndDateTime(_pts.Server.DateTime, _pts.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			SearchCriteria.Exceptions.Clear();

			CriteriaPanel.ResetPeriod();
			ExceptionPanel.ResetManufacture();
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			var range = DateTimes.UpdateStartAndEndDateTime(_pts.Server.DateTime, _pts.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			CriteriaPanel = new CriteriaPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria
			};
			CriteriaPanel.Initialize();

			_exceptionReport = new Exception.Exception
			{
				Server = _pts,
				App = App,
			};
			_exceptionReport.Initialize();

			ExceptionPanel = new ExceptionPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria
			};

			_focusReport = this;

			CriteriaPanel.OnExceptionSearchCriteriaChange += CriteriaPanelOnExceptionSearchCriteriaChange;
			CriteriaPanel.OnExceptionEdit += CriteriaPanelOnExceptionEdit;

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

		private void CriteriaPanelOnExceptionSearchCriteriaChange(Object sender, EventArgs<CashierExceptionReportParameter> e)
		{
			var parameter = e.Value;
			_exceptionReport.SearchCriteria.DateTimeSet = parameter.DateTimeSet;
			_exceptionReport.SearchCriteria.StartDateTime = parameter.StartDateTime;
			_exceptionReport.SearchCriteria.EndDateTime = parameter.EndDateTime;
			_exceptionReport.SearchCriteria.Exceptions.Clear();
			_exceptionReport.SearchCriteria.Exceptions.AddRange(parameter.Exceptions);
			_exceptionReport.SearchCriteria.POS.Clear();
			_exceptionReport.SearchCriteria.CashierIds.Clear();
			_exceptionReport.SearchCriteria.Cashiers.Clear();

			//if (!String.IsNullOrEmpty(parameter.CashierId))
			if (parameter.CashierId != null)
				_exceptionReport.SearchCriteria.CashierIds.Add(parameter.CashierId);

			//if (!String.IsNullOrEmpty(parameter.Cashier))
			if (parameter.Cashier != null)
				_exceptionReport.SearchCriteria.Cashiers.Add(parameter.Cashier);

			SearchExceptionList();
		}

		private void CriteriaPanelOnExceptionEdit(Object sender, EventArgs e)
		{
			_focusControl = ExceptionPanel;

			ExceptionPanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void ManagerMoveToSettingComplete()
		{
			_focusControl.Focus();

			if (_focusControl == ExceptionPanel)
				ExceptionPanel.ScrollTop();

			var text = TitleName + "  /  ";
			text += (Localization.ContainsKey("PTSReports_" + _focusControl.Name))
						? Localization["PTSReports_" + _focusControl.Name]
						: _focusControl.Name;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					text, "Back", "SearchException")));
		}

		private void SearchExceptionList()
		{
			_focusControl = _exceptionReport.SearchPanel;
			_focusReport = _exceptionReport;

			_exceptionReport.SearchPanel.ClearResult();

			Manager.ReplaceControl(CriteriaPanel, _exceptionReport.SearchPanel, contentPanel, ManagerMoveToSearchListComplete);
		}

		private void ManagerMoveToSearchListComplete()
		{
			_focusControl.Focus();

			_exceptionReport.SearchPanel.SearchExceptions(1);//Page 1

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					Localization["PTSReports_SearchResult"], "Back", "SaveReport")));
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			if (BlockPanel != null)
				BlockPanel.ShowThisControlPanel(this);

			//keep search result, even change to other report page
			if (_focusControl == _exceptionReport.SearchPanel)
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
					if (SearchCriteria.Exceptions.Count == 0)
					{
						TopMostMessageBox.Show(Localization["PTSReports_NotSelectExceptions"], Localization["MessageBox_Information"], MessageBoxButtons.OK);
						return;
					}

					if (_focusReport == this)
					{
						ShowCriteriaPanel();
						CriteriaPanel.SearchCashierExceptionSummaryReport();
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
						ShowCriteriaPanel();
					}
					break;
			}
		}

		public void ShowCriteriaPanel()
		{
			_focusControl = CriteriaPanel;
			_focusReport = this;

			if (!contentPanel.Controls.Contains(CriteriaPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(CriteriaPanel);
			}

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "SearchException,ClearConditional,SaveReport")));
		}

		public void SaveReport(Object sender, EventArgs e)
		{
			if (!_isActivate) return;

			SaveReport();
		}

		public void SaveReport()
		{
			if (_focusControl == CriteriaPanel)
				CriteriaPanel.SaveReport();
			else if (_focusControl == _exceptionReport.SearchPanel)
				_exceptionReport.SearchPanel.SaveReport();
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
			get { return ((_focusControl == CriteriaPanel) || (_focusControl == _exceptionReport.SearchPanel)); }
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
