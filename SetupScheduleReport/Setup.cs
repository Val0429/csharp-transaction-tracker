using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupScheduleReport
{
	public sealed partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public String TitleName { get; set; }
		public IPTS PTS;
		private IServer _server;
		public IServer Server { get { return _server; }
			set
			{
				_server = value;
				if (value is IPTS)
					PTS = value as IPTS;
			}
		}
		public IBlockPanel BlockPanel { get; set; }

		public Dictionary<String, String> Localization;

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
		private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

		public UInt16 MinimizeHeight
		{
			get { return 0; }
		}
		public Boolean IsMinimize { get; private set; }

		private ListPanel _listPanel;
		private EditPanel _editPanel;

		private Control _focusControl;

		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ScheduleReport", "Schedule Report"},

								   {"SetupScheduleReport_DeleteScheduleReport", "Delete Schedule Report"},
							   };
			Localizations.Update(Localization);

			Name = "Schedule Report";
			TitleName = Localization["Control_ScheduleReport"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_ScheduleReport"] };
			Icon.Click += DockIconClick;

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
			//---------------------------
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_listPanel = new ListPanel
			{
				PTS = PTS,
			};
			_listPanel.Initialize();

			_editPanel = new EditPanel
			{
				PTS = PTS,
			};
			_editPanel.Initialize();
			
			_listPanel.OnScheduleReportAdd += ListPanelOnAdd;
			_listPanel.OnScheduleReportEdit += ListPanelOnEdit;

			contentPanel.Controls.Add(_listPanel);
		}

		private void ShowScheduleReportList()
		{
			_focusControl = _listPanel;

			_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			var button = "";
			if (PTS.POS.ScheduleReports.Count > 0)
				button = "Delete";

			var text = TitleName;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "", button)));
		}

		private void ListPanelOnAdd(Object sender, EventArgs e)
		{
			var report = new ScheduleReport();

			PTS.POS.ScheduleReports.ReadyState = ReadyState.Modify;
			report.ReportForm.MailReceiver = Server.User.Current.Email;
			PTS.POS.ScheduleReports.Add(report);

			EditScheduleReport(report);
		}

		private void ListPanelOnEdit(Object sender, EventArgs<ScheduleReport> e)
		{
			EditScheduleReport(e.Value);
		}
		
		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			BlockPanel.ShowThisControlPanel(this);

			ShowScheduleReportList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedReports();
					ShowScheduleReportList();
					break;

				case "Delete":
					DeleteScheduleReport();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (_focusControl == _editPanel)
						{
							Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowScheduleReportList);
							return;
						}

						ShowScheduleReportList();
					}
					break;
			}
		}

		private void EditScheduleReport(ScheduleReport report)
		{
			_focusControl = _editPanel;
			_editPanel.Report = report;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToScheduleReportEditComplete);
		}

		private void ManagerMoveToScheduleReportEditComplete()
		{
			_editPanel.ParseReport();
			var text = TitleName + "  /  " + Localization["Control_ScheduleReport"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void DeleteScheduleReport()
		{
			_listPanel.ShowCheckBox();
			var text = TitleName + "  /  " + Localization["SetupScheduleReport_DeleteScheduleReport"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
		}
		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				ShowScheduleReportList();
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
	}
}
