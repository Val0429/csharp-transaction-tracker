using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace EventPanel
{
	public partial class EventFlowLayoutPanel : UserControl, IControl, IAppUse, IServerUse
	{
		public readonly Queue<EventLogWithSnapshot> StoredLog = new Queue<EventLogWithSnapshot>();

		public String TitleName { get; set; }
		
		private PictureBox _pauseEvent;
		private PictureBox _noSnapshot;
		private PictureBox _smallSize;
		private PictureBox _mediumSize;
		private PictureBox _largeSize;

		public Dictionary<String, String> Localization;

		public IApp App { get; set; }
		protected INVR NVR;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set {
				_server = value;
				if (value is INVR)
					NVR = value as INVR;
			}
		}

		private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();
		private PictureBox _resetButton;

		public UInt16 MinimizeHeight
		{
			get { return (UInt16)titlePanel.Size.Height; }
		}

		private static readonly Image _reset = Resources.GetResources(Properties.Resources.reset, Properties.Resources.IMGReset);
		private static readonly Image _pauseevent = Resources.GetResources(Properties.Resources.pause_event, Properties.Resources.IMGPauseEvent);
		private static readonly Image _pauseeventactivate = Resources.GetResources(Properties.Resources.pause_event_activate, Properties.Resources.IMGPauseEventActivate);
		private static readonly Image _nosnapshot = Resources.GetResources(Properties.Resources.no_snapshot, Properties.Resources.IMGNoSnapshot);
		private static readonly Image _nosnapshotactivate = Resources.GetResources(Properties.Resources.no_snapshot_activate, Properties.Resources.IMGNoSnapshotActivate);
		private static readonly Image _small = Resources.GetResources(Properties.Resources.small, Properties.Resources.IMGSmall);
		private static readonly Image _smallactivate = Resources.GetResources(Properties.Resources.small_activate, Properties.Resources.IMGSmallActivate);
		private static readonly Image _medium = Resources.GetResources(Properties.Resources.medium, Properties.Resources.IMGMedium);
		private static readonly Image _mediumactivate = Resources.GetResources(Properties.Resources.medium_activate, Properties.Resources.IMGMediumActivate);
		private static readonly Image _large = Resources.GetResources(Properties.Resources.large, Properties.Resources.IMGLarge);
		private static readonly Image _largeactivate = Resources.GetResources(Properties.Resources.large_activate, Properties.Resources.IMGLargeActivate);
		public EventFlowLayoutPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_Event", "Event"},

								   {"EventPanel_ClearEvent", "Clear event log"},
								   {"EventPanel_NoSnapshot", "No Snapshot"},

								   {"EventPanel_PauseEvent", "Suspended receive events"},
								   {"EventPanel_ResumeEvent", "Resuming receive events"},
							   };
			Localizations.Update(Localization);
		}

		public void Initialize()
		{
			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			_panelTitleBar.Text = TitleName = Localization["Control_Event"];
			_panelTitleBar.HideMinimize();
			titlePanel.Controls.Add(_panelTitleBar);

			AddIconSets();
			eventListPanel.Click += EventListPanelClick;
			eventListPanel.SizeChanged += EventListPanelSizeChanged;

			NVR.OnEventReceive -= EventReceive;
			NVR.OnEventReceive += EventReceive;

			for (UInt16 i = 0; i < MaxLog; i++)
			{
				StoredLog.Enqueue(new EventLogWithSnapshot
				{
					App = App,
					Server = Server,
					FlowLayoutPanel = this,
				});
			}
		}

		private void EventListPanelClick(Object sender, EventArgs e)
		{
			eventListPanel.Focus();
		}

		public void AddIconSets()
		{
			_noSnapshot = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _nosnapshot,
				BackgroundImageLayout = ImageLayout.Center
			};
			_noSnapshot.MouseClick += ChangeToNoSnapshot;
			_panelTitleBar.Controls.Add(_noSnapshot);
			SharedToolTips.SharedToolTip.SetToolTip(_noSnapshot, Localization["EventPanel_NoSnapshot"]);

			_smallSize = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _smallactivate,
				BackgroundImageLayout = ImageLayout.Center
			};
			_smallSize.MouseClick += ChangeToSmallSize;
			_panelTitleBar.Controls.Add(_smallSize);
			SharedToolTips.SharedToolTip.SetToolTip(_smallSize, "QQVGA" + Environment.NewLine + "160x120");

			_mediumSize = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _medium,
				BackgroundImageLayout = ImageLayout.Center
			};
			_mediumSize.MouseClick += ChangeToMediumSize;
			_panelTitleBar.Controls.Add(_mediumSize);
			SharedToolTips.SharedToolTip.SetToolTip(_mediumSize, "HQVGA" + Environment.NewLine + "240x160");

			_largeSize = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _large,
				BackgroundImageLayout = ImageLayout.Center
			};
			_largeSize.MouseClick += ChangeToLargeSize;
			_panelTitleBar.Controls.Add(_largeSize);
			SharedToolTips.SharedToolTip.SetToolTip(_largeSize, "QVGA" + Environment.NewLine + "320x240");

			_resetButton = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _reset,
				BackgroundImageLayout = ImageLayout.Center
			};

			SharedToolTips.SharedToolTip.SetToolTip(_resetButton, Localization["EventPanel_ClearEvent"]);

			_resetButton.MouseClick += ResetButtonMouseClick;
			_panelTitleBar.Controls.Add(_resetButton);

			_pauseEvent = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _pauseevent,
				BackgroundImageLayout = ImageLayout.Center
			};
			_pauseEvent.MouseClick += PauseEvent;
			_panelTitleBar.Controls.Add(_pauseEvent);
			SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["EventPanel_PauseEvent"]);
		}

		private void ResetButtonMouseClick(Object sender, MouseEventArgs e)
		{
			_eventCount = 1;
			foreach (EventLogWithSnapshot log in eventListPanel.Controls)
			{
				if(!StoredLog.Contains(log))
					StoredLog.Enqueue(log);

			}
			eventListPanel.Controls.Clear();
		}

		public void EventReceive(Object sender, EventArgs<List<ICameraEvent>> e)
		{
			if (IsPauseReceiveEvent) return;

			if (e.Value == null) return;

			foreach (ICameraEvent cameraEvent in e.Value)
			{
				AddEvent(cameraEvent);
			}
		}

		private UInt32 _eventCount = 1;
		private const UInt16 MaxLog = 100;
		private delegate void AddCameraEventDelegate(ICameraEvent cameraEvent);
		public void AddEvent(ICameraEvent cameraEvent)
		{
			if (IsPauseReceiveEvent) return;

			if (InvokeRequired)
			{
				try
				{
					Invoke(new AddCameraEventDelegate(AddEvent), cameraEvent);
				}
				catch (Exception)
				{
				}
				return;
			}

			if (cameraEvent.Device != null)
			{
				EventLogWithSnapshot log = GetEventLog();
				log.UpdateLog(cameraEvent, _eventCount++);

				if (!eventListPanel.Controls.Contains(log))
					eventListPanel.Controls.Add(log);

				log.BringToFront();
			}
			else
			{
				EventLogWithSnapshot log = GetEventLog();
				log.UpdateLog(cameraEvent, cameraEvent.DateTime, _eventCount++);
				
				if (!eventListPanel.Controls.Contains(log))
					eventListPanel.Controls.Add(log);

				log.BringToFront();
			}
		}

		private EventLogWithSnapshot GetEventLog()
		{
			if (StoredLog.Count > 0)
			{
				return StoredLog.Dequeue();
			}

			var log = eventListPanel.Controls[eventListPanel.Controls.Count - 1] as EventLogWithSnapshot;

			if (log != null)
				log.Reset();

			return log;
		}

		public Boolean IsPauseReceiveEvent;
		private void PauseEvent(Object sender, MouseEventArgs e)
		{
			IsPauseReceiveEvent = !IsPauseReceiveEvent;

			if (IsPauseReceiveEvent)
			{
				foreach (EventLogWithSnapshot panel in eventListPanel.Controls)
				{
					panel.UpdatePenColor();
				}

				_pauseEvent.BackgroundImage = _pauseeventactivate;
				SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["EventPanel_ResumeEvent"]);
			}
			else
			{
				_pauseEvent.BackgroundImage = _pauseevent;
				SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["EventPanel_PauseEvent"]);
			}
		}

		public String LogSize = "QQVGA";
		private void ChangeToNoSnapshot(Object sender, MouseEventArgs e)
		{
			if (LogSize == "NoSnapshot") return;

			LogSize = "NoSnapshot";
			_noSnapshot.BackgroundImage = _nosnapshotactivate;
			_smallSize.BackgroundImage = _small;
			_mediumSize.BackgroundImage = _medium;
			_largeSize.BackgroundImage = _large;

			eventListPanel.Visible = false;
			foreach (EventLogWithSnapshot panel in eventListPanel.Controls)
			{
				panel.NoSnapshot();
			}
			eventListPanel.Visible = true;
			eventListPanel.Invalidate();
		}

		private void ChangeToSmallSize(Object sender, MouseEventArgs e)
		{
			if (LogSize == "QQVGA") return;

			LogSize = "QQVGA";
			_noSnapshot.BackgroundImage = _nosnapshot;
			_smallSize.BackgroundImage = _smallactivate;
			_mediumSize.BackgroundImage = _medium;
			_largeSize.BackgroundImage = _large;

			eventListPanel.Visible = false;
			foreach (EventLogWithSnapshot panel in eventListPanel.Controls)
			{
				panel.SmallSize();
			}
			eventListPanel.Visible = true;
			eventListPanel.Invalidate();
		}

		private void ChangeToMediumSize(Object sender, MouseEventArgs e)
		{
			if (LogSize == "HQVGA") return;

			LogSize = "HQVGA";
			_noSnapshot.BackgroundImage = _nosnapshot;
			_smallSize.BackgroundImage = _small;
			_mediumSize.BackgroundImage = _mediumactivate;
			_largeSize.BackgroundImage = _large;

			eventListPanel.Visible = false;
			foreach (EventLogWithSnapshot panel in eventListPanel.Controls)
			{
				panel.MediumSize();
			}
			eventListPanel.Visible = true;
			eventListPanel.Invalidate();
		}

		private void ChangeToLargeSize(Object sender, MouseEventArgs e)
		{
			if (LogSize == "QVGA") return;

			LogSize = "QVGA";
			_noSnapshot.BackgroundImage = _nosnapshot;
			_smallSize.BackgroundImage = _small;
			_mediumSize.BackgroundImage = _medium;
			_largeSize.BackgroundImage = _largeactivate;

			eventListPanel.Visible = false;
			foreach (EventLogWithSnapshot panel in eventListPanel.Controls)
			{
				panel.LargeSize();
			}
			eventListPanel.Visible = true;
			eventListPanel.Invalidate();
		}

		private const UInt16 MaximumConnection = 3;
		private UInt16 _connection;
		public List<EventLogWithSnapshot> QueueSearchResultPanel = new List<EventLogWithSnapshot>();
		private delegate void LoadSnapshotDelegate();
		public void QueueLoadSnapshot(EventLogWithSnapshot eventLogWithSnapshot)
		{
			if (_connection < MaximumConnection)
			{
				_connection++;

				if (QueueSearchResultPanel.Contains(eventLogWithSnapshot))
					QueueSearchResultPanel.Remove(eventLogWithSnapshot);

				LoadSnapshotDelegate loadSnapshotDelegate = eventLogWithSnapshot.LoadSnapshot;
				loadSnapshotDelegate.BeginInvoke(LoadSnapshotCallback, loadSnapshotDelegate);

				return;
			}

			if (!QueueSearchResultPanel.Contains(eventLogWithSnapshot))
				QueueSearchResultPanel.Insert(0, eventLogWithSnapshot);
		}

		private void LoadSnapshotCallback(IAsyncResult result)
		{
			((LoadSnapshotDelegate)result.AsyncState).EndInvoke(result);

			_connection--;

			if (QueueSearchResultPanel.Count > 0)
				QueueLoadSnapshot(QueueSearchResultPanel[0]);
		}


		private void EventListPanelSizeChanged(Object sender, EventArgs e)
		{
			foreach (EventLogWithSnapshot panel in eventListPanel.Controls)
			{
				panel.CheckVisible();
			}
		}

		public Boolean IsActivated;
		public void Activate()
		{
			IsActivated = true;

			foreach (EventLogWithSnapshot panel in eventListPanel.Controls)
			{
				panel.CheckVisible();
			}
		}

		public void Deactivate()
		{
			IsActivated = false;
			QueueSearchResultPanel.Clear();
		}
	}
}
