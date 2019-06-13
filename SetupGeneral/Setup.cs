using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupGeneral
{
    public partial class Setup : UserControl, IControl, IServerUse, IAppUse, IBlockPanelUse, IMinimize
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<String>> OnSelectionChange;

        protected void RaiseOnSelectionChange(string selectionChangeXml)
        {
            if (OnSelectionChange != null)
            {
                OnSelectionChange(this, new EventArgs<string>(selectionChangeXml));
            }
        }

        public String TitleName { get; set; }
        public IApp App { get; set; }
        public IServer Server { get; set; }
        public IBlockPanel BlockPanel { get; set; }

        public Dictionary<String, String> Localization;

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);

        public UInt16 MinimizeHeight
        {
            get { return 0; }
        }
        public Boolean IsMinimize { get; private set; }

        protected OverallControl _overall;
        private PatrolControl _patrol;
        private DurationControl _duration;
        private MailServer _mailServer;
        private FtpServer _ftpServer;
        private SaveImagePath _saveImagePath;
        private ExportVideoPath _exportVideoPath;
        private VideoWindowTitleBar _videoWindowTitleBar;
        private Watermark _watermark;
        private CPULoadingControl _cpuLoadingSetting;
        private AutoSwitchLiveVideoStream _autoSwitchLiveVideoStream;
        private AutoSwitchDecodeIFrame _autoSwitchDecodeIFrame;
        protected StartupOptions _startupOptions;
        private BandwidthControl _bandwidthControl;
        private LockIntervalControl _lockIntervalControl;


        // Constructor
        public Setup()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Control_General", "General"},

                                   {"SetupGeneral_LivePatrolInterval", "Dwell Time of Patrol"},
                                   {"SetupGeneral_ManualRecordDuration", "Manual record duration"},
                                   {"SetupGeneral_MailServer", "Mail server"},
                                   {"SetupGeneral_FTPServer", "FTP server"},
                                   {"SetupGeneral_SaveImagePath", "Save image path"},
                                   {"SetupGeneral_ExportVideoPath", "Export video path"},
                                   {"SetupGeneral_VideoWindowTitleBar", "Video window title bar"},
                                   {"SetupGeneral_CPULoadingUpperBoundary", "CPU loading upper boundary"},
                                   {"SetupGeneral_AutoSwitchLiveVideoStream", "Auto switch live video stream"},
                                   {"SetupGeneral_AutoSwitchDecodeIFrame", "Auto switch decode I-frame"},
                                   {"SetupGeneral_StartupOptions", "Startup Options"},
                                   {"SetupGeneral_BandwidthControlSetting", "Bandwidth Control Setting"},
                                   {"SetupGeneral_AutoLockApplicationTimer", "Auto Lock Application Timer"},
                               };
            Localizations.Update(Localization);

            Name = "General";
            TitleName = Localization["Control_General"];

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackgroundImage = Manager.Background;
            //---------------------------
            Icon = new IconUI2 { IconImage = _icon, IconActivateImage = _iconActivate, IconText = Localization["Control_General"] };
            Icon.Click += DockIconClick;

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);
            //---------------------------
        }



        public virtual void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            if (_overall == null)
            {
                _overall = CreateOverall();
                _overall.Initialize();
            }

            _patrol = new PatrolControl
            {
                Server = Server,
            };
            _patrol.Initialize();

            _duration = new DurationControl
            {
                Server = Server,
            };
            _duration.Initialize();

            _mailServer = new MailServer
            {
                Server = Server,
            };
            _mailServer.Initialize();

            _ftpServer = new FtpServer
            {
                Server = Server,
            };
            _ftpServer.Initialize();

            _saveImagePath = CreateSaveImagePath();
            _saveImagePath.Initialize();

            _exportVideoPath = CreateExportVideoPath();
            _exportVideoPath.Initialize();

            _videoWindowTitleBar = new VideoWindowTitleBar
            {
                Server = Server,
                App = App,
            };
            _videoWindowTitleBar.Initialize();

            _watermark = new Watermark
            {
                Server = Server,
                App = App,
            };
            _watermark.Initialize();

            _cpuLoadingSetting = new CPULoadingControl
            {
                Server = Server
            };
            _cpuLoadingSetting.Initialize();

            _autoSwitchLiveVideoStream = new AutoSwitchLiveVideoStream
            {
                Server = Server,
            };
            _autoSwitchLiveVideoStream.Initialize();

            _autoSwitchDecodeIFrame = new AutoSwitchDecodeIFrame
            {
                Server = Server,
            };
            _autoSwitchDecodeIFrame.Initialize();

            _startupOptions = new StartupOptions
            {
                Server = Server
            };
            _startupOptions.Initialize();

            _bandwidthControl = new BandwidthControl
            {
                Server = Server
            };
            _bandwidthControl.Initialize();

            _lockIntervalControl = new LockIntervalControl()
            {
                Server = Server
            };
            _lockIntervalControl.Initialize();


            _overall.OnPatrolEdit += OverallOnPatrolEdit;
            _overall.OnDurationEdit += OverallOnDurationEdit;
            _overall.OnMailServerEdit += OverallOnMailServerEdit;
            _overall.OnFtpServerEdit += OverallOnFtpServerEdit;
            _overall.OnSaveImagePathEdit += OverallOnSaveImagePathEdit;
            _overall.OnExportVideoPathEdit += OverallOnExportVideoPathEdit;
            _overall.OnVideoWindowTitleBarEdit += OverallOnVideoWindowTitleBarEdit;
            _overall.OnWatermarkEdit += OverallOnWatermarkEdit;

            _overall.OnCPULoadingEdit += OverallOnCPULoadingEdit;
            _overall.OnAutoSwitchLiveVideoStreamEdit += OverallOnAutoSwitchLiveVideoStreamEdit;
            _overall.OnAutoSwitchDecodeIFrameEdit += OverallOnAutoSwitchDecodeIFrameEdit;
            _overall.OnStartupOptionsEdit += OverallOnStartupOptionsEdit;
            _overall.OnBandwidthControlEdit += OverallOnBandwidthControlEdit;
            _overall.OnAutoLockApplicationEdit += _overall_OnAutoLockApplicationEdit;

            contentPanel.Controls.Contains(_overall);

            Server.OnLoadComplete += ServerOnLoadComplete;
           
        }

        protected virtual OverallControl CreateOverall()
        {
            return new OverallControl
            {
                Server = Server,
                App = App,
            };
        }

        protected virtual SaveImagePath CreateSaveImagePath()
        {
            return new SaveImagePath
            {
                Server = Server,
                App = App,
            };
        }

        protected virtual ExportVideoPath CreateExportVideoPath()
        {
            return new ExportVideoPath
            {
                Server = Server,
                App = App,
            };
        }

        private delegate void RefreshContentDelegate(Object sender, EventArgs<String> e);
        private void ServerOnLoadComplete(object sender, EventArgs<string> e)
        {
            if (App.Form.InvokeRequired)
            {
                App.Form.BeginInvoke(new RefreshContentDelegate(ServerOnLoadComplete), sender, e);
                return;
            }

            try
            {
                if (_focusControl == null)
                {
                    _focusControl = _overall;
                }

                Manager.ReplaceControl(_focusControl, _overall, contentPanel, ShowOverall);
                _overall.RefreshContent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        protected Control _focusControl;

        private void OverallOnPatrolEdit(Object sender, EventArgs e)
        {
            _focusControl = _patrol;

            Manager.ReplaceControl(_overall, _patrol, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnDurationEdit(Object sender, EventArgs e)
        {
            _focusControl = _duration;

            Manager.ReplaceControl(_overall, _duration, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnMailServerEdit(Object sender, EventArgs e)
        {
            _focusControl = _mailServer;

            Manager.ReplaceControl(_overall, _mailServer, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnFtpServerEdit(Object sender, EventArgs e)
        {
            _focusControl = _ftpServer;

            Manager.ReplaceControl(_overall, _ftpServer, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnSaveImagePathEdit(Object sender, EventArgs e)
        {
            _focusControl = _saveImagePath;

            Manager.ReplaceControl(_overall, _saveImagePath, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnExportVideoPathEdit(Object sender, EventArgs e)
        {
            _focusControl = _exportVideoPath;

            Manager.ReplaceControl(_overall, _exportVideoPath, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnVideoWindowTitleBarEdit(Object sender, EventArgs e)
        {
            _focusControl = _videoWindowTitleBar;

            _videoWindowTitleBar.GeneratorInformationList();

            Manager.ReplaceControl(_overall, _videoWindowTitleBar, contentPanel, ManagerMoveToSettingComplete);
        }
        private void OverallOnWatermarkEdit(Object sender, EventArgs e)
        {
            _focusControl = _watermark;

            _watermark.GeneratorInformationList();

            Manager.ReplaceControl(_overall, _watermark, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnCPULoadingEdit(Object sender, EventArgs e)
        {
            _focusControl = _cpuLoadingSetting;

            Manager.ReplaceControl(_overall, _cpuLoadingSetting, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnAutoSwitchLiveVideoStreamEdit(Object sender, EventArgs e)
        {
            _focusControl = _autoSwitchLiveVideoStream;

            Manager.ReplaceControl(_overall, _autoSwitchLiveVideoStream, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnAutoSwitchDecodeIFrameEdit(Object sender, EventArgs e)
        {
            _focusControl = _autoSwitchDecodeIFrame;

            Manager.ReplaceControl(_overall, _autoSwitchDecodeIFrame, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnStartupOptionsEdit(Object sender, EventArgs e)
        {
            _focusControl = _startupOptions;

            _startupOptions.ParseSetting();
            Manager.ReplaceControl(_overall, _startupOptions, contentPanel, ManagerMoveToSettingComplete);
        }

        private void OverallOnBandwidthControlEdit(object sender, EventArgs e)
        {
            _focusControl = _bandwidthControl;

            _bandwidthControl.GenerateViewModel();
            Manager.ReplaceControl(_overall, _bandwidthControl, contentPanel, ManagerMoveToSettingComplete);
        }

        private void _overall_OnAutoLockApplicationEdit(object sender, EventArgs e)
        {
            _focusControl = _lockIntervalControl;

            //_lockIntervalControl.GenerateViewModel();
            Manager.ReplaceControl(_overall, _lockIntervalControl, contentPanel, ManagerMoveToSettingComplete);
        }


        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public virtual void ShowContent(Object sender, EventArgs<String> e)
        {
            BlockPanel.ShowThisControlPanel(this);

            ShowOverall();
        }

        private void ShowOverall()
        {
            _focusControl = null;
            _overall.Enabled = true;

            if (!contentPanel.Controls.Contains(_overall))
            {
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(_overall);
            }

            if (Parent != null && Parent.Visible)
            {
                var xml = Manager.SelectionChangedXml(TitleName, TitleName, "", "");
                RaiseOnSelectionChange(xml);
            }

            _overall.Focus();
        }

        public virtual void SelectionChange(Object sender, EventArgs<String> e)
        {
            String item;
            if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
                return;

            switch (item)
            {
                case "SendTestEmail":
                    SendTestEmail();
                    break;

                default:
                    if (item == TitleName || item == "Back")
                    {
                        Manager.ReplaceControl(_focusControl, _overall, contentPanel, ShowOverall);
                    }
                    break;
            }
        }

        private const String CGISendMail = @"cgi-bin/mail?";
        private void SendTestEmail()
        {
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("Mail");
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Port", Server.Configure.MailServer.Port));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("ServerAddress", Server.Configure.MailServer.Credential.Domain));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Account", Server.Configure.MailServer.Credential.UserName));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Password", Server.Configure.MailServer.Credential.Password));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Name", Server.Configure.MailServer.Sender));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("MailAddress", "Tom.Yan@isapsolution.com"));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("To", "Tom.Yan@isapsolution.com"));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Subject", "Test mail from :" + Server.Credential.Domain));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Body", "Test mail" + Environment.NewLine + Server.Credential.Domain + Environment.NewLine + Server.Credential.Port));
            
            xmlDoc.AppendChild(xmlRoot);

            Xml.PostXmlToHttp(CGISendMail, xmlDoc, Server.Credential);
        }

        protected virtual void ManagerMoveToSettingComplete()
        {
            _focusControl.Focus();

            if (_focusControl == _patrol)
                _patrol.ParseSetting();

            if (_focusControl == _duration)
                _duration.ParseSetting();

            if (_focusControl == _mailServer)
                _mailServer.ParseSetting();

            if (_focusControl == _ftpServer)
                _ftpServer.ParseSetting();

            if (_focusControl == _saveImagePath)
                _saveImagePath.ParseSetting();

            if (_focusControl == _exportVideoPath)
                _exportVideoPath.ParseSetting();

            if (_focusControl == _cpuLoadingSetting)
                _cpuLoadingSetting.ParseSetting();

            if (_focusControl == _autoSwitchLiveVideoStream)
                _autoSwitchLiveVideoStream.ParseSetting();

            if (_focusControl == _autoSwitchDecodeIFrame)
                _autoSwitchDecodeIFrame.ParseSetting();

            if (_focusControl == _lockIntervalControl)
                _lockIntervalControl.ParseSetting();

            if (OnSelectionChange != null)
            {
                var text = TitleName + "  /  ";
                text += (Localization.ContainsKey("SetupGeneral_" + _focusControl.Name))
                    ? Localization["SetupGeneral_" + _focusControl.Name]
                    : _focusControl.Name;

                var xml = Manager.SelectionChangedXml(TitleName, text, "Back", string.Empty);
                RaiseOnSelectionChange(xml);
            }
        }
        private void DockIconClick(Object sender, EventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else //dont hide self to keep at last selection panel on screen
                ShowOverall();
        }

        public void Minimize()
        {
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
                BlockPanel.HideThisControlPanel(this);

            Deactivate();
            ((IconUI2)Icon).IsActivate = false;

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            ShowContent(this, null);

            ((IconUI2)Icon).IsActivate = true;

            IsMinimize = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }
        public void WatermarkShow()
        {
            _overall.WatermarkShow();
        }

        public void SISSetupView()
        {
            _overall.SISSetupView();
        }

        protected void MobileNvrView()
        {
            _overall.MobileNvrView();
        }
    }
}
