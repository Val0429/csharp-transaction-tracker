
namespace ServerProfile.Plugin
{
	public class BeepEventHandle : ServerProfile.BeepEventHandle
	{
		public BeepEventHandle()
		{
			base.Enable = true;
		}
	}

	public class AudioEventHandle : ServerProfile.AudioEventHandle
	{
		public AudioEventHandle()
		{
			base.Enable = true;
		}
	}

	public class ExecEventHandle : ServerProfile.ExecEventHandle
	{
		public ExecEventHandle()
		{
			base.Enable = true;
		}
	}

	public class HotSpotEventHandle : ServerProfile.HotSpotEventHandle
	{
		public HotSpotEventHandle()
		{
			base.Enable = true;
		}
	}

	public class PopupPlaybackEventHandle : ServerProfile.PopupPlaybackEventHandle
	{
		public PopupPlaybackEventHandle()
		{
			base.Enable = true;
		}
	}
}
