using System;
using DeviceConstant;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        protected String CustomStreamSetting(CustomStreamSetting setting)
        {
            if (!setting.Enable) return "";
            if (Camera.Profile == null) return "";

            String stream = "";
            if (PlayMode == PlayMode.LiveStreaming)
            {
                stream = setting.StreamId > Camera.Profile.StreamConfigs.Count
                                            ? "&stream=1"
                                            : "&stream=" + setting.StreamId;
            }

            var width = Resolutions.ToWidth(setting.Resolution);
            var height = Resolutions.ToHeight(setting.Resolution);
            
            //calculator same aspect ratio
            UInt16 streamId = 0;
            if (PlayMode == PlayMode.LiveStreaming)
            {
                //currently live stream
                streamId = (ProfileId == 0) ? Camera.Profile.StreamId : ProfileId;
            }
            else
            {
                //setup playback stream
                streamId = Camera.Profile.RecordStreamId;
            }

            if (Camera.Profile.StreamConfigs.ContainsKey(streamId) && Camera.Profile.StreamConfigs[streamId].Resolution != Resolution.NA)
            {
                var resolution = Camera.Profile.StreamConfigs[streamId].Resolution;
                var aspectRatio = (width * 1.0) / Resolutions.ToWidth(resolution) * 1.0;
                
                //orange width is small than transcode width, dont make it bigger
                if (aspectRatio >= 1.0)
                {
                    width = Resolutions.ToWidth(resolution);
                    height = Resolutions.ToHeight(resolution);
                }
                else
                {
                    height = Convert.ToUInt16(Math.Round(Resolutions.ToHeight(resolution) * aspectRatio / 8.0) * 8.0);
                }
            }

            stream += "&streamtype=" + Compressions.ToString(setting.Compression).ToLower() +
                      "&width=" + width +
                      "&height=" + height;

            switch (setting.Compression)
            {
                case Compression.Mjpeg:
                    stream += "&quality=" + setting.Quality;
                    break;

                case Compression.H264:
                    var bitrate = Convert.ToInt32(Bitrates.ToString(setting.Bitrate)) * 1000;
                    stream += "&bitrate=" + bitrate;
                    break;
            }

            if (setting.Framerate > 0)
                stream += "&fps=" + setting.Framerate;

            return stream;
        }

        private Int32 _adjustBrightness = 0;
        public Int32 AdjustBrightness { 
            get
            {
                return _adjustBrightness;
            }
            set
            {
                if (PlayMode == PlayMode.GotoTimestamp || PlayMode == PlayMode.Playback1X)
                {
                    _adjustBrightness = Math.Min(Math.Max(value, -255), 255);
                    _control.AdjustBrightness(_adjustBrightness);
                }
            }
        }
    }
}
