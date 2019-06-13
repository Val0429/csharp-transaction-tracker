using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupUser
{
    public partial class EditUserPanel : UserControl
    {
        public IApp App;
        public IServer Server;
        public IUser User;
        public Dictionary<String, String> Localization;

        public Boolean IsEditing;

        private readonly List<String> _modifyed = new List<string>();

        public EditUserPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},

								   {"User_Name", "Name"},
								   {"User_Password", "Password"},
								   {"User_ConfirmPassword", "Confirm Password"},
								   {"User_Email", "Email"},
								   {"User_Group", "Group"},
								   
								   {"EditUserPanel_Information", "Information"},
								   {"EditUserPanel_DevicePermission", "Device Permission"},
								   {"EditUserPanel_NVRPermission", "NVR Permission"},
								   
								   {"EditUserPanel_UserNameCantEmpty", "User name can't be empty."},
								   {"EditUserPanel_UserNameCantTheSame", "User name can't be the same."},
								   {"EditUserPanel_UserNameAcceptNumberAndAlphaOnly", "User name only accept letters and numbers."},

								   {"EditUserPanel_PasswordCantEmpty", "Password can't be empty."},
								   {"EditUserPanel_PasswordNotMatch", "Password does not match."},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            nameTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            informationLabel.Text = Localization["EditUserPanel_Information"];

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            if (Server is ICMS)
                permissionLabel.Text = Localization["EditUserPanel_NVRPermission"];
            else
                permissionLabel.Text = Localization["EditUserPanel_DevicePermission"];

            namePanel.Paint += PaintInput;
            passwordPanel.Paint += PaintInput;
            confirmPasswordPanel.Paint += PaintInput;
            emailPanel.Paint += PaintInput;
            groupPanel.Paint += PaintInput;

            nameTextBox.LostFocus += NameTextBoxLostFocus;

            passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
            passwordTextBox.TextChanged += PasswordTextBoxChanged;

            confirmPasswordTextBox.LostFocus += PasswordTextBoxLostFocus;

            emailTextBox.TextChanged += EmailTextBoxTextChanged;
            emailTextBox.LostFocus += EmailTextBoxLostFocus;
            groupComboBox.SelectedIndexChanged += GroupComboBoxSelectedIndexChanged;
        }

        void EmailTextBoxLostFocus(object sender, EventArgs e)
        {
            if (!_modifyed.Contains("EMAIL")) return;

            Server.WriteOperationLog("Modify User %1 Information %2 to %3"
                .Replace("%1", User.Credential.UserName)
                .Replace("%2", Localization["User_Email"])
                .Replace("%3", User.Email));
            _modifyed.Remove("EMAIL");
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, control);

            if (Localization.ContainsKey("User_" + control.Tag))
                Manager.PaintText(g, Localization["User_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private readonly Queue<DevicePermissionPanel> _recycleDevice = new Queue<DevicePermissionPanel>();
        private readonly Queue<NVRPermissionPanel> _recycleNVR = new Queue<NVRPermissionPanel>();

        private readonly Regex _mailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        public void ParseUser()
        {
            if (User == null) return;

            IsEditing = false;

            nameTextBox.Text = User.Credential.UserName;
            passwordTextBox.Text = confirmPasswordTextBox.Text = User.Credential.Password;
            emailTextBox.Text = User.Email;

            if (String.IsNullOrEmpty(User.Email))
                emailTextBox.BackColor = Color.White;
            else
            {
                Match match = _mailRegex.Match(User.Email);
                if (match.Success)
                    emailTextBox.BackColor = Color.White;
                else
                    emailTextBox.BackColor = Color.FromArgb(223, 173, 183);
            }

            groupComboBox.Items.Clear();
            foreach (KeyValuePair<UInt16, IUserGroup> obj in Server.User.Groups)
            {
                groupComboBox.Items.Add(obj.Value.TitleName);
            }

            if (User.Group != null)
                groupComboBox.SelectedItem = User.Group.TitleName;
            else
                groupComboBox.SelectedIndex = 0;

            //can't edit current login user'name and belong group
            if (User.Id == 0 || User.Credential.UserName == Server.Credential.UserName)
                groupComboBox.Enabled = nameTextBox.Enabled = false;
            else
                groupComboBox.Enabled = nameTextBox.Enabled = true;

            GeneratePermissionList();

            IsEditing = true;
            permissionPanel.Focus();

            _modifyed.Clear();
        }

        protected virtual void GeneratePermissionList()
        {
            if (Server is ICMS)
                GeneratorNVRPermissionList();
            else
                GeneratorDevicePermissionList();
        }

        private void GeneratorNVRPermissionList()
        {
            if (Server == null) return;

            permissionPanel.Visible = false;
            ClearViewModel();

            var sortResult = new List<INVR>(((ICMS)Server).NVRManager.NVRs.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                permissionLabel.Visible = false;
                return;
            }
            permissionLabel.Visible = true;

            Boolean locked = User.Group.IsFullAccessToDevices;

            foreach (INVR nvr in sortResult)
            {
                if (nvr != null)
                {
                    NVRPermissionPanel nvrPermissionPanel = GetNVRPermissionPanel();
                    nvrPermissionPanel.User = User;
                    nvrPermissionPanel.Enabled = !locked;
                    nvrPermissionPanel.NVR = nvr;
                    permissionPanel.Controls.Add(nvrPermissionPanel);
                }
            }

            var nvrTitlePanel = GetNVRPermissionPanel();
            nvrTitlePanel.User = null;
            nvrTitlePanel.IsTitle = true;
            nvrTitlePanel.Enabled = !locked;
            nvrTitlePanel.SelectionVisible = nvrTitlePanel.Enabled;
            nvrTitlePanel.Cursor = Cursors.Default;
            nvrTitlePanel.OnSelectAll += PermissionPanelOnSelectAll;
            nvrTitlePanel.OnSelectNone += PermissionPanelOnSelectNone;
            permissionPanel.Controls.Add(nvrTitlePanel);

            permissionPanel.Visible = true;
        }

        private void GeneratorDevicePermissionList()
        {
            if (Server == null) return;
            if (Server is IVAS || Server is IFOS)
            {
                permissionLabel.Visible = false;
                return;
            }

            permissionPanel.Visible = false;
            ClearViewModel();

            var sortResult = new List<IDevice>(Server.Device.Devices.Values);
            sortResult.Sort((x, y) => (y.Id - x.Id));

            if (sortResult.Count == 0)
            {
                permissionLabel.Visible = false;
                return;
            }
            permissionLabel.Visible = true;

            Boolean locked = User.Group.IsFullAccessToDevices;

            foreach (var device in sortResult.OfType<ICamera>())
            {
                var devicePermissionPanel = GetDevicePermissionPanel();
                devicePermissionPanel.User = User;
                devicePermissionPanel.Enabled = !locked;
                devicePermissionPanel.Device = device;
                permissionPanel.Controls.Add(devicePermissionPanel);
            }

            var deviceTitlePanel = GetDevicePermissionPanel();
            deviceTitlePanel.User = User;
            deviceTitlePanel.IsTitle = true;
            deviceTitlePanel.Enabled = !locked;
            deviceTitlePanel.SelectionVisible = deviceTitlePanel.Enabled;
            deviceTitlePanel.PermissionSelectionVisible = false;
            deviceTitlePanel.Cursor = Cursors.Default;
            deviceTitlePanel.OnSelectAll += PermissionPanelOnSelectAll;
            deviceTitlePanel.OnSelectNone += PermissionPanelOnSelectNone;
            permissionPanel.Controls.Add(deviceTitlePanel);

            permissionPanel.Visible = true;
        }

        private DevicePermissionPanel GetDevicePermissionPanel()
        {
            if (_recycleDevice.Count > 0)
                return _recycleDevice.Dequeue();

            var devicePanel = new DevicePermissionPanel
            {
                App = App,
                SelectionVisible = true,
                PermissionSelectionVisible = false,
            };
            devicePanel.OnSelectChange += DevicePanelOnSelectChange;

            return devicePanel;
        }

        private NVRPermissionPanel GetNVRPermissionPanel()
        {
            if (_recycleNVR.Count > 0)
                return _recycleNVR.Dequeue();

            var nvrPanel = new NVRPermissionPanel
            {
                App = App,
                SelectionVisible = true,
            };
            nvrPanel.OnSelectChange += NVRPanelOnSelectChange;

            return nvrPanel;
        }

        private void DevicePanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as DevicePermissionPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (DevicePermissionPanel control in permissionPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = permissionPanel.Controls[permissionPanel.Controls.Count - 1] as DevicePermissionPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= PermissionPanelOnSelectAll;
                title.OnSelectNone -= PermissionPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += PermissionPanelOnSelectAll;
                title.OnSelectNone += PermissionPanelOnSelectNone;
            }
        }

        private void NVRPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as NVRPermissionPanel;
            if (panel == null) return;

            var selectAll = false;
            if (panel.Checked)
            {
                selectAll = true;
                foreach (NVRPermissionPanel control in permissionPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }

            var title = permissionPanel.Controls[permissionPanel.Controls.Count - 1] as NVRPermissionPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= PermissionPanelOnSelectAll;
                title.OnSelectNone -= PermissionPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += PermissionPanelOnSelectAll;
                title.OnSelectNone += PermissionPanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            if (Server == null) return;
            if (Server is ICMS)
            {
                foreach (NVRPermissionPanel panel in permissionPanel.Controls)
                {
                    panel.OnSelectChange -= DevicePanelOnSelectChange;
                    panel.NVR = null;
                    panel.Cursor = Cursors.Hand;
                    panel.Checked = false;
                    panel.SelectionVisible = true;
                    panel.OnSelectChange -= DevicePanelOnSelectChange;

                    if (panel.IsTitle)
                    {
                        panel.OnSelectAll -= PermissionPanelOnSelectAll;
                        panel.OnSelectNone -= PermissionPanelOnSelectNone;
                        panel.IsTitle = false;
                    }

                    if (!_recycleNVR.Contains(panel))
                    {
                        _recycleNVR.Enqueue(panel);
                    }
                }
            }
            else
            {
                foreach (DevicePermissionPanel panel in permissionPanel.Controls)
                {
                    panel.OnSelectChange -= DevicePanelOnSelectChange;
                    panel.Device = null;
                    panel.Cursor = Cursors.Hand;
                    panel.Checked = false;
                    panel.SelectionVisible = true;
                    panel.PermissionSelectionVisible = false;
                    panel.OnSelectChange += DevicePanelOnSelectChange;

                    if (panel.IsTitle)
                    {
                        panel.OnSelectAll -= PermissionPanelOnSelectAll;
                        panel.OnSelectNone -= PermissionPanelOnSelectNone;
                        panel.IsTitle = false;
                        panel.PermissionSelectionVisible = true;
                    }

                    if (!_recycleDevice.Contains(panel))
                    {
                        _recycleDevice.Enqueue(panel);
                    }
                }
            }

            permissionPanel.Controls.Clear();
        }

        private void PermissionPanelOnSelectAll(Object sender, EventArgs e)
        {
            permissionPanel.AutoScroll = false;
            if (Server is ICMS)
            {

                foreach (NVRPermissionPanel control in permissionPanel.Controls)
                {
                    control.Checked = true;
                }
                permissionPanel.AutoScroll = true;
                return;
            }

            foreach (DevicePermissionPanel control in permissionPanel.Controls)
            {
                control.Checked = true;
            }
            permissionPanel.AutoScroll = true;
        }

        private void PermissionPanelOnSelectNone(Object sender, EventArgs e)
        {
            permissionPanel.AutoScroll = false;
            if (Server is ICMS)
            {
                foreach (NVRPermissionPanel control in permissionPanel.Controls)
                {
                    control.Checked = false;
                }
                permissionPanel.AutoScroll = true;
                return;
            }

            foreach (DevicePermissionPanel control in permissionPanel.Controls)
            {
                control.Checked = false;
            }
            permissionPanel.AutoScroll = true;
        }

        private void NameTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            String userName = nameTextBox.Text.Trim();
            //String password = passwordTextBox.Text;

            //if (userName == User.Credential.UserName)
            //{
            //    if (password == "")
            //    {
            //        if (passwordTextBox.Focused || confirmPasswordTextBox.Focused) return;
            //        nameTextBox.LostFocus -= NameTextBoxLostFocus;
            //        TopMostMessageBox.Show(Localization["EditUserPanel_PasswordCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

            //        passwordTextBox.Focus();
            //        nameTextBox.LostFocus += NameTextBoxLostFocus;
            //    }
            //    return;
            //}

            if (userName == "")
            {
                passwordTextBox.LostFocus -= PasswordTextBoxLostFocus;
                confirmPasswordTextBox.LostFocus -= PasswordTextBoxLostFocus;

                TopMostMessageBox.Show(Localization["EditUserPanel_UserNameCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                nameTextBox.Text = User.Credential.UserName;
                nameTextBox.BackColor = Color.FromArgb(223, 173, 183);
                nameTextBox.Focus();

                passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
                confirmPasswordTextBox.LostFocus += PasswordTextBoxLostFocus;
                return;
            }

            String name = Regex.Replace(userName, "[^a-zA-Z0-9]", "");
            if (name != userName)
            {
                passwordTextBox.LostFocus -= PasswordTextBoxLostFocus;
                confirmPasswordTextBox.LostFocus -= PasswordTextBoxLostFocus;

                TopMostMessageBox.Show(Localization["EditUserPanel_UserNameAcceptNumberAndAlphaOnly"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                nameTextBox.Text = User.Credential.UserName;
                nameTextBox.BackColor = Color.FromArgb(223, 173, 183);
                nameTextBox.Focus();

                passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
                confirmPasswordTextBox.LostFocus += PasswordTextBoxLostFocus;
                return;
            }

            foreach (KeyValuePair<UInt16, IUser> obj in Server.User.Users)
            {
                if (obj.Value.Credential.UserName == userName && obj.Value != User)
                {
                    passwordTextBox.LostFocus -= PasswordTextBoxLostFocus;
                    confirmPasswordTextBox.LostFocus -= PasswordTextBoxLostFocus;

                    TopMostMessageBox.Show(Localization["EditUserPanel_UserNameCantTheSame"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nameTextBox.Text = User.Credential.UserName;
                    nameTextBox.BackColor = Color.FromArgb(223, 173, 183);
                    nameTextBox.Focus();

                    passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
                    confirmPasswordTextBox.LostFocus += PasswordTextBoxLostFocus;
                    return;
                }
            }

            nameTextBox.BackColor = Color.White;
            if (userName == User.Credential.UserName) return;

            Server.WriteOperationLog("Modify User %1 Information %2 to %3"
                .Replace("%1", User.Credential.UserName)
                .Replace("%2", Localization["User_Name"])
                .Replace("%3", userName));

            User.Credential.UserName = userName;
            //if (password == "")
            //{
            //    if (passwordTextBox.Focused || confirmPasswordTextBox.Focused) return;

            //    nameTextBox.LostFocus -= NameTextBoxLostFocus;
            //    TopMostMessageBox.Show(Localization["EditUserPanel_PasswordCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    passwordTextBox.Focus();
            //    nameTextBox.LostFocus += NameTextBoxLostFocus;
            //    return;
            //}

            UserIsModify();
        }

        private void PasswordTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            if (passwordTextBox.Focused || confirmPasswordTextBox.Focused) return;

            String newPassword = passwordTextBox.Text;
            String confirmPassword = confirmPasswordTextBox.Text;

            if (newPassword != confirmPassword)
            {
                nameTextBox.LostFocus -= NameTextBoxLostFocus;
                passwordTextBox.LostFocus -= PasswordTextBoxLostFocus;
                confirmPasswordTextBox.LostFocus -= PasswordTextBoxLostFocus;

                //if (newPassword == "")
                //    TopMostMessageBox.Show(Localization["EditUserPanel_PasswordCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                //else
                TopMostMessageBox.Show(Localization["EditUserPanel_PasswordNotMatch"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                passwordTextBox.BackColor = confirmPasswordTextBox.BackColor = Color.FromArgb(223, 173, 183);

                nameTextBox.LostFocus += NameTextBoxLostFocus;
                passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
                confirmPasswordTextBox.LostFocus += PasswordTextBoxLostFocus;
                return;
            }

            passwordTextBox.BackColor = confirmPasswordTextBox.BackColor = Color.White;

            //if (newPassword == "" && !nameTextBox.Focused)
            //{
            //    nameTextBox.LostFocus -= NameTextBoxLostFocus;
            //    passwordTextBox.LostFocus -= PasswordTextBoxLostFocus;
            //    confirmPasswordTextBox.LostFocus -= PasswordTextBoxLostFocus;

            //    TopMostMessageBox.Show(Localization["EditUserPanel_PasswordCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    nameTextBox.LostFocus += NameTextBoxLostFocus;
            //    passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
            //    confirmPasswordTextBox.LostFocus += PasswordTextBoxLostFocus;
            //    return;
            //} 

            if (newPassword == User.Credential.Password) return;

            User.Credential.Password = newPassword;

            Server.WriteOperationLog("Modify User %1 Information %2 to %3"
                .Replace("%1", User.Credential.UserName)
                .Replace("%2", Localization["User_Password"])
                .Replace("%2", User.Credential.Password));

            UserIsModify();
        }

        private void PasswordTextBoxChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            confirmPasswordTextBox.Text = "";

            passwordTextBox.BackColor = confirmPasswordTextBox.BackColor = Color.White;
        }

        private void EmailTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            User.Email = emailTextBox.Text;

            _modifyed.Add("EMAIL");

            if (String.IsNullOrEmpty(User.Email))
                emailTextBox.BackColor = Color.White;
            else
            {
                Match match = _mailRegex.Match(User.Email);
                if (match.Success)
                    emailTextBox.BackColor = Color.White;
                else
                    emailTextBox.BackColor = Color.FromArgb(223, 173, 183);
            }

            UserIsModify();
        }

        private void GroupComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            var groupName = groupComboBox.SelectedItem.ToString();
            var wasFullAccess = User.Group.IsFullAccessToDevices;

            Server.WriteOperationLog("Modify User %1 Information %2 to %3"
                .Replace("%1", User.Credential.UserName)
                .Replace("%2", Localization["User_Group"])
                .Replace("%3", groupName));

            foreach (KeyValuePair<UInt16, IUserGroup> obj in Server.User.Groups)
            {
                if (obj.Value.TitleName != groupName) continue;

                obj.Value.AddUser(User);

                if (User.Group.IsFullAccessToDevices)
                {
                    if (!wasFullAccess)
                    {
                        var cms = Server as ICMS;
                        if (cms != null)
                        {
                            User.NVRPermissions.Clear();
                            foreach (var nvr in cms.NVRManager.NVRs.Values)
                            {
                                User.NVRPermissions.Add(nvr, new List<Permission> { Permission.Access });
                            }
                        }
                        else
                        {
                            User.Permissions.Clear();
                            foreach (KeyValuePair<UInt16, IDevice> device in Server.Device.Devices)
                            {
                                User.AddFullDevicePermission(device.Value);
                            }
                        }
                    }
                    else
                        break;
                }
                else
                {

                    if (!(Server is ICMS))
                    {
                        //if switch to guest, remove permission about playback.
                        if (!App.Pages.ContainsKey("Playback") || !User.Group.CheckPermission("Playback", Permission.Access))
                        {
                            foreach (KeyValuePair<IDevice, List<Permission>> permission in User.Permissions)
                            {
                                permission.Value.Remove(Permission.ExportVideo);
                                permission.Value.Remove(Permission.PrintImage);
                            }
                        }
                    }
                }

                IsEditing = false;

                GeneratePermissionList();

                IsEditing = true;

                break;
            }
            UserIsModify();
        }

        public void UserIsModify()
        {
            if (User.ReadyState == ReadyState.Ready)
                User.ReadyState = ReadyState.Modify;
        }

        protected void SetSISView()
        {
            permissionLabel.Visible = false;
            permissionPanel.Visible = false;
        }
    }
}
