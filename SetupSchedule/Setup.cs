using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupSchedule
{
	public partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public virtual event EventHandler<EventArgs<String>> OnSelectionChange;

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

		private SchedulePanel _schedulePanel;
		private CopySchedulePanel _copySchedulePanel;

		private Control _focusControl;

		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_Schedule", "Schedule"},

								   {"SetupSchedule_CopySchedule", "Copy Schedule"},
							   };
			Localizations.Update(Localization);

			Name = "Schedule";
			TitleName = Localization["Control_Schedule"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			BackgroundImage = Manager.Background;
			//---------------------------
			Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_Schedule"] };
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

			_schedulePanel = new SchedulePanel
			{
				Server = Server,
			};
			_schedulePanel.Initialize();

			_copySchedulePanel = new CopySchedulePanel
			{
				Server = Server,
			};
			_copySchedulePanel.Initialize();
			
			_listPanel.OnDeviceEdit += ListPanelOnDeviceEdit;

			if (Server is ICMS)
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

				if (Server is ICMS)
				{
					if (_focusControl == null) 
						_focusControl = _nvrListPanel;
					Manager.ReplaceControl(_focusControl, _nvrListPanel, contentPanel, ShowNVRList);
				}
				else
				{
					if (_focusControl == null)
						_focusControl = _listPanel;
					Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowDeviceList);
				}

			}
			
		}

		private void ShowNVRList()
		{
			_focusControl = _nvrListPanel;

			//_listPanel.Enabled = true;
			if (!contentPanel.Controls.Contains(_nvrListPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_nvrListPanel);
			}

			_nvrListPanel.GenerateViewModel();

			OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
					"", "")));
		}

		//private delegate void GenerateViewModelDelegate();
		private void ShowDeviceList()
		{
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
				if(Server is ICMS)
				{
					var text = TitleName + "  /  " + _listPanel.Server;
					var cms = Server as ICMS;
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text,
					"Back", (cms.NVRManager.DeviceChannelTable.Count > 1) ? "CopySchedule" : "")));
				}
				else
				{
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
					"", (Server.Device.Devices.Count > 1) ? "CopySchedule" : "")));
				}
				
			}
				
		}

		private void NVRListPanelOnNVRClick(object sender, EventArgs<INVR> e)
		{
			EditNVR(e.Value);
		}

		private void ListPanelOnDeviceEdit(Object sender, EventArgs<IDevice> e)
		{
			EditDevice(e.Value);
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
			else if (Server is INVR)
				ShowDeviceList();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "CopySchedule":
					CopySchedule();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						if (Server is ICMS && _focusControl == _listPanel)
						{
							ShowNVRList();
						}
						else
						{
							ShowDeviceList();
						}
					}
					break;
			}
		}
		
		private void CopySchedule()
		{
			_focusControl = _copySchedulePanel;

			_copySchedulePanel.GenerateViewModel();

			Manager.ReplaceControl(_listPanel, _copySchedulePanel, contentPanel, ManagerMoveToCopyComplete);
		}

		private void EditNVR(INVR nvr)
		{
			_listPanel.Server = nvr;
			_focusControl = _listPanel;

			Manager.ReplaceControl(_nvrListPanel, _listPanel, contentPanel, ManagerMoveToNVRComplete);
		}

		protected virtual void EditDevice(IDevice device)
		{
			if (!(device is ICamera)) return;

			_focusControl = _schedulePanel;
			_schedulePanel.Camera = (ICamera)device;

			Manager.ReplaceControl(_listPanel, _schedulePanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ManagerMoveToNVRComplete()
		{
			var text = TitleName + "  /  " + _listPanel.Server;

			ShowDeviceList();


			if (OnSelectionChange != null)
			{
				var cms = Server as ICMS;
				if (cms != null)
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", (cms.NVRManager.DeviceChannelTable.Count > 1) ? "CopySchedule" : "")));
			}
		}

		protected virtual void ManagerMoveToEditComplete()
		{
			_schedulePanel.ParseDevice();
			var text = TitleName + "  /  ";
			if (Server is ICMS)
				text += _listPanel.Server + " / ";
			text += _schedulePanel.Camera;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}
		
		private void ManagerMoveToCopyComplete()
		{
			var text = TitleName + "  /  " + Localization["SetupSchedule_CopySchedule"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}
		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				if (Server is ICMS)
					ShowNVRList();
				else if (Server is INVR)
					ShowDeviceList();
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

	    public void SISSetupView()
	    {
	        _schedulePanel.Enabled = false;
	    }
	}
}
