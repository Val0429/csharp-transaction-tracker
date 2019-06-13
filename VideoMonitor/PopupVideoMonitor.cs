using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class PopupVideoMonitor : Form
    {
        public VideoMonitor VideoMonitor { get; set; }

        public event EventHandler OnDiscountVideoWindowByMenu;
        public Dictionary<String, String> Localization;

        public IApp App;
        public IServer Server;
        public IDeviceGroup DeviceGroup;
        public UInt16 Preset = 0;

        private Boolean _displayTitleBar;
        public Boolean DisplayTitleBar
        {
            get { return _displayTitleBar; }
            set
            {
                _displayTitleBar = value;
                if (VideoMonitor != null)
                    VideoMonitor.VideoTitleBar();
            }
        }

        public IDevice[] ToArray()
        {
            var deviceCount = (VideoMonitor.Devices.Count > 0) ? (VideoMonitor.Devices.Aggregate(0, (current, obj) => Math.Max(current, obj.Key)) + 1) : 0;

            if (deviceCount == 0) return new IDevice[0];

            var devices = new IDevice[deviceCount];

            foreach (KeyValuePair<Int32, IDevice> obj in VideoMonitor.Devices)
            {
                if (obj.Value == null) continue;
                devices[obj.Key] = obj.Value;
            }

            return devices;
        }

        public PopupVideoMonitor()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"VideoMonitor_VideoMonitor", "View Monitor"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();

            Text = Localization["VideoMonitor_VideoMonitor"];

            Application.AddMessageFilter(_globalMouseHandler);

            //SizeChanged += VideoMonitorSizeChanged;

            FormClosing += VideoMonitorFormClosing;
            _globalMouseHandler.TheMouseMoved += GlobalMouseHandlerTheMouseMoved;
        }

        private Point _currentMousePoint;
        private void GlobalMouseHandlerTheMouseMoved()
        {
            if (_currentMousePoint != Cursor.Position)
            {
                App.IdleTimer = 0;
            }
            _currentMousePoint = Cursor.Position;
        }

        //------------------------------------------------------------------------------------------------------------------
        public void UpdateRecordStatus()
        {
            foreach (IVideoWindow videoWindow in VideoMonitor.VideoWindows)
            {
                if (videoWindow.Visible)
                    videoWindow.Viewer.UpdateRecordStatus();
            }
        }

        public void Reset()
        {
            VisibleChanged -= VideoMonitorShown;
            VisibleChanged += VideoMonitorShown;

            if (VideoMonitor != null)
                VideoMonitor.ClearAll();

            Application.RemoveMessageFilter(_globalMouseHandler);
            Application.AddMessageFilter(_globalMouseHandler);
        }

        /*private void VideoMonitorSizeChanged(Object sender, EventArgs e)
        {
            _toolMenu.UpdateLocation();
        }*/

        private void VideoMonitorShown(Object sender, EventArgs e)
        {
            VisibleChanged -= VideoMonitorShown;
            Initialize();
            Play();
            
        }

        private Boolean _isApplyDeviceGroup = false;
        public void Initialize()
        {
            if (DeviceGroup == null) return;

            if (VideoMonitor == null)
            {
                VideoMonitor = CreateVideoMonitor();
                
                VideoMonitor.IsPopup = true;
                VideoMonitor.Initialize();
                
                VideoMonitor.HideToolPanel();
                VideoMonitor.SetLiveProperty();
                
                VideoMonitor.Parent = this;
                VideoMonitor.OnDiscountVideoWindowByMenu += VideoMonitorOnDiscountVideoWindowByMenu;
            }
            VideoMonitor.CheckMenuPermission();
            VideoMonitor.DisplayTitleBar = DisplayTitleBar;
            VideoMonitor.VideoTitleBar();

            VideoMonitor.ShowGroup(DeviceGroup);
            _isApplyDeviceGroup = true;
            //_videoMonitor.Play();
        }

        protected virtual VideoMonitor CreateVideoMonitor()
        {
            return new VideoMonitor { App = App, Server = Server };
        }

        private void VideoMonitorOnDiscountVideoWindowByMenu(Object sender, EventArgs e)
        {
            if (OnDiscountVideoWindowByMenu != null)
                OnDiscountVideoWindowByMenu(this, null);
        }

        public void Play()
        {
            if (VideoMonitor != null)
                VideoMonitor.Play();
        }

        public void Reconnect()
        {
            if (VideoMonitor == null) return;

            foreach (var videoWindow in VideoMonitor.VideoWindows)
            {
                if (!videoWindow.Visible) continue;
                if (videoWindow.Camera is IDeviceLayout || videoWindow.Camera is ISubLayout) continue;

                videoWindow.Reconnect();
            }
        }

        public void ChangeProfileMode(String mode)
        {
            if (VideoMonitor == null) return;

            VideoMonitor.ChangeProfileMode(mode);
        }

        public FormWindowState PreviousWindowState;
        public void FullScreen()
        {
            PreviousWindowState = WindowState;
            //switch to normal and switch to maximum to ensure window is REALL maximum
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;

            ShowIcon = false;

            UpdateRecordStatus();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        public void ExitFullScreen()
        {
            WindowState = PreviousWindowState;// FormWindowState.Normal;

            FormBorderStyle = FormBorderStyle.Sizable;
            UpdateRecordStatus();
            ShowIcon = true;
        }

        private GlobalMouseHandler _globalMouseHandler = new GlobalMouseHandler();

        public void GlobatMouseMoveHandler()
        {
            if (VideoMonitor != null)
                VideoMonitor.GlobalMouseHandler();
        }

        private void VideoMonitorFormClosing(Object sender, FormClosingEventArgs e)
        {
            //if (_globalMouseHandler != null)
            //{
            //    Application.RemoveMessageFilter(_globalMouseHandler);
            //    _globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;
            //}
            _isApplyDeviceGroup = false;
            VideoMonitor.ClearAll();

            //dont close window, recycle use form on next
            Hide();
            e.Cancel = true;
        }

        public List<IDevice> DeviceList
        {
            get
            {
                var deviceList = new List<IDevice>();

                //if (_isApplyDeviceGroup && VideoMonitor != null)
                //{
                //    deviceList.AddRange(VideoMonitor.Devices.Select(device => device.Value));

                //    return deviceList;
                //}

                if (DeviceGroup == null || DeviceGroup.Items == null)
                    return deviceList;

                if (VideoMonitor != null)
                {
                    if (VideoMonitor.Devices.Count == 0)
                        return deviceList;

                    foreach (var device in VideoMonitor.Devices)
                    {
                        deviceList.Add(device.Value);
                    }
                }
                else
                {
                    foreach (var device in DeviceGroup.View)
                    {
                        if (device == null) continue;

                        deviceList.Add(device);
                    }
                }

                return deviceList;
            }
        }

        public List<IDevice> DeviceListForDisconnectByMenu
        {
            get
            {
                var deviceList = new List<IDevice>();

                if (_isApplyDeviceGroup && VideoMonitor != null)
                {
                    deviceList.AddRange(VideoMonitor.Devices.Select(device => device.Value));

                    return deviceList;
                }

                if (DeviceGroup == null || DeviceGroup.Items == null)
                    return deviceList;

                foreach (var device in DeviceGroup.View)
                {
                    if (device == null) continue;

                    deviceList.Add(device);
                }
                return deviceList;
            }
        }

        public Int32 DeviceCount
        {
            get
            {
                if (_isApplyDeviceGroup && VideoMonitor != null)
                    return VideoMonitor.Devices.Count;

                if (DeviceGroup == null || DeviceGroup.Items == null)
                    return 0;

                var count = 0;
                foreach (var device in DeviceGroup.View)
                {
                    if (device == null) continue;

                    count++;
                }
                return count;
            }
        }

        public Int32 LayoutCount
        {
            get
            {
                if (_isApplyDeviceGroup && VideoMonitor != null)
                    return VideoMonitor.WindowLayout.Count;

                if (DeviceGroup == null || DeviceGroup.Layout == null)
                    return 0;

                return DeviceGroup.Layout.Count;
            }
        }

        private void LiveStreamActivated(Object sender, EventArgs e)
        {/*
			if (_globalMouseHandler != null)
			{
				_globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;
				_globalMouseHandler.TheMouseMoved += GlobatMouseMoveHandler;
			}*/
        }

        private void LiveStreamDeactivate(Object sender, EventArgs e)
        {/*
			if (_globalMouseHandler != null)
				_globalMouseHandler.TheMouseMoved -= GlobatMouseMoveHandler;*/
        }

        //------------------------------------------------------------------------------------------------------------------

        #region Globals

        [DllImport("user32")]
        private static extern int GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        private static extern bool DeleteMenu(int hMenu, int uPosition, int uFlags);
        private int _systemMenuHandle = 0;

        #endregion

        /*private const int CP_NOCLOSE_BUTTON = 0x200;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams myCp = base.CreateParams;
				myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
				return myCp;
			}
		}*/
        public void EnableCloseButton() //A standard void function to invoke EnableMenuItem()
        {
            _systemMenuHandle = GetSystemMenu(Handle, true);
            DeleteMenu(_systemMenuHandle, 6, 1024);
        }

        public void DisableCloseButton() //A standard void function to invoke EnableMenuItem()
        {
            _systemMenuHandle = GetSystemMenu(Handle, false);
            DeleteMenu(_systemMenuHandle, 6, 1024);
        }
    }
}
