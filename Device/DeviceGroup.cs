using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using Interface;

namespace Device
{
    public class DeviceGroup : IDeviceGroup
    {
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public IServer Server { get; set; }
        public List<IDevice> Items { get; set; }
        public List<IDevice> View { get; set; }
        public List<XmlElement> Regions { get; set; }
        public List<Int16> MountType { get; set; }
        public List<Boolean> DewarpEnable { get; set; }
        public List<WindowLayout> Layout { get; set; }
        public Boolean IsExpand { get; set; }
        public ReadyState ReadyState { get; set; }
        public static Boolean DisplayGroupId = true;

        public DeviceGroup()
        {
            Items = new List<IDevice>();
            View = new List<IDevice>();
            Layout = new List<WindowLayout>();
            ReadyState = ReadyState.New;
            Regions = new List<XmlElement>();
            MountType = new List<Int16>();
            DewarpEnable = new List<Boolean>();
        }

        public DeviceGroup(IServer server)
            : this()
        {
            Server = server;
        }

        public override String ToString()
        {
            if (Id != 0)
                return (DisplayGroupId) ? (Id.ToString().PadLeft(2, '0') + " " + Name) : Name;

            return Name;
        }
    }
}
