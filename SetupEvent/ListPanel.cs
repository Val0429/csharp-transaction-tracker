using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupDevice;

namespace SetupEvent
{
    public partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<IDevice>> OnDeviceEdit;

        public IServer Server;

        public void Initialize()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        protected Queue<DevicePanel> RecycleDevice = new Queue<DevicePanel>();

        private Point _previousScrollPosition;

        public void GenerateViewModel()
        {
            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();

            if (Server == null) return;

            if (Server is ICMS)
            {
                var cms = Server as ICMS;
                var sortResult = new List<INVR>(cms.NVRManager.NVRs.Values);
                sortResult.Sort((x, y) => (y.Id - x.Id));
                foreach (INVR nvr in sortResult)
                {
                    CreateDevicePanel(nvr.Device.Devices, nvr);
                }
            }
            else
            {
                CreateDevicePanel(Server.Device.Devices, Server as INVR);
            }

            containerPanel.Select();
            containerPanel.AutoScrollPosition = new Point(0, 0);
        }

        private void CreateDevicePanel(Dictionary<UInt16, IDevice> devices, INVR nvr)
        {
            if (Server == null) return;

            var sortResult = new List<IDevice>(devices.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0) return;

            containerPanel.Visible = false;
            foreach (var device in sortResult)
            {
                if (device == null || !(device is ICamera)) continue;

                var devicePanel = GetDevicePanel();

                devicePanel.Device = device;

                containerPanel.Controls.Add(devicePanel);
            }

            var deviceTitlePanel = GetDevicePanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.EditVisible = false;
            containerPanel.Controls.Add(deviceTitlePanel);
            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;

            if (Server is ICMS)
            {
                var titleLabel = new Label
                {
                    Text = nvr.ToString(),
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.BottomLeft,
                    Size = new Size(456, 25),
                    Padding = new Padding(8, 0, 0, 0),
                    Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = Color.DimGray,
                    BackColor = Color.Transparent
                };
                containerPanel.Controls.Add(titleLabel);
            }
        }

        private DevicePanel GetDevicePanel()
        {
            if (RecycleDevice.Count > 0)
            {
                return RecycleDevice.Dequeue();
            }
            var devicePanel = new DevicePanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
                DataType = "EventHandle",
                Server = Server,
            };
            devicePanel.OnDeviceEditClick += DeviceControlOnDeviceEditClick;

            return devicePanel;
        }

        private void DeviceControlOnDeviceEditClick(Object sender, EventArgs e)
        {
            if (((DevicePanel) sender).Device == null) return;

            if (OnDeviceEdit != null)
                OnDeviceEdit(this, new EventArgs<IDevice>(((DevicePanel)sender).Device));
        }

        private void ClearViewModel()
        {
            foreach (var control in containerPanel.Controls)
            {
                if (control is Label) containerPanel.Controls.Remove(control as Control);
            }

            foreach (var control in containerPanel.Controls)
            {
                var devicePanel = control as DevicePanel;
                if (devicePanel == null) continue;
                devicePanel.SelectionVisible = false;

                devicePanel.Checked = false;
                devicePanel.Device = null;
                devicePanel.Cursor = Cursors.Hand;
                devicePanel.EditVisible = true;

                if (devicePanel.IsTitle)
                {
                    //deviceControl.OnSortChange -= DeviceControlOnSortChange;
                    devicePanel.IsTitle = false;
                }

                if (!RecycleDevice.Contains(devicePanel))
                    RecycleDevice.Enqueue(devicePanel);
            }
            containerPanel.Controls.Clear();
        }
    } 
}