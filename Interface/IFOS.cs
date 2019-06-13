
using DeviceConstant;

namespace Interface
{
    public interface IFOS : IServer
    {
        INVRManager NVR { get; }
        EventHandling EventHandling { get; set; }
    }
}