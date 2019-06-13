
using System;

namespace Constant
{
    public class PeopleCountingSetting
    {
        public UInt16 FeatureThreshold = 80;//1~255
        public UInt16 FeatureNumberThreshold = 0;//0~n
        public UInt16 PersonNumber = 0;
        public UInt16 DirectNumber = 70;
        public UInt16 FrameIndex = 5; //0~10

        public UInt16 Retry = 5; //0~65535
        public UInt16 Interval = 30; //1~60 sec
    }
}