using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Constant;
using Interface;
using EVF = ExportVideoForm;

namespace ExportVideoForm.CTO
{
    public partial class ExportVideo : EVF.ExportVideo
    {
        public const String SettingFile = "ExportVideoForm.Config.xml";

        public Dictionary<String, String> Settings = new Dictionary<string, string>();

        public ExportVideo()
        {
            InitializeComponent();

            LoadCustSetting();
        }

        public void LoadCustSetting()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (path == null) return;

            var file = Path.Combine(path, SettingFile);
            if (!File.Exists(file)) return;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(file);

            var root = xmlDoc.DocumentElement;

            Settings.Clear();
            if (root != null)
                foreach (XmlNode childNode in root.ChildNodes)
                {
                    Settings[childNode.Name] = childNode.InnerText;
                }
        }

        //protected override void ExportButtonClick(Object sender, EventArgs e)
        //{
        //    base.ExportButtonClick(sender, e);
        //}

        protected override Boolean CheckPath(ICamera camera, String defaultExportPath = "")
        {
            //<Data>
            //    <Action>STOP</Action>
            //    <Name>{0}</Name>
            //    <DateTime>{1}</DateTime>
            //    <NoOfRecord>{2}</NoOfRecord>
            //</Data>

            XmlDocument xmlDoc = Xml.LoadXml(ExportInformation);
            String name = Xml.GetFirstElementValueByTagName(xmlDoc, "Name");
            UInt64 timecode = Convert.ToUInt64(Xml.GetFirstElementValueByTagName(xmlDoc, "DateTime"));
            DateTime dateTime = DateTimes.ToDateTime(timecode, Server.Server.TimeZone);
            String noOfRecord = Xml.GetFirstElementValueByTagName(xmlDoc, "NoOfRecord");

            var path = String.Format("\\{0}-{1}-{2}", noOfRecord, name, dateTime.ToString("yyyy-MM-dd-HH-mm-ss"));
            return base.CheckPath(camera, path);
        }

        protected override void ExportVideoComplete()
        {
            if (Settings.ContainsKey("AfterExportVideoComplete") && Settings["AfterExportVideoComplete"] == "1")
                AfterExportVideoComplete();

            base.ExportVideoComplete();
        }

        private void AfterExportVideoComplete()
        {
            //<Data>
            //    <Action>STOP</Action>
            //    <Name>{0}</Name>
            //    <DateTime>{1}</DateTime>
            //    <NoOfRecord>{2}</NoOfRecord>
            //</Data>

            XmlDocument xmlDoc = Xml.LoadXml(ExportInformation);
            String name = Xml.GetFirstElementValueByTagName(xmlDoc, "Name");
            UInt64 timecode = Convert.ToUInt64(Xml.GetFirstElementValueByTagName(xmlDoc, "DateTime"));
            DateTime dateTime = DateTimes.ToDateTime(timecode, Server.Server.TimeZone);
            String noOfRecord = Xml.GetFirstElementValueByTagName(xmlDoc, "NoOfRecord");

            //ExportPath
            foreach (KeyValuePair<ICamera, string> keyValuePair in ExportFileName)
            {
                var src = Path.Combine(ExportPath, keyValuePair.Value);
                var fmt = String.Format("{0}-{1}-{2}.avi", noOfRecord, name, dateTime.ToString("yyyy-MM-dd-HH-mm-ss"));
                fmt = Path.Combine(ExportPath, fmt);

                File.Copy(src, fmt);
                File.Delete(src);
            }
        }
    }
}
