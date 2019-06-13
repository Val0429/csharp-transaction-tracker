using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupDevice;

namespace SetupSchedule
{
    public sealed partial class CopySchedulePanel : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public CopySchedulePanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupSchedule_CopyFrom", "Copy From"},
                                   {"SetupSchedule_VideoRecording", "Video Recording"},
                                   {"SetupSchedule_EventHandling", "Event Handling"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            recordCheckBox.Text = Localization["SetupSchedule_VideoRecording"];
            eventHandlingCheckBox.Text = Localization["SetupSchedule_EventHandling"];

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            copyFromPanel.Paint += CopyFromPanelPaint;

            
            copyFromComboBox.SelectedIndexChanged += CopyFromComboBoxSelectedIndexChanged;
            recordCheckBox.CheckedChanged += RecordCheckBoxCheckedChanged;
            eventHandlingCheckBox.CheckedChanged += EventHandlingCheckBoxCheckedChanged;

            if(Server is ICMS)
            {
                nvrComboBox.Visible = true;
                recordCheckBox.Visible = recordCheckBox.Checked = false;
                nvrComboBox.SelectedIndexChanged += NvrComboBoxSelectedIndexChanged;
                nvrComboBox.Location = new Point(117, 9);
                copyFromComboBox.Location = new Point(117 + nvrComboBox.DropDownWidth + 5, 9);
                eventHandlingCheckBox.Location = new Point(246 + nvrComboBox.DropDownWidth + 5, 13);
            }
            else
            {
                nvrComboBox.Visible = false;
            }

        }

        private void NvrComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            copyFromComboBox.SelectedIndexChanged -= CopyFromComboBoxSelectedIndexChanged;

            copyFromComboBox.Items.Clear();

            var nvr = nvrComboBox.SelectedItem as INVR;
            if(nvr != null)
            {
                var sortResult = new List<IDevice>(nvr.Device.Devices.Values);
                sortResult.Sort((x, y) => (y.Id - x.Id));

                if (sortResult.Count == 0) return;

                sortResult.Sort((x, y) => (x.Id - y.Id));
                foreach (IDevice device in sortResult)
                {
                    copyFromComboBox.Items.Add(device);
                }

                Manager.DropDownWidth(copyFromComboBox);
                copyFromComboBox.SelectedIndex = 0;
                CopyFromComboBoxSelectedIndexChanged(this, null);
                _copyFromDevice = copyFromComboBox.SelectedItem as ICamera;
            }

            copyFromComboBox.SelectedIndexChanged += CopyFromComboBoxSelectedIndexChanged;
        }

        private void EventHandlingCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            foreach (var control in containerPanel.Controls)
            {
                var devicePanel = control as DevicePanel;
                if (devicePanel == null) continue;

                devicePanel.Checked = false;
            }

            containerPanel.Enabled = (recordCheckBox.Checked || eventHandlingCheckBox.Checked);

        }

        private void RecordCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            foreach (var control in containerPanel.Controls)
            {
                var devicePanel = control as DevicePanel;
                if (devicePanel == null) continue;

                devicePanel.Checked = false;
            }

            containerPanel.Enabled = (recordCheckBox.Checked || eventHandlingCheckBox.Checked);
        }

        private Boolean _isEditing;
        private ICamera _copyFromDevice;
        private void CopyFromComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if(!containerPanel.Enabled) return;

            if (!(copyFromComboBox.SelectedItem is ICamera)) return;
            _copyFromDevice = copyFromComboBox.SelectedItem as ICamera;

            foreach (var control in containerPanel.Controls)
            {
                var devicePanel = control as DevicePanel;
                if(devicePanel == null) continue;
                devicePanel.Checked = false;
            }

            foreach (var control in containerPanel.Controls)
            {
                var devicePanel = control as DevicePanel;
                if (devicePanel == null) continue;

                if (devicePanel.IsTitle) continue;

                devicePanel.Enabled = (devicePanel.Device != _copyFromDevice);
            }
        }

        private void CopyFromPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, copyFromPanel);

            if (copyFromPanel.Width <= 100) return;

            Manager.PaintText(g, Localization["SetupSchedule_CopyFrom"]);
        }

        private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();
        private Point _previousScrollPosition; 
        public void GenerateViewModel()
        {
            _isEditing = false;

            _previousScrollPosition = containerPanel.AutoScrollPosition;
            _previousScrollPosition.Y *= -1;
            ClearViewModel();

            if (Server == null) return;
            copyFromComboBox.Items.Clear();
            if (Server is ICMS)
            {
                var cms = Server as ICMS;
                var sortResult = new List<INVR>(cms.NVRManager.NVRs.Values);
                sortResult.Sort((x, y) => (x.Id - y.Id));

                nvrComboBox.SelectedIndexChanged -= NvrComboBoxSelectedIndexChanged;
                //copyFromComboBox.SelectedIndexChanged -= NvrComboBoxSelectedIndexChanged;
                nvrComboBox.Items.Clear();
                
                foreach (INVR nvr in sortResult)
                {
                    if (nvr.ReadyState != ReadyState.Ready) continue;
                    if (nvr.Device.Devices.Count == 0) continue; 
                    nvrComboBox.Items.Add(nvr);
                }
                nvrComboBox.SelectedIndex = 0;

                sortResult.Sort((x, y) => (y.Id - x.Id));
                foreach (INVR nvr in sortResult)
                {
                    CreateDevicePanel(nvr.Device.Devices, nvr);
                }

                NvrComboBoxSelectedIndexChanged(this, null);
                nvrComboBox.SelectedIndexChanged += NvrComboBoxSelectedIndexChanged;
                recordCheckBox.Checked = false;
                eventHandlingCheckBox.Checked = true;
            }
            else
            {
                CreateDevicePanel(Server.Device.Devices, Server as INVR);
                copyFromComboBox.SelectedIndex = 0;
                recordCheckBox.Checked = eventHandlingCheckBox.Checked = true;
            }
            copyFromComboBox.SelectedIndex = 0;
            Manager.DropDownWidth(copyFromComboBox);

            //ClearViewModel();

            //if (Server == null) return;

            //var sortResult = new List<IDevice>(Server.Device.Devices.Values);
            //sortResult.Sort((x, y) => (y.Id - x.Id));

            //if (sortResult.Count == 0) return;
            
            //containerPanel.Enabled = true;
            //containerPanel.Visible = false;
            //foreach (IDevice device in sortResult)
            //{
            //    if (device != null && device is ICamera)
            //    {
            //        var devicePanel = GetDevicePanel();

            //        devicePanel.Device = device;
            //        devicePanel.SelectionVisible = true;
            //        containerPanel.Controls.Add(devicePanel);
            //    }
            //}

            //var deviceTitlePanel = GetDevicePanel();
            //deviceTitlePanel.IsTitle = true;
            //deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
            //deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
            //deviceTitlePanel.Cursor = Cursors.Default;
            //deviceTitlePanel.EditVisible = false;
            //containerPanel.Controls.Add(deviceTitlePanel);
            //containerPanel.Visible = true;

            //copyFromComboBox.Items.Clear();

            //sortResult.Sort((x, y) => (x.Id - y.Id));
            //foreach (IDevice device in sortResult)
            //{
            //    copyFromComboBox.Items.Add(device);
            //}

            //Manager.DropDownWidth(copyFromComboBox);
            //copyFromComboBox.SelectedIndex = 0;

            //recordCheckBox.Checked = eventHandlingCheckBox.Checked = true;

            _isEditing = true;
        }

        private void CreateDevicePanel(Dictionary<UInt16, IDevice> devices, INVR nvr)
        {
            if (Server == null) return;

            var sortResult = new List<IDevice>(devices.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0) return;

            containerPanel.Enabled = true;
            containerPanel.Visible = false;
            foreach (IDevice device in sortResult)
            {
                if (device != null && device is ICamera)
                {
                    var devicePanel = GetDevicePanel();

                    devicePanel.Device = device;
                    devicePanel.Server = nvr;
                    devicePanel.SelectionVisible = true;
                    containerPanel.Controls.Add(devicePanel);
                }
            }

            var deviceTitlePanel = GetDevicePanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.EditVisible = false;
            deviceTitlePanel.Server = nvr;
            containerPanel.Controls.Add(deviceTitlePanel);
            containerPanel.Visible = true;
            containerPanel.Focus();
            containerPanel.AutoScrollPosition = _previousScrollPosition;

            sortResult.Sort((x, y) => (x.Id - y.Id));
            foreach (IDevice device in sortResult)
            {
                copyFromComboBox.Items.Add(device);
            }

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
            if (_recycleDevice.Count > 0)
            {
                return _recycleDevice.Dequeue();
            }

            var devicePanel = new DevicePanel
            {
                EditVisible = false,
                SelectionVisible = true,
                DataType = "Schedule",
                Cursor = Cursors.Hand,
                Server = Server,
                //CMS = Server as ICMS
            };
            devicePanel.OnSelectChange += DevicePanelOnSelectChange;
            return devicePanel;
        }

        private void DevicePanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;

            var titlePanel = sender as DevicePanel;
            if (titlePanel == null) return;
            var nvr = titlePanel.Server;

            foreach (var control in containerPanel.Controls)
            {
                var devicePanel = control as DevicePanel;
                if (devicePanel == null) continue;
                if (devicePanel.Server.Id != nvr.Id) continue;
                if (!devicePanel.Enabled) continue;

                devicePanel.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;

            var titlePanel = sender as DevicePanel;
            if (titlePanel == null) return;
            var nvr = titlePanel.Server;

            foreach (var control in containerPanel.Controls)
            {
                var devicePanel = control as DevicePanel;
                if (devicePanel == null) continue;
                if (devicePanel.Server.Id != nvr.Id) continue;

                devicePanel.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectChange(Object sender, EventArgs e)
        {
            if(!_isEditing) return;
            if (_copyFromDevice == null) return;
            var panel = sender as DevicePanel;
            if (panel == null) return;

            if (panel.Device == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                Boolean copyRecord = recordCheckBox.Checked;
                Boolean copyEvent = eventHandlingCheckBox.Checked;
                
                if (copyRecord || copyEvent)
                    CopyDeviceSchedule(panel.Device as ICamera, copyRecord, copyEvent);

                selectAll = true;
                foreach (var control in containerPanel.Controls)
                {
                    var devicePanel = control as DevicePanel;
                    if (devicePanel == null) continue;

                    if (devicePanel.IsTitle) continue;
                    if (!devicePanel.Checked && devicePanel.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as DevicePanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= DevicePanelOnSelectAll;
                title.OnSelectNone -= DevicePanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += DevicePanelOnSelectAll;
                title.OnSelectNone += DevicePanelOnSelectNone;
            }
        }

        private void CopyDeviceSchedule(ICamera camera, Boolean copyRecord, Boolean copyEvent)
        {
            if(camera == null) return;

            if (copyRecord)
            {
                ScheduleModes.Clone(camera.RecordSchedule, _copyFromDevice.RecordSchedule);
                camera.RecordSchedule.Description = _copyFromDevice.RecordSchedule.Description;
                camera.PreRecord = _copyFromDevice.PreRecord;
                camera.PostRecord = _copyFromDevice.PostRecord;
            }

            if (copyEvent)
            {
                ScheduleModes.Clone(camera.EventSchedule, _copyFromDevice.EventSchedule);
                camera.EventSchedule.Description = _copyFromDevice.EventSchedule.Description;

                if (camera.EventHandling != null)
                    camera.EventHandling.ReadyState = ReadyState.Modify;
            }

            Server.DeviceModify(camera);
        }

        private void ClearViewModel()
        {
            foreach (var control in containerPanel.Controls)
            {
                if (control is Label) containerPanel.Controls.Remove(control as Control);
            }

            foreach (DevicePanel devicePanel in containerPanel.Controls)
            {
                devicePanel.SelectionVisible = true;
                
                devicePanel.OnSelectChange -= DevicePanelOnSelectChange;
                devicePanel.Checked = false;
                devicePanel.Device = null;
                devicePanel.Cursor = Cursors.Hand;
                devicePanel.EditVisible = false;
                devicePanel.Enabled = true;
                devicePanel.OnSelectChange += DevicePanelOnSelectChange;

                if (devicePanel.IsTitle)
                {
                    devicePanel.OnSelectAll -= DevicePanelOnSelectAll;
                    devicePanel.OnSelectNone -= DevicePanelOnSelectNone;
                    //deviceControl.OnSortChange -= DeviceControlOnSortChange;
                    devicePanel.IsTitle = false;
                }

                if (!_recycleDevice.Contains(devicePanel))
                    _recycleDevice.Enqueue(devicePanel);
            }
            containerPanel.Controls.Clear();
        }
    } 
}