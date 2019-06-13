using System;
using System.Collections.Generic;
using System.Drawing;
using Constant;

namespace DeviceConstant
{
	public class CameraEventSearchCriteria
	{
		public UInt64 StartDateTime;// = 0;
		public UInt64 EndDateTime;//= 0;
		public DateTimeSet DateTimeSet = DateTimeSet.None;// Today, Yesterday, DayBeforeYesterday, ThisWeek
		public readonly List<NVRDevice> NVRDevice = new List<NVRDevice>(); 
		public readonly List<UInt16> Device = new List<UInt16>(); 
		public readonly List<EventType> Event = new List<EventType>();
		public static Dictionary<String, String> Localization;

		public CameraEventSearchCriteria()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Event_Motion", "Motion"},
								   {"Event_DigitalInput", "Digital Input"},
								   {"Event_DigitalOutput", "Digital Output"},
								   {"Event_NetworkLoss", "Network Lost"},
								   {"Event_NetworkRecovery", "Network Recovery"},
								   {"Event_VideoLoss", "Video Lost"},
								   {"Event_VideoRecovery", "Video Recovery"},
								   {"Event_ManualRecord", "Manual Record"},
								   {"Event_Panic", "Panic Button"},
								   {"Event_CrossLine", "Cross Line"},
                                   {"Event_IntrusionDetection", "Intrusion Detection"},
								   {"Event_LoiteringDetection", "Loitering Detection"},
                                   {"Event_ObjectCountingIn", "Object Counting In"},
								   {"Event_ObjectCountingOut", "Object Counting Out"},
                                   {"Event_AudioDetection", "Audio Detection"},
								   {"Event_TamperDetection", "Tamper Detection"}
							   };
			Localizations.Update(Localization);
		}

		public static String EventTypeToLocalizationString(EventType eventType)
		{
			var eventName = eventType.ToString();
			switch (eventType)
			{
				case EventType.Motion:
					eventName = Localization["Event_Motion"];
					break;

				case EventType.DigitalInput:
					eventName = Localization["Event_DigitalInput"];
					break;

				case EventType.DigitalOutput:
					eventName = Localization["Event_DigitalOutput"];
					break;

				case EventType.NetworkLoss:
					eventName = Localization["Event_NetworkLoss"];
					break;

				case EventType.NetworkRecovery:
					eventName = Localization["Event_NetworkRecovery"];
					break;

				case EventType.VideoLoss:
					eventName = Localization["Event_VideoLoss"];
					break;

				case EventType.VideoRecovery:
					eventName = Localization["Event_VideoRecovery"];
					break;

				case EventType.ManualRecord:
					eventName = Localization["Event_ManualRecord"];
					break;

				case EventType.Panic:
					eventName = Localization["Event_Panic"];
					break;

				case EventType.CrossLine:
					eventName = Localization["Event_CrossLine"];
					break;

                case EventType.IntrusionDetection:
                    eventName = Localization["Event_IntrusionDetection"];
                    break;

                case EventType.LoiteringDetection:
                    eventName = Localization["Event_LoiteringDetection"];
                    break;

                case EventType.ObjectCountingIn:
                    eventName = Localization["Event_ObjectCountingIn"];
                    break;

                case EventType.ObjectCountingOut:
                    eventName = Localization["Event_ObjectCountingOut"];
                    break;

                case EventType.AudioDetection:
                    eventName = Localization["Event_AudioDetection"];
                    break;

                case EventType.TamperDetection:
                    eventName = Localization["Event_TamperDetection"];
                    break;
			}

			return eventName;
		}

		public static Color GetEventTypeDefaultColor(EventType eventType)
		{
			switch (eventType)
			{
				case EventType.Motion:
					return ColorTranslator.FromHtml("#84ae45");
					
				case EventType.DigitalInput:
					return ColorTranslator.FromHtml("#eb5f56");

				case EventType.DigitalOutput:
					return ColorTranslator.FromHtml("#c25a9b");

				case EventType.NetworkLoss:
					return ColorTranslator.FromHtml("#b20a46");

				case EventType.NetworkRecovery:
					return ColorTranslator.FromHtml("#ec5575");

				case EventType.VideoLoss:
					return ColorTranslator.FromHtml("#3c79b0");

				case EventType.VideoRecovery:
					return ColorTranslator.FromHtml("#3aabe5");

				case EventType.ManualRecord:
					return ColorTranslator.FromHtml("#f9af10");

				case EventType.UserDefine:
					return ColorTranslator.FromHtml("#60a8a0");

				case EventType.Panic:
					return ColorTranslator.FromHtml("#e95b24");

				case EventType.AudioIn:
					return ColorTranslator.FromHtml("#a685c1");

				case EventType.AudioOut:
					return ColorTranslator.FromHtml("#4b4abc");

				case EventType.CrossLine:
					return ColorTranslator.FromHtml("#8af731");

                case EventType.IntrusionDetection:
                    return ColorTranslator.FromHtml("#0000FF");

                case EventType.LoiteringDetection:
                    return ColorTranslator.FromHtml("#5A00E1");

                case EventType.ObjectCountingIn:
                    return ColorTranslator.FromHtml("#FF78C7");

                case EventType.ObjectCountingOut:
                    return ColorTranslator.FromHtml("#870043");

                case EventType.AudioDetection:
                    return ColorTranslator.FromHtml("#d7ac00");

                case EventType.TamperDetection:
                    return ColorTranslator.FromHtml("#876500");

                case EventType.SDRecord:
                    return ColorTranslator.FromHtml("#383a46");//383a46

                case EventType.ArchiveServerRecord:
                    return ColorTranslator.FromHtml("#26264d");//383a46

				case EventType.PlaybackDownload:
					return Color.FromArgb(99, 101, 110);
			}

			return Color.Pink;
		}
	}
	//-------------------------------------------------------------------
	public class EventCount
	{
        public EventType EventType { get; set; }
        public Color Color { get; set; }
        public Int32 Count { get; set; }
		public String DateTime { get; set; } //yyyy-MM-DD
	}

	public class NVRDevice
	{
		public UInt16 NVRId;
		public UInt16 DeviceId;
	}
}