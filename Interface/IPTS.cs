using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Interface
{
    public interface IPOS : IDeviceGroup
    {
        String Id { get; set; }
        String Manufacture { get; set; }
        String Model { get; set; }
        UInt16 LicenseId { get; set; }
        UInt16 Exception { get; set; } //id link to exception file
        UInt16 Keyword { get; set; } //id link to exception file

        String Name { get; set; }
        IServer Server { get; set; }
        /// <summary>
        /// The device entity
        /// </summary>
        List<IDevice> Items { get; set; }//define as Object, can set value as null, better then define as Int16
        List<XmlElement> Regions { get; set; }
        List<Int16> MountType { get; set; }
        List<Boolean> DewarpEnable { get; set; }
        /// <summary>
        /// The sequence of the view. Each item of View must be in IDeviceGroup.Items or null.
        /// </summary>
        List<IDevice> View { get; set; }
        List<WindowLayout> Layout { get; set; }
        Boolean IsExpand { get; set; }
        Boolean IsCapture { get; set; }
        ReadyState ReadyState { get; set; }
        ExceptionReports ExceptionReports { get; }
    }

    public interface IPTS : IServer
    {
        event EventHandler<EventArgs<List<ICameraEvent>>> OnEventReceive;
        event EventHandler<EventArgs<POS_Exception.TransactionItem>> OnPOSEventReceive;
        event EventHandler<EventArgs<List<ICamera>>> OnCameraStatusReceive;
        event EventHandler<EventArgs<INVR>> OnNVRModify;
        event EventHandler<EventArgs<IPOS>> OnPOSModify;

        INVRManager NVR { get; }
        IPOSManager POS { get; }

        void ListenNVREvent(INVR nvr);
        void ListenPOSEvent();

        void NVRModify(INVR nvr);
        void POSModify(IPOS pos);
        INVR CreateNewNVR();

        String ReleaseBrand { get; }
    }

    public interface IPOSConnection
    {
        String Manufacture { get; set; }
        String Model { get; set; }
        UInt16 Id { get; set; }
        String Name { get; set; }
        String Protocol { get; set; }
        String QueueInfo { get; set; }
        String ConnectInfo { get; set; }
        UInt16 AcceptPort { get; set; }
        UInt16 ConnectPort { get; set; }
        Authentication Authentication { get; set; }
        List<IPOS> POS { get; set; }
        Boolean IsCapture { get; set; }
        ReadyState ReadyState { get; set; }

        String ProtocolValue(String manufacture);
        void SetDefaultAuthentication();
    }


    public interface IDivision
    {
        UInt16 Id { get; set; }
        String Name { get; set; }
        ReadyState ReadyState { get; set; }
        Dictionary<UInt16, IRegion> Regions { get; set; }
    }

    public class Division : IDivision
    {
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public ReadyState ReadyState { get; set; }
        public Dictionary<UInt16, IRegion> Regions { get; set; }

        public Division()
        {
            ReadyState = ReadyState.Ready;
            Regions = new Dictionary<UInt16, IRegion>();
        }

        public override String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + " " + Name;
        }
    }

    public interface IRegion
    {
        UInt16 Id { get; set; }
        String Name { get; set; }
        ReadyState ReadyState { get; set; }

        Dictionary<UInt16, IStore> Stores { get; set; }
    }

    public class Region : IRegion
    {
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public ReadyState ReadyState { get; set; }
        public Dictionary<UInt16, IStore> Stores { get; set; }

        public Region()
        {
            ReadyState = ReadyState.Ready;
            Stores = new Dictionary<UInt16, IStore>();
        }

        public override String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + " " + Name;
        }
    }

    public interface IStore
    {
        UInt16 Id { get; set; }
        String Name { get; set; }
        ReadyState ReadyState { get; set; }

        Dictionary<String, IPOS> Pos { get; set; }
    }

    public class PTSStore : IStore
    {
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public ReadyState ReadyState { get; set; }
        public Dictionary<String, IPOS> Pos { get; set; }

        public PTSStore()
        {
            ReadyState = ReadyState.Ready;
            Pos = new Dictionary<String, IPOS>();
        }

        public override String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + " " + Name;
        }
    }
}