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

namespace SetupDeviceGroup
{
	public sealed partial class Setup : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public String TitleName { get; set; }

		private ICMS _cms;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set {
				_server = value;
				if (value is ICMS)
					_cms = value as ICMS;
			}
		}
		public IApp App { get; set; }
		public IBlockPanel BlockPanel { get; set; }

		public Dictionary<String, String> Localization;

		public Button Icon { get; private set; }

		public UInt16 MinimizeHeight
		{
			get { return 0; }
		}
		public Boolean IsMinimize { get; private set; }

		private ListPanel _listPanel;
		private EditPanel _editPanel;
		private LayoutPanel _layoutPanel;
		private Boolean _setupLayout;// = false;
		private Control _focusControl;

		public Setup()
		{
			Localization = new Dictionary<String, String>
				{
					{"Control_DeviceGroup", "Group"},

					{"GroupPanel_NumDevice", "(%1 Device)"},
					{"GroupPanel_NumDevices", "(%1 Devices)"},

					{"SetupDeviceGroup_NewGroup", "New Group"},
					{"SetupDeviceGroup_DeleteDeviceGroup", "Delete Device Group"},
				};
			Localizations.Update(Localization);

			Image icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
			Image iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

			Name = "Group";
			TitleName = Localization["Control_DeviceGroup"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = icon, IconActivateImage = iconActivate, IconText = Localization["Control_DeviceGroup"] };
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
				Server = Server,
			};
			_listPanel.Initialize();

			_editPanel = new EditPanel
			{
				Server = Server
			};
			_editPanel.Initialize();

			_layoutPanel = new LayoutPanel
			{
				Server = Server,
				App = App
			};
			_layoutPanel.Initialize();

			_listPanel.OnDeviceGroupEdit += ListPanelOnDeviceGroupEdit;
			_listPanel.OnDeviceGroupAdd += ListPanelOnDeviceGroupAdd;

			_editPanel.OnDeviceSelectionChange += EditPanelOnDeviceSelectionChange;
			_editPanel.OnGroupLayoutEdit += EditPanelOnGroupLayoutEdit;

			contentPanel.Controls.Add(_listPanel);
			Server.OnSaveComplete += ServerOnLoadComplete;
			Server.OnLoadComplete += ServerOnLoadComplete;
		}

		private delegate void RefreshContentDelegate(Object sender, EventArgs<String> e);
		private void ServerOnLoadComplete(Object sender, EventArgs<String> e)
		{
			if (Parent != null && Parent.Visible)
			{
				try
				{
					if (InvokeRequired)
					{
						Invoke(new RefreshContentDelegate(ServerOnLoadComplete), sender, e);
						return;
					}
				}
				catch (Exception)
				{
				}

				if (_focusControl == null) _focusControl = _listPanel;
				Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowDeviceGroupList);
			}
		}

		private void ShowDeviceGroupList()
		{
			_focusControl = _listPanel;

			_setupLayout = false;
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

				if(_cms != null)
				{
					if (Server.Device.Groups.Count > 0)
						buttons = "Delete";
				}
				else if (Server.Device.Groups.Count > 1)
					buttons = "Delete";
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", buttons)));
			}
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

			ShowDeviceGroupList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedGroups();

					ShowDeviceGroupList();
					break;

				case "Delete":
					DeleteGroup();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (_setupLayout)
						{
							ListPanelOnDeviceGroupEdit(_editPanel.Group);
						}
						else
						{
							Manager.ReplaceControl(_editPanel, _listPanel, contentPanel, ShowDeviceGroupList);
						}
					}
					break;
			}
		}

		private void ListPanelOnDeviceGroupEdit(Object sender, EventArgs<IDeviceGroup> e)
		{
			if (e.Value == null) return;
			ListPanelOnDeviceGroupEdit(e.Value);
		}

		private void ListPanelOnDeviceGroupEdit(IDeviceGroup group)
		{
			_focusControl = _editPanel;

			_setupLayout = false;
			_editPanel.Enabled = false;
			_editPanel.Group = group;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ListPanelOnDeviceGroupAdd(Object sender, EventArgs e)
		{
			IDeviceGroup group = new DeviceGroup
			{
				Id = Server.Device.GetNewGroupId(),
				Server = Server,
			};

			group.Name = Localization["SetupDeviceGroup_NewGroup"] + @" " + group.Id;

			Server.WriteOperationLog("Add New Group %1".Replace("%1", group.ToString()));

			if (!Server.Device.Groups.ContainsKey(group.Id))
				Server.Device.Groups.Add(group.Id, group);

			_focusControl = _editPanel;

			_setupLayout = false;
			_editPanel.Enabled = false;
			_editPanel.Group = group;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void EditPanelOnGroupLayoutEdit(Object sender, EventArgs<IDeviceGroup> e)
		{
			if (e.Value == null) return;

			_focusControl = _layoutPanel;

			_setupLayout = true;
			_layoutPanel.Group = e.Value;
			Manager.ReplaceControl(_editPanel, _layoutPanel, contentPanel, ManagerMoveToLayoutComplete);
		}

		private void EditPanelOnDeviceSelectionChange(Object sender, EventArgs e)
		{
			if (OnSelectionChange == null) return;

			var text = TitleName + "  /  " + _editPanel.Group + "   " + (((_editPanel.Group.Items.Count <= 1)
																			  ? Localization["GroupPanel_NumDevice"]
																			  : Localization["GroupPanel_NumDevices"]).Replace("%1", _editPanel.Group.Items.Count.ToString()));

			OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void ManagerMoveToEditComplete()
		{
			_focusControl = _editPanel;

			_editPanel.ParseGroup();
			_editPanel.Enabled = true;

			if (OnSelectionChange == null) return;
			var text = TitleName + "  /  " + _editPanel.Group + "   " + ((_editPanel.Group.Items.Count <= 1)
						 ? Localization["GroupPanel_NumDevice"]
						 : Localization["GroupPanel_NumDevices"]).Replace("%1", _editPanel.Group.Items.Count.ToString());

			OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void ManagerMoveToLayoutComplete()
		{
			_layoutPanel.ShowLayout();
		}

		private void DeleteGroup()
		{
			_focusControl = _listPanel;

			_listPanel.SelectionVisible = true;
			_listPanel.ShowGroup();

			var text = TitleName + "  /  " + Localization["SetupDeviceGroup_DeleteDeviceGroup"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					text, "Back", "Confirm")));
		}

		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				ShowDeviceGroupList();
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
