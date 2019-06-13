using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using System.Xml;
using Constant;
using PanelBase;

namespace App_LoginForm
{
    public partial class LoginForm : Form
    {
        public Dictionary<String, String> Localization;
        private readonly Dictionary<String, String> _localizationList = new Dictionary<String, String>();

        protected Boolean IsLoading;
        protected AppClient App;

        protected AppClientPropertiesBase AppProperties;









        // Constructor
        public LoginForm()
        {
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

            InitializeComponent();

            FormClosing += LoginFormClosing;
            accountTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
            portTextBox.KeyPress += KeyAccept.AcceptNumberOnly;

            loginButton.BackgroundImage = Resources.GetResources(Properties.Resources.loginButton, Properties.Resources.IMGLoginButton);
            cancelButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn, Properties.Resources.IMGCancelButotn);

            hostComboBox.GotFocus += HostComboBoxGotFocus;
            portTextBox.GotFocus += PortTextBoxGotFocus;
            accountTextBox.GotFocus += AccountTextBoxGotFocus;
            passwordTextBox.GotFocus += PasswordTextBoxGotFocus; 
        }


        private void LoginFormClosing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            FormClosing -= LoginFormClosing;
            DialogResult = DialogResult.Abort;
        }

        public virtual Boolean Initialize(String[] arguments = null)
        {
            BackgroundImage = Resources.GetResources(Properties.Resources.loginPanel, Properties.Resources.BGLoginPanel);
            if (File.Exists(Application.StartupPath + "\\images\\logo.png"))
            {
                Image newLogo = Image.FromFile(Application.StartupPath + "\\images\\logo.png");
                var largeLoginPanel = Resources.GetResources(Properties.Resources.loginPanel2, Properties.Resources.BGLoginPanel2);
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

            LoadHistory();

            // Add By Tulip for Unlock Screen and Application to Relogin
            try
            {
                if (arguments != null && arguments.Length > 0)
                {
                    hostComboBox.Text = arguments[0];
                    passwordTextBox.Text = arguments[1];
                    accountTextBox.Text = arguments[2];
                    passwordTextBox.Text = arguments[3];
                }
            }
            catch { }


            LoadLocalizationList();
            Show();

            if (String.IsNullOrEmpty(Account))
            {
                accountTextBox.Focus();
            }
            else
            {
                if (String.IsNullOrEmpty(Password))
                    passwordTextBox.Focus();
                else
                    loginButton.Focus();
            }

            signMeInAutomaticallyCheckBox.Enabled = rememberCheckBox.Checked;

            return true;
        }

        protected void ClearSetting()
        {
            try
            {
                File.Delete(Path.Combine(StartupOptions.SettingFilePath(), StartupOptions.SettingFile));
            }
            catch
            {
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


        private void LoadLocalizationList()
        {
            _localizationList.Clear();
            localizationComboBox.Items.Clear();
            localizationComboBox.Enabled = false;
            localizationComboBox.SelectedIndexChanged -= LocalizationComboBoxSelectedIndexChanged;

            String langPath = Environment.CurrentDirectory + "\\lang\\"; //Application.StartupPath + "\\lang\\";
            if (!Directory.Exists(langPath))
            {
                localizationComboBox.Items.Add("English");
                localizationComboBox.SelectedIndexChanged += LocalizationComboBoxSelectedIndexChanged;
                localizationComboBox.SelectedIndex = 0;
                return;
            }

            String[] fileList = Directory.GetFiles(langPath);

            var localizationList = new List<String>();
            foreach (String path in fileList)
            {
                String[] paths = path.Split('\\');
                String fileName = paths[paths.Length - 1];
                String fileExt = fileName.Substring(fileName.LastIndexOf("."));
                if (fileExt == ".resx")
                    localizationList.Add(fileName);
            }

            if (localizationList.Count == 0) return;

            foreach (String localization in localizationList)
            {
                var resXResourceReader = new ResXResourceReader(langPath + localization);

                foreach (DictionaryEntry entry in resXResourceReader)
                {
                    if (entry.Key.ToString() == "Language")
                    {
                        _localizationList.Add(localization, entry.Value.ToString());
                        break;
                    }
                }

                resXResourceReader.Close();
            }

            if (_localizationList.Count == 0) return;

            localizationComboBox.Items.Clear();
            foreach (KeyValuePair<String, String> lang in _localizationList)
            {
                localizationComboBox.Items.Add(lang.Value);
            }

            localizationComboBox.SelectedIndexChanged += LocalizationComboBoxSelectedIndexChanged;

            try
            {
                if (AppProperties.DefaultLanguage != "")
                {
                    List<String> keys = _localizationList.Keys.ToList();
                    if (!keys.Contains(AppProperties.DefaultLanguage))
                    {
                        AppProperties.DefaultLanguage = keys.First();
                        localizationComboBox.SelectedItem = _localizationList.Values.First();
                    }
                    else
                    {
                        localizationComboBox.SelectedItem = _localizationList[AppProperties.DefaultLanguage];
                    }
                }
                else
                {
                    AppProperties.DefaultLanguage = _localizationList.Keys.First();
                    localizationComboBox.SelectedItem = _localizationList.Values.First();
                }
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
                localizationComboBox.SelectedItem = _localizationList.Values.First();
            }

            if (_localizationList.Count == 1) return;
            localizationComboBox.Enabled = true;
        }

        private String _focusPage;
        public Boolean AutoLogin(ServerCredential credential)
        {
            try
            {
                if (credential == null) return false;

                Host = credential.Domain;
                Port = credential.Port;
                Account = credential.UserName;
                Password = credential.Password;
                SSLEnable = credential.SSLEnable;

                try
                {
                    if (String.IsNullOrEmpty(AppProperties.DefaultLanguage))
                        AppProperties.DefaultLanguage = "en-us.resx";

                    LoadLocalizationResx(AppProperties.DefaultLanguage);
                }
                catch (Exception exception)
                {
                    //delete error property file. load default english
                    Propertys.Delete(exception);
                    LoadLocalizationResx("en-us.resx");
                }

                if (LoginViaCGI(true))
                {
                    ShowSplash();
                    return true;
                }

                try
                {
                    Process.Start(Process.GetCurrentProcess().MainModule.ModuleName);
                }
                catch (Exception)
                {
                }

                Environment.Exit(Environment.ExitCode);
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }

            return false;
        }

        public void Login()
        
        {
            loginButton.Enabled = false;
            if (LoginViaCGI())
            {
                //use xml to store property
                //<XML>
                //  <Host>127.0.0.1<Host>
                //  <Port>82<Port>
                //  <Account>Admin<Account>
                //  <Password><Password>
                //  <SSL>false<SSL>
                //</XML>

                var xmlDoc = new XmlDocument();
                var xmlRoot = xmlDoc.CreateElement("XML");
                xmlDoc.AppendChild(xmlRoot);
                xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Host", Encryptions.EncryptDES(Host)));
                xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Port", Encryptions.EncryptDES(Port.ToString(CultureInfo.InvariantCulture))));
                xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Account", Encryptions.EncryptDES(rememberCheckBox.Checked ? Account : "")));


                //String encryptConfig = Encryptions.EncryptDES(Host) + "," + Encryptions.EncryptDES(Port.ToString()) +
                //                       "," + Encryptions.EncryptDES(Account) + ",";

                xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Encryptions.EncryptDES(rememberCheckBox.Checked ? Password : "")));
                //encryptConfig += Encryptions.EncryptDES(Password);

                xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("SSL", sslCheckBox.Checked ? "true" : "false"));
                xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("RememberMe", rememberCheckBox.Checked ? "true" : "false"));

                String encryptConfig = Encryptions.EncryptDES(xmlDoc.InnerXml);

                if (_hostHistory.ContainsKey(Host))
                    _hostHistory[Host] = encryptConfig;
                else
                    _hostHistory.Add(Host, encryptConfig);

                try
                {
                    AppProperties.DefaultRemeberMe = rememberCheckBox.Checked;
                    AppProperties.DefaultAutoSignIn = signMeInAutomaticallyCheckBox.Checked;
                    AppProperties.DefaultHistory = ServerProperties.EncCredentialHistory(_hostHistory, Host);
                    AppProperties.SaveProperties();
                }
                catch (Exception exception)
                {
                    Propertys.Delete(exception);
                }

                loginButton.Enabled = true;

                String lang = localizationComboBox.SelectedItem.ToString();
                foreach (KeyValuePair<String, String> obj in _localizationList)
                {
                    if (obj.Value != lang) continue;

                    AppProperties.DefaultLanguage = obj.Key;
                    AppProperties.SaveProperties();

                    break;
                }

                ShowSplash();

                return;
            }

            loginButton.Enabled = true;
        }

        //---------------------------------------------------------------------------------------------------
        public Timer _timer;
        public Int64 _retryTimes = -1;
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public const int WM_CLOSE = 0x10;

        public void RetryAutoLogin()
        {
            if (_timer == null)
            {
                _timer = new Timer { Interval = (RetryLoginPeriod * 1000) };
                _timer.Tick += TimerTick;
            }
            _timer.Enabled = true;
            _timer.Start();
            _retryTimes++;
        }

        private void TimerTick(Object sender, EventArgs e)
        {
            _timer.Enabled = false;
            RemoveMessageBoxAndTimer();
            System.Threading.Thread.Sleep(3000);

            String temp = ServerProperties.LoadCredentialHistory(AppProperties.DefaultHistory).Values.First();
            try
            {
                var credential = ServerProperties.ParseDesStringToCredential(temp);
                AutoLogin(credential);
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }
        }

        public void RemoveMessageBoxAndTimer()
        {
            _timer.Stop();
            _timer.Enabled = false;
            IntPtr ptr = FindWindow(null, Localization["MessageBox_Information"]);
            if (ptr != IntPtr.Zero)
            {
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }
        //---------------------------------------------------------------------------------------------------

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private void DragPanelMouseDown(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        //---------------------------------------------------------------------------------------------------
        public virtual Boolean LoginViaCGI(Boolean isAutoLogin = false)
        {
            if (_timer == null)
                _timer = new Timer { Interval = RetryLoginPeriod * 1000, Enabled = false };
            else
                _timer.Enabled = false;

            if (Host != "" && Port != 0 && Account != "")
            {
                HttpWebResponse response = null;
                try
                {
                    var request = Xml.GetHttpRequest(CgiLogin, new ServerCredential
                    {
                        Port = Port,
                        Domain = Host,
                        UserName = Account,
                        Password = Password,
                        SSLEnable = SSLEnable,
                    });

                    if (request == null)
                    {
                        MessageBox.Show(Localization["LoginForm_SignInFailedCantCreateHttpWebRequest"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return false;
                    }

                    response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.Close();
                        return true;
                    }
                    response.Close();
                }
                catch (WebException exception)
                {
                    if (response != null)
                        response.Close();

                    String message = String.Empty;

                    switch (exception.Status)
                    {
                        case WebExceptionStatus.ConnectFailure:
                            message = Localization["LoginForm_SignInFailedConnectFailure"];
                            break;

                        case WebExceptionStatus.ProtocolError:
                            var httpWebResponse = ((HttpWebResponse)exception.Response);
                            if (httpWebResponse != null)
                            {
                                switch (httpWebResponse.StatusCode)
                                {
                                    case HttpStatusCode.Unauthorized:
                                        message = Localization["LoginForm_SignInFailedAuthFailure"];
                                        break;

                                    case HttpStatusCode.NotFound:
                                        message = Localization["LoginForm_SignInFailedPortOccupation"].Replace("%1", Port.ToString(CultureInfo.InvariantCulture));
                                        break;
                                }
                                httpWebResponse.Close();
                            }
                            break;

                        case WebExceptionStatus.Timeout:
                            if (SSLEnable)
                                message = Localization["LoginForm_SignInTimeoutSSL"];
                            else
                                message = Localization["LoginForm_SignInTimeout"];
                            break;

                        default:
                            message = Localization["LoginForm_SignInFailed"];
                            break;
                    }


                    if (!String.IsNullOrEmpty(message))
                    {
                        //add error code at bottom
                        message += Environment.NewLine + Environment.NewLine + Localization["LoginForm_ErrorCode"] +
                                   exception.Status;

                        //delete client.ini if auto login is failure
                        ClearSetting();

                        if (isAutoLogin)
                        {
                            RetryAutoLogin();
                            message += (Environment.NewLine + Environment.NewLine);
                            message += Localization["LoginForm_SignInFailedAndRetryAutoLogin"].Replace("%1", (RetryLoginPeriod).ToString(CultureInfo.InvariantCulture)).Replace("%2", _retryTimes.ToString(CultureInfo.InvariantCulture));

                            var msgBoxResult = MessageBox.Show(message, Localization["MessageBox_Information"], MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                            if (msgBoxResult == DialogResult.Retry)
                            {
                                RemoveMessageBoxAndTimer();

                                String temp = ServerProperties.LoadCredentialHistory(AppProperties.DefaultHistory).Values.First();
                                var credential = ServerProperties.ParseDesStringToCredential(temp);
                                AutoLogin(credential);
                                return false;
                            }
                            AppProperties.DefaultAutoSignIn = false;
                            AppProperties.SaveProperties();
                            RemoveMessageBoxAndTimer();
                        }
                        else
                            MessageBox.Show(message, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return false;
                }
            }

            MessageBox.Show(
                (Account == ""
                ? Localization["LoginForm_SignInFailedNoAccount"]
                : Localization["LoginForm_SignInFailed"]), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

            return false;
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
                AppProperties.DefaultLanguage = "";
                AppProperties.SaveProperties();
            }
        }

        public virtual void ShowSplash()
        {
            if (SplashForm != null) return;

            SplashForm = new SplashForm
            {
                Version = App.Version,
            };

            SplashForm.Show();
        }

        protected virtual ApplicationForm CreateApplicationForm()
        {
            var form = new ApplicationForm
            {
                App = App,
            };

            return form;
        }

        protected virtual void LoginServer(Object sender, EventArgs e)
        {
            Application.Idle -= LoginServer;
            Application.Idle += ShowProgress;

            if (IsLoading) return;
            IsLoading = true;
            Hide();

            var applicationForm = App.Form as ApplicationForm;

            if (applicationForm == null)
            {
                applicationForm = CreateApplicationForm();

                App.Form = TopMostMessageBox.MainForm = applicationForm;
            }

            try
            {
                applicationForm.Login();
            }
            catch (Exception exception)
            {
                //don't show C# exception dialog when en-count exception on release mode
                if (Debugger.IsAttached)
                {
                    Console.WriteLine(Localization["LoginForm_SignInFailed"] + Environment.NewLine + exception,
                        Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                applicationForm.Close();
                Show();
            }

            Port = DefaultPort;

            if (!String.IsNullOrEmpty(_focusPage))
            {
                foreach (var page in App.Pages)
                {
                    if (page.Key == _focusPage)
                    {
                        App.Activate(page.Value);
                        break;
                    }
                }
            }

            Application.Idle -= ShowProgress;
            SplashForm.Dispose();
            SplashForm = null;
            App = null;
        }

        protected virtual void ShowProgress(Object sender, EventArgs e)
        {
            if (SplashForm != null && App != null)
            {
                try
                {
                    SplashForm.SetInfo(App.LoginProgress);
                }
                catch (Exception exception)
                {
                    Propertys.Delete(exception);
                }
            }
        }

        private Dictionary<String, String> _hostHistory = new Dictionary<String, String>();

        public String Host
        {
            get
            {
                return (hostComboBox.SelectedItem != null) ? hostComboBox.SelectedItem.ToString().Trim() : hostComboBox.Text.Trim();
            }
            protected set
            {
                if (hostComboBox.Items.Contains(value))
                    hostComboBox.SelectedItem = value;

                hostComboBox.Text = value;
            }
        }

        protected UInt16 DefaultPort = 80;
        public UInt16 Port
        {
            get
            {
                Int32 port = DefaultPort;
                if (!String.IsNullOrEmpty(portTextBox.Text) && portTextBox.Text != @"0")
                {
                    port = Convert.ToInt32(portTextBox.Text);
                }

                if (port > 65535)
                    port = 65535;

                return Convert.ToUInt16(port);
            }
            protected set
            {
                portTextBox.Text = (value != 0) ? value.ToString(CultureInfo.InvariantCulture) : "";
            }
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

        public virtual void LoadHistory()
        {
            loginButton.Enabled = true;

            _hostHistory.Clear();

            hostComboBox.Items.Clear();

            try
            {
                if (!String.IsNullOrEmpty(AppProperties.DefaultHistory))
                    _hostHistory = ServerProperties.LoadCredentialHistory(AppProperties.DefaultHistory);

                foreach (KeyValuePair<String, String> obj in _hostHistory)
                    hostComboBox.Items.Add(obj.Key);

                //for test Kevin
                //hostComboBox.Items.Add("Isap");

                if (_hostHistory.Count > 0)
                {
                    ParseConfigToObject(_hostHistory.Values.First());
                    return;
                }
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }

            Host = "localhost";
            Port = DefaultPort;
            Account = "";
            Password = "";
            forgetMePanel.Visible = false;

            accountTextBox.Select();
        }

        private void HostComboBoxSelectionChangeCommitted(Object sender, EventArgs e)
        {
            if (_hostHistory.ContainsKey(Host))
                ParseConfigToObject(_hostHistory[Host]);
        }

        protected virtual void ParseConfigToObject(String configData)
        {
            ServerCredential credential = ServerProperties.ParseDesStringToCredential(configData);

            forgetMePanel.Visible = true;
            Host = credential.Domain;
            Port = credential.Port;
            Account = credential.UserName;
            Password = credential.Password;
            SSLEnable = credential.SSLEnable;
            RememberMe = credential.RememberMe;

            if (AppProperties.DefaultRemeberMe)
            {
                signMeInAutomaticallyCheckBox.Enabled = true;
                signMeInAutomaticallyCheckBox.Checked = AppProperties.DefaultAutoSignIn;
            }
            else
            {
                signMeInAutomaticallyCheckBox.Enabled =
                signMeInAutomaticallyCheckBox.Checked = false;
            }

            if (String.IsNullOrEmpty(Account))
                accountTextBox.Focus();
            else if (String.IsNullOrEmpty(credential.Password))
                passwordTextBox.Focus();
            else
                loginButton.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (cancelButton.Focused)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData == Keys.Enter)
            {
                Login();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoginButtonMouseClick(Object sender, MouseEventArgs e)
        {
            Login();
        }

        private void CancelButtonMouseClick(Object sender, EventArgs e)
        {
            FormClosing -= LoginFormClosing;
            Close();
            Application.Exit();
        }

        private void ForgetMePanelMouseClick(Object sender, MouseEventArgs e)
        {
            _hostHistory.Clear();

            if (AppProperties.DefaultHistory != "")
                _hostHistory = ServerProperties.LoadCredentialHistory(AppProperties.DefaultHistory);

            _hostHistory.Remove(Host);

            hostComboBox.Items.Remove(Host);

            if (_hostHistory.Count > 0)
            {
                ParseConfigToObject(_hostHistory.Values.First());
            }
            else
            {
                Host = "localhost";
                Port = DefaultPort;
                Account = "";
                Password = "";

                AppProperties.DefaultRemeberMe = false;
                //remove autologin when no history
                AppProperties.DefaultAutoSignIn = false;

                accountTextBox.Select();
                forgetMePanel.Visible = false;
            }

            AppProperties.DefaultHistory = ServerProperties.EncCredentialHistory(_hostHistory, Host);
            AppProperties.SaveProperties();
        }

        private void RememberCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                AppProperties.DefaultRemeberMe = rememberCheckBox.Checked;
                signMeInAutomaticallyCheckBox.Enabled = AppProperties.DefaultRemeberMe;
                if (!rememberCheckBox.Checked)
                {
                    signMeInAutomaticallyCheckBox.Checked = false;
                }
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }
        }

        private void SignMeInAutomaticallyCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                AppProperties.DefaultAutoSignIn = signMeInAutomaticallyCheckBox.Checked;
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }
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
    }
}
