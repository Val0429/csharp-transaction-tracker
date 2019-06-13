using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Constant;

namespace ServerProfile.Plugin 
{
	public class ServerManager : ServerProfile.ServerManager
	{
		protected override void LoadCapability()
		{
			base.LoadCapability();

			//if (!PageList.ContainsKey("SD Playback"))
			//{
			//    var doc = new XmlDocument();
			//    doc.LoadXml("<XML><Pages><Page><Name>SD Playback</Name><Config>sdplayback.xml</Config></Page></Pages></XML>");
			//    var pagesNode = doc.SelectSingleNode("//Pages");
			//    var pages = ((XmlElement)pagesNode).GetElementsByTagName("Page");

			//    List<KeyValuePair<string, XmlElement>> list = PageList.ToList();

			//    foreach (XmlElement node in pages)
			//    {
			//        var pageName = Xml.GetFirstElementValueByTagName(node, "Name");
			//        list.Insert(2, new KeyValuePair<string, XmlElement>(pageName, node));
			//    }

			//    PageList.Clear();

			//    foreach (KeyValuePair<string, XmlElement> keyValuePair in list)
			//    {
			//        PageList.Add(keyValuePair.Key, keyValuePair.Value);
			//    }
			//}
		}
	}
}
