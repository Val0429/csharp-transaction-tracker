using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Timer = System.Timers.Timer;

namespace EventPanel
{
    public partial class PriorityEventPanel : UserControl, IControl, IAppUse, IServerUse, IMinimize, IMouseHandler,
        IBlockPanelUse
    {
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        private static readonly Image _unreadEventImage = Resources.GetResources(Properties.Resources.eventCount, Properties.Resources.IMGEventCount);

        private static UInt16 MaxLog = 20;

        private readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();
        private readonly Queue<PriorityEventLog> StoredLog = new Queue<PriorityEventLog>();
        private readonly Timer _hideUnreadTimer = new Timer();
        private readonly Font _unreadFont = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        private readonly List<ICameraEvent> _unreadICameraEvent = new List<ICameraEvent>();

        private readonly Dictionary<String, String> _localization;

        protected INVR NVR;
        protected IPTS PTS;
        public Boolean PopupInstantPlayback = true;
        private UInt32 _eventCount = 1;
        private IEventListener _eventListener;
        private bool _isPuaseReceiveEvent;
        private ToolStripMenuItemUI2 _pauseMenuItem;

        protected Image _pauseevent;
        protected Image _pauseeventactivate;
        private Point _previousScrollPosition;
        protected Image _reset;
        private ToolStripMenuItemUI2 _resetMenuItem;
        private IServer _server;
        protected Boolean _showUserDefiendEvnet;
        private Int16 _unreadEventCount;


        public PriorityEventPanel()
        {
            _localization = new Dictionary<String, String>
            {
                {"Control_Event", "Event"},
                {"EventPanel_ClearEvent", "Clear event log"},
                {"EventPanel_PauseEvent", "Suspended receive events"},
                {"EventPanel_ResumeEvent", "Resuming receive events"},
                {"EventPanel_EventHelper", "Event Handle Help"},
            };
            Localizations.Update(_localization);

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

        public IApp App { get; set; }
        public IBlockPanel BlockPanel { get; set; }
        public String TitleName { get; set; }

        public virtual void Initialize()
        {
            if (Server is ICMS)
                MaxLog = 100;

            var controlPanel = Parent as IControlPanel;
            if (controlPanel != null)
            {
                BlockPanel.SyncDisplayControlList.Add(controlPanel);
            }

            PanelTitleBarUI2.Text = TitleName = _localization["Control_Event"];
            titlePanel.Controls.Add(PanelTitleBarUI2);
            PanelTitleBarUI2.InitializeToolStripMenuItem();

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            AddResetIcon();
            AddPauseIcon();
            eventListPanel.Click += EventListPanelClick;

            for (UInt16 i = 0; i < MaxLog; i++)
            {
                var log = new PriorityEventLog
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

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public event EventHandler OnMinimizeChange;
        public Button Icon { get; private set; }

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }

        public Boolean IsMinimize { get; private set; }

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
                foreach (ICameraEvent cameraEvent in _unreadICameraEvent)
                {
                    AddEvent(cameraEvent);
                }
                _unreadICameraEvent.Clear();
            }

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
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


        public event EventHandler<EventArgs<IDevice, DateTime>> OnLogDoubleClick;

        public event EventHandler<EventArgs<String>> OnPreDefineEvent;

        private void RaiseOnPreDefineEvent(object sender, EventArgs<string> e)
        {
            RaiseOnPreDefineEvent(e);
        }

        protected void RaiseOnPreDefineEvent(EventArgs<string> e)
        {
            EventHandler<EventArgs<string>> handler = OnPreDefineEvent;
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
            EventHandler<EventArgs<string>> handler = OnUserDefineEvent;
            if (handler != null)
            {
                handler(this, e);
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
                    Replace("%1", e.Value1.ToString())
                    .Replace("%2", e.Value2.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));

                var eventLog = sender as PriorityEventLog;
                if (eventLog != null)
                {
                    var viewModel = new EventViewModel(eventLog.CameraEvent);
                    viewModel.EventAcknowledged += viewModel_EventAcknowledged;
                    App.PopupInstantPlayback(e.Value1, DateTimes.ToUtc(e.Value2, Server.Server.TimeZone), viewModel);
                }
            }

            if (OnLogDoubleClick != null)
                OnLogDoubleClick(this, e);
        }

        void viewModel_EventAcknowledged(object sender, EventArgs e)
        {
            var viewModel = sender as EventViewModel;
            if (viewModel != null)
            {
                viewModel.EventAcknowledged -= viewModel_EventAcknowledged;
                var eventLog = eventListPanel.Controls.OfType<PriorityEventLog>().FirstOrDefault(log => log.CameraEvent == viewModel.CameraEvent);
                if (eventLog != null)
                {
                    eventListPanel.Controls.Remove(eventLog);
                }
            }
        }

        private void EventListPanelClick(Object sender, EventArgs e)
        {
            eventListPanel.Focus();
        }

        public void AddResetIcon()
        {
            _resetMenuItem = new ToolStripMenuItemUI2
            {
                Text = _localization["EventPanel_ClearEvent"],
            };
            _resetMenuItem.Click += ResetMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(_resetMenuItem);
        }

        public void AddPauseIcon()
        {
            _pauseMenuItem = new ToolStripMenuItemUI2
            {
                Text = _localization["EventPanel_PauseEvent"],
            };
            _pauseMenuItem.Click += PauseMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(_pauseMenuItem);
        }

        private void ResetMenuItemClick(Object sender, EventArgs e)
        {
            _eventCount = 1;
            foreach (var log in eventListPanel.Controls.OfType<PriorityEventLog>())
            {
                StoredLog.Enqueue(log);
            }

            Server.WriteOperationLog("Clear event logs");
            eventListPanel.Controls.Clear();
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

        public void ShowUserDefiendEvnet()
        {
            _showUserDefiendEvnet = true;
        }

        private void IconPaint(Object sender, PaintEventArgs e)
        {
            if (_unreadEventCount == 0) return;

            Graphics g = e.Graphics;

            g.DrawImage(_unreadEventImage, 28, 39);

            if (_unreadEventCount >= 100)
            {
                g.DrawString("99+", _unreadFont, Brushes.White, 32, 41);
                return;
            }

            int x = (_unreadEventCount < 10) ? 37 : 34;
            g.DrawString(_unreadEventCount.ToString(CultureInfo.InvariantCulture), _unreadFont, Brushes.White, x, 41);
        }

        private void AddEvent(ICameraEvent cameraEvent)
        {
            if (IsPauseReceiveEvent) return;
            // NOTE: RecordFail, RecordRecovery event don't contain device
            if (cameraEvent == null || (cameraEvent.Type == EventType.UserDefine && !_showUserDefiendEvnet)) return;

            if (InvokeRequired)
            {
                Invoke(new Action<ICameraEvent>(AddEvent), cameraEvent);
                return;
            }

            try
            {
                //dont show event when is minimize
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

                var log = GetEventLog();
                log.IsOdd = ((_eventCount % 2) == 0);

                if (log.UpdateLog(cameraEvent, _eventCount++))
                {
                    var priority = log.GetPriority();
                    var itemControl = eventListPanel.Controls.OfType<PriorityEventLog>().LastOrDefault(l => l.GetPriority() >= priority && l.CameraEvent.DateTime < log.CameraEvent.DateTime);
                    if (itemControl != null)
                    {
                        var idx = eventListPanel.Controls.IndexOf(itemControl);
                        eventListPanel.Controls.Insert(log, idx + 1);
                    }
                    else
                    {
                        eventListPanel.Controls.Insert(log, 0);
                    }
                }
                else
                {
                    _eventCount--;
                    StoredLog.Enqueue(log);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void AddEvent(POS_Exception.TransactionItem transactionItem)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new AddTransactionEventDelegate(AddEvent), transactionItem);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return;
            }

            var log = GetEventLog();
            log.IsOdd = ((_eventCount % 2) == 0);
            if (log.UpdateLog(transactionItem, _eventCount++))
                eventListPanel.Controls.Add(log);
            else
            {
                _eventCount--;
                StoredLog.Enqueue(log);
            }
        }

        private PriorityEventLog GetEventLog()
        {
            if (StoredLog.Count > 0)
            {
                return StoredLog.Dequeue();
            }

            return eventListPanel.Controls[0] as PriorityEventLog;
        }

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

        private delegate void AddTransactionEventDelegate(POS_Exception.TransactionItem transactionItem);
    }
}