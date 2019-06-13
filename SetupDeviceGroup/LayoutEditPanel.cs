using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Device;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDeviceGroup
{
    public sealed partial class LayoutEditPanel : UserControl
    {
        public event EventHandler<EventArgs<IDeviceLayout>> OnSubDeviceLayoutEdit;

        public IApp App;
        public IServer Server;
        private INVR _nvr;
        public IDeviceLayout DeviceLayout;
        public Dictionary<String, String> Localization;

        private VideoMonitor.VideoMonitor _monitor;
        public LayoutEditPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DeviceLayoutPanel_Layout", "Layout"},
                                   {"DeviceLayoutPanel_Device", "Device"},
                                   {"DeviceLayoutPanel_Name", "Name"},
                                   {"DeviceLayoutPanel_DeviceResolution", "Device Resolution"},
                                   {"DeviceLayoutPanel_SetupCrop", "Setup Crop"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;

            layoutLabel.Text = Localization["DeviceLayoutPanel_Layout"];

            layoutDoubleBufferPanel.Paint += LayoutPanelPaint;
            nameDoubleBufferPanel.Paint += NamePanelPaint;

            subLayoutDoubleBufferPanel.Paint += SubLayoutPanelPaint;
            resolutionDoubleBufferPanel.Paint += ResolutionPanelPaint;
            device1DoubleBufferPanel.Paint += DevicePanel1Paint;
            device2DoubleBufferPanel.Paint += DevicePanel2Paint;
            device3DoubleBufferPanel.Paint += DevicePanel3Paint;
            device4DoubleBufferPanel.Paint += DevicePanel4Paint;
        }

        public void Initialize()
        {
            _nvr = Server as INVR;
            _monitor = new VideoMonitor.VideoMonitor { App = App, Server = Server };
            _monitor.Initialize();
            _monitor.HidePageLabel();
            _monitor.SetEditProperty();
            _monitor.OnContentChange += MonitorOnContentChange;

            containerPanel.Controls.Add(_monitor);

            nameTextBox.MaxLength = 50;
            nameTextBox.TextChanged += NameTextBoxTextChanged;
            subLayoutDoubleBufferPanel.MouseClick += SubLayoutDoubleBufferPanelMouseClick;

            layoutComboBox.Items.Add("1x1");
            layoutComboBox.Items.Add("1x2");
            layoutComboBox.Items.Add("1x3");
            layoutComboBox.Items.Add("1x4");
            layoutComboBox.Items.Add("2x2");
            layoutComboBox.Items.Add("2x1");
            layoutComboBox.Items.Add("3x1");
            layoutComboBox.Items.Add("4x1");
            layoutComboBox.SelectedIndex = 4;
            layoutComboBox.SelectedIndexChanged += LayoutComboBoxSelectedIndexChanged;

            resolutionComboBox.SelectedIndexChanged += ResolutionComboBoxSelectedIndexChanged;

            device1ComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            device2ComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            device3ComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            device4ComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
        }

        private void SubLayoutDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
        {
            var count = DeviceLayout.Items.Count(device => device != null);
            //no device
            if (count == 0) return;

            if (OnSubDeviceLayoutEdit != null)
                OnSubDeviceLayoutEdit(this, new EventArgs<IDeviceLayout>(DeviceLayout));
        }

        private void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            if (DeviceLayout == null) return;

            UpdateDeviceLayoutItems();
        }

        private void UpdateDeviceLayoutItems()
        {
            DeviceLayout.Items.Clear();
            
            DeviceLayout.Items.Add(device1ComboBox.SelectedItem as IDevice);
            DeviceLayout.Items.Add(device2ComboBox.SelectedItem as IDevice);
            DeviceLayout.Items.Add(device3ComboBox.SelectedItem as IDevice);
            DeviceLayout.Items.Add(device4ComboBox.SelectedItem as IDevice);

            ShowSnapshot();
            _nvr.DeviceLayoutModify(DeviceLayout);
            subLayoutDoubleBufferPanel.Invalidate();

            if (DeviceLayout.Items.Count(device => device != null) == 0)
                subLayoutDoubleBufferPanel.Cursor = Cursors.Default;
            else
                subLayoutDoubleBufferPanel.Cursor = Cursors.Hand;

            _isEdit = false;

            UpdateResolution();
            UpdateDewarpSelection();

            _isEdit = true;
        }

        private Boolean _isEditLayout;
        private void ShowSnapshot()
        {
            _isEditLayout = false;

            _monitor.ClearAll();
            var group = new DeviceGroup {Name = DeviceLayout.Name};
            foreach (var device in DeviceLayout.Items)
            {
                if(device!= null)
                    group.Items.Add(device);
                group.View.Add(device);
            }
            group.Layout = WindowLayouts.LayoutGenerate(DeviceLayout.LayoutX, DeviceLayout.LayoutY);

            _monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(group.Layout));
            _monitor.ShowGroup(group);

            _isEditLayout = true;
        }

        private void MonitorOnContentChange(Object sender, EventArgs<Object> e)
        {
            if (!_isEditLayout) return;

            var devices = e.Value as IDevice[];
            if(devices == null) return;

            DeviceLayout.Items.Clear();
            DeviceLayout.Items.AddRange(devices);

            while (DeviceLayout.Items.Count < 4)
                DeviceLayout.Items.Add(null);

            _isEdit = false;

            var count = DeviceLayout.Items.Count;
            for (var i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    if (DeviceLayout.Items[0] != null)
                        device1ComboBox.SelectedItem = DeviceLayout.Items[0];
                    else
                        device1ComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 1)
                {
                    if (DeviceLayout.Items[1] != null)
                        device2ComboBox.SelectedItem = DeviceLayout.Items[1];
                    else
                        device2ComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 2)
                {
                    if (DeviceLayout.Items[2] != null)
                        device3ComboBox.SelectedItem = DeviceLayout.Items[2];
                    else
                        device3ComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 3)
                {
                    if (DeviceLayout.Items[3] != null)
                        device4ComboBox.SelectedItem = DeviceLayout.Items[3];
                    else
                        device4ComboBox.SelectedIndex = -1;
                    continue;
                }
            }

            _nvr.DeviceLayoutModify(DeviceLayout);
            UpdateDewarpSelection();

            _isEdit = true;
        }

        private void UpdateDewarpSelection()
        {
            dewarpCheckBox1.Visible = dewarpCheckBox2.Visible = dewarpCheckBox3.Visible = dewarpCheckBox4.Visible = false;
            dewarpCheckBox1.Checked = dewarpCheckBox2.Checked = dewarpCheckBox3.Checked = dewarpCheckBox4.Checked = false;

            ICamera camera;
            var count = DeviceLayout.Items.Count;
            for (var i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    if (DeviceLayout.Items[i] != null)
                    {
                        camera = (ICamera) DeviceLayout.Items[i];
                        if (!String.IsNullOrEmpty(camera.Profile.DewarpType))
                        {
                            dewarpCheckBox1.Visible = true;
                            if (DeviceLayout.Dewarps.Count > i)
                                dewarpCheckBox1.Checked = DeviceLayout.Dewarps[i];
                        }
                    }
                }

                if (i == 1)
                {
                    if (DeviceLayout.Items[i] != null)
                    {
                        camera = (ICamera)DeviceLayout.Items[i];
                        if (!String.IsNullOrEmpty(camera.Profile.DewarpType))
                        {
                            dewarpCheckBox2.Visible = true;
                            if (DeviceLayout.Dewarps.Count > i)
                                dewarpCheckBox2.Checked = DeviceLayout.Dewarps[i];
                        }
                    }
                }

                if (i == 2)
                {
                    if (DeviceLayout.Items[i] != null)
                    {
                        camera = (ICamera)DeviceLayout.Items[i];
                        if (!String.IsNullOrEmpty(camera.Profile.DewarpType))
                        {
                            dewarpCheckBox3.Visible = true;
                            if (DeviceLayout.Dewarps.Count > i)
                                dewarpCheckBox3.Checked = DeviceLayout.Dewarps[i];
                        }
                    }
                }

                if (i == 3)
                {
                    if (DeviceLayout.Items[i] != null)
                    {
                        camera = (ICamera)DeviceLayout.Items[i];
                        if (!String.IsNullOrEmpty(camera.Profile.DewarpType))
                        {
                            dewarpCheckBox4.Visible = true;
                            if (DeviceLayout.Dewarps.Count > i)
                                dewarpCheckBox4.Checked = DeviceLayout.Dewarps[i];
                        }
                    }
                }
            }
        }

        private void LayoutComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            if(DeviceLayout == null) return;

            var layout = layoutComboBox.SelectedItem.ToString().Split('x');
            DeviceLayout.LayoutX = Convert.ToUInt16(layout[0]);
            DeviceLayout.LayoutY = Convert.ToUInt16(layout[1]);

            var windowLayout = WindowLayouts.LayoutGenerate(DeviceLayout.LayoutX, DeviceLayout.LayoutY);
            
            var layoutcount = DeviceLayout.LayoutX * DeviceLayout.LayoutY;

            device1ComboBox.Enabled = (device1ComboBox.Items.Count > 0);
            device2ComboBox.Enabled = (device2ComboBox.Items.Count > 0 && layoutcount >= 2);
            device3ComboBox.Enabled = (device3ComboBox.Items.Count > 0 && layoutcount >= 3);
            device4ComboBox.Enabled = (device4ComboBox.Items.Count > 0 && layoutcount >= 4);

            Server.Device.CheckSubLayoutRange(DeviceLayout);

            _monitor.LayoutChange(null, new EventArgs<List<WindowLayout>>(windowLayout));
            _monitor.SetCurrentPage(1);
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private void ResolutionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            if (DeviceLayout == null) return;

            var resolution = Resolutions.ToIndex(resolutionComboBox.SelectedItem.ToString());
            if(resolution == Resolution.NA) return;

            DeviceLayout.WindowWidth = Resolutions.ToWidth(resolution);
            DeviceLayout.WindowHeight = Resolutions.ToHeight(resolution);

            Server.Device.CheckSubLayoutRange(DeviceLayout);
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;
            if (DeviceLayout == null) return;

            DeviceLayout.Name = nameTextBox.Text;
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        //private void CheckSubLayoutRange()
        //{
        //    var width = DeviceLayout.WindowWidth * DeviceLayout.LayoutX;
        //    var height = DeviceLayout.WindowHeight * DeviceLayout.LayoutY;

        //    foreach (var obj in DeviceLayout.SubLayouts)
        //    {
        //        var sublayout = obj.Value;
        //        if (sublayout == null) continue;

        //        //reset over range sublayout to default position
        //        if ((sublayout.X + sublayout.Width > width) || (sublayout.Y + sublayout.Height > height))
        //        {
        //            sublayout.X = 0;
        //            sublayout.Y = 0;
        //            sublayout.Width = DeviceLayout.WindowWidth;
        //            sublayout.Height = DeviceLayout.WindowHeight;
        //        }
        //    }
        //}
        
        private void SubLayoutPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (DeviceLayout.Items.Count(device => device != null) == 0)
            {
                Manager.PaintSingleInput(g, subLayoutDoubleBufferPanel);
            }
            else
            {
                Manager.PaintHighLightInput(g, subLayoutDoubleBufferPanel);
                Manager.PaintEdit(g, subLayoutDoubleBufferPanel);
            }

            Manager.PaintText(g, Localization["DeviceLayoutPanel_SetupCrop"]);
        }

        private void LayoutPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, layoutDoubleBufferPanel);

            Manager.PaintText(g, Localization["DeviceLayoutPanel_Layout"]);
        }

        private void NamePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, nameDoubleBufferPanel);

            Manager.PaintText(g, Localization["DeviceLayoutPanel_Name"]);
        }
        
        private void ResolutionPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, resolutionDoubleBufferPanel);

            Manager.PaintText(g, Localization["DeviceLayoutPanel_DeviceResolution"]);
        }

        private void DevicePanel1Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, device1DoubleBufferPanel);

            Manager.PaintText(g, Localization["DeviceLayoutPanel_Device"] + " 1");
        }

        private void DevicePanel2Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, device2DoubleBufferPanel);

            Manager.PaintText(g, Localization["DeviceLayoutPanel_Device"] + " 2");
        }

        private void DevicePanel3Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, device3DoubleBufferPanel);

            Manager.PaintText(g, Localization["DeviceLayoutPanel_Device"] + " 3");
        }

        private void DevicePanel4Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, device4DoubleBufferPanel);

            Manager.PaintText(g, Localization["DeviceLayoutPanel_Device"] + " 4");
        }

        private Boolean _isEdit;
        public void ParseDeviceLayout()
        {
            if (DeviceLayout == null) return;

            _isEdit = false;

            nameTextBox.Text = DeviceLayout.Name;
            layoutComboBox.SelectedItem = DeviceLayout.LayoutX + "x" + DeviceLayout.LayoutY;

            device1ComboBox.Items.Clear();
            device2ComboBox.Items.Clear();
            device3ComboBox.Items.Clear();
            device4ComboBox.Items.Clear();

            var sortResult = new List<IDevice>(Server.Device.Devices.Values);
            sortResult.Sort((x, y) => (x.Id - y.Id));

            foreach (var camera in sortResult)
            {
                if (!(camera is ICamera)) continue;

                device1ComboBox.Items.Add(camera);
                device2ComboBox.Items.Add(camera);
                device3ComboBox.Items.Add(camera);
                device4ComboBox.Items.Add(camera);
            }
            Manager.DropDownWidth(device1ComboBox);
            Manager.DropDownWidth(device2ComboBox);
            Manager.DropDownWidth(device3ComboBox);
            Manager.DropDownWidth(device4ComboBox);

            var layoutcount = DeviceLayout.LayoutX * DeviceLayout.LayoutY;

            device1ComboBox.Enabled = (device1ComboBox.Items.Count > 0);
            device2ComboBox.Enabled = (device2ComboBox.Items.Count > 0 && layoutcount >= 2);
            device3ComboBox.Enabled = (device3ComboBox.Items.Count > 0 && layoutcount >= 3);
            device4ComboBox.Enabled = (device4ComboBox.Items.Count > 0 && layoutcount >= 4);

            var count = DeviceLayout.Items.Count;
            for (var i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    if (DeviceLayout.Items[i] != null)
                        device1ComboBox.SelectedItem = DeviceLayout.Items[i];
                    else
                        device1ComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 1)
                {
                    if (DeviceLayout.Items[i] != null)
                        device2ComboBox.SelectedItem = DeviceLayout.Items[i];
                    else
                        device2ComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 2)
                {
                    if (DeviceLayout.Items[i] != null)
                        device3ComboBox.SelectedItem = DeviceLayout.Items[i];
                    else
                        device3ComboBox.SelectedIndex = -1;
                    continue;
                }
                if (i == 3)
                {
                    if (DeviceLayout.Items[i] != null)
                        device4ComboBox.SelectedItem = DeviceLayout.Items[i];
                    else
                        device4ComboBox.SelectedIndex = -1;
                    continue;
                }
            }

            UpdateDewarpSelection();
            UpdateResolution();
            ShowSnapshot();

            subLayoutDoubleBufferPanel.Invalidate();
            if (DeviceLayout.Items.Count(device => device != null) == 0)
                subLayoutDoubleBufferPanel.Cursor = Cursors.Default;
            else
                subLayoutDoubleBufferPanel.Cursor = Cursors.Hand;

            _isEdit = true;

            containerPanel.Focus();
        }

        private void UpdateResolution()
        {
            resolutionComboBox.Items.Clear();
            var resolutions = new List<Resolution> { Resolution.R640X480 };

            foreach (var device in DeviceLayout.Items)
            {
                var camera = device as ICamera;
                if (camera != null && camera.StreamConfig.Resolution != Resolution.NA && !resolutions.Contains(camera.StreamConfig.Resolution))
                {
                    resolutions.Add(camera.StreamConfig.Resolution);
                }
            }

            resolutions.Sort();
            foreach (var resolution in resolutions)
            {
                resolutionComboBox.Items.Add(Resolutions.ToString(resolution));
            }
            var deviceResolution = Resolutions.ToIndex(DeviceLayout.WindowWidth + "x" + DeviceLayout.WindowHeight);
            if (resolutions.Contains(deviceResolution))
                resolutionComboBox.SelectedItem = Resolutions.ToString(deviceResolution);
            else
            {
                DeviceLayout.WindowWidth = 640;
                DeviceLayout.WindowHeight = 480;
                resolutionComboBox.SelectedItem = Resolutions.ToString(Resolution.R640X480);
            }
        }

        private void DewarpCheckBox1CheckedChanged(Object sender, EventArgs e)
        {
            if(!_isEdit) return;

            while (DeviceLayout.Dewarps.Count < 4)
            {
                DeviceLayout.Dewarps.Add(false);
            }

            DeviceLayout.Dewarps[0] = dewarpCheckBox1.Checked;
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private void DewarpCheckBox2CheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;

            while (DeviceLayout.Dewarps.Count < 4)
            {
                DeviceLayout.Dewarps.Add(false);
            }

            DeviceLayout.Dewarps[1] = dewarpCheckBox2.Checked;
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private void DewarpCheckBox3CheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;

            while (DeviceLayout.Dewarps.Count < 4)
            {
                DeviceLayout.Dewarps.Add(false);
            }

            DeviceLayout.Dewarps[2] = dewarpCheckBox3.Checked;
            _nvr.DeviceLayoutModify(DeviceLayout);
        }

        private void DewarpCheckBox4CheckedChanged(Object sender, EventArgs e)
        {
            if (!_isEdit) return;

            while (DeviceLayout.Dewarps.Count < 4)
            {
                DeviceLayout.Dewarps.Add(false);
            }

            DeviceLayout.Dewarps[3] = dewarpCheckBox4.Checked;
            _nvr.DeviceLayoutModify(DeviceLayout);
        }
    } 
}
