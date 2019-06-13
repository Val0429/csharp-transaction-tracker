
namespace Constant
{
    public enum Permission : ushort
    {
        Access = 0,

        //Device Access Function
        OpticalPTZ = 1,
        AudioIn = 2,
        AudioOut = 3,
        ManualRecord = 4,
        ExportVideo = 5,
        PrintImage = 6,

        //Setup
        Server = 90,
        Device = 91,
        DeviceGroup = 92, //bye bye
        General = 93,
        User = 94,
        Event = 95,
        Schedule = 96,
        Joystick = 97,
        License = 98,
        Log = 99,
        NVR = 100,
        ImageStitching = 101,
        Mobile = 102,

        //IVS
        PeopleCounting = 301,

        // POS
        POS = 601,
        POSConnection = 602,
        Exception = 603,
        ScheduleReport = 604,
        ExceptionReport = 605,
        Division=606,
        Region=607,
        Store=608,

        // Plugin
        PluginLicense = 901,
        IOModel = 902,
        IOHandle = 903,

        // SIS
        LPR,
        LPRGroup,

        ACS,
        CardHolder,
        Department,
        Report,
    }
}
