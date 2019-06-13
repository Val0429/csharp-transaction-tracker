using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
	public class DeviceControl : Label, IDeviceControl
	{
		public event MouseEventHandler OnDeviceMouseDrag;
		public event MouseEventHandler OnDeviceMouseDown;
		public event MouseEventHandler OnDeviceMouseDoubleClick;

		protected IDevice _device;
		public IDevice Device{
			get { return _device; }
			set
			{
				_device = value;

				UpdateToolTips();
			}
		}

		private static readonly Image _record = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
		private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
		private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);

		public DeviceControl()
		{
			Dock = DockStyle.Top;
			DoubleBuffered = true;
			Height = 25;
			Cursor = Cursors.Hand;

			BackColor = Color.FromArgb(98, 98, 98);
			ForeColor = Color.White;

			MouseDoubleClick += DeviceControlMouseDoubleClick;
			MouseUp += DeviceControlMouseUp;
			MouseDown += DeviceControlMouseDown;

			Paint += DeviceControlPaint;
		}

		protected readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

		protected virtual void DeviceControlPaint(Object sender, PaintEventArgs e)
		{
			if (_device == null) return;

			Graphics g = e.Graphics;

			if (_device.ReadyState == ReadyState.New)
				g.DrawImage(_new, 4, 7);
			else
			{
				if (_device.ReadyState == ReadyState.Modify)
					g.DrawImage(_modify, 4, 7);

				if (_device is ICamera)
				{
					if (((ICamera)_device).Status == CameraStatus.Recording)
						g.DrawImage(_record, 20, 2);
				}
			}

			g.DrawString(_device.ToString(), _font, Brushes.White, 45, 5);
		}

		private Point _position;
		protected void DeviceControlMouseDown(Object sender, MouseEventArgs e)
		{
			_position = e.Location;
			MouseMove -= DeviceControlMouseMove;
			MouseMove += DeviceControlMouseMove;
		}

		protected void DeviceControlMouseUp(Object sender, MouseEventArgs e)
		{
			MouseMove -= DeviceControlMouseMove;
			if (OnDeviceMouseDown != null)
				OnDeviceMouseDown(this, e);
		}

		private void DeviceControlMouseMove(Object sender, MouseEventArgs e)
		{
			if (e.X == _position.X && e.Y == _position.Y) return;
			
			if (OnDeviceMouseDrag != null)
			{
				MouseMove -= DeviceControlMouseMove;
				OnDeviceMouseDrag(this, e);
			}
		}

		protected void DeviceControlMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (OnDeviceMouseDoubleClick != null)
				OnDeviceMouseDoubleClick(this, e);
		}

		public void UpdateToolTips()
		{
			if (_device != null)
			{
				if (_device is ICamera)
				{
					var camera = _device as ICamera;
					try
					{
						var tooltips = _device + ((_device.Server.Id != 0) ? @" (" + _device.Server + @")" : "");

						if(!String.IsNullOrEmpty(camera.Profile.NetworkAddress))
						{
							tooltips += Environment.NewLine + camera.Profile.NetworkAddress;
						}

						if (camera.StreamConfig != null)
						{
							if (camera.StreamConfig.Compression != Compression.Off)
							{
								tooltips += Environment.NewLine + Compressions.ToDisplayString(camera.StreamConfig.Compression);
								switch (camera.StreamConfig.Compression)
								{
									case Compression.Mjpeg:
										tooltips += ", " + camera.StreamConfig.VideoQuality;
										break;

									case Compression.H264:
									case Compression.Mpeg4:
										tooltips += ", " + Bitrates.ToDisplayString(camera.StreamConfig.Bitrate);
										break;
								}

								if (camera.StreamConfig.Resolution != Resolution.NA)
									tooltips += Environment.NewLine + Resolutions.ToString(camera.StreamConfig.Resolution);
							}
						}

						if (_device.Server != null && _device.Server.Server != null)
						{
							if (camera.Model.Manufacture != "UNKNOWN")
								tooltips += Environment.NewLine + _device.Server.Server.DisplayManufactures(camera.Model.Manufacture);
						}

						SharedToolTips.SharedToolTip.SetToolTip(this, tooltips);
					}
					catch (Exception)
					{
						SharedToolTips.SharedToolTip.SetToolTip(this, _device.ToString());
					}
				}
				else
				{
					SharedToolTips.SharedToolTip.SetToolTip(this, _device.ToString());
				}
			}
			else
				SharedToolTips.SharedToolTip.SetToolTip(this, "");
		}
	}
}
