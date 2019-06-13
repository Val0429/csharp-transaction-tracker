using System.Globalization;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace EventPanel
{
    public partial class EventPanel : UserControl, IControl, IAppUse, IServerUse, IMinimize, IMouseHandler, IBlockPanelUse
    {
        // Events
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<IDevice, DateTime>> OnLogDoubleClick;

        public event EventHandler<EventArgs<String>> OnPreDefineEvent;
        private void RaiseOnPreDefineEvent(object sender, EventArgs<string> e)
        {
            RaiseOnPreDefineEvent(e);
        }
        protected void RaiseOnPreDefineEvent(EventArgs<string> e)
        {
            var handler = OnPreDefineEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<EventArgs<String>> OnUserDefineEvent;
        private void RaiseOnUserDefineEvent(object sender, EventArgs<string> e)
        {
            RaiseOnUserDefineEvent(e);
        }
        protected void RaiseOnUserDefineEvent(EventArgs<string> e)
        {
            var handler = OnUserDefineEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public readonly Queue<EventLog> StoredLog = new Queue<EventLog>();
        public String TitleName { get; set; }

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        public Dictionary<String, String> Localization;

        public IApp App { get; set; }
        protected INVR NVR;
        protected IPTS PTS;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is INVR)
                    NVR = value as INVR;
                if (value is IPTS)
                    PTS = value as IPTS;
            }
        }
        public IBlockPanel BlockPanel { get; set; }

        protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();
        private ToolStripMenuItemUI2 _resetMenuItem;
        private ToolStripMenuItemUI2 _pauseMenuItem;

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }

        public Boolean IsMinimize { get; private set; }

        protected Image _reset;
        protected Image _pauseevent;
        protected Image _pauseeventactivate;
        private static readonly Image _unreadEventImage = Resources.GetResources(Properties.Resources.eventCount, Properties.Resources.IMGEventCount);

        private readonly System.Timers.Timer _hideUnreadTimer = new System.Timers.Timer();

        private IEventListener _eventListener;

        public EventPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_Event", "Event"},

								   {"EventPanel_ClearEvent", "Clear event log"},

								   {"EventPanel_PauseEvent", "Suspended receive events"},
								   {"EventPanel_ResumeEvent", "Resuming receive events"},
								   {"EventPanel_EventHelper", "Event Handle Help"},
							   };
            Localizations.Update(Localization);

            _reset = Resources.GetResources(Properties.Resources.reset, Properties.Resources.IMGReset);
            _pauseevent = Resources.GetResources(Properties.Resources.pause_event, Properties.Resources.IMGPauseEvent);
            _pauseeventactivate = Resources.GetResources(Properties.Resources.pause_event_activate, Properties.Resources.IMGPauseEventActivate);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            _hideUnreadTimer.SynchronizingObject = this;
            _hideUnreadTimer.Elapsed += HideUnread;
            _hideUnreadTimer.Interval = 3000;

            //---------------------------
            Icon = new ControlIconButton { Image = _iconActivate, BackgroundImage = ControlIconButton.IconBgActivate };
            Icon.Click += DockIconClick;
            Icon.Paint += IconPaint;
            //---------------------------
        }

        public Boolean PopupInstantPlayback = true;
        public virtual void Initialize()
        {
            if (Server is ICMS)
                MaxLog = 100;

            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            PanelTitleBarUI2.Text = TitleName = Localization["Control_Event"];
            titlePanel.Controls.Add(PanelTitleBarUI2);
            PanelTitleBarUI2.InitializeToolStripMenuItem();

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            AddResetIcon();
            AddPauseIcon();
            eventListPanel.Click += EventListPanelClick;

            for (UInt16 i = 0; i < MaxLog; i++)
            {
                var log = new EventLog
                {
                    App = App,
                    Server = Server
                };

                log.OnLogDoubleClick += LogOnLogDoubleClick;
                StoredLog.Enqueue(log);
            }

            _eventListener = CreateEventListener();

            Debug.Assert(_eventListener != null);
            if (_eventListener != null)
            {
                _eventListener.OnEventReceived += AddEvent;
                _eventListener.OnPreDefineEvent += RaiseOnPreDefineEvent;
                _eventListener.OnUserDefineEvent += RaiseOnUserDefineEvent;
            }
        }

        protected virtual IEventListener CreateEventListener()
        {
            if (NVR != null)
            {
                return new NvrEventListener(NVR);
            }

            if (PTS != null)
            {
                return new PtsEventListener(PTS);
            }

            return default(IEventListener);
        }

        protected void HideUnread(Object sender, EventArgs e)
        {
            _hideUnreadTimer.Enabled = false;
            _unreadEventCount = 0;
            Icon.Invalidate();
        }

        public void SetEMapProperty()
        {
            PopupInstantPlayback = false;
        }

        protected virtual void LogOnLogDoubleClick(Object sender, EventArgs<IDevice, DateTime> e)
        {
            if (PopupInstantPlayback && e.Value1 != null)
            {
                Server.WriteOperationLog("Review event panel popup event playback Channel:%1 Datetime:%2".
                    Replace("%1", e.Value1.ToString()).Replace("%2", e.Value2.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                App.PopupInstantPlayback(e.Value1, DateTimes.ToUtc(e.Value2, Server.Server.TimeZone));
            }

            if (OnLogDoubleClick != null)
                OnLogDoubleClick(this, e);
        }

        private void EventListPanelClick(Object sender, EventArgs e)
        {
            eventListPanel.Focus();
        }

        public void AddResetIcon()
        {
            _resetMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["EventPanel_ClearEvent"],
            };
            _resetMenuItem.Click += ResetMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(_resetMenuItem);
        }

        public void AddPauseIcon()
        {
            _pauseMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["EventPanel_PauseEvent"],
            };
            _pauseMenuItem.Click += PauseMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(_pauseMenuItem);
        }

        private void ResetMenuItemClick(Object sender, EventArgs e)
        {
            _eventCount = 1;
            foreach (EventLog log in eventListPanel.Controls)
            {
                StoredLog.Enqueue(log);
            }

            Server.WriteOperationLog("Clear event logs");
            eventListPanel.Controls.Clear();
        }

        private bool _isPuaseReceiveEvent;
        public bool IsPauseReceiveEvent
        {
            get { return _isPuaseReceiveEvent; }
            set
            {
                _isPuaseReceiveEvent = value;

                if (_eventListener == null) return;

                if (value)
                    _eventListener.Pause();
                else
                    _eventListener.Resume();
            }
        }

        private void PauseMenuItemClick(Object sender, EventArgs e)
        {
            IsPauseReceiveEvent = !IsPauseReceiveEvent;

            if (IsPauseReceiveEvent)
            {
                //_pauseMenuItem.BackgroundImage = _pauseeventactivate;
                //SharedToolTips.SharedToolTip.SetToolTip(_pauseMenuItem, Localization["EventPanel_ResumeEvent"]);

                _pauseMenuItem.IsSelected = true;

                Server.WriteOperationLog("Suspend event receiving");
            }
            else
            {
                //_pauseMenuItem.BackgroundImage = _pauseevent;
                //SharedToolTips.SharedToolTip.SetToolTip(_pauseMenuItem, Localization["EventPanel_PauseEvent"]);
                _pauseMenuItem.IsSelected = false;

                Server.WriteOperationLog("Resuming event receiving");
            }
        }

        //protected Boolean _showUserDefiendEvnet;

        //public void ShowUserDefiendEvnet()
        //{
        //    _showUserDefiendEvnet = true;
        //}

        private Int16 _unreadEventCount;
        private readonly List<ICameraEvent> _unreadICameraEvent = new List<ICameraEvent>();

        private readonly Font _unreadFont = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        private void IconPaint(Object sender, PaintEventArgs e)
        {
            if (_unreadEventCount == 0) return;

            var g = e.Graphics;

            g.DrawImage(_unreadEventImage, 28, 39);

            if (_unreadEventCount >= 100)
            {
                g.DrawString("99+", _unreadFont, Brushes.White, 32, 41);
                return;
            }

            var x = (_unreadEventCount < 10) ? 37 : 34;
            g.DrawString(_unreadEventCount.ToString(), _unreadFont, Brushes.White, x, 41);
        }

        private UInt32 _eventCount = 1;
        private static UInt16 MaxLog = 20;
        private void AddEvent(ICameraEvent cameraEvent)
        {
            if (IsPauseReceiveEvent) return;
            // NOTE: RecordFail, RecordRecovery event don't contain device
            if (cameraEvent == null || (cameraEvent.Type == EventType.UserDefine && !Server.Configure.EnableUserDefine ))
                return;
            if (!ValidateCameraEvent(cameraEvent)) return;

            if (InvokeRequired)
            {
                Invoke(new Action<ICameraEvent>(AddEvent), cameraEvent);
                return;
            }

            //dont show event when is minimize
            try
            {
                if (IsMinimize)
                {
                    _unreadEventCount++;

                    if (_unreadEventCount > 100)
                        _unreadEventCount = 100;

                    Icon.Invalidate();

                    _unreadICameraEvent.Add(cameraEvent);

                    while (_unreadICameraEvent.Count > MaxLog)
                    {
                        _unreadICameraEvent.Remove(_unreadICameraEvent[0]);
                        _eventCount++;
                    }
                    return;
                }

                EventLog log = GetEventLog();
                log.IsOdd = ((_eventCount % 2) == 0);

                if (log.UpdateLog(cameraEvent, _eventCount++))
                {
                    eventListPanel.Controls.Add(log);
                }
                else
                {
                    _eventCount--;
                    StoredLog.Enqueue(log);
                }
                //else
                //{
                //    EventLog log = GetEventLog();
                //    log.IsOdd = ((_eventCount % 2) == 0);
                //    log.UpdateLog(cameraEvent, cameraEvent.DateTime, _eventCount++);
                //    eventListPanel.Controls.Add(log);
                //}
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        private delegate void AddTransactionEventDelegate(POS_Exception.TransactionItem transactionItem);
        public void AddEvent(POS_Exception.TransactionItem transactionItem)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new AddTransactionEventDelegate(AddEvent), transactionItem);
                }
                catch (Exception)
                {
                }
                return;
            }

            EventLog log = GetEventLog();
            log.IsOdd = ((_eventCount % 2) == 0);
            if (log.UpdateLog(transactionItem, _eventCount++))
                eventListPanel.Controls.Add(log);
            else
            {
                _eventCount--;
                StoredLog.Enqueue(log);
            }
        }


        private static bool ValidateCameraEvent(ICameraEvent cameraEvent)
        {
            var camera = cameraEvent.Device as ICamera;
            switch (cameraEvent.Type)
            {
                case EventType.Motion:
                    if (camera == null)
                    {
                        return false;
                    }
                    if (camera.Model.NumberOfMotion < cameraEvent.Id)
                    {
                        return false;
                    }
                    break;

                case EventType.DigitalInput:
                    if (camera == null)
                    {
                        return false;
                    }
                    if (camera.IOPort.Count > 0)
                    {
                        if (!camera.IOPort.ContainsKey(cameraEvent.Id) || camera.IOPort[cameraEvent.Id] != IOPort.Input)
                            return false;
                    }
                    else
                    {
                        if (camera.Model.NumberOfDi < cameraEvent.Id)
                            return false;
                    }
                    break;

                case EventType.DigitalOutput:
                    if (camera == null)
                    {
                        return false;
                    }

                    if (camera.IOPort.Count > 0)
                    {
                        if (!camera.IOPort.ContainsKey(cameraEvent.Id) || camera.IOPort[cameraEvent.Id] != IOPort.Output)
                            return false;
                    }
                    else
                    {
                        if (camera.Model.NumberOfDo < cameraEvent.Id)
                            return false;
                    }
                    break;
            }
            return true;
        }

        private EventLog GetEventLog()
        {
            if (StoredLog.Count > 0)
            {
                return StoredLog.Dequeue();
            }

            return eventListPanel.Controls[0] as EventLog;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void GlobalMouseHandler()
        {
            if (Drag.IsDrop(eventListPanel))
            {
                if (!eventListPanel.AutoScroll)
                {
                    eventListPanel.AutoScroll = true;
                }

                return;
            }

            if (eventListPanel.AutoScroll)
                HideScrollBar();
        }

        private Point _previousScrollPosition;
        private void HideScrollBar()
        {
            _previousScrollPosition = eventListPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            eventListPanel.AutoScroll = false;

            //force refresh to hide scroll bar
            eventListPanel.Height++;
            eventListPanel.AutoScrollPosition = _previousScrollPosition;
        }

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else
                Minimize();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
            {
                BlockPanel.HideThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = true;
                    App.StartupOption.SaveSetting();
                }
            }

            Icon.Image = _icon;
            Icon.BackgroundImage = null;

            _hideUnreadTimer.Stop();
            _unreadEventCount = 0;
            Icon.Invalidate();

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0")
            {
                BlockPanel.ShowThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = false;
                    App.StartupOption.SaveSetting();
                }
            }

            //wait 3 sec to wipe out unread info
            _hideUnreadTimer.Start();

            Icon.Image = _iconActivate;
            Icon.BackgroundImage = ControlIconButton.IconBgActivate;

            IsMinimize = false;

            if (_unreadICameraEvent.Count > 0)
            {
                foreach (var cameraEvent in _unreadICameraEvent)
                {
                    AddEvent(cameraEvent);
                }
                _unreadICameraEvent.Clear();
            }

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }
    }
}