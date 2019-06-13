using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class VideoMenu
    {
        protected ToggleButton _stretchButton;
        private Button _dewarpButton;
        private Button _videoStream1Button;
        private Button _videoStream2Button;
        private Button _videoStream3Button;
        private Button _videoStream4Button;
        private Button _videoStream5Button;
        private Button _videoStream6Button;
        private Button _activateVideoStreamButton;

        protected static readonly Image _stretch = Resources.GetResources(Properties.Resources.stretch, Properties.Resources.IMGStretch);
        private static readonly Image _stretchActivate = Resources.GetResources(Properties.Resources.stretch_activate, Properties.Resources.IMGStretch_activate);
        private static readonly Image _fisheyeActivate = Resources.GetResources(Properties.Resources.fisheye_activate, Properties.Resources.IMGFisheye_activate);
        private static readonly Image _fisheyeActivate1 = Resources.GetResources(Properties.Resources.fisheye_activate1, Properties.Resources.IMGFisheye_activate1);
        private static readonly Image _fisheyeActivate2 = Resources.GetResources(Properties.Resources.fisheye_activate2, Properties.Resources.IMGFisheye_activate2);
        private static readonly Image _fisheye = Resources.GetResources(Properties.Resources.fisheye, Properties.Resources.IMGFisheye);
        private static readonly Image _videostream1 = Resources.GetResources(Properties.Resources.stream1, Properties.Resources.IMGStream1);
        private static readonly Image _videostream1Activate = Resources.GetResources(Properties.Resources.stream1_activate, Properties.Resources.IMGStream1_activate);
        private static readonly Image _videostream2 = Resources.GetResources(Properties.Resources.stream2, Properties.Resources.IMGStream2);
        private static readonly Image _videostream2Activate = Resources.GetResources(Properties.Resources.stream2_activate, Properties.Resources.IMGStream2_activate);
        private static readonly Image _videostream3 = Resources.GetResources(Properties.Resources.stream3, Properties.Resources.IMGStream3);
        private static readonly Image _videostream3Activate = Resources.GetResources(Properties.Resources.stream3_activate, Properties.Resources.IMGStream3_activate);
        private static readonly Image _videostream4 = Resources.GetResources(Properties.Resources.stream4, Properties.Resources.IMGStream4);
        private static readonly Image _videostream4Activate = Resources.GetResources(Properties.Resources.stream4_activate, Properties.Resources.IMGStream4_activate);
        private static readonly Image _videostream5 = Resources.GetResources(Properties.Resources.stream5, Properties.Resources.IMGStream5);
        private static readonly Image _videostream5Activate = Resources.GetResources(Properties.Resources.stream5_activate, Properties.Resources.IMGStream5_activate);
        private static readonly Image _videostream6 = Resources.GetResources(Properties.Resources.stream6, Properties.Resources.IMGStream6);
        private static readonly Image _videostream6Activate = Resources.GetResources(Properties.Resources.stream6_activate, Properties.Resources.IMGStream6_activate);

        protected void StretchButtonMouseClick(Object sender, MouseEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            if (toggleButton == null || VideoWindow == null) return;

            VideoWindow.Stretch = !VideoWindow.Stretch;

            Server.WriteOperationLog(string.Format("Device {0} Stretch {1}", VideoWindow.Camera.Id, VideoWindow.Stretch ? "On" : "Off"));
        }

        protected void DewarpButtonMouseClick(Object sender, MouseEventArgs e)
        {
            //DewarpType 0 = WALL,1 = CELLING,2 = GROUND 
            if (VideoWindow == null) return;

            if (VideoWindow.Dewarp && VideoWindow.DewarpType >= 2)
            {
                VideoWindow.Dewarp = false;
                _dewarpButton.BackgroundImage = _fisheye;
                VideoWindow.DewarpType = -1;

                SharedToolTips.SharedToolTip.SetToolTip(_dewarpButton, Localization["Menu_Dewarp"]);
            }
            else
            {
                VideoWindow.Dewarp = true;
                var type = VideoWindow.DewarpType;
                VideoWindow.DewarpType = (short)(type + 1);
                switch (VideoWindow.DewarpType)
                {
                    case 0:
                        _dewarpButton.BackgroundImage = _fisheyeActivate;
                        break;

                    case 1:
                        _dewarpButton.BackgroundImage = _fisheyeActivate1;
                        break;

                    case 2:
                        _dewarpButton.BackgroundImage = _fisheyeActivate2;
                        break;
                }

                SharedToolTips.SharedToolTip.SetToolTip(_dewarpButton, Localization["Menu_Fisheye"]);
            }

            VideoWindow.Viewer.InitFisheyeLibrary(VideoWindow.Dewarp, (short)(VideoWindow.Dewarp ? VideoWindow.DewarpType : 0));
        }

        private void VideoStreamClick(Button button, UInt16 id, Image activate)
        {
            if (VideoWindow == null) return;
            if (VideoWindow.Camera.Model == null) return;
            if (_activateVideoStreamButton == button) return; //same stream

            if (_activateVideoStreamButton == _videoStream1Button)
            {
                _videoStream1Button.Tag = "Inactivate";
                _videoStream1Button.BackgroundImage = _videostream1;
            }
            else if (_activateVideoStreamButton == _videoStream2Button)
            {
                _videoStream2Button.Tag = "Inactivate";
                _videoStream2Button.BackgroundImage = _videostream2;
            }
            else if (_activateVideoStreamButton == _videoStream3Button)
            {
                _videoStream3Button.Tag = "Inactivate";
                _videoStream3Button.BackgroundImage = _videostream3;
            }
            else if (_activateVideoStreamButton == _videoStream4Button)
            {
                _videoStream4Button.Tag = "Inactivate";
                _videoStream4Button.BackgroundImage = _videostream4;
            }
            else if (_activateVideoStreamButton == _videoStream5Button)
            {
                _videoStream5Button.Tag = "Inactivate";
                _videoStream5Button.BackgroundImage = _videostream5;
            }
            else if (_activateVideoStreamButton == _videoStream6Button)
            {
                _videoStream6Button.Tag = "Inactivate";
                _videoStream6Button.BackgroundImage = _videostream6;
            }

            if ((String.Equals(button.Tag.ToString(), "Inactivate")))
            {
                button.Tag = "Activate";
                button.BackgroundImage = activate;

                _activateVideoStreamButton = button;

                if (VideoWindow.Camera != null)
                {
                    VideoWindow.SwitchVideoStream(id);
                }
            }
        }

        private void VideoStream1ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            VideoStreamClick(_videoStream1Button, 1, _videostream1Activate);
        }

        private void VideoStream2ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            VideoStreamClick(_videoStream2Button, 2, _videostream2Activate);
        }

        private void VideoStream3ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            VideoStreamClick(_videoStream3Button, 3, _videostream3Activate);
        }

        private void VideoStream4ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            VideoStreamClick(_videoStream4Button, 4, _videostream4Activate);
        }

        private void VideoStream5ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            VideoStreamClick(_videoStream5Button, 5, _videostream5Activate);
        }

        private void VideoStream6ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            VideoStreamClick(_videoStream6Button, 6, _videostream6Activate);
        }

        private UInt16 StreamProfileCount()
        {
            UInt16 count = 0;
            foreach (var streamConfig in VideoWindow.Camera.Profile.StreamConfigs)
            {
                if (streamConfig.Value.Compression != Compression.Off || VideoWindow.Camera.Model.Manufacture == "Customization")
                    count++;
            }

            return count;
        }

        private UInt16 ReadCurrentViewerStreamId()
        {
            var id = VideoWindow.LiveVideoStreamId == 0
                            ? VideoWindow.Viewer.ProfileId == 0 ? VideoWindow.Camera.Profile.StreamId : VideoWindow.Viewer.ProfileId
                             : VideoWindow.LiveVideoStreamId;

            return id;
        }

        protected void UpdateVideoStream1Button()
        {
            if (_videoStream1Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null || Server is IPTS)
            {
                Controls.Remove(_videoStream1Button);
                return;
            }

            //if EnableBandwidthControl is enabled, user can't select stream
            if (VideoWindow.Camera.CMS != null)
            {
                if (VideoWindow.Camera.CMS.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream1Button);
                    return;
                }
            }
            else
            {
                if (VideoWindow.Camera.Server != null && VideoWindow.Camera.Server.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream1Button);
                    return;
                }
            }

            var hasStream1 = false;
            if (VideoWindow.Camera.Profile.StreamConfigs.ContainsKey(1) && StreamProfileCount() > 1 &&
               (VideoWindow.Camera.Profile.StreamConfigs[1].Compression != Compression.Off || VideoWindow.Camera.Model.Manufacture == "Customization"))
            {
                hasStream1 = true;
            }

            if (VideoWindow.PlayMode != PlayMode.LiveStreaming)
            {
                hasStream1 = false;
            }

            if (hasStream1)
            {
                var id = ReadCurrentViewerStreamId();

                if (id == 1)
                {
                    _videoStream1Button.BackgroundImage = _videostream1Activate;
                    _videoStream1Button.Tag = "Activate";
                    _activateVideoStreamButton = _videoStream1Button;
                }
                else
                {
                    _videoStream1Button.BackgroundImage = _videostream1;
                    _videoStream1Button.Tag = "Inactivate";
                }
                SetButtonPosition(_videoStream1Button);
                Controls.Add(_videoStream1Button);
                _count++;
            }
            else
            {
                Controls.Remove(_videoStream1Button);
            }
        }

        protected void UpdateVideoStream2Button()
        {
            if (_videoStream2Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null || Server is IPTS)
            {
                Controls.Remove(_videoStream2Button);
                return;
            }

            //if EnableBandwidthControl is enabled, user can't select stream
            if (VideoWindow.Camera.CMS != null)
            {
                if (VideoWindow.Camera.CMS.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream2Button);
                    return;
                }
            }
            else
            {
                if (VideoWindow.Camera.Server.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream2Button);
                    return;
                }
            }

            var hasStream2 = false;
            if (VideoWindow.Camera.Profile.StreamConfigs.ContainsKey(2) && StreamProfileCount() > 1
                && (VideoWindow.Camera.Profile.StreamConfigs[2].Compression != Compression.Off || VideoWindow.Camera.Model.Manufacture == "Customization"))
                hasStream2 = true;

            if (VideoWindow.PlayMode != PlayMode.LiveStreaming)
                hasStream2 = false;

            if (hasStream2)
            {
                var id = ReadCurrentViewerStreamId();

                if (id == 2)
                {
                    _videoStream2Button.BackgroundImage = _videostream2Activate;
                    _videoStream2Button.Tag = "Activate";
                    _activateVideoStreamButton = _videoStream2Button;
                }
                else
                {
                    _videoStream2Button.BackgroundImage = _videostream2;
                    _videoStream2Button.Tag = "Inactivate";
                }
                SetButtonPosition(_videoStream2Button);
                Controls.Add(_videoStream2Button);
                _count++;
            }
            else
            {
                Controls.Remove(_videoStream2Button);
            }
        }

        protected void UpdateVideoStream3Button()
        {
            if (_videoStream3Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null || Server is IPTS)
            {
                Controls.Remove(_videoStream3Button);
                return;
            }

            //if EnableBandwidthControl is enabled, user can't select stream
            if (VideoWindow.Camera.CMS != null)
            {
                if (VideoWindow.Camera.CMS.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream3Button);
                    return;
                }
            }
            else
            {
                if (VideoWindow.Camera.Server.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream3Button);
                    return;
                }
            }

            var hasStream3 = false;
            if (VideoWindow.Camera.Profile.StreamConfigs.ContainsKey(3) && StreamProfileCount() > 1
                && (VideoWindow.Camera.Profile.StreamConfigs[3].Compression != Compression.Off || VideoWindow.Camera.Model.Manufacture == "Customization"))
                hasStream3 = true;

            if (VideoWindow.PlayMode != PlayMode.LiveStreaming)
                hasStream3 = false;

            if (hasStream3)
            {
                var id = ReadCurrentViewerStreamId();

                if (id == 3)
                {
                    _videoStream3Button.BackgroundImage = _videostream3Activate;
                    _videoStream3Button.Tag = "Activate";
                    _activateVideoStreamButton = _videoStream3Button;
                }
                else
                {
                    _videoStream3Button.BackgroundImage = _videostream3;
                    _videoStream3Button.Tag = "Inactivate";
                }
                SetButtonPosition(_videoStream3Button);
                Controls.Add(_videoStream3Button);
                _count++;
            }
            else
            {
                Controls.Remove(_videoStream3Button);
            }
        }

        protected void UpdateVideoStream4Button()
        {
            if (_videoStream4Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null || Server is IPTS)
            {
                Controls.Remove(_videoStream4Button);
                return;
            }

            //if EnableBandwidthControl is enabled, user can't select stream
            if (VideoWindow.Camera.CMS != null)
            {
                if (VideoWindow.Camera.CMS.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream4Button);
                    return;
                }
            }
            else
            {
                if (VideoWindow.Camera.Server.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream4Button);
                    return;
                }
            }

            var hasStream4 = false;
            if (VideoWindow.Camera.Profile.StreamConfigs.ContainsKey(4) && StreamProfileCount() > 1
                && (VideoWindow.Camera.Profile.StreamConfigs[4].Compression != Compression.Off || VideoWindow.Camera.Model.Manufacture == "Customization"))
                hasStream4 = true;

            if (VideoWindow.PlayMode != PlayMode.LiveStreaming)
                hasStream4 = false;

            if (hasStream4)
            {
                var id = ReadCurrentViewerStreamId();

                if (id == 4)
                {
                    _videoStream4Button.BackgroundImage = _videostream4Activate;
                    _videoStream4Button.Tag = "Activate";
                    _activateVideoStreamButton = _videoStream4Button;
                }
                else
                {
                    _videoStream4Button.BackgroundImage = _videostream4;
                    _videoStream4Button.Tag = "Inactivate";
                }
                SetButtonPosition(_videoStream4Button);
                Controls.Add(_videoStream4Button);
                _count++;
            }
            else
            {
                Controls.Remove(_videoStream4Button);
            }
        }

        protected void UpdateVideoStream5Button()
        {
            if (_videoStream5Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null || Server is IPTS)
            {
                Controls.Remove(_videoStream5Button);
                return;
            }

            //if EnableBandwidthControl is enabled, user can't select stream
            if (VideoWindow.Camera.CMS != null)
            {
                if (VideoWindow.Camera.CMS.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream5Button);
                    return;
                }
            }
            else
            {
                if (VideoWindow.Camera.Server.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream5Button);
                    return;
                }
            }

            var hasStream5 = false;
            if (VideoWindow.Camera.Profile.StreamConfigs.ContainsKey(5) && StreamProfileCount() > 1
                && (VideoWindow.Camera.Profile.StreamConfigs[5].Compression != Compression.Off || VideoWindow.Camera.Model.Manufacture == "Customization"))
                hasStream5 = true;

            if (VideoWindow.PlayMode != PlayMode.LiveStreaming)
                hasStream5 = false;

            if (hasStream5)
            {
                var id = ReadCurrentViewerStreamId();

                if (id == 5)
                {
                    _videoStream5Button.BackgroundImage = _videostream5Activate;
                    _videoStream5Button.Tag = "Activate";
                    _activateVideoStreamButton = _videoStream5Button;
                }
                else
                {
                    _videoStream5Button.BackgroundImage = _videostream5;
                    _videoStream5Button.Tag = "Inactivate";
                }
                SetButtonPosition(_videoStream5Button);
                Controls.Add(_videoStream5Button);
                _count++;
            }
            else
            {
                Controls.Remove(_videoStream5Button);
            }
        }

        protected void UpdateVideoStream6Button()
        {
            if (_videoStream6Button == null) return;
            if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null || Server is IPTS)
            {
                Controls.Remove(_videoStream6Button);
                return;
            }

            //if EnableBandwidthControl is enabled, user can't select stream
            if (VideoWindow.Camera.CMS != null)
            {
                if (VideoWindow.Camera.CMS.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream6Button);
                    return;
                }
            }
            else
            {
                if (VideoWindow.Camera.Server.Configure.EnableBandwidthControl)
                {
                    Controls.Remove(_videoStream6Button);
                    return;
                }
            }

            var hasStream6 = false;
            if (VideoWindow.Camera.Profile.StreamConfigs.ContainsKey(6) && StreamProfileCount() > 1
                && (VideoWindow.Camera.Profile.StreamConfigs[6].Compression != Compression.Off || VideoWindow.Camera.Model.Manufacture == "Customization"))
                hasStream6 = true;

            if (VideoWindow.PlayMode != PlayMode.LiveStreaming)
                hasStream6 = false;

            if (hasStream6)
            {
                var id = ReadCurrentViewerStreamId();

                if (id == 6)
                {
                    _videoStream6Button.BackgroundImage = _videostream6Activate;
                    _videoStream6Button.Tag = "Activate";
                    _activateVideoStreamButton = _videoStream6Button;
                }
                else
                {
                    _videoStream6Button.BackgroundImage = _videostream6;
                    _videoStream6Button.Tag = "Inactivate";
                }
                SetButtonPosition(_videoStream6Button);
                Controls.Add(_videoStream6Button);
                _count++;
            }
            else
            {
                Controls.Remove(_videoStream6Button);
            }
        }

        protected void UpdateStretchButton()
        {
            if (_stretchButton == null || VideoWindow.Viewer == null) return;

            //if (VideoWindow.Viewer.Visible && VideoWindow.Camera != null && (VideoWindow.Camera.Model.Type != "AudioBox"))
            if (VideoWindow.Viewer.Visible && VideoWindow.Camera != null)
            {
                if ((VideoWindow.Camera.Model == null) || (VideoWindow.Camera.Model.Type != "AudioBox"))
                {
                    _stretchButton.Active = VideoWindow.Stretch;
                    SetButtonPosition(_stretchButton);
                    Controls.Add(_stretchButton);
                    _count++;
                }
                else
                {
                    Controls.Remove(_stretchButton);
                }
            }
            else
            {
                Controls.Remove(_stretchButton);
            }
        }

        protected void UpdateDewarpButton()
        {
            if (_dewarpButton == null) return;

            if (VideoWindow.Camera != null && VideoWindow.Camera.Profile != null && (!String.IsNullOrEmpty(VideoWindow.Camera.Profile.DewarpType) || VideoWindow.Camera.Model.Type == "fisheye"))
            {
                if (VideoWindow.Viewer.Visible)
                {
                    if (VideoWindow.Dewarp)
                    {
                        switch (VideoWindow.DewarpType)
                        {
                            case 0:
                                _dewarpButton.BackgroundImage = _fisheyeActivate;
                                break;

                            case 1:
                                _dewarpButton.BackgroundImage = _fisheyeActivate1;
                                break;

                            case 2:
                                _dewarpButton.BackgroundImage = _fisheyeActivate2;
                                break;
                        }
                        //_dewarpButton.BackgroundImage = _fisheyeActivate;
                        SharedToolTips.SharedToolTip.SetToolTip(_dewarpButton, Localization["Menu_Fisheye"]);
                    }
                    else
                    {
                        _dewarpButton.BackgroundImage = _fisheye;
                        SharedToolTips.SharedToolTip.SetToolTip(_dewarpButton, Localization["Menu_Dewarp"]);
                    }

                    SetButtonPosition(_dewarpButton);
                    Controls.Add(_dewarpButton);
                    _count++;
                }
                else
                {
                    Controls.Remove(_dewarpButton);
                }
            }
            else
            {
                Controls.Remove(_dewarpButton);
            }
        }
    }
}