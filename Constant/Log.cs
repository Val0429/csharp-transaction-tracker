using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Constant.Utility;

namespace Constant
{
    public class Log
    {
        // Constructor
        public Log()
        {

        }


        // Properties
        /// <summary>
        /// Second
        /// </summary>
        [XmlElement("Time")]
        public String Timestamp
        {
            get { return DateTime.ToUtcTimestamp(DateTimeUnit.Second).ToString(CultureInfo.InvariantCulture); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime = ConvertUtility.ToUInt64(value).ToLocalTime(DateTimeUnit.Second);
                }
            }
        }

        [XmlIgnore]
        public DateTime DateTime { get; set; }

        public String User { get; set; }
        public LogType Type { get; set; }

        [XmlElement("Desc")]
        public String Description { get; set; }

        [XmlIgnore]
        public Boolean FullDescription { get; set; }


        // Methods
        public static void Write(String log, Boolean withTimeStamp = true, String file = "log.txt")
        {
            if (!Debugger.IsAttached) return;

            try
            {
                StreamWriter streamWriter = File.AppendText(file);
                streamWriter.WriteLine(log.PadRight(100, ' ') +
                                       ((withTimeStamp) ? DateTime.Now.ToString("MM-dd HH:mm:ss.fff") : ""));
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
        }
    }

    public enum LogType : ushort
    {
        Action = 1,
        Server = 2,
        /// <summary>
        /// The log is wriiten by client.
        /// </summary>
        Operation = 3,
    }
}