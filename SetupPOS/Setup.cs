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

namespace SetupPOS
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
								   {"Control_POS", "POS"},

								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Confirm", "Confirm"},
								   
								   {"SetupPOS_NewPOS", "New POS"},
								   {"SetupPOS_DeletePOS", "Delete POS"},
								   {"SetupPOS_MaximumLicense", "Reached maximum license limit \"%1\""},
								   
								   {"SetupException_NewException", "New Exception"},
								   {"SetupPOSConnection_NewPOSConnection", "New POS Connection"},
							   };
			Localizations.Update(Localization);

			Name = "POS";
			TitleName = Localization["Control_POS"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_POS"] };
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

			_listPanel.OnPOSEdit += ListPanelOnPOSEdit;
			_listPanel.OnPOSAdd += ListPanelOnPOSAdd;
			
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
					_listPanel.RemoveSelectedPOS();

					ShowPOSList();
					break;

				case "Delete":
					DeletePOS();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowPOSList);
					}
					break;
			}
		}

		private void DeletePOS()
		{
			_listPanel.SelectedColor = Manager.DeleteTextColor;
			_listPanel.ShowCheckBox();

			var text = TitleName + "  /  " + Localization["SetupPOS_DeletePOS"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text,
					"Back", "Confirm")));
		}

		private void ShowPOSList()
		{
			_focusControl = _listPanel;

			_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			if (OnSelectionChange == null) return;
			String buttons = "";

			if (PTS != null && PTS.POS.POSServer.Count > 0)
				buttons = "Delete";

			String count = "";
			if (PTS != null && PTS.POS.POSServer.Count > 0)
			{
				count = " (" + PTS.POS.POSServer.Count + "/" + Server.License.Amount + ")";
			}

			OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName + count,
																					  "", buttons)));
		}

		private void ListPanelOnPOSEdit(Object sender, EventArgs<IPOS> e)
		{
			EditPOS(e.Value);
		}

		private void ListPanelOnPOSAdd(Object sender, EventArgs e)
		{
			POS pos = null;
			if (PTS != null)
			{
				pos = new POS
				{
					LicenseId = PTS.POS.GetNewPOSLicenseId(),
					Manufacture = POS_Exception.Manufactures[0],
				};
			}

			if (pos == null) return;

			if (pos.LicenseId == 0)
			{
				TopMostMessageBox.Show(Localization["SetupPOS_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()),
					Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			pos.Id = PTS.POS.GetNewPOSId();

			//AddDefaultExceptionIfThereIsNone(pos);

			//AddDefaultConnectionIfThereIsNone(pos);

			pos.ReadyState = ReadyState.New;

			pos.Name = Localization["SetupPOS_NewPOS"] + @" " + pos.Id;

			var existpos = PTS.POS.FindPOSById(pos.Id);
			if (existpos == null)
			{
				PTS.POS.POSServer.Add(pos);
				PTS.POSModify(pos);
			}

			EditPOS(pos);
		}

		//private void AddDefaultExceptionIfThereIsNone(IPOS pos)
		//{
		//    if (PTS == null) return;

		//    //already has exception can be use
		//    if (PTS.POS.Exceptions.Count > 0)
		//    {
		//        var keys = PTS.POS.Exceptions.Keys.ToList();
		//        keys.Sort();
		//        foreach (var key in keys)
		//        {
		//            var posException = PTS.POS.Exceptions[key];
		//            if (posException.Manufacture == pos.Manufacture)
		//            {
		//                pos.Exception = key;
		//                return;
		//            }
		//        }
		//    }

		//    //create new exception for pos
		//    var exception = new POS_Exception
		//    {
		//        Id = PTS.POS.GetNewExceptionId(),
		//        Manufacture = pos.Manufacture,// POS_Exception.Manufactures[0]
		//    };
		//    POS_Exception.SetDefaultExceptions(exception);
		//    POS_Exception.SetDefaultSegments(exception);
		//    POS_Exception.SetDefaultTags(exception);

		//    //license limit!
		//    if (exception.Id == 0) return;

		//    pos.Exception = exception.Id;

		//    exception.Name = Localization["SetupException_NewException"] + @" " + exception.Id;

		//    if (!PTS.POS.Exceptions.ContainsKey(exception.Id))
		//        PTS.POS.Exceptions.Add(exception.Id, exception);
		//}

		//private void AddDefaultConnectionIfThereIsNone(IPOS pos)
		//{
		//    if (PTS == null) return;

		//    //already has connection can be use
		//    if (PTS.POS.Connections.Count > 0)
		//    {
		//        var keys = PTS.POS.Connections.Keys.ToList();
		//        keys.Sort();
		//        foreach (var key in keys)
		//        {
		//            var connection = PTS.POS.Connections[key];
		//            if (connection.Manufacture == pos.Manufacture)
		//            {
		//                if (!connection.POS.Contains(pos))
		//                    connection.POS.Add(pos);
		//                return;
		//            }
		//        }
		//    }

		//    var posConnection = new POSConnection
		//    {
		//        Id = PTS.POS.GetNewConnectionId(),
		//        Protocol = POSConnection.ProtocolList[0],
		//        Manufacture = pos.Manufacture,// POSConnection.Manufactures[0],
		//    };
		//    posConnection.SetDefaultAuthentication();

		//    //license limit!
		//    if (posConnection.Id == 0) return;

		//    posConnection.POS.Add(pos);

		//    posConnection.ReadyState = ReadyState.New;
		//    posConnection.Name = Localization["SetupPOSConnection_NewPOSConnection"] + @" " + posConnection.Id;

		//    if (!PTS.POS.Connections.ContainsKey(posConnection.Id))
		//        PTS.POS.Connections.Add(posConnection.Id, posConnection);
		//}

		private void EditPOS(IPOS pos)
		{
			_focusControl = _editPanel;
			_editPanel.POS = pos;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ManagerMoveToEditComplete()
		{
			_editPanel.ParsePOS();

			var text = TitleName + "  /  " + _editPanel.POS;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
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
