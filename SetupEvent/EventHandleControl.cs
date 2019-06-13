using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;

namespace SetupEvent
{
    public sealed partial class EventHandleControl : UserControl
    {
        public event EventHandler OnEventEditClick;

        public IServer Server;
        public ICamera Camera;

        public Boolean IsEditing;

        public void Initialize()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
        }

        private readonly Queue<EventHandlePanel> _recycleHandle = new Queue<EventHandlePanel>();

        public void GenerateViewModel()
        {
            ClearViewModel();

            if (Camera == null) return;

            containerPanel.Visible = false;

            for (UInt16 i = 1; i <= Camera.Model.NumberOfMotion; i++)
            {
                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.Motion, i));
            }

            for (UInt16 i = 1; i <= Camera.Model.NumberOfDi; i++)
            {
                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalInput, i, true));
                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalInput, i, false));
            }

            foreach (var ioPort in Camera.IOPort)
            {
                if (ioPort.Value != IOPort.Input) continue;

                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalInput, ioPort.Key, true));
                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalInput, ioPort.Key, false));
            }

            /*for (UInt16 i = 1; i <= Camera.Model.NumberOfDo; i++)
            {
                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalOutput, i, true));
                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalOutput, i, false));
            }
             

            foreach (var ioPort in Camera.IOPorts)
            {
                if (ioPort.Value != IOPort.Output) continue;

                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalOutput, ioPort.Key, true));
                AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.DigitalOutput, ioPort.Key, false));
            }
             */

            AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.VideoLoss));
            AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.VideoRecovery));
            AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.NetworkLoss));
            AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.NetworkRecovery));

            //AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.RecordFailed));
            //AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.RecordRecovery));
            //AddEventHandlePanel(Camera.EventHandling.GetEventCondition(EventType.UserDefine));

            containerPanel.Visible = true;
            containerPanel.Focus();
        }

        public void GenerateEventModel()
        {
            ClearViewModel();

            containerPanel.Visible = false;
            var fos = Server as IFOS;
            if (fos == null) return;

            AddEventHandlePanel(fos.EventHandling.GetEventCondition(EventType.NVRFail));
            AddEventHandlePanel(fos.EventHandling.GetEventCondition(EventType.FailoverStartRecord));
            AddEventHandlePanel(fos.EventHandling.GetEventCondition(EventType.FailoverDataStartSync));
            AddEventHandlePanel(fos.EventHandling.GetEventCondition(EventType.FailoverSyncCompleted));

            containerPanel.Visible = true;
            containerPanel.Focus();
        }

        private void AddEventHandlePanel(EventCondition condition)
        {
            if (condition == null) return;

            var eventHandlePanel = GetEventHandlePanel();
            eventHandlePanel.EventCondition = condition;
            if (Camera != null)
            {
                eventHandlePanel.Camera = Camera;
                eventHandlePanel.EventHandles = Camera.EventHandling[condition];
            }
            else
            {
                var fos = Server as IFOS;
                if (fos != null)
                {
                    eventHandlePanel.EventHandles = fos.EventHandling[condition];
                }
            }

            eventHandlePanel.ShowHandles();
            containerPanel.Controls.Add(eventHandlePanel);
            eventHandlePanel.BringToFront();
        }

        private EventHandlePanel GetEventHandlePanel()
        {
            if (_recycleHandle.Count > 0)
            {
                return _recycleHandle.Dequeue();
            }

            var eventHandlePanel = new EventHandlePanel
            {
                Server = Server,
            };
            eventHandlePanel.Initialize();

            eventHandlePanel.OnEventEditClick += EventHandlePanelOnEventEditClick;
            eventHandlePanel.OnHandleChange += EventHandlePanelOnHandleChange;
            return eventHandlePanel;
        }

        private void EventHandlePanelOnHandleChange(Object sender, EventArgs e)
        {
            CameraIsModify();
        }

        public EventHandlePanel EventHandlePanel;
        private void EventHandlePanelOnEventEditClick(Object sender, EventArgs e)
        {
            EventHandlePanel = (EventHandlePanel)sender;

            if (OnEventEditClick != null)
                OnEventEditClick(this, e);
        }

        public void ParseDevice()
        {
            if (Camera == null) return;
            IsEditing = false;

            GenerateViewModel();

            IsEditing = true;
        }

        public void CameraIsModify()
        {
            if (Camera.EventHandling != null)
            {
                Camera.EventHandling.ReadyState = ReadyState.Modify;
            }
                
            Server.DeviceModify(Camera);
        }

        private void ClearViewModel()
        {
            foreach (EventHandlePanel eventHandlePanel in containerPanel.Controls)
            {
                eventHandlePanel.ClearViewModel();
                eventHandlePanel.EventCondition = null;
                eventHandlePanel.EventHandles = null;

                if (!_recycleHandle.Contains(eventHandlePanel))
                    _recycleHandle.Enqueue(eventHandlePanel);
            }

            containerPanel.Controls.Clear();
        }

        public void AddHandle()
        {
            containerPanel.Controls.Clear();
            containerPanel.Controls.Add(EventHandlePanel);
            EventHandlePanel.AddHandle();
        }

        public void RemoveSelectedHandles()
        {
            foreach (EventHandlePanel eventHandlePanel in containerPanel.Controls)
            {
                eventHandlePanel.RemoveSelectedHandles();
            }
        }

        public Boolean SelectionVisible
        {
            set
            {
                foreach (EventHandlePanel eventHandlePanel in containerPanel.Controls)
                {
                    eventHandlePanel.SettingEnable = eventHandlePanel.EditVisible = !value;
                    eventHandlePanel.SelectionVisible = value;
                }
            }
        }
    }
}
