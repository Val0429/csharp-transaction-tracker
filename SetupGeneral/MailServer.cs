using Constant;
using Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Manager = SetupBase.Manager;

namespace SetupGeneral

{
    public sealed partial class MailServer : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public MailServer()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MailServer_Sender", "Sender"},
                                   {"MailServer_EmailAddress", "Email address"},
                                   {"MailServer_Server", "Server"},
                                   {"MailServer_Security", "Security"},
                                   {"MailServer_Port", "Port"},
                                   {"MailServer_Account", "Account"},
                                   {"MailServer_Password", "Password"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Name = "MailServer";
            Dock = DockStyle.None;
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            //can type chinese or other characters
            senderComboBox.ImeMode = ImeMode.Disable;
            portTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            //accountTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            senderPanel.Paint += MailInputPanelPaint;
            addressPanel.Paint += MailInputPanelPaint;
            serverPanel.Paint += MailInputPanelPaint;
            securityPanel.Paint += MailInputPanelPaint;
            portPanel.Paint += MailInputPanelPaint;
            accountPanel.Paint += MailInputPanelPaint;
            passwordPanel.Paint += MailInputPanelPaint;

            securityComboBox.Items.Add(PLAIN);
            securityComboBox.Items.Add(SSL);
            securityComboBox.Items.Add(TLS);
        }

        private const String PLAIN = "PLAIN";
        private const String SSL = "SSL";
        private const String TLS = "TLS";

        public void ParseSetting()
        {
            senderComboBox.TextChanged -= SenderComboBoxTextChanged;
            senderComboBox.SelectedIndexChanged -= SenderComboBoxSelectedIndexChanged;
            securityComboBox.SelectedIndexChanged -= SecurityComboBoxSelectedIndexChanged;
            addressTextBox.TextChanged -= AddressTextBoxTextChanged;
            serverTextBox.TextChanged -= ServerTextBoxTextChanged;
            portTextBox.TextChanged -= PortTextBoxTextChanged;
            accountTextBox.TextChanged -= AccountTextBoxTextChanged;
            passwordTextBox.TextChanged -= PasswordTextBoxTextChanged;

            senderComboBox.Items.Clear();
            foreach (KeyValuePair<UInt16, IUser> obj in Server.User.Users)
            {
                senderComboBox.Items.Add(obj.Value.Credential.UserName);
            }

            senderComboBox.Text = Server.Configure.MailServer.Sender;
            addressTextBox.Text = Server.Configure.MailServer.MailAddress;
            serverTextBox.Text = Server.Configure.MailServer.Credential.Domain;
            portTextBox.Text = Server.Configure.MailServer.Port.ToString();
            accountTextBox.Text = Server.Configure.MailServer.Credential.UserName;
            passwordTextBox.Text = Server.Configure.MailServer.Credential.Password;

            switch (Server.Configure.MailServer.Security)
            {
                case SecurityType.PLAIN:
                    securityComboBox.SelectedItem = PLAIN;
                    break;

                case SecurityType.SSL:
                    securityComboBox.SelectedItem = SSL;
                    break;

                case SecurityType.TLS:
                    securityComboBox.SelectedItem = TLS;
                    break;
            }

            senderComboBox.TextChanged += SenderComboBoxTextChanged;
            senderComboBox.SelectedIndexChanged += SenderComboBoxSelectedIndexChanged;
            securityComboBox.SelectedIndexChanged += SecurityComboBoxSelectedIndexChanged;
            addressTextBox.TextChanged += AddressTextBoxTextChanged;
            serverTextBox.TextChanged += ServerTextBoxTextChanged;
            portTextBox.TextChanged += PortTextBoxTextChanged;
            accountTextBox.TextChanged += AccountTextBoxTextChanged;
            passwordTextBox.TextChanged += PasswordTextBoxTextChanged;
        }

        private void SenderComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (KeyValuePair<UInt16, IUser> obj in Server.User.Users)
            {
                if(obj.Value.Credential.UserName == senderComboBox.Text)
                {
                    addressTextBox.Text = obj.Value.Email;
                    break;
                }
            }
        }

        private void SecurityComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            switch (securityComboBox.SelectedItem.ToString())
            {
                case PLAIN:
                    Server.Configure.MailServer.Security = SecurityType.PLAIN;
                    portTextBox.Text = @"25";
                    break;

                case SSL:
                    Server.Configure.MailServer.Security = SecurityType.SSL;
                    portTextBox.Text = @"465";
                    break;

                case TLS:
                    Server.Configure.MailServer.Security = SecurityType.TLS;
                    portTextBox.Text = @"587";
                    break;
            }
        }
        
        private void SenderComboBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.MailServer.Sender = senderComboBox.Text;
        }

        private void AddressTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.MailServer.MailAddress = addressTextBox.Text;
        }

        private void ServerTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.MailServer.Credential.Domain = serverTextBox.Text.Trim();
        }

        private void PortTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 port = (portTextBox.Text != "") ? Convert.ToUInt32(portTextBox.Text) : 25;

            Server.Configure.MailServer.Port = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
        }

        private void AccountTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.MailServer.Credential.UserName = accountTextBox.Text;
        }

        private void PasswordTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.MailServer.Credential.Password = passwordTextBox.Text;
        }

        private void MailInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("MailServer_" + control.Tag))
                Manager.PaintText(g, Localization["MailServer_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }
    }
}
