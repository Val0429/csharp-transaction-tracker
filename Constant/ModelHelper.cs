using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Constant
{
    public static class ModelHelper
    {
        private static readonly IDictionary<string, string> Localization;

        static ModelHelper()
        {
            Localization = new Dictionary<String, String>
                           {
                               {"Event_MotionID", "Motion %1"},
                               {"Event_DigitalInputIDOn", "Digital Input %1 On"},
                               {"Event_DigitalInputIDOff", "Digital Input %1 Off"},
                               {"Event_DigitalOutputIDOn", "Digital Output %1 On"},
                               {"Event_DigitalOutputIDOff", "Digital Output %1 Off"},
                               {"Event_NetworkLoss", "Network Lost"},
                               {"Event_NetworkRecovery", "Network Recovery"},
                               {"Event_VideoLoss", "Video Lost"},
                               {"Event_VideoRecovery", "Video Recovery"},
                               {"Event_RecordFailed", "Record Failed"},
                               {"Event_RecordRecovery", "Record Recovery"},
                               {"Event_UserDefined", "User Defined"},
                               {"Event_ManualRecord", "Manual Record"},

                               //failover
                               {"Event_NVRFail", "NVR Fail"},
                               {"Event_FailoverStartRecord", "Failover Start Record"},
                               {
                                   "Event_FailoverDataStartSync",
                                   "Failover Recording Data Start Syncing"
                               },
                               {"Event_FailoverSyncCompleted", "Failover Sync Completed"},

                               //enhancement
                               {"Event_Panic", "Panic Button"},
                               {"Event_CrossLine", "CrossLine"},
                               {"Event_IntrusionDetection", "Intrusion Detection"},
                               {"Event_LoiteringDetection", "Loitering Detection"},
                               {"Event_ObjectCountingIn", "Object Counting In"},
                               {"Event_ObjectCountingOut", "Object Counting Out"},
                               {"Event_AudioDetection", "Audio Detection"},
                               {"Event_TamperDetection", "Tamper Detection"}
                           };
            Localizations.Update(Localization);
        }

        public static string ToLocalizationString(this EventType type, bool value, UInt16 id)
        {
            switch (type)
            {
                case EventType.Motion:
                    return Localization["Event_MotionID"].Replace("%1", id.ToString(CultureInfo.InvariantCulture));

                case EventType.DigitalInput:
                    return (value)
                        ? Localization["Event_DigitalInputIDOn"].Replace("%1", id.ToString(CultureInfo.InvariantCulture))
                        : Localization["Event_DigitalInputIDOff"].Replace("%1",
                            id.ToString(CultureInfo.InvariantCulture));

                case EventType.DigitalOutput:
                    return (value)
                        ? Localization["Event_DigitalOutputIDOn"].Replace("%1",
                            id.ToString(CultureInfo.InvariantCulture))
                        : Localization["Event_DigitalOutputIDOff"].Replace("%1",
                            id.ToString(CultureInfo.InvariantCulture));

                case EventType.CrossLine:
                    return Localization["Event_CrossLine"];

                case EventType.NetworkLoss:
                    return Localization["Event_NetworkLoss"];

                case EventType.NetworkRecovery:
                    return Localization["Event_NetworkRecovery"];

                case EventType.VideoLoss:
                    return Localization["Event_VideoLoss"];

                case EventType.VideoRecovery:
                    return Localization["Event_VideoRecovery"];

                case EventType.RecordFailed:
                    return Localization["Event_RecordFailed"];

                case EventType.RecordRecovery:
                    return Localization["Event_RecordRecovery"];

                case EventType.UserDefine:
                    return Localization["Event_UserDefined"];

                case EventType.ManualRecord:
                    return Localization["Event_ManualRecord"];

                case EventType.NVRFail:
                    return Localization["Event_NVRFail"];

                case EventType.FailoverStartRecord:
                    return Localization["Event_FailoverStartRecord"];

                case EventType.FailoverDataStartSync:
                    return Localization["Event_FailoverDataStartSync"];

                case EventType.FailoverSyncCompleted:
                    return Localization["Event_FailoverSyncCompleted"];

                case EventType.Panic:
                    return Localization["Event_Panic"];

                case EventType.IntrusionDetection:
                    return Localization["Event_IntrusionDetection"];

                case EventType.LoiteringDetection:
                    return Localization["Event_LoiteringDetection"];

                case EventType.ObjectCountingIn:
                    return Localization["Event_ObjectCountingIn"];

                case EventType.ObjectCountingOut:
                    return Localization["Event_ObjectCountingOut"];

                case EventType.AudioDetection:
                    return Localization["Event_AudioDetection"];

                case EventType.TamperDetection:
                    return Localization["Event_TamperDetection"];

                default:
                    return type.ToString(value, id);
            }
        }

        public static string ToString(this EventType type, bool value, ushort id)
        {
            switch (type)
            {
                case EventType.Motion:
                    return "Motion " + id;

                case EventType.DigitalInput:
                    return "Digital Input " + id + " " + ((value) ? "On" : "Off");

                case EventType.DigitalOutput:
                    return "Digital Output " + id + " " + ((value) ? "On" : "Off");

                case EventType.NetworkLoss:
                    return "Network Lost";

                case EventType.NetworkRecovery:
                    return "Network Recovery";

                case EventType.VideoLoss:
                    return "Video Lost";

                case EventType.VideoRecovery:
                    return "Video Recovery";

                case EventType.RecordFailed:
                    return "Record Failed";

                case EventType.RecordRecovery:
                    return "Record Recovery";

                case EventType.ManualRecord:
                    return "Manual Record";

                case EventType.UserDefine:
                    return "User Defined";
                //return "User Defined " + Id + " " + ((Value) ? "On" : "Off");

                case EventType.NVRFail:
                    return "NVR Fail";

                case EventType.FailoverStartRecord:
                    return "Failover Start Record";

                case EventType.FailoverDataStartSync:
                    return "Failover Recording Data Start Syncing";

                case EventType.FailoverSyncCompleted:
                    return "Failover Sync Completed";

                case EventType.Panic:
                    return "Panic Button";

                case EventType.CrossLine:
                    return "Cross Line";

                case EventType.IntrusionDetection:
                    return "Intrusion Detection";

                case EventType.LoiteringDetection:
                    return "Loitering Detection";

                case EventType.ObjectCountingIn:
                    return "Object Counting In";

                case EventType.ObjectCountingOut:
                    return "Object Counting Out";

                case EventType.AudioDetection:
                    return "Audio Detection";

                case EventType.TamperDetection:
                    return "Tamper Detection";

                default:
                    return type.ToString();
            }
        }
    }
}
