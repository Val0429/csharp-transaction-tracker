using System.Windows.Forms;
using DeviceConstant;

namespace VideoMonitor
{
	public partial class VideoMonitor
	{
		public void KeyboardPress(Keys keyData)
		{
			if(ActivateVideoWindow == null) return;
			if(ActivateVideoWindow.Camera == null) return;
			if(ActivateVideoWindow.PlayMode != PlayMode.Playback1X && ActivateVideoWindow.PlayMode != PlayMode.GotoTimestamp) return;
			
			switch (keyData)
			{
				case Keys.Add:
					ActivateVideoWindow.Viewer.AdjustBrightness += 10;
					break;

				case Keys.Subtract:
					ActivateVideoWindow.Viewer.AdjustBrightness -= 10;
					break;
			}
		}
	}
}
