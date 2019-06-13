using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace PTSReports.Tracking
{
    public sealed partial class Tracking : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
        public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.trackingIcon, Properties.Resources.IMGTrackingIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.trackingIcon_activate, Properties.Resources.IMGTrackingIconActivate);
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
					_pts = value as IPTS;
			}
		}
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		private Control _focusReport;
		private IndexPanel _indexPanel;

		private Summary.Summary _b1Report;
		private Cashier.Cashier _b2Report;
		private ThresholdDeviation.ThresholdDeviation _b3Report;

		private Top2Cashier.Top2Cashier _c1Report;
		private ExceptionPercentage.ExceptionPercentage _c2Report;
		private ExceptionTrend.ExceptionTrend _c3Report;

		public Tracking()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ReportTracking", "Report"},

								   {"PTSReports_SearchResult", "Search Result"},
								   
								   {"PTSReports_FrequentExceptionCategoriesIncurred", "Frequent exception categories"},
								   {"PTSReports_FrequentExceptionsIncurredByEmployees", "Frequent exception categories by employees"},
								   {"PTSReports_PerformanceDeviationAgainstPredefinedThresholds", "Performance deviation against predefined thresholds"},
								   {"PTSReports_HighAlertTop2EmployeesExceptionMonitoring", "Ranking of incidence with counts and employees"},
								   {"PTSReports_EmployeeProductivityLossRanking", "Ranking of employees with the highest incidence"},
								   {"PTSReports_EmployeeProductivityImprovementTrend", "Incidence Trend of employees"},
							   };
			Localizations.Update(Localization);

			Name = "ReportTracking";
			TitleName = Localization["Control_ReportTracking"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);

            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_ReportTracking"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
		}

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			var control = (Control)sender;

			if (control == null || control.Parent == null) return;

			var g = e.Graphics;

			Manager.PaintHighLightInput(g, control);
			Manager.PaintEdit(g, control);

			if (Localization.ContainsKey("PTSReports_" + control.Tag))
				Manager.PaintText(g, Localization["PTSReports_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_b1Report = new Summary.Summary
			{
				Server = _pts,
				App = App,
				TitleName = Localization["PTSReports_FrequentExceptionCategoriesIncurred"],
			};
			_b1Report.OnSelectionChange += B1ReportOnSelectionChange;
			_b1Report.Initialize();

			_b2Report = new Cashier.Cashier
			{
				Server = _pts,
				App = App,
				TitleName = Localization["PTSReports_FrequentExceptionsIncurredByEmployees"],
			};
			_b2Report.OnSelectionChange += B2ReportOnSelectionChange;
			_b2Report.Initialize();

			_b3Report = new ThresholdDeviation.ThresholdDeviation
			{
				Server = _pts,
				App = App,
				TitleName = Localization["PTSReports_PerformanceDeviationAgainstPredefinedThresholds"],
			};
			_b3Report.OnSelectionChange += B3ReportOnSelectionChange;
			_b3Report.Initialize();

			_c1Report = new Top2Cashier.Top2Cashier
			{
				Server = _pts,
				App = App,
				TitleName = Localization["PTSReports_HighAlertTop2EmployeesExceptionMonitoring"],
			};
			_c1Report.OnSelectionChange += C1ReportOnSelectionChange;
			_c1Report.Initialize();

			_c2Report = new ExceptionPercentage.ExceptionPercentage
			{
				Server = _pts,
				App = App,
				TitleName = Localization["PTSReports_EmployeeProductivityLossRanking"],
			};
			_c2Report.OnSelectionChange += C2ReportOnSelectionChange;
			_c2Report.Initialize();

			_c3Report = new ExceptionTrend.ExceptionTrend
			{
				Server = _pts,
				App = App,
				TitleName = Localization["PTSReports_EmployeeProductivityImprovementTrend"],
			};
			_c3Report.OnSelectionChange += C3ReportOnSelectionChange;
			_c3Report.Initialize();

			_indexPanel = new IndexPanel
			{
				PTS = _pts,
			};
			_indexPanel.OnReportSelect += IndexPanelOnReportSelect;
		}

		private void IndexPanelOnReportSelect(Object sender, EventArgs<String> e)
		{
			switch (e.Value)
			{
				case "B1":
					ShowB1Report();
					break;

				case "B2":
					ShowB2Report();
					break;

				case "B3":
					ShowB3Report();
					break;

				case "C1":
					ShowC1Report();
					break;

				case "C2":
					ShowC2Report();
					break;

				case "C3":
					ShowC3Report();
					break;
			}
		}

		private Boolean _isActivate;
		public void Activate()
		{
			if (!BlockPanel.IsFocusedControl(this)) return;

			_indexPanel.Activate();
			_isActivate = true;
		}

		public void Deactivate()
		{
			_isActivate = false;
		}

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowIndexPanel();
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
			if (_focusReport == _b1Report)
			{
				_b1Report.ShowContent(sender, e);

				return;
			}
			if (_focusReport == _b2Report)
			{
				_b2Report.ShowContent(sender, e);

				return;
			}
			if (_focusReport == _b3Report)
			{
				_b3Report.ShowContent(sender, e);

				return;
			}
			if (_focusReport == _c1Report)
			{
				_c1Report.ShowContent(sender, e);

				return;
			}
			if (_focusReport == _c2Report)
			{
				_c2Report.ShowContent(sender, e);

				return;
			}
			if (_focusReport == _c3Report)
			{
				_c3Report.ShowContent(sender, e);

				return;
			}

			ShowIndexPanel();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			XmlDocument xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");

			if (_focusReport == _b1Report)
			{
				if (!(item == "Back" && _b1Report.IsAtIndexPage))
				{
					fromNode.InnerText = _b1Report.TitleName;
					_b1Report.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));

					return;
				}
			}
			else if (_focusReport == _b2Report)
			{
				if (!(item == "Back" && _b2Report.IsAtIndexPage))
				{
					fromNode.InnerText = _b2Report.TitleName;
					_b2Report.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));

					return;
				}
			}
			else if (_focusReport == _b3Report)
			{
				if (!(item == "Back" && _b3Report.IsAtIndexPage))
				{
					fromNode.InnerText = _b3Report.TitleName;
					_b3Report.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));

					return;
				}
			}
			else if (_focusReport == _c1Report)
			{
				if (!(item == "Back" && _c1Report.IsAtIndexPage))
				{
					fromNode.InnerText = _c1Report.TitleName;
					_c1Report.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));

					return;
				}
			}
			else if (_focusReport == _c2Report)
			{
				if (!(item == "Back" && _c2Report.IsAtIndexPage))
				{
					fromNode.InnerText = _c2Report.TitleName;
					_c2Report.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));

					return;
				}
			}
			else if (_focusReport == _c3Report)
			{
				if (!(item == "Back" && _c3Report.IsAtIndexPage))
				{
					fromNode.InnerText = _c3Report.TitleName;
					_c3Report.SelectionChange(sender, new EventArgs<String>(xmlDoc.InnerXml));

					return;
				}
			}

			if (item == TitleName || item == "Back")
			{
				ShowIndexPanel();
			}
			//if (OnSelectionChange != null)
			//    OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
			//        Localization["PTSReports_SearchResult"], "Back", "SearchException")));
		}

		private void ShowIndexPanel()
		{
			if (!contentPanel.Controls.Contains(_indexPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_indexPanel);
				_focusReport = null;
			}

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
		}
		//--------------------------------------------------------------------------------------------------------------
		private void ShowB1Report()
		{
			_focusReport = _b1Report;

			Manager.ReplaceControl(_indexPanel, _b1Report.RootPanel, contentPanel, ManagerMoveToB1Complete);
		}

		private void ShowB2Report()
		{
			_focusReport = _b2Report;

			Manager.ReplaceControl(_indexPanel, _b2Report.RootPanel, contentPanel, ManagerMoveToB2Complete);
		}

		private void ShowB3Report()
		{
			_focusReport = _b3Report;

			Manager.ReplaceControl(_indexPanel, _b3Report.RootPanel, contentPanel, ManagerMoveToB3Complete);
		}

		private void ShowC1Report()
		{
			_focusReport = _c1Report;

			Manager.ReplaceControl(_indexPanel, _c1Report.RootPanel, contentPanel, ManagerMoveToC1Complete);
		}

		private void ShowC2Report()
		{
			_focusReport = _c2Report;

			Manager.ReplaceControl(_indexPanel, _c2Report.RootPanel, contentPanel, ManagerMoveToC2Complete);
		}

		private void ShowC3Report()
		{
			_focusReport = _c3Report;

			Manager.ReplaceControl(_indexPanel, _c3Report.RootPanel, contentPanel, ManagerMoveToC3Complete);
		}
		//--------------------------------------------------------------------------------------------------------------
		private void ManagerMoveToB1Complete()
		{
			_b1Report.ShowCriteriaPanel();
			_b1Report.CriteriaPanel.Focus();
			//_b1Report.CriteriaPanel.SearchExceptionSummaryReport();

			var btn = "SearchException,ClearConditional";
			if(_b1Report.IsAtSearchPanel)
				btn += ",SaveReport";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					_b1Report.TitleName, "Back", btn)));
		}

		private void ManagerMoveToB2Complete()
		{
			_b2Report.ShowCriteriaPanel();
			_b2Report.CriteriaPanel.Focus();
			//_b2Report.CriteriaPanel.SearchCashierExceptionSummaryReport();

			var btn = "SearchException,ClearConditional";
			if (_b2Report.IsAtSearchPanel)
				btn += ",SaveReport";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					_b2Report.TitleName, "Back", btn)));
		}

		private void ManagerMoveToB3Complete()
		{
			_b3Report.ShowCriteriaPanel();
			_b3Report.CriteriaPanel.Focus();
			//_b3Report.CriteriaPanel.SearchExceptionThresholdDeviationReport();

			var btn = "SearchException,ClearConditional";
			if (_b3Report.IsAtSearchPanel)
				btn += ",SaveReport";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					_b3Report.TitleName, "Back", btn)));
		}

		private void ManagerMoveToC1Complete()
		{
			_c1Report.ShowCriteriaPanel();
			_c1Report.CriteriaPanel.Focus();
			//_c1Report.CriteriaPanel.SearchCashierExceptionSummaryReport();

			var btn = "SearchException,ClearConditional";
			if (_c1Report.IsAtSearchPanel)
				btn += ",SaveReport";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					_c1Report.TitleName, "Back", btn)));
		}

		private void ManagerMoveToC2Complete()
		{
			_c2Report.ShowCriteriaPanel();
			_c2Report.CriteriaPanel.Focus();
			//_c2Report.CriteriaPanel.SearchCashierExceptionPercentageReport();

			var btn = "SearchException,ClearConditional";
			if (_c2Report.IsAtSearchPanel)
				btn += ",SaveReport";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					_c2Report.TitleName, "Back", btn)));
		}

		private void ManagerMoveToC3Complete()
		{
			_c3Report.ShowCriteriaPanel();
			_c3Report.CriteriaPanel.Focus();
			//_c3Report.CriteriaPanel.SearchCashierExceptionTrendReport();

			var btn = "SearchException,ClearConditional";
			if (_c3Report.IsAtSearchPanel)
				btn += ",SaveReport";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					_c3Report.TitleName, "Back", btn)));
		}
		//--------------------------------------------------------------------------------------------------------------
		private void B1ReportOnSelectionChange(Object sender, EventArgs<String> e)
		{
			if (_focusReport != _b1Report) return;

			var xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
			fromNode.InnerText = TitleName;

			var previousNode = Xml.GetFirstElementByTagName(xmlDoc, "Previous");
			previousNode.InnerText = "Back";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(xmlDoc.InnerXml));
		}

		private void B2ReportOnSelectionChange(Object sender, EventArgs<String> e)
		{
			if (_focusReport != _b2Report) return;

			var xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
			fromNode.InnerText = TitleName;

			var previousNode = Xml.GetFirstElementByTagName(xmlDoc, "Previous");
			previousNode.InnerText = "Back";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(xmlDoc.InnerXml));
		}

		private void B3ReportOnSelectionChange(Object sender, EventArgs<String> e)
		{
			if (_focusReport != _b3Report) return;

			var xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
			fromNode.InnerText = TitleName;

			var previousNode = Xml.GetFirstElementByTagName(xmlDoc, "Previous");
			previousNode.InnerText = "Back";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(xmlDoc.InnerXml));
		}

		private void C1ReportOnSelectionChange(Object sender, EventArgs<String> e)
		{
			if (_focusReport != _c1Report) return;

			var xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
			fromNode.InnerText = TitleName;

			var previousNode = Xml.GetFirstElementByTagName(xmlDoc, "Previous");
			previousNode.InnerText = "Back";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(xmlDoc.InnerXml));
		}

		private void C2ReportOnSelectionChange(Object sender, EventArgs<String> e)
		{
			if (_focusReport != _c2Report) return;

			var xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
			fromNode.InnerText = TitleName;

			var previousNode = Xml.GetFirstElementByTagName(xmlDoc, "Previous");
			previousNode.InnerText = "Back";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(xmlDoc.InnerXml));
		}

		private void C3ReportOnSelectionChange(Object sender, EventArgs<String> e)
		{
			if (_focusReport != _c3Report) return;

			var xmlDoc = Xml.LoadXml(e.Value);
			var fromNode = Xml.GetFirstElementByTagName(xmlDoc, "From");
			fromNode.InnerText = TitleName;

			var previousNode = Xml.GetFirstElementByTagName(xmlDoc, "Previous");
			previousNode.InnerText = "Back";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(xmlDoc.InnerXml));
		}
		//--------------------------------------------------------------------------------------------------------------

		public void SaveReport(Object sender, EventArgs e)
		{
			if (!_isActivate) return;

			if (_focusReport == _b1Report)
			{
				_b1Report.SaveReport();
			}
			else if (_focusReport == _b2Report)
			{
				_b2Report.SaveReport();
			}
			else if (_focusReport == _b3Report)
			{
				_b3Report.SaveReport();
			}
			else if (_focusReport == _c1Report)
			{
				_c1Report.SaveReport();
			}
			else if (_focusReport == _c2Report)
			{
				_c2Report.SaveReport();
			}
		}
	}
}
