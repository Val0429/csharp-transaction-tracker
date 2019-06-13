using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using Interface;


namespace DeviceTree.Sort
{
	public partial class DeviceTreeSort : DeviceTree
	{
		public override event EventHandler<EventArgs<IDeviceGroup>> OnGroupDoubleClick;
		public override event EventHandler<EventArgs<IDevice>> OnDeviceDoubleClick;
		public override event EventHandler<EventArgs<Object>> OnDragStart;

		private ComboBox _sortOptions;
		private new ListViewSort _view;

		public new Dictionary<String, ListViewSort> ViewList = new Dictionary<String, ListViewSort>();
		public override void InitialzeViewList()
		{
			var view = new ListViewSort
			{
				NVR = NVR,
			};

			view.OnGroupMouseDown += ViewModelPanelMouseDown;
			view.OnGroupMouseDrag += ViewModelPanelGroupMouseDrag;
			view.OnGroupMouseDoubleClick += ViewModelPanelGroupMouseDoubleClick;

			view.OnDeviceMouseDown += ViewModelPanelMouseDown;
			view.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
			view.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

			view.Name = "list";
			view.ViewModelPanel = viewModelPanel;

			ViewList.Add(view.Name, view);

			if (_view == null)
				_view = ViewList["list"];
		}

		public override void ViewModelPanelMouseDown(Object sender, MouseEventArgs e)
		{
			_view.ViewModelPanel.Focus();
		}

		public override void ViewModelPanelDeviceMouseDrag(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as DeviceControl;
			if (control != null && OnDragStart != null)
			{
				DragDropLabel.ForeColor = control.ForeColor;
				DragDropLabel.BackColor = control.BackColor;

				//control.Device.DeviceType = DeviceType.Device;
				DragDropLabel.Text = ((DeviceControl)sender).Device.ToString();

				OnDragStart(this, new EventArgs<Object>(control.Device));
			}
		}

		public override void ViewModelPanelGroupMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as GroupControl;
			if (control != null && OnGroupDoubleClick != null)
			{
				OnGroupDoubleClick(this, new EventArgs<IDeviceGroup>(control.DeviceGroup));
			}
		}

		public override void ViewModelPanelDeviceMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as DeviceControl;
			if (control != null && OnDeviceDoubleClick != null)
			{
				OnDeviceDoubleClick(this, new EventArgs<IDevice>(control.Device));
			}
		}

		public void AddSortOptionIcon()
		{
			var sortPanel = new Panel
			{
				Dock = DockStyle.Right,
				Size = new Size(80, PanelTitleBar.Size.Height),
				BackColor = Color.Transparent
			};

			_sortOptions = new ComboBox
			{
				Cursor = Cursors.Hand,
				Size = new Size(72, 25),
				DropDownStyle = ComboBoxStyle.DropDownList,
				Location = new Point(0, 4)
			};

			_sortOptions.SelectedIndexChanged += SortOptionsSelectedIndexChanged;
			_sortOptions.Items.Add("ID");
			_sortOptions.Items.Add("NAME");
			_sortOptions.Items.Add("GROUP");
			_sortOptions.Items.Add("STATUS");
			_sortOptions.SelectedIndex = 0;

			sortPanel.Controls.Add(_sortOptions);
			PanelTitleBar.Controls.Add(sortPanel);
		}

		private String _sortMode = "GROUP";
		public override String SortMode
		{
			get
			{
				return _sortMode;
			}
			set
			{
				_sortMode = value;
				if (_view == null) return;

				if (CMS != null)
					_view.UpdateNVRView(_sortMode);
				else
					_view.UpdateView(_sortMode);
			}
		}

		void SortOptionsSelectedIndexChanged(object sender, EventArgs e)
		{
			var obj = sender as ComboBox;

			if (obj != null)
				SortMode = obj.SelectedItem.ToString();
		}

		public override void Activate()
		{
			if (!Server.Configure.IsPatrol && PatrolButton != null)
				ToolTip.SetToolTip(PatrolButton, Localization["DeviceTree_EnablePatrol"].Replace("{SEC}", Server.Configure.PatrolInterval.ToString()));

			if (CMS != null)
				_view.UpdateNVRView(_sortOptions.SelectedItem.ToString());
			else
				_view.UpdateView(_sortOptions.SelectedItem.ToString());
		}
	}
}
