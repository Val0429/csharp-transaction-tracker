using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Constant
{
    public class StartupOptionDeviceGroup
    {
        public String Layout{ get; set; }
        public String Items{ get; set; }
    }

    public class StartupOptions
    {
        public const String SettingFile = "client.ini";
        private static String _path;
        public static String SettingFilePath()
        {
            if (String.IsNullOrEmpty(_path))
            {
                _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            return _path;
        }

        public String Doamin { get; set; }
        public String Port { get; set; }
        public String Account { get; set; }
        public String Password { get; set; }
        public String Language { get; set; }
        public String Mode { get; set; }//Kevin ADLogin

        public String ADDomain { get; set; }//Kevin ADLogin
        public Boolean SSLEnable { get; set; }

        public Boolean Enabled;
        public String PageName;

        public Boolean VideoTitleBar;
        public Boolean FullScreen;
        public Boolean HidePanel;
        public Int16 TotalBitrate;
        /// <summary>
        /// Device View Patrol
        /// </summary>
        public Boolean GroupPatrol { get; set; }
        /// <summary>
        /// Device List Patrol
        /// </summary>
        public Boolean DevicePatrol { get; set; }
        public UInt16 TourItem;

        public String DeviceGroup;
        public String Layout;
        public String Items;
        public List<StartupOptionDeviceGroup> PopupWindows;
        public List<Int16> StreamProfileId;

        public UInt64 TimeCode;
        public float PlaySpeed;
        public Int16 PlayDirection;
        public Boolean Loading;
        public Boolean LogoutProcess;

        public StartupOptions()
        {
            Doamin = "";
            Port = "";
            Account = "";
            Password = "";
            Language = "";
            ADDomain = "";//Kevin ADLogin
            Mode = "";//Kevin ADLogin
            SSLEnable = false;

            Enabled = false;
            PageName = "Live";
            VideoTitleBar = false;
            FullScreen = false;
            HidePanel = false;
            TotalBitrate = -1;
            GroupPatrol = false;
            DevicePatrol = false;
            TourItem = 0;
            DeviceGroup = "";
            Layout = "";
            Items = "";
            PopupWindows = new List<StartupOptionDeviceGroup>();
            StreamProfileId = new List<short>();
            TimeCode = 0;
            PlaySpeed = 1;
            PlayDirection = 1;
            Loading = true;
            LogoutProcess = false;
        }

        public StartupOptions Clone()
        {
            return MemberwiseClone() as StartupOptions;
        }

        public StartupOptions LoadSetting()
        {
            var file = Path.Combine(SettingFilePath(), SettingFile);
            if (!File.Exists(file)) return null;

            string code = File.ReadAllText(file);
            if (String.IsNullOrEmpty(code)) return null;

            var text = Encryptions.DecryptDES(code);

            var xmlDoc = new XmlDocument();
            
            try
            {
                xmlDoc.LoadXml(text);
            }
            catch (Exception)
            {
                return null;
            }

            var nvrStillAlive = false;
            var processIdStr = Xml.GetFirstElementValueByTagName(xmlDoc, "ProcessId");

            if (!String.IsNullOrEmpty(processIdStr))
            {
                //if process id still exist -> NVR still alive -> dont need to load it.
                var processId = Convert.ToInt32(processIdStr);
                nvrStillAlive = Process.GetProcesses().Any(x => x.Id == processId);
            }

            if (nvrStillAlive)
            {
                return null;
            }

            //String enabled = "True" ;
            String fullScreen = Xml.GetFirstElementValueByTagName(xmlDoc, "FullScreen");
            String hidePanel = Xml.GetFirstElementValueByTagName(xmlDoc, "HidePanel");
            String totalBitrate = Xml.GetFirstElementValueByTagName(xmlDoc, "TotalBitrate");
            String groupPatrol = Xml.GetFirstElementValueByTagName(xmlDoc, "GroupPatrol");
            String devicePatrol = Xml.GetFirstElementValueByTagName(xmlDoc, "DevicePatrol");
            String tourItem = Xml.GetFirstElementValueByTagName(xmlDoc, "TourItem");
            if (tourItem == "-1") tourItem = "0";
            String items = Xml.GetFirstElementValueByTagName(xmlDoc, "Items");
            String streamProfileId = Xml.GetFirstElementValueByTagName(xmlDoc, "StreamProfileId");
            String timeCode = Xml.GetFirstElementValueByTagName(xmlDoc, "TimeCode");

            var popupWindows = xmlDoc.SelectNodes("/Client/PopupWindows/PopupWindow");
            if (popupWindows != null)
                foreach (XmlElement popupWindow in popupWindows)
                {
                    var newOptionsPopupWindow = new StartupOptionDeviceGroup
                    {
                        Layout = Xml.GetFirstElementValueByTagName(popupWindow, "Layout"),
                        Items = Xml.GetFirstElementValueByTagName(popupWindow, "Items")
                    };
                    PopupWindows.Add(newOptionsPopupWindow);
                }
            //var listItem = new List<Int16>();
            //if (items != "")
            //{
            //    var a = items.Split(',');
            //    listItem.AddRange(from s in a where !String.IsNullOrEmpty(s) select Convert.ToInt16(s));
            //}

            var listProfile = new List<Int16>();
            if (streamProfileId != "")
            {
                var a = streamProfileId.Split(',');
                listProfile.AddRange(from s in a where !String.IsNullOrEmpty(s) select Convert.ToInt16(s));
            }

            Enabled = true;
            PageName = Xml.GetFirstElementValueByTagName(xmlDoc, "PageName");
            FullScreen = (fullScreen == "true");
            HidePanel = (hidePanel == "true");
            TotalBitrate = (String.IsNullOrEmpty(totalBitrate)) ? (Int16)(-1) : Convert.ToInt16(totalBitrate);
            GroupPatrol = (groupPatrol == "true");
            DevicePatrol = (devicePatrol == "true");
            TourItem = (String.IsNullOrEmpty(tourItem)) ? (UInt16)(0) : Convert.ToUInt16(tourItem);
            DeviceGroup = Xml.GetFirstElementValueByTagName(xmlDoc, "DeviceGroup");
            Layout = Xml.GetFirstElementValueByTagName(xmlDoc, "Layout");
            TimeCode = (String.IsNullOrEmpty(timeCode)) ? 0 : Convert.ToUInt64(timeCode);
            Items = items;
            StreamProfileId = listProfile;

            return this;
        }


        private String _processId;

        public void SaveSetting()
        {
            if (LogoutProcess) return;

            if (String.IsNullOrEmpty(_processId))
                _processId = Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture);

            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("Client");
            xmlDoc.AppendChild(xmlRoot);

            var strProfiles = "";
            if (StreamProfileId != null)
            {
                strProfiles = StreamProfileId.Aggregate(strProfiles, (current, profileId) => current + ("," + profileId.ToString(CultureInfo.InvariantCulture)));
                if (strProfiles.Length >= 2) strProfiles = strProfiles.Substring(1);
            }

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ProcessId", _processId));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Doamin", Doamin));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", Port));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Mode", Mode));//Kevin ADLogin
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ADDomain", ADDomain));//Kevin ADLogin
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Account", Account));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Password", Password));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Language", Language));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "SSLEnable", (SSLEnable ? "true" : "false")));

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Enabled", (Enabled ? "true" : "false")));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "PageName", PageName));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "FullScreen", (FullScreen ? "true" : "false")));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "HidePanel", (HidePanel ? "true" : "false")));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TotalBitrate", TotalBitrate.ToString(CultureInfo.InvariantCulture)));

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "GroupPatrol", (GroupPatrol ? "true" : "false")));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DevicePatrol", (DevicePatrol ? "true" : "false")));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TourItem", TourItem.ToString(CultureInfo.InvariantCulture)));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DeviceGroup", DeviceGroup));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Layout", Layout));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", Items));

            var popupWindows = Xml.CreateXmlElementWithText(xmlDoc, "PopupWindows", "");
            foreach (StartupOptionDeviceGroup popupWindow in PopupWindows)
            {
                var popupWindowNode = Xml.CreateXmlElementWithText(xmlDoc, "PopupWindow", "");
                popupWindowNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Layout", popupWindow.Layout));
                popupWindowNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", popupWindow.Items));
                popupWindows.AppendChild(popupWindowNode);
            }
            xmlRoot.AppendChild(popupWindows);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StreamProfileId", strProfiles));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TimeCode", TimeCode.ToString(CultureInfo.InvariantCulture)));

            var code = Encryptions.EncryptDES(xmlRoot.OuterXml);

            try
            {
                File.WriteAllText(Path.Combine(SettingFilePath(), SettingFile), code);
            }
            catch
            {
            }
        }

        public void ClearSetting()
        {
            try
            {
                File.Delete(Path.Combine(SettingFilePath(), SettingFile));
            }
            catch
            {
            }
        }
    }
}
