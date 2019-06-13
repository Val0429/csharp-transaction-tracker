using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DeviceConstant;

namespace Interface
{
    public interface IClient
    {
        // Property
        Dictionary<String, IPage> Pages { get; }

        IPage PageActivated { get; set; }

        bool IsFullScreen { get; set; }


        // Methods
        void DragStart(Object sender, EventArgs<Object> e);

        void Activate();

        void Activate(IPage page);

        void Deactivate();

        void UpdateClientSetting(RestoreClientColumn keyColumn, String value1, List<Int16> value2);

        void SwitchPage(String page, Object parameter);


        // Event
        event EventHandler<EventArgs<String, Object>> OnSwitchPage;
    }

    public interface IApp : IViewerProvider, IClient
    {
        event EventHandler OnLogout;
        event EventHandler OnCustomVideoStream;
        event EventHandler<EventArgs<ICamera>> OnHotSpotEvent;
        event EventHandler<EventArgs<Boolean>> OnLockApplication;

        //====  Plugin event handle     ====
        event EventHandler<EventArgs<Int32, Int32, Int32>> OnJoystickMove;
        event EventHandler<EventArgs<String, String, Int32>> OnJoystickOperation;

        event EventHandler<EventArgs<String>> OnOpcTreeReceived;
        String KeyPad { get; set; }

        event EventHandler<EventArgs<Boolean>> OnAutoLoadVideoOn;
        event EventHandler<EventArgs<Boolean>> OnTitleBarFormatOn;
        event EventHandler<EventArgs<Boolean>> OnProfileSelectionOn;
        event EventHandler<EventArgs<Boolean>> OnTranscodeStreamOn;

        event EventHandler OnAppStarted;
        event EventHandler OnUserDefineDeviceGroupModify;

        event EventHandler SaveCompleted;

        Boolean isSupportImageStitching { get; set; }
        event EventHandler<EventArgs<Boolean>> OnSupportImageStitching;
        void ImageStitching();

        // Switcher DVRMode
        Boolean SwitchDVRMode(String mode, bool status);
        Int64 IdleTimer { get; set; }

        void RemoveProperties();
        //====  Plugin event handle     ====

        String Version { get; }
        String DevicePackVersion { get; }

        UInt64 PlaybackTimeCode { get; set; }

        Boolean IsLock { get; }
        Boolean IsInitialize { get; }
        Boolean IsHidePanel { get; set; }

        Form Form { get; set; }
        ServerCredential Credential { get; set; }
        String Language { get; set; }

        String LoginProgress { get; }

        Boolean Login();
        void Logout();
        void Quit();
        void Save();
        void Undo();


        void PopupInstantPlayback(IDevice device, UInt64 timecode, object info);
        void PopupInstantPlayback(IDevice device, UInt64 timecode);
        void PopupLiveStream(IDevice device);
        void ExportVideo(IDevice[] usingDevices, DateTime start, DateTime end);
        void ExportVideoWithInfo(IDevice[] usingDevices, DateTime start, DateTime end, String xmlInfo);


        void DownloadCase(IDevice[] usingDevices, DateTime start, DateTime end, XmlDocument xmlDoc);
        void PrintImage(List<ICamera> printDevices, Dictionary<ICamera, Image> printImages, DateTime dateTime);

        void SaveUserDefineDeviceGroup(IDeviceGroup group);
        void DeleteUserDefineDeviceGroup(IDeviceGroup group);

        void GlobatMouseMoveHandler();
        void WindowFocusGet();
        void WindowFocusLost();

        void KeyPress(Keys keyData);

        Int32 AudioOutChannelCount { get; }

        StartupOptions StartupOption { get; set; }


        Boolean OpenAnotherProcessAfterLogout { get; set; }

        Panel HeaderPanel { get; set; }
        Panel ToolPanel { get; set; }
        Panel SwitchPagePanel { get; set; }
        Panel PageFunctionPanel { get; set; }
        Panel PageDockIconPanel { get; set; }
        Panel MiniFunctionPanel { get; set; }
        Panel MainPanel { get; set; }
        Panel WorkPanel { get; set; }
        Panel StatePanel { get; set; }

    }

    public interface IViewerProvider
    {
        //---------------------------ViewerProvider---------------------------
        void RegistViewer(UInt16 count);
        IViewer RegistViewer();
        void UnregistViewer(IViewer viewer);
        Int32 PlaybackCount { get; set; }

        CustomStreamSetting CustomStreamSetting { get; }
    }
}
