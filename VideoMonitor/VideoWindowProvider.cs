using System.Collections.Generic;
using System.Windows.Forms;
using DeviceConstant;

namespace VideoMonitor
{
	public class VideoWindowProvider
	{
		private readonly static Queue<VideoWindow> StoredVideoWindow = new Queue<VideoWindow>();
		private readonly static List<VideoWindow> UsingVideoWindow = new List<VideoWindow>();


		public static VideoWindow RegistVideoWindow()
		{
			VideoWindow videoWindow;
			if (StoredVideoWindow.Count > 0)
			{
				videoWindow = StoredVideoWindow.Dequeue();
				videoWindow.RegisterViewer();
			}
			else
			{
				videoWindow = new VideoWindow();
			}
			UsingVideoWindow.Add(videoWindow);
			

			return videoWindow;
		}

		public static void UnregisterVideoWindow(VideoWindow videoWindow)
		{
			if (videoWindow == null) return;

			UsingVideoWindow.Remove(videoWindow);

			videoWindow.DisplayTitleBar = false;
			videoWindow.Dock = DockStyle.None;
			videoWindow.Active = false;
			videoWindow.Stretch = true;
			videoWindow.Dewarp = false;
		    videoWindow.DewarpType = -1;
			videoWindow.Viewer.AudioIn = false;
			videoWindow.Viewer.PtzMode = PTZMode.Disable;
			videoWindow.Parent = null;
			videoWindow.Server = null;
			videoWindow.Visible = true;
			videoWindow.Reset();
			videoWindow.ClearAllEventHandler();

			if (!StoredVideoWindow.Contains(videoWindow))
				StoredVideoWindow.Enqueue(videoWindow);
		}
	}
}
