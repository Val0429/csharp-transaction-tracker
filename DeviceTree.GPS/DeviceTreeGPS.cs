using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceTree.GPS.Objects;
using DeviceTree.Objects;
using Interface;
using ServerProfile;

namespace DeviceTree.GPS
{
	public partial class DeviceTreeGPS : DeviceTree
	{
		public event EventHandler<EventArgs<IDevice>> OnDeviceClick;

		public override event EventHandler<EventArgs<IDeviceGroup>> OnGroupDoubleClick;
		public override event EventHandler<EventArgs<IDevice>> OnDeviceDoubleClick;
		public override event EventHandler<EventArgs<Object>> OnDragStart;

		//protected INVR NVR;
		//protected ICMS CMS;
		//private IServer _server;
		//public override IServer Server
		//{
		//    get { return _server; }
		//    set
		//    {
		//        _server = value;
		//        if (value is INVR)
		//            NVR = value as INVR;
		//        if (value is ICMS)
		//            CMS = value as ICMS;
		//    }
		//}

		public override sealed Image Icon { get; set; }

		private Control _activeDeviceControl;
		private readonly String[] _polyColor = new[] {""
			, "#072F67", "#076767", "#077F87", "#379767", "#379737", "#076707", "#072F07", "#376707", "#372F07", "#676737"
			, "#877F07", "#9F6737", "#672F07", "#9F6707", "#CF2F07", "#9F2F07", "#9F0707", "#870707", "#A70727", "#9F0737"
			, "#670737", "#9F2F67", "#9F079F", "#870787", "#670767", "#9F07CF", "#9F07FF", "#6707CF", "#67679F", "#372F9F"
			, "#070767", "#0707CF", "#07079F", "#072F9F", "#3767CF", "#37679F", "#07679F", "#07979F", "#07CF9F", "#07CF67"
			, "#07CF07", "#077F07", "#079707", "#679767", "#9FCF07", "#CFCF07", "#CF9707", "#FF9707", "#CF6707", "#FF2F07"
			, "#FF0707", "#CF0707", "#CF0767", "#D70797", "#CF2F9F", "#CF079F", "#CF07Cf", "#CF07FF", "#9F2FFF", "#6707FF"
			, "#372FcF", "#372FFF", "#0707FF", "#072FCF"};

		private bool _setDeviceBackGorund = false;
		public void SetDeviceBackground()
		{
			_setDeviceBackGorund = true;
		}

		public DeviceTreeGPS()
		{
			Icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);


		}

		//public override void EventReceive(Object sender, EventArgs<List<ICamera>> e)
		//{

		//}

		public new Dictionary<String, ListViewGPS> ViewList = new Dictionary<String, ListViewGPS>();
		public override void InitialzeViewList()
		{
			var view = new ListViewGPS
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

			if (e.Button != MouseButtons.Left) return;

			var control = sender as GPSControl;
			if (control == null) return;

			ActivateDeviceControl(control);

			if (OnDeviceClick != null)
				OnDeviceClick(this, new EventArgs<IDevice>(control.Device));
		}

		public override void ViewModelPanelDeviceMouseDrag(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as GPSControl;
			if (control != null && OnDragStart != null)
			{
				ActivateDeviceControl(control);

				DragDropLabel.ForeColor = control.ForeColor;
				DragDropLabel.BackColor = control.BackColor;

				//((GPSControl)sender).Device.DeviceType = App.PageActivated.Name == "Tracker" ? DeviceType.Tracker : DeviceType.GPS;

				DragDropLabel.Text = ((GPSControl)sender).Device.ToString();

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

			var control = sender as GPSControl;
			if (control != null && OnDeviceDoubleClick != null)
			{
				ActivateDeviceControl(control);

				OnDeviceDoubleClick(this, new EventArgs<IDevice>(control.Device));
			}
		}

		private void ActivateDeviceControl(Control control)
		{
			if (!_setDeviceBackGorund) return;

			if (_activeDeviceControl != null)
				_activeDeviceControl.BackColor = Color.Transparent;
			
			_activeDeviceControl = control;

			int r = Convert.ToInt32(_polyColor[((DeviceControl)control).Device.Id].Substring(1, 2), 16);
			int g = Convert.ToInt32(_polyColor[((DeviceControl)control).Device.Id].Substring(3, 2), 16);
			int b = Convert.ToInt32(_polyColor[((DeviceControl)control).Device.Id].Substring(5, 2), 16);

			control.BackColor = Color.FromArgb(r,g,b);
		}

	}
}
