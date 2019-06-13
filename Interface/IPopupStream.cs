
using System;

namespace Interface
{
    public interface IPopupStream
    {
        void PopupInstantPlayback(IDevice device, UInt64 timecode, object info);
        void PopupInstantPlayback(IDevice device, UInt64 timecode);
        void PopupLiveStream(IDevice device);

        UInt16 GetLiveStreamCount { get; }
        UInt16 GetInstantPlaybackCount { get; }
    }
}
