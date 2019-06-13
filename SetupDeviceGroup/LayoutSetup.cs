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
	public sealed partial class LayoutSetup : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public String TitleName { get; set; }

		public IServer Server { get; set; }
		public IApp App { get; set; }
		public IBlockPanel BlockPanel { get; set; }

		public Dictionary<String, String> Localization;

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon_layout, Properties.Resources.IMGIconLayout);
		private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_layout_activate, Properties.Resources.IMGIconLayoutActivate);

		public UInt16 MinimizeHeight
		{
			get { return 0; }
		}
		public Boolean IsMinimize { get; private set; }

		private LayoutListPanel _listPanel;
		private LayoutEditPanel _editPanel;
		private SubLayoutEditPanel _subLayoutEditPanel;

		private ImmerVisionLayoutEditPanel _immerVisionEditPanel;
		private ImmerVisionSubLayoutEditPanel _immerVisionsubLayoutEditPanel;
		private Control _focusControl;

		public LayoutSetup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_ImageStitching", "Image Stitching"},

								   {"DeviceLayoutPanel_SetupCrop", "Setup Crop"},
								   {"DeviceLayoutPanel_Panorama", "(%1 Panorama)"},

								   {"SetupDeviceLayout_NewPanorama", "New Panorama"},
								   {"SetupDeviceLayout_DeletePanorama", "Delete Panorama"},
							   };
			Localizations.Update(Localization);

			Name = "Image Stitching";
			TitleName = Localization["Control_ImageStitching"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_ImageStitching"] };
			Icon.Click += DockIconClick;

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
			//---------------------------
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_listPanel = new LayoutListPanel
			{
				Server = Server,
			};
			_listPanel.Initialize();

			_editPanel = new LayoutEditPanel
			{
				App = App,
				Server = Server
			};
			_editPanel.Initialize();

			_immerVisionEditPanel = new ImmerVisionLayoutEditPanel
			{
				App = App,
				Server = Server
			};
			_immerVisionEditPanel.Initialize();

			_subLayoutEditPanel = new SubLayoutEditPanel()
			{
				App = App,
				Server = Server
			};
			_subLayoutEditPanel.Initialize();
			
			_immerVisionsubLayoutEditPanel = new ImmerVisionSubLayoutEditPanel
				{
					App = App,
					Server = Server
				};
			_immerVisionsubLayoutEditPanel.Initialize();

			_listPanel.OnDeviceLayoutEdit += ListPanelOnDeviceLayoutEdit;
			_listPanel.OnDeviceLayoutAdd += ListPanelOnDeviceLayoutAdd;
			_listPanel.OnImmerVisionLayoutAdd += _listPanel_OnImmerVisionLayoutAdd;

			_editPanel.OnSubDeviceLayoutEdit += EditPanelOnSubDeviceLayoutEdit;
			_immerVisionEditPanel.OnSubDeviceLayoutEdit += _immerVisionEditPanel_OnSubDeviceLayoutEdit;

			contentPanel.Controls.Add(_listPanel);
			Server.OnSaveComplete += ServerOnLoadComplete;
			Server.OnLoadComplete += ServerOnLoadComplete;
		}

		private delegate void RefreshContentDelegate(Object sender, EventArgs<String> e);
		private void ServerOnLoadComplete(object sender, EventArgs<string> e)
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
				Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowDeviceLayoutList);
			}
		}

		private void ShowDeviceLayoutList()
		{
			_subLayoutEditPanel.Deactivate();
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

				if (Server.Device.DeviceLayouts.Count > 0)
					buttons = "Delete";

				//no need show panorama count, no meaning
				//var count = Localization["DeviceLayoutPanel_Panorama"].Replace("%1", Server.Device.DeviceLayouts.Count.ToString());

				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", buttons)));
			}
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
			_subLayoutEditPanel.Deactivate();

			//back to index page
			if(_focusControl != _listPanel)
				ShowDeviceLayoutList();
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			BlockPanel.ShowThisControlPanel(this);

			ShowDeviceLayoutList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedDeviceLayouts();

					ShowDeviceLayoutList();
					break;

				case "Delete":
					DeleteLayout();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (_focusControl == _subLayoutEditPanel)
						{
							_subLayoutEditPanel.Deactivate();
							ListPanelOnDeviceLayoutEdit(_subLayoutEditPanel.DeviceLayout);
						}
						else if (_focusControl == _immerVisionsubLayoutEditPanel)
						{
							_immerVisionsubLayoutEditPanel.Deactivate();
							ListPanelOnImmerVisionDeviceLayoutEdit(_immerVisionsubLayoutEditPanel.DeviceLayout);
						}
						else
						{
							Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowDeviceLayoutList);
						}
					}
					break;
			}
		}

		private void ListPanelOnDeviceLayoutEdit(Object sender, EventArgs<IDeviceLayout> e)
		{
			if (e.Value == null) return;

			if (e.Value.isImmerVision)
				ListPanelOnImmerVisionDeviceLayoutEdit(e.Value);
			else 
				ListPanelOnDeviceLayoutEdit(e.Value);
		}

		private void ListPanelOnDeviceLayoutEdit(IDeviceLayout layout)
		{
			_focusControl = _editPanel;

			_editPanel.Enabled = false;
			_editPanel.DeviceLayout = layout;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ListPanelOnImmerVisionDeviceLayoutEdit(IDeviceLayout layout)
		{
			_focusControl = _immerVisionEditPanel;

			_immerVisionEditPanel.Enabled = false;
			//_immerVisionEditPanel.Camera = _immerVisionsubLayoutEditPanel.Camera;
			_immerVisionEditPanel.DeviceLayout = layout;

			Manager.ReplaceControl(_listPanel, _immerVisionEditPanel, contentPanel, ManagerMoveToImmerVisionEditComplete);
		}

		private void EditPanelOnSubDeviceLayoutEdit(Object sender, EventArgs<IDeviceLayout> e)
		{
			_focusControl = _subLayoutEditPanel;

			_subLayoutEditPanel.DeviceLayout = _editPanel.DeviceLayout;
			Manager.ReplaceControl(_editPanel, _subLayoutEditPanel, contentPanel, ManagerMoveToSubLayoutEditComplete);
		}

		private void _immerVisionEditPanel_OnSubDeviceLayoutEdit(object sender, EventArgs<IDeviceLayout> e)
		{
			_focusControl = _immerVisionsubLayoutEditPanel;

			_immerVisionsubLayoutEditPanel.DeviceLayout = e.Value;
			//_immerVisionsubLayoutEditPanel.Camera = e.Value2;
			Manager.ReplaceControl(_immerVisionEditPanel, _immerVisionsubLayoutEditPanel, contentPanel, ManagerMoveToImmerVisonSubLayoutEditComplete);
		}

		private UInt16 GetNewLayoutId()
		{
			if (Server == null) return 0;

			for (UInt16 id = 1; id <= Server.Device.DeviceLayouts.Count + 1; id++)
			{
				if (Server.Device.DeviceLayouts.ContainsKey(id)) continue;
				return id;
			}

			return 0;
		}

		private void ListPanelOnDeviceLayoutAdd(Object sender, EventArgs e)
		{
			IDeviceLayout layout = new DeviceLayout
			{
				Id = GetNewLayoutId(),
				ReadyState = ReadyState.New,
			};

			layout.Name = Localization["SetupDeviceLayout_NewPanorama"] + @" " + layout.Id;
			layout.Server = Server;

			if (!Server.Device.DeviceLayouts.ContainsKey(layout.Id))
				Server.Device.DeviceLayouts.Add(layout.Id, layout);

			_focusControl = _editPanel;

			_editPanel.Enabled = false;
			_editPanel.DeviceLayout = layout;

			Manager.ReplaceControl(_listPanel, _editPanel, contentPanel, ManagerMoveToEditComplete);
		}

		void _listPanel_OnImmerVisionLayoutAdd(object sender, EventArgs e)
		{
			IDeviceLayout layout = new DeviceLayout
			{
				isImmerVision = true,
				Id = GetNewLayoutId(),
				ReadyState = ReadyState.New,
			};

			//layout.Name = Localization["SetupDeviceLayout_NewDeviceLayout"] + @" " + layout.Id;
			layout.Name = "Ceiling-PTZ" + @" " + layout.Id;
			layout.Server = Server;


			if (!Server.Device.DeviceLayouts.ContainsKey(layout.Id))
				Server.Device.DeviceLayouts.Add(layout.Id, layout);

			_focusControl = _immerVisionEditPanel;

			_immerVisionEditPanel.Enabled = false;
			_immerVisionEditPanel.DeviceLayout = layout;
			//_immerVisionEditPanel.Camera = null;

			Manager.ReplaceControl(_listPanel, _immerVisionEditPanel, contentPanel, ManagerMoveToImmerVisionEditComplete);
		}

		private void ManagerMoveToEditComplete()
		{
			_focusControl = _editPanel;

			_editPanel.ParseDeviceLayout();
			_editPanel.Enabled = true;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, _editPanel.DeviceLayout.ToString(), "Back", "")));
		}

		private void ManagerMoveToImmerVisionEditComplete()
		{
			_focusControl = _immerVisionEditPanel;

			_immerVisionEditPanel.ParseDeviceLayout();
			_immerVisionEditPanel.Enabled = true;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, _immerVisionEditPanel.DeviceLayout.ToString(), "Back", "")));
		}

		private void ManagerMoveToSubLayoutEditComplete()
		{
			_focusControl = _subLayoutEditPanel;

			_subLayoutEditPanel.ParseDeviceLayout();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, _subLayoutEditPanel.DeviceLayout.ToString(), "Back", "")));
		}

		private void ManagerMoveToImmerVisonSubLayoutEditComplete()
		{
			_focusControl = _immerVisionsubLayoutEditPanel;

			_immerVisionsubLayoutEditPanel.ParseDeviceLayout();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, _immerVisionsubLayoutEditPanel.DeviceLayout.ToString(), "Back", "")));
		}

		private void DeleteLayout()
		{
			_focusControl = _listPanel;

			_listPanel.ShowCheckBox();

			var text = TitleName + "  /  " + Localization["SetupDeviceLayout_DeletePanorama"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
		}

		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				ShowDeviceLayoutList();
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
