using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Xml;
using Constant;
using DeviceConstant;

namespace Interface
{
	public interface ICamera : IDevice
	{
		event EventHandler<EventArgs<UInt16, ExportVideoStatus>> OnExportVideoProgress;
		event EventHandler OnExportVideoComplete;

		event EventHandler<EventArgs<String>> OnSmartSearchResult;
		event EventHandler OnSmartSearchComplete;

		CameraStatus Status { get; set; }

		CameraProfile Profile { get; set; }
		CameraModel Model { get; set; }

		UInt32 PreRecord { get; set; }
		UInt32 PostRecord { get; set; }
		Schedule RecordSchedule { get; set; }
		Schedule EventSchedule { get; set; }
		EventHandling EventHandling { get; set; }

		Boolean IsAudioOut { get; set; }
		Boolean IsManualRecord { get; set; }
		Boolean IsInUse { get; set; } 
		Dictionary<UInt16, Boolean> DigitalOutputStatus { get; set; }

		//Preset
		UInt16 PresetPointGo { get; set; }
		UInt16 PresetTourGo { get; set; }
		PresetPoints PresetPoints { get; set; }
		PresetTours PresetTours { get; set; }
		void AddPresetPoint(UInt16 id);
		void DeletePresetPoint(UInt16 id);
        Boolean IsLoadPresetPoint { get; set; }

		//bookmark
		List<Bookmark> Bookmarks { get; set; }
		//List<Bookmark> DescBookmarks { get; set; } //for reverse quick search

		CameraType Type { get; set; }
		CameraMode Mode { get; set; }
		StreamConfig StreamConfig { get; }
		UInt16 MaximumSearchResult { get; }
		
		//Motion
		Dictionary<UInt16, Rectangle> MotionRectangles { get; set; }
		Dictionary<UInt16, UInt16> MotionThreshold { get; set; } //(1~100)

		//VAS
		List<PeopleCountingRectangle> Rectangles { get; set; }
		NetworkCredential Dispatcher { get; set; }
		PeopleCountingSetting PeopleCountingSetting { get; set; }

		//IOPort
		Dictionary<UInt16, IOPort> IOPort { get; set; }

		//Via CGI
		Queue<Record> GetParts(UInt16 part, UInt64 start, UInt64 end);
        Queue<Record> GetPlaybackDownloadPart(UInt16 part, UInt64 start, UInt64 end);
		Queue<Record> GetEventParts(UInt16 part, UInt64 start, UInt64 end, EventType events);
		DateTime GetNextRecord(UInt64 now);
		DateTime GetPreviousRecord(UInt64 now);

	    DateTime GetBeginRecord();

		List<CameraEvent> SearchCameraEvent(UInt64 start, UInt64 end, EventType events);
		//Image LoadImageFromHttp(String url);

		Image Snapshot { get; set; }
		Image GetSnapshot();
		Image GetSnapshot(Size size);
		Image GetSnapshot(UInt64 timecode);
		Image GetSnapshot(UInt64 timecode, Size size);
		Image GetFullResolutionSnapshot(Size size,UInt16 channelId);
		
		void ManualRecord();
	    void ManualRecord(String duration);
	    void UserDefine(int eventId, String data);
		void Panic();
		void SmartSearch(UInt32 period, UInt64 keyFrame, UInt64 startTime, UInt64 endTime, Rectangle[] areas);
		void TimePeriodSearch(UInt32 period, UInt64 startTime, UInt64 endTime);
		void EventSearch(UInt64 startTime, UInt64 endTime, List<EventType> events, List<UInt64> period);
		void StopSearch();

		//Via Utility
		String ExportVideo(UInt64 startTime, UInt64 endTime, Boolean displayOsd, Boolean audioIn, Boolean audioOut, UInt16 encode, String path, UInt16 quality, UInt16 scale, UInt16 osdWatermark); //Boolean encode
		void StopExportVideo();
		void ExportVideoProgress(UInt16 progress, ExportVideoStatus status);
	    void InitFisheyeLibrary(ICamera camera, bool dewarpEnable, short mountType);

		Int32 StartAudioTransfer();
		void StopAudioTransfer();

		void DigitalOutput();
		void DigitalOutput(UInt16 id);

        XmlElement XmlFromServer { get; set; }
		ICMS CMS { get; set; }
		String PlaybackSessionId { get; set; }
        Boolean IsArchiveRecord(UInt64 now);
        List<Record> ArchiveServerRecord { get; set; }

        ICamera PIPDevice { get; set; }
        UInt16 PIPStreamId { get; set; }
        Position PIPPosition { get; set; }

        UInt16 PatrolInterval { get; set; }
        Dictionary<UInt16, WindowPTZRegionLayout> PatrolPoints { get; set; }
	}
}