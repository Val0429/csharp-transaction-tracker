using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
	public sealed class SubLayoutControlUI2 : Label
	{
		public event MouseEventHandler OnSubLayoutMouseDrag;
		public event MouseEventHandler OnSubLayoutMouseDown;
		public event MouseEventHandler OnSubLayoutMouseDoubleClick;

		private ISubLayout _subLayout;
		public ISubLayout SubLayout{
			get { return _subLayout; }
			set
			{
				_subLayout = value;

				UpdateToolTips();
			}
		}

		private static readonly Image _record = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
		private static readonly Image _normal = Resources.GetResources(Properties.Resources.normal, Properties.Resources.IMGNormal);
		private static readonly Image _online = Resources.GetResources(Properties.Resources.online, Properties.Resources.IMGOnLine);
		private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
		private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
		private static readonly Image _bg = Resources.GetResources(Properties.Resources.devicePanelBG, Properties.Resources.IMGDevicePanelBG);

		public SubLayoutControlUI2()
		{
			Dock = DockStyle.Top;
			DoubleBuffered = true;
			Height = 31;
			BackColor = Color.FromArgb(46, 49, 55);//drag label will use it
			ForeColor = Color.DarkGray; //drag label will use it

			BackgroundImage = _bg;
			BackgroundImageLayout = ImageLayout.Center;
			Cursor = Cursors.Hand;

			MouseDoubleClick += SubLayoutControlMouseDoubleClick;
			MouseUp += SubLayoutControlMouseUp;
			MouseDown += SubLayoutControlMouseDown;

			Paint += DeviceControlPaint;
		}

		private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
		private void DeviceControlPaint(Object sender, PaintEventArgs e)
		{
			if (_subLayout == null) return;

			Graphics g = e.Graphics;

			if (_subLayout.ReadyState == ReadyState.New)
				g.DrawImage(_new, 4, 7);
			else
			{
				if (_subLayout.ReadyState == ReadyState.Modify)
					g.DrawImage(_modify, 4, 7);

				ICamera camera = null;
				var id = SubLayoutUtility.CheckSubLayoutRelativeCamera(_subLayout);
				if (id > 0 && _subLayout.Server.Device.Devices.ContainsKey(Convert.ToUInt16(id)))
				{
					camera = _subLayout.Server.Device.Devices[Convert.ToUInt16(id)] as ICamera;
				}
				//----------------------------------------------------------------------

				if (camera != null && camera.ReadyState != ReadyState.New)
				{
					switch (camera.Status)
					{
						case CameraStatus.Recording:
							g.DrawImage(_record, 24, 7);
							break;

						case CameraStatus.Streaming:
							g.DrawImage(_online, 24, 7);
							break;

						default:
							g.DrawImage(_normal, 24, 7);
							break;
					}
				}
			}

			g.DrawString(_subLayout.ToString(), _font, Brushes.DarkGray, 45, 8);
		}

		private Point _position;

		private void SubLayoutControlMouseDown(Object sender, MouseEventArgs e)
		{
			_position = e.Location;
			MouseMove -= SubLayoutControlMouseMove;
			MouseMove += SubLayoutControlMouseMove;
		}

		private void SubLayoutControlMouseUp(Object sender, MouseEventArgs e)
		{
			MouseMove -= SubLayoutControlMouseMove;
			if (OnSubLayoutMouseDown != null)
				OnSubLayoutMouseDown(this, e);
		}

		private void SubLayoutControlMouseMove(Object sender, MouseEventArgs e)
		{
			if (e.X == _position.X && e.Y == _position.Y) return;
			
			if (OnSubLayoutMouseDrag != null)
			{
				MouseMove -= SubLayoutControlMouseMove;
				OnSubLayoutMouseDrag(this, e);
			}
		}

		private void SubLayoutControlMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (OnSubLayoutMouseDoubleClick != null)
				OnSubLayoutMouseDoubleClick(this, e);
		}

		public void UpdateToolTips()
		{
			if (_subLayout != null)
			{
				SharedToolTips.SharedToolTip.SetToolTip(this, _subLayout.ToString());
			}
			else
				SharedToolTips.SharedToolTip.SetToolTip(this, "");
		}
	}
}
