using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using Constant;
using Constant.Utility;
using Device;
using DeviceConstant;
using Interface;

namespace ServerProfile
{
    public class ConfigureManager : IConfigureManager
    {
        private const String CgiLoadSetting = @"cgi-bin/generalconfig?action=load";
        private const String CgiSaveSetting = @"cgi-bin/generalconfig?action=save";

        private bool _displayDeviceId;
        private bool _displayGroupId;
        private bool _displayNvrId;

        public event EventHandler OnLoadComplete;
        public event EventHandler OnSaveComplete;

        protected virtual void RaiseOnSaveComplete(EventArgs e)
        {
            var handler = OnSaveComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public ManagerReadyState ReadyStatus { get; set; }

        public IServer Server { get; set; }

        public UInt16 PatrolInterval { get; set; }
        public Boolean DisplayDeviceId
        {
            get { return _displayDeviceId; }
            set
            {
                _displayDeviceId = value;
                BasicDevice.DisplayDeviceId = value;
            }
        }
        public Boolean DisplayGroupId
        {
            get { return _displayGroupId; }
            set
            {
                _displayGroupId = value;
                DeviceGroup.DisplayGroupId = value;
            }
        }
        public Boolean DisplayNVRId
        {
            get { return _displayNvrId; }
            set
            {
                _displayNvrId = value;
                NVR.DisplayNVRId = value;
            }
        }
        public Boolean StorageAlert { get; set; }
        public Boolean EnableJoystick { get; set; }
        public Boolean EnableAxisJoystick { get; set; }     // Val added, 2015.11.20. For Axis Joystick. Under design, interface has to be modified right here.
        public UInt16 ManualRecordDuration { get; set; }
        public Int16 InstantPlaybackSeconds { get; set; }
        public Boolean ImageWithTimestamp { get; set; }
        public MailServer MailServer { get; private set; }
        public FtpServer FtpServer { get; private set; }
        public String SaveImagePath { get; set; }
        public String ExportVideoPath { get; set; }
        public UInt16 ExportVideoMaxiFileSize { get; set; }

        public Boolean StretchLiveVideo { get; set; }
        public Boolean StretchPlaybackVideo { get; set; }
        public Boolean KeepLastFrame { get; set; }

        public List<VideoWindowTitleBarInformation> VideoWindowTitleBarInformations { get; set; }
        public String VideoTitleBarFontFamify { get; set; }
        public UInt16 VideoTitleBarFontSize { get; set; }
        public String VideoTitleBarFontColor { get; set; }
        public String VideoTitleBarBackgroundColor { get; set; }

        public String WatermarkFontFamify { get; set; }
        public UInt16 WatermarkFontSize { get; set; }
        public String WatermarkFontColor { get; set; }
        public String WatermarkText { get; set; }


        public UInt16 CPULoadingUpperBoundary { get; set; }
        public UInt16 AutoLockApplicationTimer { get; set; }

        public Boolean EnableAutoSwitchLiveStream { get; set; }
        public UInt16 AutoSwitchLiveHighProfileCount { get; set; }
        public UInt16 AutoSwitchLiveLowProfileCount { get; set; }

        public Boolean EnableAutoSwitchDecodeIFrame { get; set; }
        public UInt16 AutoSwitchDecodeIFrameCount { get; set; }

        public Store Store { get; private set; }
        public String TransactionTimeOption { get; set; }
        // Lock Application after idling ? Seconds
        //public Int16 LockApplicationIdleTime { get; set; }

        //-----------Custom Live / Playback Compression
        public CustomStreamSetting CustomStreamSetting { get; set; }

        // Startup Options
        public StartupOptions StartupOptions { get; set; }

        //Bandwith Control
        public Boolean EnableBandwidthControl { get; set; }
        public Bitrate BandwidthControlBitrate { get; set; }
        public UInt16 BandwidthControlStream { get; set; }

        public Boolean WithArchiveServer { get; set; }
        public Boolean EnablePlaybackSmooth { get; set; }

        public Boolean CameraLastImage { get; set; }
        public Boolean FullScreenBestResolution { get; set; }
        public Boolean EnableUserDefine { get; set; }


        public ConfigureManager()
        {
            ReadyStatus = ManagerReadyState.New;

            MailServer = new MailServer();
            FtpServer = new FtpServer();
            PatrolInterval = 15;
            ManualRecordDuration = 600;
            InstantPlaybackSeconds = -10;
            ImageWithTimestamp = true;
            DisplayDeviceId = true;
            DisplayGroupId = true;
            DisplayNVRId = true;
            StorageAlert = true;
            EnableJoystick = true;
            EnableAxisJoystick = false; // default to normal joystick
            SaveImagePath = "Desktop";//default
            ExportVideoPath = "Desktop";//default
            ExportVideoMaxiFileSize = 1843;
            CPULoadingUpperBoundary = 95;
            AutoLockApplicationTimer = 0;
            EnableAutoSwitchLiveStream = false; //DEFAULT FALSE!!!
            AutoSwitchLiveHighProfileCount = 4;
            AutoSwitchLiveLowProfileCount = 9;

            EnableAutoSwitchDecodeIFrame = false; //DEFAULT FALSE!!!
            AutoSwitchDecodeIFrameCount = 4;

            StretchLiveVideo = false;
            StretchPlaybackVideo = false;
            KeepLastFrame = false;
            EnableBandwidthControl = false;//DEFAULT FALSE!!!
            BandwidthControlStream = 1;
            CustomStreamSetting = new CustomStreamSetting();

            WithArchiveServer = false;//DEFAULT FALSE!!!
            EnablePlaybackSmooth = false;//DEFAULT FALSE!!!
            VideoWindowTitleBarInformations = new List<VideoWindowTitleBarInformation>
                                                  {
                                                      VideoWindowTitleBarInformation.CameraName,
                                                      VideoWindowTitleBarInformation.Compression,
                                                      VideoWindowTitleBarInformation.Resolution,
                                                      VideoWindowTitleBarInformation.Bitrate,
                                                      VideoWindowTitleBarInformation.FPS,
                                                      VideoWindowTitleBarInformation.DateTime
                                                  };


            //DefaultView	 = new DeviceGroup();

            VideoTitleBarFontFamify = "Arial";
            VideoTitleBarFontSize = 8;
            VideoTitleBarBackgroundColor = "#0000FF";
            VideoTitleBarFontColor = "#FFFFFF";

            WatermarkFontFamify = "Arial";
            WatermarkFontSize = 8;
            WatermarkFontColor = "#FFFFFF";
            WatermarkText = "";


            StartupOptions = new StartupOptions();

            Store = new Store();

            TransactionTimeOption = "System";//System , Transaction

            CameraLastImage = false;
            FullScreenBestResolution = false;
            EnableUserDefine = false;
        }

        public String Status
        {
            get { return "Configure : " + ReadyStatus + ", Used Time: " + _watch.Elapsed.TotalSeconds.ToString("0.00") + "Sec"; }
        }

        public void Initialize()
        {
        }

        private readonly Stopwatch _watch = new Stopwatch();
        public void Load()
        {
            ReadyStatus = ManagerReadyState.Loading;

            _watch.Reset();
            _watch.Start();

            LoadDelegate loadGeneralSettingDelegate = LoadGeneralSetting;
            loadGeneralSettingDelegate.BeginInvoke(LoadCallback, loadGeneralSettingDelegate);
        }

        public void Load(String xml)
        {

        }

        private delegate void LoadDelegate();
        private void LoadCallback(IAsyncResult result)
        {
            ((LoadDelegate)result.AsyncState).EndInvoke(result);

            if (_loadGeneralFlag)
            {
                _watch.Stop();

                ReadyStatus = ManagerReadyState.Ready;

                if (OnLoadComplete != null)
                    OnLoadComplete(this, null);
            }
        }

        private Boolean _loadGeneralFlag;
        public void LoadGeneralSetting()
        {
            //<Config>
            //  <LivePatrolInterval>15</LivePatrolInterval>
            //  <ManualRecordDuration>600</ManualRecordDuration>
            //</Config>
            _loadGeneralFlag = false;
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadSetting, Server.Credential);
            ParserGeneralSetting(xmlDoc);

            _loadGeneralFlag = true;
        }

        protected virtual void ParserGeneralSetting(XmlDocument xmlDoc)
        {
            if (xmlDoc == null) return;

            XmlNode node = xmlDoc.SelectSingleNode("Config");

            if (node == null) return;

            String patrol = Xml.GetFirstElementValueByTagName(node, "LivePatrolInterval");
            if (patrol != "")
            {
                try
                {
                    PatrolInterval = Convert.ToUInt16(patrol);
                }
                catch (Exception)
                {
                    PatrolInterval = 15;
                }
            }

            String displayDeviceId = Xml.GetFirstElementValueByTagName(node, "DisplayDeviceId");
            if (!String.IsNullOrEmpty(displayDeviceId))
                DisplayDeviceId = (displayDeviceId == "true");

            String displayGroupId = Xml.GetFirstElementValueByTagName(node, "DisplayGroupId");
            if (!String.IsNullOrEmpty(displayGroupId))
                DisplayGroupId = (displayGroupId == "true");

            String displayNVRId = Xml.GetFirstElementValueByTagName(node, "DisplayNVRId");
            if (!String.IsNullOrEmpty(displayGroupId))
                DisplayNVRId = (displayNVRId == "true");

            String storageAlert = Xml.GetFirstElementValueByTagName(node, "StorageAlert");
            if (!String.IsNullOrEmpty(storageAlert))
                StorageAlert = (storageAlert == "true");

            String enableJoystick = Xml.GetFirstElementValueByTagName(node, "EnableJoystick");
            if (!String.IsNullOrEmpty(enableJoystick))
                EnableJoystick = (enableJoystick == "true");

            // Val added, 2015.11.20. For Axis Joystick.
            String enableAxisJoystick = Xml.GetFirstElementValueByTagName(node, "EnableAxisJoystick");
            if (!String.IsNullOrEmpty(enableAxisJoystick))
                EnableAxisJoystick = (enableAxisJoystick == "true");

            String manualRecordDuration = Xml.GetFirstElementValueByTagName(node, "ManualRecordDuration");
            if (!String.IsNullOrEmpty(manualRecordDuration))
                ManualRecordDuration = Convert.ToUInt16(manualRecordDuration);

            String timestamp = Xml.GetFirstElementValueByTagName(node, "ImageWithTimestamp");
            if (!String.IsNullOrEmpty(timestamp))
                ImageWithTimestamp = (timestamp == "true");

            String saveImagePath = Xml.GetFirstElementValueByTagName(node, "SaveImagePath");
            if (!String.IsNullOrEmpty(saveImagePath))
                SaveImagePath = saveImagePath;

            String exportVideoPath = Xml.GetFirstElementValueByTagName(node, "ExportVideoPath");
            if (!String.IsNullOrEmpty(exportVideoPath))
                ExportVideoPath = exportVideoPath;

            //String exportVideoMaxiFileSize = Xml.GetFirstElementValueByTagName(node, "ExportVideoMaxiFileSize");
            //ExportVideoMaxiFileSize = (ushort) (String.IsNullOrEmpty(exportVideoMaxiFileSize)
            //                                        ? 1843 //1.8GB
            //                                        : Convert.ToUInt16(exportVideoMaxiFileSize));

            String stretchLiveVideo = Xml.GetFirstElementValueByTagName(node, "StretchLiveVideo");
            if (!String.IsNullOrEmpty(stretchLiveVideo))
                StretchLiveVideo = (stretchLiveVideo == "true");

            String stretchPlaybackVideo = Xml.GetFirstElementValueByTagName(node, "StretchPlaybackVideo");
            if (!String.IsNullOrEmpty(stretchPlaybackVideo))
                StretchPlaybackVideo = (stretchPlaybackVideo == "true");

            KeepLastFrame = Xml.GetFirstElementValueByTagName(node, "KeepLastFrame") == "true";
            CameraLastImage = Xml.GetFirstElementValueByTagName(node, "LastPictureEnabled") == "true";

            ParseMailServerFromXml((XmlElement)node.SelectSingleNode("Mail"));
            ParseFtpServerFromXml((XmlElement)node.SelectSingleNode("Ftp"));
            ParseStoreFromXml((XmlElement)node.SelectSingleNode("Store"));
            ParseTitleBarInformationsFromXml((XmlElement)node.SelectSingleNode("TitleBarInformations"));
            ParseWatermarkInformationsFromXml((XmlElement)node.SelectSingleNode("WatermarkInformations"));
            ParseStartupOptionsFromXml((XmlElement)node.SelectSingleNode("StartupOptions"));

            var cpuLoadingUpperBoundary = Xml.GetFirstElementValueByTagName(node, "CPULoadingUpperBoundary");
            if (!String.IsNullOrEmpty(cpuLoadingUpperBoundary))
                CPULoadingUpperBoundary = Convert.ToUInt16(cpuLoadingUpperBoundary);

            var autoLockApplicationTimer = Xml.GetFirstElementValueByTagName(node, "AutoLockApplicationTimer");
            if (!String.IsNullOrEmpty(autoLockApplicationTimer))
                AutoLockApplicationTimer = Convert.ToUInt16(autoLockApplicationTimer);

            var enableAutoSwitchLiveStream = Xml.GetFirstElementValueByTagName(node, "EnableAutoSwitchLiveStream");
            if (!String.IsNullOrEmpty(enableAutoSwitchLiveStream))
                EnableAutoSwitchLiveStream = (enableAutoSwitchLiveStream == "true");

            var autoSwitchLiveHighProfileCount = Xml.GetFirstElementValueByTagName(node, "AutoSwitchLiveHighProfileCount");
            if (!String.IsNullOrEmpty(autoSwitchLiveHighProfileCount))
                AutoSwitchLiveHighProfileCount = Convert.ToUInt16(autoSwitchLiveHighProfileCount);

            var autoSwitchLiveLowProfileCount = Xml.GetFirstElementValueByTagName(node, "AutoSwitchLiveLowProfileCount");
            if (!String.IsNullOrEmpty(autoSwitchLiveLowProfileCount))
                AutoSwitchLiveLowProfileCount = Convert.ToUInt16(autoSwitchLiveLowProfileCount);

            var enableAutoSwitchDecodeIFrame = Xml.GetFirstElementValueByTagName(node, "EnableAutoSwitchDecodeIFrame");
            if (!String.IsNullOrEmpty(enableAutoSwitchDecodeIFrame))
                EnableAutoSwitchDecodeIFrame = (enableAutoSwitchDecodeIFrame == "true");

            var autoSwitchDecodeIFrameCount = Xml.GetFirstElementValueByTagName(node, "AutoSwitchDecodeIFrameCount");
            if (!String.IsNullOrEmpty(autoSwitchDecodeIFrameCount))
                AutoSwitchDecodeIFrameCount = Convert.ToUInt16(autoSwitchDecodeIFrameCount);

            var enableBandwidthControl = Xml.GetFirstElementValueByTagName(node, "EnableBandwidthControl");
            if (!String.IsNullOrEmpty(enableBandwidthControl))
                EnableBandwidthControl = (enableBandwidthControl == "true");

            if (Server is INVR)
            {
                var bandwidthBitrate = Xml.GetFirstElementValueByTagName(node, "BandwidthBitrate");
                if (!String.IsNullOrEmpty(bandwidthBitrate))
                    BandwidthControlBitrate = Bitrates.ToIndex(bandwidthBitrate);

                var bandwidthStream = Xml.GetFirstElementValueByTagName(node, "BandwidthStream");
                if (!String.IsNullOrEmpty(bandwidthStream))
                    BandwidthControlStream = Convert.ToUInt16(bandwidthStream);
            }

            String transactionTimeReference = Xml.GetFirstElementValueByTagName(node, "TransactionTimeReference");
            if (transactionTimeReference != "")
                TransactionTimeOption = transactionTimeReference;
        }

        private void ParseMailServerFromXml(XmlElement node)
        {
            //<Mail>
            //  <Sender>Deray</Sender>
            //  <MailAddress>deray@deray.org</MailAddress>
            //  <Server>mail.deray.org</Server>
            //  <Security>PLAIN</Security>
            //  <Port>25</Port>
            //  <Account>deray</Account>
            //  <Password>123456</Password>
            //</Mail>
            if (node == null) return;

            MailServer.Sender = Xml.GetFirstElementValueByTagName(node, "Sender");
            MailServer.MailAddress = Xml.GetFirstElementValueByTagName(node, "MailAddress");
            String security = Xml.GetFirstElementValueByTagName(node, "Security");

            if (Enum.IsDefined(typeof(SecurityType), security))
                MailServer.Security = (SecurityType)Enum.Parse(typeof(SecurityType), security, true);

            String port = Xml.GetFirstElementValueByTagName(node, "Port");
            if (port != "")
                MailServer.Port = Convert.ToUInt16(port);

            MailServer.Credential.Domain = Xml.GetFirstElementValueByTagName(node, "Server").Trim();
            MailServer.Credential.UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Account"));
            MailServer.Credential.Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Password"));
        }

        private void ParseFtpServerFromXml(XmlElement node)
        {
            //<Ftp>
            //  <Server>ftp.deray.org</Server>
            //  <Directory>ftp.deray.org</Directory>
            //  <Port>21</Port>
            //  <Account>deray</Account>
            //  <Password>123456</Password>
            //</Ftp>
            if (node == null) return;

            FtpServer.Directory = Xml.GetFirstElementValueByTagName(node, "Directory");

            String port = Xml.GetFirstElementValueByTagName(node, "Port");
            if (port != "")
                FtpServer.Port = Convert.ToUInt16(port);

            FtpServer.Credential.Domain = Xml.GetFirstElementValueByTagName(node, "Server").Trim();
            FtpServer.Credential.UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Account"));
            FtpServer.Credential.Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(node, "Password"));
        }

        private void ParseStoreFromXml(XmlElement node)
        {
            //<Store>
            //  <Id>false</Id>
            //  <Name>MJPEG</Name>
            //  <Address>640</Address>
            //  <Phone>480</Phone>
            //</Store>
            if (node == null) return;

            Store.Id = Xml.GetFirstElementValueByTagName(node, "Id");
            Store.Name = Xml.GetFirstElementValueByTagName(node, "Name");
            Store.Address = Xml.GetFirstElementValueByTagName(node, "Address");
            Store.Phone = Xml.GetFirstElementValueByTagName(node, "Phone");
        }

        private void ParseStartupOptionsFromXml(XmlElement node)
        {
            //<StartupOptions>
            //    <Enabled>true</Enabled>
            //    <VideoTitleBar>true</VideoTitleBar>
            //    <FullScreen>true</FullScreen>
            //    <HideToolbar>true</HideToolbar>
            //    <HidePanel>true</HidePanel>
            //    <TotalBitrate>512</TotalBitrate>
            //    <ViewTour>true</ViewTour>
            //    <View>01</View>
            //</StartupOptions>
            if (node == null) return;

            StartupOptions.Enabled = (Xml.GetFirstElementValueByTagName(node, "Enabled") == "true");
            StartupOptions.VideoTitleBar = (Xml.GetFirstElementValueByTagName(node, "VideoTitleBar") == "true");
            StartupOptions.FullScreen = (Xml.GetFirstElementValueByTagName(node, "FullScreen") == "true");
            StartupOptions.HidePanel = (Xml.GetFirstElementValueByTagName(node, "HidePanel") == "true");

            var strBitrate = Xml.GetFirstElementValueByTagName(node, "TotalBitrate");
            if (!String.IsNullOrEmpty(strBitrate))
                StartupOptions.TotalBitrate = Convert.ToInt16(strBitrate);

            StartupOptions.GroupPatrol = (Xml.GetFirstElementValueByTagName(node, "ViewTour") == "true");
            StartupOptions.DeviceGroup = Xml.GetFirstElementValueByTagName(node, "View");
        }

        private void ParseTitleBarInformationsFromXml(XmlElement node)
        {
            //<TitleBarInformations>
            //  <Information seq="1">ChannelNumber</Information>
            //  <Information seq="2">CameraName</Information>
            //  <Information seq="3">FPS</Information>
            //  <Information seq="4">Compression</Information>
            //  <Information seq="5">Resolution</Information>
            //  <Information seq="6">Bitrate</Information>
            //  <Information  seq="7">Datetime</Information>
            //  <FontFamily>Arial</FontFamily>
            //  <FontSize>8</FontSize>
            //  <FontColor>#FFFFFF</FontColor>
            //  <BackgroundColor>#0000FF</BackgroundColor>
            //</TitleBarInformations>
            VideoWindowTitleBarInformations.Clear();

            if (node == null)
            {
                ParseTitleBarInformationsToDefault();
                return;
            }

            var informations = node.SelectNodes("Information");
            if (informations == null)
            {
                ParseTitleBarInformationsToDefault();
                return;
            }

            foreach (XmlElement information in informations)
            {
                var info = VideoWindowTitleBarInformationFormats.ToIndex(information.InnerText);

                if (info != VideoWindowTitleBarInformation.NA)
                    VideoWindowTitleBarInformations.Add(info);
            }

            var fontFamily = Xml.GetFirstElementValueByTagName(node, "FontFamily");
            if (!String.IsNullOrEmpty(fontFamily))
                VideoTitleBarFontFamify = fontFamily;

            var fontSize = Xml.GetFirstElementValueByTagName(node, "FontSize");
            if (!String.IsNullOrEmpty(fontSize))
                VideoTitleBarFontSize = Convert.ToUInt16(fontSize);

            var fontColor = Xml.GetFirstElementValueByTagName(node, "FontColor");
            if (!String.IsNullOrEmpty(fontColor))
                VideoTitleBarFontColor = fontColor;

            var backgroundColor = Xml.GetFirstElementValueByTagName(node, "BackgroundColor");
            if (!String.IsNullOrEmpty(backgroundColor))
                VideoTitleBarBackgroundColor = backgroundColor;
        }

        private void ParseWatermarkInformationsFromXml(XmlElement node)
        {
            //<TitleBarInformations>
            //  <Information seq="1">ChannelNumber</Information>
            //  <Information seq="2">CameraName</Information>
            //  <Information seq="3">FPS</Information>
            //  <Information seq="4">Compression</Information>
            //  <Information seq="5">Resolution</Information>
            //  <Information seq="6">Bitrate</Information>
            //  <Information  seq="7">Datetime</Information>
            //  <FontFamily>Arial</FontFamily>
            //  <FontSize>8</FontSize>
            //  <FontColor>#FFFFFF</FontColor>
            //  <BackgroundColor>#0000FF</BackgroundColor>
            //</TitleBarInformations>
            /*
            VideoWindowTitleBarInformations.Clear();

            if (node == null)
            {
                ParseTitleBarInformationsToDefault();
                return;
            }
            
            var informations = node.SelectNodes("Information");
            if (informations == null)
            {
              //  ParseTitleBarInformationsToDefault();
                return;
            }
           

            foreach (XmlElement information in informations)
            {
                var info = VideoWindowTitleBarInformationFormats.ToIndex(information.InnerText);

                if (info != VideoWindowTitleBarInformation.NA)
                    VideoWindowTitleBarInformations.Add(info);
            } */

            var fontFamily = Xml.GetFirstElementValueByTagName(node, "FontFamily");
            if (!String.IsNullOrEmpty(fontFamily))
                WatermarkFontFamify = fontFamily;

            var fontSize = Xml.GetFirstElementValueByTagName(node, "FontSize");
            if (!String.IsNullOrEmpty(fontSize))
                WatermarkFontSize = Convert.ToUInt16(fontSize);

            var fontColor = Xml.GetFirstElementValueByTagName(node, "FontColor");
            if (!String.IsNullOrEmpty(fontColor))
                WatermarkFontColor = fontColor;

            var text = Xml.GetFirstElementValueByTagName(node, "Text");
            if (!String.IsNullOrEmpty(text))
                WatermarkText = text;
        }

        private void ParseTitleBarInformationsToDefault()
        {
            VideoWindowTitleBarInformations.Clear();

            //VideoWindowTitleBarInformations.Add(VideoWindowTitleBarInformation.ChannelNumber);
            VideoWindowTitleBarInformations.Add(VideoWindowTitleBarInformation.CameraName);
            VideoWindowTitleBarInformations.Add(VideoWindowTitleBarInformation.Compression);
            VideoWindowTitleBarInformations.Add(VideoWindowTitleBarInformation.Resolution);
            VideoWindowTitleBarInformations.Add(VideoWindowTitleBarInformation.Bitrate);
            VideoWindowTitleBarInformations.Add(VideoWindowTitleBarInformation.FPS);
            VideoWindowTitleBarInformations.Add(VideoWindowTitleBarInformation.DateTime);
        }

        public void Save()
        {
            ReadyStatus = ManagerReadyState.Saving;

            _watch.Reset();
            _watch.Start();

            SaveDelegate saveGeneralSettingDelegate = SaveGeneralSetting;
            saveGeneralSettingDelegate.BeginInvoke(SaveCallback, saveGeneralSettingDelegate);
        }

        public void Save(String xml)
        {
        }

        private delegate void SaveDelegate();
        private void SaveCallback(IAsyncResult result)
        {
            ((SaveDelegate)result.AsyncState).EndInvoke(result);

            if (_saveGeneralFlag)
            {
                _watch.Stop();

                Trace.WriteLine(@"Configure Save: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

                ReadyStatus = ManagerReadyState.Ready;

                RaiseOnSaveComplete(EventArgs.Empty);
            }
        }

        private Boolean _saveGeneralFlag;
        private void SaveGeneralSetting()
        {
            // SaveImagePath
            if (String.IsNullOrEmpty(SaveImagePath))
                SaveImagePath = "Desktop";
            if (SaveImagePath == Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
            {
                SaveImagePath = "Desktop";
            }
            else if (SaveImagePath == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
            {
                SaveImagePath = "Document";
            }
            else if (SaveImagePath == Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
            {
                SaveImagePath = "Picture";
            }

            if (String.IsNullOrEmpty(ExportVideoPath))
                ExportVideoPath = "Desktop";
            if (ExportVideoPath == Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
            {
                ExportVideoPath = "Desktop";
            }
            else if (ExportVideoPath == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
            {
                ExportVideoPath = "Document";
            }
            else if (ExportVideoPath == Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
            {
                ExportVideoPath = "Picture";
            }

            _saveGeneralFlag = false;

            //<Config>
            //  <LivePatrolInterval>15</LivePatrolInterval>
            //  <DisplayDeviceId>true</DisplayDeviceId>
            //  <StorageAlert>true</DisplayDeviceId>
            //  <EnableJoystick>true</DisplayDeviceId>
            //  <ImageWithTimestamp>true</DisplayDeviceId>
            //</Config>
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadSetting, Server.Credential);
            if (orangeXmlDoc == null)
            {
                return;
            }

            var config = CreateSavedConfigure(orangeXmlDoc.InnerXml);

            var configXmlString = config.ToString();
            if (!String.Equals(orangeXmlDoc.InnerXml, configXmlString))
            {
                // Save to Media server in XML format
                Xml.PostXmlToHttp(CgiSaveSetting, configXmlString, Server.Credential);
            }

            _saveGeneralFlag = true;
        }

        protected virtual XElement CreateSavedConfigure(string xml)
        {
            var config = string.IsNullOrEmpty(xml) ? new XElement("Config") : XElement.Parse(xml);
            config.SetElementValue("LivePatrolInterval", PatrolInterval);
            config.SetElementValue("DisplayDeviceId", DisplayDeviceId);
            config.SetElementValue("DisplayGroupId", DisplayGroupId);
            config.SetElementValue("DisplayNVRId", DisplayNVRId);
            config.SetElementValue("StorageAlert", StorageAlert);
            config.SetElementValue("EnableJoystick", EnableJoystick);
            config.SetElementValue("EnableAxisJoystick", EnableAxisJoystick);       // Val added, 2015.11.20. For Axis Joystick.
            config.SetElementValue("ManualRecordDuration", ManualRecordDuration);
            config.SetElementValue("ImageWithTimestamp", ImageWithTimestamp);

            config.SetElementValue("SaveImagePath", SaveImagePath);
            config.SetElementValue("ExportVideoPath", ExportVideoPath);

            config.SetElementValue("StretchLiveVideo", StretchLiveVideo);
            config.SetElementValue("StretchPlaybackVideo", StretchPlaybackVideo);
            config.SetElementValue("KeepLastFrame", KeepLastFrame);

            config.SetElementValue("CPULoadingUpperBoundary", CPULoadingUpperBoundary);

            config.SetElementValue("AutoLockApplicationTimer", AutoLockApplicationTimer);

            config.SetElementValue("EnableAutoSwitchLiveStream", EnableAutoSwitchLiveStream);
            config.SetElementValue("AutoSwitchLiveHighProfileCount", AutoSwitchLiveHighProfileCount);
            config.SetElementValue("AutoSwitchLiveLowProfileCount", AutoSwitchLiveLowProfileCount);

            config.SetElementValue("EnableAutoSwitchDecodeIFrame", EnableAutoSwitchDecodeIFrame);
            config.SetElementValue("AutoSwitchDecodeIFrameCount", AutoSwitchDecodeIFrameCount);

            config.SetElementValue("EnableBandwidthControl", EnableBandwidthControl);
            config.SetElementValue("LastPictureEnabled", CameraLastImage);

            if (Server is INVR)
            {
                config.SetElementValue("BandwidthBitrate", EnableBandwidthControl ? Bitrates.ToString(BandwidthControlBitrate) : Bitrates.ToString(Bitrate.NA));
                config.SetElementValue("BandwidthStream", EnableBandwidthControl ? BandwidthControlStream.ToString() : "1");
            }

            config.AppendOrReplace(ParseMailServerToXml());
            config.AppendOrReplace(ParseFtpServerToXml());
            config.AppendOrReplace(ParseStoreToXml());
            config.AppendOrReplace(ParseTitleBarInformationsToXml());
            config.AppendOrReplace(ParseWatermarkInformationsToXml());
            config.AppendOrReplace(ParseStartupOptionsToXml());

            config.SetElementValue("TransactionTimeReference", TransactionTimeOption);

            return config;
        }

        private XElement ParseMailServerToXml()
        {
            //<Mail>
            //  <Sender>Deray</Sender>
            //  <MailAddress>deray@deray.org</MailAddress>
            //  <Server>mail.deray.org</Server>
            //  <Security>PLAIN</Security>
            //  <Port>25</Port>
            //  <Account>deray</Account>
            //  <Password>123456</Password>
            //</Mail>
            XElement node = new XElement("Mail");

            node.SetElementValue("Sender", MailServer.Sender);
            node.SetElementValue("MailAddress", MailServer.MailAddress);
            node.SetElementValue("Server", MailServer.Credential.Domain);
            node.SetElementValue("Security", MailServer.Security);
            node.SetElementValue("Port", MailServer.Port);
            node.SetElementValue("Account", Encryptions.EncryptDES(MailServer.Credential.UserName));
            node.SetElementValue("Password", Encryptions.EncryptDES(MailServer.Credential.Password));

            return node;
        }

        private XElement ParseFtpServerToXml()
        {
            //<Ftp>
            //  <Server>ftp.deray.org</Server>
            //  <Directory>ftp.deray.org</Directory>
            //  <Port>21</Port>
            //  <Account>deray</Account>
            //  <Password>123456</Password>
            //</Ftp>
            XElement node = new XElement("Ftp");

            node.SetElementValue("Server", FtpServer.Credential.Domain);
            node.SetElementValue("Directory", FtpServer.Directory);
            node.SetElementValue("Port", FtpServer.Port);
            node.SetElementValue("Account", Encryptions.EncryptDES(FtpServer.Credential.UserName));
            node.SetElementValue("Password", Encryptions.EncryptDES(FtpServer.Credential.Password));

            return node;
        }

        private static XmlElement AppendCustimStreamSetting(XmlDocument xmlDoc, String tagName, CustomStreamSetting setting)
        {
            XmlElement node = xmlDoc.CreateElement(tagName);
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Enable", setting.Enable ? "true" : "false"));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Stream", setting.StreamId));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Encode", Compressions.ToString(setting.Compression)));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Width", Resolutions.ToWidth(setting.Resolution)));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Height", Resolutions.ToHeight(setting.Resolution)));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Bitrate", Bitrates.ToString(setting.Bitrate)));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Quality", setting.Quality));
            node.AppendChild(xmlDoc.CreateXmlElementWithText("Fps", setting.Framerate));
            return node;
        }

        private XElement ParseStoreToXml()
        {
            //<Store>
            //  <Id>false</Id>
            //  <Name>MJPEG</Name>
            //  <Address>640</Address>
            //  <Phone>480</Phone>
            //</Store>
            var node = new XElement("Store");

            node.SetElementValue("Id", Store.Id);
            node.SetElementValue("Name", Store.Name);
            node.SetElementValue("Address", Store.Address);
            node.SetElementValue("Phone", Store.Phone);

            return node;
        }

        private XElement ParseStartupOptionsToXml()
        {
            //<StartupOptions>
            //    <Enabled>true</Enabled>
            //    <VideoTitleBar>true</VideoTitleBar>
            //    <FullScreen>true</FullScreen>
            //    <HideToolbar>true</HideToolbar>
            //    <HidePanel>true</HidePanel>
            //    <TotalBitrate>512</TotalBitrate>
            //    <ViewTour>true</ViewTour>
            //    <View>01</View>
            //</StartupOptions>
            var node = new XElement("StartupOptions");

            node.SetElementValue("Enabled", StartupOptions.Enabled);
            node.SetElementValue("VideoTitleBar", StartupOptions.Enabled ? StartupOptions.VideoTitleBar : false);
            node.SetElementValue("FullScreen", StartupOptions.Enabled ? StartupOptions.FullScreen : false);
            node.SetElementValue("HidePanel", StartupOptions.Enabled ? StartupOptions.HidePanel : false);
            node.SetElementValue("TotalBitrate", StartupOptions.TotalBitrate);
            node.SetElementValue("ViewTour", StartupOptions.GroupPatrol);
            node.SetElementValue("View", StartupOptions.DeviceGroup);

            return node;
        }


        private XElement ParseTitleBarInformationsToXml()
        {
            //<TitleBarInformations>
            //  <Information seq="1">ChannelNumber</Information>
            //  <Information seq="2">CameraName</Information>
            //  <Information seq="3">FPS</Information>
            //  <Information seq="4">Compression</Information>
            //  <Information seq="5">Resolution</Information>
            //  <Information seq="6">Bitrate</Information>
            //  <Information  seq="7">Datetime</Information>
            //  <FontFamily>Arial</FontFamily>
            //  <FontSize>8</FontSize>
            //  <FontColor>#FFFFFF</FontColor>
            //  <BackgroundColor>#0000FF</BackgroundColor>
            //</TitleBarInformations>

            var node = new XElement("TitleBarInformations");

            var seq = 1;
            foreach (VideoWindowTitleBarInformation information in VideoWindowTitleBarInformations)
            {
                var infoNode = new XElement("Information", VideoWindowTitleBarInformationFormats.ToString(information));
                infoNode.SetAttributeValue("seq", seq);
                node.Add(infoNode);
                seq++;
            }

            node.Add(new XElement("FontFamily", VideoTitleBarFontFamify));
            node.Add(new XElement("FontSize", VideoTitleBarFontSize));
            node.Add(new XElement("FontColor", VideoTitleBarFontColor));
            node.Add(new XElement("BackgroundColor", VideoTitleBarBackgroundColor));

            return node;
        }
        private XElement ParseWatermarkInformationsToXml()
        {
            //<TitleBarInformations>
            //  <Information seq="1">ChannelNumber</Information>
            //  <Information seq="2">CameraName</Information>
            //  <Information seq="3">FPS</Information>
            //  <Information seq="4">Compression</Information>
            //  <Information seq="5">Resolution</Information>
            //  <Information seq="6">Bitrate</Information>
            //  <Information  seq="7">Datetime</Information>
            //  <FontFamily>Arial</FontFamily>
            //  <FontSize>8</FontSize>
            //  <FontColor>#FFFFFF</FontColor>
            //  <BackgroundColor>#0000FF</BackgroundColor>
            //</TitleBarInformations>
            var node = new XElement("WatermarkInformations");


            node.Add(new XElement("FontFamily", WatermarkFontFamify));
            node.Add(new XElement("FontSize", WatermarkFontSize));
            node.Add(new XElement("FontColor", WatermarkFontColor));
            node.Add(new XElement("Text", WatermarkText));

            return node;
        }
    }
}
