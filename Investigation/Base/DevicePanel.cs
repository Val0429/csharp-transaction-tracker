using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DeviceConstant;
using Interface;
using PanelBase;

namespace Investigation.Base
{
    public sealed partial class DevicePanel : UserControl
    {
        public event EventHandler OnSelectChange;
        public ICMS CMS;
        public INVR NVR;
        public CameraEventSearchCriteria SearchCriteria;

        public DevicePanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Device";
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        private Boolean _isEditing;

        public void ParseSetting()
        {
            _isEditing = false;

            ClearViewModel();

            //if (CMS != null)
            //{
            //    var sortResult = new List<INVR>(CMS.NVRManager.NVRs.Values);
            //    sortResult.Sort((x, y) => (y.Id - x.Id));
            //    foreach (INVR nvr in sortResult)
            //    {
            //        CreateDevicePanel(nvr.Device.Devices, nvr);
            //    }
            //}
            //else
            //{
            //    CreateDevicePanel(NVR.Device.Devices, NVR);
            //}
            CreateDevicePanel(NVR.Device.Devices, NVR);

            containerPanel.Select();
            containerPanel.AutoScrollPosition = new Point(0, 0);

            _isEditing = true;
        } 

        private void CreateDevicePanel(Dictionary<UInt16, IDevice> devices, INVR nvr)
        {
            _isEditing = false;

            //ClearViewModel();

            var sortResult = new List<IDevice>(devices.Values);

            if (sortResult.Count == 0) return;

            sortResult.Sort((x, y) => (y.Id - x.Id));
            var selectAll = true;
            var count = 0;
            //containerPanel.Visible = false;
            foreach (IDevice device in sortResult)
            {
                SetupDevice.DevicePanel devicePanel = GetDevicePanel();

                devicePanel.Device = device;
                if(CMS != null)
                    devicePanel.Server = device.Server;
                devicePanel.Device = device;

                if(CMS != null)
                {
                    var isChecked = SearchCriteria.NVRDevice.Any(nvrDevice => nvrDevice.NVRId == device.Server.Id && nvrDevice.DeviceId == device.Id);

                    if (isChecked)
                    {
                        count++;
                        devicePanel.Checked = true;
                    }
                    else
                        selectAll = false;
                }
                else
                {
                    if (SearchCriteria.Device.Contains(device.Id))
                    {
                        count++;
                        devicePanel.Checked = true;
                    }
                    else
                        selectAll = false;
                }
                

                containerPanel.Controls.Add(devicePanel);
            }
            if (count == 0 && selectAll)
                selectAll = false;

            SetupDevice.DevicePanel deviceTitlePanel = GetDevicePanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.EditVisible = false;
            deviceTitlePanel.Checked = selectAll;
            deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
            deviceTitlePanel.Server = nvr;
            //deviceTitlePanel.CMS = CMS;
            containerPanel.Controls.Add(deviceTitlePanel);
            //containerPanel.Visible = true;

            if(CMS != null)
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
            //containerPanel.Select();
            //containerPanel.AutoScrollPosition = new Point(0, 0);

            _isEditing = true;
        }

        private readonly Queue<SetupDevice.DevicePanel> _recycleDevice = new Queue<SetupDevice.DevicePanel>();
        private SetupDevice.DevicePanel GetDevicePanel()
        {
            if (_recycleDevice.Count > 0)
            {
                return _recycleDevice.Dequeue();
            }

            var devicePanel = new SetupDevice.DevicePanel
            {
                Server = NVR,
                EditVisible = false,
                SelectionVisible = true,
            };

            if (CMS != null)
                devicePanel.CMS = CMS;

            devicePanel.OnSelectChange += DevicePanelOnSelectChange;

            return devicePanel;
        }

        private void DevicePanelOnSelectChange(Object sender, EventArgs e)
        {
            if(!_isEditing) return;

            var panel = sender as SetupDevice.DevicePanel;
            if (panel == null) return;
            if (panel.IsTitle) return;

            var selectAll = false;
            if (panel.Checked)
            {
                if(CMS != null)
                {
                    var isExists = SearchCriteria.NVRDevice.Any(nvrDevice => nvrDevice.NVRId == panel.Device.Server.Id && nvrDevice.DeviceId == panel.Device.Id);
                    if(!isExists)
                    {
                        SearchCriteria.NVRDevice.Add(new NVRDevice{NVRId = panel.Device.Server.Id, DeviceId = panel.Device.Id});
                        SearchCriteria.NVRDevice.OrderBy(c => c.NVRId).ThenBy(c => c.DeviceId);
                    }
                }
                else
                {
                    if (!SearchCriteria.Device.Contains(panel.Device.Id))
                    {
                        SearchCriteria.Device.Add(panel.Device.Id);
                        SearchCriteria.Device.Sort((x, y) => (x - y));
                    }
                }

                selectAll = true;
                foreach (var devicePanel in containerPanel.Controls)
                {
                    var control = devicePanel as SetupDevice.DevicePanel;
                    if(control == null) continue;
                    if (control.IsTitle) continue;
                    if(CMS != null)
                        if(control.Server.Id != panel.Server.Id) continue;

                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }
            else
            {
                if(CMS != null)
                {
                    foreach (NVRDevice nvrDevice in SearchCriteria.NVRDevice)
                    {
                        if(nvrDevice.NVRId == panel.Device.Server.Id && nvrDevice.DeviceId == panel.Device.Id)
                        {
                            SearchCriteria.NVRDevice.Remove(nvrDevice);
                            break;
                        }
                    }
                }
                else
                {
                    SearchCriteria.Device.Remove(panel.Device.Id);
                }
            }

            if (OnSelectChange != null)
                OnSelectChange(null, null);

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as SetupDevice.DevicePanel;

            if(CMS != null)
            {
                foreach (var devicePanel in containerPanel.Controls)
                {
                    var control = devicePanel as SetupDevice.DevicePanel;
                    if (control == null) continue;
                    if(!control.IsTitle) continue;
                    if (control.Server.Id == panel.Server.Id)
                        title = control;
                }
            }
            
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= DevicePanelOnSelectAll;
                title.OnSelectNone -= DevicePanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += DevicePanelOnSelectAll;
                title.OnSelectNone += DevicePanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (var control in containerPanel.Controls)
            {
                if(control is Label) containerPanel.Controls.Remove(control as Control);
            }

            foreach (SetupDevice.DevicePanel devicePanel in containerPanel.Controls)
            {
                devicePanel.SelectionVisible = false;

                devicePanel.Checked = false;
                devicePanel.Device = null;
                devicePanel.Cursor = Cursors.Hand;
                devicePanel.EditVisible = false;
                devicePanel.SelectionVisible = true;

                if (devicePanel.IsTitle)
                {
                    devicePanel.OnSelectAll -= DevicePanelOnSelectAll;
                    devicePanel.OnSelectNone -= DevicePanelOnSelectNone;
                    devicePanel.IsTitle = false;
                }

                if (!_recycleDevice.Contains(devicePanel))
                {
                    _recycleDevice.Enqueue(devicePanel);
                }
            }
            containerPanel.Controls.Clear();
        }

        private void DevicePanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            var titlePanel = sender as SetupDevice.DevicePanel;
            if (titlePanel == null) return;

            var nvr = titlePanel.Server;

            foreach (var devicePanel in containerPanel.Controls.OfType<SetupDevice.DevicePanel>())
            {
                if (devicePanel.Server.Id != nvr.Id) continue;
                devicePanel.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            var titlePanel = sender as SetupDevice.DevicePanel;
            if (titlePanel == null) return;
            var nvr = titlePanel.Server;

            foreach (var devicePanel in containerPanel.Controls.OfType<SetupDevice.DevicePanel>())
            {
                if (devicePanel.Server.Id != nvr.Id) continue;
                devicePanel.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        public Panel ContainerPanel
        {
            get { return containerPanel; }
        }
    }
}
