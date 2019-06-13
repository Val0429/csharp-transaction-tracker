using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using App;
using Constant;
using Device;
using DeviceConstant;
using DeviceTree.Objects;
using DeviceTree.View;
using Interface;
using PanelBase;
using POSRegister.Objects;

namespace POSRegister
{
    public partial class POSRegister : UserControl, IControl, IAppUse, IServerUse, IDrag, IDrop, IMinimize, IMouseHandler, IBlockPanelUse
	{
		public event EventHandler OnMinimizeChange;

		public event EventHandler<EventArgs<IDeviceGroup>> OnPOSDoubleClick;
		public virtual event EventHandler<EventArgs<IDevice>> OnDeviceDoubleClick;

		public virtual event EventHandler<EventArgs<Object>> OnDragStart;

		public Label DragDropLabel { get; private set; }
		public Panel DragDropProxy { get; private set; }

		public Dictionary<String, String> Localization;
		public IApp App { get; set; }

		public String TitleName { get; set; }

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
		protected IPTS PTS;
		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;
				if (value is IPTS)
					PTS = value as IPTS;
			}
		}
        public IBlockPanel BlockPanel { get; set; }
        protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();

		public UInt16 MinimizeHeight
		{
			get { return (UInt16)titlePanel.Size.Height; }
		}
		public Boolean IsMinimize { get; private set; }

        private IStore SelectedStore = null;

		public POSRegister()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_POSRegister", "POS Register"},

								   {"GroupPanel_NumDevice", "(%1 Device)"},
								   {"GroupPanel_NumDevices", "(%1 Devices)"},
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
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			//InitialzeViewList();
            InitinalRegisterComboBox();

            PanelTitleBarUI2.Text = TitleName = Localization["Control_POSRegister"];
            titlePanel.Controls.Add(PanelTitleBarUI2);
          
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

			if (PTS != null)
			{
				PTS.OnCameraStatusReceive -= EventReceive;
				PTS.OnCameraStatusReceive += EventReceive;

				PTS.OnPOSModify += POSModify;
			}
		}

		public void SetSwitchPage()
		{
			App.OnSwitchPage += MinimizePOSRegister;
		}

		public void MinimizePOSRegister(Object sender, EventArgs<String, Object> e)
		{
			if (!String.Equals(e.Value1, "Playback")) return;
			var transactionListParameter = e.Value2 as TransactionListParameter;
			if (transactionListParameter == null) return;

			Minimize();
		}

		public Boolean CheckDragDataType(Object dragObj)
		{
			return (dragObj is POS || dragObj is IDevice);
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
			if (_reloadTree)
			{
				if (PTS != null)
                    if ( _view != null)
					    ((POSListView)_view).UpdateView();
			}

            InitinalRegisterComboBox();

            keywordComboBox.DroppedDown = false;

            _reloadTree = false;
		}

		private Boolean _reloadTree = true;
		public void POSModify(Object sender, EventArgs<IPOS> e)
		{
			_reloadTree = true;
		}

		public virtual void EventReceive(Object sender, EventArgs<List<ICamera>> e)
		{
			if (_view != null)
			{
				if (PTS != null)
					((POSListView)_view).UpdateRecordingStatus();
			}
		}

		public Dictionary<String, IViewBase> ViewList = new Dictionary<String, IViewBase>();

		private ViewBase _view;

        public void InitinalRegisterComboBox()
        {
            keywordComboBox.Text = "";
            keywordComboBox.Items.Clear();

            foreach (IDivision division in PTS.POS.DivisionManager.Values)
            {
                var divisionNode = new ComboxItem { Depth = 0, Object = division, Text = division.ToString() , Selectable = false }; keywordComboBox.Items.Add(divisionNode);

                foreach (IRegion region in division.Regions.Values)
                {
                    var regionNode = new ComboxItem { Depth = 1, Object = region, Text = region.ToString(), Selectable = false, ParentNode = divisionNode }; keywordComboBox.Items.Add(regionNode);

                    foreach (IStore store in region.Stores.Values)
                    {
                        var storeNode = new ComboxItem { Depth = 2, Object = store, Text = store.ToString(), Selectable = true, ParentNode = regionNode }; keywordComboBox.Items.Add(storeNode);
                    }
                }
            }
            /*
            if (keywordComboBox.Items.Count == 0)
            {
                searchPanel.Visible = false;
            }
            else {
                searchPanel.Visible = true;
            }
            */
        }
        

        public virtual void InitialzeViewList()
		{
			POSListView view;
			if (PTS != null)
			{
				view = new POSListView
				{
					PTS = PTS,
                    Store = SelectedStore
				};

				view.OnPOSMouseDown += ViewModelPanelMouseDown;
				view.OnPOSMouseDrag += ViewModelPanelPOSMouseDrag;
				view.OnPOSMouseDoubleClick += ViewModelPanelPOSMouseDoubleClick;

				view.OnDeviceMouseDown += ViewModelPanelMouseDown;
				view.OnDeviceMouseDrag += ViewModelPanelDeviceMouseDrag;
				view.OnDeviceMouseDoubleClick += ViewModelPanelDeviceMouseDoubleClick;
			}
			else
				return;

			view.Name = "list";
			view.ViewModelPanel = viewModelPanel;

			ViewList[view.Name] = view;

            _view = view as ViewBase;

            //if (_view == null)
            //	_view = ViewList["list"] as ViewBase;
		}

		public void Deactivate()
		{
		}

		public void ViewModelPanelPOSMouseDrag(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

            var control = sender as POSControlUI2;
			if (control != null && OnDragStart != null)
			{
				DragDropLabel.ForeColor = control.ForeColor;
				DragDropLabel.BackColor = control.BackColor;

                var pos = ((POSControlUI2)sender).POS;
				var num = ((pos.Items.Count <= 1) ? Localization["GroupPanel_NumDevice"] : Localization["GroupPanel_NumDevices"]).
					Replace("%1", pos.Items.Count.ToString());

				DragDropLabel.Text = pos + @" " + num;

				OnDragStart(this, new EventArgs<Object>(pos));
			}
		}

		private System.Timers.Timer _timer;
		private UInt16 _refreshCounter;
		public virtual void ViewModelPanelMouseDown(Object sender, MouseEventArgs e)
		{
			if (_timer == null)
			{
				_timer = new System.Timers.Timer(500);
				_timer.Elapsed += RefreshPanel;
				_timer.SynchronizingObject = Server.Form;
			}
			viewModelPanel.Focus();

			_refreshCounter = 0;
			_timer.Enabled = true;
		}

		private void RefreshPanel(Object sender, EventArgs e)
		{
			_refreshCounter++;
			_view.Refresh();
			viewModelPanel.Invalidate();
			if (_refreshCounter > 5)
				_timer.Enabled = false;
		}

		public virtual void ViewModelPanelDeviceMouseDrag(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as DeviceControl;
			if (control != null && OnDragStart != null)
			{
				DragDropLabel.ForeColor = control.ForeColor;
				DragDropLabel.BackColor = control.BackColor;

				control.Device.DeviceType = DeviceType.Device;
				DragDropLabel.Text = ((DeviceControl)sender).Device.ToString();

				OnDragStart(this, new EventArgs<Object>(control.Device));
			}
		}

		public void ViewModelPanelPOSMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

            var control = sender as POSControlUI2;
			if (control != null && OnPOSDoubleClick != null)
			{
				OnPOSDoubleClick(this, new EventArgs<IDeviceGroup>(control.POS));
			}
		}

		public virtual void ViewModelPanelDeviceMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			var control = sender as DeviceControl;
			if (control != null && OnDeviceDoubleClick != null)
			{
				OnDeviceDoubleClick(this, new EventArgs<IDevice>(control.Device));
			}
		}

		public void GlobalMouseHandler()
		{
			if (Drag.IsDrop(viewModelPanel))
			{
				if (!viewModelPanel.AutoScroll)
				{
					viewModelPanel.AutoScroll = true;
					//viewModelPanel.AutoScrollPosition = _previousScrollPosition;
				}
				//viewModelPanel.Focus();

				return;
			}
			//viewModelPanel.AutoScroll = false;
			if (viewModelPanel.AutoScroll)
				HideScrollBar();
		}

		private Point _previousScrollPosition;
		private void HideScrollBar()
		{
			_previousScrollPosition = viewModelPanel.AutoScrollPosition;
			_previousScrollPosition.Y *= -1;
			viewModelPanel.AutoScroll = false;

			//force refresh to hide scroll bar
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
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
            {
                BlockPanel.HideThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = true;
                    App.StartupOption.SaveSetting();
                }
            }

            Icon.Image = _icon;
            Icon.BackgroundImage = null;

            Icon.Invalidate();

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
		}

		public void Maximize()
		{
            if (BlockPanel.LayoutManager.Page.Version == "2.0")
            {
                BlockPanel.ShowThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = false;
                    App.StartupOption.SaveSetting();
                }
            }

            Icon.Image = _iconActivate;
            Icon.BackgroundImage = ControlIconButton.IconBgActivate;

            IsMinimize = false;

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
		}

        private void searchButton_Click(object sender, EventArgs e)
        {
            foreach (ComboxItem item in keywordComboBox.Items)
            {
                if (item.ToString().IndexOf(keywordComboBox.Text) >= 0)
                    item.Visiable = true;
                else
                    item.Visiable = false;
            }

            for (int i = (keywordComboBox.Items.Count - 1); i >= 0; i--)
            {
                var item = keywordComboBox.Items[i] as ComboxItem;
                if (item.Visiable)
                {
                    if (item.ParentNode != null)
                        item.ParentNode.Visiable = true;
                }
                else
                {
                    keywordComboBox.Items.Remove(item);
                }
            }

            keywordComboBox.DroppedDown = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {


            InitinalRegisterComboBox();

            keywordComboBox.DroppedDown = false;
        }

        private void keywordComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                var keyword = keywordComboBox.Text;


                keywordComboBox.DroppedDown = true;
            }
            else
            {
                keywordComboBox.DroppedDown = false;
            }
        }

        private void keywordComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var item = keywordComboBox.Items[e.Index] as ComboxItem;

            if (item.Selectable)
                e.DrawBackground();

            var Rectangle = new Rectangle(
                e.Bounds.X + (item.Depth * 10),
                e.Bounds.Y,
                e.Bounds.Width - (item.Depth * 10),
                e.Bounds.Height
            );

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Far;
            sf.Alignment = StringAlignment.Near;

            Brush brush = new SolidBrush(ForeColor);
            if (e.State == System.Windows.Forms.DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                brush = new SolidBrush(SystemColors.HighlightText);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                brush = new SolidBrush(e.ForeColor);
            }

            e.Graphics.DrawString(keywordComboBox.Items[e.Index].ToString(), Font, brush, Rectangle, sf);
        }

        private void keywordComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = keywordComboBox.SelectedItem as ComboxItem;
            if (!item.Selectable)
            {
                var idx = keywordComboBox.SelectedIndex;

                for (int i = idx; i < keywordComboBox.Items.Count; i++)
                {
                    var itm = keywordComboBox.Items[i] as ComboxItem;

                    if (itm == null) continue;

                    if (itm.Selectable)
                    {
                        keywordComboBox.SelectedItem = itm;
                        break;
                    }
                }
            }

            if (keywordComboBox.SelectedItem == null) return;

            SelectedStore = (keywordComboBox.SelectedItem as ComboxItem).Object as IStore;

            if ( SelectedStore == null) return;

            InitialzeViewList();

            _reloadTree = true;
            Activate();
        }
    }
}
