using System;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        protected Boolean DecodeIframeFlag;
        public void AutoDropFrame(Object sender, EventArgs e)
        {
            AutoDropFrame();
        }

        public void AutoDropFrame()
        {
            DecodeIframeFlag = false;
            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.AutoDropFrame();
            }
            foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
            {
                foreach (IVideoWindow videoWindow in popupVideoMonitor.VideoMonitor.VideoWindows)
                {
                    videoWindow.AutoDropFrame();
                }
            }
            if (ActivateVideoWindow != null)
            {
                ToolMenu.CheckStatus();
            }

            if (OnDecodeChange != null)
                OnDecodeChange(this, new EventArgs<DecodeMode>((DecodeIframeFlag ? DecodeMode.DecodeI : DecodeMode.DropFrame)));
        }

        public void DecodeIframe(Object sender, EventArgs e)
        {
            DecodeIframe();
        }

        public void DecodeIframe()
        {
            DecodeIframeFlag = true;
            foreach (var videoWindow in VideoWindows)
            {
                videoWindow.DecodeIframe();
            }

            foreach (PopupVideoMonitor popupVideoMonitor in UsingPopupVideoMonitor)
            {
                if(popupVideoMonitor.VideoMonitor == null) continue;
                foreach (IVideoWindow videoWindow in popupVideoMonitor.VideoMonitor.VideoWindows)
                {
                    videoWindow.DecodeIframe();
                }
            }

            if (ActivateVideoWindow != null)
            {
                ToolMenu.CheckStatus();
            }

            if (OnDecodeChange != null)
                OnDecodeChange(this, new EventArgs<DecodeMode>((DecodeIframeFlag ? DecodeMode.DecodeI : DecodeMode.DropFrame)));
        }

        public void ChangeProfileMode(String mode)
        {
            foreach (var videoWindow in VideoWindows)
            {
                if (!videoWindow.Visible) continue;
                if (videoWindow.Camera == null) continue;
                if (videoWindow.Camera.Profile == null) continue;

                switch (mode)
                {
                    case "HIGH":
                        videoWindow.SwitchVideoStream(videoWindow.Camera.Profile.HighProfile);
                        break;

                    case "MEDIUM":
                        videoWindow.SwitchVideoStream(videoWindow.Camera.Profile.MediumProfile);
                        break;

                    case "LOW":
                        videoWindow.SwitchVideoStream(videoWindow.Camera.Profile.LowProfile);
                        break;

                    default:
                        videoWindow.SwitchVideoStream(0); //orange live stream setting
                        break;
                }
            }
        }
    }
}
