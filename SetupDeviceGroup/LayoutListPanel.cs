using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupDeviceGroup
{
    public sealed partial class LayoutListPanel : UserControl
    {
        public event EventHandler<EventArgs<IDeviceLayout>> OnDeviceLayoutEdit;
        public event EventHandler OnDeviceLayoutAdd;

        public event EventHandler OnImmerVisionLayoutAdd;

        public IServer Server;
        private INVR _nvr;
        public Dictionary<String, String> Localization;

        public LayoutListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupDeviceLayout_AddNewDeviceLayout", "Add new device layout..."},
                                   {"SetupDeviceLayout_AddedDeviceLayout", "Added device layout"},
								   {"SetupDeviceLayout_ImmervisionLayout", "Add new ImmerVision Panomorph Lens Layout..."},

                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
            addedLayoutLabel.Text = Localization["SetupDeviceLayout_AddedDeviceLayout"];
        }

        public void Initialize()
        {
            _nvr = Server as INVR;
            addNewDoubleBufferPanel.Paint += InputPanelPaint;
            addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;

            immervisionDoubleBufferPanel.Paint += InputPanelPaint;
            immervisionDoubleBufferPanel.MouseClick += ImmervisionDoubleBufferPanelMouseClick;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
            Manager.PaintEdit(g, addNewDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupDeviceLayout_" + ((Control)sender).Tag]);
        }

        private readonly Queue<DeviceLayoutPanel> _recycleLayout = new Queue<DeviceLayoutPanel>();

        private Point _previousScrollPosition;
        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();
            addedLayoutLabel.Visible = addNewDoubleBufferPanel.Visible = immervisionDoubleBufferPanel.Visible = true;

            if (Server.Device.DeviceLayouts == null) return;

            var sortResult = new List<IDeviceLayout>(Server.Device.DeviceLayouts.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                addedLayoutLabel.Visible = false;
                return;
            }

            addedLayoutLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (IDeviceLayout layout in sortResult)
            {
                if (layout == null) continue;

                DeviceLayoutPanel deviceLayoutPanel = GetDeviceLayoutPanel();

                deviceLayoutPanel.DeviceLayout = layout;
                deviceLayoutPanel.EditVisible = true;

                containerPanel.Controls.Add(deviceLayoutPanel);
            }

            var deviceTitlePanel = GetDeviceLayoutPanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.EditVisible = false;
            deviceTitlePanel.OnSelectAll += DeviceLayoutPanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += DeviceLayoutPanelOnSelectNone;
            containerPanel.Controls.Add(deviceTitlePanel);
            containerPanel.Visible = true;

            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;
        }

        public Boolean SelectionVisible
        {
            set
            {
                foreach (DeviceLayoutPanel deviceLayoutPanel in containerPanel.Controls)
                    deviceLayoutPanel.SelectionVisible = value;
            }
        }

        public void ShowCheckBox()
        {
            addNewDoubleBufferPanel.Visible = addedLayoutLabel.Visible = immervisionDoubleBufferPanel.Visible = false;

            foreach (DeviceLayoutPanel control in containerPanel.Controls)
            {
                control.SelectionVisible = true;
                control.EditVisible = false;
            }

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        private DeviceLayoutPanel GetDeviceLayoutPanel()
        {
            if (_recycleLayout.Count > 0)
            {
                return _recycleLayout.Dequeue();
            }

            var deviceLayoutPanel = new DeviceLayoutPanel
            {
                EditVisible = true,
            };

            deviceLayoutPanel.OnSelectChange += DeviceLayoutPanelOnSelectChange;
            deviceLayoutPanel.OnDeviceLayoutEditClick += DeviceLayoutPanelOnDeviceLayoutEditClick;

            return deviceLayoutPanel;
        }

        public void RemoveSelectedDeviceLayouts()
        {
            foreach (DeviceLayoutPanel deviceLayoutPanel in containerPanel.Controls)
            {
                if (!deviceLayoutPanel.Checked || deviceLayoutPanel.DeviceLayout == null) continue;

                deviceLayoutPanel.DeviceLayout.ReadyState = ReadyState.Delete;
                _nvr.DeviceLayoutModify(deviceLayoutPanel.DeviceLayout);
                Server.Device.DeviceLayouts.Remove(deviceLayoutPanel.DeviceLayout.Id);
            }
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDeviceLayoutAdd != null)
                OnDeviceLayoutAdd(this, e);
        }

        void ImmervisionDoubleBufferPanelMouseClick(object sender, MouseEventArgs e)
        {
            if (OnImmerVisionLayoutAdd != null)
                OnImmerVisionLayoutAdd(this, e);
        }

        private void DeviceLayoutPanelOnDeviceLayoutEditClick(Object sender, EventArgs e)
        {
            if (OnDeviceLayoutEdit != null)
                OnDeviceLayoutEdit(this, new EventArgs<IDeviceLayout>(((DeviceLayoutPanel)sender).DeviceLayout));
        }

        private void DeviceLayoutPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as DeviceLayoutPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (DeviceLayoutPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as DeviceLayoutPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= DeviceLayoutPanelOnSelectAll;
                title.OnSelectNone -= DeviceLayoutPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += DeviceLayoutPanelOnSelectAll;
                title.OnSelectNone += DeviceLayoutPanelOnSelectNone;
            }
        }

        private void DeviceLayoutPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (DeviceLayoutPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void DeviceLayoutPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (DeviceLayoutPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        private void ClearViewModel()
        {
            foreach (DeviceLayoutPanel deviceGroupControl in containerPanel.Controls)
            {
                deviceGroupControl.OnSelectChange -= DeviceLayoutPanelOnSelectChange;
                deviceGroupControl.Checked = false;
                deviceGroupControl.Cursor = Cursors.Hand;
                deviceGroupControl.EditVisible = true;
                deviceGroupControl.DeviceLayout = null;
                deviceGroupControl.OnSelectChange += DeviceLayoutPanelOnSelectChange;

                if (deviceGroupControl.IsTitle)
                {
                    deviceGroupControl.OnSelectAll -= DeviceLayoutPanelOnSelectAll;
                    deviceGroupControl.OnSelectNone -= DeviceLayoutPanelOnSelectNone;
                    deviceGroupControl.IsTitle = false;
                }

                if (!_recycleLayout.Contains(deviceGroupControl))
                    _recycleLayout.Enqueue(deviceGroupControl);
            }

            containerPanel.Controls.Clear();
        }
    }
}
