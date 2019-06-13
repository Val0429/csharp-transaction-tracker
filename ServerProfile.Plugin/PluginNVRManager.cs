using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Constant;
using Interface;
//using PosBase;

namespace ServerProfile.Plugin
{
	public class PluginNVRManager : NVRManager
	{
		//private readonly POS _pos = new POS();

		protected override void LoadNVR()
		{
			NVRs.Clear();

			_loadCMSFlag = false;
			//<CMS>
			//<NVR id="1" name="gaga">
			//    <Domain>172.16.1.99</Domain>
			//    <Port>88</Port>
			//    <Account>2c5ZjyTLEx0=</Account>
			//    <Password>vNIHQ8oOrg0=</Password>
			//</NVR>
			//</CMS>

			//XmlDocument xmlDoc = Xml.LoadXmlFromHttpWithGzip(CgiLoadCMS, Server.Credential);
			//XmlDocument xmlDoc = _pos.LoadNVRFromDatabase();

			//if (xmlDoc != null)
			//{
			//    XmlNodeList nvrNodes = xmlDoc.GetElementsByTagName("NVR");
			//    foreach (XmlElement nvrNode in nvrNodes)
			//    {
			//        INVR nvr = new NVR
			//        {
			//            Id = Convert.ToUInt16(nvrNode.GetAttribute("id")),
			//            Name = nvrNode.GetAttribute("name"),
			//            Credential = new ServerCredential
			//            {
			//                Domain = Xml.GetFirstElementsValueByTagName(nvrNode, "Domain").Trim(),
			//                Port = Convert.ToUInt16(Xml.GetFirstElementsValueByTagName(nvrNode, "Port")),
			//                UserName = Xml.GetFirstElementsValueByTagName(nvrNode, "Account"),
			//                Password = Xml.GetFirstElementsValueByTagName(nvrNode, "Password"),
			//            },
			//            Form = Server.Form,
			//        };

			//        if (NVRs.ContainsKey(nvr.Id)) continue;

			//        NVRs.Add(nvr.Id, nvr);
			//    }
			//}
			//if (Server is ICMS)
			//    LoadMap();

			_loadCMSFlag = true;
		}

		protected override void SaveNVR()
		{
			_saveNVRFlag = false;

			XmlDocument doc = ParseNVRToXml();

			//if (doc != null)
			//    _pos.SaveNVRToDatabase(doc);

			_saveNVRFlag = true;
		}

		protected override XmlDocument ParseNVRToXml()
		{
			XmlDocument xmlDoc = new XmlDocument();

			XmlElement xmlRoot = xmlDoc.CreateElement("CMS");
			xmlDoc.AppendChild(xmlRoot);

			List<INVR> nvrSortResult = new List<INVR>(NVRs.Values);
			nvrSortResult.Sort((x, y) => (x.Id - y.Id));

			foreach (INVR nvr in nvrSortResult)
			{
				XmlElement nvrNode = xmlDoc.CreateElement("NVR");
				nvrNode.SetAttribute("id", nvr.Id.ToString());
				nvrNode.SetAttribute("name", nvr.Name);

				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Domain", nvr.Credential.Domain));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Port", nvr.Credential.Port.ToString()));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Account", nvr.Credential.UserName));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Password", nvr.Credential.Password));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsListenEvent", nvr.IsListenEvent ? "true" : "false"));
				nvrNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsPatrolInclude", nvr.IsPatrolInclude ? "true" : "false"));

				xmlRoot.AppendChild(nvrNode);
			}

			return xmlDoc;
		}
	}
}
