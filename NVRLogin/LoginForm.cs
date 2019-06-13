using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Constant;
using PanelBase;

namespace NVRLogin
{
    public partial class LoginForm : Form
    {
        private Dictionary<String, String> _localizationList = new Dictionary<String, String>();
        public Dictionary<String, String> Localization;
        public Logininfo.MyLogin Logininfo;
       
        

        public void ShowLoginForm()
        {
            Show();
        }


        public LoginForm()
        {
            InitializeComponent();
            Localization = new Dictionary<String, String>
								{
									{"Common_Cancel", "Cancel"},
									
									{"MessageBox_Information", "Information"},
									{"MessageBox_Error", "Error"},
									
									{"LoginForm_ErrorCode", "Error Code: "},
									{"LoginForm_SignIn", "Sign In"},
									{"LoginForm_Host", "Host"},
									{"LoginForm_Account", "Account"},
									{"LoginForm_Password", "Password"},
									{"LoginForm_Language", "Language"},
									{"LoginForm_SSL", "Enable SSL connection"},
									{"LoginForm_ForgetMe", "Forget me"},
                                    {"LoginForm_RememberMe", "Remember me"},
									{"LoginForm_SignInAutomatically", "Sign me in automatically"},
									{"LoginForm_SignInFailed", "Login failure"},
									{"LoginForm_SignInFailedCantCreateHttpWebRequest", "Can not create HttpWebRequest object."},

									{"LoginForm_SignInFailedNoAccount", "The Account Name field cannot be empty."},
									{"LoginForm_SignInFailedConnectFailure", "Failed to connect to server. Please confirm host and port validity."},
									{"LoginForm_SignInTimeout", "Sign-In timeout. Please check firewall setting."},
									{"LoginForm_SignInTimeoutSSL", "Sign-In timeout. Please check if firewall setting and SSL port are correct."},
									{"LoginForm_SignInFailedAuthFailure", "Sign-In failure. Please confirm account and password validity."},
									{"LoginForm_SignInFailedPortOccupation", "Sign-In failure. Please verify if port %1 is already used by another application."},
									{"LoginForm_SignInFailedAndRetryAutoLogin", "The System will auto sign in again after %1 seconds. (Signin: %2)"},
								};
            accountTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            portTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            FormClosing += LoginFormClosing;
            hostComboBox.GotFocus += HostComboBoxGotFocus;
            portTextBox.GotFocus += PortTextBoxGotFocus;
            accountTextBox.GotFocus += AccountTextBoxGotFocus;
            passwordTextBox.GotFocus += PasswordTextBoxGotFocus;

            hostLabel.Text = "Host";
            label2.Visible = true;
            portTextBox.Visible = true;
            hostComboBox.Items.Add("localhost");
            //hostComboBox.Text = "localhost";
            DomainLabel.Visible = false;
            DomainTextBox.Visible = false;
        }

        public void InitPanel(string Host, string Port, string Account, string Password, bool SSLEnable, bool RememberMe, string ADDomain, Dictionary<String, String> localizationList, bool Autologin,string Mode)
        {
            if (Mode == "NVR")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
             //   radioButton1.PerformClick();
                radioButton1_Click(null, EventArgs.Empty);

            }
            else if(Mode == "AD")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;

                radioButton2_Click(null,new EventArgs());
              //  radioButton2.PerformClick();
            }
            BackgroundImage = Resources.GetResources(NVRLogin.Properties.Resources.loginPanel, NVRLogin.Properties.Resources.BGLoginPanel);
            if (File.Exists(Application.StartupPath + "\\images\\logo.png"))
            {
                Image newLogo = Image.FromFile(Application.StartupPath + "\\images\\logo.png");
                var largeLoginPanel = Resources.GetResources(NVRLogin.Properties.Resources.loginPanel2, NVRLogin.Properties.Resources.BGLoginPanel2);
                logoPictureBox.Image = newLogo;
                logoPictureBox.Visible = true;
                BackgroundImage = largeLoginPanel;
                MinimumSize = MaximumSize = largeLoginPanel.Size;
            }

            if (File.Exists(Application.StartupPath + "\\images\\banner.png"))
            {
                Image banner = Image.FromFile(Application.StartupPath + "\\images\\banner.png");
                titlePanel.Controls.Remove(signInLabel);
                titlePanel.BackgroundImage = banner;
                titlePanel.BackgroundImageLayout = ImageLayout.Stretch;
            }
            if (Autologin)
            {
                signMeInAutomaticallyCheckBox.Enabled = true;
                signMeInAutomaticallyCheckBox.Checked = Autologin;
            }
            else
            {
                //   signMeInAutomaticallyCheckBox.Enabled =
                //   signMeInAutomaticallyCheckBox.Checked = false;
            }
            _localizationList = localizationList;
            localizationComboBox.Items.Clear();
            localizationComboBox.SelectedIndexChanged -= LocalizationComboBoxSelectedIndexChanged;
            if (localizationList == null)
            {
                localizationComboBox.Items.Add("English");
                localizationComboBox.SelectedIndexChanged += LocalizationComboBoxSelectedIndexChanged;
                localizationComboBox.SelectedIndex = 0;
            }
            foreach (KeyValuePair<String, String> lang in localizationList)
            {
                localizationComboBox.Items.Add(lang.Value);
            }
            if (localizationList.Count>0)
            localizationComboBox.SelectedItem = localizationList.Values.First();
            localizationComboBox.SelectedIndexChanged += LocalizationComboBoxSelectedIndexChanged;

            this.Host= Host;
            //  DomainTextBox.Text = Host;
            this.Port = Port;
            this.Account = Account;
            this.Password = Password;
            this.SSLEnable = SSLEnable;
            this.RememberMe = RememberMe;
            this.ADDomain = ADDomain;
        }
        public Boolean SSLEnable
        {
            get
            {
                return sslCheckBox.Checked;
            }
            protected set
            {
                sslCheckBox.Checked = value;
            }
        }

        public Boolean RememberMe
        {
            get
            {
                return rememberCheckBox.Checked;
            }
            protected set
            {
                rememberCheckBox.Checked = value;
            }
        }
        public String Domaininfo
        {
            get
            {
                return DomainTextBox.Text;
            }
            protected set
            {
                DomainTextBox.Text = value;
            }
        }

        public String ADDomain
        {
            get
            {
                return DomainTextBox.Text;
            }
            protected set
            {
                DomainTextBox.Text = value;
            }
        }
        public String Account
        {
            get
            {
                return accountTextBox.Text;
            }
            protected set
            {
                accountTextBox.Text = value;
            }
        }

        public String Password
        {
            get
            {
                return passwordTextBox.Text;
            }
            protected set
            {
                passwordTextBox.Text = value;
            }
        }
        public String Host
        {
            get
            {
                return hostComboBox.Text;
            }
            protected set
            {
                hostComboBox.Text = value;
            }
        }
        public String Port
        {
            get
            {
                return portTextBox.Text;
            }
            protected set
            {
                portTextBox.Text = value;
            }
        }


        private void loginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Host) || Convert.ToUInt16(Port) == 0 || string.IsNullOrEmpty(Account))
            {
                
                MessageBox.Show(
                    (Account == ""
                        ? Localization["LoginForm_SignInFailedNoAccount"]
                        : Localization["LoginForm_SignInFailed"]), Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (radioButton2.Checked && string.IsNullOrEmpty(ADDomain))
            {
                MessageBox.Show(
                   ( Localization["LoginForm_SignInFailed"]), Localization["MessageBox_Information"],
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Hide();
            string Mode = "";
            if (radioButton1.Checked)
            {
                Mode = "NVR";
            }
            else if (radioButton2.Checked)
            {
                Mode = "AD";
            }
            String lang = localizationComboBox.SelectedItem.ToString();
            Console.WriteLine(lang);
            XElement company =
                // new XElement("Information",
                new XElement("Login",
                    new XElement("ADDomain", DomainTextBox.Text),
                    new XElement("Port", portTextBox.Text),
                    new XElement("Host", hostComboBox.Text),
                    new XElement("Account", accountTextBox.Text),
                    new XElement("Password", passwordTextBox.Text),
                    new XElement("lang", lang),
                    new XElement("LoginMode", Mode),
                    new XElement("SSL", sslCheckBox.Checked ? "true" : "false"),
                    new XElement("RememberMe", rememberCheckBox.Checked ? "true" : "false"),
                    new XElement("Autologin", signMeInAutomaticallyCheckBox.Checked ? "true" : "false"));

            ReturnValueDelegate handler = ReturnValueCallback;
            if (handler != null)
                ReturnValueCallback(company.ToString());
        }
        public delegate void ReturnValueDelegate(string pValue);
        public event ReturnValueDelegate ReturnValueCallback;

        private void cancelButton_Click(object sender, EventArgs e)
        {
            ReturnValueDelegate handler = ReturnValueCallback;
            if (handler != null)
                ReturnValueCallback("");
            FormClosing -= LoginFormClosing;
            Close();
            Application.Exit();
        }



        private void radioButton1_Click(object sender, EventArgs e)
        {
            hostLabel.Text = "Host";
            label2.Visible = true;
            portTextBox.Visible = true;
            //hostComboBox.Text = "localhost";
            DomainLabel.Visible = false;
            DomainTextBox.Visible = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            DomainLabel.Visible = true;
            DomainTextBox.Visible = true;
        }
        private void LocalizationComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            String lang = localizationComboBox.SelectedItem.ToString();
            foreach (KeyValuePair<String, String> obj in _localizationList)
            {
                if (obj.Value != lang) continue;

                LoadLocalizationResx(obj.Key);
                break;
            }
            UpdateUI();
        }
        protected virtual void UpdateUI()
        {
            signInLabel.Text = Localization["LoginForm_SignIn"];
            hostLabel.Text = Localization["LoginForm_Host"];
            accountLabel.Text = Localization["LoginForm_Account"];
            passwordLabel.Text = Localization["LoginForm_Password"];
            languageLabel.Text = Localization["LoginForm_Language"];
            sslCheckBox.Text = Localization["LoginForm_SSL"];
            rememberCheckBox.Text = Localization["LoginForm_RememberMe"];
            signMeInAutomaticallyCheckBox.Text = Localization["LoginForm_SignInAutomatically"];
            loginButton.Text = Localization["LoginForm_SignIn"];
            cancelButton.Text = Localization["Common_Cancel"];

            SharedToolTips.SharedToolTip.SetToolTip(forgetMePanel, Localization["LoginForm_ForgetMe"]);
        }
        private void LoadLocalizationResx(String lang)
        {
            Localizations.Dictionary.Clear();

            Localizations.Key = lang.Replace(".resx", "");
            String langPath = Environment.CurrentDirectory + "\\lang\\"; //Application.StartupPath + "\\lang\\";
            if (File.Exists(langPath + lang))
            {
                Localizations.Load(langPath + lang);

                Localizations.Update(Localization);
            }
            else
            {
                //   AppProperties.DefaultLanguage = "";
                //   AppProperties.SaveProperties();
            }
        }
        private void HostComboBoxGotFocus(Object sender, EventArgs e)
        {
            hostComboBox.SelectAll();
        }

        private void PortTextBoxGotFocus(Object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                portTextBox.SelectAll();
            }
        }

        private void AccountTextBoxGotFocus(Object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                accountTextBox.SelectAll();
            }
        }

        private void PasswordTextBoxGotFocus(Object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                passwordTextBox.SelectAll();
            }
        }
        private void LoginFormClosing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            FormClosing -= LoginFormClosing;

            Environment.Exit(Environment.ExitCode);
        }

    }
    

}
