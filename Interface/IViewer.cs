using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;

//using DeviceProfile;

namespace Interface
{
	public interface IViewer
	{
		event EventHandler OnFullScreen;
		event EventHandler<EventArgs<String>> OnCloseFullScreen;

		event EventHandler OnNetworkStatusChange;
		event EventHandler<EventArgs<Int32>> OnConnect;
		event EventHandler<EventArgs<Int32>> OnPlay;
		event EventHandler<EventArgs<Int32>> OnDisconnect;
		event EventHandler<EventArgs<Int32, Int32, Int32>> OnMouseKeyDown;
		event EventHandler<EventArgs<String>> OnFrameTimecodeUpdate;
		event EventHandler<EventArgs<Int32>> OnBitrateUpdate;

		String Version { get; }
		String ComponentName { get; }

		IApp App { get; set; }
		ICamera Camera { get; set; }
		Size Size { get; set; }
		Control Parent { get; set; }
		Boolean Visible { get; set; }
		Boolean Active { set; }
		Boolean StretchToFit { get; set; }
		Boolean Dewarp { get; set; }
        Int16 DewarpType { get; set; }
		NetworkStatus NetworkStatus { get; }
		PTZMode PtzMode { get; set; }
		String Title { set; }
		Int32 TimeZone { set; }
		UInt64 Timecode { get; }
		UInt64 PlaybackTimecode { get; set; }

		UInt16 Port { set; }
		String Host { set; }
		String UserName { set; }
		String UserPwd { set; }
		Boolean DisplayTitleBar { get;  set; }
		Boolean AudioIn { get; set; }
		PlayMode PlayMode { get; }
		Boolean TranscodeStream { get; set; }
		Boolean IsDigitalPtzZoom { get; }
		
		Int32 AdjustBrightness { get; set; }

		void BringToFront();
		void Focus();
		void Connect();
		void Play();
		void GoTo(UInt64 timecode);
		void Playback(UInt64 timecode);
		void Stop();
		void Reconnect();
		void SendPTZCommand(String cmd);
		void Snapshot(String filename, Boolean withTimestamp);
		void SetText(Int16 x, Int16 y, String text, Int16 fontsize, Int16 colorRed, Int16 colorGreen, Int16 colorBlue);
		void CloseFullScreen();
		void SetVisible(Boolean visible);
		void NextFrame();
		void PreviousFrame();

	    void EnablePlaybackSmoothMode(UInt16 mode);
        void SetPlaySpeed(UInt16 speed);
        void EnableKeepLastFrame(UInt16 enable);
        void InitFisheyeLibrary(Boolean dewarpEnable, Int16 mountType);
	    void ShowRIPWindow(Boolean enable);
        String GetDigitalPtzRegion();
        void SetDigitalPtzRegion(String xmlDoc);
	    void SetDigitalPtzRegionCount(UInt16 count);
		void AutoDropFrame();
		void DecodeIframe();
		UInt16 ProfileId { get; set; }
		void SwitchVideoStream(UInt16 streamId);

		void SetupMotionStart();
		String GetMotionRegion();
		void EnableMotionDetection(Boolean enable);
		void SetSubLayoutRegion(ISubLayout subLayout);
		void SetSubLayoutRegion(List<ISubLayout> subLayouts);

		void UpdateSubLayoutRegion(ISubLayout subLayout);
		String UpdateSubLayoutRegion();

		void UserDefineEventTrigger(String msg);
		void PreDefineEventTrigger(String msg);
		void UpdateRecordStatus();
	}
}