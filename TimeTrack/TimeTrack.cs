using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant.Utility;

namespace TimeTrack
{
    public partial class TimeTrack : UserControl, IControl, IAppUse, IServerUse, IDrop, IMinimize, IKeyPress
    {
        public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<UInt64>> OnPlay;
        public event EventHandler<EventArgs<UInt64, UInt16>> OnPlayOnRate;
        public event EventHandler<EventArgs<String>> OnTimecodeChange;
        public event EventHandler OnNextFrame;
        public event EventHandler OnPreviousFrame;
        public event EventHandler OnEnablePlaybackSmooth;
        public event EventHandler<EventArgs<TimeUnit, UInt64[]>> OnTimeUnitChange;
        public event EventHandler<EventArgs<Object>> OnContentChange;
        public event EventHandler<EventArgs<String>> OnExportStartDateTimeChange;
        public event EventHandler<EventArgs<String>> OnExportEndDateTimeChange;
        public event EventHandler<EventArgs<Dictionary<IDevice, Record>>> OnRecordDataChange;

        private ITrackerContainer _trackerContainer;
        public virtual ITrackerContainer TrackerContainer
        {
            get
            {
                if (_trackerContainer == null)
                {
                    _trackerContainer = CreateTrackerContainer(MaxConnection, TrackerNumPerPage);
                    var trackerContainer = _trackerContainer as Control;
                    Debug.Assert(trackerContainer != null);
                    trackerContainer.MouseDown += TrackerContainerMouseDown;
                    _trackerContainer.OnTrackerContainerMouseDown += TrackerContainerMouseDown;// click on tracker
                }

                return _trackerContainer;
            }
        }

        protected virtual ITrackerContainer CreateTrackerContainer(ushort maxConnection, ushort trackerNumPerPage)
        {
            var trackerContainer = new TrackerContainer
            {
                MaxConnection = maxConnection,
                TrackerNumPerPage = trackerNumPerPage
            };

            return trackerContainer;
        }

        public DeviceContainer DeviceContainer;
        public Boolean IsMinimize { get; private set; }

        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public Button Icon { get; private set; }

        public IApp App { get; set; }

        public IServer Server { get; set; }

        public List<IVideoWindow> VideoWindows = new List<IVideoWindow>();

        private readonly TimeTrackController _timeTrackController = new TimeTrackController();
        protected virtual TimeTrackController TimeTrackController
        {
            get { return _timeTrackController; }
        }

        private static readonly Image _audioIn = Resources.GetResources(Properties.Resources.audioin, Properties.Resources.IMGAudioIn);
        private static readonly Image _audioInOn = Resources.GetResources(Properties.Resources.audioin_on, Properties.Resources.IMGAudioInOn);
        private static readonly Image _audioOut = Resources.GetResources(Properties.Resources.audioout, Properties.Resources.IMGAudioOut);
        private static readonly Image _audioOutOn = Resources.GetResources(Properties.Resources.audioout_on, Properties.Resources.IMGAudioOutOn);
        private static readonly Image _motion = Resources.GetResources(Properties.Resources.motion, Properties.Resources.IMGMotion);
        private static readonly Image _motionOn = Resources.GetResources(Properties.Resources.motion_on, Properties.Resources.IMGMotionOn);
        private static readonly Image _di = Resources.GetResources(Properties.Resources.di, Properties.Resources.IMGDI);
        private static readonly Image _diOn = Resources.GetResources(Properties.Resources.di_on, Properties.Resources.IMGDIOn);
        private static readonly Image _do = Resources.GetResources(Properties.Resources._do, Properties.Resources.IMGDO);
        private static readonly Image _doOn = Resources.GetResources(Properties.Resources.do_on, Properties.Resources.IMGDOOn);
        private static readonly Image _networkLoss = Resources.GetResources(Properties.Resources.networkloss, Properties.Resources.IMGNetworkLoss);
        private static readonly Image _networkLossOn = Resources.GetResources(Properties.Resources.networkloss_on, Properties.Resources.IMGNetworkLossOn);
        private static readonly Image _networkRecovery = Resources.GetResources(Properties.Resources.networkrecovery, Properties.Resources.IMGNetworkRecovery);
        private static readonly Image _networkRecoveryOn = Resources.GetResources(Properties.Resources.networkrecovery_on, Properties.Resources.IMGNetworkRecoveryOn);
        private static readonly Image _videoLoss = Resources.GetResources(Properties.Resources.videoloss, Properties.Resources.IMGVideoLoss);
        private static readonly Image _videoLossOn = Resources.GetResources(Properties.Resources.videoloss_on, Properties.Resources.IMGVideoLossOn);
        private static readonly Image _videoRecovery = Resources.GetResources(Properties.Resources.videorecovery, Properties.Resources.IMGVideoRecovery);
        private static readonly Image _videoRecoveryOn = Resources.GetResources(Properties.Resources.videorecovery_on, Properties.Resources.IMGVideoRecoveryOn);
        private static readonly Image _manualRecord = Resources.GetResources(Properties.Resources.manualrecord, Properties.Resources.IMGManualRecord);
        private static readonly Image _manualRecordOn = Resources.GetResources(Properties.Resources.manualrecord_on, Properties.Resources.IMGManualRecordOn);
        private static readonly Image _panic = Resources.GetResources(Properties.Resources.panic, Properties.Resources.IMGPanic);
        private static readonly Image _panicOn = Resources.GetResources(Properties.Resources.panic_on, Properties.Resources.IMGPanicOn);
        private static readonly Image _crossline = Resources.GetResources(Properties.Resources.crossline, Properties.Resources.IMGCrossLine);
        private static readonly Image _crosslineOn = Resources.GetResources(Properties.Resources.crossline_on, Properties.Resources.IMGCrossLineOn);

        private static readonly Image _intrusionDetection = Resources.GetResources(Properties.Resources.intrusionDetection, Properties.Resources.IMGIntrusionDetection);
        private static readonly Image _intrusionDetectionOn = Resources.GetResources(Properties.Resources.intrusionDetection_on, Properties.Resources.IMGIntrusionDetectionOn);
        private static readonly Image _loiteringDetection = Resources.GetResources(Properties.Resources.loiteringDetection, Properties.Resources.IMGLoiteringDetection);
        private static readonly Image _loiteringDetectionOn = Resources.GetResources(Properties.Resources.loiteringDetection_on, Properties.Resources.IMGLoiteringDetectionOn);
        private static readonly Image _objectCountingIn = Resources.GetResources(Properties.Resources.objectCountingIn, Properties.Resources.IMGObjectCountingIn);
        private static readonly Image _objectCountingInOn = Resources.GetResources(Properties.Resources.objectCountingIn_on, Properties.Resources.IMGObjectCountingInOn);
        private static readonly Image _objectCountingOut = Resources.GetResources(Properties.Resources.objectCountingOut, Properties.Resources.IMGObjectCountingOut);
        private static readonly Image _objectCountingOutOn = Resources.GetResources(Properties.Resources.objectCountingOut_on, Properties.Resources.IMGObjectCountingOutOn);
        private static readonly Image _audioDetection = Resources.GetResources(Properties.Resources.audioDetection, Properties.Resources.IMGAudioDetection);
        private static readonly Image _audioDetectionOn = Resources.GetResources(Properties.Resources.audioDetection_on, Properties.Resources.IMGAudioDetectionOn);
        private static readonly Image _temperingDetection = Resources.GetResources(Properties.Resources.temperingDetection, Properties.Resources.IMGTemperingDetection);
        private static readonly Image _temperingDetectionOn = Resources.GetResources(Properties.Resources.temperingDetection_on, Properties.Resources.IMGTemperingDetectionOn);

        private static readonly Image _userdefine = Resources.GetResources(Properties.Resources.userdefine, Properties.Resources.IMGUserDefine);
        private static readonly Image _userdefineOn = Resources.GetResources(Properties.Resources.userdefine_on, Properties.Resources.IMGUserDefineOn);

        private static readonly Image _right = Resources.GetResources(Properties.Resources.expand, Properties.Resources.IMGExpand);
        private static readonly Image _left = Resources.GetResources(Properties.Resources.left, Properties.Resources.IMGLeft);

        private static readonly Image _up = Resources.GetResources(Properties.Resources.up, Properties.Resources.IMGUp);
        private static readonly Image _upOn = Resources.GetResources(Properties.Resources.up_on, Properties.Resources.IMGUpOn);
        private static readonly Image _down = Resources.GetResources(Properties.Resources.down, Properties.Resources.IMGDown);
        private static readonly Image _downOn = Resources.GetResources(Properties.Resources.down_on, Properties.Resources.IMGDownOn);

        protected readonly System.Timers.Timer DateValueChangeTimer = new System.Timers.Timer();
        protected readonly System.Timers.Timer ShowLoadingTimer = new System.Timers.Timer();
        protected readonly List<TimeScale> ScaleList = new List<TimeScale>();
        protected UInt16 MaxConnection = 64;//TimeTrackSwitch

        private float _rate;
        public float Rate
        {
            get { return _rate; }
            set
            {
                if (value == 0)
                {
                    TimeTrackController.ActiveButton("Pause");
                }
                else
                {
                    if (value == 1)
                        TimeTrackController.ActiveButton("Play");
                    else if (value > 0)
                        TimeTrackController.ActiveButton("FastPlay");
                    else
                        TimeTrackController.ActiveButton("FastReverse");
                }

                if (_rate == value) return;

                _rate = value;

                TrackerContainer.Rate = _rate;

                if (value == 0)
                {
                    TrackerContainer.Stop();
                }
                else if (value.Equals(1))
                {
                    if (UsingDevices.Any(device => device is IDeviceLayout || device is ISubLayout))
                    {
                        TrackerContainer.PlayOnRate();
                    }
                }
                else
                {
                    TrackerContainer.PlayOnRate();
                }
            }
        }

        protected const Int16 ScalePosition1 = 18; //1sec
        protected const Int16 ScalePosition2 = 34; //10 secs
        protected const Int16 ScalePosition3 = 50; //1min
        protected const Int16 ScalePosition4 = 66; //10 mins
        protected const Int16 ScalePosition5 = 82; //1 hr
        protected const Int16 ScalePosition6 = 98; //1 day

        private bool _recordButtonVisible = true;
        private TimeScale _currentlyScale;
        protected TimeScale CurrentlyScale
        {
            get
            {
                return _currentlyScale;
            }
            set
            {
                _currentlyScale = value;
                int x = ScalePosition2; //default
                switch (value)
                {
                    case TimeScale.Scale1Second:
                        TrackerContainer.UnitTime = TimeUnit.Unit1Senond;
                        x = ScalePosition1;
                        break;

                    case TimeScale.Scale10Seconds:
                        TrackerContainer.UnitTime = TimeUnit.Unit10Senonds;
                        x = ScalePosition2;
                        break;

                    case TimeScale.Scale1Minute:
                        TrackerContainer.UnitTime = TimeUnit.Unit1Minute;
                        x = ScalePosition3;
                        break;

                    case TimeScale.Scale10Minutes:
                        TrackerContainer.UnitTime = TimeUnit.Unit10Minutes;
                        x = ScalePosition4;
                        break;

                    case TimeScale.Scale1Hour:
                        TrackerContainer.UnitTime = TimeUnit.Unit1Hour;
                        x = ScalePosition5;
                        break;

                    //case TimeScale.Scale4Hours:
                    //    TrackerContainer.UnitTime = TimeUnit.Unit4Hours;
                    //    x = ScalePosition6;
                    //    break;

                    case TimeScale.Scale1Day:
                        TrackerContainer.UnitTime = TimeUnit.Unit1Day;
                        x = ScalePosition6;
                        break;
                }

                scaleButton.Location = new Point(x, scaleButton.Location.Y);
            }
        }


        // Constructor
        public TimeTrack()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Common_Sec", "sec"},
                                   {"Common_Min", "min"},
                                   {"Common_Hr", "hr"},
                                   {"Common_Day", "day"},

                                   {"App_Loading", "Loading"},

                                   {"MessageBox_Confirm", "Confirm"},
                                   {"MessageBox_Information", "Information"},

                                   {"PageSelector_Prev", "Previous Page"},
                                   {"PageSelector_Next", "Next Page"},

                                   {"TimeTrack_NoMoreRecord", "No other records"},
                                   {"TimeTrack_NoMoreBookmarks", "No other bookmarks"},
                                   {"TimeTrack_AddBookmark", "Add Bookmark"},
                                   {"TimeTrack_ClearBookmark", "Clear Bookmark"},
                                   {"TimeTrack_ClearSelectionRange", "Clear selection range"},
                                   {"TimeTrack_SetStartPoint", "Set start point"},
                                   {"TimeTrack_SetEndPoint", "Set end point"},
                                   {"TimeTrack_IncreaseTimeScale", "Increase time scale"},
                                   {"TimeTrack_DecreaseTimeScale", "Decrease time scale"},
                                   {"TimeTrack_JumpNextBookmark", "Jump to next bookmark"},
                                   {"TimeTrack_JumpPreviousBookmark", "Jump to previous bookmark"},
                                   {"TimeTrack_JumpNextRecord", "Jump to next record"},
                                   {"TimeTrack_JumpPreviousRecord", "Jump to previous record"},
                                   {"TimeTrack_JumpPresentTime", "Jump to present time"},
                                   {"TimeTrack_JumpBeginRecordingTime","Jump to begin record"},
                                   {"TimeTrack_EnablePlaybackSmoothly","Enable Playback Smoothly"},
                                   {"Menu_AudioIn", "Audio In"},
                                   {"Menu_AudioOut", "Audio Out"},

                                   {"Event_Motion", "Motion"},
                                   {"Event_DigitalInput", "Digital Input"},
                                   {"Event_DigitalOutput", "Digital Output"},
                                   {"Event_NetworkLoss", "Network Lost"},
                                   {"Event_NetworkRecovery", "Network Recovery"},
                                   {"Event_VideoLoss", "Video Lost"},
                                   {"Event_VideoRecovery", "Video Recovery"},
                                   {"Event_ManualRecord", "Manual Record"},
                                   {"Event_Panic", "Panic Button"},
                                   {"Event_CrossLine", "Cross Line"},
                                   {"Event_IntrusionDetection", "Intrusion Detection"},
                                   {"Event_LoiteringDetection", "Loitering Detection"},
                                   {"Event_ObjectCountingIn", "Object Counting In"},
                                   {"Event_ObjectCountingOut", "Object Counting Out"},
                                   {"Event_AudioDetection", "Audio Detection"},
                                   {"Event_TamperDetection", "Tamper Detection"},
                                   {"Event_UserDefined", "User Defined"},

                                   {"TimeTrack_DeleteBookmarkConfirm", "Are you sure you want to delete ALL selected device's bookmarks?"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            Dock = DockStyle.Fill;
            DoubleBuffered = true;

            loadingLabel.Text = Localization["App_Loading"];

            SharedToolTips.SharedToolTip.SetToolTip(addBookmarkButton, Localization["TimeTrack_AddBookmark"]);
            SharedToolTips.SharedToolTip.SetToolTip(eraserBookmarkButton, Localization["TimeTrack_ClearBookmark"]);
            SharedToolTips.SharedToolTip.SetToolTip(clearSelectionButton, Localization["TimeTrack_ClearSelectionRange"]);
            SharedToolTips.SharedToolTip.SetToolTip(setStartButton, Localization["TimeTrack_SetStartPoint"]);
            SharedToolTips.SharedToolTip.SetToolTip(setEndButton, Localization["TimeTrack_SetEndPoint"]);
            SharedToolTips.SharedToolTip.SetToolTip(plusButton, Localization["TimeTrack_IncreaseTimeScale"]);
            SharedToolTips.SharedToolTip.SetToolTip(minusButton, Localization["TimeTrack_DecreaseTimeScale"]);
            SharedToolTips.SharedToolTip.SetToolTip(nextBookmarkButton, Localization["TimeTrack_JumpNextBookmark"]);
            SharedToolTips.SharedToolTip.SetToolTip(previousBookmarkButton, Localization["TimeTrack_JumpPreviousBookmark"]);
            SharedToolTips.SharedToolTip.SetToolTip(nextRecordButton, Localization["TimeTrack_JumpNextRecord"]);
            SharedToolTips.SharedToolTip.SetToolTip(previousRecordButton, Localization["TimeTrack_JumpPreviousRecord"]);
            SharedToolTips.SharedToolTip.SetToolTip(gotoCurrentButton, Localization["TimeTrack_JumpPresentTime"]);
            SharedToolTips.SharedToolTip.SetToolTip(gotoBeginButton, Localization["TimeTrack_JumpBeginRecordingTime"]);

            InitializeEventButton(audioInButton, EventType.AudioIn, _audioIn, _audioInOn, Localization["Menu_AudioIn"]);
            InitializeEventButton(audioOutButton, EventType.AudioOut, _audioOut, _audioOutOn, Localization["Menu_AudioOut"]);
            InitializeEventButton(motionButton, EventType.Motion, _motion, _motionOn, Localization["Event_Motion"]);
            InitializeEventButton(manualRecordButton, EventType.ManualRecord, _manualRecord, _manualRecordOn, Localization["Event_ManualRecord"]);
            InitializeEventButton(diButton, EventType.DigitalInput, _di, _diOn, Localization["Event_DigitalInput"]);
            InitializeEventButton(doButton, EventType.DigitalOutput, _do, _doOn, Localization["Event_DigitalOutput"]);
            InitializeEventButton(networkLossButton, EventType.NetworkLoss, _networkLoss, _networkLossOn, Localization["Event_NetworkLoss"]);
            InitializeEventButton(networkRecoveryButton, EventType.NetworkRecovery, _networkRecovery, _networkRecoveryOn, Localization["Event_NetworkRecovery"]);
            InitializeEventButton(videoLossButton, EventType.VideoLoss, _videoLoss, _videoLossOn, Localization["Event_VideoLoss"]);
            InitializeEventButton(videoRecoveryButton, EventType.VideoRecovery, _videoRecovery, _videoRecoveryOn, Localization["Event_VideoRecovery"]);
            InitializeEventButton(panicButton, EventType.Panic, _panic, _panicOn, Localization["Event_Panic"]);
            InitializeEventButton(crossLineButton, EventType.CrossLine, _crossline, _crosslineOn, Localization["Event_CrossLine"]);

            InitializeEventButton(instrusionDetectionButton, EventType.IntrusionDetection, _intrusionDetection, _intrusionDetectionOn, Localization["Event_IntrusionDetection"]);
            InitializeEventButton(loiteringDetectionButton, EventType.LoiteringDetection, _loiteringDetection, _loiteringDetectionOn, Localization["Event_LoiteringDetection"]);
            InitializeEventButton(objectCountingInButton, EventType.ObjectCountingIn, _objectCountingIn, _objectCountingInOn, Localization["Event_ObjectCountingIn"]);
            InitializeEventButton(objectCountingOutButton, EventType.ObjectCountingOut, _objectCountingOut, _objectCountingOutOn, Localization["Event_ObjectCountingOut"]);
            InitializeEventButton(audioDetectionButton, EventType.AudioDetection, _audioDetection, _audioDetectionOn, Localization["Event_AudioDetection"]);
            InitializeEventButton(temperingDetectionButton, EventType.TamperDetection, _temperingDetection, _temperingDetectionOn, Localization["Event_TamperDetection"]);
            InitializeEventButton(userdefineButton, EventType.UserDefine, _userdefine, _userdefineOn, Localization["Event_UserDefined"]);

            SharedToolTips.SharedToolTip.SetToolTip(upButton, Localization["PageSelector_Prev"]);
            SharedToolTips.SharedToolTip.SetToolTip(downButton, Localization["PageSelector_Next"]);

            toolPanel.BackgroundImage = Resources.GetResources(Properties.Resources.banner, Properties.Resources.BGBanner);
            controllerPanel.BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.BGControllerBG);
            addBookmarkButton.BackgroundImage = Resources.GetResources(Properties.Resources.bookmark, Properties.Resources.IMGBookmark);
            eraserBookmarkButton.BackgroundImage = Resources.GetResources(Properties.Resources.eraser, Properties.Resources.IMGEraser);
            nextBookmarkButton.BackgroundImage = Resources.GetResources(Properties.Resources.next_bookmark, Properties.Resources.IMGNext_bookmark);
            previousBookmarkButton.BackgroundImage = Resources.GetResources(Properties.Resources.previous_bookmark, Properties.Resources.IMGPrevious_bookmark);
            nextRecordButton.BackgroundImage = Resources.GetResources(Properties.Resources.next_record, Properties.Resources.IMGNext_record);
            previousRecordButton.BackgroundImage = Resources.GetResources(Properties.Resources.previous_record, Properties.Resources.IMGPrevious_record);
            gotoCurrentButton.BackgroundImage = Resources.GetResources(Properties.Resources.current, Properties.Resources.IMGCurrent);
            gotoBeginButton.BackgroundImage = Resources.GetResources(Properties.Resources.begin, Properties.Resources.IMGBegin);
            //rangeLeftPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.export_range_left, Properties.Resources.IMGExport_range_left);
            //rangeRightPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.export_range_right, Properties.Resources.IMGExport_range_right);
            minimizePanel.BackgroundImage = Resources.GetResources(Properties.Resources.mini2, Properties.Resources.IMGMini2);
            minusButton.BackgroundImage = Resources.GetResources(Properties.Resources.minus_scale, Properties.Resources.IMGMinus_scale);
            plusButton.BackgroundImage = Resources.GetResources(Properties.Resources.plus_scale, Properties.Resources.IMGPlus_scale);
            scalePanel.BackgroundImage = Resources.GetResources(Properties.Resources.scaleBar, Properties.Resources.IMGScaleBar);
            scaleButton.BackgroundImage = Resources.GetResources(Properties.Resources.scalePoint, Properties.Resources.IMGScalePoint);
            clearSelectionButton.BackgroundImage = Resources.GetResources(Properties.Resources.set_clear, Properties.Resources.IMGSet_clear);
            setEndButton.BackgroundImage = Resources.GetResources(Properties.Resources.set_end, Properties.Resources.IMGSet_end);
            setStartButton.BackgroundImage = Resources.GetResources(Properties.Resources.set_start, Properties.Resources.IMGSet_start);

            UpdateButtonStatus();

            eventPanelButton.MouseClick += EventPanelButtonMouseClick;
            plusButton.MouseClick += PlusButtonMouseClick;
            scaleButton.MouseDown += ScaleButtonMouseDown;
            minusButton.MouseClick += MinusButtonMouseClick;
            gotoCurrentButton.MouseClick += GotoCurrentButtonMouseClick;
            gotoBeginButton.MouseClick += GotoBeginButtonMouseClick;
            minimizePanel.MouseClick += MinimizePanelClick;
            clearSelectionButton.MouseClick += ClearSelectionButtonMouseClick;
            setStartButton.MouseClick += SetStartButtonMouseClick;
            setEndButton.MouseClick += SetEndButtonMouseClick;
            addBookmarkButton.MouseClick += BookmarkButtonMouseClick;
            eraserBookmarkButton.MouseClick += EraserBookmarkButtonMouseClick;
            previousBookmarkButton.MouseDown += PreviousBookmarkButtonMouseDown;
            nextBookmarkButton.MouseDown += NextBookmarkButtonMouseDown;
            previousRecordButton.MouseDown += PreviousRecordButtonMouseDown;
            nextRecordButton.MouseDown += NextRecordButtonMouseDown;
            rangeRightPanel.MouseDown += RangeRightPanelMouseDown;
            rangeLeftPanel.MouseDown += RangeLeftPanelMouseDown;

            upButton.MouseClick += UpButtonMouseClick;
            downButton.MouseClick += DownButtonMouseClick;

            datePicker.CloseUp += DatePickerCloseUp;
            datePicker.DropDown += DatePickerDropDown;
            datePicker.ValueChanged += DatePickerValueChanged;

            timePicker.ValueChanged += TimePickerValueChanged;

            switchPanel.Paint += SwitchPanelPaint;

            withArchiveServerCheckBox.CheckedChanged += WithArchiveServerCheckBoxCheckedChanged;
            enablePlaybackSmoothCheckBox.CheckedChanged += EnablePlaybackSmoothCheckBoxCheckedChanged;
            scaleLabel.Text = @"10 " + Localization["Common_Sec"];

            this.datePicker.CustomFormat = DateTimeConverter.GetDatePattern();
            this.timePicker.CustomFormat = DateTimeConverter.GetTimePattern();
        }

        private void EnablePlaybackSmoothCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Stop();
            Server.Configure.EnablePlaybackSmooth = enablePlaybackSmoothCheckBox.Checked;
            if (OnEnablePlaybackSmooth != null)
                OnEnablePlaybackSmooth(this, null);
        }

        private void WithArchiveServerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Stop();
            Server.Configure.WithArchiveServer = withArchiveServerCheckBox.Checked;
            //if (OnTimecodeChange != null)
            //    OnTimecodeChange(this, e);

            if (Server.Configure.WithArchiveServer)
            {
                foreach (KeyValuePair<ushort, ITracker> tracker in TrackerContainer.Trackers)
                {
                    tracker.Value.ShowRecord();
                }
            }


            TrackerContainer.DateTime = TrackerContainer.DateTime.AddTicks(1);
            TrackerContainer.DateTime = TrackerContainer.DateTime.AddTicks(-1);

        }

        protected void InitializeEventButton(EventButton button, EventType eventType, Image image, Image activeImage, string toolTip)
        {
            SharedToolTips.SharedToolTip.SetToolTip(button, toolTip);
            button.BackgroundImage = image;
            button.Image = image;
            button.ActiveImage = activeImage;
            button.EventType = eventType;
            button.MouseClick += EventButtonMouseClick;
        }

        public virtual void Initialize()
        {
            TimeTrackController.Server = Server;
            TimeTrackController.Initialize();

            TrackerContainer.Server = Server;

            TrackerContainer.OnDateTimeRangeChange += TrackerContainerOnDateTimeRangeChange;
            TrackerContainer.OnDateTimeChange += TrackerContainerOnDateTimeChange;
            TrackerContainer.OnSelectionChange += TimeTrackControllerOnStop;
            TrackerContainer.OnTimecodeChange += TrackerContainerOnTimecodeChange;
            TrackerContainer.OnGetPartStart += TrackerContainerOnGetPartStart;
            TrackerContainer.OnGetPartCompleted += TrackerContainerOnGetPartCompleted;

            //TrackerContainer.OnRecordDataChange += TrackerContainerOnRecordDataChange;
            TrackerContainer.OnTimeUnitChange += TrackerContainerOnTimeUnitChange;

            TimeTrackController.Visible = false;
            controllerPanel.Controls.Add(TimeTrackController);
            TimeTrackController.Location = new Point((controllerPanel.Width - TimeTrackController.Width) / 2, 0);

            TimeTrackController.OnPlay += TimeTrackControllerOnPlay;
            TimeTrackController.OnStop += TimeTrackControllerOnStop;
            TimeTrackController.OnPlayRateChanged += TimeTrackControllerOnPlayRateChanged;

            ScaleList.Add(TimeScale.Scale1Second);
            ScaleList.Add(TimeScale.Scale10Seconds);
            ScaleList.Add(TimeScale.Scale1Minute);
            ScaleList.Add(TimeScale.Scale10Minutes);
            ScaleList.Add(TimeScale.Scale1Hour);
            ScaleList.Add(TimeScale.Scale1Day);
            //ScaleList.Add(TimeScale.Scale4Hours);

            DeviceContainer = new DeviceContainer
            {
                MaxConnection = MaxConnection,
                TrackerNumPerPage = TrackerNumPerPage
            };
            DeviceContainer.OnSizeChange += DeviceContainerOnSizeChange;

            devicePanel.Controls.Add(DeviceContainer);

            var trackerContainter = _trackerContainer as Control;
            Debug.Assert(trackerContainter != null);
            trackPanel.Controls.Add(trackerContainter);

            scaleButton.LocationChanged += ScaleButtonLocationChanged;

            DateValueChangeTimer.Interval = 200;
            DateValueChangeTimer.Elapsed += DatePickerValueChangedSubmit;
            DateValueChangeTimer.SynchronizingObject = Server.Form;

            ShowLoadingTimer.Interval = 200;
            ShowLoadingTimer.Elapsed += ShowLoadingTimerLabel;
            ShowLoadingTimer.SynchronizingObject = Server.Form;

            TimeTrackController.BringToFront();

            CurrentlyScale = TimeScale.Scale10Seconds;

            if ((Server is ICMS) || (Server is IFOS))
            {
                controllerPanel.Controls.Remove(enablePlaybackSmoothCheckBox);
            }

            if (!(Server is IPTS)) return;

            var hideUnSupportButtons = ((IPTS)Server).ReleaseBrand == "Salient";

            if (!hideUnSupportButtons) return;
            clearSelectionButton.Location = setEndButton.Location;
            setEndButton.Location = nextRecordButton.Location;
            setStartButton.Location = previousRecordButton.Location;

            controllerPanel.Controls.Remove(addBookmarkButton);
            controllerPanel.Controls.Remove(eraserBookmarkButton);
            controllerPanel.Controls.Remove(previousBookmarkButton);
            controllerPanel.Controls.Remove(nextBookmarkButton);
            controllerPanel.Controls.Remove(previousRecordButton);
            controllerPanel.Controls.Remove(nextRecordButton);
            controllerPanel.Controls.Remove(enablePlaybackSmoothCheckBox);
            gotoBeginButton.Visible = false;
            toolPanel.Controls.Remove(eventPanel);
            toolPanel.Controls.Remove(eventPanelButton);
        }

        private void SwitchPanelPaint(Object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.DrawLine(Pens.Black, 0, switchPanel.Padding.Top - 1, 0, switchPanel.Height);
        }

        private void DeviceContainerOnSizeChange(Object sender, EventArgs e)
        {
            SetRangeStartDate(TrackerContainer.RangeStartDate);

            if (TrackerContainer.RangeEndDate != DateTime.MaxValue)
                SetRangeEndDate(TrackerContainer.RangeEndDate);
        }

        public void SetPlaybackProperty()
        {
            App.OnSwitchPage += PlaybackTimePropertyChange;
        }

        public void DisableFindRecord()
        {
            _recordButtonVisible = false;

            var shiftRight = nextRecordButton.Width + previousRecordButton.Width;
            var y = clearSelectionButton.Location.Y;
            clearSelectionButton.Location = new Point(clearSelectionButton.Location.X + shiftRight, y);
            setEndButton.Location = new Point(setEndButton.Location.X + shiftRight, y);
            setStartButton.Location = new Point(setStartButton.Location.X + shiftRight, y);
        }

        //private Int32 _trackPanelHeight = 129;
        public void SetSmartSearchProperty()
        {
            MaxConnection = 1;
            trackPanel.Height = 40;// _trackPanelHeight = 40;

            DeviceContainer.MaxConnection = MaxConnection;
            TrackerContainer.MaxConnection = MaxConnection;

            switchPanel.Controls.Remove(upButton);
            switchPanel.Controls.Remove(downButton);
        }

        public void Stop()
        {
            Rate = 0;
            TrackerContainer.OnStop -= TrackerContainerOnStop;
            TrackerContainer.OnGetDownloadPartCompleted -= TrackerContainerOnGetDownloadPartCompleted;
        }

        public void StopForDownload()
        {
            Rate = 0;
            TrackerContainer.OnGetDownloadPartCompleted -= TrackerContainerOnGetDownloadPartCompleted;
        }

        public void Stop(Object sender, EventArgs<UInt16, UInt16> e)
        {
            Stop();
        }

        public void PageChangeLayout(Object sender, EventArgs<List<IVideoWindow>> e)
        {
            VideoWindows = e.Value;
            TrackerContainer.VideoWindows = e.Value;
        }

        public void PlayOnRate()
        {
            if (UsingDevices == null || UsingDevices.All(device => device == null)) return;

            if (UsingDevices.Any(device => device is IDeviceLayout || device is ISubLayout)) return;

            // PlayMode.Playback on Rate
            if (OnPlayOnRate != null)
            {
                OnPlayOnRate(this, new EventArgs<UInt64, UInt16>(DateTimes.ToUtc(TrackerContainer.DateTime, Server.Server.TimeZone), (ushort)Rate));
                TrackerContainer.OnStop -= TrackerContainerOnStop;
                TrackerContainer.OnStop += TrackerContainerOnStop;
            }
        }

        public void Play()
        {
            if (UsingDevices == null || UsingDevices.All(device => device == null)) return;

            Rate = 1;

            if (UsingDevices.Any(device => device is IDeviceLayout || device is ISubLayout)) return;

            // PlayMode.Playback1X
            if (OnPlay != null)
            {
                OnPlay(this, new EventArgs<UInt64>(DateTimes.ToUtc(TrackerContainer.DateTime, Server.Server.TimeZone)));
                TrackerContainer.OnStop -= TrackerContainerOnStop;
                TrackerContainer.OnStop += TrackerContainerOnStop;
            }
        }

        protected void TimeTrackControllerOnPlay(Object sender, EventArgs e)
        {
            Play();
        }

        protected void TimeTrackControllerOnStop(Object sender, EventArgs e)
        {
            Stop();
        }

        protected void ShowLoadingTimerLabel(Object sender, EventArgs e)
        {
            if (!ShowLoadingTimer.Enabled) return;

            ShowLoadingTimer.Enabled = false;
            loadingLabel.Visible = true;
        }

        protected void TrackerContainerOnGetPartStart(Object sender, EventArgs e)
        {
            ShowLoadingTimer.Enabled = true;
            toolPanel.Enabled = switchPanel.Enabled = false;
            previousBookmarkButton.Enabled = nextBookmarkButton.Enabled = previousRecordButton.Enabled = nextRecordButton.Enabled = false;
        }

        protected void TrackerContainerOnGetPartCompleted(Object sender, EventArgs e)
        {
            ShowLoadingTimer.Enabled = false;
            loadingLabel.Visible = false;
            toolPanel.Enabled = switchPanel.Enabled = true;
            previousBookmarkButton.Enabled = nextBookmarkButton.Enabled = previousRecordButton.Enabled = nextRecordButton.Enabled = true;
        }

        protected void TrackerContainerOnGetDownloadPartCompleted(Object sender, EventArgs e)
        {
            TrackerContainer.OnGetDownloadPartCompleted -= TrackerContainerOnGetDownloadPartCompleted;
            Play();
        }

        protected void TrackerContainerOnRecordDataChange(Object sender, EventArgs<Dictionary<IDevice, Record>> e)
        {
            if (OnRecordDataChange != null)
                OnRecordDataChange(this, e);
        }

        protected void TrackerContainerOnTimecodeChange(Object sender, EventArgs<String> e)
        {
            if (App == null) return;

            App.PlaybackTimeCode = Convert.ToUInt64(Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "Timestamp"));
            Console.WriteLine("PlaybackTimeCode:" + App.PlaybackTimeCode);
            if (OnTimecodeChange != null)
                OnTimecodeChange(this, e);
        }

        protected void TrackerContainerOnTimeUnitChange(Object sender, EventArgs<TimeUnit, UInt64[]> e)
        {
            if (OnTimeUnitChange != null)
                OnTimeUnitChange(this, e);
        }

        protected void TimeTrackControllerOnPlayRateChanged(Object sender, EventArgs<float> e)
        {
            //0.5x -> next frame
            if (e.Value == 0.5)
            {
                if (OnNextFrame != null)
                {
                    _isGotoPreviousNextFrame = true;
                    OnNextFrame(this, null);
                }
                Rate = 0;
                TimeTrackController.ActiveButton("NextFrame");
                return;
            }
            //-0.5x -> previous frame
            if (e.Value == -0.5)
            {
                if (OnPreviousFrame != null)
                {
                    _isGotoPreviousNextFrame = true;
                    OnPreviousFrame(this, null);
                }
                Rate = 0;
                TimeTrackController.ActiveButton("PreviousFrame");
                return;
            }

            Rate = e.Value;

            if (OnPlay != null)
            {
                TrackerContainer.OnStop -= TrackerContainerOnStop;
                TrackerContainer.OnStop += TrackerContainerOnStop;
            }

            if (OnPlayOnRate != null)
            {
                if (Server.Configure.EnablePlaybackSmooth && Rate > 0)
                    PlayOnRate();
            }
        }

        protected void TrackerContainerOnDateTimeRangeChange(Object sender, EventArgs e)
        {
            if (TrackerContainer.RangeStartDate > DateTime.MinValue)
            {
                SetRangeStartDate(TrackerContainer.RangeStartDate);
            }

            if (TrackerContainer.RangeEndDate < DateTime.MaxValue)
            {
                SetRangeEndDate(TrackerContainer.RangeEndDate);
            }
        }

        protected void TrackerContainerOnDateTimeChange(Object sender, EventArgs e)
        {
            timePicker.Value = datePicker.Value = TrackerContainer.DateTime;
        }

        private DateTime _previousDateTime;
        private void DatePickerDropDown(Object sender, EventArgs e)
        {
            _previousDateTime = datePicker.Value;
            datePicker.ValueChanged -= DatePickerValueChanged;
            Stop();
        }

        private void DatePickerCloseUp(Object sender, EventArgs e)
        {
            if (_previousDateTime != datePicker.Value)
            {
                DateValueChangeTimer.Enabled = false;
                DateValueChangeTimer.Enabled = true;
            }
            datePicker.ValueChanged -= DatePickerValueChanged;
            datePicker.ValueChanged += DatePickerValueChanged;
        }

        protected void DatePickerValueChanged(Object sender, EventArgs e)
        {
            DateValueChangeTimer.Enabled = false;
            DateValueChangeTimer.Enabled = true;
        }

        protected void DatePickerValueChangedSubmit(Object sender, EventArgs e)
        {
            DateValueChangeTimer.Enabled = false;
            TrackerContainer.DateTime = datePicker.Value;
        }

        protected void TimePickerValueChanged(Object sender, EventArgs e)
        {
            TrackerContainer.DateTime = timePicker.Value;
        }

        public Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is IDeviceGroup || dragObj is IDevice);
        }

        public UInt16 MinimizeHeight
        {
            get
            {
                return (UInt16)(controllerPanel.Size.Height + toolPanel.Size.Height); //210 for test what's happen inside
            }
        }

        public virtual void Activate()
        {
            // Val: Jogdial Handler Activate
            if (Server.Configure.EnableAxisJoystick)
            {
                Server.Utility.OnAxisJogDialRotate += UtilityOnAxisJogDialRotate;
                Server.Utility.OnAxisJogDialShuttle += UtilityOnAxisJogDialShuttle;
                Server.Utility.OnAxisJogDialButtonDown += UtilityOnAxisJogDialButtonDown;
            }

            if (App.PlaybackTimeCode == 0)
            {
                if (TrackerContainer.DateTime.Ticks == 0)
                {
                    TrackerContainer.DateTime = Server.Server.DateTime.AddSeconds(Server.Configure.InstantPlaybackSeconds);
                }
            }
            else
            {
                TrackerContainer.DateTime = DateTimes.ToDateTime(App.PlaybackTimeCode, Server.Server.TimeZone);
            }

            if (Server is ICMS)
            {
                var cms = Server as ICMS;
                if (String.IsNullOrEmpty(cms.NVRManager.ArchiveServer.Domain))
                {
                    cms.Configure.WithArchiveServer =
                    withArchiveServerCheckBox.Checked = false;
                }

                withArchiveServerCheckBox.Enabled = !String.IsNullOrEmpty(cms.NVRManager.ArchiveServer.Domain);
            }

            if (Server is INVR)
            {
                enablePlaybackSmoothCheckBox.Checked = Server.Configure.EnablePlaybackSmooth;
            }

            TrackerContainer.RefreshTracker = false;
            if (TrackerContainer.Count == 0) return;

            TrackerContainer.RefreshTracker = true;
            TrackerContainer.ShowRecord();
        }

        public virtual void Deactivate()
        {
            // Val: Jogdial Handler Activate
            Server.Utility.OnAxisJogDialRotate -= UtilityOnAxisJogDialRotate;
            Server.Utility.OnAxisJogDialShuttle -= UtilityOnAxisJogDialShuttle;
            Server.Utility.OnAxisJogDialRotate -= UtilityOnAxisJogDialRotate;

            ShowLoadingTimer.Enabled = false;
            TrackerContainer.RefreshTracker = false;
            Stop();
        }

        public void TimecodeChange(Object sender, EventArgs<String> e)
        {
            String value = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "Timestamp");
            if (value == "") return;

            DateTime dateTime = (value != "0")
                                    ? DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone)
                                    : Server.Server.DateTime;
            GoTo(dateTime);
        }

        public void GoTo(DateTime dateTime)
        {
            if (_isGotoPreviousNextFrame)
            {
                _isGotoPreviousNextFrame = false;
                TrackerContainer.IgnoreTriggerOnTimecodeChange = true;
            }
            //playback auto break for waiting download bar 
            if (Server is ICMS && dateTime.Ticks > 0 && !_isGoto)
            {
                var trackers = TrackerContainer.Trackers.Values.OfType<Tracker>().ToArray();
                foreach (var track in trackers)
                {
                    var videoWindow = VideoWindows.FirstOrDefault(x => x.Camera == track.Camera);
                    if (videoWindow == null) continue;
                    if (videoWindow.Visible == false) continue;

                    if (track.GetRecordFromDateTime() != null)
                    {
                        if (track.PlaybackParts.All(r => r.EndDateTime <= dateTime) && track.GetRightNowRecord() != null)
                        {
                            if (OnPlay != null)
                            {
                                TrackerContainerOnStop(this, null);
                            }
                            return;
                        }
                    }
                }
            }

            TrackerContainer.DateTime = dateTime;
        }

        private void TrackerContainerOnStop(object sender, EventArgs e)
        {
            StopForDownload();
            TrackerContainer.OnGetDownloadPartCompleted += TrackerContainerOnGetDownloadPartCompleted;
        }

        private Boolean _isGoto;
        public void GoTo(Object sender, EventArgs<String> e)
        {
            Stop();
            _isGoto = true;
            TimecodeChange(sender, e);
            _isGoto = false;
        }

        public void PlaybackTimePropertyChange(Object sender, EventArgs<String, Object> e)
        {
            if (!String.Equals(e.Value1, "Playback")) return;

            var playbackParameter = e.Value2 as PlaybackParameter;
            if (playbackParameter != null)
                ApplyPlaybackParameter(playbackParameter);

            var transactionListParameter = e.Value2 as TransactionListParameter;
            if (transactionListParameter != null)
                ApplyTransactionListParameter(transactionListParameter);
        }

        //(IDevice device, UInt64 timecode, TimeUnit timeunit)
        private void ApplyPlaybackParameter(PlaybackParameter parameter)
        {
            if (parameter == null) return;

            UsingDevices = new IDevice[0];

            PageIndex = 1;
            DeviceContainer.PageIndex = PageIndex;
            TrackerContainer.PageIndex = PageIndex;

            DeviceContainer.RemoveAll();
            TrackerContainer.RemoveAll();

            switch (parameter.TimeUnit)
            {
                case TimeUnit.Unit1Senond:
                    CurrentlyScale = TimeScale.Scale1Second;
                    break;

                case TimeUnit.Unit10Senonds:
                    CurrentlyScale = TimeScale.Scale10Seconds;
                    break;

                case TimeUnit.Unit1Minute:
                    CurrentlyScale = TimeScale.Scale1Minute;
                    break;

                case TimeUnit.Unit10Minutes:
                    CurrentlyScale = TimeScale.Scale10Minutes;
                    break;

                case TimeUnit.Unit1Hour:
                    CurrentlyScale = TimeScale.Scale1Hour;
                    break;

                //case TimeUnit.Unit4Hours:
                //    CurrentlyScale = TimeScale.Scale4Hours;
                //    break;

                case TimeUnit.Unit1Day:
                    CurrentlyScale = TimeScale.Scale1Day;
                    break;

                default:
                    CurrentlyScale = TimeScale.Scale10Seconds;
                    break;
            }

            TrackerContainer.DateTime = (parameter.Timecode != 0)
                                    ? DateTimes.ToDateTime(Convert.ToUInt64(parameter.Timecode), Server.Server.TimeZone)
                                    : Server.Server.DateTime;
        }

        private void ApplyTransactionListParameter(TransactionListParameter parameter)
        {
            if (parameter == null) return;
            TrackerContainer.DateTime = DateTimes.ToDateTime(parameter.ExceptionDateTime, Server.Server.TimeZone);
        }

        protected IDevice[] UsingDevices;
        public void ContentChange(Object sender, EventArgs<Object> e)
        {
            OnContentChanged(e.Value);
        }

        protected virtual void OnContentChanged(object obj)
        {
            UsingDevices = new IDevice[0];

            if (obj is IDevice)
            {
                var device = obj as IDevice;
                UsingDevices = new IDevice[MaxConnection];

                UsingDevices[0] = device;
            }
            else if (obj is IDevice[])
            {
                var devices = obj as IDevice[];

                UsingDevices = new IDevice[MaxConnection];

                for (int index = 0; index < devices.Length; index++)
                {
                    if (index >= UsingDevices.Length) break;

                    if (devices[index] != null)
                        UsingDevices[index] = devices[index];
                }
            }

            if (UsingDevices.Length > 0)
            {
                TrackerContainer.RefreshTracker = true;

                DeviceContainer.PageIndex = PageIndex;
                TrackerContainer.PageIndex = PageIndex;

                DeviceContainer.UpdateContent(UsingDevices);
                TrackerContainer.UpdateContent(UsingDevices);
            }
            else
            {
                TrackerContainer.RefreshTracker = false;

                PageIndex = 1;
                DeviceContainer.PageIndex = PageIndex;
                TrackerContainer.PageIndex = PageIndex;

                DeviceContainer.RemoveAll();
                TrackerContainer.RemoveAll();
            }

            if (TrackerContainer.RangeStartDate == DateTime.MinValue)
            {
                SetRangeStartDate(DateTime.MinValue);
                if (OnExportStartDateTimeChange != null)
                    OnExportStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(
                    DateTimes.ToUtcString(TrackerContainer.VisibleMinDateTime, Server.Server.TimeZone))));
            }

            TimeTrackController.Visible =
                rangeLeftPanel.Visible = rangeRightPanel.Visible = (UsingDevices.Length > 0);

            if (!IsMinimize)
            {
                enablePlaybackSmoothCheckBox.Visible =
                eventPanel.Visible = eventPanelButton.Visible =
                upButton.Visible = downButton.Visible =
                clearSelectionButton.Visible = setStartButton.Visible = setEndButton.Visible =
                addBookmarkButton.Visible = eraserBookmarkButton.Visible =
                previousBookmarkButton.Visible = nextBookmarkButton.Visible = UsingDevices.Length > 0;
                previousRecordButton.Visible = nextRecordButton.Visible = (UsingDevices.Length > 0) && _recordButtonVisible;
                if (Server.Server.SupportAchiveServer) withArchiveServerCheckBox.Visible = (UsingDevices.Length > 0);
                //withArchiveServerCheckBox.Visible = (UsingDevices.Length > 0) && Server is ICMS;
            }

            if (TrackerContainer.DateTime.Ticks != 0)
            {
                Stop();
                TrackerContainer.DateTime = TrackerContainer.DateTime.AddTicks(1);
            }
        }

        public void DragStop(Point point, EventArgs<Object> e)
        {
            if (OnContentChange != null)
            {
                if (!Drag.IsDrop(this, point)) return;

                OnContentChange(this, e);
            }
        }

        public void DragMove(MouseEventArgs e)
        {
        }

        private static readonly Image _mini = Resources.GetResources(Properties.Resources.mini, Properties.Resources.IMGMini);
        private static readonly Image _mini2 = Resources.GetResources(Properties.Resources.mini2, Properties.Resources.IMGMini2);
        public void Minimize()
        {
            TrackerContainer.IsMinimize = IsMinimize = true;
            minimizePanel.BackgroundImage = _mini;

            upButton.Visible = downButton.Visible =
            clearSelectionButton.Visible = setStartButton.Visible = setEndButton.Visible =
            addBookmarkButton.Visible = eraserBookmarkButton.Visible =
            scaleLabel.Visible = minusButton.Visible = plusButton.Visible = scalePanel.Visible =
            previousBookmarkButton.Visible = nextBookmarkButton.Visible =
            previousRecordButton.Visible = nextRecordButton.Visible = withArchiveServerCheckBox.Visible = false;

            eventPanel.Visible = eventPanelButton.Visible = false;

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
        }

        public void Maximize()
        {
            TrackerContainer.IsMinimize = IsMinimize = false;
            minimizePanel.BackgroundImage = _mini2;

            scaleLabel.Visible = minusButton.Visible = plusButton.Visible = scalePanel.Visible = true;

            eventPanel.Visible = eventPanelButton.Visible =
            upButton.Visible = downButton.Visible =
            clearSelectionButton.Visible = setStartButton.Visible = setEndButton.Visible =
            addBookmarkButton.Visible = eraserBookmarkButton.Visible =
            previousBookmarkButton.Visible = nextBookmarkButton.Visible = (UsingDevices != null && UsingDevices.Length > 0);
            previousRecordButton.Visible = nextRecordButton.Visible = (UsingDevices != null && UsingDevices.Length > 0) && _recordButtonVisible;

            if (Server.Server.SupportAchiveServer) withArchiveServerCheckBox.Visible = (UsingDevices != null && UsingDevices.Length > 0);
            //withArchiveServerCheckBox.Visible = (UsingDevices != null && UsingDevices.Length > 0) && Server is ICMS;

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);

            rangeLeftPanel.Size = new Size(rangeLeftPanel.Width, trackPanel.Height - rangeLeftPanel.Location.Y);
            rangeRightPanel.Size = new Size(rangeRightPanel.Width, trackPanel.Height - rangeRightPanel.Location.Y);

            TrackerContainer.ShowRecord();
            TrackerContainer.Invalidate();
        }

        protected virtual void MinimizePanelClick(Object sender, MouseEventArgs e)
        {
            if (IsMinimize)
                Maximize();
            else
                Minimize();
        }

        public void ExportVideo(Object sender, EventArgs e)
        {
            var start = (TrackerContainer.RangeStartDate == DateTime.MinValue)
                ? TrackerContainer.VisibleMinDateTime
                : TrackerContainer.RangeStartDate;

            var end = (TrackerContainer.RangeEndDate == DateTime.MaxValue)
                ? TrackerContainer.VisibleMaxDateTime
                : TrackerContainer.RangeEndDate;

            if (start == TrackerContainer.RangeStartDate && end < start)
                end = start.AddSeconds(600);

            if (end == TrackerContainer.RangeEndDate && end < start)
                start = end.AddSeconds(-600);

            App.ExportVideo(UsingDevices, start, end);
        }

        public void DownloadCase(Object sender, EventArgs e)
        {
            var start = (TrackerContainer.RangeStartDate == DateTime.MinValue)
                ? TrackerContainer.VisibleMinDateTime
                : TrackerContainer.RangeStartDate;

            var end = (TrackerContainer.RangeEndDate == DateTime.MaxValue)
                ? TrackerContainer.VisibleMaxDateTime
                : TrackerContainer.RangeEndDate;

            if (start == TrackerContainer.RangeStartDate && end < start)
                end = start.AddSeconds(600);

            if (end == TrackerContainer.RangeEndDate && end < start)
                start = end.AddSeconds(-600);

            App.DownloadCase(UsingDevices, start, end, null);
        }

        public delegate void ServerTimeZoneChangeDelegate(Object sender, EventArgs e);
        public void ServerTimeZoneChange(Object sender, EventArgs e)
        {
            if (((Control)TrackerContainer).InvokeRequired)
            {
                ((Control)TrackerContainer).Invoke(new ServerTimeZoneChangeDelegate(ServerTimeZoneChange), sender, e);
                return;
            }

            //TrackerContainer.ResetBackgroundScalePosition();
            TrackerContainer.ShowRecord();
            TrackerContainer.Invalidate();
        }

        protected void GotoCurrentButtonMouseClick(Object sender, MouseEventArgs e)
        {
            Stop();

            TrackerContainer.DateTime = Server.Server.DateTime.AddSeconds(Server.Configure.InstantPlaybackSeconds);
        }

        protected void TrackerContainerMouseDown(Object sender, MouseEventArgs e)
        {
            //datePicker timePicker  lost selection
            if (datePicker.Focused)
            {
                datePicker.Enabled = false;
                datePicker.Enabled = true;
            }
            if (timePicker.Focused)
            {
                timePicker.Enabled = false;
                timePicker.Enabled = true;
            }
        }

        private bool _isGotoPreviousNextFrame;
        public void KeyboardPress(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Space:
                    if (Rate == 0)
                        Play();
                    else
                        Stop();
                    break;

                case Keys.Left:
                    if (OnPreviousFrame != null)
                    {
                        if (Rate != 0)
                            Stop();

                        _isGotoPreviousNextFrame = true;
                        OnPreviousFrame(this, null);
                    }
                    //TrackerContainer.DateTime = TrackerContainer.DateTime.AddMilliseconds(-100);
                    break;

                case Keys.Right:
                    if (OnNextFrame != null)
                    {
                        if (Rate != 0)
                            Stop();

                        _isGotoPreviousNextFrame = true;
                        OnNextFrame(this, null);
                    }
                    //TrackerContainer.DateTime = TrackerContainer.DateTime.AddMilliseconds(100);
                    break;
            }
        }

        private Boolean _isEventExpand;
        private void EventPanelButtonMouseClick(Object sender, MouseEventArgs e)
        {
            _isEventExpand = !_isEventExpand;

            if (_isEventExpand)
            {
                eventPanelButton.BackgroundImage = _left;
                eventPanel.Width = eventPanel.Padding.Left + 32 * EventButtonCount;//space + width X button count
            }
            else
            {
                eventPanelButton.BackgroundImage = _right;
                eventPanel.Width = eventPanel.Padding.Left + 32 * 4;//space + width X button count
            }
        }

        protected virtual int EventButtonCount
        {
            get
            {
                if (Server != null)
                {
                    if (!Server.Configure.EnableUserDefine)
                        eventPanel.Controls.Remove(userdefineButton);
                    else
                    {
                        if (!eventPanel.Controls.Contains(userdefineButton))
                            eventPanel.Controls.Add(userdefineButton);
                    }
                }

                return eventPanel.Controls.Count;
            }
        }
    }
}