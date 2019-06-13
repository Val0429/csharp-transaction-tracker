using System.Xml.Serialization;

namespace Constant
{
    public enum PayloadType : ushort
    {
        Preserve = 1, NVR = 2, Camera = 3, CameraEvent = 4, CameraState = 5, GPS = 6, OBD = 7, Vehicle = 8, DigitalInput = 9, SmartCare = 10
    }

    [XmlRoot("XML", IsNullable = false)]
    public class UserDefinedMsg
    {
        [XmlElement(ElementName = "DeviceID")]
        public ushort DeviceId { get; set; }

        [XmlElement(ElementName = "Status")]
        public Message Msg { get; set; }


        public class Message
        {
            [XmlAttribute("id")]
            public ushort EventId { get; set; }
            [XmlText]
            public string Value { get; set; }
            [XmlIgnore]
            public PayloadType Type
            {
                get { return (PayloadType)EventId; }
                set { EventId = (ushort)value; }
            }
        }
    }
}