using System;

namespace DeviceConstant
{
    public class CaptureCardConfig
    {
        public UInt16 Brightness = 128; //(0~255)
        public UInt16 Contrast = 128; //(0~255)
        public UInt16 Hue = 128; //(0~255)
        public UInt16 Saturation = 128; //(0~255)
        public UInt16 Sharpness = 0; //(0~255)
        public UInt16 Gamma = 0; //(0~255)
        public UInt16 ColorEnable = 0; //(0~255)
        public UInt16 WhiteBalance = 0; //(0~255)
        public UInt16 BacklightCompensation = 0; //(0~255)
        public UInt16 Gain = 0; //(0~255)

        public UInt16 TemporalSensitivity = 0; //(0~15)
        public UInt16 SpatialSensitivity = 0; //(0~15)
        public UInt16 LevelSensitivity = 0; //(0~15)
        public UInt16 Speed = 0; //(0~63)
    }
}
