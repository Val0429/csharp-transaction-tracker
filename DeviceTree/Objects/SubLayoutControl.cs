using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
	public sealed class SubLayoutControl : Label
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

		private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
		private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);

		public SubLayoutControl()
		{
			Dock = DockStyle.Top;
			DoubleBuffered = true;
			Height = 25;
			Cursor = Cursors.Hand;

			BackColor = Color.FromArgb(98, 98, 98);
			ForeColor = Color.White;

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
				g.DrawImage(_new, 45, 4);
			else
			{
				if (_subLayout.ReadyState == ReadyState.Modify)
					g.DrawImage(_modify, 45, 4);
			}

			g.DrawString(_subLayout.ToString(), _font, Brushes.White, 60, 5);
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
