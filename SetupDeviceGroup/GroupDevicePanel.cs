using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Interface;
using PanelBase;
using SetupDevice;

namespace SetupDeviceGroup
{
    public sealed class GroupDevicePanel : Panel
    {
        public event EventHandler OnDeviceSelectionChange;
        public event EventHandler OnDeviceGroupEditClick;

        public IServer Server;
        private readonly GroupPanel _groupPanel;
        private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();
        private readonly List<DeviceListPanel> _deviceListPanels = new List<DeviceListPanel>();

        public Panel CustomGroupPanelContainer;

        public IDeviceGroup Group{
            get
            {
                return _groupPanel.Group;
            }
            set
            {
                _groupPanel.Server = Server;
                _groupPanel.Group = value;

                _groupPanel.Cursor = ((value != null && value.Id == 0))
                                           ? Cursors.Default
                                           : Cursors.Hand;
            }
        }

        public GroupDevicePanel()
        {
            Dock = DockStyle.Top;
            Padding = new Padding(0, 0, 0, 15);
            DoubleBuffered = true;
            AutoSize = true;
            MinimumSize = new Size(0, 15);

            _groupPanel = new GroupPanel();
            _groupPanel.OnGroupEditClick += EditButtonMouseClick;

            _deviceListPanels.Add(new DeviceListPanel());
        }

        public void ShowDevices()
        {
            _isEditing = false;
            ClearDeviceListControls();
            var panel = new DeviceListPanel();
            _deviceListPanels.Add(panel);

            if (Group.Items.Count > 0)
            {
                panel.Padding = new Padding(0, 0, 0, 0);
                panel.Show();

                var list = new List<IDevice>(Group.Items);
                list.Sort((x, y) => (y.Id - x.Id));
                foreach (IDevice device in list)
                {
                    if (device == null) continue;

                    if (!(device is ICamera)) continue;

                    DevicePanel devicePanel = GetDevicePanel();
                    devicePanel.Device = device;

                    panel.Controls.Add(devicePanel);
                }

                var deviceTitlePanel = GetDevicePanel();
                deviceTitlePanel.IsTitle = true;
                deviceTitlePanel.Cursor = Cursors.Default;
                deviceTitlePanel.EditVisible = false;
                panel.Controls.Add(deviceTitlePanel);

                var invasable = GetDevicePanel();
                invasable.IsTitle = true;
                invasable.Height = 0;
                panel.Controls.Add(invasable);

                Controls.Add(panel);
            }

            if (CustomGroupPanelContainer != null)
                CustomGroupPanelContainer.Controls.Add(_groupPanel);
            else
                Controls.Add(_groupPanel);
        }

        public void ShowDevicesWithSelection()
        {
            _isEditing = false;
            ClearDeviceListControls();

            if (Server is ICMS)
            {
                var nvrs = new List<INVR>(((ICMS)Server).NVRManager.NVRs.Values);
                nvrs.Sort((x, y) => (y.Id - x.Id));
                foreach (INVR nvr in nvrs)
                {
                    if (nvr.Device.Devices.Count == 0) continue;

                    AppendDeviceListPanel(nvr, new DeviceListPanel { Padding = new Padding(0, 22, 0, 0), NVR = nvr });
                }
            }
            else
            {
                AppendDeviceListPanel((INVR)Server, new DeviceListPanel { Padding = new Padding(0, 0, 0, 0) });
            }

            _isEditing = true;
            _groupPanel.TextEditorVisible = true;
            _groupPanel.Cursor = Cursors.Default;

            if (CustomGroupPanelContainer != null)
                CustomGroupPanelContainer.Controls.Add(_groupPanel);
            else
                Controls.Add(_groupPanel);
        }

        private void AppendDeviceListPanel(INVR nvr, DeviceListPanel listPanel)
        {
            var selectAll = true;
            var devices = new List<IDevice>(nvr.Device.Devices.Values);

            //reverse
            devices.Sort((x, y) => (y.Id - x.Id));
            if (devices.Count > 0)
            {
                foreach (IDevice device in devices)
                {
                    if (!(device is ICamera)) continue;

                    var devicePanel = GetDevicePanel();
                    devicePanel.SelectionVisible = true;
                    devicePanel.Device = device;

                    if (Group.Items.Contains(device))
                        devicePanel.Checked = true;
                    else
                        selectAll = false;

                    listPanel.Controls.Add(devicePanel);
                }

                var deviceTitlePanel = GetDevicePanel();
                deviceTitlePanel.IsTitle = true;
                deviceTitlePanel.Cursor = Cursors.Default;
                deviceTitlePanel.EditVisible = false;
                deviceTitlePanel.SelectionVisible = true;
                deviceTitlePanel.Checked = selectAll;
                deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
                deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
                listPanel.Controls.Add(deviceTitlePanel);
            }

            Controls.Add(listPanel);
            _deviceListPanels.Add(listPanel);
        }

        private DevicePanel GetDevicePanel()
        {
            if (_recycleDevice.Count > 0)
            {
                return _recycleDevice.Dequeue();
            }

            var devicePanel = new DevicePanel
            {
                EditVisible = false,
                SelectionVisible = false,
                Cursor = Cursors.Default,
                Server = Server,
            };
            devicePanel.OnSelectChange += DeviceControlOnSelectChange;

            return devicePanel;
        }

        public Boolean Checked
        {
            get
            {
                return _groupPanel.Checked;
            }
            set
            {
                _groupPanel.Checked = value;
            }
        }

        public List<IDevice> DeviceSelection
        {
            get
            {
                var devices = new List<IDevice>();
                foreach (DeviceListPanel listPanel in _deviceListPanels)
                {
                    foreach (DevicePanel control in listPanel.Controls)
                    {
                        if (!control.Checked || control.Device == null) continue;

                        devices.Add(control.Device);
                    }
                }

                devices.Sort((x, y) => (x.Id - y.Id));
                return devices;
            }
        }

        private Boolean _isEditing;
        private void DeviceControlOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            var panel = sender as DevicePanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (DevicePanel control in panel.Parent.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = panel.Parent.Controls[panel.Parent.Controls.Count - 1] as DevicePanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= DevicePanelOnSelectAll;
                title.OnSelectNone -= DevicePanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += DevicePanelOnSelectAll;
                title.OnSelectNone += DevicePanelOnSelectNone;
            }

            if (OnDeviceSelectionChange != null) 
                OnDeviceSelectionChange(this, null);
        }

        public Boolean SelectionVisible
        {
            get { return _groupPanel.SelectionVisible; }
            set
            {
                if(value)
                {
                    if (Group == null || Group.Id == 0)
                        Visible = false;
                }
                else
                {
                    Visible = true;
                }
                _groupPanel.SelectionVisible = value;
            }
        }

        public Boolean EditVisible
        {
            set
            {
                _groupPanel.EditVisible = value;
            }
        }

        public void ShowGroup()
        {
            foreach (DeviceListPanel listPanel in _deviceListPanels)
            {
                foreach (DevicePanel control in listPanel.Controls)
                {
                    control.SelectionVisible = false;
                    control.Checked = false;
                    control.Device = null;
                    control.EditVisible = false;
                    control.Cursor = Cursors.Hand;
                    control.IsTitle = false;
                    if(control.Height == 0)
                        continue;

                    if (!_recycleDevice.Contains(control))
                        _recycleDevice.Enqueue(control);
                }
                listPanel.Controls.Clear();
                listPanel.Hide();
            }

            if (CustomGroupPanelContainer != null)
                CustomGroupPanelContainer.Controls.Add(_groupPanel);
            else
                Controls.Add(_groupPanel);
        }

        private void EditButtonMouseClick(Object sender, EventArgs e)
        {
            if(Group != null && Group.Id == 0 )return;

            if (OnDeviceGroupEditClick != null)
                OnDeviceGroupEditClick(this, e);
        }

        public void ClearViewModel()
        {
            _isEditing = false;
            ClearDeviceListControls();
            _groupPanel.Group = null;

            Controls.Clear();
            Checked = false;
        }

        private void ClearDeviceListControls()
        {
            foreach (DeviceListPanel listPanel in _deviceListPanels)
            {
                foreach (DevicePanel control in listPanel.Controls)
                {
                    control.SelectionVisible = false;
                    control.Checked = false;
                    control.Device = null;
                    control.EditVisible = false;

                    if (control.IsTitle)
                    {
                        control.OnSelectAll -= DevicePanelOnSelectAll;
                        control.OnSelectNone -= DevicePanelOnSelectNone;
                        control.IsTitle = false;
                    }

                    if (control.Height == 0)
                        continue;

                    if (!_recycleDevice.Contains(control))
                        _recycleDevice.Enqueue(control);
                }
                listPanel.Controls.Clear();
                Controls.Remove(listPanel);
            }
            _deviceListPanels.Clear();
        }

        private void DevicePanelOnSelectAll(Object sender, EventArgs e)
        {
            var devicePanel = sender as DevicePanel;
            if(devicePanel == null || devicePanel.Parent == null) return;

            Panel panel = null;
            try
            {
                panel = ((Panel) Parent);
            }
            catch (Exception)
            {
            }

            var scrollTop = 0;

            if (panel != null)
            {
                scrollTop = panel.AutoScrollPosition.Y * -1;
                panel.AutoScroll = false;
            }

            foreach (DevicePanel control in devicePanel.Parent.Controls)
            {
                control.Checked = true;
            }

            if (panel != null)
            {
                panel.AutoScroll = true;
                panel.AutoScrollPosition = new Point(0, scrollTop);
            }
        }

        private void DevicePanelOnSelectNone(Object sender, EventArgs e)
        {
            var devicePanel = sender as DevicePanel;
            if (devicePanel == null || devicePanel.Parent == null) return;

            Panel panel = null;
            try
            {
                panel = ((Panel)Parent);
            }
            catch (Exception)
            {
            }

            var scrollTop = 0;

            if (panel != null)
            {
                scrollTop = panel.AutoScrollPosition.Y * -1;
                panel.AutoScroll = false;
            }

            foreach (DevicePanel control in devicePanel.Parent.Controls)
            {
                control.Checked = false;
            }

            if (panel != null)
            {
                panel.AutoScroll = true;
                panel.AutoScrollPosition = new Point(0, scrollTop);
            }

        }
    }

    public sealed class DeviceListPanel : Panel
    {
        public INVR NVR;
        public DeviceListPanel()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Top;
            AutoSize = true;
            BackColor = Color.Transparent;

            Paint += DeviceListPanelPaint;
        }

        private void DeviceListPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null || NVR == null) return;

            e.Graphics.DrawString(NVR.ToString(), Manager.Font, Brushes.Black, 5, 5);
        }
    }
}
