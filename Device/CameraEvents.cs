
using DeviceConstant;
using Interface;

namespace Device
{
    public class CameraEvents : CameraEvent, ICameraEvent
    {
        public CameraEvents()
        {
        }


        public IDevice Device{ get; set; }
        public INVR NVR { get; set; }
    }
}