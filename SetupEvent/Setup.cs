using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupEvent
{
	public partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;

		public String TitleName { get; set; }
		public IServer Server { get; set; }
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

		private NVRListPanel _nvrListPanel;
		private ListPanel _listPanel;

		protected EventHandleListPanel _eventHandleListPanel;
		private CopyEventHandlePanel _copyEventHandlePanel;

		private Control _focusControl;

		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_Event", "Event"},

								   {"SetupEvent_CopyEventHandling", "Copy Event Handling"},
								   {"SetupEvent_DeleteEventHandler", "Delete Event Handler"},
							   };
			Localizations.Update(Localization);

			Name = "Event";
			TitleName = Localization["Control_Event"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Event"] };
			Icon.Click += DockIconClick;

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
			//---------------------------
		}

		public virtual void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_nvrListPanel = new NVRListPanel()
			{
				Server = Server
			};
			_nvrListPanel.Initialize();
			_nvrListPanel.OnNVRClick += NVRListPanelOnNVRClick;

			_listPanel = new ListPanel
			{
				Server = Server,
			};
			_listPanel.Initialize();
			_listPanel.OnDeviceEdit += ListPanelOnDeviceEdit;

			if (_eventHandleListPanel == null)
			{
				_eventHandleListPanel = new EventHandleListPanel
				{
					Server = Server,
				};
				_eventHandleListPanel.Initialize();

				_eventHandleListPanel.OnEventEditClick += EventHandleListPanelOnEventEditClick;
			}
			
			_copyEventHandlePanel = new CopyEventHandlePanel
			{
				Server = Server,
			};
			_copyEventHandlePanel.Initialize();

			if(Server is ICMS) 
				contentPanel.Controls.Add(_nvrListPanel);

			contentPanel.Controls.Add(_listPanel);

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

				if(Server is ICMS)
				{
					if (_focusControl == null) _focusControl = _nvrListPanel;
					Manager.ReplaceControl(_focusControl, _nvrListPanel, contentPanel, ShowNVRList);
				}
				else
				{
					if (_focusControl == null) _focusControl = _listPanel;
					Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowDeviceList);
				}
				
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

			 if (Server is ICMS)
				ShowNVRList();
			else if(Server is INVR)
				ShowDeviceList();
			else if (Server is IFOS)
				ShowEventList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_eventHandleListPanel.RemoveSelectedHandles();

					if (Server is IFOS)
						ShowEventList();
					else if (Server is INVR)
						EditDevice(_eventHandleListPanel.Camera);
					break;

				case "Delete":
					DeleteHandle();
					break;

				case "CopyEventHandling":
					CopyEventHandle();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (Server is IFOS)
						{
							ShowEventList();
						}
						else
						{
							if(Server is ICMS && _focusControl == _listPanel)
							{
								ShowNVRList();
							}
							else
							{
								if (_addNewHandler)
								{
									EditDevice(_eventHandleListPanel.Camera);
								}
								else
								{
									ShowDeviceList();
								}
							}
							
						}
					}
					break;
			}
		}

		private void CopyEventHandle()
		{
			_focusControl = _copyEventHandlePanel;

			_copyEventHandlePanel.GenerateViewModel();

			Manager.ReplaceControl(_listPanel, _copyEventHandlePanel, contentPanel, ManagerMoveToCopyComplete);
		}

		private void ShowNVRList()
		{
			_addNewHandler = false;
			_focusControl = _nvrListPanel;

			//_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_nvrListPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_nvrListPanel);
			}

			_nvrListPanel.GenerateViewModel();

			OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
					"","")));
		}


		private void ShowDeviceList()
		{
			_addNewHandler = false;
			_focusControl = _listPanel;

			//_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			if (OnSelectionChange != null)
			{
				if (Server is ICMS)
				{
					var text = TitleName + "  /  " + _listPanel.Server;

					var cms = Server as ICMS;
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text,
					"Back", (cms.NVRManager.DeviceChannelTable.Count > 1) ? "CopyEventHandling" : "")));
				}
				else
				{
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						 TitleName, "", ((Server.Device.Devices.Count > 1) ? "CopyEventHandling" : ""))));
				}
			}
				
		}

		private void ShowEventList()
		{
			_addNewHandler = false;
			_focusControl = _eventHandleListPanel;

			if (!contentPanel.Controls.Contains(_eventHandleListPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_eventHandleListPanel);
			}

			_eventHandleListPanel.GenerateEventModel();

			Boolean hasHandle = false;
			if (Server is IFOS)
			{
				foreach (var obj in ((IFOS)Server).EventHandling)
				{
					if (obj.Value.Count <= 0) continue;
					hasHandle = true;
					break;
				}
			}

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", ((hasHandle) ? "Delete" : ""))));
		}

		private void NVRListPanelOnNVRClick(object sender, EventArgs<INVR> e)
		{
			EditNVR(e.Value);
		}

		private void ListPanelOnDeviceEdit(Object sender, EventArgs<IDevice> e)
		{
			EditDevice(e.Value);
		}
		
		private Boolean _addNewHandler = false;

		protected void EventHandleListPanelOnEventEditClick(Object sender, EventArgs e)
		{
			_addNewHandler = true;
			_eventHandleListPanel.AddHandle();

			var text = TitleName + "  /  ";
			if (Server is ICMS) 
				text += _listPanel.Server + " / ";
			text += _eventHandleListPanel.Camera + " / ";
			text += _eventHandleListPanel.EventHandlePanel.EventCondition.CameraEvent.ToLocalizationString();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void EditNVR(INVR nvr)
		{
			_addNewHandler = false;
			_listPanel.Server = nvr;
			_focusControl = _listPanel;

			Manager.ReplaceControl(_nvrListPanel, _listPanel, contentPanel, ManagerMoveToNVRComplete);
		}

		private void EditDevice(IDevice device)
		{
			_addNewHandler = false;
			if (!(device is ICamera)) return;

			_focusControl = _eventHandleListPanel;
			_eventHandleListPanel.Camera = (ICamera)device;

			Manager.ReplaceControl(_listPanel, _eventHandleListPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ManagerMoveToNVRComplete()
		{
			var text = TitleName + "  /  " + _listPanel.Server;

			ShowDeviceList();

			
			if (OnSelectionChange != null)
			{
				var cms = Server as ICMS;
				if (cms != null)
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", (cms.NVRManager.DeviceChannelTable.Count > 1) ? "CopyEventHandling" : "")));
			}
		}

		private void ManagerMoveToEditComplete()
		{
			_eventHandleListPanel.ParseDevice();

			Boolean hasHandle = false;
			foreach (var obj in _eventHandleListPanel.Camera.EventHandling)
			{
				if (obj.Value.Count <= 0) continue;
				hasHandle = true;
				break;
			}

			var text = TitleName;
			if(Server is ICMS)
			{
				text += "  /  " + _listPanel.Server;
			}
			
			text += "  /  " + _eventHandleListPanel.Camera;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", ((hasHandle) ? "Delete" : ""))));
		}
		
		private void ManagerMoveToCopyComplete()
		{
			var text = TitleName + "  /  " + Localization["SetupEvent_CopyEventHandling"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void DeleteHandle()
		{
			_eventHandleListPanel.SelectionVisible = true;

			var text = TitleName + "  /  " ;
			if(Server is ICMS)
				text += _listPanel.Server + "  /  ";
			text += _eventHandleListPanel.Camera + " - " + Localization["SetupEvent_DeleteEventHandler"];
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "Confirm")));
		}
		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
			{
				if (Server is ICMS)
					ShowNVRList();
				else if (Server is INVR)
					ShowDeviceList();
				else if (Server is IFOS)
					ShowEventList();
			}
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
