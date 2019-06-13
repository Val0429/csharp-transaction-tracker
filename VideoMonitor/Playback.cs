using System;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        protected UInt64 Timecode;

        public void GoTo(UInt64 timecode)
        {
            Timecode = timecode;
            GetTimecodeTimer.Enabled = false;

            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.GoTo(timecode);
            }
        }

        public void GoTo(Object sender, EventArgs<String> e)
        {
            String value = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "Timestamp");

            if (value != "")
                GoTo(Convert.ToUInt64(value));
        }

        public void Playback(UInt64 timecode)
        {
            //_timecode = timecode;
            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.Playback(timecode);
            }

            GetTimecodeTimer.Enabled = true;
        }

        public void Playback(Object sender, EventArgs<UInt64> e)
        {
            SetPlaySpeed(1);
            Playback(e.Value);
        }

        public void PlaybackOnRate(Object sender, EventArgs<UInt64, UInt16> e)
        {
            SetPlaySpeed(e.Value2);
            Playback(e.Value1);
        }

        private void SetPlaySpeed(UInt16 speed)
        {
            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.SetPlaySpeed(speed);
            }
        }

        public void NextFrame(Object sender, EventArgs e)
        {
            GetTimecodeTimer.Enabled = false;
            //call focus windows's frame, other window goto the same time after it's call back return
            IVideoWindow focusWindow = GetFirstOrActivedWindowThatHaveDevice();
            if (focusWindow == null) return;

            focusWindow.Viewer.OnFrameTimecodeUpdate -= OtherWindowGotoTheSameTimecode;
            focusWindow.Viewer.OnFrameTimecodeUpdate += OtherWindowGotoTheSameTimecode;

            //jump together
            foreach (var videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null || !videoWindow.Visible) continue;
                videoWindow.NextFrame();
            }
            //focusWindow.NextFrame();
        }

        public void PreviousFrame(Object sender, EventArgs e)
        {
            GetTimecodeTimer.Enabled = false;
            //call focus windows's frame, other window goto the same time after it's call back return
            IVideoWindow focusWindow = GetFirstOrActivedWindowThatHaveDevice();
            if (focusWindow == null) return;

            focusWindow.Viewer.OnFrameTimecodeUpdate -= OtherWindowGotoTheSameTimecode;
            focusWindow.Viewer.OnFrameTimecodeUpdate += OtherWindowGotoTheSameTimecode;

            ////jump together
            foreach (var videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null || !videoWindow.Visible) continue;
                videoWindow.PreviousFrame();
            }
            //focusWindow.PreviousFrame();
        }

        public void EnablePlaybackSmooth(Object sender, EventArgs e)
        {
            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.EnablePlaybackSmooth();
            }
        }

        private void OtherWindowGotoTheSameTimecode(Object sender, EventArgs<String> e)
        {
            IVideoWindow focusWindow = GetFirstOrActivedWindowThatHaveDevice();

            if (focusWindow == null || focusWindow.Camera == null) return;

            focusWindow.Viewer.OnFrameTimecodeUpdate -= OtherWindowGotoTheSameTimecode;

            //millisecond
            Timecode = focusWindow.GetTimecode();

            var focusWindowTimecode = Timecode / 1000;

            foreach (var videoWindow in VideoWindows)
            {
                if (videoWindow == focusWindow || videoWindow.Camera == null || !videoWindow.Visible) continue;

                //not at the same SECEND
                if (focusWindowTimecode != videoWindow.GetTimecode() / 1000)
                    videoWindow.GoTo(Timecode);
            }

            RaiseOnTimecodeChange(Timecode);
        }

        protected virtual IVideoWindow GetFirstOrActivedWindowThatHaveDevice()
        {
            if (ActivateVideoWindow != null && ActivateVideoWindow.Camera != null)
            {
                return ActivateVideoWindow;
            }

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible || videoWindow.Camera == null) continue;

                return videoWindow;
            }

            return null;
        }

        protected static String TimecodeChangeXml(String timestamp)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Timestamp", timestamp));

            return xmlDoc.InnerXml;
        }

        protected void GetTimecode(Object sender, EventArgs e)
        {
            GetTimecode();
        }

        protected virtual void GetTimecode()
        {
            UInt64 timecode = 0;
            IVideoWindow fitsrWindow = null;
            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible || videoWindow.Camera == null) continue;

                fitsrWindow = videoWindow;
                timecode = Math.Max(timecode, videoWindow.GetTimecode());
            }

            if (fitsrWindow == null) return;
            if (timecode == 0) return;

            //already di the sheft at videoWindow.GetTimecode()
            //if (Server.Server.TimeZone != fitsrWindow.Camera.Server.Server.TimeZone)
            //{
            //    Int64 time = Convert.ToInt64(timecode);
            //    time += (fitsrWindow.Camera.Server.Server.TimeZone * 1000);
            //    time -= (Server.Server.TimeZone * 1000);
            //    timecode = Convert.ToUInt64(time);
            //}

            App.PlaybackTimeCode = Timecode;

            Timecode = timecode;
            RaiseOnTimecodeChange(timecode);
        }

        protected void GetBitrate(Object sender, EventArgs e)
        {
            if (!UpdateBitrateTimer.Enabled) return;
            if (OnBitrateUsageChange == null) return;

            Int32 bitrate = 0;

            foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
            {
                foreach (IVideoWindow videoWindow in popupVideoMonitor.VideoMonitor.VideoWindows)
                {
                    if (!videoWindow.Visible || videoWindow.Camera == null || (videoWindow.PlayMode != PlayMode.LiveStreaming && videoWindow.PlayMode != PlayMode.Playback1X)) continue;

                    bitrate += videoWindow.LiveBitrate;
                }
            }

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible || videoWindow.Camera == null || (videoWindow.PlayMode != PlayMode.LiveStreaming && videoWindow.PlayMode != PlayMode.Playback1X)) continue;

                bitrate += videoWindow.LiveBitrate;
            }

            var bitrateStr = "";
            if (bitrate > 0)
            {
                var mb = Convert.ToInt32(Math.Floor(bitrate / 1000.0));
                if (mb > 0)
                    bitrateStr = mb + "M";

                bitrateStr += (bitrate % 1000) + "K";
            }
            else if(bitrate == 0)
            {
                bitrateStr = "N/A";
            }

            if (!UpdateBitrateTimer.Enabled) return;

            OnBitrateUsageChange(this, new EventArgs<String>(bitrateStr));
        }
    }
}
