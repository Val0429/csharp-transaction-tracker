using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceTree.Objects;
using DeviceTree.View;
using Interface;
using PanelBase;

namespace DeviceTree
{
    public sealed partial class DeviceLayoutTree : UserControl, IControl, IServerUse, IDrag, IDrop, IMinimize, IMouseHandler
    {
        public event EventHandler OnMinimizeChange;

        public event EventHandler<EventArgs<IDevice>> OnDeviceLayoutDoubleClick;
        public event EventHandler<EventArgs<IDeviceLayout>> OnImmerVisionDeviceLayoutDoubleClick;

        public event EventHandler<EventArgs<IDevice>> OnSubLayoutDoubleClick;

        public event EventHandler<EventArgs<Object>> OnDragStart;

        private readonly PanelTitleBar _panelTitleBar = new PanelTitleBar();

        public Label DragDropLabel { get; private set; }
        public Panel DragDropProxy { get; private set; }

        public Dictionary<String, String> Localization;

        public String TitleName { get; set; }

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

        private INVR _nvr;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is INVR)
                    _nvr = value as INVR;
            }
        }

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }
        public Boolean IsMinimize { get; private set; }

        public DeviceLayoutTree()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_DeviceLayout", "Device Layout"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();

            Dock = DockStyle.Fill;
            //---------------------------
            Icon = new ControlIconButton { Image = _icon };
            Icon.Click += DockIconClick;
            //---------------------------
        }

        public void Initialize()
        {
            InitialzeViewList();

            _panelTitleBar.Text = TitleName = Localization["Control_ImageStitching"];
            _panelTitleBar.Panel = this;
            titlePanel.Controls.Add(_panelTitleBar);

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            DragDropProxy = new DoubleBufferPanel
            {
                Width = Width,
                Height = 25,
                BackColor = Color.Orange,
                Padding = new Padding(1),
            };

            DragDropLabel = new DoubleBufferLabel
            {
                AutoSize = true,//avoid text too oong and wrap lines
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                MinimumSize = new Size(DragDropProxy.Width - 2, DragDropProxy.Height - 2) //border
            };
            DragDropProxy.Controls.Add(DragDropLabel);
        }

        public SortOrder SortOrder { get; set; }

        public String SortMode { get; set; }

        public Dictionary<String, IViewBase> ViewList = new Dictionary<String, IViewBase>();

        private IViewBase _view;
        public IViewBase View
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
                }
            }
        }

        public void InitialzeViewList()
        {
            var view = new DeviceLayoutListView
            {
                NVR = _nvr
            };
            view.OnDeviceLayoutMouseDown += ViewModelPanelDeviceLayoutMouseDown;
            view.OnDeviceLayoutMouseDrag += ViewModelPanelDeviceLayoutMouseDrag;
            view.OnDeviceLayoutMouseDoubleClick += ViewModelPanelDeviceLayoutMouseDoubleClick;


            view.OnSubLayoutMouseDown += ViewModelPanelDeviceLayoutMouseDown;
            view.OnSubLayoutMouseDrag += ViewModelPanelSubLayoutMouseDrag;
            view.OnSubLayoutMouseDoubleClick += ViewModelPanelSubLayoutMouseDoubleClick;

            view.Name = "list";
            view.ViewModelPanel = viewModelPanel;

            ViewList.Add(view.Name, view);

            if (_view == null)
                _view = ViewList["list"] as ViewBase;
        }

        public Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is IDeviceLayout || dragObj is ISubLayout);
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

        private Boolean _checkIfHideAtFirstAppear;
        public void Activate()
        {
            if (_nvr == null) return;

            if (!_checkIfHideAtFirstAppear)
            {
                _checkIfHideAtFirstAppear = true;

                if (_nvr.Device.DeviceLayouts == null || _nvr.Device.DeviceLayouts.Count == 0)
                {
                    Minimize();
                    return;
                }
            }
            ((DeviceLayoutListView)_view).UpdateView();
        }

        public void Deactivate()
        {
        }

        public void ViewModelPanelDeviceLayoutMouseDown(Object sender, MouseEventArgs e)
        {
            viewModelPanel.Focus();
        }

        public void ViewModelPanelDeviceLayoutMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceLayoutControl;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;
                DragDropLabel.Text = ((DeviceLayoutControl)sender).DeviceLayout.ToString();

                OnDragStart(this, new EventArgs<Object>(control.DeviceLayout));
            }
        }

        public void ViewModelPanelDeviceLayoutMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as DeviceLayoutControl;
            var layout = control.DeviceLayout;

            if (layout.isImmerVision)
            {
                if (control != null && OnImmerVisionDeviceLayoutDoubleClick != null)
                {
                    OnImmerVisionDeviceLayoutDoubleClick(this, new EventArgs<IDeviceLayout>(control.DeviceLayout));
                }
            }
            else
            {
                if (control != null && OnDeviceLayoutDoubleClick != null)
                {
                    OnDeviceLayoutDoubleClick(this, new EventArgs<IDevice>(control.DeviceLayout));
                }
            }
        }

        public void ViewModelPanelSubLayoutMouseDrag(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as SubLayoutControl;
            if (control != null && OnDragStart != null)
            {
                DragDropLabel.ForeColor = control.ForeColor;
                DragDropLabel.BackColor = control.BackColor;

                DragDropLabel.Text = control.SubLayout.ToString();

                OnDragStart(this, new EventArgs<Object>(control.SubLayout));
            }
        }

        public void ViewModelPanelSubLayoutMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var control = sender as SubLayoutControl;
            if (control != null && OnSubLayoutDoubleClick != null)
            {
                OnSubLayoutDoubleClick(this, new EventArgs<IDevice>(control.SubLayout));
            }
        }

        public void GlobalMouseHandler()
        {
            if (Drag.IsDrop(viewModelPanel))
            {
                if (!viewModelPanel.AutoScroll)
                {
                    viewModelPanel.AutoScroll = true;
                }

                return;
            }

            if (viewModelPanel.AutoScroll)
                HideScrollBar();
        }

        private Point _previousScrollPosition;
        private void HideScrollBar()
        {
            _previousScrollPosition = viewModelPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            viewModelPanel.AutoScroll = false;

            viewModelPanel.Height++;
            viewModelPanel.AutoScrollPosition = _previousScrollPosition;
        }

        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else
                Minimize();
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
