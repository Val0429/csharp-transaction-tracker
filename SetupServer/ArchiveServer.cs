using Constant;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Manager = SetupBase.Manager;

namespace SetupServer
{
    public sealed partial class ArchiveServerControl : UserControl
    {
        public IServer Server;
        public ICMS CMS;
        public Dictionary<String, String> Localization;
        public Boolean IsEditing;

        private readonly List<String> _modifyed = new List<string>();

        public ArchiveServerControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},

                                   {"NVR_Domain", "Domain"},
                                   {"NVR_Port", "Port"},
                                   {"NVR_Account", "Account"},
                                   {"NVR_Password", "Password"},
                                   {"NVR_SSLConnection", "SSL Connection"},
                                   

                                   {"EditNVRPanel_Enable", "Enable"},
                               };
            Localizations.Update(Localization);
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            Name = "ArchiveServer";

            accountTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            sslCheckBox.Text = Localization["EditNVRPanel_Enable"];

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        private readonly UInt16[] _portArray = new UInt16[] { 80, 82, 443, 8080, 8088, 19999 };

        public void Initialize()
        {
            domainPanel.Paint += PaintInput;
            portPanel.Paint += PaintInput;
            sslConnectionPanel.Paint += PaintInput;
            accountPanel.Paint += PaintInput;
            passwordPanel.Paint += PaintInput;

           
            foreach (UInt16 port in _portArray)
            {
                portComboBox.Items.Add(port);
            }

            portComboBox.KeyPress += KeyAccept.AcceptNumberOnly;

            domainTextBox.TextChanged += DomainTextBoxTextChanged;
            portComboBox.TextChanged += PortComboBoxTextChanged;
            accountTextBox.TextChanged += AccountTextBoxTextChanged;
            passwordTextBox.TextChanged += PasswordTextBoxTextChanged;

            domainTextBox.LostFocus += DomainTextBoxLostFocus;
            portComboBox.LostFocus += PortComboBoxLostFocus;
            accountTextBox.LostFocus += AccountTextBoxLostFocus;
            passwordTextBox.LostFocus += PasswordTextBoxLostFocus;
            sslCheckBox.CheckedChanged += SSLCheckBoxCheckedChanged;
            
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

        public void ParseArchiveServer()
        {
            if (CMS == null) return;

            IsEditing = false;

            domainTextBox.Text = CMS.NVRManager.ArchiveServer.Domain;
            portComboBox.Text = CMS.NVRManager.ArchiveServer.Port.ToString();
            accountTextBox.Text = CMS.NVRManager.ArchiveServer.UserName;
            passwordTextBox.Text = CMS.NVRManager.ArchiveServer.Password;
            sslCheckBox.Checked = CMS.NVRManager.ArchiveServer.SSLEnable;

            IsEditing = true;

            _modifyed.Clear();
        }

        private void DomainTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("DOMAIN")) return;

            _modifyed.Remove("DOMAIN");
        }

        private void PortComboBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("PORT")) return;

            _modifyed.Remove("PORT");
        }

        private void AccountTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("ACCOUNT")) return;

            _modifyed.Remove("ACCOUNT");
        }

        private void PasswordTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (!_modifyed.Contains("PASSWORD")) return;

            _modifyed.Remove("PASSWORD");
        }

        private void DomainTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _modifyed.Add("DOMAIN");

            CMS.NVRManager.ArchiveServer.Domain = domainTextBox.Text.Trim();
        }

        private void PortComboBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            UInt32 port = (portComboBox.Text != "") ? Convert.ToUInt32(portComboBox.Text) : 80;

            CMS.NVRManager.ArchiveServer.Port = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));

            _modifyed.Add("PORT");
        }

        private void AccountTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _modifyed.Add("ACCOUNT");

            CMS.NVRManager.ArchiveServer.UserName = accountTextBox.Text.Trim();
        }

        private void PasswordTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            _modifyed.Add("PASSWORD");

            CMS.NVRManager.ArchiveServer.Password = passwordTextBox.Text;
        }

        private void SSLCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            CMS.NVRManager.ArchiveServer.SSLEnable = sslCheckBox.Checked;
        }


    }
}
