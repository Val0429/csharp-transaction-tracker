using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Device;
using Interface;
using PanelBase;
using SetupBase;

namespace PeopleCounting
{
    public sealed partial class ListPanel : UserControl
    {
        public event EventHandler<EventArgs<IDevice>> OnDeviceEdit;
        public event EventHandler OnDeviceAdd;
        
        public IApp App;
        public IVAS VAS;
        public Dictionary<String, String> Localization;

        public ListPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   
                                   {"PeopleCounting_MaximumLicense", "Reached maximum license limit \"%1\""},
                                   {"PeopleCounting_AddedDevice", "Added Device"},
                                   {"PeopleCounting_AddDeviceFromNVR", "Add device from NVR..."},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            addedDeviceLabel.Text = Localization["PeopleCounting_AddedDevice"];

            BackgroundImage = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);
        }

        public void Initialize()
        {
            addNewDoubleBufferPanel.Paint += InputPanelPaint;
            addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
            Manager.PaintEdit(g, addNewDoubleBufferPanel);

            Manager.PaintText(g, Localization["PeopleCounting_AddDeviceFromNVR"]);
        }

        private readonly Queue<NVRDeviceComboBoxPanel> _recycleDevice = new Queue<NVRDeviceComboBoxPanel>();
        public void GenerateViewModel()
        {
            ClearViewModel();
            addNewDoubleBufferPanel.Visible = true;

            List<IDevice> sortResult = new List<IDevice>();
            List<UInt16> keys= null;
            if (VAS != null)
                keys = new List<UInt16>(VAS.Device.Devices.Keys);

            if (keys == null) return;
            keys.Sort((x, y) => (y - x));

            foreach (UInt16 key in keys)
            {
                sortResult.Add(VAS.Device.Devices[key]);
            }

            if (sortResult.Count == 0)
            {
                addedDeviceLabel.Visible = false;
                return;
            }
            
            addedDeviceLabel.Visible = true;
            containerPanel.Visible = false;
            foreach (IDevice device in sortResult)
            {
                if (!(device is ICamera)) continue;
                
                var devicePanel = GetNVRDeviceComboBoxPanel();

                devicePanel.VAS = VAS;
                devicePanel.Camera = (ICamera)device;

                containerPanel.Controls.Add(devicePanel);
            }

            AddTitle();
        }

        public void AddDevice()
        {
            INVR nvr = null;
            foreach (KeyValuePair<UInt16, INVR> obj in VAS.NVR.NVRs)
            {
                if(obj.Value.Device != null && obj.Value.Device.Devices.Count > 0)
                {
                    nvr = obj.Value;
                    break;
                }
            }
            if (nvr == null) return;

            var devicePanel = GetNVRDeviceComboBoxPanel();
            
            var copyFrom = nvr.Device.Devices.Values.First() as ICamera;
            if (copyFrom == null) return;
            var camera = new Camera
                             {
                                 Id = VAS.Device.GetNewDeviceId(),
                                 Server = copyFrom.Server,
                                 Profile = copyFrom.Profile,
                                 Name = copyFrom.Name,
                                 Model = copyFrom.Model,
                                 EventSchedule = new Schedule(),
                                 //Rectangles = copyFrom.Rectangles,
                                 //Dispatcher = copyFrom.Dispatcher,
                             };
            camera.EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
            camera.EventSchedule.Description = ScheduleMode.FullTimeEventHandling;

            devicePanel.VAS = VAS;
            devicePanel.Camera = camera;

            VAS.Device.Devices.Add(camera.Id, camera);

            if (VAS.Device.Groups.Values.First() != null)
            {
                if (!VAS.Device.Groups.Values.First().Items.Contains(camera))
                {
                    VAS.Device.Groups.Values.First().Items.Add(camera);
                }
            }

            containerPanel.Controls.Add(devicePanel);
            
            if (containerPanel.Controls.Count == 1)
                AddTitle();
            else
                devicePanel.BringToFront();
        }

        private void AddTitle()
        {
            NVRDeviceComboBoxPanel deviceTitlePanel = GetNVRDeviceComboBoxPanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.HideSetting();
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.EditVisible = false;
            deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
            containerPanel.Controls.Add(deviceTitlePanel);
            containerPanel.Visible = true;

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
            
            addedDeviceLabel.Visible = true;
        }

        private NVRDeviceComboBoxPanel GetNVRDeviceComboBoxPanel()
        {
            if (_recycleDevice.Count > 0)
            {
                return _recycleDevice.Dequeue();
            }

            var devicePanel = new NVRDeviceComboBoxPanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
                VAS = VAS,
            };devicePanel.OnDeviceEditClick += DeviceControlOnDeviceEditClick;

            devicePanel.OnSelectChange += DevicePanelOnSelectChange;

            return devicePanel;
        }

        public Brush SelectedColor{
            set
            {
                foreach (NVRDeviceComboBoxPanel control in containerPanel.Controls)
                    control.SelectedColor = value;
            }
        }

        public void ShowCheckBox()
        {
            addNewDoubleBufferPanel.Visible = addedDeviceLabel.Visible = false;

            foreach (NVRDeviceComboBoxPanel control in containerPanel.Controls)
            {
                control.SelectionVisible = true;
                control.EditVisible = false;
            }

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        public void RemoveSelectedDevices()
        {
            foreach (NVRDeviceComboBoxPanel control in containerPanel.Controls)
            {
                if (!control.Checked || control.Camera == null) continue;

                if (VAS != null)
                {
                    VAS.Device.Devices.Remove(control.Camera.Id);

                    foreach (KeyValuePair<UInt16, IDeviceGroup> obj in VAS.Device.Groups)
                    {
                        obj.Value.Items.Remove(control.Camera);
                    }
                }
            }
        }

        private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnDeviceAdd != null)
                OnDeviceAdd(this, e);
        }

        private void DeviceControlOnDeviceEditClick(Object sender, EventArgs e)
        {
            if (((NVRDeviceComboBoxPanel)sender).Camera != null)
            {
                if (OnDeviceEdit != null)
                    OnDeviceEdit(this, new EventArgs<IDevice>(((NVRDeviceComboBoxPanel)sender).Camera));
            }
        }

        private void ClearViewModel()
        {
            foreach (NVRDeviceComboBoxPanel devicePanel in containerPanel.Controls)
            {
                devicePanel.SelectionVisible = false;

                devicePanel.Checked = false;
                devicePanel.Camera = null;
                devicePanel.Cursor = Cursors.Hand;
                devicePanel.EditVisible = true;
                devicePanel.ShowSetting();
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
            foreach (NVRDeviceComboBoxPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (NVRDeviceComboBoxPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        private void DevicePanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as NVRDeviceComboBoxPanel;
            if (panel == null) return;

            if (panel.Camera == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (NVRDeviceComboBoxPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as NVRDeviceComboBoxPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= DevicePanelOnSelectAll;
                title.OnSelectNone -= DevicePanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += DevicePanelOnSelectAll;
                title.OnSelectNone += DevicePanelOnSelectNone;
            }
        }

        public void CloneSelectedDevices()
        {
            foreach (NVRDeviceComboBoxPanel control in containerPanel.Controls)
            {
                if (!control.Checked || control.Camera == null) continue;

                var copyFrom = control.Camera;

                var id = VAS.Device.GetNewDeviceId();

                if (id == 0)
                {
                    TopMostMessageBox.Show(Localization["PeopleCounting_MaximumLicense"].Replace("%1", VAS.License.Amount.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }

                var camera = new Camera
                {
                    Id = id,
                    Server = copyFrom.Server,
                    Profile = copyFrom.Profile,
                    Name = copyFrom.Name,
                    Model = copyFrom.Model,
                    EventSchedule = new Schedule(),
                };
                camera.EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
                camera.EventSchedule.Description = ScheduleMode.FullTimeEventHandling;
                camera.PeopleCountingSetting = new PeopleCountingSetting
                                                   {
                                                       FeatureThreshold = copyFrom.PeopleCountingSetting.FeatureThreshold,
                                                       FeatureNumberThreshold = copyFrom.PeopleCountingSetting.FeatureNumberThreshold,
                                                       PersonNumber = copyFrom.PeopleCountingSetting.PersonNumber,
                                                       DirectNumber = copyFrom.PeopleCountingSetting.DirectNumber,
                                                       FrameIndex = copyFrom.PeopleCountingSetting.FrameIndex,
                                                       Retry = copyFrom.PeopleCountingSetting.Retry,
                                                       Interval = copyFrom.PeopleCountingSetting.Interval
                                                   };
                camera.Rectangles.Clear();

                foreach (var peopleCountingRectangle in copyFrom.Rectangles)
                {
                    camera.Rectangles.Add(new PeopleCountingRectangle
                            {
                                Rectangle = new Rectangle
                                                {
                                                    X = peopleCountingRectangle.Rectangle.X,
                                                    Y = peopleCountingRectangle.Rectangle.Y,
                                                    Width = peopleCountingRectangle.Rectangle.Width,
                                                    Height = peopleCountingRectangle.Rectangle.Height,
                                                },
                                StartPoint = new Point
                                                {
                                                    X = peopleCountingRectangle.StartPoint.X,
                                                    Y = peopleCountingRectangle.StartPoint.Y
                                                },
                                EndPoint = new Point
                                                {
                                                    X = peopleCountingRectangle.EndPoint.X,
                                                    Y = peopleCountingRectangle.EndPoint.Y
                                                },
                                In = peopleCountingRectangle.In,
                                Out = peopleCountingRectangle.Out,
                                PeopleCountingIn = peopleCountingRectangle.PeopleCountingIn,
                                PeopleCountingOut = peopleCountingRectangle.PeopleCountingOut
                            });
                }

                camera.Dispatcher.Domain = copyFrom.Dispatcher.Domain;

                VAS.Device.Devices.Add(camera.Id, camera);

                if (VAS.Device.Groups.Values.First() != null)
                {
                    if (!VAS.Device.Groups.Values.First().Items.Contains(camera))
                    {
                        VAS.Device.Groups.Values.First().Items.Add(camera);
                    }
                }
            }
        }
    } 
}