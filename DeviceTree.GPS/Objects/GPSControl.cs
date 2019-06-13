using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.GPS.Objects
{
	public class GPSControl : DeviceControl
	{
		//private readonly Image _gps;
		private readonly Image _fireEngine_offline ;
		private readonly Image _fireEngine_online ; 
		private readonly Image _fireFighter_offline ;
		private readonly Image _fireFighter_online;

		public INVR NVR;

		public GPSControl()
		{
			//_gps = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
			_fireEngine_offline = Resources.GetResources(Properties.Resources.fireEngine_offline, Properties.Resources.IMGFireEngine_offline);
			_fireFighter_offline = Resources.GetResources(Properties.Resources.fireFighter_offline, Properties.Resources.IMGFireFighter_offline);
			_fireEngine_online = Resources.GetResources(Properties.Resources.fireEngine_online, Properties.Resources.IMGFireEngine_online);
			_fireFighter_online = Resources.GetResources(Properties.Resources.fireFighter_online, Properties.Resources.IMGFireFighter_online);

			Dock = DockStyle.Top;
			DoubleBuffered = true;
			Height = 25;
			Cursor = Cursors.Hand;
			BackColor = ColorTranslator.FromHtml("#626262");
			ForeColor = Color.White;

			MouseDoubleClick += DeviceControlMouseDoubleClick;
			MouseUp += DeviceControlMouseUp;
			MouseDown += DeviceControlMouseDown;

			Paint += DeviceControlPaint;
		}

		protected override void DeviceControlPaint(Object sender, PaintEventArgs e)
		{
			if (_device == null) return;

			Graphics g = e.Graphics;

			if (((ICamera)NVR.Device.FindDeviceById(_device.Id)).Status == CameraStatus.Recording)
			{
				switch ( _device.GPSInfo.ModelHost)
				{
					case ModelHost.Vehicle:
						g.DrawImage(_fireEngine_online, 10, 0);
						break;
					case ModelHost.Personal :
						g.DrawImage(_fireFighter_online, 10, 0);
						break;
					default:
						break;

				}
			}
			else
			{
				switch (_device.GPSInfo.ModelHost)
				{
					case ModelHost.Vehicle:
						g.DrawImage(_fireEngine_offline, 10, 0);
						break;
					case ModelHost.Personal:
						g.DrawImage(_fireFighter_offline, 10, 0);
						break;
					default:
						break;
				}

			}

			g.DrawString(_device.ToString(), _font, Brushes.White, 45, 5);
		}
	}
}
