
namespace Constant
{
    public enum ExportVideoStatus : ushort
    {
        ExportAVIStartup = 0,
        SyncInformationWithServer = 1,
        StartDownload = 2,
        ConvertToAVI = 3,
        ExportFinished = 4,
        UserAborted = 5,
        ExportFailed = 6,
        //Converting = 1,
        //CloseAVIFile = 2,
        //ExportAVIFinished = 3,
        //UserAborted = 4,
        //ExportFailed = 5,
    }
}
