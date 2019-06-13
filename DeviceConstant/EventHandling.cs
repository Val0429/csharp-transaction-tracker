using System;
using System.Collections.Generic;
using Constant;

namespace DeviceConstant
{
    public class EventHandling : Dictionary<EventCondition, List<EventHandle>>
    {
        public ReadyState ReadyState = ReadyState.New;



        public EventHandling()
        {
        }

        public void SetDefaultEventHandling(CameraModel model)
        {
            Clear();
            switch (model.Manufacture)
            {
                case "iSapSolution":
                case "ONVIF":
                case "Kedacom":
                case "Customization":
                    break;

                default:
                    Add(new EventCondition { CameraEvent = { Type = EventType.NetworkLoss, } }, new List<EventHandle>());
                    Add(new EventCondition { CameraEvent = { Type = EventType.NetworkRecovery, Value = false } }, new List<EventHandle>());
                    break;
            }

            switch (model.Manufacture)
            {
                case "Axis":
                case "VIVOTEK":
                case "Brickcom":
                    Add(new EventCondition { CameraEvent = { Type = EventType.CrossLine, } }, new List<EventHandle>());
                    break;
            }

            for (UInt16 i = 1; i <= model.NumberOfMotion; i++)
            {
                Add(new EventCondition { CameraEvent = { Type = EventType.Motion, Id = i, } }, new List<EventHandle>());
            }

            for (UInt16 i = 1; i <= model.NumberOfDi; i++)
            {
                Add(new EventCondition { Interval = 0, CameraEvent = { Type = EventType.DigitalInput, Id = i, Value = true, } }, new List<EventHandle>());

                Add(new EventCondition { Interval = 0, CameraEvent = { Type = EventType.DigitalInput, Id = i, Value = false, } }, new List<EventHandle>());
            }

            if (model.IOPorts.Count > 0)
            {
                foreach (var ioPort in model.IOPorts)
                {
                    Add(new EventCondition { Interval = 0, CameraEvent = { Type = EventType.DigitalInput, Id = ioPort.Key, Value = true, } }, new List<EventHandle>());

                    Add(new EventCondition { Interval = 0, CameraEvent = { Type = EventType.DigitalInput, Id = ioPort.Key, Value = false, } }, new List<EventHandle>());
                }
            }

            if (model.Type == "VideoServer")
            {
                Add(new EventCondition { CameraEvent = { Type = EventType.VideoLoss, } }, new List<EventHandle>());

                Add(new EventCondition { CameraEvent = { Type = EventType.VideoRecovery, } }, new List<EventHandle>());
            }

            Add(new EventCondition { CameraEvent = { Type = EventType.UserDefine, Value = true, } }, new List<EventHandle>());

            Add(new EventCondition { CameraEvent = { Type = EventType.Panic, } }, new List<EventHandle>());

            Add(new EventCondition { CameraEvent = { Type = EventType.IntrusionDetection, } }, new List<EventHandle>());

            Add(new EventCondition { CameraEvent = { Type = EventType.LoiteringDetection, } }, new List<EventHandle>());

            Add(new EventCondition { CameraEvent = { Type = EventType.ObjectCountingIn, } }, new List<EventHandle>());

            Add(new EventCondition { CameraEvent = { Type = EventType.ObjectCountingOut, } }, new List<EventHandle>());

            Add(new EventCondition { CameraEvent = { Type = EventType.AudioDetection, } }, new List<EventHandle>());

            Add(new EventCondition { CameraEvent = { Type = EventType.TamperDetection, } }, new List<EventHandle>());
        }

        public void SetEventHandlingViaCameraModel(CameraModel model)
        {
            switch (model.Manufacture)
            {
                case "iSapSolution":
                case "ONVIF":
                case "Kedacom":
                case "Customization":
                    var condition = GetEventCondition(EventType.NetworkLoss);
                    if (condition != null)
                        Remove(condition);

                    condition = GetEventCondition(EventType.NetworkRecovery);
                    if (condition != null)
                        Remove(condition);
                    break;

                default:
                    if (GetEventCondition(EventType.NetworkLoss) == null)
                    {
                        Add(new EventCondition
                        {
                            CameraEvent =
                            {
                                Type = EventType.NetworkLoss,
                            }
                        }, new List<EventHandle>());
                    }

                    if (GetEventCondition(EventType.NetworkRecovery) == null)
                    {
                        Add(new EventCondition
                        {
                            CameraEvent =
                            {
                                Type = EventType.NetworkRecovery,
                                Value = false
                            }
                        }, new List<EventHandle>());
                    }
                    break;
            }

            switch (model.Manufacture)
            {
                case "Axis":
                case "VIVOTEK":
                    if (GetEventCondition(EventType.CrossLine) == null)
                    {
                        Add(new EventCondition
                        {
                            CameraEvent =
                            {
                                Type = EventType.CrossLine,
                            }
                        }, new List<EventHandle>());
                    }
                    break;

                default:
                    var condition = GetEventCondition(EventType.CrossLine);
                    if (condition != null)
                        Remove(condition);
                    break;
            }

            for (UInt16 i = 1; i <= model.NumberOfMotion; i++)
            {
                if (GetEventCondition(EventType.Motion, i) == null)
                {
                    Add(new EventCondition
                    {
                        CameraEvent =
                        {
                            Type = EventType.Motion,
                            Id = i,
                        }
                    }, new List<EventHandle>());
                }
            }

            for (UInt16 i = Convert.ToUInt16(model.NumberOfMotion + 1); i <= 10; i++)
            {
                var condition = GetEventCondition(EventType.Motion, i);
                if (condition != null)
                    Remove(condition);
            }

            //--------------------------------------------------------------------

            for (UInt16 i = 1; i <= model.NumberOfDi; i++)
            {
                if (GetEventCondition(EventType.DigitalInput, i, true) == null)
                {
                    Add(new EventCondition
                    {
                        Interval = 0,
                        CameraEvent =
                        {
                            Type = EventType.DigitalInput,
                            Id = i,
                            Value = true
                        }
                    }, new List<EventHandle>());
                }
                if (GetEventCondition(EventType.DigitalInput, i, false) == null)
                {
                    Add(new EventCondition
                    {
                        Interval = 0,
                        CameraEvent =
                        {
                            Type = EventType.DigitalInput,
                            Id = i,
                            Value = false
                        }
                    }, new List<EventHandle>());
                }
            }

            for (UInt16 i = Convert.ToUInt16(model.NumberOfDi + 1); i <= 4; i++)
            {
                var condition = GetEventCondition(EventType.DigitalInput, i, true);
                if (condition != null)
                    Remove(condition);

                condition = GetEventCondition(EventType.DigitalInput, i, false);
                if (condition != null)
                    Remove(condition);
            }

            //--------------------------------------------------------------------
            //Do dont need event handle

            if (model.IOPorts.Count > 0)
            {
                foreach (var ioPort in model.IOPorts)
                {
                    if (GetEventCondition(EventType.DigitalInput, ioPort.Key, true) == null)
                    {
                        Add(new EventCondition
                        {
                            Interval = 0,
                            CameraEvent =
                            {
                                Type = EventType.DigitalInput,
                                Id = ioPort.Key,
                                Value = true,
                            }
                        }, new List<EventHandle>());
                    }

                    if (GetEventCondition(EventType.DigitalInput, ioPort.Key, false) == null)
                    {
                        Add(new EventCondition
                        {
                            Interval = 0,
                            CameraEvent =
                            {
                                Type = EventType.DigitalInput,
                                Id = ioPort.Key,
                                Value = false,
                            }
                        }, new List<EventHandle>());
                    }
                }
            }

            //--------------------------------------------------------------------

            if (model.Type == "VideoServer")
            {
                if (GetEventCondition(EventType.VideoLoss) == null)
                {
                    Add(new EventCondition
                    {
                        CameraEvent =
                        {
                            Type = EventType.VideoLoss,
                        }
                    }, new List<EventHandle>());
                }

                if (GetEventCondition(EventType.VideoRecovery) == null)
                {
                    Add(new EventCondition
                    {
                        CameraEvent =
                        {
                            Type = EventType.VideoRecovery,
                        }
                    }, new List<EventHandle>());
                }
            }
            else
            {
                var condition = GetEventCondition(EventType.VideoLoss);
                if (condition != null)
                    Remove(condition);

                condition = GetEventCondition(EventType.VideoRecovery);
                if (condition != null)
                    Remove(condition);
            }
        }

        public void SetDefaultFailoverEventHandling()
        {
            Add(new EventCondition
            {
                Interval = 0,
                CameraEvent =
                {
                    Type = EventType.NVRFail,
                }
            }, new List<EventHandle>());

            Add(new EventCondition
            {
                Interval = 0,
                CameraEvent =
                {
                    Type = EventType.FailoverStartRecord,
                }
            }, new List<EventHandle>());

            Add(new EventCondition
            {
                Interval = 0,
                CameraEvent =
                {
                    Type = EventType.FailoverDataStartSync,
                }
            }, new List<EventHandle>());

            Add(new EventCondition
            {
                Interval = 0,
                CameraEvent =
                {
                    Type = EventType.FailoverSyncCompleted,
                }
            }, new List<EventHandle>());
        }

        public List<EventHandle> GetEventHandleViaCameraEvent(EventType type, UInt16 id, Boolean value)
        {
            EventCondition condition = null;
            switch (type)
            {
                case EventType.DigitalInput:
                case EventType.DigitalOutput:
                case EventType.UserDefine:
                case EventType.ObjectCountingIn:
                case EventType.ObjectCountingOut:
                    condition = GetEventCondition(type, id, value);
                    break;

                case EventType.Motion:
                    condition = GetEventCondition(type, id);
                    break;

                case EventType.VideoLoss:
                case EventType.VideoRecovery:
                case EventType.NetworkLoss:
                case EventType.NetworkRecovery:
                case EventType.NVRFail:
                case EventType.FailoverStartRecord:
                case EventType.FailoverDataStartSync:
                case EventType.FailoverSyncCompleted:
                case EventType.Panic:
                case EventType.CrossLine:
                case EventType.IntrusionDetection:
                case EventType.LoiteringDetection:
                case EventType.AudioDetection:
                case EventType.TamperDetection:
                    condition = GetEventCondition(type);
                    break;
            }

            if (condition == null) return null;

            return (ContainsKey(condition)) ? this[condition] : null;
        }

        public EventCondition GetEventCondition(EventType type, UInt16 id, Boolean value)
        {
            foreach (var condition in Keys)
            {
                if (condition.CameraEvent.Type == type && condition.CameraEvent.Id == id && condition.CameraEvent.Value == value)
                    return condition;
            }

            return null;
        }

        public EventCondition GetEventCondition(EventType type, UInt16 id)
        {
            foreach (var condition in Keys)
            {
                if (condition.CameraEvent.Type == type && condition.CameraEvent.Id == id)
                    return condition;
            }

            return null;
        }

        public EventCondition GetEventCondition(EventType type)
        {
            foreach (var condition in Keys)
            {
                if (condition.CameraEvent.Type == type)
                    return condition;
            }

            return null;
        }

        public void ClearEventHandling()
        {
            foreach (List<EventHandle> eventHandles in Values)
            {
                eventHandles.Clear();
            }
        }
    }

    public class EventCondition
    {
        public Boolean Enable = true;
        public CameraEvent CameraEvent = new CameraEvent();
        public UInt16 Interval = 1;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public EventCondition()
        {
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(Object sender, System.Timers.ElapsedEventArgs e)
        {
            Enable = true;
            _timer.Enabled = false;
        }

        public void CoolDownTriggerInterval()
        {
            //interval is 0 <- no need to cooldown
            if (Interval == 0) return;
            if (_timer.Enabled) return;

            Enable = false;
            _timer.Enabled = false;
            _timer.Interval = Interval * 1000;
            _timer.Enabled = true;
        }
    }

    public enum HandleType : ushort
    {
        HotSpot,
        ExecCmd,
        Audio,
        Beep,
        GoToPreset,
        PopupPlayback,
        TriggerDigitalOut,
        SendMail,
        UploadFtp,
        PopupLive,
    }
}
