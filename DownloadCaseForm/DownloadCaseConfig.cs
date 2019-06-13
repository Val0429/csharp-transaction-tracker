using System;
using System.Collections.Generic;
using System.Xml;
using Interface;

namespace DownloadCaseForm
{
    public class DownloadCaseConfig
    {
        public Int32 Id;
        public QueueStatus Status = QueueStatus.Stop;
        public Queue<ICamera> ExportDevices;
        public DateTime StartDateTime;
        public DateTime EndDateTime;
        public String DownloadPath;
        public Boolean AudioIn;
        public Boolean AudioOut;
        public Boolean EncodeAVI;
        public Boolean OverlayOSD;
        public XmlDocument AttachXmlDoc;
        public String Comments;

        public DownloadCaseConfig()
        {
            ExportDevices = new Queue<ICamera>();
        }
    }

    public enum QueueStatus : ushort
    {
        Downloading,
        Completed,
        Failed,
        Queue,
        Stop,
    }
}
