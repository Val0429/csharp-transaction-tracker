using System;
using System.Collections.Generic;
using Constant;

namespace DeviceConstant
{
    public class CameraModel
    {
        public String Manufacture = "UNKNOWN";
        public String Model = "UNKNOWN";
        public String Alias = "UNKNOWN";
        public String Type = "UNKNOWN";
        public String Series = "";

        public ConnectionProtocol[] ConnectionProtocol = new[] { 
            DeviceConstant.ConnectionProtocol.NonSpecific, 
        };

        public Encryption[] Encryption = new[] { 
            Constant.Encryption.Basic, 
            Constant.Encryption.Plain, 
            Constant.Encryption.Digest,
        };

        public List<CameraMode> CameraMode = new List<CameraMode>
        {
            DeviceConstant.CameraMode.Single
        };
        //----------------------------------------------
        public UInt16 NumberOfChannel;
        public UInt16 NumberOfAudioIn;
        public UInt16 NumberOfAudioOut;
        public UInt16 NumberOfDi;
        public UInt16 NumberOfDo;
        public UInt16 NumberOfMotion;
        //----------------------------------------------
        public Boolean IOPortConfigurable;
        public IOPort[] IOPortSupport;
        public Dictionary<UInt16, IOPort> IOPorts = new Dictionary<UInt16, IOPort>(); //Default
        //----------------------------------------------
        public Boolean PanSupport;
        public Boolean TiltSupport;
        public Boolean ZoomSupport;
        public Boolean FocusSupport;
        public Boolean AutoTrackingSupport;
        public List<String> LensType = new List<String>();
        public Boolean IsSupportPTZ
        {
            get { return (PanSupport || TiltSupport || ZoomSupport); }
        }
    }
}
