using System;
using System.Collections.Generic;
using Constant;
using Constant.Serialization;
using Interface;

namespace EventPanel
{
    public interface IEventListener
    {
        void Pause();
        void Resume();

        event EventHandler<EventArgs<String>> OnPreDefineEvent;
        event EventHandler<EventArgs<String>> OnUserDefineEvent;

        event Action<ICameraEvent> OnEventReceived;
    }

    class NvrEventListener : ServerEventListener
    {
        public NvrEventListener(INVR nvr)
        {
            nvr.OnEventReceive += ServerOnEventReceive;
        }
    }

    class PtsEventListener : ServerEventListener
    {
        public PtsEventListener(IPTS pts)
        {
            pts.OnEventReceive += ServerOnEventReceive;
        }
    }

    class ServerEventListener : IEventListener
    {
        private bool _pauseReceiveEvent;


        protected void ServerOnEventReceive(object sender, EventArgs<List<ICameraEvent>> e)
        {
            if (_pauseReceiveEvent) return;
            if (e.Value == null) return;

            foreach (var cameraEvent in e.Value)
            {
                RaiseOnEventReceive(cameraEvent);

                if (cameraEvent.Type == EventType.UserDefine)
                {
                    if (OnUserDefineEvent != null && cameraEvent.Device != null)
                    {
                        var str = EventTriggerXml(cameraEvent);
                        OnUserDefineEvent(this, new EventArgs<String>(str));
                    }
                }
                else
                {
                    // NOTE: RecordFail event doesn't carry the 'Device'
                    if (OnPreDefineEvent != null && cameraEvent.Device != null)
                    {
                        var str = EventTriggerXml(cameraEvent);
                        OnPreDefineEvent(this, new EventArgs<String>(str));
                    }
                }
            }
        }

        public void Pause()
        {
            _pauseReceiveEvent = true;
        }

        public void Resume()
        {
            _pauseReceiveEvent = false;
        }

        //<Event>
        //    <DeviceID>4</DeviceID>
        //    <Type>Motion</Type>
        //    <LocalTime>1310457814548</LocalTime>
        //    <DeviceTime>1310457814548</DeviceTime>
        //    <Count>2</Count>
        //    <Status id="1" trigger="1" value="1">Hello</Status>
        //    <Status id="3" trigger="1" value="1">Hello</Status>
        //</Event>
        protected static String EventTriggerXml(ICameraEvent cameraEvent)
        {
            // <XML>
            //    <DeviceID>1</DeviceID>
            //    <Status id="1">123</Status>
            // </XML>

            var msg = new UserDefinedMsg()
                      {
                          DeviceId = cameraEvent.Device.Id,
                          Msg = new UserDefinedMsg.Message()
                                  {
                                      EventId = cameraEvent.Id,
                                      Value = cameraEvent.Status
                                  }
                      };

            var xml = msg.Serialize();

            return xml;
        }


        // Events
        public event EventHandler<EventArgs<string>> OnPreDefineEvent;

        public event EventHandler<EventArgs<string>> OnUserDefineEvent;

        public event Action<ICameraEvent> OnEventReceived;

        protected void RaiseOnEventReceive(ICameraEvent cameraEvent)
        {
            if (OnEventReceived != null)
            {
                OnEventReceived(cameraEvent);
            }
        }
    }
}