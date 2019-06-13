using System;
using DeviceConstant;

namespace Interface
{
	public class BeepEventHandle : EventHandle
	{
		public UInt16 Times = 1;
		public UInt16 Duration = 1;//Sec
		public UInt16 Interval = 1;//Sec interval between beep! <--> beep!
		
		public BeepEventHandle()
		{
			Type = HandleType.Beep;
			//Enable = false;
		}
	}

	public class AudioEventHandle : EventHandle
	{
		public String FileName = "";

		public AudioEventHandle()
		{
			Type = HandleType.Audio;
			//Enable = false;
		}
	}

	public class ExecEventHandle : EventHandle
	{
		public String FileName = "";

		public ExecEventHandle()
		{
			Type = HandleType.ExecCmd;
			//Enable = false;
		}
	}

	public class HotSpotEventHandle : EventHandle
	{
		public IDevice Device;

		public HotSpotEventHandle()
		{
			Type = HandleType.HotSpot;
			//Enable = false;
		}
	}

	public class GotoPresetEventHandle : EventHandle
	{
		public IDevice Device;
		public UInt16 PresetPoint;

		public GotoPresetEventHandle()
		{
			Type = HandleType.GoToPreset;
		}
	}

	public class PopupPlaybackEventHandle : EventHandle
	{
		public IDevice Device;

		public PopupPlaybackEventHandle()
		{
			Type = HandleType.PopupPlayback;
		}
	}

	public class PopupLiveEventHandle : EventHandle
	{
		public IDevice Device;

		public PopupLiveEventHandle()
		{
			Type = HandleType.PopupLive;
		}
	}

	public class TriggerDigitalOutEventHandle : EventHandle
	{
		public IDevice Device;
		public UInt16 DigitalOutId = 1;
		public Boolean DigitalOutStatus = true;

		public TriggerDigitalOutEventHandle()
		{
			Type = HandleType.TriggerDigitalOut;
		}
	}

	public class SendMailEventHandle : EventHandle
	{
		public String MailReceiver;
		public String Subject;
		public String Body;
		public IDevice Device;
		public Boolean AttachFile;

		public SendMailEventHandle()
		{
			Type = HandleType.SendMail;
		}
	}

	public class UploadFtpEventHandle : EventHandle
	{
		public IDevice Device;
		public String FileName;
		public Boolean TimeStamp = true; // always true

		public UploadFtpEventHandle()
		{
			Type = HandleType.UploadFtp;
		}
	}
}