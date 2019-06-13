using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Constant;
using Interface;

namespace GPSTree.Objects
{
	public sealed class GPSControl : Label
	{
		public event MouseEventHandler OnDeviceMouseDrag;
		public event MouseEventHandler OnDeviceMouseDoubleClick;

		private IDevice _device;
		public IDevice Device
		{
			get { return _device; }
			set
			{
				_device = value;

				if (_device != null)
				{
					if (_device is ICamera)
					{
						_toolTip.SetToolTip(this, _device + Environment.NewLine
							+ ((ICamera)_device).Profile.NetworkAddress + Environment.NewLine
							+ Compressions.ToString(((ICamera)_device).StreamConfig.Compression) + Environment.NewLine
							+ Resolutions.ToString(((ICamera)_device).StreamConfig.Resolution));
					}
					else
					{
						_toolTip.SetToolTip(this, _device.ToString());
					}
				}
				else
					_toolTip.SetToolTip(this, "");
			}
		}
		private readonly ToolTip _toolTip = new ToolTip();

		public GPSControl()
		{
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

		private readonly Image _gps = Properties.Resources.gps;
		private readonly Image _car = Properties.Resources.car;
		private readonly Image _people = Properties.Resources.people;
		
		private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
		private void DeviceControlPaint(Object sender, PaintEventArgs e)
		{
			if (_device == null) return;

			Graphics g = e.Graphics;

//			if (((ICamera)_device).Status == CameraStatus.Recording)
			if (_device.Id <= 2)
				g.DrawImage(_car, 10, 0);
			else
				g.DrawImage(_people, 10, 0);

			g.DrawString(_device.ToString(), _font, Brushes.White, 45, 5);
		}

		private Int32 _positionX;
		private void DeviceControlMouseDown(Object sender, MouseEventArgs e)
		{
			_positionX = e.X;
			MouseMove -= DeviceControlMouseMove;
			MouseMove += DeviceControlMouseMove;
		}

		private void DeviceControlMouseUp(Object sender, MouseEventArgs e)
		{
			MouseMove -= DeviceControlMouseMove;
		}

		private void DeviceControlMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (OnDeviceMouseDoubleClick != null)
				OnDeviceMouseDoubleClick(this, e);
		}

		private void DeviceControlMouseMove(Object sender, MouseEventArgs e)
		{
			if (e.X == _positionX) return;
			if (OnDeviceMouseDrag != null)
			{
				MouseMove -= DeviceControlMouseMove;
				OnDeviceMouseDrag(this, e);
			}
		}				
	}
}
