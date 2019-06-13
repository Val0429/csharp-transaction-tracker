using System;
using System.Xml;
using Constant;

namespace ServerProfile.Plugin
{
    public class ConfigureManager : ServerProfile.ConfigureManager
    {
        public ConfigureManager()
        {
            LocalStreamSetting = new CustomStreamConfig();
            RemoteStreamSetting = new RemoteStreamConfig();

            //AutoLoadViewID = 0;
        }

        protected override void ParserGeneralSetting(XmlDocument xmlDoc)
        {
            base.ParserGeneralSetting(xmlDoc);

            //if (xmlDoc != null)
            //{
            //    XmlNode node = xmlDoc.SelectSingleNode("Config");

            //    if (node != null)
            //    {
            //        String autoLoadView = Xml.GetFirstElementValueByTagName(node, "AutoLoadView");
            //        if (autoLoadView != "")
            //            AutoLoadViewID = Convert.ToInt16(autoLoadView);
            //    }
            //}
        }
    }
}
