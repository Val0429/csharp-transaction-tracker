using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;

namespace Interface
{
	public interface IVideoWindow
	{
		event EventHandler OnSelectionChange;
		event MouseEventHandler OnWindowMouseDrag;
		event EventHandler<MouseEventArgs> OnMouseDown;
		event EventHandler OnFullScreen;
		event EventHandler<EventArgs<String>> OnCloseFullScreen;

		IApp App { get; set; }
		Boolean Visible { get; set; }
		Boolean Active { get; set; }
		ICamera Camera { get; set; }
		IViewer Viewer { get; set; }
		ITrackerContainer Track { get; set; }
		Boolean DisplayTitleBar { get; set; }
		Boolean IsDecodeIFrame { get; }
		IVideoMenu ToolMenu { get; set; }

		Size Size { get; set; }
		Int32 Width { get; set; }
		Int32 Height { get; set; }
		Control Parent { get; set; }
		Point Location { get; set; }
		Padding Padding { get; set; }
		PlayMode PlayMode { get; set; }
		Boolean Stretch { get; set; }
		Boolean Dewarp { get; set; }
        Int16 DewarpType { get; set; }
		PTZMode PtzMode { get; set; }
		WindowLayout WindowLayout { get; set; }
		UInt16 LiveVideoStreamId { get; }
		Int32 LiveBitrate { get; }

		UInt64 GetTimecode();

		void Activate();
		void Deactivate();
		void Stop();
		void Initialize();
		void Play();
		void GoTo(UInt64 timecode);
		void Playback(UInt64 timecode);
		void Reconnect();
		void SaveImage(Boolean withTimestamp);
		void Snapshot(Boolean withTimestamp);
		void AutoDropFrame();
		void DecodeIframe();
		void SwitchVideoStream(UInt16 streamId);
		void GlobalMouseHandler();
		void NextFrame();
		void PreviousFrame();
        void EnablePlaybackSmooth();
        void SetPlaySpeed(UInt16 speed);

		void StartInstantPlayback();
		void StartInstantPlayback(DateTime dateTime);
		void StopInstantPlayback();
		void Reset();

		void ClearAllEventHandler();
		void RegisterViewer(ICamera camera);
		void SendPTZCommand(String cmd);

		void BringToFront();

		void SwitchProfileId(UInt16 profileid);

	    void ServerTimeZoneChange();
	    void DragStop(Point point, EventArgs<Object> e);
	}
}
