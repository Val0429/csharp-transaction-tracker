using Constant;
using Device;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Timer = System.Windows.Forms.Timer;

namespace VideoMonitor
{
    public partial class VideoMonitor : UserControl, IControl, IAppUse, IServerUse, IDrop, IMouseHandler, IFullScreen, IKeyPress,
        IUpdateClientSetting, IPopupStream//IFocus, 
    {
        public event EventHandler<EventArgs<ICamera, PTZMode>> OnSelectionChange;
        protected void RaiseOnSelectionChange(EventArgs<ICamera, PTZMode> e)
        {
            if (OnSelectionChange != null)
            {
                OnSelectionChange(this, e);
            }
        }

        private Boolean _stopTriggerOnContentChangeEvent = false;
        public event EventHandler<EventArgs<Object>> OnContentChange;
        protected void RaiseOnContentChange(Object obj)
        {
            if (_stopTriggerOnContentChangeEvent) return;

            if (OnContentChange != null)
                OnContentChange(this, new EventArgs<Object>(obj));
        }

        public event EventHandler<EventArgs<List<WindowLayout>>> OnLayoutChange;
        public event EventHandler<EventArgs<String>> OnPTZModeChange;
        public event EventHandler<EventArgs<UInt16, UInt16>> OnPageChange;
        public event EventHandler<EventArgs<List<IVideoWindow>>> OnPageChangeLayout;
        public event EventHandler<EventArgs<DecodeMode>> OnDecodeChange;
        public event EventHandler<EventArgs<Boolean>> OnTitleBarVisibleChange;

        protected void RaiseOnTitleBarVisibleChanged(EventArgs<bool> e)
        {
            var handler = OnTitleBarVisibleChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<EventArgs<String>> OnBitrateUsageChange;
        public event EventHandler<EventArgs<Int32>> OnViewingDeviceNumberChange;
        protected void RaiseOnViewingDeviceNumberChange(EventArgs<int> e)
        {
            var handler = OnViewingDeviceNumberChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<EventArgs<List<IDevice>>> OnViewingDeviceListChange;
        protected void RaiseOnViewingDeviceListChange(EventArgs<List<IDevice>> e)
        {
            var handler = OnViewingDeviceListChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler OnDiscountVideoWindowByMenu;
        public event EventHandler<EventArgs<List<WindowMountLayout>>> OnReadAllVideoWindows;

        public event EventHandler OnExitFullScreen;

        public event EventHandler<EventArgs<String>> OnTimecodeChange;

        protected void RaiseOnTimecodeChange(ulong timecode)
        {
            if (OnTimecodeChange != null)
            {
                OnTimecodeChange(this, new EventArgs<string>(TimecodeChangeXml(timecode.ToString(CultureInfo.InvariantCulture))));
            }
        }

        protected UInt16 MaxConnection = 64;
        protected Boolean AllowDuplicate = true;
        public Boolean IsPopup { get; set; }

        public UInt16 PageIndex = 1;
        public UInt16 PageCount { get; protected set; }

        public String TitleName { get; set; }
        public IApp App { get; set; }
        protected INVR NVR { get; set; }
        protected ICMS CMS { get; set; }
        protected IPTS PTS { get; set; }

        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;

                if (value is INVR)
                    NVR = value as INVR;
                if (value is ICMS)
                    CMS = value as ICMS;
                if (value is IPTS)
                    PTS = value as IPTS;

                App.OnLockApplication -= AppOnLockApplication;
                App.OnLockApplication += AppOnLockApplication;
            }
        }

        public List<IVideoWindow> VideoWindows = new List<IVideoWindow>();
        public Dictionary<Int32, IDevice> Devices = new Dictionary<Int32, IDevice>();
        protected readonly System.Timers.Timer GetTimecodeTimer = new System.Timers.Timer();
        protected readonly System.Timers.Timer UpdateBitrateTimer = new System.Timers.Timer();

        public Dictionary<String, String> Localization { get; set; }

        protected virtual IVideoWindow ActivateVideoWindow { get; set; }

        protected PlayMode PlayMode = PlayMode.LiveStreaming;

        protected Pager.Pager PagerControl;

        private static readonly Image _nextPage = Resources.GetResources(Properties.Resources.nextPage, Properties.Resources.IMGNextPage);
        private static readonly Image _previousPage = Resources.GetResources(Properties.Resources.previousPage, Properties.Resources.IMGPreviousPage);
        private static readonly Image _popup = Resources.GetResources(Properties.Resources.popup, Properties.Resources.IMGPopup);

        private static readonly Image _left = Resources.GetResources(Properties.Resources.left, Properties.Resources.IMGLeft);
        private static readonly Image _right = Resources.GetResources(Properties.Resources.right, Properties.Resources.IMGRight);


        // Constructor
        public VideoMonitor()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Confirm", "Confirm"},
								   
								   {"VideoMonitor_ExitFullScreen", "Exit Full Screen"},
								   {"VideoMonitor_Previous", "Previous"},
								   {"VideoMonitor_Next", "Next"},
								   {"VideoMonitor_Popup", "Pop up View Monitor"},
								   {"VideoMonitor_PageQuantity", "%1 of %2"},

								   {"VideoMonitor_DeleteGroupConfirm", "Are you sure want to delete \"%1\" ?"},
								   {"VideoMonitor_NoPermissionToDelete", "You don't have permission to delete shared view."},
								   
								   {"VideoMonitor_MaximumPopupVideoMonitor", "Reached maximum popup video monitor \"%1\""},
								   {"VideoMonitor_SetupStorageFirst", "Setup storage first. Please goto Setup -> Server -> Storage"},

							   };

            Localizations.Update(Localization);

            InitializeComponent();

            BackgroundImage = Resources.GetResources(Properties.Resources.monitorBG, Properties.Resources.BGMonitorBG);
            DoubleBuffered = true;

            pagerPanelButton.BackgroundImage = _left;
            pagerPanelButton.MouseClick += PagerPanelButtonMouseClick;

            nameLabel.Paint += NameLabelPaint;
        }

        private void AppOnLockApplication(object sender, EventArgs<Boolean> e)
        {
            foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
            {
                popupVideoMonitor.Enabled = e.Value != true;
            }
        }

        public virtual void Initialize()
        {
            InitializeMenu();
            InitializeLayout();
            InitializePager();

            GetTimecodeTimer.Elapsed += GetTimecode;
            GetTimecodeTimer.Interval = 1000;
            GetTimecodeTimer.SynchronizingObject = Server.Form;

            UpdateBitrateTimer.Elapsed += GetBitrate;
            UpdateBitrateTimer.Interval = 1000;
            UpdateBitrateTimer.SynchronizingObject = Server.Form;

            Dock = DockStyle.Fill;

            windowsPanel.SizeChanged += WindowsPanelSizeChanged;

            nextPageButton.BackgroundImage = previousPageButton.BackgroundImage = popupPanel.BackgroundImage = null;
            nextPageButton.Enabled = previousPageButton.Enabled = popupPanel.Enabled = false;

            if (NVR != null)
            {
                NVR.OnDeviceModify += DeviceModify;
                NVR.OnGroupModify += GroupModify;
                NVR.OnDeviceLayoutModify += DeviceLayoutModify;
                NVR.OnSubLayoutModify += SubLayoutModify;

                NVR.OnCameraStatusReceive -= EventReceive;
                NVR.OnCameraStatusReceive += EventReceive;
            }

            if (CMS != null)
            {
                CMS.OnNVRModify += NVRModify;
            }

            if (PTS != null)
            {
                PTS.OnNVRModify += NVRModify;
                PTS.OnPOSModify += POSModify;
                popupPanel.Visible = false;
            }

            CoolDownTimer.Elapsed += AppendDeviceCoolDown;
            CoolDownTimer.Interval = 500;
            CoolDownTimer.SynchronizingObject = Server.Form;

            App.OnCustomVideoStream += AppOnCustomVideoStream;
        }

        private void InitializePager()
        {
            PagerControl = new Pager.Pager { Dock = DockStyle.Fill, Server = Server };
            pagerPanel.Controls.Add(PagerControl);

            PagerControl.OnLayoutChange += LayoutChange;
            OnLayoutChange += PagerControl.LayoutChange;
        }

        private Int32 _toolPanelHeight;
        public void FullScreen()
        {
            if (!App.IsFullScreen) return;

            _toolPanelHeight = toolPanel.Height;
            toolPanel.Height = 0;
            ToolMenu.PanelPoint = windowsPanel.Location;

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Visible)
                    videoWindow.Viewer.UpdateRecordStatus();
            }

            foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
            {
                popupVideoMonitor.FullScreen();
            }

            //create group menu everytime on fullscreen enable
            if (_exitFullScreenMenuItem != null && contextMenu != null)
            {
                contextMenu.MenuItems.Clear();

                if (NVR != null)
                {
                    var groups = NVR.Device.Groups.Values.OrderBy(g => g.Id);

                    foreach (var deviceGroup in groups)
                    {
                        if (deviceGroup.Items.Count == 0) continue;

                        var groupMenuItem = new MenuItem(deviceGroup.ToString()) { Tag = deviceGroup };
                        groupMenuItem.Click += GroupMenuItemClick;
                        contextMenu.MenuItems.Add(groupMenuItem);
                    }

                    groups = NVR.User.Current.DeviceGroups.Values.OrderBy(g => g.Id);

                    foreach (var deviceGroup in groups)
                    {
                        if (deviceGroup.Items.Count == 0) continue;

                        var groupMenuItem = new MenuItem(deviceGroup.ToString()) { Tag = deviceGroup };
                        groupMenuItem.Click += GroupMenuItemClick;
                        contextMenu.MenuItems.Add(groupMenuItem);
                    }
                }
                contextMenu.MenuItems.Add(_exitFullScreenMenuItem);
            }

            windowsPanel.ContextMenu = contextMenu;
        }

        public void ExitFullScreen()
        {
            if (App.IsFullScreen) return;

            if (_toolPanelHeight > 0)
            {
                toolPanel.Height = _toolPanelHeight;
                ToolMenu.PanelPoint = windowsPanel.Location;
            }

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Visible)
                    videoWindow.Viewer.UpdateRecordStatus();
            }

            foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
            {
                popupVideoMonitor.ExitFullScreen();
            }

            windowsPanel.ContextMenu = null;
        }

        public void EventReceive(Object sender, EventArgs<List<ICamera>> e)
        {
            if (ActivateVideoWindow != null)
            {
                if (ToolMenu != null)
                    ToolMenu.CheckStatus();
            }

            foreach (var camera in e.Value)
            {
                if (camera == null) continue;

                foreach (var videoWindow in VideoWindows)
                {
                    if (videoWindow.Camera == camera)
                        videoWindow.Viewer.UpdateRecordStatus();
                }

                foreach (var instantPlayback in UsingInstantPlayback)
                {
                    if (instantPlayback.Camera == camera)
                        instantPlayback.UpdateRecordStatus();
                }

                foreach (var popupLiveStream in UsingPopupLive)
                {
                    if (popupLiveStream.Camera == camera)
                        popupLiveStream.UpdateRecordStatus();
                }

                foreach (var popupVideoMonitor in UsingPatrolVideoMonitor)
                {
                    popupVideoMonitor.UpdateRecordStatus();
                }
            }
        }

        private List<WindowMountLayout> _cameraRegions = new List<WindowMountLayout>();
        public void ReadAllVideoWindows(Object sender, EventArgs e)
        {
            _cameraRegions.Clear();
            foreach (var videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null)
                {
                    _cameraRegions.Add(null);
                    continue;
                }

                var regionString = videoWindow.Viewer.GetDigitalPtzRegion();

                try
                {
                    _cameraRegions.Add(new WindowMountLayout
                                           {
                                               RegionXML = (XmlElement)Xml.LoadXml(regionString).SelectSingleNode("/PTZRegions"),
                                               DewarpEnable = videoWindow.Viewer.Dewarp,
                                               MountType = videoWindow.Viewer.DewarpType
                                           });
                }
                catch (Exception)
                {
                    _cameraRegions.Add(null);
                }
            }

            if (OnReadAllVideoWindows != null)
                OnReadAllVideoWindows(this, new EventArgs<List<WindowMountLayout>>(_cameraRegions));
        }

        protected virtual void ChangeNameLabelText()
        {
            if (ActivateVideoWindow != null && ActivateVideoWindow.Camera != null)
            {
                _nameLabelText = ActivateVideoWindow.Camera.ToString();

                // FIXME: ActivateVideoWindow.Camera.Server is null because SetTrackerProperty() assigning Camera without Server
                if (CMS != null && ActivateVideoWindow.Camera.Server != null)
                {
                    _nameLabelText += @" (" + ActivateVideoWindow.Camera.Server + @" " +
                                          ActivateVideoWindow.Camera.Server.Credential.Domain + @")";
                }
                else if (PTS != null && ActivateVideoWindow.Camera.Server != null)
                {
                    //It's POS
                    if (FocusGroup != null)// && FocusGroup.Server == null
                        _nameLabelText += @" - " + " (" + ActivateVideoWindow.Camera.Server + @" " +
                                              ActivateVideoWindow.Camera.Server.Credential.Domain + @")";
                    //else
                    //nameLabel.Text += @" (" + ActivateVideoWindow.Camera.Server.Name + @" " +
                    //ActivateVideoWindow.Camera.Server.Credential.Domain + @")";
                }
                else if (FocusGroup != null && NVR != null && FocusGroup.Name != null)
                    _nameLabelText += @" (" + FocusGroup + @")";
            }
            else if (FocusGroup != null)
            {
                _nameLabelText = FocusGroup.ToString();
                if (CMS != null)
                {
                    if (FocusGroup.Server != null)
                        _nameLabelText += @" (" + FocusGroup.Server + @" " + FocusGroup.Server.Credential.Domain + @")";
                }
                else if (PTS != null)
                {
                    if (FocusGroup.Server != null)
                        _nameLabelText += @" (" + FocusGroup.Server.Name + @" " + FocusGroup.Server.Credential.Domain + @")";
                }
            }
            else
                _nameLabelText = "";

            nameLabel.Invalidate();
        }

        public void SetCurrentPage(UInt16 pageIndex)
        {
            PageIndex = pageIndex;
            SetCurrentPage();

            //換頁時也要更新撥放列表
            if (Server is ICMS)
            {
                if (OnViewingDeviceListChange != null)
                {
                    OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
                }
            }
        }

        protected virtual void SetCurrentPage()
        {
            if (PageIndex > PageCount)
                PageIndex = 1;
            else if (PageIndex < 1)
                PageIndex = PageCount;

            RefreshPageLabel();
            var startIndex = WindowLayout.Count * (PageIndex - 1);

            //show -> hide , kepp screen no flash.

            windowsPanel.Visible = false;
            Boolean activateWindowIsVisible = false;
            for (Int32 i = startIndex; i < startIndex + WindowLayout.Count; i++)
            {
                if (VideoWindows.Count > i)
                {
                    UpdateVideoWindowSize(VideoWindows[i]);
                    VideoWindows[i].Visible = true;
                    if (ActivateVideoWindow == VideoWindows[i])
                        activateWindowIsVisible = true;
                }
            }
            windowsPanel.Visible = true;

            for (Int32 i = 0; i < VideoWindows.Count; i++)
            {
                if (i < startIndex || i >= (startIndex + WindowLayout.Count))
                {
                    if (VideoWindows.Count > i)
                        VideoWindows[i].Visible = false;
                }
            }

            if (!activateWindowIsVisible && ActivateVideoWindow != null)
            {
                ActivateVideoWindow.Active = false;
            }

            //change stream id (if ness) before call ActivateVideoWindow.Activate
            AutoChangeProfileMode();

            //HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Play IS HERE!!!!!!!!!!
            foreach (IVideoWindow window in VideoWindows)
            {
                if (window.Visible)
                {
                    if (!activateWindowIsVisible)
                    {
                        //dont active no camera window
                        if (Devices.Count > 0 && window.Camera != null)
                        {
                            ActivateVideoWindow = window;
                            ActivateVideoWindow.Active = true;
                            activateWindowIsVisible = true;
                        }
                        //ToolMenu.VideoWindow = null;
                        ToolMenu.Visible = false;
                    }

                    if (window.Camera != null)
                        window.Activate();
                }
                else
                {
                    window.Stop();
                }
            }

            ChangeNameLabelText();

            ToolMenu.CheckStatus();
            ToolMenu.UpdateLocation();

            if (OnPageChange != null)
                OnPageChange(this, new EventArgs<UInt16, UInt16>(PageIndex, PageCount));

            if (OnPageChangeLayout != null)
                OnPageChangeLayout(this, new EventArgs<List<IVideoWindow>>(VideoWindows));
        }

        protected void RefreshPageLabel()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshPageLabel));

                return;
            }

            Int32 deviceCount = (Devices.Count > 0) ? (Devices.Aggregate(0, (current, obj) => Math.Max(current, obj.Key)) + 1) : 0;

            //when there have device always viaible = true
            //dont hide it, after hide and show, the dock order is chaos
            if (deviceCount > 0)
            {
                if (nextPageButton.BackgroundImage == null)
                {
                    SharedToolTips.SharedToolTip.SetToolTip(nextPageButton, Localization["VideoMonitor_Next"]);
                    SharedToolTips.SharedToolTip.SetToolTip(previousPageButton, Localization["VideoMonitor_Previous"]);
                    SharedToolTips.SharedToolTip.SetToolTip(popupPanel, Localization["VideoMonitor_Popup"]);

                    nextPageButton.BackgroundImage = _nextPage;
                    previousPageButton.BackgroundImage = _previousPage;
                    popupPanel.BackgroundImage = _popup;
                    nextPageButton.Enabled = previousPageButton.Enabled = popupPanel.Enabled = true;
                }
                pageLabel.Text = Localization["VideoMonitor_PageQuantity"].Replace("%1", PageIndex.ToString()).Replace("%2", PageCount.ToString());
            }
            else
            {
                if (nextPageButton.BackgroundImage != null)
                {
                    SharedToolTips.SharedToolTip.SetToolTip(previousPageButton, "");
                    SharedToolTips.SharedToolTip.SetToolTip(nextPageButton, "");
                    SharedToolTips.SharedToolTip.SetToolTip(popupPanel, "");

                    nextPageButton.BackgroundImage = previousPageButton.BackgroundImage = popupPanel.BackgroundImage = null;
                    nextPageButton.Enabled = previousPageButton.Enabled = popupPanel.Enabled = false;
                }
                pageLabel.Text = "";
            }
        }

        public void SetCurrentPage(Object sender, EventArgs<UInt16> e)
        {
            PageIndex = Convert.ToUInt16(e.Value);
            SetCurrentPage();

            //換頁時也要更新撥放列表
            if (Server is ICMS)
            {
                if (OnViewingDeviceListChange != null)
                {
                    OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
                }
            }
        }

        public void Play()
        {
            GetTimecodeTimer.Enabled = false;
            UpdateBitrateTimer.Enabled = true;

            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.Play();
            }
        }

        public virtual void ClearAll(Object sender, EventArgs e)
        {
            ClearAll();

            foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
            {
                popupVideoMonitor.VideoMonitor.ClearAll();
                popupVideoMonitor.Hide();
            }

            UsingPopupVideoMonitor.Clear();
        }

        public virtual void ClearAll()
        {
            FocusGroup = null;
            PreviousAppendDevice = null;
            IsCoolDown = true;
            CoolDownTimer.Enabled = false;
            _nameLabelText = "";
            nameLabel.Invalidate();

            if (Devices.Count > 0)
            {
                var layout = new List<WindowLayout>(WindowLayout.ToArray());
                Devices.Clear();
                windowsPanel.Visible = false;
                ClearLayout();
                SetLayout(layout);
                windowsPanel.Visible = true;
            }

            UpdateBitrateTimer.Enabled = false;

            if (OnBitrateUsageChange != null)
                OnBitrateUsageChange(this, new EventArgs<String>("N/A"));

            RaiseOnSelectionChange(new EventArgs<ICamera, PTZMode>(null, PTZMode.Disable));

            RaiseOnContentChange(null);

            if (OnViewingDeviceNumberChange != null)
                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(0));

            if (OnViewingDeviceListChange != null)
            {
                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(new List<IDevice>()));
            }
        }

        private delegate void ManualRecordDelegate();
        public void ManualRecord(Object sender, EventArgs e)
        {
            foreach (var videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible || !videoWindow.Viewer.Visible || videoWindow.Camera == null) continue;

                var doRecord = (videoWindow.PlayMode == PlayMode.LiveStreaming);

                if (doRecord)
                {
                    if (videoWindow.Camera is IDeviceLayout)
                    {
                        doRecord = true;
                    }
                    else if (videoWindow.Camera is ISubLayout)
                    {
                        doRecord = false;
                    }
                    else
                    {
                        doRecord = videoWindow.Camera.CheckPermission(Permission.ManualRecord);

                        if (doRecord && videoWindow.Camera.Status == CameraStatus.Nosignal)
                            doRecord = false;
                    }
                }

                if (!doRecord) continue;

                if (videoWindow.Camera is IDeviceLayout)
                {
                    var deviceLayout = videoWindow.Camera as IDeviceLayout;

                    foreach (var device in deviceLayout.Items)
                    {
                        var camera = device as ICamera;
                        if (camera == null) continue;

                        if (!camera.CheckPermission(Permission.ManualRecord)) continue;

                        camera.IsManualRecord = true;

                        //it call CGI to activate manual record, when server busy, CGI response will slow -> cause AP freeze.
                        ManualRecordDelegate manualRecordDelegate = camera.ManualRecord;
                        manualRecordDelegate.BeginInvoke(null, null);

                        Server.WriteOperationLog("Device %1 Start Manual Record".Replace("%1", camera.Id.ToString(CultureInfo.InvariantCulture)));
                    }
                }
                else
                {
                    videoWindow.Camera.IsManualRecord = true;

                    //it call CGI to activate manual record, when server busy, CGI response will slow -> cause AP freeze.
                    ManualRecordDelegate manualRecordDelegate = videoWindow.Camera.ManualRecord;
                    manualRecordDelegate.BeginInvoke(null, null);

                    Server.WriteOperationLog("Device %1 Start Manual Record".Replace("%1", videoWindow.Camera.Id.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }

        public void SaveImage(Object sender, EventArgs e)
        {
            foreach (var videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible || videoWindow.Camera == null) continue;

                videoWindow.SaveImage(Server.Configure.ImageWithTimestamp);
                Server.WriteOperationLog("Device %1 Save Image".Replace("%1", videoWindow.Camera.Id.ToString(CultureInfo.InvariantCulture)));
            }
        }

        public void Broadcast(Object sender, EventArgs e)
        {
            foreach (var videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible || videoWindow.Camera == null) continue;

                videoWindow.Camera.StartAudioTransfer();

                Server.WriteOperationLog("Device %1 Enable Speaker".Replace("%1", videoWindow.Camera.Id.ToString()));
            }
        }

        public void Snapshot(Object sender, EventArgs e)
        {
            if (windowsPanel.Width == 0 || windowsPanel.Height == 0) return;

            //snapshot all visible camera , and merge to ONE picture by current layout
            var imageCount = 0;
            var mergeSnapshot = new Bitmap(windowsPanel.Width, windowsPanel.Height);
            Graphics graph = Graphics.FromImage(mergeSnapshot);
            graph.FillRectangle(Brushes.Black, 0, 0, mergeSnapshot.Width, mergeSnapshot.Height);

            foreach (var videoWindow in VideoWindows)
            {
                if (videoWindow.Width == 0 || videoWindow.Height == 0) continue;
                if (!videoWindow.Visible || videoWindow.Camera == null) continue;
                try
                {
                    //remember call clear before getdata, else will get previous data if clipboard didn't update
                    Clipboard.Clear();
                    videoWindow.Snapshot(Server.Configure.ImageWithTimestamp);//use this config, watermark after image resize, has different font and is unreadable. also ugly.

                    IDataObject data = Clipboard.GetDataObject();
                    if (data == null || !data.GetDataPresent(DataFormats.Bitmap)) continue;

                    Image image = (Image)data.GetData(DataFormats.Bitmap, true);
                    if (image == null) continue;

                    imageCount++;

                    //stretch image
                    if (videoWindow.Stretch)
                    {
                        graph.DrawImage(image, videoWindow.Location.X, videoWindow.Location.Y, videoWindow.Width, videoWindow.Height);
                        continue;
                    }

                    //draw image in center
                    if (image.Width < videoWindow.Width && image.Height < videoWindow.Height)
                    {
                        var xspace = (videoWindow.Width - image.Width) / 2;
                        var yspace = (videoWindow.Height - image.Height) / 2;
                        graph.DrawImage(image, videoWindow.Location.X + xspace, videoWindow.Location.Y + yspace, image.Width, image.Height);
                        continue;
                    }

                    var widthpercent = image.Width / (videoWindow.Width * 1.0);
                    var heightpercent = image.Height / (videoWindow.Height * 1.0);

                    //resize the small side(width / height)
                    if (widthpercent == heightpercent)//nearly impossible, but maybe...result equal stretch = true
                    {
                        graph.DrawImage(image, videoWindow.Location.X, videoWindow.Location.Y, videoWindow.Width, videoWindow.Height);
                        continue;
                    }

                    //resize height
                    if (widthpercent > heightpercent)
                    {
                        var newheight = Convert.ToInt32(image.Height / widthpercent);
                        var yspace = (videoWindow.Height - newheight) / 2;
                        graph.DrawImage(image, videoWindow.Location.X, videoWindow.Location.Y + yspace, videoWindow.Width, newheight);
                    }
                    else//resize width
                    {
                        var newwidth = Convert.ToInt32(image.Width / heightpercent);
                        var xspace = (videoWindow.Width - newwidth) / 2;
                        graph.DrawImage(image, videoWindow.Location.X + xspace, videoWindow.Location.Y, newwidth, videoWindow.Height);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(@"Control occupy the thread, it may cause  要求的剪貼簿作業失敗。");
                }
            }

            if (imageCount > 0)
                Clipboard.SetImage(mergeSnapshot);
        }

        public Boolean DisplayTitleBar;
        public void VideoTitleBar()
        {
            DisplayTitleBar = !DisplayTitleBar;
            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.DisplayTitleBar = DisplayTitleBar;
            }

            RaiseOnTitleBarVisibleChanged(new EventArgs<bool>(DisplayTitleBar));
        }

        public void VideoTitleBar(Object sender, EventArgs e)
        {
            VideoTitleBar();
        }

        public void PopupWindowVideoTitleBar(Object sender, EventArgs e)
        {
            foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
            {
                popupVideoMonitor.DisplayTitleBar = !DisplayTitleBar;
            }
        }

        protected readonly System.Timers.Timer CoolDownTimer = new System.Timers.Timer();
        protected IDevice PreviousAppendDevice;

        public override string ToString()
        {
            var deviceCount = (Devices.Count > 0) ? (Devices.Aggregate(0, (current, obj) => Math.Max(current, obj.Key)) + 1) : 0;

            var ids = new String[deviceCount];

            foreach (KeyValuePair<Int32, IDevice> obj in Devices)
            {
                if (obj.Value == null) continue;
                ids[obj.Key] = obj.Value.Id.ToString(CultureInfo.InvariantCulture);
            }

            return String.Join(",", ids);
        }

        protected IDevice[] ToArray()
        {
            var deviceCount = (Devices.Count > 0) ? (Devices.Aggregate(0, (current, obj) => Math.Max(current, obj.Key)) + 1) : 0;

            if (deviceCount == 0) return new IDevice[] { };

            var devices = new IDevice[deviceCount];

            foreach (KeyValuePair<Int32, IDevice> obj in Devices)
            {
                if (obj.Value == null) continue;
                devices[obj.Key] = obj.Value;
            }

            return devices;
        }

        public void LayoutChange(Object sender, EventArgs<List<WindowLayout>> e)
        {
            if (e.Value != null)
                SetLayout(e.Value);

            if (OnPageChangeLayout != null)
                OnPageChangeLayout(this, new EventArgs<List<IVideoWindow>>(VideoWindows));
        }

        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            if (e.Value is IDevice)
                AppendDevice((IDevice)e.Value);
            else if (e.Value is IDeviceGroup)
                ShowGroup((IDeviceGroup)e.Value);
        }

        protected void PreviousPageButtonMouseClick(Object sender, MouseEventArgs e)
        {
            PageIndex--;
            SetCurrentPage();
        }

        protected void NextPageButtonMouseClick(Object sender, MouseEventArgs e)
        {
            PageIndex++;
            SetCurrentPage();
        }

        private void PopupPanelMouseClick(Object sender, MouseEventArgs e)
        {
            IDeviceGroup deviceGroup = new DeviceGroup { Layout = new List<WindowLayout>(WindowLayout) };

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible) continue;

                deviceGroup.View.Add(videoWindow.Camera);

                if (videoWindow.Camera != null && !deviceGroup.Items.Contains(videoWindow.Camera))
                    deviceGroup.Items.Add(videoWindow.Camera);
            }

            var videoMonitor = PopupVideoMonitor(deviceGroup);
            var count = 0;
            if (OnViewingDeviceNumberChange != null)
            {
                var deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                count = Math.Min(layoutCount, deviceCount);

                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
            }

            if (OnViewingDeviceListChange != null)
            {
                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
            }

            if (videoMonitor != null)
            {
                videoMonitor.Show();
                videoMonitor.BringToFront();
                ReadPopupWindowsAndSave();

                if (Server.Configure.EnableAutoSwitchDecodeIFrame)
                {
                    //When Layout.Count >= 16 , Auto Switch to Decode-I Frame
                    if (count >= Server.Configure.AutoSwitchDecodeIFrameCount)
                        DecodeIframe();
                    else //When Layout.Count < 16 , Auto Switch to autoDrop Frame
                        AutoDropFrame();
                }
                else
                {
                    if (DecodeIframeFlag)
                    {
                        DecodeIframe();
                    }
                    else
                    {
                        AutoDropFrame();
                    }
                }
            }

            AutoChangeProfileMode();
        }

        public List<IDevice> ReadViewDeviceList()
        {
            //var listDevices = UsingPopupVideoMonitor.SelectMany(popupVideoMonitor => popupVideoMonitor.DeviceGroup.Items).ToList();
            var listDevices = new List<IDevice>();
            foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
            {
                foreach (IDevice device in popupVideoMonitor.DeviceGroup.Items)
                {
                    //if(listDevices.Contains(device)) continue;
                    listDevices.Add(device);
                }
            }

            //check real devices count on layout
            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible) continue;
                if (videoWindow.Viewer.Camera == null) continue;
                listDevices.Add(videoWindow.Viewer.Camera);
            }

            return listDevices;
        }

        public void ReadPopupWindowsAndSave()
        {
            if (App.StartupOption == null)
                return;

            App.StartupOption.PopupWindows.Clear();
            foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
            {
                if (popupVideoMonitor == null) continue;
                if (popupVideoMonitor.DeviceGroup == null) continue;

                var newOptionsPopupWindow = new StartupOptionDeviceGroup
                {
                    Layout = WindowLayouts.LayoutToString(popupVideoMonitor.DeviceGroup.Layout),
                    Items = DeviceConverter.DeviceViewToString(new List<IDevice>(popupVideoMonitor.ToArray()))
                };
                App.StartupOption.PopupWindows.Add(newOptionsPopupWindow);
            }
            App.StartupOption.SaveSetting();
        }

        public void GlobalMouseHandler()
        {
            if (ActivateVideoWindow != null && ActivateVideoWindow.Visible)
                ActivateVideoWindow.GlobalMouseHandler();
        }

        private Boolean _isActivate;
        private Boolean _isInitial;
        private StartupOptions _tempStartOptions;
        public virtual void Activate()
        {
            if (!_isInitial)//for start option(after bind event)
            {
                if (Server.Configure.StartupOptions.VideoTitleBar)
                    VideoTitleBar();

                _tempStartOptions = new StartupOptions();
                if (App.StartupOption != null)
                {
                    foreach (StartupOptionDeviceGroup startupOptionDeviceGroup in App.StartupOption.PopupWindows)
                    {
                        _tempStartOptions.PopupWindows.Add(startupOptionDeviceGroup);
                    }
                }
            }

            _isInitial = true;
            _isActivate = true;
            ToolMenu.CheckPermission();

            App.OnAppStarted -= AppOnAppStarted;
            App.OnAppStarted += AppOnAppStarted;

            //trigger device number change for custom bitrate control
            if (OnViewingDeviceNumberChange != null)
            {
                var deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);

                if (Server.Configure.EnableAutoSwitchDecodeIFrame)
                {
                    //When Layout.Count >= 16 , Auto Switch to Decode-I Frame
                    if (count >= Server.Configure.AutoSwitchDecodeIFrameCount)
                        DecodeIframe();
                    else //When Layout.Count < 16 , Auto Switch to autoDrop Frame
                        AutoDropFrame();
                }

                AutoChangeProfileMode();

                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
            }

            if (OnViewingDeviceListChange != null)
            {
                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
            }

            if (ActivateVideoWindow != null)
                ActivateVideoWindow.Active = true;

            foreach (IVideoWindow videoWindow in VideoWindows)
                videoWindow.Activate();

            if (WindowLayout != null)
            {
                var deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);

                if (count > 0)
                {
                    UpdateBitrateTimer.Enabled = true;
                }
                else
                {
                    UpdateBitrateTimer.Enabled = false;

                    if (OnBitrateUsageChange != null)
                        OnBitrateUsageChange(this, new EventArgs<String>("N/A"));
                }
            }

            Boolean reloadStretch = false;
            switch (PlayMode)
            {
                case PlayMode.LiveStreaming:
                    if (_stretchStatus != Server.Configure.StretchLiveVideo)
                    {
                        _stretchStatus = Server.Configure.StretchLiveVideo;
                        reloadStretch = true;
                    }
                    break;

                case PlayMode.GotoTimestamp:
                    if (_stretchStatus != Server.Configure.StretchPlaybackVideo)
                    {
                        _stretchStatus = Server.Configure.StretchPlaybackVideo;
                        reloadStretch = true;
                    }
                    break;
            }

            if (reloadStretch)
            {
                foreach (IVideoWindow videoWindow in VideoWindows)
                    videoWindow.Stretch = (videoWindow.PlayMode == PlayMode.LiveStreaming)
                                              ? Server.Configure.StretchLiveVideo
                                              : Server.Configure.StretchPlaybackVideo;
            }

            ChangeNameLabelText();

            // Val: PTZ Handler Move to Activate
            if (_server.Configure.EnableJoystick)
            {
                _server.Utility.OnClickButton += UtilityOnClickButton;
                _server.Utility.OnMoveAxis += UtilityOnMoveAxis;
            }
            else if (_server.Configure.EnableAxisJoystick)
            {
                _server.Utility.OnAxisJoystickButtonDown += UtilityOnAxisClickButton;
                _server.Utility.OnAxisJoystickRotate += UtilityOnAxisMoveAxis;

                _server.Utility.OnAxisJogDialButtonDown += UtilityOnAxisJogDialClickButton;

                _server.Utility.OnAxisKeyPadButtonDown += UtilityOnAxisKeypadClickButton;
            }
        }

        public void CheckMenuPermission()
        {
            ToolMenu.CheckPermission();
        }

        private void GroupMenuItemClick(Object sender, EventArgs e)
        {
            var groupMenuItem = sender as MenuItem;

            if (groupMenuItem == null) return;
            if (groupMenuItem.Tag == null) return;

            ShowGroup(groupMenuItem.Tag as IDeviceGroup);
        }

        public virtual void Deactivate()
        {
            App.OnAppStarted -= AppOnAppStarted;

            _isActivate = false;
            foreach (IVideoWindow videoWindow in VideoWindows)
                videoWindow.Deactivate();

            UpdateBitrateTimer.Enabled = false;
            StopMultiScreenPatrol();

            if (OnBitrateUsageChange != null)
                OnBitrateUsageChange(this, new EventArgs<String>("N/A"));

            // PTZ Handler Move to Deactivate
            _server.Utility.OnClickButton -= UtilityOnClickButton;
            _server.Utility.OnMoveAxis -= UtilityOnMoveAxis;

            _server.Utility.OnAxisJoystickButtonDown -= UtilityOnAxisClickButton;
            _server.Utility.OnAxisJoystickRotate -= UtilityOnAxisMoveAxis;

            _server.Utility.OnAxisJogDialButtonDown -= UtilityOnAxisJogDialClickButton;

            _server.Utility.OnAxisKeyPadButtonDown -= UtilityOnAxisKeypadClickButton;
        }

        protected void WindowsPanelSizeChanged(Object sender, EventArgs e)
        {
            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Visible)
                    UpdateVideoWindowSize(videoWindow);
            }
            ToolMenu.UpdateLocation();
        }

        protected void UpdateVideoWindowSize(IVideoWindow videoWindow)
        {
            if (videoWindow.WindowLayout == null) return;

            videoWindow.Location = new Point(Convert.ToInt32(windowsPanel.Size.Width * videoWindow.WindowLayout.X / 100),
                                             Convert.ToInt32(windowsPanel.Size.Height * videoWindow.WindowLayout.Y / 100));

            videoWindow.Size = new Size(Convert.ToInt32(windowsPanel.Size.Width * videoWindow.WindowLayout.Width / 100),
                                        Convert.ToInt32(windowsPanel.Size.Height * videoWindow.WindowLayout.Height / 100));
        }

        protected virtual void DisconnectWindow(IVideoWindow videoWindow)
        {
            Devices.Remove(VideoWindows.IndexOf(videoWindow));

            RefreshPageLabel();

            videoWindow.PlayMode = PlayMode;
            videoWindow.Reset();
            videoWindow.PlayMode = PlayMode;
            videoWindow.Active = false;
            FocusGroup = null;
            _nameLabelText = "";
            nameLabel.Invalidate();
        }

        public void ServerTimeZoneChange(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, EventArgs>(ServerTimeZoneChange), sender, e);
                return;
            }

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                videoWindow.ServerTimeZoneChange();
            }
        }

        private void AppOnCustomVideoStream(Object sender, EventArgs e)
        {
            if (!_isActivate) return;

            foreach (var videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible) continue;
                if (videoWindow.Camera is IDeviceLayout || videoWindow.Camera is ISubLayout) continue;

                videoWindow.Reconnect();
            }

            foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
            {
                popupVideoMonitor.Reconnect();
            }
        }

        private bool _hotSpotLoading = false;
        protected void AppOnHotSpotEvent(Object sender, EventArgs<ICamera> e)
        {
            if (_hotSpotLoading) return;

            var device = e.Value;
            var firstWindow = VideoWindows.FirstOrDefault();

            if (firstWindow == null) return;

            _hotSpotLoading = true;

            //check if first window's device is already the same device
            if (firstWindow.Camera == device && firstWindow.PlayMode == PlayMode.LiveStreaming)
            {
                //same device
                IdleHotSpotTimer();
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, EventArgs<ICamera>>(AppOnHotSpotEvent), sender, e);
                return;
            }

            if (ActivateVideoWindow == firstWindow)
            {
                ActivateVideoWindow = null;
                firstWindow.Active = false;
            }

            firstWindow.Reset();
            firstWindow.PlayMode = PlayMode.LiveStreaming;
            firstWindow.Camera = device;

            var appendId = VideoWindows.IndexOf(firstWindow);

            if (!Devices.ContainsKey(appendId))
            {
                Devices.Add(appendId, device);
            }
            else
                Devices[appendId] = device;

            AutoChangeProfileMode();

            firstWindow.Play();

            IdleHotSpotTimer();
        }


        private void IdleHotSpotTimer()
        {
            var timer = new Timer { Interval = 10000 };//2000
            timer.Tick += (s, e1) =>
            {
                _hotSpotLoading = false;
                timer.Enabled = false;
                timer = null;
            };

            timer.Enabled = true;
        }

        private void AutoChangeProfileMode()
        {
            if (IsPopup) return;

            if (Server.Configure.EnableAutoSwitchLiveStream)
            {
                var deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);

                var mode = "";
                if (count < Server.Configure.AutoSwitchLiveHighProfileCount)
                    mode = "HIGH";
                else if (count >= Server.Configure.AutoSwitchLiveHighProfileCount && count <= Server.Configure.AutoSwitchLiveLowProfileCount)
                    mode = "MEDIUM";
                else
                    mode = "LOW";

                ChangeProfileMode(mode);

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    popupVideoMonitor.ChangeProfileMode(mode);
                }
            }
        }

        public void HidePageLabel()
        {
            toolPanel.Controls.Remove(pageLabel);
            toolPanel.Controls.Remove(previousPageButton);
            toolPanel.Controls.Remove(nextPageButton);
            toolPanel.Controls.Remove(popupPanel);
        }

        public void HideToolPanel()
        {
            toolPanel.Height = 0;
            Controls.Remove(toolPanel);

            ToolMenu.PanelPoint = new Point(0, 0);
        }

        public void SetSubLayoutRegion(ISubLayout subLayout)
        {
            foreach (var videoWindow in VideoWindows)
            {
                if (!(videoWindow.Camera is IDeviceLayout)) continue;

                videoWindow.Viewer.SetSubLayoutRegion(subLayout);
            }
        }

        public void UpdateSubLayoutRegion(ISubLayout subLayout)
        {
            foreach (var videoWindow in VideoWindows)
            {
                if (!(videoWindow.Camera is IDeviceLayout)) continue;

                videoWindow.Viewer.UpdateSubLayoutRegion(subLayout);
            }
        }

        public IDeviceGroup GetGroupLayoutInfo(IDeviceGroup group)
        {
            var items = new List<IDevice>();
            var views = new List<IDevice>();

            foreach (var videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible) continue;
                views.Add(videoWindow.Camera);

                if (videoWindow.Camera != null && !items.Contains(videoWindow.Camera))
                    items.Add(videoWindow.Camera);
            }

            items.Sort((x, y) => (x.Id - y.Id));

            group.Items = items;
            group.View = views;
            group.Layout = new List<WindowLayout>(WindowLayout);

            return group;
        }

        public void UpdateClientSetting()
        {
            var tmpGroup = new DeviceGroup { Server = Server };

            GetGroupLayoutInfo(tmpGroup);

            App.UpdateClientSetting(RestoreClientColumn.Layout, WindowLayouts.LayoutToString(tmpGroup.Layout), null);
            App.UpdateClientSetting(RestoreClientColumn.Items, DeviceConverter.DeviceViewToString(tmpGroup.View), null);
        }

        private readonly Font _nameLabelFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        protected String _nameLabelText;
        private readonly Brush _nameLabelBrush = new SolidBrush(Color.FromArgb(214, 214, 214));

        private void NameLabelPaint(Object sender, PaintEventArgs e)
        {
            if (String.IsNullOrEmpty(_nameLabelText)) return;
            var g = e.Graphics;
            var maxWidth = nameLabel.Width - 16; //left,right space 8
            if (maxWidth <= 0) return;

            var text = _nameLabelText;

            SizeF fSize = g.MeasureString(_nameLabelText, _nameLabelFont);
            //trim text if too long
            while (fSize.Width > maxWidth)
            {
                if (text.Length <= 1) break;

                text = text.Substring(0, text.Length - 1);
                fSize = g.MeasureString(text, _nameLabelFont);
            }
            if (text != _nameLabelText)
            {
                text += "...";
                fSize = g.MeasureString(text, _nameLabelFont);
            }

            g.DrawString(text, _nameLabelFont, _nameLabelBrush, Convert.ToInt32((nameLabel.Width - fSize.Width) / 2), 7);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        public void PopupInstantPlayback(IDevice device, UInt64 timecode)
        {
            var instantPlayback = GetInstantPlayback();

            instantPlayback.Reset();

            instantPlayback.Camera = (ICamera)device;
            instantPlayback.DateTime = DateTimes.ToDateTime(Convert.ToUInt64(timecode), Server.Server.TimeZone);

            instantPlayback.Show();
            instantPlayback.BringToFront();
        }

        public virtual void PopupInstantPlayback(IDevice device, ulong timecode, object info)
        {

        }

        public static Queue<InstantPlayback> RecycleInstantPlayback = new Queue<InstantPlayback>();

        public static List<InstantPlayback> UsingInstantPlayback = new List<InstantPlayback>();

        public virtual InstantPlayback GetInstantPlayback()
        {
            InstantPlayback instantPlayback;
            if (RecycleInstantPlayback.Count > 0)
            {
                instantPlayback = RecycleInstantPlayback.Dequeue();
            }
            else
            {
                instantPlayback = CreateInstantPlayback();
            }

            //add to using
            if (!UsingInstantPlayback.Contains(instantPlayback))
                UsingInstantPlayback.Add(instantPlayback);

            return instantPlayback;
        }

        protected virtual InstantPlayback CreateInstantPlayback()
        {
            return new InstantPlayback
            {
                App = App,
                Server = Server,
                Icon = Server.Form.Icon,
            };
        }

        //---------------------------------------------------------------------------------------------------------------------------------
        public void PopupLiveStream(IDevice device)
        {
            //Popup Window
            var liveStream = GetPopupLive();

            //add to using
            if (!UsingPopupLive.Contains(liveStream))
                UsingPopupLive.Add(liveStream);

            liveStream.Reset();

            liveStream.Camera = (ICamera)device;

            liveStream.Show();
            liveStream.BringToFront();
        }

        public static Queue<PopupLiveStream> RecyclePopupLive = new Queue<PopupLiveStream>();
        public static List<PopupLiveStream> UsingPopupLive = new List<PopupLiveStream>();
        public PopupLiveStream GetPopupLive()
        {
            if (RecyclePopupLive.Count > 0)
            {
                return RecyclePopupLive.Dequeue();
            }

            return CreatePopupLiveStream();
        }

        protected virtual PopupLiveStream CreatePopupLiveStream()
        {
            return new PopupLiveStream
            {
                App = App,
                Server = Server,
                Icon = Server.Form.Icon,
            };
        }

        //---------------------------------------------------------------------------------------------------------------------------------
        private const UInt16 MaximumPopupVideoMonitor = 4;
        public PopupVideoMonitor PopupVideoMonitor(IDeviceGroup deviceGroup)
        {
            if (GetVideoMonitorCount >= MaximumPopupVideoMonitor)
            {
                TopMostMessageBox.Show(Localization["VideoMonitor_MaximumPopupVideoMonitor"].Replace("%1", MaximumPopupVideoMonitor.ToString()),
                        Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            //Popup Window
            var videoMonitor = GetPopupVideoMonitor();
            videoMonitor.EnableCloseButton();

            videoMonitor.DisplayTitleBar = !DisplayTitleBar;

            //add to using
            if (!UsingPopupVideoMonitor.Contains(videoMonitor))
                UsingPopupVideoMonitor.Add(videoMonitor);

            videoMonitor.Reset();

            videoMonitor.DeviceGroup = deviceGroup;

            Screen availableScreen = null;
            foreach (var screen in Screen.AllScreens)
            {
                var bounds = screen.Bounds;

                Boolean screenIsUsing = false;
                //check main form
                var screenCenter = new Point(Server.Form.Bounds.X + (Server.Form.Size.Width / 2),
                    Server.Form.Bounds.Y + (Server.Form.Size.Height / 2));
                if (bounds.X <= screenCenter.X && (bounds.X + bounds.Width) >= screenCenter.X)
                {
                    if (bounds.Y <= screenCenter.Y && (bounds.Y + bounds.Height) >= screenCenter.Y)
                    {
                        screenIsUsing = true;
                    }
                }
                if (screenIsUsing) continue;

                //check other popup video monitor

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    if (popupVideoMonitor == videoMonitor) continue;
                    //if (!popupVideoMonitor.Visible) continue;

                    screenCenter = new Point(popupVideoMonitor.Bounds.X + (popupVideoMonitor.Size.Width / 2),
                    popupVideoMonitor.Bounds.Y + (popupVideoMonitor.Size.Height / 2));
                    if (bounds.X <= screenCenter.X && (bounds.X + bounds.Width) >= screenCenter.X)
                    {
                        if (bounds.Y <= screenCenter.Y && (bounds.Y + bounds.Height) >= screenCenter.Y)
                        {
                            screenIsUsing = true;
                        }
                    }
                    if (screenIsUsing) break;
                }

                if (screenIsUsing) continue;

                availableScreen = screen;
                break;
            }

            if (availableScreen != null)
            {
                videoMonitor.Location = new Point(availableScreen.Bounds.X +
                                        (Math.Max(0, availableScreen.Bounds.Width - videoMonitor.Bounds.Width) / 2),
                                        availableScreen.Bounds.Y +
                                        (Math.Max(0, availableScreen.Bounds.Height - videoMonitor.Bounds.Height) / 2));
            }

            return videoMonitor;
        }

        public static Queue<PopupVideoMonitor> RecyclePopupVideoMonitor = new Queue<PopupVideoMonitor>();
        public static List<PopupVideoMonitor> UsingPopupVideoMonitor = new List<PopupVideoMonitor>();
        public PopupVideoMonitor GetPopupVideoMonitor()
        {
            if (RecyclePopupVideoMonitor.Count > 0)
            {
                return RecyclePopupVideoMonitor.Dequeue();
            }
            var popupVideoMonitor = CreatePopupVideoMonitor();
            popupVideoMonitor.FormClosing += PopupVideoMonitorFormClosing;
            popupVideoMonitor.OnDiscountVideoWindowByMenu += PopupVideoMonitorOnDiscountVideoWindowByMenu;
            return popupVideoMonitor;
        }

        protected virtual PopupVideoMonitor CreatePopupVideoMonitor()
        {
            return new PopupVideoMonitor
            {
                App = App,
                Server = Server,
                Icon = Server.Form.Icon,
            };
        }

        private void PopupVideoMonitorOnDiscountVideoWindowByMenu(Object sender, EventArgs e)
        {
            if (OnViewingDeviceNumberChange != null)
            {
                var deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);

                if (Server.Configure.EnableAutoSwitchDecodeIFrame)
                {
                    //When Layout.Count >= 16 , Auto Switch to Decode-I Frame
                    if (count >= Server.Configure.AutoSwitchDecodeIFrameCount)
                        DecodeIframe();
                    else //When Layout.Count < 16 , Auto Switch to autoDrop Frame
                        AutoDropFrame();
                }
                else
                {
                    if (DecodeIframeFlag)
                    {
                        DecodeIframe();
                    }
                    else
                    {
                        AutoDropFrame();
                    }
                }

                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
            }

            if (OnViewingDeviceListChange != null)
            {
                var listDevices = new List<IDevice>();
                foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    foreach (IDevice device in popupVideoMonitor.DeviceListForDisconnectByMenu)
                    {
                        //if(listDevices.Contains(device)) continue;
                        listDevices.Add(device);
                    }
                }

                if (Devices != null)
                    foreach (IDevice device in Devices.Values)
                    {
                        listDevices.Add(device);
                    }

                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(listDevices));
            }

            AutoChangeProfileMode();
        }

        private void PopupVideoMonitorFormClosing(Object sender, FormClosingEventArgs e)
        {
            var videoMonitor = (PopupVideoMonitor)sender;
            //remove from using);
            if (UsingPopupVideoMonitor.Contains(videoMonitor))
                UsingPopupVideoMonitor.Remove(videoMonitor);

            //recycle
            if (!RecyclePopupVideoMonitor.Contains(videoMonitor))
                RecyclePopupVideoMonitor.Enqueue(videoMonitor);

            if (OnViewingDeviceNumberChange != null)
            {
                var deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);
                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));

                if (Server.Configure.EnableAutoSwitchDecodeIFrame)
                {
                    //When Layout.Count >= 16 , Auto Switch to Decode-I Frame
                    if (count >= Server.Configure.AutoSwitchDecodeIFrameCount)
                        DecodeIframe();
                    else //When Layout.Count < 16 , Auto Switch to autoDrop Frame
                        AutoDropFrame();
                }
                else
                {
                    if (DecodeIframeFlag)
                    {
                        DecodeIframe();
                    }
                    else
                    {
                        AutoDropFrame();
                    }
                }
            }

            if (OnViewingDeviceListChange != null)
            {
                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
            }

            AutoChangeProfileMode();
            ReadPopupWindowsAndSave();
        }

        //---------------------------------------------------------------------------------------------------------------------------------
        public UInt16 GetLiveStreamCount
        {
            get
            {
                return Convert.ToUInt16(UsingPopupLive.Count);
            }
        }

        public UInt16 GetInstantPlaybackCount
        {
            get
            {
                return Convert.ToUInt16(UsingInstantPlayback.Count);
            }
        }

        public UInt16 GetVideoMonitorCount
        {
            get
            {
                return Convert.ToUInt16(UsingPopupVideoMonitor.Count);
            }
        }

    }
}
