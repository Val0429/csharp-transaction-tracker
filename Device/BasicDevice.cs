using System;
using Constant;
using DeviceConstant;
using Interface;
using System.Drawing;

namespace Device
{
    public class BasicDevice : IDevice
    {
        public IServer Server { get; set; }
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public ReadyState ReadyState { get; set; }
        public DeviceType DeviceType { get; set; }

        public static Boolean DisplayDeviceId = true;

        public BasicDevice()
        {
            ReadyState = ReadyState.New;
        }

        public virtual Boolean CheckPermission(Permission permission)
        {
            return (Server != null && Server.User.Current.CheckPermission(this, permission));
        }

        public override String ToString()
        {
            return (DisplayDeviceId) ? (Id.ToString().PadLeft(2, '0') + " " + Name) : Name;
        }

        public IDevice CloneDevice()
        {
            var ret = MemberwiseClone() as IDevice;
            return ret;
        }

        public String LastPicture { get; set; }
        
    }
}
