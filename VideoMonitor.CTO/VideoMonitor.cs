using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Constant;
using Interface;
using VM = VideoMonitor;

namespace VideoMonitor.CTO
{
    public partial class VideoMonitor : VM.VideoMonitor
    {
        public const String SettingFile = "VideoMonitor.Config.xml";

        public Dictionary<String, String> Settings = new Dictionary<string, string>();

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
            foreach (XmlNode childNode in root.ChildNodes)
            {
                Settings[childNode.Name] = childNode.InnerText;
            }
        }

        public VideoMonitor()
        {
            InitializeComponent();

            LoadCustSetting();

            toolPanel.Controls.Remove(popupPanel);
        }

        
        public void SetCustProperty()
        {
            // Hide the layout pager in C3 Colima Project
            if (Settings["LayoutPager"] != "1")
            {
                pagerPanel.Visible = false;
                pagerPanelButton.Visible = false;
            }

            // Hide ToolMenu in C3 Colima Project
            if (Settings["ToolMenu"] != "1")
            {
                ToolMenu.GeneratorInterrogationToolMenu();
            }

            // Default Layout 1 Layout in C3 Colima Project
            if (Settings["DefaultLayout"] != "")
            {
                var count = Convert.ToUInt16(Settings["DefaultLayout"]);
                var layout = WindowLayouts.LayoutGenerate(count);

                SetLayout(layout);
            }

            
        }


        private DateTime? _startRecordTime = null;
        private DateTime? _stopRecordTime = null ;

        public void StartInterrogationRecord(Object sender, EventArgs<String, DateTime, String> e )
        {
            if (ActivateVideoWindow == null || !ActivateVideoWindow.Visible || !ActivateVideoWindow.Viewer.Visible) return;
            if (ActivateVideoWindow.Camera == null) return;

            var cam = ActivateVideoWindow.Camera;
            cam.ManualRecord("86400");

            _startRecordTime = DateTime.Now;

            var timecode = DateTimes.ToUtcString(e.Value2, Server.Server.TimeZone);
            var xml = String.Format("<Data><Name>{0}</Name><DateTime>{1}</DateTime><NoOfRecord>{2}</NoOfRecord></Data>", e.Value1, timecode, e.Value3); 

            cam.UserDefine(1, xml);
        }

        public void StopInterrogationRecord(Object sender, EventArgs<String, DateTime, String> e )
        {
            if (_startRecordTime != null)
            {
                _stopRecordTime = DateTime.Now;

                var cam = ActivateVideoWindow.Camera;
                cam.ManualRecord("0");

                var timecode = DateTimes.ToUtcString(e.Value2, Server.Server.TimeZone);
                var xml = String.Format("<Data><Name>{0}</Name><DateTime>{1}</DateTime><NoOfRecord>{2}</NoOfRecord></Data>", e.Value1, timecode, e.Value3);

                //cam.UserDefine(1, xml);

                App.ExportVideoWithInfo(new IDevice[] {cam}, (DateTime)_startRecordTime, (DateTime)_stopRecordTime, xml);

                _startRecordTime = null;
                _stopRecordTime = null;
            }
        }


        public override void AppendDevice(IDevice device)
        {
            ClearAll();
            base.AppendDevice(device);
        }

        public override void ShowGroup(IDeviceGroup group)
        {
            return;
        }

    }
}
