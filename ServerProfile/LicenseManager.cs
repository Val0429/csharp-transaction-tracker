using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;
using Timer = System.Timers.Timer;

namespace ServerProfile
{
    public class LicenseManager : ILicenseManager
    {
        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;
        public event EventHandler OnPluginSaveComplete;

        protected virtual void RaiseOnPluginSaveComplete()
        {
            var handler = OnPluginSaveComplete;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        //use CgiLicenseInfo replace this two cgi
        //private const String CgiLicenseAmount = @"cgi-bin/sysconfig?action=licenseamount";
        //private const String CgiMaximumLicense = @"cgi-bin/sysconfig?action=licensemaxiumamount";

        private const String CgiLicenseInfo = @"cgi-bin/sysconfig?action=licenseinfo";
        private const String CgiVerifyLicenseKey = @"cgi-bin/sysconfig?action=verifylicensekey"; //check online license-amount
        private const String CgiAddLicenseKey = @"cgi-bin/sysconfig?action=addlicensekey";       //online
        private const String CgiAddLicenseFile = @"cgi-bin/sysconfig?action=addlicensexml"; //offline

        public ManagerReadyState ReadyStatus { get; set; }
        public IServer Server;

        public Dictionary<String, String> Localization;
        private UInt16 _totalAmountIgnoreMaximum;
        public UInt16 Amount { get; private set; }
        public UInt16 Maximum { get; private set; }
        public List<Adaptor> Adaptor { get; private set; }
        public Boolean CheckLicenseExpire { get; set; }

        private readonly Timer _reloadLicenseTimer = new Timer();

        private readonly Dictionary<PluginPackage, Int16> _pluginLicense = new Dictionary<PluginPackage, short>();
        public Dictionary<PluginPackage, Int16> PluginLicense
        {
            get { return _pluginLicense; }
        }


        // Constructor
        public LicenseManager(IServer server)
            : this()
        {
            Server = server;
        }

        public LicenseManager()
        {
            Localization = new Dictionary<String, String>
							   {
									{"MessageBox_Error", "Error"},
									{"MessageBox_Confirm", "Confirm"},
									{"MessageBox_Information", "Information"},

									{"Application_AmountConfirm", "License quantity is \"%1\" .\r\nConfirm to register?"},
									{"Application_RegistrationFailed", "Registration failed. Please check the registration key is correct."},
									{"Application_RegistrationFailed2", "Registration failed. Please check Internet connection and registration key."},
									
									{"Application_RegistrationCompleted", "Registration is completed. Quantity of licenses increase to \"%1\"."},
									{"Application_RegistrationTimeout", "License registration timeout."},
								   
									{"Application_LicenseKeyWillExpire", "License key \"%1\" will expire on \"%2\"."},

									{"Application_RegistrationServerNotAvailable", "Can't connect to registration server. (Error code: 1)"},
									{"Application_LicenseKeyError", "License key error. (Error: 2)"},
									{"Application_LicenseKeyUsed", "License key has been used. (Error code: 3)"},
									{"Application_LicenseKeyExpired", "License key has expired. (Erro coder: 4)"},
									{"Application_NetworkCardNotAvailable", "Can't find network card. (Error code: 5)"},
									{"Application_LicenseFileCantUpdate", "Can't update license file. (Error code: 6)"},
							   };
            Localizations.Update(Localization);

            ReadyStatus = ManagerReadyState.New;
            _totalAmountIgnoreMaximum = 0;
            Amount = 0;
            Maximum = 0;
            CheckLicenseExpire = true;
            Adaptor = new List<Adaptor>();
        }

        public void Initialize()
        {
            AddKeyTimer.Elapsed += CheckAddKeyTimeout;
            AddKeyTimer.Interval = AddKeyTimeout;
            AddKeyTimer.SynchronizingObject = Server.Form;

            _reloadLicenseTimer.Elapsed += LoadLicense;
            _reloadLicenseTimer.SynchronizingObject = Server.Form;
        }

        public String Status
        {
            get { return "License : " + ReadyStatus + ", License Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        private const UInt16 AddKeyTimeout = 60000;
        protected readonly Timer AddKeyTimer = new Timer();
        protected readonly Stopwatch _watch = new Stopwatch();
        public void Load()
        {
            ReadyStatus = ManagerReadyState.Loading;

            _watch.Reset();
            _watch.Start();

            LoadDelegate loadLicenseDelegate = LoadLicense;
            loadLicenseDelegate.BeginInvoke(LoadCallback, loadLicenseDelegate);
        }

        public void Load(String xml)
        {
        }

        private delegate void LoadDelegate();
        private void LoadCallback(IAsyncResult result)
        {
            ((LoadDelegate)result.AsyncState).EndInvoke(result);

            if (_loadLicenseFlag)
            {
                _watch.Stop();
                //const String msg = "License Ready";
                //Trace.WriteLine(msg + _watch.Elapsed.TotalSeconds.ToString("0.00"));
                ReadyStatus = ManagerReadyState.Ready;

                if (OnLoadComplete != null)
                    OnLoadComplete(this, null);
            }
        }

        protected Boolean _loadLicenseFlag;
        public virtual void LoadLicense()
        {
            _loadLicenseFlag = false;

            var infoDoc = Xml.LoadXmlFromHttp(CgiLicenseInfo, Server.Credential);

            if (infoDoc != null && infoDoc.InnerXml != "")
            {
                var adaptorNodes = infoDoc.GetElementsByTagName("Adaptor");
                if (adaptorNodes.Count > 0)
                {
                    //make sure there have license info -> clear current license info
                    Adaptor.Clear();
                    _totalAmountIgnoreMaximum = 0;
                    Amount = 0;
                    foreach (var adaptorNode in adaptorNodes)
                    {
                        ParseAdaptorXml((XmlElement)adaptorNode);
                    }

                    String maximum = Xml.GetFirstElementValueByTagName(infoDoc, "Maximum");
                    if (maximum != "")
                    {
                        try
                        {
                            Maximum = Convert.ToUInt16(maximum);

                            //can't large than maximum
                            _totalAmountIgnoreMaximum = Amount;
                            Amount = Math.Min(Amount, Maximum);
                        }
                        catch (Exception)
                        {
                            Maximum = 0;
                        }
                    }
                }
            }

            _loadLicenseFlag = true;

            if (!CheckLicenseExpire) return;

            CheckExpireDelegate checkExpireDelegate = CheckExpire;
            checkExpireDelegate.BeginInvoke(null, null);
        }

        private Boolean _isAlertExpire;
        private delegate void CheckExpireDelegate();
        private void CheckExpire()
        {
            var expireSoonLicense = new List<LicenseInfo>();
            var now = DateTimes.ToUtc(Server.Server.DateTime, 0);

            var hasExpireLicense = false;
            foreach (var adaptor in Adaptor)
            {
                foreach (var licenseInfo in adaptor.LicenseInfo)
                {
                    //is trial & not yet expired, and will expir in 7 days
                    if (!licenseInfo.Trial) continue;
                    hasExpireLicense = true;
                    if (licenseInfo.Expired) continue;

                    var datetime = Array.ConvertAll(licenseInfo.ExpiresDate.Split('-'), Convert.ToInt32);
                    var expiredate = DateTimes.ToUtc(new DateTime(datetime[0], datetime[1], datetime[2]), 0);
                    //already expire?           less then 7 days
                    if ((expiredate < now) || ((expiredate - now) / 86400000.0 <= 7))
                    {
                        expireSoonLicense.Add(licenseInfo);
                    }
                }
            }

            if (hasExpireLicense)
            {
                _reloadLicenseTimer.Enabled = false;

                //check as server 12:00
                var nextDay = new DateTime(Server.Server.DateTime.Year, Server.Server.DateTime.Month, Server.Server.DateTime.Day).AddDays(1);
                _reloadLicenseTimer.Interval = DateTimes.ToUtc(nextDay, 0) - now;

                //check at user local time 12:00
                //var today = DateTime.Now;
                //var nextDay = new DateTime(today.Year, today.Month, today.Day).AddDays(1);
                //_reloadLicenseTimer.Interval = (nextDay - today).TotalMilliseconds;

                _reloadLicenseTimer.Enabled = true;
            }

            //only show 1 alert message box.
            if (expireSoonLicense.Count > 0 && !_isAlertExpire)
            {
                _isAlertExpire = true;

                var info = expireSoonLicense.Select(licenseInfo => Localization["Application_LicenseKeyWillExpire"].Replace("%1", licenseInfo.Serial).Replace("%2", licenseInfo.ExpiresDate))
                                            .ToArray();

                TopMostMessageBox.Show(String.Join(Environment.NewLine, info), Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _isAlertExpire = false;
            }
        }

        protected virtual void LoadLicense(Object sender, EventArgs e)
        {
            _reloadLicenseTimer.Enabled = false;
            LoadLicense();
        }

        private static String[] SplitByLength(String str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToArray();
        }


        private void ParseAdaptorXml(XmlElement node)
        {
            var adaptor = new Adaptor
            {
                Description = Xml.GetFirstElementValueByTagName(node, "Description"),
                Mac = String.Join("-", SplitByLength(Xml.GetFirstElementValueByTagName(node, "MAC"), 2))
            };

            var keyNodes = node.GetElementsByTagName("Key");
            if (keyNodes.Count > 0)
            {
                foreach (var keyNode in keyNodes)
                {
                    var info = ParseKeyXml((XmlElement)keyNode);

                    if (!info.Expired)
                    {
                        Amount += info.Quantity;
                    }

                    adaptor.LicenseInfo.Add(info);
                }
            }
            Adaptor.Add(adaptor);
        }

        private LicenseInfo ParseKeyXml(XmlElement node)
        {
            var info = new LicenseInfo
            {
                Serial = node.GetAttribute("val"),
                Trial = (node.GetAttribute("Trial") == "1"),
                Expired = (node.GetAttribute("Expired") == "1"),
                ExpiresDate = node.GetAttribute("ExpireDate").Replace("/", "-"),
                Quantity = Convert.ToUInt16(node.GetAttribute("Count"))
            };

            return info;
        }

        public void Save()
        {
        }

        private Boolean _onlineRegistratio;
        private Boolean _offlineRegistratio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml">
        /// Online: XXXXX-XXXXX-XXXXX-XXXXX-XXXXX;FF:FF:FF:FF:FF:FF (SN;MAC)
        /// Offline: XML
        /// </param>
        public virtual void Save(String xml)
        {
            _watch.Reset();
            _watch.Start();
            AddKeyTimer.Enabled = true;

            ApplicationForms.ShowProgressBar(Server.Form);
            Application.RaiseIdle(null);

            _onlineRegistratio = _offlineRegistratio = false;
            if (xml.Length == 47)
            {
                _onlineRegistratio = true;
                _licenseKey = xml;
                VerifyKeyDelegate verifyKeyDelegate = VerifyLicenseKey;
                verifyKeyDelegate.BeginInvoke(xml.Split(';')[0], VerifyCallback, verifyKeyDelegate);

                ApplicationForms.ProgressBarValue = 30;
            }
            else
            {
                _offlineRegistratio = true;
                _previousAmount = _totalAmountIgnoreMaximum;
                SubmitLicenseDelegate submitLicenseDelegate = SubmitLicense;
                submitLicenseDelegate.BeginInvoke(xml, SaveCallback, submitLicenseDelegate);

                ApplicationForms.ProgressBarValue = 80;
            }
        }

        public virtual void SavePlugin()
        {
        }

        public virtual void SavePlugin(String xml)
        {
        }

        protected void VerifyCallback(IAsyncResult result)
        {
            if (!AddKeyTimer.Enabled) return;

            String verify = ((VerifyKeyDelegate)result.AsyncState).EndInvoke(result);

            ApplicationForms.ProgressBarValue = 50;

            AddKeyTimer.Enabled = false;

            Boolean passVerify = true;

            switch (verify)
            {
                case "":
                case "0":
                    TopMostMessageBox.Show(Localization["Application_RegistrationFailed"], Localization["MessageBox_Error"],
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;

                case "-2":
                    TopMostMessageBox.Show(Localization["Application_LicenseKeyError"], Localization["MessageBox_Error"],
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;

                case "-3":
                    TopMostMessageBox.Show(Localization["Application_LicenseKeyUsed"], Localization["MessageBox_Error"],
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;
            }

            if (!passVerify)
            {
                ApplicationForms.HideProgressBar();

                if (OnSaveComplete != null)
                    OnSaveComplete(this, null);
                return;
            }

            DialogResult confirm = TopMostMessageBox.Show(Localization["Application_AmountConfirm"].Replace("%1", verify), Localization["MessageBox_Confirm"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (confirm == DialogResult.Yes)
            {
                AddKeyTimer.Enabled = true;

                _previousAmount = _totalAmountIgnoreMaximum;

                SubmitLicenseDelegate submitLicenseDelegate = SubmitLicense;
                submitLicenseDelegate.BeginInvoke(_licenseKey, SaveCallback, submitLicenseDelegate);
            }
            else
            {
                ApplicationForms.HideProgressBar();

                if (OnSaveComplete != null)
                    OnSaveComplete(this, null);
            }
        }

        protected delegate String SubmitLicenseDelegate(String xml);

        protected virtual void SaveCallback(IAsyncResult result)
        {
            String verify = ((SubmitLicenseDelegate)result.AsyncState).EndInvoke(result);

            if (!AddKeyTimer.Enabled)
            {
                ApplicationForms.HideProgressBar();
                return;
            }

            AddKeyTimer.Enabled = false;
            _watch.Stop();
            Trace.WriteLine(@"License Add: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));
            ReadyStatus = ManagerReadyState.Ready;

            Adaptor.Clear();
            var retry = 10;
            while (Adaptor.Count == 0 && retry >= 0)
            {
                ApplicationForms.ProgressBarValue += 5;
                Thread.Sleep(6000);//wait 6 sec to get new license amount, server need a little time to update license file

                LoadLicense();
                retry--;
            }

            var passVerify = true;

            switch (verify)
            {
                case "-1":
                    TopMostMessageBox.Show(Localization["Application_RegistrationServerNotAvailable"], Localization["MessageBox_Error"],
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;

                case "-2":
                    TopMostMessageBox.Show(Localization["Application_LicenseKeyError"], Localization["MessageBox_Error"],
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;

                case "-3":
                    TopMostMessageBox.Show(Localization["Application_LicenseKeyUsed"], Localization["MessageBox_Error"],
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;

                case "-4":
                    TopMostMessageBox.Show(Localization["Application_LicenseKeyExpired"], Localization["MessageBox_Error"],
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;

                case "-5":
                    TopMostMessageBox.Show(Localization["Application_NetworkCardNotAvailable"], Localization["MessageBox_Error"],
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;

                case "-6":
                    TopMostMessageBox.Show(Localization["Application_LicenseFileCantUpdate"], Localization["MessageBox_Error"],
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passVerify = false;
                    break;
            }

            ApplicationForms.ProgressBarValue = 100;

            if (passVerify)
            {
                if (_totalAmountIgnoreMaximum > _previousAmount)
                {
                    TopMostMessageBox.Show(Localization["Application_RegistrationCompleted"].Replace("%1", Amount.ToString()), Localization["MessageBox_Information"],
                                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (_onlineRegistratio)
                    {
                        TopMostMessageBox.Show(Localization["Application_RegistrationFailed2"], Localization["MessageBox_Error"],
                                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (_offlineRegistratio)
                    {
                        TopMostMessageBox.Show(Localization["Application_RegistrationFailed"], Localization["MessageBox_Error"],
                                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (OnSaveComplete != null)
                OnSaveComplete(this, null);

            ApplicationForms.HideProgressBar();
        }

        protected UInt16 _previousAmount;
        protected String _licenseKey;

        private void CheckAddKeyTimeout(Object sender, EventArgs e)
        {
            AddKeyTimer.Enabled = false;
            _watch.Stop();

            TopMostMessageBox.Show(Localization["Application_RegistrationTimeout"], Localization["MessageBox_Error"]
                , MessageBoxButtons.OK, MessageBoxIcon.Stop);

            ApplicationForms.HideProgressBar();

            if (OnSaveComplete != null)
                OnSaveComplete(this, null);
        }

        protected virtual String SubmitLicense(String key)
        {
            if (key.Length == 47)
            {
                //key=SDPFD-CWSDF-LBFDF-SNFJF-NUGYT&mac=002522C2C665
                var temp = key.Split(';');

                return Xml.PostTextToHttp(CgiAddLicenseKey, "key=" + temp[0] + "&mac=" + temp[1].ToUpper(), Server.Credential, AddKeyTimeout, false, 0);
            }

            return Xml.PostTextToHttp(CgiAddLicenseFile, key, Server.Credential, AddKeyTimeout, false, 0);
        }

        protected delegate String VerifyKeyDelegate(String xml);
        protected virtual String VerifyLicenseKey(String key)
        {
            if (key.Length == 29)
                return Xml.PostTextToHttp(CgiVerifyLicenseKey, key, Server.Credential, AddKeyTimeout, false, 0);

            return "";
        }
    }
}
