using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
	public sealed class DeviceLayoutControl : Panel
	{
		public event MouseEventHandler OnDeviceLayoutMouseDrag;
		public event MouseEventHandler OnDeviceLayoutMouseDown;
		public event MouseEventHandler OnDeviceLayoutMouseDoubleClick;
		
		public DoubleBufferPanel SubLayoutControlContainer = new DoubleBufferPanel();
		private IDeviceLayout _deviceLayout;
		public IDeviceLayout DeviceLayout
		{
			get { return _deviceLayout; }
			set
			{
				_deviceLayout = value;

				UpdateToolTips();
			}
		}

		private static readonly Image _listIcon = Resources.GetResources(Properties.Resources.list, Properties.Resources.IMGList);
		private static readonly Image _list2Icon = Resources.GetResources(Properties.Resources.list2, Properties.Resources.IMGList2);
		public DeviceLayoutControl()
		{
			Dock = DockStyle.Top;
			DoubleBuffered = true;
			AutoSize = true;

			BackColor = Color.FromArgb(98, 98, 98);
			ForeColor = Color.White;

			Padding = new Padding(0, 25, 0, 0);
			Cursor = Cursors.Hand;

			SubLayoutControlContainer.BackColor = Color.FromArgb(98, 98, 98);
			SubLayoutControlContainer.Dock = DockStyle.Fill;
			SubLayoutControlContainer.AutoSize = true;
			SubLayoutControlContainer.Padding = new Padding(0, 0, 0, 10);

			Controls.Add(SubLayoutControlContainer);

			MouseDoubleClick += DeviceLayoutControlMouseDoubleClick;
			MouseUp += DeviceLayoutControlMouseUp;
			MouseDown += DeviceLayoutControlMouseDown;

			Paint += DeviceLayoutControlPaint;
		}
		
		public void HideFolderIcon()
		{
			_displayIcon = false;
		}

		private Rectangle _switchRectangle = new Rectangle(20, 0, 20, 25);
		private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

		private Boolean _displayIcon = true;
		private void DeviceLayoutControlPaint(Object sender, PaintEventArgs e)
		{
			if (_deviceLayout == null) return;

			Graphics g = e.Graphics;

			if (_displayIcon)
				g.DrawImage((SubLayoutControlContainer.Visible ? _list2Icon : _listIcon), 24, 6);

			g.DrawString(_deviceLayout.ToString(), _font, Brushes.White, 45, 5);
		}

		private Point _position;

		private void DeviceLayoutControlMouseDown(Object sender, MouseEventArgs e)
		{
			if (_switchRectangle.Contains(e.X, e.Y) && _displayIcon)
			{
				SubLayoutControlContainer.Visible = !SubLayoutControlContainer.Visible;
				Invalidate();
				return;
			}

			_position = e.Location;
			MouseMove -= DeviceControlMouseMove;
			MouseMove += DeviceControlMouseMove;
		}

		private void DeviceLayoutControlMouseUp(Object sender, MouseEventArgs e)
		{
			MouseMove -= DeviceControlMouseMove;
			if (OnDeviceLayoutMouseDown != null)
				OnDeviceLayoutMouseDown(this, e);
		}

		private void DeviceControlMouseMove(Object sender, MouseEventArgs e)
		{
			if (e.X == _position.X && e.Y == _position.Y) return;

			if (OnDeviceLayoutMouseDrag != null)
			{
				MouseMove -= DeviceControlMouseMove;
				OnDeviceLayoutMouseDrag(this, e);
			}
		}

		private void DeviceLayoutControlMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (_switchRectangle.Contains(e.X, e.Y)) return;

			if (OnDeviceLayoutMouseDoubleClick != null)
				OnDeviceLayoutMouseDoubleClick(this, e);
		}

		public void UpdateToolTips()
		{
			if (_deviceLayout != null)
			{
				var tooltips = _deviceLayout + " " + _deviceLayout.Width + "x" + _deviceLayout.Height;

				SharedToolTips.SharedToolTip.SetToolTip(this, tooltips);
			}
			else
				SharedToolTips.SharedToolTip.SetToolTip(this, "");
		}
	}
}
