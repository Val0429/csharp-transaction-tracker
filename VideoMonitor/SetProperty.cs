using System;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Device;
using DeviceConstant;
using ServerProfile;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        protected Boolean _stretchStatus;// = false;
        private MenuItem _exitFullScreenMenuItem;
        public virtual void SetLiveProperty()
        {
            //only live need hotspot
            App.OnHotSpotEvent += AppOnHotSpotEvent;
            App.OnSwitchPage += AppendLiveDevice;

            _stretchStatus = Server.Configure.StretchLiveVideo;
            ToolMenu.GenerateLiveToolMenu();

            PagerControl.SetLiveProperty();
            pagerPanel.Width = 20 + PagerControl.ButtonWidth * PagerControl.MiniButtonCount;

            _patrolDeviceTimer.Elapsed += DeviceMultiScreenPatrol;
            _patrolDeviceTimer.SynchronizingObject = Server.Form;

            _exitFullScreenMenuItem = new MenuItem(Localization["VideoMonitor_ExitFullScreen"]);
            _exitFullScreenMenuItem.Click += ExitFullScreenMenuItemClick;
        }

        public virtual void SetPlaybackProperty()
        {
            toolPanel.Controls.Remove(popupPanel);

            _stretchStatus = Server.Configure.StretchPlaybackVideo;
            //MaxConnection = 16;
            if (Server is NVR)
            {
                MaxConnection = Server.Server.CheckProductNoToSupportNumber("playbackChannel");
            }
            else
            {
                MaxConnection = 16;
            }

            AllowDuplicate = false;
            PlayMode = PlayMode.GotoTimestamp;

            ToolMenu.GeneratePlaybackToolMenu();

            PagerControl.SetPlaybackProperty();
            pagerPanel.Width = 20 + PagerControl.ButtonWidth * PagerControl.MiniButtonCount;

            _exitFullScreenMenuItem = new MenuItem(Localization["VideoMonitor_ExitFullScreen"]);
            _exitFullScreenMenuItem.Click += ExitFullScreenMenuItemClick;

            App.OnSwitchPage += AppendPlaybackDevice;

            App.PlaybackCount = WindowLayout.Count;
        }

        public void SetEditProperty()
        {
            toolPanel.Controls.Remove(popupPanel);

            App.OnAppStarted -= AppOnAppStarted;
            PlayMode = PlayMode.Snapshot;

            if (NVR != null)
            {
                NVR.OnDeviceModify -= DeviceModify;
                NVR.OnGroupModify -= GroupModify;
                NVR.OnDeviceLayoutModify -= DeviceLayoutModify;
                NVR.OnSubLayoutModify -= SubLayoutModify;

                NVR.OnCameraStatusReceive -= EventReceive;
            }

            if (CMS != null)
            {
                CMS.OnNVRModify -= NVRModify;
            }

            Controls.Remove(toolPanel); //page, pager, name label ..etc

            if (ToolMenu == null) return;

            Controls.Remove((Control)ToolMenu);
        }

        public void SetDeviceLayoutEditProperty()
        {
            toolPanel.Controls.Remove(popupPanel);

            App.OnAppStarted -= AppOnAppStarted;
            MaxConnection = 1;
            _stretchStatus = Server.Configure.StretchLiveVideo;

            NVR.OnDeviceLayoutModify -= DeviceLayoutModify;
            NVR.OnSubLayoutModify -= SubLayoutModify;

            HidePageLabel();

            Controls.Remove(toolPanel); //page, pager, name label ..etc

            if (ToolMenu == null) return;

            Controls.Remove((Control)ToolMenu);
        }

        private Boolean _isPagerExpand;
        private void PagerPanelButtonMouseClick(Object sender, MouseEventArgs e)
        {
            _isPagerExpand = !_isPagerExpand;

            if (_isPagerExpand)
            {
                pagerPanelButton.BackgroundImage = _right;
                pagerPanel.Width = 20 + PagerControl.ButtonWidth * PagerControl.TotalButtonCount;//space + width X button count
            }
            else
            {
                pagerPanelButton.BackgroundImage = _left;
                pagerPanel.Width = 20 + PagerControl.ButtonWidth * PagerControl.MiniButtonCount;//space + width X button count
            }
        }

        private void ExitFullScreenMenuItemClick(Object sender, EventArgs e)
        {
            if (OnExitFullScreen != null)
                OnExitFullScreen(this, null);
        }

        protected virtual void AppOnAppStarted(Object sender, EventArgs e)
        {
            App.OnAppStarted -= AppOnAppStarted;

            //restore crash view
            if (App.StartupOption.Items != "")
            {
                var temp = App.StartupOption.Clone();
                var innerText = App.StartupOption.Items;
                var items = innerText.Split(',');
                var viewId = items.ToList();

                var list = DeviceConverter.StringToDeviceList(Server, viewId);
                var view = DeviceConverter.StringToDeviceView(Server, viewId, list);

                var windowLayout = DeviceConverter.StringToLayout(App.StartupOption.Layout);

                var tmpGroup = new DeviceGroup { Items = list, View = view, Layout = windowLayout, Server = Server };

                ShowGroup(tmpGroup);
                AppOnAppStartedPopupWindow(_tempStartOptions);
                return;
            }

            //use startup options's view(in live mode only)
            if (PlayMode == PlayMode.LiveStreaming && NVR.Configure.StartupOptions.DeviceGroup != "" && NVR.Configure.StartupOptions.DeviceGroup != "-1")
            {
                var id = Convert.ToUInt16(NVR.Configure.StartupOptions.DeviceGroup);
                if (!NVR.Device.Groups.ContainsKey(id)) return;
                var group = NVR.Device.Groups[id];
                if (group.Items.Count == 0) return; //dont append no device view on startup

                ShowGroup(group);
            }
        }

        private void AppOnAppStartedPopupWindow(StartupOptions startupOptions)
        {
            if (startupOptions.PopupWindows.Count == 0) return;

            foreach (StartupOptionDeviceGroup startupOptionDeviceGroup in startupOptions.PopupWindows)
            {
                var innerText = startupOptionDeviceGroup.Items;
                var items = innerText.Split(',');
                var viewId = items.ToList();

                var list = DeviceConverter.StringToDeviceList(Server, viewId);
                var view = DeviceConverter.StringToDeviceView(Server, viewId, list);

                var windowLayout = DeviceConverter.StringToLayout(startupOptionDeviceGroup.Layout);

                var tmpGroup = new DeviceGroup { Items = list, View = view, Layout = windowLayout, Server = Server };

                var videoMonitor = PopupVideoMonitor(tmpGroup);

                if (videoMonitor != null)
                {
                    videoMonitor.Show();
                    videoMonitor.BringToFront();
                }

                AutoChangeProfileMode();
            }

        }

        public virtual void SetCameraLastImageOn()
        {
            Server.Configure.CameraLastImage = true;
        }

        public virtual void SetFullScreenBestResolutionOn()
        {
            Server.Configure.FullScreenBestResolution = true;
        }

        public virtual void EnableUserDefine()
        {
            Server.Configure.EnableUserDefine = true;
        }
    }
}

