using System;
using Interface;

namespace Device
{
    public class SubLayout : Camera, ISubLayout
    {
        public IDeviceLayout DeviceLayout { get; set; }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public Int32 Dewarp { get; set; }

        public SubLayout()
        {
            X = 0;
            Y = 0;
            Width = 640;
            Height = 480;
            Dewarp = 0;
        }
    }
}