using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupExceptionReport
{
	public sealed partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public String TitleName { get; set; }
		public IPTS PTS;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
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

		private POSListPanel _posListPanel;
		private ListPanel _listPanel;
		private EditPanel _editPanel;
		private CopyExceptionReportPanel _copyExceptionReportPanel;

		private Control _focusControl;

		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ExceptionReport", "Exception Report"},

								   {"SetupExceptionReport_ExceptionReport", "Exception Report"},
								   {"SetupExceptionReport_CopyExceptionReport", "Copy Exception Report"},
								   {"SetupExceptionReport_DeleteExceptionReport", "Delete Exception Report"},
							   };
			Localizations.Update(Localization);

			Name = "Exception Report";
			TitleName = Localization["Control_ExceptionReport"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_ExceptionReport"] };
			Icon.Click += DockIconClick;

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
			//---------------------------
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_posListPanel = new POSListPanel
			{
				PTS = PTS,
			};
			_posListPanel.Initialize();

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

			_copyExceptionReportPanel = new CopyExceptionReportPanel
			{
				PTS = PTS,
			};
			_copyExceptionReportPanel.Initialize();

			_posListPanel.OnPOSEdit += POSListPanelOnPOSEditClick;
			
			_listPanel.OnExceptionReportAdd += ListPanelOnAdd;
			_listPanel.OnExceptionReportEdit += ListPanelOnEdit;

			contentPanel.Controls.Add(_posListPanel);
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

			ShowPOSList();
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

					EditPOS(_listPanel.POS);
					break;

				case "Delete":
					DeleteExceptionReport();
					break;

				case "CopyExceptionReport":
					CopyExceptionReport();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (_addNewHandler)
						{
							EditPOS(_editPanel.POS);
						}
						else
						{
							ShowPOSList();
						}
					}
					break;
			}
		}

		private void ShowPOSList()
		{
			_addNewHandler = false;
			_focusControl = _posListPanel;

			//_posListPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_posListPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_posListPanel);
			}

			_posListPanel.GenerateViewModel();

			var btn = "";
			if (PTS.POS.POSServer.Count > 1)
				btn = "CopyExceptionReport";

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						 TitleName, "", btn)));
		}

		private void POSListPanelOnPOSEditClick(Object sender, EventArgs<IPOS> e)
		{
			EditPOS(e.Value);
		}

		private Boolean _addNewHandler;// = false;

		private void ListPanelOnEdit(Object sender, EventArgs<ExceptionReport> e)
		{
			EditExceptionReport(e.Value);
		}

		private void ListPanelOnAdd(Object sender, EventArgs e)
		{
			var exceptions = (PTS.POS.Exceptions.ContainsKey(_listPanel.POS.Exception))
								 ? PTS.POS.Exceptions[_listPanel.POS.Exception]
								 : ((PTS.POS.Exceptions.Values.Count > 0)
									? PTS.POS.Exceptions.Values.First()
									: null);
			if (exceptions == null || exceptions.Exceptions.Count == 0) return;

			var str = exceptions.Exceptions.Select(exception => exception.Key).ToList();
			str.Sort((x, y) => (String.Compare(x, y)));

			var report = new ExceptionReport
			{
				Exception = str[0],
			};
			report.ReportForm.Exceptions.Add(report.Exception);
			report.ReportForm.POS.Add(_listPanel.POS.Id);
			report.ReportForm.MailReceiver = Server.User.Current.Email;

			_listPanel.POS.ExceptionReports.Add(report);

			if (_listPanel.POS.ExceptionReports.ReadyState == ReadyState.Ready)
				_listPanel.POS.ExceptionReports.ReadyState = ReadyState.Modify;

			if (_listPanel.POS.ReadyState == ReadyState.Ready)
				_listPanel.POS.ReadyState = ReadyState.Modify;

			EditExceptionReport(report);
		}

		private void CopyExceptionReport()
		{
			_focusControl = _copyExceptionReportPanel;

			_copyExceptionReportPanel.GenerateViewModel();

			Manager.ReplaceControl(_posListPanel, _copyExceptionReportPanel, contentPanel, ManagerMoveToCopyComplete);
		}

		private void EditExceptionReport(ExceptionReport report)
		{
			_addNewHandler = true;

			_focusControl = _editPanel;
			_editPanel.Report = report;
			_editPanel.POS = _listPanel.POS;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToExceptionReportEditComplete);
		}

		private void EditPOS(IPOS pos)
		{
			_addNewHandler = false;

			_focusControl = _listPanel;
			_listPanel.POS = pos;

			Manager.ReplaceControl(_posListPanel, _listPanel, contentPanel, ManagerMoveToReportEditComplete);
		}

		private void ManagerMoveToReportEditComplete()
		{
			_listPanel.GenerateViewModel();

			Boolean hasReport = (_listPanel.POS.ExceptionReports.Count > 0);

			var text = TitleName + "  /  " + _listPanel.POS;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						 text, "Back", ((hasReport) ? "Delete" : ""))));
		}

		private void ManagerMoveToExceptionReportEditComplete()
		{
			_editPanel.ParseReport();

			var text = TitleName + "  /  " + Localization["SetupExceptionReport_ExceptionReport"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text,
					"Back", "")));
		}

		private void ManagerMoveToCopyComplete()
		{
			var text = TitleName + "  /  " + Localization["SetupExceptionReport_CopyExceptionReport"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void DeleteExceptionReport()
		{
			_listPanel.ShowCheckBox();

			var text = TitleName + "  /  " + Localization["SetupExceptionReport_DeleteExceptionReport"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
		}
		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				ShowPOSList();
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
