using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using GPSTree.Objects;
using Interface;
using PanelBase;

namespace GPSTree
{
	public partial class TreeView : UserControl, IControl, IAppUse, IServerUse, IDrag, IDrop, IMinimize, IMouseHandler
	{
		public event EventHandler OnMinimizeChange;

		public event EventHandler<EventArgs<IDeviceGroup>> OnGroupDoubleClick;
		public event EventHandler<EventArgs<IDevice>> OnDeviceDoubleClick;

		public event EventHandler<EventArgs<Object>> OnDragStart;

		private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();

		public Label DragDropLabel { get; private set; }
		public Panel DragDropProxy { get; private set; }

		public Dictionary<String, String> Localization;

		public String TitleName
		{
			get
			{
				return _panelTitleBar.Text;
			}
			set
			{
				_panelTitleBar.Text = value;
			}
		}

		public Image Icon
		{
			get { return Properties.Resources.icon; }
		}

        public IApp App { get; set; }
        protected INVR NVR;
        protected ICMS CMS;
	    private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is INVR)
                    NVR = value as INVR;
                if (value is ICMS)
                    CMS = value as ICMS;
            }
        }

		public UInt16 MinimizeHeight
		{
			get { return (UInt16)titlePanel.Size.Height; }
		}
		public Boolean IsMinimize { get; private set; }
		private readonly Timer _hideScrollBarTimer = new Timer();

		public void Initialize()
		{
			InitializeComponent();

			InitialzeViewList();

			Dock = DockStyle.Fill;
			_panelTitleBar.Panel = this;
			titlePanel.Controls.Add(_panelTitleBar);

			DragDropProxy = new Panel
			{
				Width = Width,
				Height = 25,
				BackColor = Color.Orange,
				Padding = new Padding(1),
			};

			DragDropLabel = new Label
			{
				Dock = DockStyle.Fill,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				TextAlign = ContentAlignment.MiddleCenter,
			};
			DragDropProxy.Controls.Add(DragDropLabel);

            _hideScrollBarTimer.Tick += HideScrollBar;
            _hideScrollBarTimer.Interval = 1000;
        }

        private SortOrder _sortOrder = SortOrder.Descending;
        public SortOrder SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                _sortOrder = value;
                if (_view != null)
                {
					_view.UpdateView();
                }
            }
        }

        private String _sortMode = "GROUP";
        public String SortMode{
            get
            {
                return _sortMode;
            }
            set
            {
                _sortMode = value;
                if (_view != null)
                {
                    _view.UpdateView();
                }
            }
		}

		public Dictionary<String, ViewBase> ViewList = new Dictionary<String, ViewBase>();

		private ViewBase _view;
		public ViewBase View
		{
			get
			{
				return _view;
			}
			set
			{
				if (ViewList.ContainsKey(value.Name))
				{
					_view = value;

					_view.UpdateView();
				}
			}
		}

		private void InitialzeViewList()
		{
			ViewBase view = new ListView {NVR = CMS != null ? CMS.NVR.NVRs[1] : NVR};

			((ListView)view).OnGroupMouseDrag += ViewModelPanelGroupMouseDrag;
			((ListView)view).OnGroupMouseDoubleClick += ViewModelPanelGroupMouseDoubleClick;

			((ListView)view).OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
			((ListView)view).OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;

			view.Name = "list";
			view.ViewModelPanel = viewModelPanel;

			ViewList.Add(view.Name, view);

			if (_view == null)
				_view = ViewList["list"];
		}

		public Boolean CheckDragDataType(Object dragObj)
		{
			return (dragObj is INVR || dragObj is IDeviceGroup || dragObj is IDevice);
		}

		public void DragStop(Point point, EventArgs<Object> e)
		{
			if (DragDropProxy != null)
				DragDropProxy.Visible = false;
		}

		public void DragMove(MouseEventArgs e)
		{
			if (DragDropProxy == null) return;

			Point location = DragDropProxy.Location;
			location.X = e.X - 10;// -(DragDropProxy.Size.Width / 2);
			location.Y = e.Y - (DragDropProxy.Size.Height / 2);
			DragDropProxy.Location = location;
		}

		public void Activate()
		{
			_view.UpdateView();
		}

		public void Deactivate()
		{
		}
		
		public void ViewModelPanelGroupMouseDrag(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as GroupControl;
			if (control != null && OnDragStart != null)
			{
				DragDropLabel.ForeColor = control.ForeColor;
				DragDropLabel.BackColor = control.BackColor;
				DragDropLabel.Text = ((GroupControl)sender).DeviceGroup.Name;

				OnDragStart(this, new EventArgs<Object>(((GroupControl)sender).DeviceGroup));
			}
		}

		public void ViewModelPanelDeviceMouseDrag(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as GPSControl;
			if (control != null && OnDragStart != null)
			{
				DragDropLabel.ForeColor = control.ForeColor;
				DragDropLabel.BackColor = control.BackColor;

				if (App.PageActivated.Name == "Tracker")
					((GPSControl)sender).Device.DeviceType = DeviceType.Tracker ;
				else
					((GPSControl) sender).Device.DeviceType = DeviceType.GPS;
				
				DragDropLabel.Text = ((GPSControl)sender).Device.ToString();

				OnDragStart(this, new EventArgs<Object>(((GPSControl)sender).Device));
			}
		}

		public void ViewModelPanelGroupMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as GroupControl;
			if (control != null && OnGroupDoubleClick != null)
			{
				OnGroupDoubleClick(this, new EventArgs<IDeviceGroup>(((GroupControl)sender).DeviceGroup));
			}
		}

		public void ViewModelPanelDeviceMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as GPSControl;
			if (control != null && OnDeviceDoubleClick != null)
			{
				OnDeviceDoubleClick(this, new EventArgs<IDevice>(((GPSControl)sender).Device));
			}
		}
						
		public void GlobalMouseHandler()
		{
			if (Drag.IsDrop(this))
			{
				if (!viewModelPanel.AutoScroll)
				{
					viewModelPanel.AutoScroll = true;
				}
				viewModelPanel.Focus();

				_hideScrollBarTimer.Enabled = false;

				return;
			}

			if (viewModelPanel.AutoScroll)
				_hideScrollBarTimer.Enabled = true;
		}

		private void HideScrollBar(Object sender, EventArgs e)
		{
			_hideScrollBarTimer.Enabled = false;
			viewModelPanel.AutoScroll = false;

			//force refresh to hide scroll bar
			viewModelPanel.Height++;
		}

		public void Minimize()
		{
			IsMinimize = true;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}

		public void Maximize()
		{
			IsMinimize = false;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}				
	}
}
