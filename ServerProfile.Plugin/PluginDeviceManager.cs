using System.Xml;
using Constant;
using Interface;

namespace ServerProfile.Plugin
{
	public class PluginDeviceManager : DeviceManager
	{
		protected override IDevice ParseDeviceProfileFromXml(XmlElement node)
		{
			IDevice camera = base.ParseDeviceProfileFromXml(node);

			if (camera != null)
			{
				XmlNode gpsInfo = node.SelectSingleNode("DeviceSetting//GPSInfo");

				if (camera.GPSInfo == null)
					camera.GPSInfo = new GPSInfo();

				if (gpsInfo != null)
				{
					camera.GPSInfo.ModelCarrier = ModelCarriers.ToIndex(Xml.GetFirstElementsValueByTagName(gpsInfo, "CarrierType"));
					camera.GPSInfo.ServerNo = Xml.GetFirstElementsValueByTagName(gpsInfo, "ServerNo");
					camera.GPSInfo.VehicleNo = Xml.GetFirstElementsValueByTagName(gpsInfo, "VehicleNo");
					camera.GPSInfo.DriverId = Xml.GetFirstElementsValueByTagName(gpsInfo, "DriverId");
					camera.GPSInfo.DriverName = Xml.GetFirstElementsValueByTagName(gpsInfo, "DriverName");
					camera.GPSInfo.DriverMobile = Xml.GetFirstElementsValueByTagName(gpsInfo, "DriverMobile");
				}
			}

			return camera;
		}

		protected override XmlDocument ParseDeviceProfileToXml(IDevice device)
		{
			var xmlDoc = base.ParseDeviceProfileToXml(device);

			var settingNode = xmlDoc.SelectSingleNode("DeviceConnectorConfiguration//DeviceSetting");
			if (settingNode != null)
			{
				var gpsInfo = xmlDoc.CreateElement("GPSInfo");
				settingNode.AppendChild(gpsInfo);

				var camera = device as ICamera;
				gpsInfo.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "CarrierType", ModelCarriers.ToString(camera.GPSInfo.ModelCarrier)));

				gpsInfo.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "ServerNo", camera.GPSInfo.ServerNo));
				gpsInfo.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "VehicleNo", camera.GPSInfo.VehicleNo));
				gpsInfo.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DriverId", camera.GPSInfo.DriverId));
				gpsInfo.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DriverName", camera.GPSInfo.DriverName));
				gpsInfo.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DriverMobile", camera.GPSInfo.DriverMobile));
			}

			return xmlDoc;
		}
	}
}
