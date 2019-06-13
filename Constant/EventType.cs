using System.Xml.Serialization;

namespace Constant
{
    public enum EventType : ushort
    {
        [XmlEnum(Name = "Motion")]
        Motion = 0, //1~N
        [XmlEnum(Name = "DigitalInput")]
        DigitalInput = 1,//1~N  , On~Off
        [XmlEnum(Name = "DigitalOutput")]
        DigitalOutput = 2,//1~N  , On~Off
        /// <summary>
        /// Raising on IP camera network is disconnected
        /// </summary>
        [XmlEnum(Name = "NetworkLoss")]
        NetworkLoss = 3,
        /// <summary>
        /// Raising on IP camera network recover 
        /// </summary>
        [XmlEnum(Name = "NetworkRecovery")]
        NetworkRecovery = 4,
        /// <summary>
        /// Raising on the video server cable disconnected
        /// </summary>
        [XmlEnum(Name = "VideoLoss")]
        VideoLoss = 5,
        [XmlEnum(Name = "VideoRecovery")]
        VideoRecovery = 6,
        [XmlEnum(Name = "RecordFailed")]
        RecordFailed = 7,
        [XmlEnum(Name = "RecordRecovery")]
        RecordRecovery = 8,
        [XmlEnum(Name = "UserDefine")]
        UserDefine = 9,
        [XmlEnum(Name = "ManualRecord")]
        ManualRecord = 10,
        RAIDDegraded = 11,
        RAIDInactive = 12,
        IVS = 13,
        CrossLine = 14,

        //failover
        NVRFail = 15,
        FailoverStartRecord = 16,
        FailoverDataStartSync = 17,
        FailoverSyncCompleted = 18,

        //enhancement
        [XmlEnum(Name = "Panic")]
        Panic = 19,
        AudioIn = 20,
        AudioOut = 21,
        VideoRecord = 22,
        PlaybackDownload = 23,

        LPR_BlackList = 24,
        LPR_WhiteList = 25,
        LPR_Employee = 26,
        LPR_Unregistered = 27,
        LPR_VIP = 28,
        [XmlEnum(Name = "IntrusionDetection")]
        IntrusionDetection = 29,
        [XmlEnum(Name = "LoiteringDetection")]
        LoiteringDetection = 30,
        [XmlEnum(Name = "ObjectCountingIn")]
        ObjectCountingIn = 31,
        [XmlEnum(Name = "ObjectCountingOut")]
        ObjectCountingOut = 32,
        SDRecord = 33,

        [XmlEnum(Name = "AudioDetection")]
        AudioDetection = 34,
        [XmlEnum(Name = "TamperDetection")]
        TamperDetection = 35,

        CardPermitted = 36,
        CardDenied = 37,
        /// <summary>
        /// Release button / Touch sensor open
        /// </summary>
        TouchOpen = 38,
        DoorHoldOpen = 39,
        DoorForceOpen = 40,
        /// <summary>
        /// Valid card, but no privilege to access the door
        /// </summary>
        NoPrivilege = 42,
        WrongPIN = 43,
        PINTooLate = 44,
        InputOn = 45,
        InputOff = 46,

        IVS_CarParking = 47,

        Undefined = 65535,

        ArchiveServerRecord = 41,

        TakeOff = 48,
        StartMission = 49,
        GoHome = 50,
        Land = 51,
        Stabilize = 52,

        // for BNP Project
        GardenLeave = 53,
        //Please add type to the end, do not add type into the middle of list to cause device pack error!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }
}
