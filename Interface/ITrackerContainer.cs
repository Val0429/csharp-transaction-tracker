using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using DeviceConstant;

namespace Interface
{
	public interface ITrackerContainer
	{
		event EventHandler OnSelectionChange;
		event EventHandler<EventArgs<String>> OnTimecodeChange;
		event EventHandler<EventArgs<TimeUnit, UInt64[]>> OnTimeUnitChange;
		event EventHandler OnDateTimeChange;
		event EventHandler OnDateTimeRangeChange;
		event EventHandler OnGetPartStart;
		event EventHandler OnGetPartCompleted;
        event EventHandler OnGetDownloadPartCompleted;
        event EventHandler OnStop;
		event EventHandler<EventArgs<Dictionary<IDevice, Record>>> OnRecordDataChange;
        event EventHandler<MouseEventArgs> OnTrackerContainerMouseDown;

		Control Parent { get; set; }
		void Invalidate();

		float Rate { get; set; }
		TimeUnit UnitTime { get; set; }
		IServer Server { get; set; }
		Boolean IsMinimize { get; set; }
		UInt16 PageIndex { get; set; }
		UInt16 MaxConnection { get; set; }

		DateTime RangeStartDate { get; set; }
		DateTime RangeEndDate { get; set; }

		DateTime VisibleMaxDateTime { get; set; }
		DateTime VisibleMinDateTime { get; set; }
		
		DateTime DateTime { get; set; }
		Int32 Count { get; }
		Boolean RefreshTracker { set; }
		Boolean IgnoreTriggerOnTimecodeChange { get; set; }
        List<IVideoWindow> VideoWindows { get; set; }

		void AppendDevice(UInt16 index, IDevice device);
		void Stop();
		void PlayOnRate();
		Int32 TicksToX(Int64 ticks);
		Int32 TicksToWidth(Int64 ticks);
		void ShowRecord();
		void UpdateContent(IDevice[] devices);
		void RemoveAll();

		void AddBookmark();
		void EraserBookmark();
		Int64 NextBookmark();
		Int64 PreviousBookmark();
		Int64 NextRecord();
		Int64 PreviousRecord();
        Int64 BeginRecord();

		void ExportRangeStart(Int32 position,Int32 min);
		void ExportRangeEnd(Int32 position, Int32 max);

		void SearchEventAdd(EventType type);
		void SearchEventRemove(EventType type);

		//void ResetBackgroundScalePosition();
		List<EventType> SearchEventType { get; }
	    Dictionary<UInt16, ITracker> Trackers { get; set; }
	}
}
