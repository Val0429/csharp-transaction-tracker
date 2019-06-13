using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DeviceTree.Objects;
using Interface;

namespace DeviceTree.View
{
	public class DeviceLayoutListView : ViewBase
	{
		public virtual event MouseEventHandler OnDeviceLayoutMouseDrag;
		public virtual event MouseEventHandler OnDeviceLayoutMouseDown;
		public virtual event MouseEventHandler OnDeviceLayoutMouseDoubleClick;

		public virtual event MouseEventHandler OnSubLayoutMouseDrag;
		public virtual event MouseEventHandler OnSubLayoutMouseDown;
		public virtual event MouseEventHandler OnSubLayoutMouseDoubleClick;
		
		public INVR NVR;
		public override void GenerateViewModel()
		{
			var sortResult = DeviceLayoutsSortById();

			var layoutControls = new List<DeviceLayoutControl>();
			foreach (IDeviceLayout deviceLayout in sortResult)
			{
				//if (deviceLayout.SubLayouts.Count(subLayout => subLayout.Value != null) == 0) continue;
				if (deviceLayout.Items.Count(device => device != null) == 0) continue;

				var layoutControl = GetDeviceLayoutControl();

				layoutControl.DeviceLayout = deviceLayout;
				layoutControls.Add(layoutControl);

				var list = deviceLayout.SubLayouts.Values.Where(obj => obj != null).ToList();
				list.Sort((x, y) => (y.Id - x.Id));
				var deviceControls = new List<SubLayoutControl>();

				foreach (ISubLayout layout in list)
				{
					if (layout == null) continue;
					if (layout.Id == 99) continue;

					var deviceControl = GetSubLayoutControl();

					deviceControl.SubLayout = layout;
					deviceControls.Add(deviceControl);
				}
				if (deviceControls.Count > 0)
					layoutControl.SubLayoutControlContainer.Controls.AddRange(deviceControls.ToArray());
			}
			if (layoutControls.Count > 0)
			{
				ViewModelPanel.Controls.AddRange(layoutControls.ToArray());
				layoutControls.Clear();
			}
		}

		protected List<IDeviceLayout> DeviceLayoutsSortById()
		{
			var sortResult = new List<IDeviceLayout>(NVR.Device.DeviceLayouts.Values);

			//reverse
			sortResult.Sort((x, y) => (y.Id - x.Id));

			return sortResult;
		}

		public override void UpdateView()
		{
			UpdateView("ID");
		}

		public override void UpdateView(String sort)
		{
			if (NVR == null || NVR.Device.DeviceLayouts == null) return;

			ClearViewModel();

			GenerateViewModel();
		}

		public override void ClearViewModel()
		{
			if (ViewModelPanel == null) return;

			foreach (DeviceLayoutControl layoutControl in ViewModelPanel.Controls)
			{
				if (!RecycleLayout.Contains(layoutControl))
					RecycleLayout.Enqueue(layoutControl);

				foreach (SubLayoutControl deviceControl in layoutControl.SubLayoutControlContainer.Controls)
				{
					if (!RecycleSubLayout.Contains(deviceControl))
					{
						deviceControl.SubLayout = null;
						RecycleSubLayout.Enqueue(deviceControl);
					}
				}

				layoutControl.SubLayoutControlContainer.Controls.Clear();
			}

			ViewModelPanel.Controls.Clear();
		}

		public override void UpdateToolTips()
		{
			foreach (DeviceLayoutControl layoutControl in ViewModelPanel.Controls)
			{
				foreach (SubLayoutControl deviceControl in layoutControl.SubLayoutControlContainer.Controls)
				{
					deviceControl.UpdateToolTips();
				}
			}
		}

		protected Queue<DeviceLayoutControl> RecycleLayout = new Queue<DeviceLayoutControl>();
		protected Queue<SubLayoutControl> RecycleSubLayout = new Queue<SubLayoutControl>();
		public DeviceLayoutControl GetDeviceLayoutControl()
		{
			if (RecycleLayout.Count > 0)
			{
				return RecycleLayout.Dequeue();
			}

			var layoutControl = new DeviceLayoutControl();

			layoutControl.OnDeviceLayoutMouseDrag += OnDeviceLayoutMouseDrag;
			layoutControl.OnDeviceLayoutMouseDown += OnDeviceLayoutMouseDown;
			layoutControl.OnDeviceLayoutMouseDoubleClick += OnDeviceLayoutMouseDoubleClick;

			return layoutControl;
		}

		public SubLayoutControl GetSubLayoutControl()
		{
			if (RecycleSubLayout.Count > 0)
			{
				return RecycleSubLayout.Dequeue();
			}

			var deviceControl = new SubLayoutControl();
			deviceControl.OnSubLayoutMouseDown += OnSubLayoutMouseDown;
			deviceControl.OnSubLayoutMouseDrag += OnSubLayoutMouseDrag;
			deviceControl.OnSubLayoutMouseDoubleClick += OnSubLayoutMouseDoubleClick;

			return deviceControl;
		}

		public override void Refresh()
		{
			foreach (DeviceLayoutControl layoutControl in ViewModelPanel.Controls)
			{
				layoutControl.Invalidate();
				foreach (SubLayoutControl deviceControl in layoutControl.SubLayoutControlContainer.Controls)
				{
					deviceControl.Invalidate();
				}
			}
		}
	}
}