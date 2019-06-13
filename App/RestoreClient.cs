using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Constant;
using Interface;

namespace App
{
	public class RestoreClient : IRestoreClient
	{
		private const String SettingFile = "client.ini";

		public Boolean Enabled { get; set; }
		public String PageName { get; set; }

		public Boolean FullScreen { get; set; }
		public Boolean HideToolbar { get; set; }
		public Boolean HidePanel { get; set; }
		public Int16 TotalBitrate { get; set; }

		public Int16 ViewTour { get; set; }
		public Int16 TourItem { get; set; }

		public Int16 DeviceGroup { get; set; }
		public String Layout { get; set; }
		public List<Int16> Items { get; set; }
		public List<Int16> StreamProfileId { get; set; }

		public UInt64 TimeCode { get; set; }
		public float PlaySpeed{ get; set; }
		public Int16 PlayDirection { get; set; }

		public RestoreClient()
		{
			Enabled = false;
			PageName = "Live";
			FullScreen = false;
			HideToolbar = false;
			HidePanel = false;
			TotalBitrate = -1;
			ViewTour = -1;
			TourItem = -1;
			DeviceGroup = -1;
			Layout = "";
			Items = new List<short>();
			StreamProfileId = new List<short>();
			TimeCode = 0;
			PlaySpeed = 1;
			PlayDirection = 1;
		}

		public IRestoreClient LoadSetting()
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var file = Path.Combine(path, SettingFile);

			string code = "";
			if (File.Exists(file))
				code = File.ReadAllText(file);
			else
				return null;

			if (code == "")
				return null;
			else
			{
				var text = Encryptions.DecryptDES(code);

				var xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(text);

				String enabled = Xml.GetFirstElementValueByTagName(xmlDoc, "Enabled");
				String fullScreen = Xml.GetFirstElementValueByTagName(xmlDoc, "FullScreen");
				String hideToolbar = Xml.GetFirstElementValueByTagName(xmlDoc, "HideToolbar");
				String hidePanel = Xml.GetFirstElementValueByTagName(xmlDoc, "HidePanel");
				String totalBitrate = Xml.GetFirstElementValueByTagName(xmlDoc, "TotalBitrate");
				String viewTour = Xml.GetFirstElementValueByTagName(xmlDoc, "ViewTour");
				String tourItem = Xml.GetFirstElementValueByTagName(xmlDoc, "TourItem");
				String deviceGroup = Xml.GetFirstElementValueByTagName(xmlDoc, "DeviceGroup");
				String items = Xml.GetFirstElementValueByTagName(xmlDoc, "Items");
				String streamProfileId = Xml.GetFirstElementValueByTagName(xmlDoc, "StreamProfileId");
				String timeCode = Xml.GetFirstElementValueByTagName(xmlDoc, "TimeCode");

				var listItem = new List<Int16>();
				if (items != "")
				{
					var a = items.Split(',');
					listItem.AddRange(from s in a where !String.IsNullOrEmpty(s) select Convert.ToInt16(s));
				}

				var listProfile = new List<Int16>();
				if (streamProfileId != "")
				{
					var a = streamProfileId.Split(',');
					listProfile.AddRange(from s in a where !String.IsNullOrEmpty(s) select Convert.ToInt16(s));
				}

				Enabled = (enabled == "true");
				PageName = Xml.GetFirstElementValueByTagName(xmlDoc, "PageName");
				FullScreen = (fullScreen == "true");
				HideToolbar = (hideToolbar == "true");
				HidePanel = (hidePanel == "true");
				TotalBitrate = (String.IsNullOrEmpty(totalBitrate)) ? (Int16) (-1) : Convert.ToInt16(totalBitrate);
				ViewTour = (String.IsNullOrEmpty(viewTour)) ? (Int16) (-1) : Convert.ToInt16(viewTour);
				TourItem = (String.IsNullOrEmpty(tourItem)) ? (Int16) (-1) : Convert.ToInt16(tourItem);
				DeviceGroup = (String.IsNullOrEmpty(deviceGroup)) ? (Int16) (-1) : Convert.ToInt16(deviceGroup);
				Layout = Xml.GetFirstElementValueByTagName(xmlDoc, "Layout");
				TimeCode = (String.IsNullOrEmpty(timeCode)) ? 0 : Convert.ToUInt64(timeCode);
				Items = listItem;
				StreamProfileId = listProfile;
			}

			return this;
		}

		public Boolean SaveSetting()
		{
			var xmlDoc = new XmlDocument();
			var xmlRoot = xmlDoc.CreateElement("Client");
			xmlDoc.AppendChild(xmlRoot);

			var strItems = "";
			if (Items != null)
			{
				strItems = Items.Aggregate(strItems, (current, item) => current + ("," + item.ToString()));
				if (strItems.Length >= 2) strItems = strItems.Substring(1);
			}

			var strProfiles = "";
			if (StreamProfileId != null)
			{
				strProfiles = StreamProfileId.Aggregate(strProfiles, (current, profileId) => current + ("," + profileId.ToString()));
				if (strProfiles.Length >= 2) strProfiles = strProfiles.Substring(1);
			}

			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Enabled", (Enabled ? "true" : "false")));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "PageName", PageName));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "FullScreen", (FullScreen ? "true" : "false")));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "HideToolbar", (HideToolbar ? "true" : "false")));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "HidePanel", (HidePanel ? "true" : "false")));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TotalBitrate", TotalBitrate.ToString()));

			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ViewTour", ViewTour.ToString()));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TourItem", TourItem.ToString()));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DeviceGroup", DeviceGroup.ToString()));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Layout", Layout));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", strItems));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StreamProfileId", strProfiles));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TimeCode", TimeCode.ToString()));

			var code = Encryptions.EncryptDES(xmlRoot.OuterXml);

			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var file = Path.Combine(path, SettingFile);

			File.WriteAllText(file, code);

			return true;
		}
	}
}
