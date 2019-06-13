using System;
using System.Collections.Generic;
using DeviceConstant;
using Interface;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        public static class VideoProviderUtility
        {
            //private static readonly Dictionary<String, VideoProvider> VideoProviderDic = new Dictionary<String, VideoProvider>();
            //private static readonly Dictionary<String, List<String>> VideoProviderUsagec = new Dictionary<String, List<String>>();
        //    public static void ConnectDeviceLayout(IDeviceLayout deviceLayout, PlayMode playMode, ref String layoutguid)
        //    {
        //        String guid = (playMode == DeviceConstant.PlayMode.LiveStreaming) ? deviceLayout.LiveGUID : deviceLayout.PlaybackGUID;

        //        if (VideoProviderUsagec.ContainsKey(guid))
        //        {
        //            layoutguid = Guid.NewGuid().ToString();
        //            VideoProviderUsagec[guid].Add(layoutguid);
        //            return;
        //        }

        //        var videoProvider = RegistVideoProvider();
                
        //        videoProvider.DeviceLayout = deviceLayout;

        //        VideoProviderDic.Add(guid, videoProvider);
        //        layoutguid = Guid.NewGuid().ToString();
        //        VideoProviderUsagec[guid] = new List<String> { layoutguid };

        //        if (playMode == DeviceConstant.PlayMode.LiveStreaming) 
        //            videoProvider.Play();
        //        else
        //            videoProvider.Playback();
        //    }

        //    public static void DisconnectDeviceLayout(IDeviceLayout deviceLayout, PlayMode playMode, ref String layoutguid)
        //    {
        //        String guid = (playMode == DeviceConstant.PlayMode.LiveStreaming) ? deviceLayout.LiveGUID : deviceLayout.PlaybackGUID;

        //        if (!VideoProviderUsagec.ContainsKey(guid)) return;

        //        VideoProviderUsagec[guid].Remove(layoutguid);
        //        //still some one use this crop strean
        //        if (VideoProviderUsagec[guid].Count != 0)
        //            return;

        //        var videoProvider = VideoProviderDic[guid];
        //        videoProvider.Stop();
        //        StoredVideoProvider.Enqueue(videoProvider);

        //        VideoProviderUsagec.Remove(guid);
        //        VideoProviderDic.Remove(guid);
        //    }

        //    public static void GoTo(UInt64 timecode, String guid)
        //    {
        //        if (VideoProviderDic.ContainsKey(guid))
        //        {
        //            VideoProviderDic[guid].GoTo(timecode);
        //            return;
        //        }
        //    }

        //    public static void NextFrame(String guid)
        //    {
        //        if (VideoProviderDic.ContainsKey(guid))
        //        {
        //            VideoProviderDic[guid].NextFrame();
        //            return;
        //        }
        //    }

        //    public static void PreviousFrame(String guid)
        //    {
        //        if (VideoProviderDic.ContainsKey(guid))
        //        {
        //            VideoProviderDic[guid].PreviousFrame();
        //            return;
        //        }
        //    }

        //    private static readonly Queue<VideoProvider> StoredVideoProvider = new Queue<VideoProvider>();

        //    private static VideoProvider RegistVideoProvider()
        //    {
        //        VideoProvider videoProvider;
        //        if (StoredVideoProvider.Count > 0)
        //        {
        //            videoProvider = StoredVideoProvider.Dequeue();
        //        }
        //        else
        //        {
        //            videoProvider = new VideoProvider();
        //        }

        //        return videoProvider;
        //    }
        }

    }
}
