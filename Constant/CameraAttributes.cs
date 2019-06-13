using System;
using System.Collections.Generic;

namespace Constant
{
    public class CameraAttributes : MapNode
    {
        public UInt16 SystemId;
        public UInt16 NVRSystemId;
        public Int32 Rotate;
        public Boolean IsSpeakerEnabled;
        public Double SpeakerX;
        public Double SpeakerY;
        public Boolean IsAudioEnabled;
        public Double AudioX;
        public Double AudioY;
        public Boolean IsDefaultOpenVideoWindow;
        public Boolean IsVideoWindowOpen;
        public Double VideoWindowX;
        public Double VideoWindowY;
        public Int32 VideoWindowSize;
        public Boolean IsEventAlarm;
        public List<CameraEventRecord> EventRecords;
        public String CameraStatus; //because cross reference
    }
}
