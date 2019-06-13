using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using Investigation.Base;
using PanelBase;

namespace Investigation.EventCalendar
{
	public sealed partial class EventCalendar : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<Boolean>> OnTitleBarVisibleChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;
		public event EventHandler<EventArgs<CameraEventSearchCriteria>> OnEventSearch;
			
		private INVR _nvr;
		public IApp App { get; set; }
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
				if (value is INVR)
					_nvr = value as INVR;
			}
		}
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public CameraEventSearchCriteria SearchCriteria;

		public CriteriaPanel CriteriaPanel;
        public NVRListPanel NVRListPanel;
		public DevicePanel DevicePanel;
		public DateTimePanel DateTimePanel;
		public EventPanel EventPanel;
		public SearchPanel SearchPanel;

		private Control _focusControl;

		public String TitleName { get; set; }

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.calendarIcon, Properties.Resources.IMGCalendarIcon);
		private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.calendarIcon_activate, Properties.Resources.IMGCalendarIconActivate);

		public UInt16 MinimizeHeight
		{
			get { return 0; }
		}
		public Boolean IsMinimize { get; private set; }

		public EventCalendar()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_EventCalendar", "Calendar"},

								   {"MessageBox_Information", "Information"},
								   
								   {"Investigation_Device", "Device"},
								   {"Investigation_DateTime", "Date / Time"},
								   {"Investigation_Event", "Event"},
								   {"Investigation_NotSelectDevice", "Not select device"},
								   {"Investigation_NotSelectEvent", "Not select event"},
								   
								   {"Investigation_SearchResult", "Search Result"},
							   };
			Localizations.Update(Localization);

			Name = "EventCalendar";
			TitleName = Localization["Control_EventCalendar"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;

			SearchCriteria = new CameraEventSearchCriteria
			{
				DateTimeSet = DateTimeSet.ThisMonth
			};

			//---------------------------
			Icon = new ControlIconButton { Image = _iconActivate, BackgroundImage = ControlIconButton.IconBgActivate };
			Icon.Click += DockIconClick;

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
			//---------------------------
		}

		private void SetDefaultCondition()
		{
			SearchCriteria.DateTimeSet = DateTimeSet.ThisMonth;
			var range = DateTimes.UpdateStartAndEndDateTime(_nvr.Server.DateTime, _nvr.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

            SearchCriteria.NVRDevice.Clear();
			SearchCriteria.Device.Clear();
			SearchCriteria.Event.Clear();

			//var sortResult = new List<UInt16>(_nvr.Device.Devices.Keys);
			//sortResult.Sort((x, y) => (x - y));

			//foreach (var deviceId in sortResult)
			//{
			//    SearchCriteria.Device.Add(deviceId);
			//}
		}

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			var range = DateTimes.UpdateStartAndEndDateTime(_nvr.Server.DateTime, _nvr.Server.TimeZone, SearchCriteria.DateTimeSet);
			SearchCriteria.StartDateTime = range[0];
			SearchCriteria.EndDateTime = range[1];

			//var sortResult = new List<UInt16>(_nvr.Device.Devices.Keys);
			//sortResult.Sort((x, y) => (x - y));

			//foreach (var deviceId in sortResult)
			//{
			//    SearchCriteria.Device.Add(deviceId);
			//}

			CriteriaPanel = new CriteriaPanel
			{
				NVR = _nvr,
				SearchCriteria = SearchCriteria
			};
			CriteriaPanel.Initialize();
			if (Server is ICMS)
			{
				CriteriaPanel.CMS = (ICMS)Server;
			}

		    NVRListPanel = new NVRListPanel
		    {
                Server = Server,
                SearchCriteria = SearchCriteria
		    };
            NVRListPanel.Initialize();
            NVRListPanel.OnNVRClick += NVRListPanelOnNVRClick;

			DevicePanel = new DevicePanel
			{
				NVR = _nvr,
				SearchCriteria = SearchCriteria
			};


            if (Server is ICMS)
            {
                DevicePanel.CMS = (ICMS)Server;
            }

			DateTimePanel = new DateTimePanel
			{
				NVR = _nvr,
				SearchCriteria = SearchCriteria,
				DateSetArray = new[] { DateTimeSet.ThisMonth, DateTimeSet.LastMonth, DateTimeSet.TheMonthBeforeLast },
			};
			DateTimePanel.ShowDateTimeSelectionRange();
			DateTimePanel.ShowOnlyMonthSelection();
			DateTimePanel.Initialize();

			EventPanel = new EventPanel
			{
				NVR = _nvr,
				SearchCriteria = SearchCriteria,
				ShowEventColor = true
			};

			SearchPanel = new SearchPanel
			{
				NVR = _nvr,
				App = App,
				SearchCriteria = SearchCriteria
			};
			if (Server is ICMS)
			{
				SearchPanel.CMS = (ICMS)Server;
			}
			SearchPanel.Initialize();

			_nvr.OnDeviceModify -= NVROnDeviceModify;

			CriteriaPanel.OnDeviceEdit += CriteriaPanelOnDeviceEdit;
			CriteriaPanel.OnDateTimeEdit += CriteriaPanelOnDateTimeEdit;
			CriteriaPanel.OnEventEdit += CriteriaPanelOnEventEdit;

			SearchPanel.OnEventSearchCriteriaChange += SearchPanelOnEventSearchCriteriaChange;
			contentPanel.Controls.Contains(CriteriaPanel);
		}

        private void NVRListPanelOnNVRClick(object sender, EventArgs<INVR> e)
        {
            EditNVR(e.Value);
        }

        private void EditNVR(INVR nvr)
        {
            DevicePanel.NVR = nvr;
            _focusControl = DevicePanel;

            Manager.ReplaceControl(NVRListPanel, DevicePanel, contentPanel, ManagerMoveToNVRComplete);
        }

		private Boolean _isActivate;
        private Boolean _isInitial;
		public void Activate()
		{
			if (!BlockPanel.IsFocusedControl(this)) return;

            if (!_isInitial)//for start option(after bind event)
            {
                if (Server.Configure.StartupOptions.VideoTitleBar)
                    VideoTitleBar(this, null);
            }

            _isInitial = true;

			_isActivate = true;
		}

		public void Deactivate()
		{
			_isActivate = false;
		}

        private Boolean _displayTitleBar;
        public void VideoTitleBar(Object sender, EventArgs e)
        {
            _displayTitleBar = !_displayTitleBar;

            if (OnTitleBarVisibleChange != null)
                OnTitleBarVisibleChange(this, new EventArgs<Boolean>(_displayTitleBar));
        }

		private void CriteriaPanelOnDeviceEdit(Object sender, EventArgs e)
		{
            if(Server is ICMS)
            {
                ShowNVRList();
            }
            else
            {
                _focusControl = DevicePanel;
                DevicePanel.ParseSetting();
                Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
            }
		}

		private void CriteriaPanelOnDateTimeEdit(Object sender, EventArgs e)
		{
			_focusControl = DateTimePanel;

			DateTimePanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}

		private void CriteriaPanelOnEventEdit(Object sender, EventArgs e)
		{
			_focusControl = EventPanel;

			EventPanel.ParseSetting();

			Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToSettingComplete);
		}


		private void SearchPanelOnEventSearchCriteriaChange(Object sender, EventArgs<CameraEventSearchCriteria> e)
		{
			if (OnEventSearch != null)
				OnEventSearch(this, e);
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			if (BlockPanel != null)
				BlockPanel.ShowThisControlPanel(this);

			var text = TitleName + "  /  " + Localization["Investigation_SearchResult"];

			//keep search result, even change to other report page
			if (_focusControl == SearchPanel)
			{
				if (OnSelectionChange != null)
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", ""))); 

				return;
			}
			ShowCriteriaPanel();
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "SearchEvent":
					if (_server is ICMS)
					{
						if (SearchCriteria.NVRDevice.Count == 0)
						{
							TopMostMessageBox.Show(Localization["Investigation_NotSelectDevice"], Localization["MessageBox_Information"], MessageBoxButtons.OK);
							return;
						}
					}
					else
					{
						if (SearchCriteria.Device.Count == 0)
						{
							TopMostMessageBox.Show(Localization["Investigation_NotSelectDevice"], Localization["MessageBox_Information"], MessageBoxButtons.OK);
							return;
						}
					}

					if (SearchCriteria.Event.Count == 0)
					{
						TopMostMessageBox.Show(Localization["Investigation_NotSelectEvent"], Localization["MessageBox_Information"], MessageBoxButtons.OK);
						return;
					}
					if (!_isSearching)
						SearchEvent();
					break;

				case "ClearConditional":
					SetDefaultCondition();
					CriteriaPanel.Invalidate();
                    if (Server is ICMS)
                        NVRListPanel.Invalidate();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
                        if (Server is ICMS && _focusControl == DevicePanel)
                        {
                            ShowNVRList();
                        }
                        else
                        {
                            ShowCriteriaPanel();
                        }
					}
					break;
			}
		}

        private void ShowNVRList()
        {
            _focusControl = NVRListPanel;
            if (!contentPanel.Controls.Contains(NVRListPanel))
            {
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(NVRListPanel);
            }

            NVRListPanel.GenerateViewModel();
            Manager.ReplaceControl(CriteriaPanel, _focusControl, contentPanel, ManagerMoveToNVRComplete);
        }

		private Boolean _isSearching;
		private void SearchEvent()
		{
			_isSearching = true;
			_focusControl = SearchPanel;

			Manager.ReplaceControl(CriteriaPanel, SearchPanel, contentPanel, ManagerMoveToSearchComplete);
		}

		private void ShowCriteriaPanel()
		{
			_isSearching = false;

			_focusControl = CriteriaPanel;

			if (!contentPanel.Controls.Contains(CriteriaPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(CriteriaPanel);
			}

			CriteriaPanel.ShowCriteria();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "SearchEvent,ClearConditional")));
		}

        private void ManagerMoveToNVRComplete()
        {
            _focusControl.Focus();

            var text = TitleName;
            text += "  /  " + (_focusControl is DevicePanel ? DevicePanel.NVR.ToString() : Localization["Investigation_Device"]);

            DevicePanel.ParseSetting();

            if (OnSelectionChange != null)
            {
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", _focusControl is DevicePanel ? "SearchEvent" : "SearchEvent,ClearConditional")));
            }
        }

		private void ManagerMoveToSettingComplete()
		{
			_focusControl.Focus();

			var text = TitleName + "  /  ";
			text += (Localization.ContainsKey("Investigation_" + _focusControl.Name))
						? Localization["Investigation_" + _focusControl.Name]
						: _focusControl.Name;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					text, "Back", "SearchEvent")));
		}

		private void ManagerMoveToSearchComplete()
		{
			SearchPanel.Focus();

			SearchPanel.SearchEvent(SearchCriteria.StartDateTime, SearchCriteria.EndDateTime);//Page 1

			var text = TitleName + "  /  " + Localization["Investigation_SearchResult"];

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, text, "Back", "")));
		}

		private void NVROnDeviceModify(Object sender, EventArgs<IDevice> e)
		{
			if (e.Value.ReadyState == ReadyState.Delete)
			{
				if (_server is ICMS)
				{
					foreach (NVRDevice nvrDevice in SearchCriteria.NVRDevice)
					{
						if (nvrDevice.NVRId == e.Value.Server.Id && nvrDevice.DeviceId == e.Value.Id)
						{
							SearchCriteria.NVRDevice.Remove(nvrDevice);
							break;
						}
					}
					return;
				}
				SearchCriteria.Device.Remove(e.Value.Id);
			}
		}

		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else //dont hide self to keep at last selection panel on screen
				ShowCriteriaPanel();
		}

		public void Minimize()
		{
			if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
				BlockPanel.HideThisControlPanel(this);

			Deactivate();
			Icon.Image = _icon;
			Icon.BackgroundImage = null;

			IsMinimize = true;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}

		public void Maximize()
		{
			ShowContent(this, null);

			Icon.Image = _iconActivate;
			Icon.BackgroundImage = ControlIconButton.IconBgActivate;

			IsMinimize = false;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}
	}
}
