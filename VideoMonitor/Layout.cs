using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public List<WindowLayout> WindowLayout;

        //protected UInt16 MaximumAutoDropFrame = 16;
        public virtual void InitializeLayout()
        {
            var layout = WindowLayouts.LayoutGenerate(4);
            FocusGroup = null;
            _nameLabelText = "";
            nameLabel.Invalidate();
            SetLayout(layout);
        }

        protected virtual void UnregisterVideoWindow()
        {
            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.OnSelectionChange -= VideoWindowOnSelectionChanged;
                videoWindow.OnWindowMouseDrag -= VideoWindowOnWindowMouseDrag;
                videoWindow.OnMouseDown -= VideoWindowOnMouseDown;
                videoWindow.OnFullScreen -= videoWindow_OnFullScreen;
                videoWindow.OnCloseFullScreen -= videoWindow_OnCloseFullScreen;

                videoWindow.PlayMode = PlayMode;
                VideoWindowProvider.UnregisterVideoWindow((VideoWindow)videoWindow);
            }
        }

        protected virtual void ClearLayout()
        {
            UnregisterVideoWindow();

            windowsPanel.Controls.Clear();
            VideoWindows.Clear();
            WindowLayout.Clear();
            WindowLayout = null;
        }

        public virtual void SetLayout(List<WindowLayout> layout)
        {
            var devicescount = Devices.Count(device => device.Value != null);

            if (PlayMode == PlayMode.Snapshot && FocusGroup != null && devicescount == FocusGroup.Items.Count)
                FocusGroup.Layout = new List<WindowLayout>(layout.ToArray());

            if (PlayMode == PlayMode.GotoTimestamp)
            {
                App.PlaybackCount = layout.Count;
            }

            SetVideoWindow(layout);

            WindowLayout = new List<WindowLayout>(layout.ToArray());

            Int32 windowsCount = Math.Max(layout.Count, 1);
            Int32 deviceCount = Devices.Count > 0 ? (Devices.Aggregate(0, (current, obj) => Math.Max(current, obj.Key)) + 1) : 0;

            PageCount = Convert.ToUInt16(Math.Ceiling(deviceCount / (Double)windowsCount));
            if (PageCount == 0)
                PageCount = 1;

            PageIndex = 1;

            if (ActivateVideoWindow != null && ActivateVideoWindow.Visible)
                PageIndex = Convert.ToUInt16(Math.Ceiling((VideoWindows.IndexOf(ActivateVideoWindow) + 1) / (Double)WindowLayout.Count));

            SetCurrentPage();

            deviceCount = (Devices == null) ? 0 : Devices.Count;
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
            //keep current decode setting
            //else
            //{
            //    AutoDropFrame();
            //}

            if (count > 0)
                UpdateBitrateTimer.Enabled = true;
            else
            {
                UpdateBitrateTimer.Enabled = false;

                if (OnBitrateUsageChange != null)
                    OnBitrateUsageChange(this, new EventArgs<String>("N/A"));
            }

            if (OnViewingDeviceNumberChange != null)
            {
                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
            }

            if (OnViewingDeviceListChange != null)
            {
                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
            }

            if (OnLayoutChange != null)
                OnLayoutChange(this, new EventArgs<List<WindowLayout>>(layout));
        }

        protected void SetVideoWindow(List<WindowLayout> layout)
        {
            if (layout.Count == 0)
                return;

            Int32 deviceCount = (Devices.Count > 0) ? (Devices.Aggregate(0, (current, obj) => Math.Max(current, obj.Key)) + 1) : 0;

            Int32 num = Math.Max(layout.Count, deviceCount);

            num = Convert.ToInt32(Math.Ceiling((Double)num / layout.Count) * layout.Count);

            //if layout is different, change video window value
            if (!WindowLayouts.LayoutCompare(WindowLayout, layout))
            {
                while (VideoWindows.Count < num)
                {
                    var videoWindow = RegisterVideoWindow();

                    var viewerSize = videoWindow.Viewer.Size;

                    windowsPanel.Controls.Add(((Control)videoWindow));

                    if (viewerSize.Width != videoWindow.Viewer.Size.Width || viewerSize.Height != videoWindow.Viewer.Size.Height)
                    {
                        videoWindow.Viewer.Size = viewerSize;
                    }

                    VideoWindows.Add(videoWindow);
                }
            }

            foreach (var videoWindow in VideoWindows)
            {
                IDevice device = Devices.ContainsKey(VideoWindows.IndexOf(videoWindow))
                                     ? Devices[VideoWindows.IndexOf(videoWindow)]
                                     : null;

                videoWindow.WindowLayout = layout[VideoWindows.IndexOf(videoWindow) % layout.Count];

                var viewerSize = videoWindow.Viewer.Size;
                if (viewerSize.Width != videoWindow.Viewer.Size.Width || viewerSize.Height != videoWindow.Viewer.Size.Height)
                {
                    videoWindow.Viewer.Size = viewerSize;
                }

                if (videoWindow.Camera != device && device is ICamera)
                    videoWindow.Camera = (ICamera)device;
            }
        }

        protected virtual IVideoWindow RegisterVideoWindow()
        {
            IVideoWindow videoWindow = CreateVideoWindow();

            videoWindow.App = App;
            videoWindow.PlayMode = PlayMode;
            videoWindow.Initialize();

            switch (PlayMode)
            {
                case PlayMode.LiveStreaming:
                    videoWindow.Stretch = Server.Configure.StretchLiveVideo;
                    break;

                case PlayMode.GotoTimestamp:
                    videoWindow.Stretch = Server.Configure.StretchPlaybackVideo;
                    break;
            }

            var window = videoWindow as VideoWindow;
            if (window != null)
            {
                window.Server = Server;
                window.Cursor = (PlayMode == PlayMode.Snapshot) ? Cursors.NoMove2D : Cursors.Default;
            }

            if (DecodeIframeFlag)
                videoWindow.DecodeIframe();

            videoWindow.DisplayTitleBar = DisplayTitleBar;
            videoWindow.ToolMenu = ToolMenu;
            videoWindow.OnSelectionChange += VideoWindowOnSelectionChanged;
            videoWindow.OnWindowMouseDrag += VideoWindowOnWindowMouseDrag;
            videoWindow.OnMouseDown += VideoWindowOnMouseDown;
            videoWindow.OnFullScreen += videoWindow_OnFullScreen;
            videoWindow.OnCloseFullScreen += videoWindow_OnCloseFullScreen;

            return videoWindow;
        }

        protected void videoWindow_OnCloseFullScreen(object sender, EventArgs<string> e)
        {
            if (!Server.Configure.FullScreenBestResolution) return;

            foreach (var videoWindow in VideoWindows.Where(videoWindow => videoWindow != sender))
            {
                videoWindow.Activate();
            }
        }

        protected void videoWindow_OnFullScreen(object sender, EventArgs e)
        {
            if (!Server.Configure.FullScreenBestResolution) return;

            foreach (var videoWindow in VideoWindows.Where(videoWindow => videoWindow != sender))
            {
                videoWindow.Deactivate();
            }
        }

        protected virtual IVideoWindow CreateVideoWindow()
        {
            return VideoWindowProvider.RegistVideoWindow();
        }

        protected void VideoWindowOnMouseDown(Object sender, MouseEventArgs e)
        {
            if (windowsPanel.ContextMenu != null)
                windowsPanel.ContextMenu.Show((Control)ActivateVideoWindow, e.Location);
        }

        protected virtual void VideoWindowOnSelectionChanged(Object sender, EventArgs e)
        {
            if (ActivateVideoWindow == sender)
            {
                ActivateVideoWindow.Active = true;
                return;
            }

            if (ActivateVideoWindow != null)
                ActivateVideoWindow.Active = false;

            ActivateVideoWindow = null;

            if (sender is VideoWindow)
            {
                foreach (IVideoWindow videoWindow in VideoWindows)
                {
                    if (videoWindow != sender) continue;

                    ActivateVideoWindow = videoWindow;
                    ActivateVideoWindow.Active = true;

                    ChangeNameLabelText();

                    ToolMenu.UpdateLocation();

                    if (OnSelectionChange != null)
                        OnSelectionChange(this, new EventArgs<ICamera, PTZMode>(ActivateVideoWindow.Camera,
                            ((ActivateVideoWindow.Viewer != null) ? ActivateVideoWindow.Viewer.PtzMode : PTZMode.Disable)));
                    break;
                }
            }

            if (ActivateVideoWindow == null)
                _nameLabelText = "";

            nameLabel.Invalidate();
        }
    }
}
