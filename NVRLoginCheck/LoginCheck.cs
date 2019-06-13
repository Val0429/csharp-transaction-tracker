using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using System.IO;
namespace NVRLoginCheck
{
    public class LoginCheck
    {
        public Timer _timer;
        public Int64 _retryTimes = -1;
        public const UInt16 RetryLoginPeriod = 10; //sec
        protected String CgiLogin = @"cgi-bin/login?action=login";
        public Dictionary<String, String> Localization;
        protected AppClientPropertiesBase AppProperties;
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
                // AutoLogin(credential);
            }
            catch (Exception exception)
            {
                Propertys.Delete(exception);
            }
        }
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public const int WM_CLOSE = 0x10;
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

        
        public Logininfo.MyLogin Logininfo;
        public void ParseLoginXML(string Xmlstring)
        {
            Logininfo = new Logininfo.MyLogin();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Xmlstring);

            var value = Xml.GetFirstElementValueByTagName(xmlDoc, "Host");

            if (!String.IsNullOrEmpty(value))
                Logininfo.Host = value;

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "Port");
            if (!String.IsNullOrEmpty(value))
                Logininfo.Port = Convert.ToUInt16(value);

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "Account");
            if (!String.IsNullOrEmpty(value))
                Logininfo.Account = value;

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "Password");
            if (!String.IsNullOrEmpty(value))
                Logininfo.Password = value;

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "SSL");
            if (!String.IsNullOrEmpty(value))
                Logininfo.SSLEnable = (value == "true");

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "RememberMe");
            if (!String.IsNullOrEmpty(value))
                Logininfo.RememberMe = (value == "true");

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "ADDomain");
            if (!String.IsNullOrEmpty(value))
                Logininfo.ADDomain = value;

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "lang");
            if (!String.IsNullOrEmpty(value))
                Logininfo.Lang = value;
            value = Xml.GetFirstElementValueByTagName(xmlDoc, "LoginMode");
            if (!String.IsNullOrEmpty(value))
                Logininfo.Mode = value;

            value = Xml.GetFirstElementValueByTagName(xmlDoc, "Autologin");
            if (!String.IsNullOrEmpty(value))
                Logininfo.Autologin = (value == "true");
        }

        public bool LoginViaCGI(Boolean isAutoLogin = false)
        {
            if (Logininfo == null)
            {
                ReturnValueCallback(false, Logininfo);
                return false;
            }
             
           

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
                        ReturnValueCallback(false, Logininfo);
                        return false;
                    }

                    response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.Close();
                        ReturnValueCallback(true, Logininfo);
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
                             //   AutoLogin(credential);//不要重複呼叫AutoLogin造成死結
                                ReturnValueCallback(false, Logininfo);
                                return false;
                            }
                            AppProperties.DefaultAutoSignIn = false;
                            AppProperties.SaveProperties();
                            RemoveMessageBoxAndTimer();
                        }
                        else
                            MessageBox.Show(message, Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    ReturnValueCallback(false, Logininfo);
                    return false;
                }
            }
            
            /*

            MessageBox.Show(
                (Account == ""
                ? Localization["LoginForm_SignInFailedNoAccount"]
                : Localization["LoginForm_SignInFailed"]), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            */
            ReturnValueCallback(false,Logininfo);
            return false;
        }
        public delegate void ReturnValueDelegate_NVRCheck(bool Bcheck, Logininfo.MyLogin Login);
        public event ReturnValueDelegate_NVRCheck ReturnValueCallback;

        public string Host
        {
            get { return Logininfo.Host; }
            set
            {
                Logininfo.Host = value;
            }
        }
        public string Account
        {
            get { return Logininfo.Account; }
            set
            {
                Logininfo.Account = value;
            }
        }
        public string Password
        {
            get { return Logininfo.Password; }
            set
            {
                Logininfo.Password = value;
            }
        }
        public bool SSLEnable
        {
            get { return Logininfo.SSLEnable; }
            set
            {
                Logininfo.SSLEnable = value;
            }
        }
        public ushort Port
        {
            get { return Logininfo.Port; }
            set
            {
                Logininfo.Port = value;
            }
        }

    }

}
