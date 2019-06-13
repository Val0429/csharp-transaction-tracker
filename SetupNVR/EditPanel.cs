using Constant;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Manager = SetupBase.Manager;

namespace SetupNVR
{
    public partial class EditPanel : UserControl
    {
        public IServer Server;

        public INVR NVR;
        public ICMS CMS;
        public IVAS VAS;
        public IFOS FOS;
        public IPTS PTS;
        public Dictionary<String, String> Localization;
        public event EventHandler<EventArgs<IDevice>> OnDeviceEdit;
        public event EventHandler<EventArgs> OnDeviceSelect;
        public Boolean IsEditing;

        private readonly List<String> _modifyed = new List<string>();

        public EditPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "Sec"},
                                   {"Common_Min", "Min"},

                                   {"MessageBox_Information", "Information"},
                                   
                                   {"NVR_Name", "Name"},
                                   {"NVR_Manufacture", "Manufacture"},
                                   {"NVR_Domain", "Domain"},
                                   {"NVR_Port", "Port"},
                                   {"NVR_ServerPort", "Server Port"},
                                   {"NVR_Account", "Account"},
                                   {"NVR_Password", "Password"},
                                   {"NVR_Event", "Event"},
                                   {"NVR_Patrol", "Patrol"},
                                   {"NVR_ListenEvent", "Events receiving and actions in Client"},
                                   {"NVR_LaunchTime", "Failover Launch Time"},
                                   {"NVR_BlockSize", "Block Size"},
                                   {"NVR_SSLConnection", "SSL Connection"},
                                   {"NVR_ServerStatusCheckInterval", "Server Status Check Interval (10 ~ 600 Seconds)"},
                                   {"EditNVRPanel_Include", "Include"},
                                   {"EditNVRPanel_Enable", "Enable"},
                                   {"EditNVRPanel_Information", "Information"},

                                    {"EditNVRPanel_Device", "Device"},
                                    {"SetupDevice_SearchNoResult", "No Device Found"},
								   {"EditNVRPanel_NumDevice", "%1 Device"},
								   {"EditNVRPanel_NumDevices", "%1 Devices"},
								   {"SetupDevice_DeviceSelected", "%1 Device Selected"},
								   {"SetupDevice_DevicesSelected", "%1 Devices Selected"},
                                   {"SetupNVR_MaximumLicense", "Reached maximum license limit \"%1\""},
                               };
            Localizations.Update(Localization);
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            accountTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            listenEventCheckBox.Text = Localization["EditNVRPanel_Enable"];
            patrolCheckBox.Text = Localization["EditNVRPanel_Include"];
            sslCheckBox.Text = Localization["EditNVRPanel_Enable"];
            addedNVRLabel.Text = Localization["EditNVRPanel_Device"];

            sslConnectionPanel.Visible = false;

            BackgroundImage = Manager.BackgroundNoBorder;
        }


        private readonly UInt16[] _portArray = new UInt16[] { 80, 82, 443, 8080, 8088 };
        private readonly UInt32[] _lanuchTime = new UInt32[] { 30000, 40000, 50000, 60000, 120000, 180000, 240000, 300000, 360000, 420000, 480000, 540000, 600000 };
        private readonly UInt32[] _blockSize = new UInt32[] {/* 4096, 8192, 16384, 32768, 65536, 131072, 262144, */524288, 786432, 1048576, 2097152, 3145728, 4194304, 5242880, 6291456, 7340032, 8388608 }; //512K~8M
        private String[] _manufacture;
        private readonly Dictionary<UInt32, String> _lanuchStr = new Dictionary<UInt32, String>();
        private readonly Dictionary<UInt32, String> _blockStr = new Dictionary<UInt32, String>();

        protected virtual string[] CreateManufactureList()
        {
            if (PTS != null)
            {
                return new[] { PTS.ReleaseBrand };
            }

            if (CMS != null)
            {
                return new[] { "iSAP", "Diviotec", "Hikvision", "Milestone", "Salient", "VioStor", "DIGIEVER", "SAMSUNG", "iSAP Failover Server", "ACTi", "ACTi Enterprise", "ShanyLink", "Digifort", "VIVOTEK", "3TSmart", "Siemens", "Certis", "Customization" };//, "Digifort", "VIVOTEK", "ACTi" 
            }

            return new string[] { };
        }

        public void Initialize()
        {
            var list = CreateManufactureList();
            Array.Sort(list);
            _manufacture = list;

            containerPanel.Paint += ContainerPanelPaint;
            
            namePanel.Paint += PaintInput;
            domainPanel.Paint += PaintInput;
            portPanel.Paint += PaintInput;
            serverPortPanel.Paint += PaintInput;
            sslConnectionPanel.Paint += PaintInput;
            accountPanel.Paint += PaintInput;
            passwordPanel.Paint += PaintInput;

            if (CMS != null)
            {
                manufacturePanel.Paint += PaintInput;
                eventPanel.Paint += PaintInput;
                patrolPanel.Paint += PaintInput;
                serverStatusIntervalDoubleBufferPanel.Paint += PaintInput;

                serverStatusCheckIntervalComboBox.Items.Add(10);
                serverStatusCheckIntervalComboBox.Items.Add(30);
                serverStatusCheckIntervalComboBox.Items.Add(60);
                serverStatusCheckIntervalComboBox.Items.Add(180);
                serverStatusCheckIntervalComboBox.Items.Add(300);
                serverStatusCheckIntervalComboBox.Items.Add(600);

                foreach (String manufacture in _manufacture)
                {
                    manufactureComboBox.Items.Add(manufacture);
                }
                manufactureComboBox.SelectedIndex = 0;
                manufactureComboBox.Enabled = (_manufacture.Length > 1);
                manufacturePanel.Visible = manufactureComboBox.Items.Count > 1;
            }
            else if (FOS != null)
            {
                launchTimePanel.Paint += PaintInput;
                blockSizePanel.Paint += PaintInput;

                launchTimePanel.Visible = blockSizePanel.Visible =
                patrolPanel.Visible = true;
                
                manufacturePanel.Visible = 
                addedNVRLabel.Visible =
                serverPortPanel.Visible =
                sslConnectionPanel.Visible = eventPanel.Visible = patrolPanel.Visible = false;
            }
            else if (PTS != null)
            {
                manufacturePanel.Paint += PaintInput;
                patrolPanel.Paint += PaintInput;

                patrolPanel.Visible = true;
                addedNVRLabel.Visible =
                eventPanel.Visible = launchTimePanel.Visible = false;

                manufacturePanel.Visible = true;
                serverPortPanel.Visible = false;
                foreach (String manufacture in _manufacture)
                {
                    manufactureComboBox.Items.Add(manufacture);
                }
                manufactureComboBox.SelectedIndex = 0;
                manufactureComboBox.Enabled = (_manufacture.Length > 1);
            }
            else if (VAS != null)
            {
                eventPanel.Visible = patrolPanel.Visible =
                sslConnectionPanel.Visible = launchTimePanel.Visible = patrolPanel.Visible = false;
            }

            foreach (UInt16 port in _portArray)
            {
                portComboBox.Items.Add(port);
            }

            serverPortComboBox.Items.Add(8000);
            serverPortComboBox.SelectedItem = 8000;

            portComboBox.KeyPress += KeyAccept.AcceptNumberOnly;
            serverStatusCheckIntervalComboBox.KeyPress += KeyAccept.AcceptNumberOnly;

            nameTextBox.TextChanged += NameTextBoxTextChanged;
            domainTextBox.TextChanged += DomainTextBoxTextChanged;
            portComboBox.TextChanged += PortComboBoxTextChanged;
            serverPortComboBox.TextChanged += ServerPortComboBoxTextChanged;
            serverStatusCheckIntervalComboBox.TextChanged += ServerStatusCheckIntervalComboBoxTextChanged;
            accountTextBox.TextChanged += AccountTextBoxTextChanged;
            passwordTextBox.TextChanged += PasswordTextBoxTextChanged;

            nameTextBox.LostFocus += NameTextBoxLostFocus;
            domainTextBox.LostFocus += DomainTextBoxLostFocus;
            portComboBox.LostFocus += PortComboBoxLostFocus;
            accountTextBox.LostFocus += AccountTextBoxLostFocus;
            passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
            if (CMS != null)
            {
                manufactureComboBox.SelectedIndexChanged += ManufactureComboBoxSelectedIndexChanged;
                listenEventCheckBox.CheckedChanged += ListenEventCheckBoxCheckedChanged;
                patrolCheckBox.CheckedChanged += PatrolCheckBoxCheckedChanged;
                sslCheckBox.CheckedChanged += SSLCheckBoxCheckedChanged;
            }
            else if (FOS != null)
            {
                foreach (UInt32 time in _lanuchTime)
                {
                    var str = DurationToText(time);
                    _lanuchStr.Add(time, str);
                    launchTimeComboBox.Items.Add(str);
                }

                foreach (UInt32 block in _blockSize)
                {
                    var str = SizeToText(block);
                    _blockStr.Add(block, str);
                    blockSizeComboBox.Items.Add(str);
                }

                launchTimeComboBox.SelectedIndexChanged += LaunchTimeComboBoxSelectedIndexChanged;
                blockSizeComboBox.SelectedIndexChanged += BlockSizeComboBoxSelectedIndexChanged;
            }
            else if (PTS != null)
            {
                manufactureComboBox.SelectedIndexChanged += ManufactureComboBoxSelectedIndexChanged;
                patrolCheckBox.CheckedChanged += PatrolCheckBoxCheckedChanged;
                sslCheckBox.CheckedChanged += SSLCheckBoxCheckedChanged;
            }
        }

        private Boolean _isEditing;
        private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();
        public void GenerateViewModel()
        {
            GenerateViewModel(NVR.TempDevices);
            CheckedSelectedDevice();
        }

        public void GenerateViewModel(Dictionary<UInt16, IDevice> devices)
        {
            _isEditing = false;
            ClearViewModel();

            var sortResult = new List<IDevice>(devices.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                addedNVRLabel.Text = Localization["SetupDevice_SearchNoResult"].Replace("%1", sortResult.Count.ToString());
                _isEditing = true;
                return;
            }

            addedNVRLabel.Text = sortResult.Count == 1 ? Localization["EditNVRPanel_NumDevice"].Replace("%1", (sortResult.Count).ToString()) : Localization["EditNVRPanel_NumDevices"].Replace("%1", (sortResult.Count).ToString());

            deviceContainer.Enabled = true;
            deviceContainer.Visible = false;

            foreach (IDevice device in sortResult)
            {
                if (device != null && device is ICamera)
                {
                    var devicePanel = GetDevicePanel();

                    devicePanel.Device = device;
                    devicePanel.SelectionVisible = true;
                    devicePanel.EditVisible = true;
                    devicePanel.OnDeviceEditClick += DevicePanelOnDeviceEditClick;
                    deviceContainer.Controls.Add(devicePanel);
                }
            }

            var deviceTitlePanel = GetDevicePanel();
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.EditVisible = false;
            deviceTitlePanel.CMS = CMS;
            deviceContainer.Controls.Add(deviceTitlePanel);
            deviceContainer.Visible = devices.Count > 0;
            sortResult.Clear();
            _isEditing = true;
        }

        private void DevicePanelOnDeviceEditClick(object sender, EventArgs e)
        {
            if (((DevicePanel)sender).Device != null)
            {
                if (OnDeviceEdit != null)
                    OnDeviceEdit(this, new EventArgs<IDevice>(((DevicePanel)sender).Device));
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
                Cursor = Cursors.Hand,
                Server = Server,
            };
            devicePanel.OnSelectChange += DevicePanelOnSelectChange;

            return devicePanel;
        }

        private void DevicePanelOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;
            var panel = sender as DevicePanel;
            if (panel == null) return;

            _isEditing = false;

            if (panel.Checked)
            {
                if (CMS.NVRManager.DeviceChannelTable.Count >= Server.License.Amount)//Server.License.Amount
                {
                    TopMostMessageBox.Show(Localization["SetupNVR_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _isEditing = true;
                    panel.Checked = false;
                    return;
                }
                CMS.NVRManager.AddDeviceChannelTable(panel.Device);
            }
            else
            {
                CMS.NVRManager.RemoveDeviceChannelTable(panel.Device);
            }

            NVR.Device.Devices.Clear();
            foreach (DevicePanel control in deviceContainer.Controls)
            {
                if (control.Device == null) continue;
                if (!control.Checked)
                {
                    control.Device.ReadyState = ReadyState.JustAdd;
                    continue;
                }

                if (!NVR.Device.Devices.ContainsKey(control.Device.Id))
                {
                    control.Device.ReadyState = ReadyState.Modify;
                    NVR.Device.Devices.Add(control.Device.Id, control.Device);
                }

            }

            var resultCount = deviceContainer.Controls.Count - 1;
            addedNVRLabel.Text = resultCount == 1 ? Localization["EditNVRPanel_NumDevice"].Replace("%1", resultCount.ToString()) : Localization["EditNVRPanel_NumDevices"].Replace("%1", resultCount.ToString());

            if (NVR.ReadyState != ReadyState.New)
                NVR.ReadyState = ReadyState.Modify;
            CMS.NVRModify(NVR);
            if (NVR.Device.Devices.Count == 0)
            {
                _isEditing = true;
                if (OnDeviceSelect != null)
                    OnDeviceSelect(this, new EventArgs());
                return;
            }
            ChangeAddLabelStringBySelectedDevice();
            foreach (DevicePanel control in deviceContainer.Controls)
            {
                if (!control.IsTitle) continue;

                control.Checked = NVR.Device.Devices.Count == deviceContainer.Controls.Count - 1;
            }
            _isEditing = true;
        }

        private void ChangeAddLabelStringBySelectedDevice()
        {
            addedNVRLabel.Text += @" - ";
            addedNVRLabel.Text += (NVR.Device.Devices.Count == 1) ? Localization["SetupDevice_DeviceSelected"].Replace("%1", NVR.Device.Devices.Count.ToString()) : Localization["SetupDevice_DevicesSelected"].Replace("%1", NVR.Device.Devices.Count.ToString());

            if (OnDeviceSelect != null)
                OnDeviceSelect(this, new EventArgs());
        }

        private List<DevicePanel> _temp = new List<DevicePanel>();
        private void DevicePanelOnSelectAll(Object sender, EventArgs e)
        {
            if (!_isEditing) return;
            if (CMS.NVRManager.DeviceChannelTable.Count >= Server.License.Amount)//Server.License.Amount
            {
                TopMostMessageBox.Show(Localization["SetupNVR_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            deviceContainer.AutoScroll = false;
            _temp.Clear();

            foreach (DevicePanel control in deviceContainer.Controls)
            {
                _temp.Add(control);
            }

            _temp.Reverse();

            foreach (DevicePanel control in _temp)
            {
                if (!control.Enabled) continue;

                control.Checked = true;
            }
            deviceContainer.AutoScroll = true;
            NVR.ReadyState = ReadyState.Modify;
            CMS.NVRModify(NVR);
            _temp.Clear();
        }

        private void DevicePanelOnSelectNone(Object sender, EventArgs e)
        {
            if (!_isEditing) return;
            deviceContainer.AutoScroll = false;
            foreach (DevicePanel control in deviceContainer.Controls)
            {
                if (control.IsTitle) continue;
                control.Checked = false;
                control.Device.ReadyState = ReadyState.JustAdd;
            }
            deviceContainer.AutoScroll = true;
            NVR.ReadyState = ReadyState.Modify;
            CMS.NVRModify(NVR);
        }

        private readonly Dictionary<UInt16, IDevice> _removedDevices = new Dictionary<UInt16, IDevice>();
        private void CheckedSelectedDevice()
        {
            _isEditing = false;
            _removedDevices.Clear();

            foreach (KeyValuePair<UInt16, IDevice> device in NVR.Device.Devices)
            {

                var hasDevice = false;
                foreach (DevicePanel devicePanel in deviceContainer.Controls)
                {
                    if (devicePanel.Device == null) continue;
                    if (device.Key == devicePanel.Device.Id)
                    {
                        hasDevice = devicePanel.Checked = true;
                        break;
                    }
                }
                if (!hasDevice)
                {
                    _removedDevices.Add(device.Key, device.Value);
                }
            }

            var sortResult = new List<IDevice>(_removedDevices.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            foreach (IDevice device in sortResult)
            {
                NVR.Device.Devices.Remove(device.Id);
            }

            var hasTitle = false;
            foreach (DevicePanel control in deviceContainer.Controls)
            {
                if (control.IsTitle)
                {
                    if (sortResult.Count > 0)
                    {
                        deviceContainer.Controls.Remove(control);
                    }
                    else
                    {
                        hasTitle = true;
                    }
                    break;
                }
            }

            if (!hasTitle)
            {
                var deviceTitlePanel = GetDevicePanel();
                deviceTitlePanel.IsTitle = true;
                deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
                deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
                deviceTitlePanel.Cursor = Cursors.Default;
                deviceTitlePanel.EditVisible = false;
                deviceTitlePanel.Checked = deviceContainer.Controls.Count == NVR.Device.Devices.Count;
                deviceContainer.Controls.Add(deviceTitlePanel);
                deviceContainer.Visible = NVR.Device.Devices.Count > 0;
            }
            _removedDevices.Clear();
            _isEditing = true;
        }

        private void ClearViewModel()
        {
            foreach (DevicePanel devicePanel in deviceContainer.Controls)
            {
                devicePanel.SelectionVisible = true;

                devicePanel.OnSelectChange -= DevicePanelOnSelectChange;
                devicePanel.Checked = false;
                devicePanel.Device = null;
                devicePanel.CMS = null;
                devicePanel.Cursor = Cursors.Hand;
                devicePanel.EditVisible = false;
                devicePanel.Enabled = true;
                devicePanel.OnSelectChange += DevicePanelOnSelectChange;
                devicePanel.OnDeviceEditClick -= DevicePanelOnDeviceEditClick;
                if (devicePanel.IsTitle)
                {
                    devicePanel.OnSelectAll -= DevicePanelOnSelectAll;
                    devicePanel.OnSelectNone -= DevicePanelOnSelectNone;
                    devicePanel.IsTitle = false;
                }

                if (!_recycleDevice.Contains(devicePanel))
                    _recycleDevice.Enqueue(devicePanel);
            }
            deviceContainer.Controls.Clear();
        }

        private void NameTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("NAME")) return;

            Server.WriteOperationLog("Modify NVR Information %1 to %2".Replace("%1", Localization["NVR_Name"]).Replace("%2", nameTextBox.Text));
            _modifyed.Remove("NAME");
        }

        private void DomainTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("DOMAIN")) return;

            Server.WriteOperationLog("Modify NVR Information %1 to %2".Replace("%1", Localization["NVR_Domain"]).Replace("%2", domainTextBox.Text));
            _modifyed.Remove("DOMAIN");
        }

        private void PortComboBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("PORT")) return;

            Server.WriteOperationLog("Modify NVR Information %1 to %2".Replace("%1", Localization["NVR_Port"]).Replace("%2", NVR.Credential.Port.ToString()));
            _modifyed.Remove("PORT");
        }

        private void AccountTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("ACCOUNT")) return;

            Server.WriteOperationLog("Modify NVR Information %1 to %2".Replace("%1", Localization["NVR_Account"]).Replace("%2", NVR.Credential.UserName));
            _modifyed.Remove("ACCOUNT");
        }

        private void PasswordTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("PASSWORD")) return;

            Server.WriteOperationLog("Modify NVR Information %1 to %2".Replace("%1", Localization["NVR_Password"]).Replace("%2", passwordTextBox.Text));
            _modifyed.Remove("PASSWORD");
        }

        private void ContainerPanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["EditNVRPanel_Information"], Manager.Font, Brushes.DimGray, 8, 0);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (Localization.ContainsKey("NVR_" + ((Control)sender).Tag))
                Manager.PaintText(g, Localization["NVR_" + ((Control)sender).Tag]);
            else
                Manager.PaintText(g, ((Control)sender).Tag.ToString());
        }

        public void CompareNVRSettingAfterCloseSetupForm()
        {
            if (domainTextBox.Text != NVR.Credential.Domain || portComboBox.Text != NVR.Credential.Port.ToString() ||
                accountTextBox.Text != NVR.Credential.UserName || passwordTextBox.Text != NVR.Credential.Password)
                NVRIsChangeCredential();
        }

        public Boolean CheckHost()
        {
            if (String.IsNullOrEmpty(domainTextBox.Text) || String.IsNullOrEmpty(portComboBox.Text))
                return false;
            return true;
        }

        public Boolean CheckUser()
        {
            if (String.IsNullOrEmpty(accountTextBox.Text))// || String.IsNullOrEmpty(passwordTextBox.Text))
                return false;
            return true;
        }

        public void ParseNVR()
        {
            if (NVR == null) return;

            IsEditing = false;

            nameTextBox.Text = NVR.Name;
            domainTextBox.Text = NVR.Credential.Domain;
            portComboBox.Text = NVR.Credential.Port.ToString();
            accountTextBox.Text = NVR.Credential.UserName;
            passwordTextBox.Text = NVR.Credential.Password;

            // accountTextBox.ReadOnly = (NVR.Manufacture == "iSap");

            if (CMS != null)
            {
                listenEventCheckBox.Checked = NVR.IsListenEvent;
                patrolCheckBox.Checked = NVR.IsPatrolInclude;
                sslCheckBox.Checked = NVR.Credential.SSLEnable;
                serverPortComboBox.Text = NVR.ServerPort.ToString();
                serverPortPanel.Visible = NVR.Manufacture == "Hikvision";
                serverStatusIntervalDoubleBufferPanel.Visible = NVR.Manufacture == "iSAP Failover Server";
                eventPanel.Visible = NVR.Manufacture != "iSAP Failover Server";
                serverStatusCheckIntervalComboBox.Text = NVR.ServerStatusCheckInterval.ToString();
                GenerateViewModel(NVR.TempDevices);
                CheckedSelectedDevice();
                addedNVRLabel.Text = NVR.TempDevices.Count == 1 ? Localization["EditNVRPanel_NumDevice"].Replace("%1", (NVR.TempDevices.Count).ToString()) : Localization["EditNVRPanel_NumDevices"].Replace("%1", (NVR.TempDevices.Count).ToString());
                ChangeAddLabelStringBySelectedDevice();

                foreach (var item in manufactureComboBox.Items)
                {
                    if (item.ToString().ToUpper() == NVR.Manufacture.ToUpper())
                    {
                        manufactureComboBox.SelectedItem = item;
                        break;
                    }
                }
            }

            if (FOS != null)
            {
                launchTimeComboBox.SelectedItem = _lanuchStr.ContainsKey(NVR.FailoverSetting.LaunchTime) ? _lanuchStr[NVR.FailoverSetting.LaunchTime] : DurationToText(60000);
                blockSizeComboBox.SelectedItem = _blockStr.ContainsKey(NVR.FailoverSetting.BlockSize) ? _blockStr[NVR.FailoverSetting.BlockSize] : SizeToText(4194304);
            }
            if (PTS != null)
            {
                patrolCheckBox.Checked = NVR.IsPatrolInclude;
                if (NVR.Manufacture == "Salient")
                {
                    sslCheckBox.Visible = patrolPanel.Visible = false;
                }
                else
                {
                    sslCheckBox.Visible = patrolPanel.Visible = true;
                    sslCheckBox.Checked = NVR.Credential.SSLEnable;
                }


                foreach (var item in manufactureComboBox.Items)
                {
                    if (item.ToString() == NVR.Manufacture)
                    {
                        manufactureComboBox.SelectedItem = item;
                        break;
                    }
                }
            }

            if (manufactureComboBox.Items.Count == 1 && CMS == null)
                manufacturePanel.Visible = false;

            containerPanel.Focus();

            IsEditing = true;

            _modifyed.Clear();
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _modifyed.Add("NAME");

            NVR.Name = nameTextBox.Text;
            NVRIsModify();
        }

        private void DomainTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _modifyed.Add("DOMAIN");

            NVR.Credential.Domain = domainTextBox.Text.Trim();
            NVRIsChangeCredential();
        }

        private void PortComboBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            UInt32 port = (portComboBox.Text != "") ? Convert.ToUInt32(portComboBox.Text) : 80;

            NVR.Credential.Port = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

            _modifyed.Add("PORT");

            NVRIsChangeCredential();
        }

        private void ServerPortComboBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            UInt32 port = (serverPortComboBox.Text != "") ? Convert.ToUInt32(serverPortComboBox.Text) : 80;

            NVR.ServerPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

            _modifyed.Add("SERVERPORT");

            NVRIsChangeCredential();
        }

        private void ServerStatusCheckIntervalComboBoxTextChanged(object sender, EventArgs e)
        {
            if (!IsEditing) return;

            UInt32 interval = (ushort) ((serverStatusCheckIntervalComboBox.Text != "") ? Convert.ToUInt32(serverStatusCheckIntervalComboBox.Text) : 600);

            NVR.ServerStatusCheckInterval = Convert.ToUInt16(Math.Min(Math.Max(interval, 10), 600));

            _modifyed.Add("SERVER_STATUS_CHECK_INTERVAL");
            NVRIsModify();
        }

        private void LaunchTimeComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            var time = launchTimeComboBox.SelectedItem.ToString();

            foreach (var obj in _lanuchStr)
            {
                if (String.Equals(obj.Value, time))
                {
                    NVR.FailoverSetting.LaunchTime = obj.Key;
                    break;
                }
            }
            NVRIsModify();
        }

        private void BlockSizeComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            var block = blockSizeComboBox.SelectedItem.ToString();

            foreach (var obj in _blockStr)
            {
                if (String.Equals(obj.Value, block))
                {
                    NVR.FailoverSetting.BlockSize = obj.Key;
                    break;
                }
            }
            NVRIsModify();
        }

        private void ManufactureComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            var manufacture = manufactureComboBox.SelectedItem.ToString();
            //var nvr = NVR;
            //no nvr manufacture selection allow
            switch (manufacture)
            {
                case "Salient":
                    accountTextBox.Text = @"admin";
                    accountTextBox.ReadOnly = false;
                    serverStatusIntervalDoubleBufferPanel.Visible = false;
                    if (Server is ICMS) NVR.Driver = manufacture;
                    eventPanel.Visible = true;
                    //NVR = new NVRSalient();
                    break;

                case "iSAP Failover Server":
                    //accountTextBox.Text = @"Admin";
                    accountTextBox.ReadOnly = false;
                    serverStatusIntervalDoubleBufferPanel.Visible = true;
                    if (Server is ICMS) NVR.Driver = "iSap";
                    eventPanel.Visible = false;
                    listenEventCheckBox.Checked = false;
                    NVR.IsListenEvent = false;
                    //NVR = new NVR();
                    break;

                default:
                    //accountTextBox.Text = @"Admin";
                    accountTextBox.ReadOnly = false;
                    serverStatusIntervalDoubleBufferPanel.Visible = false;
                    if (Server is ICMS) NVR.Driver = manufacture;
                    eventPanel.Visible = true;
                    //NVR = new NVR();
                    break;
            }

            //NVR.Id = nvr.Id;
            //NVR.Name = nvr.Name;
            //NVR.Form = nvr.Form;
            NVR.Manufacture = manufacture;
            
            //NVR.Credential = new ServerCredential 
            //{
            //    Domain = nvr.Credential.Domain,
            //    Port = nvr.Credential.Port,
            //    UserName = nvr.Credential.UserName,
            //    Password = nvr.Credential.Password,
            //};
            //NVR.IsListenEvent = nvr.IsListenEvent;
            //NVR.IsPatrolInclude = nvr.IsPatrolInclude;

            //convert device list from nvr to nvr

            serverPortPanel.Visible = manufacture == "Hikvision";

            NVRIsModify();
        }

        private void AccountTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _modifyed.Add("ACCOUNT");

            NVR.Credential.UserName = accountTextBox.Text.Trim();
            NVRIsModify();
        }

        private void PasswordTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _modifyed.Add("PASSWORD");

            NVR.Credential.Password = passwordTextBox.Text;
            NVRIsChangeCredential();
        }

        private void ListenEventCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            NVR.IsListenEvent = listenEventCheckBox.Checked;
            NVRIsModify();
        }

        private void PatrolCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            NVR.IsPatrolInclude = patrolCheckBox.Checked;
            NVRIsModify();
        }

        private void SSLCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            NVR.Credential.SSLEnable = sslCheckBox.Checked;
            NVRIsModify();
        }

        public void NVRIsModify()
        {
            if (NVR.ReadyState == ReadyState.Ready)
                NVR.ReadyState = ReadyState.Modify;
        }

        public void NVRIsChangeCredential()
        {
            if (NVR.ReadyState != ReadyState.New)
            {
                NVR.ReadyState = ReadyState.Unavailable;

                if (CMS != null)
                    CMS.NVRModify(NVR);
                else if (PTS != null)
                    PTS.NVRModify(NVR);
                else if (VAS != null)
                    VAS.NVRModify(NVR);
            }
        }

        private String SizeToText(UInt32 block)
        {
            if (block < 1048576)
                return KBToStr(block);

            return MBToStr(block);
        }

        private String KBToStr(UInt32 block)
        {
            return (block / 1024) + @"K";
        }

        private String MBToStr(UInt32 block)
        {
            String str = "";
            if (block >= 1048576)
                str = (block / 1048576) + @"M";

            if (block % 1048576 != 0)
                str += " " + KBToStr(block % 1048576);

            return str;
        }

        private String DurationToText(UInt32 duration)
        {
            if (duration < 60000)
                return SecToStr(duration);

            return MinToStr(duration);
        }

        private String SecToStr(UInt32 sec)
        {
            return (sec / 1000) + Localization["Common_Sec"];
        }

        private String MinToStr(UInt32 min)
        {
            String str = "";
            if (min >= 60000)
                str = (min / 60000) + Localization["Common_Min"];

            if (min % 60000 != 0)
                str += " " + SecToStr(min % 60000);

            return str;
        }

        internal void SISSetupView()
        {
            patrolPanel.Visible = false;
            eventPanel.Visible = false;
        }

    }
}
