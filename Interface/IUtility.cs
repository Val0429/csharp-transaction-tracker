using System;
using System.Collections.Generic;
using Constant;

namespace Interface
{
    public interface IUtility
    {
        String Version { get; }
        String ComponentName { get; }

        IServer Server { set; }

        void Quit();

        //ONLY start once(login), after use UpdateEventRecive to refresh listen channels
        void StartEventReceive();
        void StopEventReceive();
        void UpdateEventRecive();
        void GetAllChannelStatus();
        void GetAllNVRStatus();

        //Joystick
        Boolean JoystickEnabled { get; }
        void InitializeJoystick();
        void InitializeAxisJoystick();      // Val added, 2015.11.20. For Axis Joystick. Under design, interface has to be modified right here.
        void StartJoystickTread();
        void StopJoystickTread();
        event EventHandler<EventArgs<JoystickEvent>>OnMoveAxis;
        event EventHandler<EventArgs<Dictionary<UInt16, Boolean>>> OnClickButton;

        // Val added, 2015.11.20. For Axis Joystick New Events. Under design, interface has to be modified right here.
        #region Axis Events
        event EventHandler<EventArgs<AxisJoystickEvent>> OnAxisJoystickRotate;
        event EventHandler<EventArgs<AxisJoystickButton>> OnAxisJoystickButtonDown;

        event EventHandler<EventArgs<AxisJogDialEvent>> OnAxisJogDialRotate;
        event EventHandler<EventArgs<AxisJogDialEvent>> OnAxisJogDialShuttle;
        event EventHandler<EventArgs<AxisJogDialButton>> OnAxisJogDialButtonDown;

        event EventHandler<EventArgs<AxisKeyPadButton>> OnAxisKeyPadButtonDown;
        #endregion Axis Events

        void PlaySystemSound(UInt16 times, UInt16 duration, UInt16 interval);

        Int32 AudioOutChannelCount { get; }
        Int32 StartAudioTransfer(String channels);
        void StopAudioTransfer();

        String ExportVideo(ICamera camera, UInt64 startTime, UInt64 endTime, Boolean displayOsd, Boolean audioIn, Boolean audioOut, UInt16 encode, String path, UInt16 quality, UInt16 scale, UInt16 osdWatermark);//Boolean encode
        void StopExportVideo();

        void UploadPack(String fileName);
        void StopUploadPack(String fileName);
        void InitFisheyeLibrary(ICamera camera, Boolean dewarpEnable, Int16 mountType);
    }
}
