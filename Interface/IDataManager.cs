using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Constant;
using DeviceConstant;

namespace Interface
{
    public interface IDataManager
    {
        event EventHandler OnLoadComplete;
        event EventHandler OnSaveComplete;

        ManagerReadyState ReadyStatus { get; set; }
        void Initialize();
        void Load();
        void Load(String xml);
        void Save();
        void Save(String xml);

        String Status { get; }
    }

    public interface INVRManager : IDataManager
    {
        event EventHandler OnNVRStatusUpdate;
        event EventHandler DeviceChanged;

        UInt16 MaximunNVRAmount { get; }
        FailoverStatus FailoverStatus { get; }
        UInt16 SynchronizeProgress { get; }
        Dictionary<UInt16, INVR> NVRs { get; }
        Dictionary<String, MapAttribute> Maps { get; }
        Dictionary<IDevice, UInt16> DeviceChannelTable { get; }
        List<INVR> SearchNVR(String manufacture);

        void LoadMap();
        void SaveMap(XmlDocument mapDocument);

        void UpdateFailoverDeviceList(UInt16 id, INVR nvr);
        void UpdateNVRStatus();
        void SaveNVRDocument();

        Boolean UploadMap(Bitmap map, String filename);
        Bitmap GetMap(String filename);

        UInt16 GetNewNVRId();
        INVR FindNVRById(UInt16 nvrId);
        MapAttribute FindMapById(String mapId);

        void AddDeviceChannelTable(IDevice device);
        void RemoveDeviceChannelTable(IDevice device);
        void LoadNVRDevicePresetPoint(INVR nvr);
        ServerCredential ArchiveServer { get; set; }
    }

    public interface IDeviceManager : IDataManager
    {
        Dictionary<UInt16, IDevice> Devices { get; }
        Dictionary<UInt16, IDeviceGroup> Groups { get; }
        // Add By Tulip for User Define Device Group
        Dictionary<UInt16, IDeviceGroup> UserDefineGroups { get; set; }
        Dictionary<UInt16, IDeviceLayout> DeviceLayouts { get; }
        Dictionary<String, List<CameraModel>> Manufacture { get; }

        UInt16 GetNewDeviceId();
        UInt16 GetNewGroupId();
        IDevice FindDeviceById(UInt16 deviceId);
        //List<ICamera> Search();
        List<ICamera> Search(String manufacturer);

        XmlDocument LoadAllDeviceAsXmlDocument();

        void SavePresetTour(ICamera camera);
        void SetDeviceCapabilityByModel(ICamera camera);
        void CheckSubLayoutPositionAndResolution(IDevice device);
        void CheckSubLayoutRange(IDeviceLayout deviceLayout);
        void LoadAllEvent();
        String ReadFisheyeVendorByCamera(ICamera camera);
        String ReadFisheyeDewarpTypeByCamera(ICamera camera);
    }

    public interface ILicenseManager : IDataManager
    {
        UInt16 Amount { get; }
        UInt16 Maximum { get; }
        List<Adaptor> Adaptor { get; }
        Boolean CheckLicenseExpire { get; set; }
        event EventHandler OnPluginSaveComplete;

        Dictionary<PluginPackage, Int16> PluginLicense { get; }
        void SavePlugin();
        void SavePlugin(String xml);
    }

    public interface IUserManager : IDataManager
    {
        IUser Current { get; }
        Dictionary<UInt16, IUser> Users { get; }
        Dictionary<UInt16, IUserGroup> Groups { get; }
        Dictionary<IUser, IEnumerable<String>> UserStringPermission { get; }

        //Dictionary<IUserGroup, IEnumerable<String>> DeviceStringPermission { get; }//Kevin AD Login

        UInt16 GetNewUserId();
        //UInt16 GetNewGroupId();//Kevin AD Login

        //IUser FindUserById(UInt16 id);
        //IUserGroup FindUserGroupById(UInt16 id);
    }

    public interface IConfigureManager : IDataManager
    {
        UInt16 PatrolInterval { get; set; }
        Boolean DisplayDeviceId { get; set; }
        Boolean DisplayGroupId { get; set; }
        Boolean DisplayNVRId { get; set; }
        Boolean StorageAlert { get; set; }
        Boolean EnableJoystick { get; set; }
        Boolean EnableAxisJoystick { get; set; }    // Val added, 2015.11.20. For Axis Joystick. Under design, interface has to be modified right here.
        UInt16 ManualRecordDuration { get; set; }
        Int16 InstantPlaybackSeconds { get; set; }
        Boolean ImageWithTimestamp { get; set; }
        MailServer MailServer { get; }
        FtpServer FtpServer { get; }
        String SaveImagePath { get; set; }
        String ExportVideoPath { get; set; }
        UInt16 ExportVideoMaxiFileSize { get; set; }
        Boolean StretchLiveVideo { get; set; }
        Boolean StretchPlaybackVideo { get; set; }
        Boolean KeepLastFrame { get; set; }

        List<VideoWindowTitleBarInformation> VideoWindowTitleBarInformations { get; set; }
        String VideoTitleBarFontFamify { get; set; }
        UInt16 VideoTitleBarFontSize { get; set; }
        String VideoTitleBarFontColor { get; set; }
        String VideoTitleBarBackgroundColor { get; set; }

        String WatermarkFontFamify { get; set; }
        UInt16 WatermarkFontSize { get; set; }
        String WatermarkFontColor { get; set; }
        String WatermarkText { get; set; }
        

        UInt16 CPULoadingUpperBoundary { get; set; }
        UInt16 AutoLockApplicationTimer { get; set; }

        Boolean EnableAutoSwitchLiveStream { get; set; }
        UInt16 AutoSwitchLiveHighProfileCount { get; set; }
        UInt16 AutoSwitchLiveLowProfileCount { get; set; }

        Boolean EnableAutoSwitchDecodeIFrame { get; set; }
        UInt16 AutoSwitchDecodeIFrameCount { get; set; }

        Boolean EnableBandwidthControl { get; set; }
        Bitrate BandwidthControlBitrate { get; set; }
        UInt16 BandwidthControlStream { get; set; }
        Store Store { get; }
        //-----------Custom Live / Playback Compression
        CustomStreamSetting CustomStreamSetting { get; set; }


        // Startup Options
        StartupOptions StartupOptions { get; set; }

        // Lock Application after idling ? Seconds
        //Int16 LockApplicationIdleTime { get; set; }

        //--------TransactionTimeOption
        String TransactionTimeOption { get; set; }

        Boolean WithArchiveServer { get; set; }
        Boolean EnablePlaybackSmooth { get; set; }

        Boolean CameraLastImage { get; set; }
        Boolean FullScreenBestResolution { get; set; }
        Boolean EnableUserDefine { get; set; }
    }


    public interface IServerManager : IDataManager
    {
        event EventHandler<EventArgs<String>> OnServerTimeZoneChange;
        event EventHandler OnStorageStatusUpdate;
        event EventHandler OnRAIDProcessUpdate;
        event EventHandler OnDateTimeUpdate;
        event EventHandler<EventArgs<UInt16>> OnEthernetUpdateCableStatus;
        event EventHandler OnCompleteUpdateEthernetLogout;

        UInt16 Port { get; set; }
        UInt16 SSLPort { get; set; }
        List<Log> LoadLog(DateTime dateTime, LogType[] types);
        Dictionary<String, CameraManufactureFile> DeviceManufacture { get; }
        Dictionary<String, XmlElement> PageList { get; }
        List<Storage> ChangedStorage { get; }// C , 10(10GC keep space)
        List<Storage> Storage { get; }// C , 10(10GC keep space)
        Dictionary<String, DiskInfo> StorageInfo { get; }
        Boolean KeepDaysEnabled { get; set; }
        UInt16 DefaultDeepDays { get; set; }
        Dictionary<UInt16, UInt16> DeviceRecordKeepDays { get; } //Device Id, (1~60) dafault 14

        List<Constant.TimeZone> TimeZones { get; }
        DateTime DateTime { get; set; }
        DateTime ChangedDateTime { get; set; }

        Int32 TimeZone { get; set; }
        Int32 ChangedTimeZone { get; set; }

        String Location { get; }
        String ChangedLocation { get; set; }

        Boolean EnableDaylight { get; set; }
        Boolean ChangedEnableDaylight { get; set; }

        Int32 Daylight { get; set; }
        Int32 ChangedDaylight { get; set; }

        Boolean EnableNTPServer { get; set; }
        String NTPServer { get; set; }

        String Brand { get; }
        String ProductNo { get; }
        Platform Platform { get; }
        RAID RAID { get; }
        Dictionary<UInt16, Ethernet> Ethernets { get; set; }
        DNS DNS { get; set; }
        List<String> DisabledItems { get; }
        List<UInt16> NotAllowPorts { get; }
        String DisplayManufactures(String formal);
        String FormalManufactures(String display);

        ServerCredential Database { get; }
        UInt16 DatabaseKeepMonths { get; set; }
        ReadyState DatabaseReadyStatus { get; set; }
        ReadyState DatabaseKeepMonthsReadyStatus { get; set; }
        Boolean IsPortChange { get; }
        Boolean IsSSLPortChange { get; }

        Int32 CPUUsage { get; set; }

        String ServerVersion { get; }
        String DevicePackVersion { get; }

        void LoadServerTime();
        void LoadStorageInfo();

        void Backup();
        void Restore(Stream configFile, List<String> contents);
        String Upgrade(Stream configFile);
        void Reboot();
        void Shutdown();
        void SaveStorage();
        void LoadRAID();
        void RAIDFormat();
        void SaveEthernet(UInt16 id);
        void UpdateEthernetCableStatus(UInt16 id);
        Boolean CheckSetupEnabled(String item);
        Boolean CheckPorts(UInt16 port);
        Boolean CheckProductNoToSupport(String supportFunction);
        UInt16 CheckProductNoToSupportNumber(String supportFunction);

        Boolean SupportAchiveServer { get; set; }
        Boolean SupportPIP { get; set; }
        Boolean HideExceptionAmount { get; set; }

        Boolean EnableUPNP { get; set; }
    }

    public interface IPOSManager : IDataManager
    {
        event EventHandler<EventArgs<POS_Exception.TransactionItem>> OnPOSLiveEventReceive;

        Dictionary<UInt16, POS_Exception> Exceptions { get; }
        Dictionary<UInt16, IPOSConnection> GenericPOSSetting { get; }
        List<IPOS> POSServer { get; }

        Dictionary<UInt16, IDivision> DivisionManager { get; }
        Dictionary<UInt16, IRegion> RegionManager { get; }
        Dictionary<UInt16, IStore> StoreManager { get; }


        Dictionary<UInt16, IPOSConnection> Connections { get; }
        Dictionary<String, POS_Exception.ExceptionThreshold> ExceptionThreshold { get; }
        ScheduleReports ScheduleReports { get; }
        List<POS_Exception.TemplateConfig> TemplateConfigs { get; }
        XmlDocument ParseExceptionToXml(POS_Exception posException);
        POS_Exception ParserXmlToException(XmlElement configurationNode);
        IPOS FindPOSById(String posId);
        Boolean UsePTSId(String posId);
        String GetNewPOSId();
        UInt16 GetNewPOSLicenseId();

        UInt16 GetNewDivisionId();

        UInt16 GetNewRegionId();
        UInt16 GetNewStoreId();
        UInt16 GetNewConnectionId(Boolean reverse = false);
        UInt16 GetNewExceptionId(Boolean reverse = false);
        UInt16 GetNewGenericPOSSettingId();
        //UInt16 GetNewKeywordId();

        //---------------------------------------------------------------------------
        void StartListenPOSLiveEvent();
        void StopListenPOSLiveEvent();

        //---------------------------------------------------------------------------
        void SaveTemplate();
        XmlDocument ConvertTemplateToXmlDocument();
        void ConvertXmlDocumentToTemplate(XmlDocument xmlDoc);

        //---------------------------------------------------------------------------
        POS_Exception.ExceptionCountList ReadDailyReportByStationGroupByException(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions);

        //---------------------------------------------------------------------------
        POS_Exception.TransactionList ReadTransactionHeadByCondition(Int32 pageIndex, String condition);
        POS_Exception.TransactionList ReadTransactionHeadByCondition(String posId, UInt64 startutc, UInt64 endutc, String searchText, Int32 pageIndex, UInt16 count);
        POS_Exception.TransactionList ReadTransactionHeadByCondition(String[] posIds, String[] cashierIds, UInt64 startutc, UInt64 endutc, String[] exceptions, String[] tags, String[] keywords, Int32 pageIndex, UInt16 count);
        POS_Exception.TransactionList ReadTransactionHeadByCondition(String transactionId, UInt64 startutc, UInt64 endutc, String[] exceptions);

        //---------------------------------------------------------------------------
        POS_Exception.ExceptionDetailList ReadExceptionByCondition(Int32 pageIndex, String condition);
        POS_Exception.ExceptionDetailList ReadExceptionByCondition(String posId, UInt64 startutc, UInt64 endutc, String searchText, Int32 pageIndex, UInt16 count);
        POS_Exception.ExceptionDetailList ReadExceptionByCondition(String[] posIds, String[] cashierIds, String[] cashiers, UInt64 startutc, UInt64 endutc, String[] exceptions, String[] keywords, Int32 pageIndex, UInt16 count);

        //---------------------------------------------------------------------------
        Dictionary<String, List<POS_Exception.ExceptionCount>> ReadExceptionCalculationByDateGroupByRegister(String[] posIds, String[] cashierIds, String[] cashiers, UInt64 startutc, UInt64 endutc, String[] exceptions);

        //---------------------------------------------------------------------------
        List<POS_Exception.ExceptionCount> ReadExceptionCalculationByMonth(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions);

        //---------------------------------------------------------------------------
        POS_Exception.TransactionItemList ReadTransactionById(String transactionId);

        //---------------------------------------------------------------------------
        XmlDocument ReadTransactionByCondition(POS_Exception.AdvancedSearchCriteria criteria, UInt64 startutc, UInt64 endutc, ref XmlDocument conditionXml);
        POS_Exception.TransactionList ReadTransactionByCondition(POS_Exception.AdvancedSearchCriteria criteria);
        XmlDocument ReadTransactionByCondition(XmlDocument xmlDoc);

        //---------------------------------------------------------------------------
        List<POS_Exception.CashierExceptionCountList> ReadDailyReportExceptionGroupByCashier(UInt64 startutc, UInt64 endutc);

        //---------------------------------------------------------------------------
    }
}
