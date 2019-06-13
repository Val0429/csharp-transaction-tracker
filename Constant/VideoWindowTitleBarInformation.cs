using System;
using System.Collections.Generic;

namespace Constant
{
    public enum VideoWindowTitleBarInformation : ushort
    {
        NA,
        ChannelNumber,
        CameraName,
        Compression,
        Resolution,
        Bitrate,
        FPS,
        DateTime
    }

    public static class VideoWindowTitleBarInformationFormats
    {
        public static VideoWindowTitleBarInformation ToIndex(String value)
        {
            foreach (KeyValuePair<VideoWindowTitleBarInformation, String> keyValuePair in List)
                if (String.Equals(value.ToUpper(), keyValuePair.Value.ToUpper()))
                    return keyValuePair.Key;

            return VideoWindowTitleBarInformation.NA;
        }

        public static String ToString(VideoWindowTitleBarInformation index)
        {
            foreach (KeyValuePair<VideoWindowTitleBarInformation, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return String.Empty;
        }

        public static readonly Dictionary<VideoWindowTitleBarInformation, String> List = new Dictionary<VideoWindowTitleBarInformation, String>
                                                             {
                                                                    //{ VideoWindowTitleBarInformation.ChannelNumber, "ChannelNumber" },
                                                                    { VideoWindowTitleBarInformation.CameraName, "CameraName" },
                                                                    { VideoWindowTitleBarInformation.Compression, "Compression" },
                                                                    { VideoWindowTitleBarInformation.Resolution, "Resolution" },
                                                                    { VideoWindowTitleBarInformation.Bitrate, "Bitrate" },
                                                                    { VideoWindowTitleBarInformation.FPS, "FPS" },
                                                                    { VideoWindowTitleBarInformation.DateTime, "DateTime" },
                                                             };
    }

}
