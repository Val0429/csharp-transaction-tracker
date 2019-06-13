using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;

namespace VideoMonitor
{
	public partial class VideoMenu
	{
		private Button _reconnectButton;
		protected Button _disconnectButton;

		private static readonly Image _reconnect = Resources.GetResources(Properties.Resources.reconnect, Properties.Resources.IMGReconnect);
		private static readonly Image _disconnect = Resources.GetResources(Properties.Resources.disconnect, Properties.Resources.IMGDisconnect);

		private void ReconnectButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if (VideoWindow == null || VideoWindow.Camera == null) return;

			Server.WriteOperationLog("Device %1 Reconnect".Replace("%1", VideoWindow.Camera.Id.ToString()));
			VideoWindow.Reconnect();
		}

		protected void UpdateReconnectButton()
		{
			if (_reconnectButton == null || VideoWindow.Camera == null) return;

			if (VideoWindow.PlayMode == PlayMode.LiveStreaming)
			{
				SetButtonPosition(_reconnectButton);
				Controls.Add(_reconnectButton);
				_count++;
			}
			else
			{
				Controls.Remove(_reconnectButton);
			}
		}
	}
}