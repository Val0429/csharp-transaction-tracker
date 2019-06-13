using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ServerProfile;
using SetupDevice;

namespace SetupEvent
{
    public sealed partial class CopyEventHandlePanel : UserControl
    {
        public IServer Server;
        
        public Dictionary<String, String> Localization;

        public CopyEventHandlePanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupEvent_CopyFrom", "Copy From"},
                               };
            Localizations.Update(Localization);
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            copyFromPanel.Paint += CopyFromPanelPaint;

            copyFromComboBox.SelectedIndexChanged += CopyFromComboBoxSelectedIndexChanged;

            if (Server is ICMS)
            {
                nvrComboBox.Visible = true;
                nvrComboBox.SelectedIndexChanged += NvrComboBoxSelectedIndexChanged;
                nvrComboBox.Location = new Point(117, 9);
                copyFromComboBox.Location = new Point(117 + nvrComboBox.DropDownWidth + 5, 9);
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
            if (nvr != null)
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
                if (devicePanel == null) continue;
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

            Manager.PaintText(g, Localization["SetupEvent_CopyFrom"]);
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
            }
            else
            {
                CreateDevicePanel(Server.Device.Devices, Server as INVR);
            }

            Manager.DropDownWidth(copyFromComboBox);
            copyFromComboBox.SelectedIndex = 0;

            //ClearViewModel();

            //if (Server == null) return;

            //var sortResult = new List<IDevice>(Server.Device.Devices.Values);
            //sortResult.Sort((x, y) => (y.Id - x.Id));

            //if (sortResult.Count == 0) return;
            
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
                DataType = "EventHandle",
                Cursor = Cursors.Hand,
                Server = Server,
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
                CopyDeviceEventHandle(panel.Device as ICamera);
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

        private void CopyDeviceEventHandle(ICamera camera)
        {
            if (camera == null) return;

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in camera.EventHandling)
            {
                EventCondition condition = _copyFromDevice.EventHandling.GetEventCondition(
                    obj.Key.CameraEvent.Type, obj.Key.CameraEvent.Id, obj.Key.CameraEvent.Value);

                obj.Value.Clear();
                if (condition == null)
                    continue;

                obj.Key.Interval = condition.Interval;
                var handles = _copyFromDevice.EventHandling[condition];
                foreach (var eventHandle in handles)
                {
                    switch (eventHandle.Type)
                    {
                        case HandleType.Beep:
                            var beepEventHandle = eventHandle as BeepEventHandle;
                            if (beepEventHandle == null) continue;
                            obj.Value.Add(new BeepEventHandle
                                              {
                                                  Times = beepEventHandle.Times,
                                                  Duration = beepEventHandle.Duration,
                                                  Interval = beepEventHandle.Interval,
                                              });
                            break;

                        case HandleType.TriggerDigitalOut:
                            var triggerDigitalOutEventHandle = eventHandle as TriggerDigitalOutEventHandle;
                            if (triggerDigitalOutEventHandle == null) continue;
                            obj.Value.Add(new TriggerDigitalOutEventHandle
                            {
                                Device = triggerDigitalOutEventHandle.Device,
                                DigitalOutId = triggerDigitalOutEventHandle.DigitalOutId,
                                DigitalOutStatus = triggerDigitalOutEventHandle.DigitalOutStatus,
                            });
                            break;

                        case HandleType.SendMail:
                            var sendMailEventHandle = eventHandle as SendMailEventHandle;
                            if (sendMailEventHandle == null) continue;
                            obj.Value.Add(new SendMailEventHandle
                            {
                                MailReceiver = sendMailEventHandle.MailReceiver,
                                Subject = sendMailEventHandle.Subject,
                                Body = sendMailEventHandle.Body,
                                Device = sendMailEventHandle.Device,
                                AttachFile = sendMailEventHandle.AttachFile,
                            });
                            break;

                        case HandleType.UploadFtp:
                            var uploadFtpEventHandle = eventHandle as UploadFtpEventHandle;
                            if (uploadFtpEventHandle == null) continue;
                            obj.Value.Add(new UploadFtpEventHandle
                            {
                                Device = uploadFtpEventHandle.Device,
                                FileName = uploadFtpEventHandle.FileName,
                                TimeStamp = uploadFtpEventHandle.TimeStamp,
                            });
                            break;

                        case HandleType.HotSpot:
                            var hotSpotEventHandle = eventHandle as HotSpotEventHandle;
                            if (hotSpotEventHandle == null) continue;
                            obj.Value.Add(new HotSpotEventHandle
                            {
                                Device = hotSpotEventHandle.Device,
                            });
                            break;

                        case HandleType.ExecCmd:
                            var execEventHandle = eventHandle as ExecEventHandle;
                            if (execEventHandle == null) continue;
                            obj.Value.Add(new ExecEventHandle
                            {
                                FileName = execEventHandle.FileName,
                            });
                            break;

                        case HandleType.Audio:
                            var audioEventHandle = eventHandle as AudioEventHandle;
                            if (audioEventHandle == null) continue;
                            obj.Value.Add(new AudioEventHandle
                            {
                                FileName = audioEventHandle.FileName,
                            });
                            break;

                        case HandleType.GoToPreset:
                            var gotoPresetEventHandle = eventHandle as GotoPresetEventHandle;
                            if (gotoPresetEventHandle == null) continue;
                            obj.Value.Add(new GotoPresetEventHandle
                            {
                                Device = gotoPresetEventHandle.Device,
                                PresetPoint = gotoPresetEventHandle.PresetPoint,
                            });
                            break;

                        case HandleType.PopupPlayback:
                            var popupPlaybackEventHandle = eventHandle as PopupPlaybackEventHandle;
                            if (popupPlaybackEventHandle == null) continue;
                            obj.Value.Add(new PopupPlaybackEventHandle
                            {
                                Device = popupPlaybackEventHandle.Device,
                            });
                            break;

                        case HandleType.PopupLive:
                            var popupLiveEventHandle = eventHandle as PopupLiveEventHandle;
                            if (popupLiveEventHandle == null) continue;
                            obj.Value.Add(new PopupLiveEventHandle
                            {
                                Device = popupLiveEventHandle.Device,
                            });
                            break;
                    }
                }
            }

            if (camera.EventHandling != null)
            {
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
                    devicePanel.IsTitle = false;
                }

                if (!_recycleDevice.Contains(devicePanel))
                    _recycleDevice.Enqueue(devicePanel);
            }
            containerPanel.Controls.Clear();
        }
    } 
}