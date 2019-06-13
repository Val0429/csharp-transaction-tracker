using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using PTSReports.Base;
using SetupBase;
using Manager = SetupBase.Manager;

namespace PTSReports.ExceptionTrend
{
	public sealed partial class ExceptionTrend : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse
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

		private Control _focusControl;
		private Control _focusReport;
		public String TitleName { get; set; }

		public ExceptionTrend()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportExceptionTrend", "Exception Trend"},

								   {"MessageBox_Information", "Information"},
								   {"PTSReports_Exception", "Exception"},

								   {"PTSReports_SearchResult", "Search Result"},
								   {"PTSReports_NotSelectExceptions", "Not select exceptions"},
							   };
			Localizations.Update(Localization);

			Name = "ReportExceptionTrend";
			TitleName = Localization["Control_ReportExceptionTrend"];

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

			ExceptionPanel = new ExceptionPanel
			{
				PTS = _pts,
				SearchCriteria = SearchCriteria
			};

			_focusReport = this;

			//CriteriaPanel.OnExceptionSearchCriteriaChange += CriteriaPanelOnExceptionSearchCriteriaChange;
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

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			if (BlockPanel != null)
				BlockPanel.ShowThisControlPanel(this);

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
						CriteriaPanel.SearchCashierExceptionTrendReport();
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
			get { return (_focusControl == CriteriaPanel); }
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
