using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupServer
{
    public partial class DeviceKeepDaysRecording : UserControl
    {
        public event EventHandler<EventArgs<IDevice>> OnDeviceEdit;

        public IServer Server;
        public Dictionary<String, String> Localization;
        private const UInt16 MinimumDays = 1;
        private const UInt16 MaximumDays = 365;
        private const UInt16 DefaultDays = 90;

        public void Initialize()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupServer_EnabledKeepDays", "Enable device keep days recording"},
                                   {"SetupServer_KeepDaysOfNewlyAddedDevices", "Keep Days of newly added devices"},
                                   {"SetupServer_ApplyAllDevicesToKeepDays", "Apply to all devices"},
                                   {"SetupServer_Enabled", "Enabled"},
                                   {"SetupServer_KeepDayWarning", "Device keep days recording is between %1 ~ %2 days."}
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.BackgroundNoBorder;

            enableKeepDaysPanel.Paint += InputPanelPaint;
            applyAllPanel.Paint += InputPanelPaint;

            enabledCheckBox.Text = Localization["SetupServer_Enabled"];
            enabledCheckBox.Click += EnabledCheckBoxClick;

            for (int i = MinimumDays; i <= MaximumDays; i++)
                defaultDaysComboBox.Items.Add(i);

            defaultDaysComboBox.SelectedIndex = 0;
            defaultDaysComboBox.KeyPress += KeyAccept.AcceptNumberOnly;
            defaultDaysComboBox.TextChanged -= DefaultDaysChanged;
            defaultDaysComboBox.TextChanged += DefaultDaysChanged;

            setButton.Text = Localization["SetupServer_ApplyAllDevicesToKeepDays"];
            setButton.Click += SetButtonClick;

            infoLabel.Text = Localization["SetupServer_KeepDayWarning"].Replace("%1", MinimumDays.ToString()).Replace("%2", MaximumDays.ToString());
        }

        private void SetButtonClick(object sender, EventArgs e)
        {
            if (_applyAll) return;
            _applyAll = true;
            defaultDaysComboBox.TextChanged -= DefaultDaysChanged;
            var value = String.IsNullOrEmpty(defaultDaysComboBox.Text) ? 1 : Convert.ToInt16(defaultDaysComboBox.Text);
            if (value > MaximumDays) value = (short)MaximumDays;
            if (value < 1) value = (short)MinimumDays;

            foreach (DevicePanel control in containerPanel.Controls)
            {
                if (control.Tag == null) continue;
                control.KeepDays = (ushort)value;
                if (Server.Server.DeviceRecordKeepDays.ContainsKey((ushort)control.Tag))
                {
                    Server.Server.DeviceRecordKeepDays[(ushort)control.Tag] = (ushort)value;
                }
                else
                {
                    Server.Server.DeviceRecordKeepDays.Add((ushort)control.Tag, (ushort)value);
                }
            }
            defaultDaysComboBox.TextChanged += DefaultDaysChanged;
            _applyAll = false;
        }

        private Boolean _applyAll = false;
        private void DefaultDaysChanged(object sender, EventArgs e)
        {
            if (_applyAll) return;
            _applyAll = true;
            defaultDaysComboBox.TextChanged -= DefaultDaysChanged;
            var value = String.IsNullOrEmpty(defaultDaysComboBox.Text) ? 1 : Convert.ToInt16(defaultDaysComboBox.Text);
            if (value > MaximumDays) value = (short) MaximumDays;
            if (value < 1) value = (short) MinimumDays;

            Server.Server.DefaultDeepDays = (ushort) value;
            defaultDaysComboBox.TextChanged += DefaultDaysChanged;
            _applyAll = false;
        }

        private DevicePanel _titlePanel;
        private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();

        private void EnabledCheckBoxClick(object sender, EventArgs e)
        {
            Server.Server.KeepDaysEnabled = enabledCheckBox.Checked;
            //if (enabledCheckBox.Checked)
            //{
            //    foreach (DevicePanel control in containerPanel.Controls)
            //    {
            //        if (control.Tag == null) continue;
            //        if (!Server.Server.DeviceRecordKeepDays.ContainsKey((ushort)control.Tag))
            //            Server.Server.DeviceRecordKeepDays.Add((ushort)control.Tag, control.KeepDays);
            //        control.InUse = true;
            //    }
            //}
            //else
            //{
            //    foreach (DevicePanel control in containerPanel.Controls)
            //        control.InUse = false;
            //    Server.Server.DeviceRecordKeepDays.Clear();
            //}
            foreach (DevicePanel control in containerPanel.Controls)
                control.InUse = Server.Server.KeepDaysEnabled;

            //applyAllComboBox.Enabled = enabledCheckBox.Checked;
            //GenerateViewModel();
        }

        public void GenerateViewModel()
        {
            enabledCheckBox.Checked = Server.Server.KeepDaysEnabled;
            defaultDaysComboBox.SelectedIndex = Server.Server.DefaultDeepDays-1;
            //applyAllComboBox.Enabled = enabledCheckBox.Checked;

            foreach (DevicePanel control in containerPanel.Controls)
            {
                if(control.Tag == null) continue;
                if (!_recycleDevice.Contains(control))
                    _recycleDevice.Enqueue(control); 
            }
            containerPanel.Controls.Clear();

            if (_titlePanel == null && Server.Device.Devices.Count>0)
            {
                 _titlePanel = GetDevicePanel();
                _titlePanel.Cursor = Cursors.Default;
                containerPanel.Visible = true;
            }

            if (_titlePanel != null) containerPanel.Controls.Add(_titlePanel);

            _applyAll = true;

            var sortResult = new List<IDevice>(Server.Device.Devices.Values);
            //reverse
            sortResult.Sort((x, y) => (x.Id - y.Id));

            foreach (IDevice obj in sortResult)
            {
                DevicePanel devicePanel = GetDevicePanel();
                devicePanel.Tag = obj.Id;
                devicePanel.DisplayEditor = true;
                
                devicePanel.Device = obj;
                devicePanel.InUse = enabledCheckBox.Checked;
                //devicePanel.InUse = Server.Server.DeviceRecordKeepDays.ContainsKey(obj.Id);
                devicePanel.KeepDays = (ushort) (Server.Server.DeviceRecordKeepDays.ContainsKey(obj.Id) ? Server.Server.DeviceRecordKeepDays[obj.Id] : DefaultDays);

                containerPanel.Controls.Add(devicePanel);
                devicePanel.BringToFront();

                devicePanel.Enabled = true;
                //devicePanel.MouseClick += DevicePanelMouseClick;
            }
            _applyAll = false;

            containerPanel.BringToFront();
            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
            containerPanel.AutoScrollPosition = new Point(0,0);
            //CheckAllTheSame();
        }

        private DevicePanel GetDevicePanel()
        {
            if (_recycleDevice.Count > 0)
            {
                return _recycleDevice.Dequeue();
            }

            var devicePanel = new DevicePanel
            {
                SelectedColor = Manager.SelectedTextColor,
                Server = Server,
            };
            devicePanel.OnKeepDaysChange += DevicePanelOnKeepDaysChange;

            return devicePanel;
        }

        private void CheckAllTheSame()
        {
            _applyAll = true;
            var same = true;
            var tempValue = -1;

            foreach (DevicePanel control in containerPanel.Controls)
            {
                if(control.Tag == null) continue;
                if(tempValue == -1)
                {
                    tempValue = control.KeepDays;
                    continue;
                }

                if (tempValue != control.KeepDays)
                {
                    same = false;
                    break;
                }
            }

            if (tempValue == -1) tempValue = DefaultDays;
            defaultDaysComboBox.TextChanged -= DefaultDaysChanged;
            defaultDaysComboBox.Text = same ? tempValue.ToString() : String.Empty;
            defaultDaysComboBox.TextChanged += DefaultDaysChanged;
            _applyAll = false;
        }

        private void DevicePanelOnKeepDaysChange(object sender, EventArgs e)
        {
            if (!(sender is DevicePanel) || ((DevicePanel)sender).Tag == null || _applyAll) return;
            var devicePanel = sender as DevicePanel;
            if (!devicePanel.InUse) return;

            if (Server.Server.DeviceRecordKeepDays.ContainsKey((ushort) devicePanel.Tag))
            {
                Server.Server.DeviceRecordKeepDays[(ushort)devicePanel.Tag] = devicePanel.KeepDays;
            }
            else
            {
                Server.Server.DeviceRecordKeepDays.Add((ushort)devicePanel.Tag, devicePanel.KeepDays);
            }
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("SetupServer_" + control.Tag))
                Manager.PaintText(g, Localization["SetupServer_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

    }

    public sealed class DevicePanel : Panel
    {
        public UInt16 Order;
        public IServer Server;
        public IDevice Device;
        public event EventHandler OnKeepDaysChange;
        public Dictionary<String, String> Localization;
        private const UInt16 MinimumDays = 1;
        private const UInt16 MaximumDays = 365;
        private Boolean _inUse;
        public Boolean InUse
        {
            get
            {
                return _inUse;
            }
            set
            {
                _keepSpaceComboBox.Enabled = _inUse = value;
            }
        }
        private readonly ComboBox _keepSpaceComboBox;
        public DevicePanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePanel_ID", "ID"},
                                   {"DevicePanel_Name", "Name"},
                                   {"StoragePanel_KeepDays", "Keep Days"}
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;

            _keepSpaceComboBox = new ComboBox
            {
                Width = 45,
                Dock = DockStyle.None,
                Location = new Point(310, 10),
                Visible = false,
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                MaxLength = 4,
                Enabled = false,
                ImeMode = ImeMode.Disable
            };

            for (int i = MinimumDays; i <= MaximumDays; i++)
                _keepSpaceComboBox.Items.Add(i);

            _keepSpaceComboBox.KeyPress += KeyAccept.AcceptNumberOnly;
            _keepSpaceComboBox.TextChanged += KeepDaysComboBoxChanged;
            Controls.Add(_keepSpaceComboBox);

            Paint += StoragePanelPaint;
        }

        public UInt16 KeepDays
        {
            get
            {
                UInt32 space = (_keepSpaceComboBox.Text != "") ? Convert.ToUInt32(_keepSpaceComboBox.Text) : 1;

                return Convert.ToUInt16(Math.Min(Math.Max(space, MinimumDays), MaximumDays));
            }
            set
            {
                _keepSpaceComboBox.Text = value.ToString();
            }
        }

        public Boolean DisplayEditor
        {
            set
            {
                _keepSpaceComboBox.Visible = value;
            }
        }

        private void KeepDaysComboBoxChanged(Object sender, EventArgs e)
        {
            if (OnKeepDaysChange != null)
                OnKeepDaysChange(this, null);
        }

        public Brush SelectedColor = Manager.SelectedTextColor;

        private void StoragePanelPaint(Object sender, PaintEventArgs e)
        {
            if (Server == null) return;
            if (Server.Server.StorageInfo.Count == 0) return;

            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (Width <= 100) return;

            if (Tag == null)
            {
                Manager.PaintTitleTopInput(g, this);
                Manager.PaintTitleText(g, Localization["DevicePanel_ID"]);
                
                if (Width <= 210) return;
                g.DrawString(Localization["DevicePanel_Name"], Manager.Font, Manager.TitleTextColor, 80, 13);

                if (Width <= 310) return;
                g.DrawString(Localization["StoragePanel_KeepDays"], Manager.Font, Manager.TitleTextColor, 310, 13);

                return;
            }

            Manager.Paint(g, this);
            Brush brush = Brushes.Black;
            //if (_inUse)
            //{
            //    brush = SelectedColor;
            //    Manager.PaintSelected(g);
            //}
            if (!Enabled)
                brush = Brushes.Gray;

            g.DrawString(Device.Id.ToString(), Manager.Font, brush, 40, 13); 
        
            if (Width <= 210) return;
            g.DrawString(Device.Name, Manager.Font, brush, 80, 13);

            if (Width <= 310) return;
            //g.DrawString((DiskInfo.Total / Gb2Byte).ToString("0") + " GB", Manager.Font, brush, 216, 13);
        }
    }
}