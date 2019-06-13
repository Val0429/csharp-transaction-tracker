using System;

namespace Constant
{
    public class Ethernet
    {
        public ManagerReadyState ReadyStatus { get; set; }
        public Boolean DeviceExist { get; set; }
        public Boolean DynamicIP { get; set; }
        public String IPAddress { get; set; }
        public String Mask { get; set; }
        public String MACAddress { get; set; }
        public Boolean DeviceCarrier { get; set; }
        public String Gateway { get; set; }
        public UInt64 LastModify { get; set; }
    }

    public class DNS
    {
        public String Hostname { get; set; }
        public Boolean DynamicDNS { get; set; }
        public String PrimaryDNS { get; set; }
        public String SecondDNS { get; set; }
    }
}
