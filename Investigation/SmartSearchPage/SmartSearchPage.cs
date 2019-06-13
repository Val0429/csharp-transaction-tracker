using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;
using PanelBase;

namespace Investigation.SmartSearchPage
{
	public sealed partial class SmartSearchPage : UserControl, IControl, IAppUse, IServerUse, IBlockPanelUse, IKeyPress, IMouseHandler, IMinimize//, IFocus
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnSelectionChange;
		public event EventHandler<EventArgs<Boolean>> OnTitleBarVisibleChange;
		public event EventHandler<EventArgs<Int32>> OnViewingDeviceNumberChange;

		public IApp App { get; set; }
		private INVR _nvr;
		private ICMS _cms;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
				if (value is INVR)
				{
                    _nvr = value as INVR;
                    Visible = Icon.Visible = _server.Server.CheckProductNoToSupport("smartSearch");
				}
				if (value is ICMS)
					_cms = value as ICMS;
			}
		}
		public IBlockPanel BlockPanel { get; set; }
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.smartSearchIcon, Properties.Resources.IMGSmartSearchIcon);
		private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.smartSearchIcon_activate, Properties.Resources.IMGSmartSearchIconActivate);

		public UInt16 MinimizeHeight
		{
			get { return 0; }
		}
		public Boolean IsMinimize { get; private set; }
		private Int32 _smartSearchPanelHeight;
		private Int32 _timeTrackPanelHeight;

		public SmartSearchPage()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_SmartSearch", "Smart Search"},
							   };
			Localizations.Update(Localization);

			Name = "SmartSearch";
			TitleName = Localization["Control_SmartSearch"];

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			BackgroundImage = Manager.Background;

			_smartSearchPanelHeight = smartSearchPanel.Height;
			_timeTrackPanelHeight = timeTrackPanel.Height;
			//---------------------------
			Icon = new ControlIconButton { Image = _iconActivate, BackgroundImage = ControlIconButton.IconBgActivate };
			Icon.Click += DockIconClick;

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
			//---------------------------
		}

		private SmartSearch.SearchCondition _searchCondition;
		private SmartSearch.SmartSearch _smartSearch;
		private SmartSearch.SearchResult _searchResult;
		private TimeTrack.TimeTrack _timeTrack;
		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_smartSearch = new SmartSearch.SmartSearch
							   {
								   Server = Server,
								   App = App,
							   };
			smartSearchPanel.Controls.Add(_smartSearch);
			_smartSearch.Initialize();

			_searchCondition = new SmartSearch.SearchCondition
			{
				Server = Server,
				App = App,
				SmartSearchControl = _smartSearch,
				VideoWindow = _smartSearch.VideoWindow,
			};
			conditionPanel.Controls.Add(_searchCondition);
			_searchCondition.Initialize();

			_smartSearch.SearchConditionControl = _searchCondition;

			_searchResult = new SmartSearch.SearchResult
			{
				Server = Server,
			};
			_searchResult.OnMinimizeChange += SearchResultOnMinimizeChange;
			searchResultPanel.Controls.Add(_searchResult);
			_searchResult.Initialize();

			_timeTrack = new TimeTrack.TimeTrack
			{
				Server = Server,
				App = App,
			};
			timeTrackPanel.Controls.Add(_timeTrack);
			_timeTrack.Initialize();
			_timeTrack.OnMinimizeChange += TimeTrackOnMinimizeChange;
			_timeTrack.SetSmartSearchProperty();

			//----------------------------------------------------------------------------------
			
			_searchCondition.OnSmartSearchResult += _searchResult.SmartSearchResult;
			_searchCondition.OnSmartSearchComplete += _searchResult.SmartSearchComplete;
			_searchCondition.OnSearchStart += _searchResult.SearchStart;
			_searchCondition.OnSearchStartDateTimeChange += _timeTrack.SearchStartDateTimeChange;
			_searchCondition.OnSearchEndDateTimeChange += _timeTrack.SearchEndDateTimeChange;
			
			_smartSearch.OnTimecodeChange += _timeTrack.TimecodeChange;
			_smartSearch.OnContentChange += _searchResult.ContentChange;
			_smartSearch.OnContentChange += _timeTrack.ContentChange;
			_smartSearch.OnViewingDeviceNumberChange += ViewingDeviceNumberChange;

			_searchResult.OnTimecodeChange += _timeTrack.GoTo;

			_timeTrack.OnTimeUnitChange += _searchCondition.TimeUnitChange;
			_timeTrack.OnExportStartDateTimeChange += _searchCondition.ExportStartDateTimeChange;
			_timeTrack.OnExportEndDateTimeChange += _searchCondition.ExportEndDateTimeChange;

			_timeTrack.OnTimecodeChange += _smartSearch.GoTo;
			_timeTrack.OnTimecodeChange += _searchCondition.GoTo;
			_timeTrack.OnNextFrame += _smartSearch.NextFrame;
			_timeTrack.OnPreviousFrame += _smartSearch.PreviousFrame;
			_timeTrack.OnPlay += _smartSearch.Playback;
		    _timeTrack.OnPlayOnRate += _smartSearch.PlaybackOnRate;
			//----------------------------------------------------------------------------------
            if(Server is ICMS)
            {
                _cms.OnNVRModify += NVRModify;
            }

			if (_nvr != null)
			{
				_nvr.OnGroupModify += GroupModify;
				_nvr.OnDeviceModify += DeviceModify;
			}
		}

		private void ViewingDeviceNumberChange(Object sender, EventArgs<Int32> e)
		{
			if (OnViewingDeviceNumberChange != null)
				OnViewingDeviceNumberChange(this, e);
		}

		private void TimeTrackOnMinimizeChange(Object sender, EventArgs e)
		{
			if (_timeTrack.IsMinimize)
			{
				timeTrackPanel.Height = _timeTrack.MinimizeHeight;
			}
			else
			{
				timeTrackPanel.Height = _timeTrackPanelHeight;
			}
		}

		private void SearchResultOnMinimizeChange(Object sender, EventArgs e)
		{
			if (_searchResult.IsMinimize)
			{
                _smartSearch.Visible = true;
				smartSearchPanel.Height = _smartSearchPanelHeight;
			}
			else
			{
                _smartSearch.Visible = false;
				smartSearchPanel.Height = 0;
			}
		}
		
		private Boolean _reloadTree = true;

        private void NVRModify(object sender, EventArgs<INVR> e)
        {
            _reloadTree = true;
        }

		public void GroupModify(Object sender, EventArgs<IDeviceGroup> e)
		{
			_reloadTree = true;
		}

		public void DeviceModify(Object sender, EventArgs<IDevice> e)
		{
			_reloadTree = true;
		}

		private Boolean _displayTitleBar;
		public void VideoTitleBar(Object sender, EventArgs e)
		{
			_displayTitleBar = !_displayTitleBar;
			_smartSearch.VideoTitleBar();

			if (OnTitleBarVisibleChange != null)
				OnTitleBarVisibleChange(this, new EventArgs<Boolean>(_displayTitleBar));
		}

		public void Disconnect(Object sender, EventArgs e)
		{
			_smartSearch.Disconnect();
			_searchCondition.Disconnect();
		}

		private Boolean _isActivate;
        private Boolean _isInitial;
		public void Activate()
		{
			if (!BlockPanel.IsFocusedControl(this)) return;

            if (_reloadTree)
                _searchCondition.UpdateDevice();

			if (_isActivate) return;
			_reloadTree = false;

			_smartSearch.Activate();
			_searchCondition.Activate();
			_searchResult.Activate();
			_timeTrack.Activate();

			_isActivate = true;

            if (!_isInitial)//for start option(after bind event)
            {
                if (Server.Configure.StartupOptions.VideoTitleBar)
                    VideoTitleBar(this, null);
            }

            _isInitial = true;
		}

		public void Deactivate()
		{
			_smartSearch.Deactivate();
			_searchCondition.Deactivate();
			_searchResult.Deactivate();
			_timeTrack.Deactivate();

			_isActivate = false;
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			if (BlockPanel != null)
			{
				if (BlockPanel is IAppUse)
				{
					var panelApp = (IAppUse) BlockPanel;
					panelApp.App = App;
				}

				BlockPanel.ShowThisControlPanel(this);
			}
				
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "")));
		}

		//same device , same eventtype
		public void ApplyPlaybackParameter(Object sender, EventArgs<List<CameraEvents>, DateTime, UInt64, UInt64> obj)
		{
			Maximize();

			var eventList = obj.Value1;
			if(eventList == null || eventList.Count == 0) return;

			_smartSearch.AppendDevice(eventList[0].Device);
			_timeTrack.TrackerContainer.DateTime = obj.Value2;

			_searchResult.ApplyPlaybackParameter(eventList);

			_searchCondition.ApplyPlaybackParameter(eventList[0], obj.Value3, obj.Value4);
		}

		public void KeyboardPress(Keys keyData)
		{
			_timeTrack.KeyboardPress(keyData);
			_smartSearch.KeyboardPress(keyData);
		}

		//public void WindowFocusGet()
		//{
		//    _smartSearch.WindowFocusGet();
		//}

		//public void WindowFocusLost()
		//{
		//    _smartSearch.WindowFocusLost();
		//}

		public void GlobalMouseHandler()
		{
			_smartSearch.GlobalMouseHandler();

			//_searchResult.GlobalMouseHandler();
		}

		public void ExportVideo(Object sender, EventArgs e)
		{
			_timeTrack.ExportVideo(sender, e);
		}

		public void PrintImage(Object sender, EventArgs e)
		{
			_smartSearch.PrintImage(sender, e);
		}

		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			//else //dont hide self to keep at last selection panel on screen
			//    Minimize();
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

			Refresh();
		}
	}
}
