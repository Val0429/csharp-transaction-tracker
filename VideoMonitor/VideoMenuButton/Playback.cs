using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;

namespace VideoMonitor
{
    public partial class VideoMenu
    {

        protected Button _instantplaybackButton;
        protected Button _playbackButton;

        protected static readonly Image _instantplayback = Resources.GetResources(Properties.Resources.instantplayback, Properties.Resources.IMGInstantplayback);
        protected static readonly Image _instantplaybackactivate = Resources.GetResources(Properties.Resources.instantplayback_activate, Properties.Resources.IMGInstantplayback_activate);
        protected static readonly Image _playback = Resources.GetResources(Properties.Resources.playback, Properties.Resources.IMGPlayback);

        protected void InstantPlaybackButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if ((String.Equals(_instantplaybackButton.Tag.ToString(), "Inactivate")))
            {
                _instantplaybackButton.Tag = "Activate";
                _instantplaybackButton.BackgroundImage = _instantplaybackactivate;
                if (VideoWindow != null)
                {
                    VideoWindow.StartInstantPlayback();
                    Server.WriteOperationLog("Device %1 Start Instant Playback".Replace("%1", VideoWindow.Camera.Id.ToString()));
                }
            }
            else
            {
                _instantplaybackButton.Tag = "Inactivate";
                _instantplaybackButton.BackgroundImage = _instantplayback;
                if (VideoWindow != null)
                {
                    VideoWindow.StopInstantPlayback();
                    Server.WriteOperationLog("Device %1 Stop Instant Playback".Replace("%1", VideoWindow.Camera.Id.ToString()));
                }
            }
        }

        protected void UpdateInstantPlaybackButton()
        {
            if (_instantplaybackButton == null || VideoWindow.Viewer == null) return;
            _count++;
            if (VideoWindow.PlayMode == PlayMode.GotoTimestamp || VideoWindow.PlayMode == PlayMode.Playback1X)
            {
                _instantplaybackButton.BackgroundImage = _instantplaybackactivate;
                _instantplaybackButton.Tag = "Activate";
            }
            else
            {
                _instantplaybackButton.BackgroundImage = _instantplayback;
                _instantplaybackButton.Tag = "Inactivate";
            }
            Controls.Add(_instantplaybackButton);
        }

        protected void UpdatePlaybackButton()
        {
            if (_playbackButton == null || VideoWindow.Viewer == null) return;

            if ((VideoWindow.PlayMode == PlayMode.GotoTimestamp || VideoWindow.PlayMode == PlayMode.Playback1X))
            {
                SetButtonPosition(_playbackButton);
                Controls.Add(_playbackButton);
                _count++;
            }
            else
            {
                Controls.Remove(_playbackButton);
            }
        }
    }
}