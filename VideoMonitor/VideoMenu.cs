using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class VideoMenu : Panel, IVideoMenu
    {
        public event EventHandler<EventArgs<String>> OnButtonClick;

        private VideoWindow _videoWindow;
        public virtual IVideoWindow VideoWindow
        {
            get { return _videoWindow; }
            set { _videoWindow = value as VideoWindow; }
        }

        private readonly System.Timers.Timer _hideMenuTimer = new System.Timers.Timer();
        public System.Timers.Timer HideMenuTimer
        {
            get { return _hideMenuTimer; }
        }

        public Point PanelPoint { get; set; }

        public IApp App { get; set; }
        public Boolean IsPressButton { get; set; }

        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (_server != null)
                    _hideMenuTimer.SynchronizingObject = _server.Form;
            }
        }

        public static Dictionary<String, String> Localization;

        public ToolMenuType MenuType { get; set; }

        static VideoMenu()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Menu_AudioIn", "Audio In"},
								   {"Menu_AudioOut", "Audio Out"},
								   {"Menu_Snapshot", "Snapshot"},
								   {"Menu_SaveImage", "Save Image"},
								   {"Menu_Stretch", "Stretch"},
								   {"Menu_Playback", "Playback"},
								   {"Menu_Disconnect", "Disconnect"},
								   {"Menu_OpticalPTZ", "Optical PTZ"},
								   {"Menu_Panic", "Panic Button"},
								   {"Menu_DigitalOutput1", "Digital Output 1"},
								   {"Menu_DigitalOutput2", "Digital Output 2"},
								   {"Menu_DigitalOutput3", "Digital Output 3"},
								   {"Menu_DigitalOutput4", "Digital Output 4"},
								   {"Menu_DigitalOutput5", "Digital Output 5"},
								   {"Menu_DigitalOutput6", "Digital Output 6"},
								   {"Menu_DigitalOutput7", "Digital Output 7"},
								   {"Menu_DigitalOutput8", "Digital Output 8"},
								   {"Menu_InstantPlayback", "Instant Playback"},
								   {"Menu_Reconnect", "Reconnect"},
								   {"Menu_ManualRecord", "Manual Record"},
								   {"Menu_Fisheye", "Fisheye"},
								   {"Menu_Dewarp", "Dewarp"},
								   {"Menu_VideoStream1", "Video Stream 1"},
								   {"Menu_VideoStream2", "Video Stream 2"},
								   {"Menu_VideoStream3", "Video Stream 3"},
								   {"Menu_VideoStream4", "Video Stream 4"},
								   {"Menu_VideoStream5", "Video Stream 5"},
								   {"Menu_VideoStream6", "Video Stream 6"},
							   };
            Localizations.Update(Localization);
        }

        public VideoMenu()
        {
            Anchor = AnchorStyles.None;// AnchorStyles.Bottom | AnchorStyles.Right;
            Size = new Size(30, 30);
            //MinimumSize = new Size(30, 30);
            //AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.FromArgb(42, 45, 50);//White
            Visible = false;

            _hideMenuTimer.Elapsed += HideMenu;
            _hideMenuTimer.Interval = 500;
        }

        public virtual void GenerateInstantPlaybackToolMenu()
        {
            _audioInButton = CreateMenuButton("Audio In", _audioin);
            _audioInButton.Tag = "Inactivate";
            _audioInButton.MouseClick -= ButtonMouseClick;
            _audioInButton.MouseClick += AudioInMouseClick;

            _snapshotButton = CreateMenuButton("Snapshot", _snapshot);
            _snapshotButton.MouseClick -= ButtonMouseClick;
            _snapshotButton.MouseClick += SnapshotButtonMouseClick;

            _saveImageButton = CreateMenuButton("Save Image", _saveImage);
            _saveImageButton.MouseClick -= ButtonMouseClick;
            _saveImageButton.MouseClick += SaveImageButtonMouseClick;

            _stretchButton = CreateMenuButton("Stretch", _stretch, _stretchActivate);
            _stretchButton.MouseClick -= ButtonMouseClick;
            _stretchButton.MouseClick += StretchButtonMouseClick;

            _dewarpButton = CreateMenuButton("Dewarp", _fisheyeActivate);
            _dewarpButton.MouseClick -= ButtonMouseClick;
            _dewarpButton.MouseClick += DewarpButtonMouseClick;

            _playbackButton = CreateMenuButton("Playback", _playback);
            //_playbackButton.Visible = false;

            while (Controls.Count >= 1)
                Controls.Remove(Controls[0]);

            MenuType = ToolMenuType.InstantPlayback;

            //Controls.Add(_playbackButton);
            Controls.Add(_dewarpButton);
            Controls.Add(_stretchButton);
            Controls.Add(_saveImageButton);
            Controls.Add(_snapshotButton);
            Controls.Add(_audioInButton);
        }

        public virtual void GeneratePlaybackToolMenu()
        {
            _audioInButton = CreateMenuButton("Audio In", _audioin);
            _audioInButton.Tag = "Inactivate";
            _audioInButton.MouseClick -= ButtonMouseClick;
            _audioInButton.MouseClick += AudioInMouseClick;

            _snapshotButton = CreateMenuButton("Snapshot", _snapshot);
            _snapshotButton.MouseClick -= ButtonMouseClick;
            _snapshotButton.MouseClick += SnapshotButtonMouseClick;

            _saveImageButton = CreateMenuButton("Save Image", _saveImage);
            _saveImageButton.MouseClick -= ButtonMouseClick;
            _saveImageButton.MouseClick += SaveImageButtonMouseClick;

            _stretchButton = CreateMenuButton("Stretch", _stretch, _stretchActivate);
            _stretchButton.MouseClick -= ButtonMouseClick;
            _stretchButton.MouseClick += StretchButtonMouseClick;

            _dewarpButton = CreateMenuButton("Dewarp", _fisheyeActivate);
            _dewarpButton.MouseClick -= ButtonMouseClick;
            _dewarpButton.MouseClick += DewarpButtonMouseClick;

            _disconnectButton = CreateMenuButton("Disconnect", _disconnect);

            while (Controls.Count >= 1)
                Controls.Remove(Controls[0]);

            MenuType = ToolMenuType.PlaybackVideo;

            Controls.Add(_disconnectButton);
            Controls.Add(_dewarpButton);
            Controls.Add(_stretchButton);
            Controls.Add(_saveImageButton);
            Controls.Add(_snapshotButton);
            Controls.Add(_audioInButton);
        }

        public void GenerateSmartSearchToolMenu()
        {
            _audioInButton = CreateMenuButton("Audio In", _audioin);
            _audioInButton.Tag = "Inactivate";
            _audioInButton.MouseClick -= ButtonMouseClick;
            _audioInButton.MouseClick += AudioInMouseClick;

            _snapshotButton = CreateMenuButton("Snapshot", _snapshot);
            _snapshotButton.MouseClick -= ButtonMouseClick;
            _snapshotButton.MouseClick += SnapshotButtonMouseClick;

            _saveImageButton = CreateMenuButton("Save Image", _saveImage);
            _saveImageButton.MouseClick -= ButtonMouseClick;
            _saveImageButton.MouseClick += SaveImageButtonMouseClick;

            _stretchButton = CreateMenuButton("Stretch", _stretch, _stretchActivate);
            _stretchButton.MouseClick -= ButtonMouseClick;
            _stretchButton.MouseClick += StretchButtonMouseClick;

            _playbackButton = CreateMenuButton("Playback", _playback);

            _dewarpButton = CreateMenuButton("Dewarp", _fisheyeActivate);
            _dewarpButton.MouseClick -= ButtonMouseClick;
            _dewarpButton.MouseClick += DewarpButtonMouseClick;

            _disconnectButton = CreateMenuButton("Disconnect", _disconnect);

            while (Controls.Count >= 1)
                Controls.Remove(Controls[0]);

            MenuType = ToolMenuType.SmartSearch;

            Controls.Add(_disconnectButton);
            Controls.Add(_dewarpButton);
            Controls.Add(_playbackButton);
            Controls.Add(_stretchButton);
            Controls.Add(_saveImageButton);
            Controls.Add(_snapshotButton);
            Controls.Add(_audioInButton);
        }

        public void GeneratePresetPointToolMenu()
        {
            //_reconnectButton = CreateMenuButton("Reconnect", (Bitmap)_reconnect);
            //_reconnectButton.MouseClick -= ButtonMouseClick;
            //_reconnectButton.MouseClick += ReconnectButtonMouseClick;

            _opticalPTZButton = CreateMenuButton("Optical PTZ", _opticalptz);
            _opticalPTZButton.Tag = "Inactivate";
            _opticalPTZButton.MouseClick -= ButtonMouseClick;
            _opticalPTZButton.MouseClick += OpticalPTZButtonMouseClick;

            _stretchButton = CreateMenuButton("Stretch", _stretch, _stretchActivate);
            _stretchButton.MouseClick -= ButtonMouseClick;
            _stretchButton.MouseClick += StretchButtonMouseClick;

            while (Controls.Count >= 1)
                Controls.Remove(Controls[0]);

            MenuType = ToolMenuType.PresetPoint;

            Controls.Add(_stretchButton);
            Controls.Add(_opticalPTZButton);
            //Controls.Add(_reconnectButton);
        }

        public virtual void GenerateLiveToolMenu()
        {
            _audioInButton = CreateMenuButton("Audio In", _audioin);
            _audioInButton.Tag = "Inactivate";
            _audioInButton.MouseClick -= ButtonMouseClick;
            _audioInButton.MouseClick += AudioInMouseClick;

            _audioOutButton = CreateMenuButton("Audio Out", _audioout);
            _audioOutButton.Tag = "Inactivate";
            _audioOutButton.MouseClick -= ButtonMouseClick;
            _audioOutButton.MouseDown += AudioOutButtonMouseDown;
            _audioOutButton.MouseUp += AudioOutButtonMouseUp;

            _do1Button = CreateMenuButton("Digital Output 1", _digitaloutput1);
            _do1Button.Tag = "Inactivate";
            _do1Button.MouseClick -= ButtonMouseClick;
            _do1Button.MouseClick += Do1MouseClick;

            _do2Button = CreateMenuButton("Digital Output 2", _digitaloutput2);
            _do2Button.Tag = "Inactivate";
            _do2Button.MouseClick -= ButtonMouseClick;
            _do2Button.MouseClick += Do2MouseClick;

            _do3Button = CreateMenuButton("Digital Output 3", _digitaloutput3);
            _do3Button.Tag = "Inactivate";
            _do3Button.MouseClick -= ButtonMouseClick;
            _do3Button.MouseClick += Do3MouseClick;

            _do4Button = CreateMenuButton("Digital Output 4", _digitaloutput4);
            _do4Button.Tag = "Inactivate";
            _do4Button.MouseClick -= ButtonMouseClick;
            _do4Button.MouseClick += Do4MouseClick;

            _do5Button = CreateMenuButton("Digital Output 5", _digitaloutput5);
            _do5Button.Tag = "Inactivate";
            _do5Button.MouseClick -= ButtonMouseClick;
            _do5Button.MouseClick += Do5MouseClick;

            _do6Button = CreateMenuButton("Digital Output 6", _digitaloutput6);
            _do6Button.Tag = "Inactivate";
            _do6Button.MouseClick -= ButtonMouseClick;
            _do6Button.MouseClick += Do6MouseClick;

            _do7Button = CreateMenuButton("Digital Output 7", _digitaloutput7);
            _do7Button.Tag = "Inactivate";
            _do7Button.MouseClick -= ButtonMouseClick;
            _do7Button.MouseClick += Do7MouseClick;

            _do8Button = CreateMenuButton("Digital Output 8", _digitaloutput8);
            _do8Button.Tag = "Inactivate";
            _do8Button.MouseClick -= ButtonMouseClick;
            _do8Button.MouseClick += Do8MouseClick;

            _videoStream1Button = CreateMenuButton("Video Stream 1", _videostream1);
            _videoStream1Button.Tag = "Inactivate";
            _videoStream1Button.MouseClick -= ButtonMouseClick;
            _videoStream1Button.MouseClick += VideoStream1ButtonMouseClick;

            _videoStream2Button = CreateMenuButton("Video Stream 2", _videostream2);
            _videoStream2Button.Tag = "Inactivate";
            _videoStream2Button.MouseClick -= ButtonMouseClick;
            _videoStream2Button.MouseClick += VideoStream2ButtonMouseClick;

            _videoStream3Button = CreateMenuButton("Video Stream 3", _videostream3);
            _videoStream3Button.Tag = "Inactivate";
            _videoStream3Button.MouseClick -= ButtonMouseClick;
            _videoStream3Button.MouseClick += VideoStream3ButtonMouseClick;

            _videoStream4Button = CreateMenuButton("Video Stream 4", _videostream4);
            _videoStream4Button.Tag = "Inactivate";
            _videoStream4Button.MouseClick -= ButtonMouseClick;
            _videoStream4Button.MouseClick += VideoStream4ButtonMouseClick;

            _videoStream5Button = CreateMenuButton("Video Stream 5", _videostream5);
            _videoStream5Button.Tag = "Inactivate";
            _videoStream5Button.MouseClick -= ButtonMouseClick;
            _videoStream5Button.MouseClick += VideoStream5ButtonMouseClick;

            _videoStream6Button = CreateMenuButton("Video Stream 6", _videostream6);
            _videoStream6Button.Tag = "Inactivate";
            _videoStream6Button.MouseClick -= ButtonMouseClick;
            _videoStream6Button.MouseClick += VideoStream6ButtonMouseClick;

            _instantplaybackButton = CreateMenuButton("Instant Playback", _instantplayback);
            _instantplaybackButton.Tag = "Inactivate";
            _instantplaybackButton.MouseClick -= ButtonMouseClick;
            _instantplaybackButton.MouseClick += InstantPlaybackButtonMouseClick;

            _playbackButton = CreateMenuButton("Playback", _playback);

            _reconnectButton = CreateMenuButton("Reconnect", _reconnect);
            _reconnectButton.MouseClick -= ButtonMouseClick;
            _reconnectButton.MouseClick += ReconnectButtonMouseClick;

            _opticalPTZButton = CreateMenuButton("Optical PTZ", _opticalptz);
            _opticalPTZButton.Tag = "Inactivate";
            _opticalPTZButton.MouseClick -= ButtonMouseClick;
            _opticalPTZButton.MouseClick += OpticalPTZButtonMouseClick;

            _snapshotButton = CreateMenuButton("Snapshot", _snapshot);
            _snapshotButton.MouseClick -= ButtonMouseClick;
            _snapshotButton.MouseClick += SnapshotButtonMouseClick;

            _saveImageButton = CreateMenuButton("Save Image", _saveImage);
            _saveImageButton.MouseClick -= ButtonMouseClick;
            _saveImageButton.MouseClick += SaveImageButtonMouseClick;

            _stretchButton = CreateMenuButton("Stretch", _stretch, _stretchActivate);
            _stretchButton.MouseClick -= ButtonMouseClick;
            _stretchButton.MouseClick += StretchButtonMouseClick;

            _manualRecordButton = CreateMenuButton("Manual Record", _record);
            _manualRecordButton.MouseClick -= ButtonMouseClick;
            _manualRecordButton.MouseClick += ManualRecordButtonMouseClick;

            _dewarpButton = CreateMenuButton("Dewarp", _fisheyeActivate);
            _dewarpButton.MouseClick -= ButtonMouseClick;
            _dewarpButton.MouseClick += DewarpButtonMouseClick;

            _panicButton = CreateMenuButton("Panic", _panicActive);
            _panicButton.MouseClick -= ButtonMouseClick;
            _panicButton.MouseClick += PanicButtonMouseClick;

            _disconnectButton = CreateMenuButton("Disconnect", _disconnect);

            //_playbackButton.Visible = false;

            while (Controls.Count >= 1)
                Controls.Remove(Controls[0]);

            MenuType = ToolMenuType.LiveVideo;

            Controls.Add(_disconnectButton);
            Controls.Add(_panicButton);
            Controls.Add(_dewarpButton);
            Controls.Add(_manualRecordButton);
            Controls.Add(_videoStream6Button);
            Controls.Add(_videoStream5Button);
            Controls.Add(_videoStream4Button);
            Controls.Add(_videoStream3Button);
            Controls.Add(_videoStream2Button);
            Controls.Add(_videoStream1Button);
            //Controls.Add(_playbackButton);
            Controls.Add(_stretchButton);
            Controls.Add(_saveImageButton);
            Controls.Add(_snapshotButton);
            Controls.Add(_opticalPTZButton);
            Controls.Add(_do8Button);
            Controls.Add(_do7Button);
            Controls.Add(_do6Button);
            Controls.Add(_do5Button);
            Controls.Add(_do4Button);
            Controls.Add(_do3Button);
            Controls.Add(_do2Button);
            Controls.Add(_do1Button);
            Controls.Add(_audioOutButton);
            Controls.Add(_audioInButton);
            Controls.Add(_reconnectButton);
            Controls.Add(_instantplaybackButton);
        }

        public void GeneratorCommandCenterToolMenu()
        {
            _instantplaybackButton = CreateMenuButton("Instant Playback", _instantplayback);
            _instantplaybackButton.Tag = "Inactivate";
            _instantplaybackButton.MouseClick -= ButtonMouseClick;
            _instantplaybackButton.MouseClick += InstantPlaybackButtonMouseClick;

            _stretchButton = CreateMenuButton("Stretch", _stretch, _stretchActivate);
            _stretchButton.MouseClick -= ButtonMouseClick;
            _stretchButton.MouseClick += StretchButtonMouseClick;

            _disconnectButton = CreateMenuButton("Disconnect", _disconnect);

            while (Controls.Count >= 1)
                Controls.Remove(Controls[0]);

            MenuType = ToolMenuType.CommandCenter;

            Controls.Add(_disconnectButton);
            Controls.Add(_stretchButton);
            Controls.Add(_instantplaybackButton);
        }

        public void GenerateInterrogationToolMenu()
        {
            while (Controls.Count >= 1)
                Controls.Remove(Controls[0]);

            MenuType = ToolMenuType.Interrogation;
        }

        private Boolean _checked;
        public Boolean HasPlaybackPage;
        public void CheckPermission()
        {
            if (_checked) return;
            _checked = true;
            if (HasPlaybackPage) return;

            if (_instantplaybackButton != null)
                Controls.Remove(_instantplaybackButton);

            if (_playbackButton != null)
                Controls.Remove(_playbackButton);

            _instantplaybackButton = null;
            _playbackButton = null;
        }

        public delegate void HideMenuDelegate(Object sender, EventArgs e);
        protected void HideMenu(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new HideMenuDelegate(HideMenu), sender, e);
                return;
            }

            _hideMenuTimer.Enabled = false;
            Visible = false;

            if (VideoWindow == null) return;
            if (VideoWindow.Viewer == null) return;
            VideoWindow.Viewer.ShowRIPWindow(false);
        }

        private static String SelectionChangeXml(String button, String status)
        {
            var xmlDoc = new XmlDocument();

            XmlElement xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Button", button));
            xmlRoot.AppendChild(xmlDoc.CreateXmlElementWithText("Status", status));

            return xmlDoc.InnerXml;
        }

        protected virtual void ButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (VideoWindow == null) return;

            if (OnButtonClick != null)
                OnButtonClick(this, new EventArgs<String>(SelectionChangeXml(((Control)sender).Name, "")));
        }

        protected const UInt16 ButtonHeight = 30;
        protected const UInt16 ButtonWidth = 30;
        protected Button CreateMenuButton(String name, Image image)
        {
            var button = new Button
            {
                Name = name,
                Size = new Size(ButtonWidth, ButtonHeight),
                Dock = DockStyle.None,
                //Dock = DockStyle.Top,
                Location = new Point(0, 0),
                FlatStyle = FlatStyle.Flat,
                BackgroundImage = image,
                BackgroundImageLayout = ImageLayout.Center,
                Cursor = Cursors.Hand,
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.CheckedBackColor = Color.Transparent;
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;

            String key = "Menu_" + name.Replace(" ", "");

            SharedToolTips.SharedToolTip.SetToolTip(button, (Localization.ContainsKey(key)) ? Localization[key] : name);

            button.MouseClick += ButtonMouseClick;
            return button;
        }

        protected ToggleButton CreateMenuButton(String name, Image inactiveImage, Image activeImage, bool active = false)
        {
            var key = "Menu_" + name.Replace(" ", "");
            var toolTip = Localization.ContainsKey(key) ? Localization[key] : name;

            var button = new ToggleButton()
            {
                Name = name,
                Size = new Size(ButtonWidth, ButtonHeight),
                Dock = DockStyle.None,
                Location = new Point(0, 0),
                ToolTipText = toolTip,
                ActiveBackgroundImage = activeImage,
                InactiveBackgroundImage = inactiveImage,
                Active = active
            };

            button.MouseClick += ButtonMouseClick;
            return button;
        }

        protected UInt16 _count;

        protected void SetButtonPosition(Button button)
        {
            //vertical
            button.Location = new Point(0, ButtonHeight * _count);
            //horizon
            //button.Location = new Point(ButtonWidth * _count, 0);
        }

        public void CheckStatus()
        {
            if (VideoWindow == null) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(CheckStatus));
                return;
            }

            _count = 0;

            UpdateButtonStatus();

            if (_disconnectButton != null && VideoWindow.Camera != null)
            {
                SetButtonPosition(_disconnectButton);
                Controls.Add(_disconnectButton);
            }

            if (App.PageActivated != null)
            {
                switch (App.PageActivated.Name)
                {
                    case "Interrogation":
                        Height = 0;
                        break;
                    default:
                        //vertical
                        Height = Controls.Count * ButtonHeight;
                        break;
                }
            }

            //horizon
            //Width = Controls.Count * ButtonWidth;

            BringToFront();
            Refresh();
        }

        protected virtual void UpdateButtonStatus()
        {
            UpdateInstantPlaybackButton();
            UpdateReconnectButton();
            UpdateAudioInButton();
            UpdateAudioOutButton();
            UpdateDo1Button();
            UpdateDo2Button();
            UpdateDo3Button();
            UpdateDo4Button();
            UpdateDo5Button();
            UpdateDo6Button();
            UpdateDo7Button();
            UpdateDo8Button();
            UpdateVideoStream1Button();
            UpdateVideoStream2Button();
            UpdateVideoStream3Button();
            UpdateVideoStream4Button();
            UpdateVideoStream5Button();
            UpdateVideoStream6Button();
            UpdateOpticalPTZButton();
            UpdateSnapshotButton();
            UpdateSaveImageButton();
            UpdateStretchButton();
            UpdatePlaybackButton();
            UpdateManualRecordButton();
            UpdateDewarpButton();
            UpdatePanicButton();
        }

        public Boolean UserDefineLocation;
        public virtual void UpdateLocation()
        {
            if (VideoWindow == null || VideoWindow.Parent == null || UserDefineLocation) return;
            if (Controls.Count == 0) return;

            var parentHeight = VideoWindow.Parent.Height;

            //Vertical on top-right
            if (parentHeight - VideoWindow.Location.Y - (VideoWindow.Viewer.Visible && VideoWindow.DisplayTitleBar ? 17 : 0) > Height)
            {
                Location = new Point(VideoWindow.Location.X + VideoWindow.Width - Width + PanelPoint.X - VideoWindow.Padding.Right,
                                     VideoWindow.Location.Y + PanelPoint.Y + (VideoWindow.Viewer.Visible && VideoWindow.DisplayTitleBar ? 17 : 0) + VideoWindow.Padding.Top);
            }
            else
            {
                Location = new Point(VideoWindow.Location.X + VideoWindow.Width - Width + PanelPoint.X - VideoWindow.Padding.Right,
                                     parentHeight + PanelPoint.Y - Height - VideoWindow.Padding.Bottom);
            }

            //horizon on bottom-left
            // 46 is VideoWindow's instantPlaybackDoubleBufferPanel HEIGHT

            //if (parentWidth - VideoWindow.Location.X > Width)
            //{
            //    Location = new Point(VideoWindow.Location.X + PanelPoint.X + VideoWindow.Padding.Left,
            //                         VideoWindow.Location.Y + PanelPoint.Y + VideoWindow.Height - Height - ((VideoWindow.Track != null) ? 46 : 0) - VideoWindow.Padding.Bottom);
            //}
            //else
            //{
            //    Location = new Point(parentWidth + PanelPoint.X - Width - VideoWindow.Padding.Right,
            //                         VideoWindow.Location.Y + PanelPoint.Y + VideoWindow.Height - Height - ((VideoWindow.Track != null) ? 46 : 0) - VideoWindow.Padding.Bottom);
            //}

            ChangeOpticalPTZButton();

            BringToFront();
            Refresh();
        }
    }
}