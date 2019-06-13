using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace EventPanel
{
    public sealed class EventLog : Panel
    {
        public IApp App;
        public IPTS PTS;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is IPTS)
                    PTS = value as IPTS;
            }
        }
        public event EventHandler<EventArgs<IDevice, DateTime>> OnLogDoubleClick;

        private Boolean _isOdd;
        public Boolean IsOdd
        {
            get { return _isOdd; }
            set
            {
                _isOdd = value;
                if (_isOdd)
                {
                    BackColor = ColorTranslator.FromHtml("#373b44");
                }
                else
                {
                    BackColor = ColorTranslator.FromHtml("#2f343a");
                }
            }
        }

        public ICameraEvent CameraEvent
        {
            get { return _cameraEvent; }
        }

        public Dictionary<String, String> Localization;

        private Image _image;
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

        public EventLog()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"EventPanel_EventNo", "No. %1"},
								   {"Event_RecordFailed", "Record Failed"},
								   {"Event_RecordRecovery", "Record Recovery"},
								   {"Event_RAIDDegraded", "RAID Degrade"},
								   {"Event_RAIDInactive", "RAID Inactive"}
							   };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Height = 70;

            Padding = new Padding(0, 1, 0, 1);
            //BorderStyle = BorderStyle.FixedSingle;

            MouseDoubleClick += EventLogDoubleClick;

            Click += EventLogClick;

            Cursor = Cursors.Hand;

            Paint += PanelPaint;
        }

        private void EventLogClick(Object sender, EventArgs e)
        {
            if (Parent != null)
                Parent.Focus();
        }

        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private readonly Brush _brush = new SolidBrush(Color.FromArgb(137, 140, 147));
        private void PanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            try
            {
                g.DrawLine(new Pen(Color.FromArgb(60, 65, 71)), 0, 0, Width - 1, 0);
                g.DrawLine(new Pen(Color.FromArgb(38, 42, 45)), 0, Height - 1, Width - 1, Height - 1);

                var info = "";
                if (CameraEvent != null)
                {
                    info = ((CameraEvent.Device != null)
                                ? CameraEvent.Device.ToString()
                                : CameraEvent.NVR.Credential.Domain);
                    var text = info;

                    SizeF fSize = g.MeasureString(text, _font);

                    //trim text if too long
                    while (fSize.Width > 125)
                    {
                        if (text.Length <= 1) break;

                        text = text.Substring(0, text.Length - 1);
                        fSize = g.MeasureString(text, _font);
                    }

                    if (text != info)
                        info = text + @"...";

                    info += Environment.NewLine;
                }
                else if (_transactionItem != null)
                {
                    var pos = PTS.POS.FindPOSById(_transactionItem.POS);
                    if (pos != null)
                        info = pos + Environment.NewLine;
                }

                if (Server is ICMS)
                {
                    var text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                        Localization["EventPanel_EventNo"].Replace("%1", _count.ToString(CultureInfo.InvariantCulture)), Environment.NewLine,
                        _nvrStr, Environment.NewLine,
                        info, _eventStr, Environment.NewLine,
                        _dateTime);

                    g.DrawString(text, _font, _brush, 22, 9);
                    g.DrawImage(_image, 155, 25);
                }
                else
                {
                    g.DrawString(Localization["EventPanel_EventNo"].Replace("%1", _count.ToString(CultureInfo.InvariantCulture)) + Environment.NewLine +
                    info + _eventStr + Environment.NewLine + _dateTime, _font, _brush, 22, 9);

                    if (Server is IPTS)
                    {
                        g.DrawImage(_image, 248, 15);
                    }
                    else
                    {
                        g.DrawImage(_image, 155, 15);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        private void EventLogDoubleClick(Object sender, MouseEventArgs e)
        {
            if (OnLogDoubleClick != null)
                OnLogDoubleClick(this, new EventArgs<IDevice, DateTime>(CameraEvent.Device, CameraEvent.DateTime));
        }

        private UInt32 _count;
        private String _dateTime;
        private String _eventStr;
        private String _nvrStr;
        private ICameraEvent _cameraEvent;
        private POS_Exception.TransactionItem _transactionItem;

        public Boolean UpdateLog(ICameraEvent cameraEvent, UInt32 count)
        {
            //Height = 70;
            if (Server is ICMS)
            {
                Height = 90;
            }

            _cameraEvent = cameraEvent;

            _count = count;

            _dateTime = cameraEvent.DateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            _eventStr = (CameraEvent is CameraEvent) ? ((CameraEvent)CameraEvent).ToLocalizationString() : "";

            var camera = cameraEvent.Device as ICamera;

            if (Server is ICMS && camera != null)
                _nvrStr = camera.Server.ToString();
            else
                _nvrStr = String.Empty;

            switch (cameraEvent.Type)
            {
                case EventType.Motion:
                    if (camera != null)
                    {
                        if (camera.Model.NumberOfMotion < cameraEvent.Id)
                            return false;
                        _image = _motion;
                    }
                    break;

                case EventType.DigitalInput:
                    if (camera != null)
                    {
                        if (camera.IOPort.Count > 0)
                        {
                            if (!camera.IOPort.ContainsKey(cameraEvent.Id) || camera.IOPort[cameraEvent.Id] != IOPort.Input)
                                return false;
                        }
                        else
                        {
                            if (camera.Model.NumberOfDi < cameraEvent.Id)
                                return false;
                        }

                        _image = (cameraEvent.Value) ? _diOn : _diOff;
                    }
                    break;

                case EventType.DigitalOutput:
                    if (camera != null)
                    {
                        if (camera.IOPort.Count > 0)
                        {
                            if (!camera.IOPort.ContainsKey(cameraEvent.Id) || camera.IOPort[cameraEvent.Id] != IOPort.Output)
                                return false;
                        }
                        else
                        {
                            if (camera.Model.NumberOfDo < cameraEvent.Id)
                                return false;
                        }

                        _image = (cameraEvent.Value) ? _doOn : _doOff;

                        if (camera.DigitalOutputStatus.ContainsKey(cameraEvent.Id))
                            camera.DigitalOutputStatus[cameraEvent.Id] = (cameraEvent.Value);
                    }
                    break;

                case EventType.NetworkLoss:
                    _image = _networkloss;
                    break;

                case EventType.NetworkRecovery:
                    _image = _networkrecovery;
                    break;

                case EventType.VideoLoss:
                    _image = _videoloss;
                    break;

                case EventType.VideoRecovery:
                    _image = _videorecovery;
                    break;

                case EventType.ManualRecord:
                    _image = _manualrecord;
                    break;

                case EventType.Panic:
                    _image = _panic;
                    break;

                case EventType.RecordFailed:
                    _image = _recordfailed;
                    break;

                case EventType.RecordRecovery:
                    _image = _recordrecovery;
                    break;

                case EventType.CrossLine:
                    _image = _crossline;
                    break;

                case EventType.IntrusionDetection:
                    _image = _intrusionDetection;
                    break;

                case EventType.LoiteringDetection:
                    _image = _loiteringDetection;
                    break;

                case EventType.ObjectCountingIn:
                    _image = _objectCountingIn;
                    break;

                case EventType.ObjectCountingOut:
                    _image = _objectCountingOut;
                    break;

                case EventType.AudioDetection:
                    _image = _audioDetection;
                    break;

                case EventType.TamperDetection:
                    _image = _temperingDetection;
                    break;

                default:
                    _image = _userdefine;
                    break;
            }

            SharedToolTips.SharedToolTip.SetToolTip(this, _eventStr);

            Invalidate();
            return true;
        }

        public void UpdateLog(ICameraEvent cameraEvent, DateTime dateTime, UInt32 count)
        {
            //Height = 70;
            _cameraEvent = cameraEvent;

            _count = count;
            CameraEvent.NVR = cameraEvent.NVR;

            _dateTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            switch (cameraEvent.Type)
            {
                case EventType.RecordFailed:
                    _image = _recordfailed;
                    _eventStr = Localization["Event_RecordFailed"];
                    break;

                case EventType.RAIDDegraded:
                    _image = _recordfailed;
                    _eventStr = Localization["Event_RAIDDegraded"];
                    break;

                case EventType.RAIDInactive:
                    _image = _recordfailed;
                    _eventStr = Localization["Event_RAIDInactive"];
                    break;

                case EventType.RecordRecovery:
                    _image = _recordrecovery;
                    _eventStr = Localization["Event_RecordRecovery"];
                    break;

                //case EventType.UserDefine:
                //    _pictureBox.Image = _event;
                //    eventStr = "User Define";
                //    break;
            }
            SharedToolTips.SharedToolTip.SetToolTip(this, _eventStr);

            Invalidate();
        }

        public Boolean UpdateLog(POS_Exception.TransactionItem transactionItem, UInt32 count)
        {
            _transactionItem = transactionItem;

            _count = count;

            _dateTime = transactionItem.DateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var temp = transactionItem.Content.Split(',');
            var tempList = new Queue<String>();

            var usefulTransaction = false;
            foreach (var tempStr in temp)
            {
                var tempStr2 = tempStr.Split('$');

                foreach (var tempStr3 in tempStr2)
                {
                    if (tempStr3 == tempStr2[0])
                        tempList.Enqueue(tempStr3.Trim());
                    else
                    {
                        usefulTransaction = true;
                        tempList.Enqueue("$" + tempStr3.Trim());
                    }
                }
            }
            if (tempList.Count > 0 && tempList.Peek() == _transactionItem.POS)
            {
                tempList.Dequeue();
            }
            _eventStr = String.Join(Environment.NewLine, tempList.ToArray());

            if (String.IsNullOrEmpty(_eventStr)) return false;

            if (tempList.Count == 1)
            {
                var pos = PTS.POS.FindPOSById(_transactionItem.POS);
                if (pos != null)
                {
                    if (PTS.POS.Exceptions.ContainsKey(pos.Exception))
                    {
                        var value = tempList.Peek();
                        var exceptions = PTS.POS.Exceptions[pos.Exception];
                        foreach (var exception in exceptions.Exceptions)
                        {
                            if (String.Equals(value, exception.Value))
                            {
                                usefulTransaction = true;
                                _image = _panic;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                _image = _panic;
            }

            if (!usefulTransaction)
            {
                Console.WriteLine(_eventStr);
                return false;
            }

            SharedToolTips.SharedToolTip.SetToolTip(this, _eventStr);

            Invalidate();

            return true;
        }
    }
}
