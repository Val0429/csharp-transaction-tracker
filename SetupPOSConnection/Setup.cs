using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupPOSConnection
{
	public sealed partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

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

		private ListPanel _listPanel;
		private EditPanel _editPanel;

		private Control _focusControl;
		public String TitleName { get; set; }
		
		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_POSConnection", "POS Connection"},

								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   
								   {"SetupPOSConnection_NewPOSConnection", "New POS Connection"},
								   {"SetupPOSConnection_DeletePOSConnection", "Delete POS Connection"},

								   {"SetupPOS_MaximumLicense", "Reached maximum license limit \"%1\""},
							   };
			Localizations.Update(Localization);

			Name = "POSConnection";
			TitleName = Localization["Control_POSConnection"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_POSConnection"] };
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
				PTS = PTS
			};
			_listPanel.Initialize();

			_editPanel = new EditPanel
			{
				PTS = PTS
			};
			_editPanel.Initialize();

			_listPanel.OnPOSConnectionEdit += ListPanelOnPOSConnectionEdit;
			_listPanel.OnPOSConnectionAdd += ListPanelOnPOSConnectionAdd;

			contentPanel.Controls.Contains(_listPanel);
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

			ShowPOSConnectionList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if(!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedPOSConnection();

					ShowPOSConnectionList();
					break;

				case "Delete":
					DeletePOSConnection();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowPOSConnectionList);
					}
					break;
			}
		}

		private void DeletePOSConnection()
		{
			_listPanel.SelectionVisible = true;
			_listPanel.ShowPOSConnection();

			var text = TitleName + "  /  " + Localization["SetupPOSConnection_DeletePOSConnection"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
		}

		private void ShowPOSConnectionList()
		{
			_focusControl = _listPanel;

			_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			_listPanel.SelectionVisible = false;

			if (OnSelectionChange != null)
			{
				String buttons = "";

				if (PTS != null && PTS.POS.Connections.Count > 0)
					buttons = "Delete";

				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
					"", buttons)));
			}
		}

		private void ListPanelOnPOSConnectionEdit(Object sender, EventArgs<IPOSConnection> e)
		{
			EditPOSConnection(e.Value);
		}

		private void ListPanelOnPOSConnectionAdd(Object sender, EventArgs e)
		{
			IPOSConnection posConnection = null;
			if (PTS != null)
			{
				posConnection = new POSConnection
				{
					Id = PTS.POS.GetNewConnectionId(),
					Protocol = POSConnection.ProtocolList[0],
					Manufacture = POS_Exception.Manufactures[0],
				};
				posConnection.SetDefaultAuthentication();
			}

			if (posConnection == null) return;

			if (posConnection.Id == 0)
			{
				TopMostMessageBox.Show(Localization["SetupPOS_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()),
					Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			posConnection.ReadyState = ReadyState.New;
			posConnection.Name = Localization["SetupPOSConnection_NewPOSConnection"] + @" " + posConnection.Id;

			if (!PTS.POS.Connections.ContainsKey(posConnection.Id))
			{
				PTS.POS.Connections.Add(posConnection.Id, posConnection);
			}

			EditPOSConnection(posConnection);
		}

		private void EditPOSConnection(IPOSConnection posConnection)
		{
			_focusControl = _editPanel;
			_editPanel.POSConnection = posConnection;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ManagerMoveToEditComplete()
		{
			_editPanel.ParsePOSConnection();
			var text = TitleName + "  /  " + _editPanel.POSConnection;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}
		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				ShowPOSConnectionList();
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
