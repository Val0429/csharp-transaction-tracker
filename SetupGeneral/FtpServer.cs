using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGeneral
{
    public sealed partial class FtpServer : UserControl
    {
        public IServer Server;
        public Dictionary<String, String> Localization;

        public FtpServer()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"FtpServer_Server", "Server"},
                                   {"FtpServer_RemoteDirectory", "Remote directory"},
                                   {"FtpServer_Port", "Port"},
                                   {"FtpServer_Account", "Account"},
                                   {"FtpServer_Password", "Password"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Name = "FTPServer";
            Dock = DockStyle.None;
            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            directoryTextBox.ImeMode = ImeMode.Disable;
            portTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            //accountTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            directoryTextBox.KeyDown += DirectoryTextBoxKeyDown;
            directoryTextBox.KeyPress += DirectoryTextBoxKeyPress;

            serverPanel.Paint += FtpInputPanelPaint;
            directoryPanel.Paint += FtpInputPanelPaint;
            portPanel.Paint += FtpInputPanelPaint;
            accountPanel.Paint += FtpInputPanelPaint;
            passwordPanel.Paint += FtpInputPanelPaint;
        }

        public void ParseSetting()
        {
            serverTextBox.TextChanged -= ServerTextBoxTextChanged;
            directoryTextBox.TextChanged -= DirectoryTextBoxTextChanged;
            portTextBox.TextChanged -= PortTextBoxTextChanged;
            accountTextBox.TextChanged -= AccountTextBoxTextChanged;
            passwordTextBox.TextChanged -= PasswordTextBoxTextChanged;

            serverTextBox.Text = Server.Configure.FtpServer.Credential.Domain;
            directoryTextBox.Text = Server.Configure.FtpServer.Directory;
            portTextBox.Text = Server.Configure.FtpServer.Port.ToString();
            accountTextBox.Text = Server.Configure.FtpServer.Credential.UserName;
            passwordTextBox.Text = Server.Configure.FtpServer.Credential.Password;

            serverTextBox.TextChanged += ServerTextBoxTextChanged;
            directoryTextBox.TextChanged += DirectoryTextBoxTextChanged;
            portTextBox.TextChanged += PortTextBoxTextChanged;
            accountTextBox.TextChanged += AccountTextBoxTextChanged;
            passwordTextBox.TextChanged += PasswordTextBoxTextChanged;
        }

        private Boolean _invalidateChar;
        private void DirectoryTextBoxKeyDown(Object sender, KeyEventArgs e)
        {
            _invalidateChar = false;
            if (e.Shift)
            {
                switch (e.KeyValue)
                {
                    case 56:  //*
                    case 186: //:
                    case 188: //<
                    case 190: //>
                    case 191: //?
                    case 220: //|
                    case 222: //"
                        _invalidateChar = true;
                        break;
                }
            }
            else
            {
                switch (e.KeyValue)
                {
                    case 106: //*
                        _invalidateChar = true;
                        break;
                }
            }
        }

        private void DirectoryTextBoxKeyPress(Object sender, KeyPressEventArgs e)
        {
            //stop char input
            if (_invalidateChar)
                e.Handled = true;
        }

        private void ServerTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.FtpServer.Credential.Domain = serverTextBox.Text.Trim();
        }

        private void DirectoryTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.FtpServer.Directory = directoryTextBox.Text;
        }

        private void PortTextBoxTextChanged(Object sender, EventArgs e)
        {
            UInt32 port = (portTextBox.Text != "") ? Convert.ToUInt32(portTextBox.Text) : 21;

            Server.Configure.FtpServer.Port = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
        }

        private void AccountTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.FtpServer.Credential.UserName = accountTextBox.Text;
        }

        private void PasswordTextBoxTextChanged(Object sender, EventArgs e)
        {
            Server.Configure.FtpServer.Credential.Password = passwordTextBox.Text;
        }

        private void FtpInputPanelPaint(Object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            if(control == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (containerPanel.Width <= 100) return;

            if (Localization.ContainsKey("FtpServer_" + control.Tag))
                Manager.PaintText(g, Localization["FtpServer_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }
    }
}
