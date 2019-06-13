using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Constant;
using Constant.Serialization;
using Constant.Utility;
using Interface;

namespace EventPanel
{
    public class EventPriority
    {
        [XmlAttribute("type")]
        public EventType Type { get; set; }

        [XmlAttribute("priority")]
        public int Priority { get; set; }
    }

    public class EventPriorityCollection
    {
        private EventPriority[] _items;

        [XmlElement("Event")]
        public EventPriority[] Items
        {
            get { return _items ?? (_items = new EventPriority[] { }); }
            set { _items = value; }
        }
    }

    public class EventViewModel
    {
        public EventViewModel(ICameraEvent cameraEvent)
        {
            CameraEvent = cameraEvent;
        }


        public ICameraEvent CameraEvent { get; set; }

        public bool Acknowledged
        {
            get { return _acknowledged; }
            set
            {
                if (_acknowledged != value)
                {
                    _acknowledged = value;

                    if (_acknowledged)
                    {
                        OnEventAcknowledged(EventArgs.Empty);
                    }
                }
            }
        }

        private bool _acknowledged;


        public event EventHandler EventAcknowledged;

        private void OnEventAcknowledged(EventArgs e)
        {
            var handler = EventAcknowledged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    static class EventPriorityHelper
    {
        private static readonly EventPriority[] EventPriorities;

        static EventPriorityHelper()
        {
            try
            {
                var file = Path.Combine(GenericUtility.GetWorkingDirectory(), "EventPriority.config");
                if (File.Exists(file))
                {
                    var xml = File.ReadAllText(file);
                    var collection = xml.Deserialize<EventPriorityCollection>();
                    EventPriorities = collection.Items;
                }
                else
                {
                    EventPriorities = new EventPriority[] { };
                }
            }
            catch (System.Exception ex)
            {
                EventPriorities = new EventPriority[] { };

                Debug.WriteLine(ex);
            }
        }


        public static int GetPriority(this EventLog eventLogControl)
        {
            return eventLogControl.CameraEvent.GetPriority();
        }

        public static int GetPriority(this PriorityEventLog eventLogControl)
        {
            return eventLogControl.CameraEvent.GetPriority();
        }

        public static int GetPriority(this ICameraEvent cameraEvent)
        {
            var eventPriority = EventPriorities.FirstOrDefault(p => p.Type == cameraEvent.Type);

            return eventPriority != null ? eventPriority.Priority : 1;
        }
    }
}