using System;

namespace DeviceConstant
{
    public class StreamConfig
    {
        public Bitrate Bitrate = Bitrate.Bitrate1M500K; //DeviceBitrate try to set 1.5M(DeviceBitrate.Bitrate1M500K if avaliable) if not avaliable try closed one
        //public UInt16 Brightness = 50;//(1~100)

        public UInt16 Channel = 1;
        //public UInt16 Contrast = 50;//(1~100)
        public ConnectionProtocol ConnectionProtocol;

        public Compression Compression;//DeviceProfile.Compression.Mjpeg;// MPEG4 for ACTi, MJPEG for other
        public UInt16 Framerate;// = 0;//try to set to 30(if avaliable) if not avaliable try maximum one

        //public UInt16 Hue = 50;//(1~100)

        public Resolution Resolution = Resolution.R720X480;//DeviceResolution check if D1(720*480) / VGA(640*480) is avaliable, if no set to maximum resolution
        public Dewarp Dewarp = Dewarp.NonSpecific;
        public Int32 RegionStartPointX; //=0 for  x of region start point;
        public Int32 RegionStartPointY;//=0 for  y  of region end point;
        public UInt16 MotionThreshold;
        //public UInt16 Saturation = 50;//(1~100)

        public BitrateControl BitrateControl = BitrateControl.NA;
        //public String VideoPosition = "0,0";
        public UInt16 VideoQuality = 60;//(1~100)

        public Ports ConnectionPort = new Ports();

        public String URI;
        public String MulticastNetworkAddress;
        public UInt16 ProfileMode;
    }

    public static class StreamConfigs
    {
        public static StreamConfig Clone(StreamConfig config)
        {
            return new StreamConfig
            {
                ProfileMode = config.ProfileMode,
                Bitrate = config.Bitrate,
                //Brightness = config.Brightness,
                Channel = config.Channel,
                //Contrast = config.Contrast,
                ConnectionProtocol = config.ConnectionProtocol,
                Compression = config.Compression,
                Framerate = config.Framerate,
                //Hue = config.Hue,
                Resolution = config.Resolution,
                //Saturation = config.Saturation,
                //VideoPosition = config.VideoPosition,
                Dewarp = config.Dewarp,
                VideoQuality = config.VideoQuality,
                RegionStartPointX = config.RegionStartPointX,
                RegionStartPointY = config.RegionStartPointY,
                MotionThreshold = config.MotionThreshold,
                BitrateControl = config.BitrateControl,
                URI = config.URI,
                ConnectionPort =
                {
                    Control = config.ConnectionPort.Control,
                    Rtsp = config.ConnectionPort.Rtsp,
                    Streaming = config.ConnectionPort.Streaming,
                    Https = config.ConnectionPort.Https,
                    VideoIn = config.ConnectionPort.VideoIn,
                    AudioIn = config.ConnectionPort.AudioIn
                }
            };
        }
    }
}
