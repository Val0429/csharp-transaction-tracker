using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;

namespace VideoMonitor
{
    public partial class VideoMenu
    {
        protected Button _audioInButton;
        protected Button _audioOutButton;

        private static readonly Image _audioin = Resources.GetResources(Properties.Resources.audioin, Properties.Resources.IMGAudioin);
        private static readonly Image _audioindisable = Resources.GetResources(Properties.Resources.audioin_disable, Properties.Resources.IMGAudioin_disable);
        private static readonly Image _audioinactivate = Resources.GetResources(Properties.Resources.audioin_activate, Properties.Resources.IMGAudioin_activate);

        private static readonly Image _audioout = Resources.GetResources(Properties.Resources.audioout, Properties.Resources.IMGAudioout);
        private static readonly Image _audiooutactivate = Resources.GetResources(Properties.Resources.audioout_activate, Properties.Resources.IMGAudioout_activate);

        protected virtual void AudioInMouseClick(Object sender, MouseEventArgs e)
        {
            if ((String.Equals(_audioInButton.Tag.ToString(), "Inactivate")))
            {
                _audioInButton.Tag = "Activate";
                _audioInButton.BackgroundImage = _audioinactivate;

                if (VideoWindow != null && VideoWindow.Viewer != null)
                    VideoWindow.Viewer.AudioIn = true;

                if (VideoWindow != null && VideoWindow.Camera != null)
                    Server.WriteOperationLog("Device %1 Enable Audio".Replace("%1", VideoWindow.Camera.Id.ToString()));
            }
            else
            {
                _audioInButton.Tag = "Inactivate";
                _audioInButton.BackgroundImage = _audioin;

                if (VideoWindow != null && VideoWindow.Viewer != null)
                    VideoWindow.Viewer.AudioIn = false;

                if (VideoWindow != null && VideoWindow.Camera != null)
                    Server.WriteOperationLog("Device %1 Disable Audio".Replace("%1", VideoWindow.Camera.Id.ToString()));
            }
        }

        protected virtual void AudioOutButtonMouseDown(Object sender, MouseEventArgs e)
        {
            if (VideoWindow != null && VideoWindow.Camera != null)
            {
                IsPressButton = true;
                VideoWindow.Camera.StartAudioTransfer();
                App.IdleTimer = -1;
                Server.WriteOperationLog("Device %1 Enable Speaker".Replace("%1", VideoWindow.Camera.Id.ToString()));
                //var result = VideoWindow.Camera.StartAudioTransfer();
                //if (result == 1)
                //{
                //    _audioOutButton.Tag = "Activate";
                //    _audioOutButton.BackgroundImage = _audiooutactivate;
                //}
            }

            //if (OnButtonClick != null)
            //    OnButtonClick(this, new EventArgs<String>(OnButtonClickXml.Replace("{BUTTON}", _audioOutButton.Name).Replace("%1", _audioOutButton.Tag.ToString())));
        }

        protected virtual void AudioOutButtonMouseUp(Object sender, MouseEventArgs e)
        {
            _audioOutButton.Tag = "Inactivate";
            _audioOutButton.BackgroundImage = _audioout;

            IsPressButton = false;

            if (VideoWindow != null && VideoWindow.Camera != null)
            {
                VideoWindow.Camera.StopAudioTransfer();

                Server.WriteOperationLog("Device %1 Disable Speaker".Replace("%1", VideoWindow.Camera.Id.ToString()));
            }
            App.IdleTimer = 0;
            //if (OnButtonClick != null)
            //    OnButtonClick(this, new EventArgs<String>(OnButtonClickXml.Replace("{BUTTON}", _audioOutButton.Name).Replace("%1", _audioOutButton.Tag.ToString())));
        }

        protected void UpdateAudioInButton()
        {
            if (_audioInButton == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_audioInButton);
                return;
            }

            if (VideoWindow.Viewer.Visible && VideoWindow.Camera.Model.NumberOfAudioIn > 0
                && VideoWindow.Camera.CheckPermission(Permission.AudioIn))
            {
                if (VideoWindow.IsDecodeIFrame)
                {
                    _audioInButton.BackgroundImage = _audioindisable;
                    _audioInButton.Tag = "Activate";
                    _audioInButton.Enabled = false;
                }
                else
                {
                    if (VideoWindow.Viewer.AudioIn)
                    {
                        _audioInButton.BackgroundImage = _audioinactivate;
                        _audioInButton.Tag = "Activate";
                    }
                    else
                    {
                        _audioInButton.BackgroundImage = _audioin;
                        _audioInButton.Tag = "Inactivate";
                    }
                    _audioInButton.Enabled = true;
                }

                SetButtonPosition(_audioInButton);
                Controls.Add(_audioInButton);
                _count++;
            }
            else
            {
                _audioInButton.BackgroundImage = _audioin;
                _audioInButton.Tag = "Inactivate";
                Controls.Remove(_audioInButton);
            }
        }

        protected void UpdateAudioOutButton()
        {
            if (_audioOutButton == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
            {
                Controls.Remove(_audioOutButton);
                return;
            }

            if (VideoWindow.Viewer.Visible && VideoWindow.Track == null && VideoWindow.Camera.Model.NumberOfAudioOut > 0
                && VideoWindow.Camera.CheckPermission(Permission.AudioOut))
            {
                //Console.WriteLine("IsAudioOut " + VideoWindow.Camera.IsAudioOut);
                if (VideoWindow.Camera.IsAudioOut)
                {
                    _audioOutButton.BackgroundImage = _audiooutactivate;
                    _audioOutButton.Tag = "Activate";
                }
                else
                {
                    _audioOutButton.BackgroundImage = _audioout;
                    _audioOutButton.Tag = "Inactivate";
                }

                SetButtonPosition(_audioOutButton);
                Controls.Add(_audioOutButton);
                _count++;
            }
            else
            {
                _audioOutButton.BackgroundImage = _audioout;
                _audioOutButton.Tag = "Inactivate";
                Controls.Remove(_audioOutButton);
                //_audioOutButton.BackgroundImage = _audioout;
                //_audioOutButton.Tag = "Inactivate";
            }
        }
    }
}