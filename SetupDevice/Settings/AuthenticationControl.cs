using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace SetupDevice
{
    public sealed partial class AuthenticationControl : UserControl
    {

        public Dictionary<String, String> Localization;
        public EditPanel EditPanel;

        public AuthenticationControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"DevicePanel_Authentication", "Authentication"},
                                   {"DevicePanel_Account", "Account"},
                                   {"DevicePanel_Password", "Password"},
                                   {"DevicePanel_Encryption", "Encryption"},
                                   {"DevicePanel_OccupancyPriority", "Who has higher channel occupancy priority?"},
                                   {"DevicePanel_PredecessorHasHigherPriority", "Predecessor"},
                                   {"DevicePanel_SuccessorHasHigherPriority", "Successor"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            Paint += AuthenticationControlPaint;

            accountPanel.Paint += PaintInput;
            passwordPanel.Paint += PaintInput;
            encryptionPanel.Paint += PaintInput;
            occupancyPriorityPanel.Paint += PaintInput;
            //userNameTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            encryptionPanel.Visible = false;

            occupancyPriorityComboBox.Items.Add(Localization["DevicePanel_PredecessorHasHigherPriority"]);
            occupancyPriorityComboBox.Items.Add(Localization["DevicePanel_SuccessorHasHigherPriority"]);
            occupancyPriorityComboBox.SelectedIndex = 0;

            //encryptionComboBox.SelectedIndexChanged += EncryptionComboBoxSelectedIndexChanged;
            userNameTextBox.TextChanged += UserNameTextBoxTextChanged;
            passwordTextBox.TextChanged += PasswordTextBoxTextChanged;
            occupancyPriorityComboBox.SelectedIndexChanged += OccupancyPriorityComboBoxSelectedIndexChanged;
        }

        private void AuthenticationControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_Authentication"], SetupBase.Manager.Font, Brushes.DimGray, 8, 10);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;

            SetupBase.Manager.Paint(g, control);

            if (Localization.ContainsKey("DevicePanel_" + control.Tag))
                SetupBase.Manager.PaintText(g, Localization["DevicePanel_" + control.Tag]);
            else
                SetupBase.Manager.PaintText(g, control.Tag.ToString());
        }

        private void UserNameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.Authentication.UserName = userNameTextBox.Text;

            EditPanel.CameraIsModify();
        }

        private void PasswordTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.Authentication.Password = passwordTextBox.Text;

            EditPanel.CameraIsModify();
        }

        private void OccupancyPriorityComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!EditPanel.IsEditing) return;

            EditPanel.Camera.Profile.Authentication.OccupancyPriority = (uint)(occupancyPriorityComboBox.SelectedItem.ToString() == Localization["DevicePanel_PredecessorHasHigherPriority"] ? 0 : 1);

            EditPanel.CameraIsModify();
        }

        //private void EncryptionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        //{
        //    if (!EditPanel.IsEditing) return;

        //    EditPanel.Camera.Profile.Authentication.Encryption = Encryptions.ToIndex(encryptionComboBox.SelectedItem.ToString());

        //    EditPanel.CameraIsModify();
        //}

        public void UpdateSettingContent()
        {
            userNameTextBox.Text = EditPanel.Camera.Profile.Authentication.UserName;
            passwordTextBox.Text = EditPanel.Camera.Profile.Authentication.Password;

            occupancyPriorityPanel.Visible = EditPanel.Camera.Model.Type == "SmartPatrolService";
            occupancyPriorityComboBox.SelectedIndex = (int) EditPanel.Camera.Profile.Authentication.OccupancyPriority;
        }

        public void UpdateSettingToEditComponent(CameraModel model)
        {
            encryptionComboBox.Items.Clear();

            foreach (Encryption encryption in model.Encryption)
                encryptionComboBox.Items.Add(Encryptions.ToString(encryption));
        }

        public void ParseDevice()
        {
            userNameTextBox.Text = EditPanel.Camera.Profile.Authentication.UserName;
            passwordTextBox.Text = EditPanel.Camera.Profile.Authentication.Password;

            occupancyPriorityComboBox.SelectedIndex = (int) EditPanel.Camera.Profile.Authentication.OccupancyPriority;

            occupancyPriorityPanel.Visible = EditPanel.Camera.Model.Type == "SmartPatrolService";

            encryptionComboBox.SelectedItem = Encryptions.ToString(EditPanel.Camera.Profile.Authentication.Encryption);
        }
    }
}
