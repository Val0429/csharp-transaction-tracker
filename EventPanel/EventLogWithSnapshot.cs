using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using DeviceConstant;
using Interface;
using PanelBase;

namespace EventPanel
{
    public sealed class EventLogWithSnapshot : Panel
    {
        private IApp _app;
        public IApp App
        {
            get { return _app; }
            set
            {
                _app = value;
                _checkLocationTimer.SynchronizingObject = value.Form;
            }
        }

        public IServer Server { get; set; }

        private readonly PictureBox _snapshot;
        private readonly PictureBox _pictureBox;
        private readonly DoubleBufferPanel _infoPanel;
        private readonly System.Timers.Timer _checkLocationTimer = new System.Timers.Timer();

        public EventFlowLayoutPanel FlowLayoutPanel;
        public Dictionary<String, String> Localization;

        private static readonly Image _noImage = Resources.GetResources(Properties.Resources.no_image, Properties.Resources.IMGNoimage);
        private static readonly Image _snapshotBg = Resources.GetResources(Properties.Resources.image, Properties.Resources.IMGImage);

        private static readonly Image _motion = Resources.GetResources(Properties.Resources.motion, Properties.Resources.IMGMotion);
        private static readonly Image _diOn = Resources.GetResources(Properties.Resources.dion, Properties.Resources.IMGDion);
        private static readonly Image _diOff = Resources.GetResources(Properties.Resources.dioff, Properties.Resources.IMGDioff);
        private static readonly Image _doOn = Resources.GetResources(Properties.Resources.doon, Properties.Resources.IMGDoon);
        private static readonly Image _doOff = Resources.GetResources(Properties.Resources.dooff, Properties.Resources.IMGDooff);
        private static readonly Image _networkloss = Resources.GetResources(Properties.Resources.networkloss, Properties.Resources.IMGNetworkloss);
        private static readonly Image _networkrecovery = Resources.GetResources(Properties.Resources.networkrecovery, Properties.Resources.IMGNetworkrecovery);
        private static readonly Image _videoloss = Resources.GetResources(Properties.Resources.videoloss, Properties.Resources.IMGVideoloss);
        private static readonly Image _videorecovery = Resources.GetResources(Properties.Resources.videorecovery, Properties.Resources.IMGVideorecovery);
        private static readonly Image _panic = Resources.GetResources(Properties.Resources.panic, Properties.Resources.IMGPanic);
        private static readonly Image _userdefine = Resources.GetResources(Properties.Resources.userdefine, Properties.Resources.IMGUserDefine);
        private static readonly Image _manualrecord = Resources.GetResources(Properties.Resources.manualrecord, Properties.Resources.IMGManualrecord);
        private static readonly Image _recordrecovery = Resources.GetResources(Properties.Resources.recordrecovery, Properties.Resources.IMGRecordrecovery);
        private static readonly Image _recordfailed = Resources.GetResources(Properties.Resources.recordfailed, Properties.Resources.IMGRecordfailed);
        private static readonly Image _crossline = Resources.GetResources(Properties.Resources.crossline, Properties.Resources.IMGCrossLine);
        private static readonly Image _intrusionDetection = Resources.GetResources(Properties.Resources.intrusionDetection, Properties.Resources.IMGIntrusionDetection);
        private static readonly Image _loiteringDetection = Resources.GetResources(Properties.Resources.loiteringDetection, Properties.Resources.IMGLoiteringDetection);
        private static readonly Image _objectCountingIn = Resources.GetResources(Properties.Resources.objectCountingIn, Properties.Resources.IMGObjectCountingIn);
        private static readonly Image _objectCountingOut = Resources.GetResources(Properties.Resources.objectCountingOut, Properties.Resources.IMGObjectCountingOut);
        private static readonly Image _audioDetection = Resources.GetResources(Properties.Resources.audioDetection, Properties.Resources.IMGAudioDetection);
        private static readonly Image _temperingDetection = Resources.GetResources(Properties.Resources.temperingDetection, Properties.Resources.IMGTemperingDetection);

        public EventLogWithSnapshot()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"EventPanel_EventNo", "No. %1"},
                                   {"Event_RecordFailed", "Record Failed"},
                                   {"Event_RecordRecovery", "Record Recovery"},
                                   {"Event_RAIDDegraded", "RAID Degrade"},
                                   {"Event_RAIDInactive", "RAID Inactive"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.None;
            Size = new Size(440, 160);
            Padding = new Padding(1);
            //BackColor = Color.Transparent;
            //ForeColor = Color.White;
            //BorderStyle = BorderStyle.FixedSingle;
            Cursor = Cursors.Hand;

            _snapshot = new PictureBox
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Dock = DockStyle.Right,
                Size = new Size(240, 160),
                //Location = new Point(200, 0),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = _snapshotBg,
            };

            _pictureBox = new PictureBox
            {
                BackColor = Color.Transparent,
                Size = new Size(40, 40),
                Dock = DockStyle.None,
                Padding = new Padding(0),
                Location = new Point(157, 43),
                SizeMode = PictureBoxSizeMode.CenterImage,
            };

            _infoPanel = new DoubleBufferPanel
            {
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                Location = new Point(0, 0),
                Size = new Size(155, 160),
            };

            _checkLocationTimer.Elapsed += CheckVisible;
            _checkLocationTimer.Interval = 1500;

            MouseDoubleClick += LogDoubleClick;
            _infoPanel.MouseDoubleClick += LogDoubleClick;
            _pictureBox.MouseDoubleClick += LogDoubleClick;
            _snapshot.MouseDoubleClick += LogDoubleClick;

            _infoPanel.Click += LogClick;
            _pictureBox.Click += LogClick;

            Controls.Add(_infoPanel);
            Controls.Add(_pictureBox);
            Controls.Add(_snapshot);

            _infoPanel.Paint += InfoPanelPaint;
            LocationChanged += EventLogWithSnapshotLocationChanged;
            ParentChanged += EventLogWithSnapshotLocationChanged;
        }

        private Pen _pen;
        private void EventLogPaint(Object sender, PaintEventArgs e)
        {
            if (!FlowLayoutPanel.IsPauseReceiveEvent)
                UpdatePenColor();

            if (_pen != null)
                e.Graphics.DrawRectangle(_pen, 0, 0, Width - 1, Height - 1);
        }

        public void UpdatePenColor()
        {
            if (DateTime.Now.Ticks - _ticks < 5000000)
                _pen = Pens.Orange;
            else if (DateTime.Now.Ticks - _ticks < 15000000)
                _pen = Pens.Yellow;
            else if (DateTime.Now.Ticks - _ticks < 30000000)
                _pen = Pens.LightYellow;
            else
                Paint -= EventLogPaint;
        }

        private void EventLogWithSnapshotLocationChanged(Object sender, EventArgs e)
        {
            CheckVisible();
        }

        public void CheckVisible()
        {
            if (!FlowLayoutPanel.IsPauseReceiveEvent)
                UpdatePenColor();

            if (IsLoad || FlowLayoutPanel == null || !FlowLayoutPanel.IsActivated || !_snapshot.Visible) return;

            _checkLocationTimer.Enabled = false;
            _checkLocationTimer.Enabled = true;
        }

        private void CheckVisible(Object sender, EventArgs e)
        {
            if (Parent == null)
            {
                if (FlowLayoutPanel.QueueSearchResultPanel.Contains(this))
                    FlowLayoutPanel.QueueSearchResultPanel.Remove(this);
                return;
            }

            _checkLocationTimer.Enabled = false;
            if ((Location.Y + Height) < 0 || Location.Y > Parent.Height)
            {
                if (FlowLayoutPanel.QueueSearchResultPanel.Contains(this))
                    FlowLayoutPanel.QueueSearchResultPanel.Remove(this);
                return;
            }

            IsLoad = true;
            FlowLayoutPanel.QueueLoadSnapshot(this);
        }

        private Boolean _isReset;
        public Boolean IsLoadingImage;
        public Boolean IsLoad;

        public void Reset()
        {
            _timecode = 0;
            IsLoad = false;
            _snapshot.Image = _snapshotBg;
            _snapshot.BackgroundImage = null;
            _checkLocationTimer.Enabled = false;
            _isReset = true;
        }

        public void LoadSnapshot()
        {
            if (_cameraEvent.Device == null || _timecode == 0 || !(_cameraEvent.Device is ICamera)) return;
            //ICamera camera = _cameraEvent.Device as ICamera;
            _isReset = false;
            IsLoadingImage = true;

            var bitmap = ((ICamera)_cameraEvent.Device).GetSnapshot(_timecode, new Size(320, 240));// as Bitmap;
            UInt32 retry = 1;
            while (bitmap == null && retry > 0 && !_isReset)
            {
                Application.RaiseIdle(null);
                retry--;
                bitmap = ((ICamera)_cameraEvent.Device).GetSnapshot(_timecode, new Size(320, 240));// as Bitmap;// ?? _noImage
            }

            IsLoadingImage = false;
            if (_isReset)
            {
                return;
            }

            if (bitmap == null)
            {
                _snapshot.Image = _noImage;
            }
            else
            {
                _snapshot.Image = null;
                _snapshot.BackgroundImage = bitmap;
            }
        }

        private void LogClick(Object sender, EventArgs e)
        {
            if (Parent != null)
                Parent.Focus();
        }

        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private readonly Font _largeFont = new Font("Arial", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        private void InfoPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            g.DrawString(_cameraEvent.NVR.Credential.Domain + Environment.NewLine + _cameraEvent.NVR.Name, _largeFont, Brushes.White, 4, 3);

            g.DrawString(Localization["EventPanel_EventNo"].Replace("%1", _count.ToString()) + Environment.NewLine +
                ((_cameraEvent.Device != null) ? _cameraEvent.Device + Environment.NewLine : "") +
                _eventStr + Environment.NewLine +
                _dateTime, _font, Brushes.White, 4, 55);

        }

        private void LogDoubleClick(Object sender, MouseEventArgs e)
        {
            if (_cameraEvent.Device != null)
            {

                Server.WriteOperationLog("Event Panel Popup Instant Playback %1/%2/%3".Replace("%1", _cameraEvent.NVR.ToString())
                        .Replace("%2", _cameraEvent.Device.ToString())
                        .Replace("%3", _cameraEvent.DateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                    );

                App.PopupInstantPlayback(_cameraEvent.Device, _timecode);
            }
        }

        private UInt64 _timecode;
        private UInt32 _count;
        private String _dateTime;
        private String _eventStr;
        private Int64 _ticks;
        private ICameraEvent _cameraEvent;

        public Boolean UpdateLog(ICameraEvent cameraEvent, UInt32 count)
        {
            _cameraEvent = cameraEvent;

            _count = count;

            _dateTime = cameraEvent.DateTime.ToDateTimeString();

            _ticks = DateTime.Now.Ticks;
            Paint -= EventLogPaint;
            Paint += EventLogPaint;

            _timecode = DateTimes.ToUtc(cameraEvent.DateTime, _cameraEvent.NVR.Server.TimeZone);

            _eventStr = (_cameraEvent is CameraEvent) ? ((CameraEvent)_cameraEvent).ToLocalizationString() : "";

            switch (cameraEvent.Type)
            {
                case EventType.Motion:
                    _pictureBox.Image = _motion;
                    break;

                case EventType.DigitalInput:
                    if (_cameraEvent.Device is ICamera)
                    {
                        if (((ICamera)_cameraEvent.Device).IOPort.Count > 0)
                        {
                            var ioports = ((ICamera)_cameraEvent.Device).IOPort;
                            if (!ioports.ContainsKey(cameraEvent.Id) || ioports[cameraEvent.Id] != IOPort.Input)
                                return false;
                        }
                        else
                        {
                            if (((ICamera)_cameraEvent.Device).Model.NumberOfDi < cameraEvent.Id)
                                return false;
                        }

                        _pictureBox.Image = (cameraEvent.Value)
                            ? _diOn : _diOff;
                    }

                    break;

                case EventType.DigitalOutput:
                    if (_cameraEvent.Device is ICamera)
                    {
                        if (((ICamera)_cameraEvent.Device).IOPort.Count > 0)
                        {
                            var ioports = ((ICamera)_cameraEvent.Device).IOPort;
                            if (!ioports.ContainsKey(cameraEvent.Id) || ioports[cameraEvent.Id] != IOPort.Output)
                                return false;
                        }
                        else
                        {
                            if (((ICamera)_cameraEvent.Device).Model.NumberOfDo < cameraEvent.Id)
                                return false;
                        }

                        _pictureBox.Image = (cameraEvent.Value)
                            ? _doOn : _doOff;

                        if (((ICamera)_cameraEvent.Device).DigitalOutputStatus.ContainsKey(cameraEvent.Id))
                            ((ICamera)_cameraEvent.Device).DigitalOutputStatus[cameraEvent.Id] = (cameraEvent.Value);
                    }

                    break;

                case EventType.NetworkLoss:
                    _pictureBox.Image = _networkloss;
                    break;

                case EventType.NetworkRecovery:
                    _pictureBox.Image = _networkrecovery;
                    break;

                case EventType.VideoLoss:
                    _pictureBox.Image = _videoloss;
                    break;

                case EventType.VideoRecovery:
                    _pictureBox.Image = _videorecovery;
                    break;

                case EventType.ManualRecord:
                    _pictureBox.Image = _manualrecord;
                    break;

                case EventType.Panic:
                    _pictureBox.Image = _panic;
                    break;

                case EventType.CrossLine:
                    _pictureBox.Image = _crossline;
                    break;

                case EventType.IntrusionDetection:
                    _pictureBox.Image = _intrusionDetection;
                    break;

                case EventType.LoiteringDetection:
                    _pictureBox.Image = _loiteringDetection;
                    break;

                case EventType.ObjectCountingIn:
                    _pictureBox.Image = _objectCountingIn;
                    break;

                case EventType.ObjectCountingOut:
                    _pictureBox.Image = _objectCountingOut;
                    break;

                case EventType.AudioDetection:
                    _pictureBox.Image = _audioDetection;
                    break;

                case EventType.TamperDetection:
                    _pictureBox.Image = _temperingDetection;
                    break;

                default:
                    _pictureBox.Image = _userdefine;
                    break;
            }
            SharedToolTips.SharedToolTip.SetToolTip(_infoPanel, _eventStr);
            SharedToolTips.SharedToolTip.SetToolTip(_pictureBox, _eventStr);

            switch (FlowLayoutPanel.LogSize)
            {
                case "QQVGA":
                    SmallSize();
                    break;

                case "HQVGA":
                    MediumSize();
                    break;

                case "QVGA":
                    LargeSize();
                    break;

                case "NoSnapshot":
                    NoSnapshot();
                    break;
            }
            return true;
        }

        public void UpdateLog(ICameraEvent cameraEvent, DateTime dateTime, UInt32 count)
        {
            _cameraEvent = cameraEvent;

            _count = count;

            _dateTime = dateTime.ToDateTimeString();

            _ticks = DateTime.Now.Ticks;
            Paint -= EventLogPaint;
            Paint += EventLogPaint;

            _timecode = DateTimes.ToUtc(dateTime, _cameraEvent.NVR.Server.TimeZone);

            switch (cameraEvent.Type)
            {
                case EventType.RecordFailed:
                    _pictureBox.Image = _recordfailed;
                    _eventStr = Localization["Event_RecordFailed"];
                    break;

                case EventType.RAIDDegraded:
                    _pictureBox.Image = _recordfailed;
                    _eventStr = Localization["Event_RAIDDegraded"];
                    break;

                case EventType.RAIDInactive:
                    _pictureBox.Image = _recordfailed;
                    _eventStr = Localization["Event_RAIDInactive"];
                    break;

                case EventType.RecordRecovery:
                    _pictureBox.Image = _recordrecovery;
                    _eventStr = Localization["Event_RecordRecovery"];
                    break;

                    //case EventType.UserDefine:
                    //    _eventPictureBox.Image = _event;
                    //    eventStr = "User Define";
                    //    break;
            }

            IsLoad = true;
            _snapshot.Image = _noImage;
            _snapshot.BackgroundImage = null;

            SharedToolTips.SharedToolTip.SetToolTip(_infoPanel, _eventStr);
            SharedToolTips.SharedToolTip.SetToolTip(_pictureBox, _eventStr);

            switch (FlowLayoutPanel.LogSize)
            {
                case "QQVGA":
                    SmallSize();
                    break;

                case "HQVGA":
                    MediumSize();
                    break;

                case "QVGA":
                    LargeSize();
                    break;

                case "NoSnapshot":
                    NoSnapshot();
                    break;
            }
        }

        private String _size;
        public void NoSnapshot()
        {
            if (_size == "NoSnapshot") return;

            _size = "NoSnapshot";
            Size = new Size(210, 120);
            _snapshot.Visible = false;
            Invalidate();
        }

        public void SmallSize()
        {
            if (_size == "QQVGA") return;

            _size = "QQVGA";
            Size = new Size(360, 120);
            _snapshot.Visible = true;
            _snapshot.Size = new Size(160, 120);
            Invalidate();
        }

        public void MediumSize()
        {
            if (_size == "HQVGA") return;

            _size = "HQVGA";
            Size = new Size(440, 160);
            _snapshot.Visible = true;
            _snapshot.Size = new Size(240, 160);
            Invalidate();
        }

        public void LargeSize()
        {
            if (_size == "QVGA") return;

            _size = "QVGA";
            Size = new Size(520, 240);
            _snapshot.Visible = true;
            _snapshot.Size = new Size(320, 240);
            Invalidate();
        }
    }
}
