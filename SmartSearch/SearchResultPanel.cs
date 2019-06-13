using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using Interface;
using PanelBase;

namespace SmartSearch
{
    public class SearchResultPanel : Panel
    {
        public event EventHandler OnSelectionChange;

        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (_server != null)
                    _checkLocationTimer.SynchronizingObject = _server.Form;
            }
        }
        public ICamera Camera;
        public UInt64 Timecode;
        public SearchResult SearchResult;

        protected Image _image;
        private Image _snapshotImage;
        private Boolean _snapshotLoadFailure = false;

        public virtual String Type
        {
            set
            {
                switch (value)
                {
                    case "ManualRecord":
                        _image = ManualRecord;
                        break;

                    case "IVS":
                        _image = IVS;
                        break;

                    case "NetworkLoss":
                        _image = NetworkLoss;
                        break;

                    case "NetworkRecovery":
                        _image = NetworkRecovery;
                        break;

                    case "VideoLoss":
                        _image = VideoLoss;
                        break;

                    case "VideoRecovery":
                        _image = VideoRecovery;
                        break;

                    case "Panic":
                        _image = Panic;
                        break;

                    case "Motion":
                        _image = Motion;
                        break;

                    case "DigitalInput":
                        _image = DigitalInput;
                        break;

                    case "DigitalOutput":
                        _image = DigitalOutput;
                        break;

                    case "CrossLine":
                        _image = CrossLine;
                        break;

                    case "IntrusionDetection":
                        _image = IntrusionDetection;
                        break;

                    case "LoiteringDetection":
                        _image = LoiteringDetection;
                        break;

                    case "ObjectCountingIn":
                        _image = ObjectCountingIn;
                        break;

                    case "ObjectCountingOut":
                        _image = ObjectCountingOut;
                        break;

                    case "AudioDetection":
                        _image = AudioDetection;
                        break;

                    case "TamperDetection":
                        _image = TamperDetection;
                        break;
                    case "UserDefine":
                        _image = Userdefined;
                        break;
                    default:
                        _image = null;
                        break;
                }
            }
        }

        private readonly System.Timers.Timer _checkLocationTimer = new System.Timers.Timer();

        public SearchResultPanel()
        {
            DoubleBuffered = true;
            Dock = DockStyle.None;

            MediumSize();

            MouseClick += SearchResultPanelMouseClick;

            MouseDoubleClick += SearchResultPanelMouseDoubleClick;

            _checkLocationTimer.Elapsed += CheckVisible;
            _checkLocationTimer.Interval = 1000;

            Paint += SearchResultPanelPaint;
            ParentChanged += SearchResultPanelLocationChanged;
            LocationChanged += SearchResultPanelLocationChanged;
        }

        private void SearchResultPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            if (Timecode == 0) return;

            UInt64 timecode = Timecode;

            Graphics g = e.Graphics;

            if (_size == "QQVGA")
            {
                g.DrawString(timecode.ToLongTimeString(Camera.Server.Server.TimeZone), Manager.Font, Manager.TitleTextColor, 53, 138);
                if (_image != null)
                    g.DrawImage(_image, 5, 138, 18, 13);

                if (!Server.Server.CheckProductNoToSupport("snapshot"))
                {
                    g.DrawImage(_playImage, 40, 27, 80, 80);
                    return;
                }

                if (_snapshotImage != null)
                {
                    g.DrawImage(_snapshotImage, 0, 0, 160, 135);
                }
                else
                {
                    g.DrawImage(_snapshotLoadFailure ? _noImage : _snapshotBg, 40, 27, 80, 80);
                }
            }
            else if (_size == "HQVGA")
            {
                g.DrawString(DateTimes.ToDateTimeString(timecode, Camera.Server.Server.TimeZone), Manager.Font, Manager.TitleTextColor, 60, 178);
                if (_image != null)
                    g.DrawImage(_image, 5, 178, 18, 13);

                if (!Server.Server.CheckProductNoToSupport("snapshot"))
                {
                    g.DrawImage(_playImage, 80, 49, 80, 80);
                    return;
                }

                if (_snapshotImage != null)
                {
                    g.DrawImage(_snapshotImage, 0, 0, 240, 175);
                }
                else
                {
                    g.DrawImage(_snapshotLoadFailure ? _noImage : _snapshotBg, 80, 49, 80, 80);
                }
            }
            else
            {
                g.DrawString(DateTimes.ToDateTimeString(timecode, Camera.Server.Server.TimeZone), Manager.Font, Manager.TitleTextColor, 100, 258);
                if (_image != null)
                    g.DrawImage(_image, 5, 258, 18, 13);

                if (!Server.Server.CheckProductNoToSupport("snapshot"))
                {
                    g.DrawImage(_playImage, 120, 87, 80, 80);
                    return;
                }

                if (_snapshotImage != null)
                {
                    g.DrawImage(_snapshotImage, 0, 0, 320, 255);
                }
                else
                {
                    g.DrawImage(_snapshotLoadFailure ? _noImage : _snapshotBg, 120, 87, 80, 80);
                }
            }
        }

        private void SearchResultPanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (Parent == null) return;
            Parent.Focus();

            //reload image
            if (_snapshotImage != null || IsLoadingImage) return;

            _snapshotLoadFailure = false;
            _snapshotImage = Camera.GetSnapshot(Timecode, _defaultSnapshotSize) as Bitmap;

            if (_snapshotImage == null)
            {
                _snapshotLoadFailure = true;
            }
            else
            {
                Tag = _snapshotImage.Tag;
            }

            Invalidate();
        }

        private void SearchResultPanelLocationChanged(Object sender, EventArgs e)
        {
            CheckVisible();
        }

        private UInt16 _rightClickCount;
        private void SearchResultPanelMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            //use mouse middle to dbl-click will open IE and open image url
            if (Tag == null) return;

            if (e.Button == MouseButtons.Right)
            {
                if (String.IsNullOrEmpty(Tag.ToString())) return;
                _rightClickCount++;
                if (_rightClickCount < 2) return;

                var httpMode = (Camera.Server.Credential.SSLEnable) ? "https://" : "http://";
                var process = new System.Diagnostics.Process
                {
                    StartInfo =
                            {
                                FileName = httpMode + Camera.Server.Credential.Domain + @":" + Camera.Server.Credential.Port + @"/" + Tag
                            }
                };

                process.Start();
                _rightClickCount = 0;
                return;
            }

            if (OnSelectionChange != null)
                OnSelectionChange(this, null);
        }

        private void CheckVisible(Object sender, EventArgs e)
        {
            if (Parent == null)
            {
                if (SearchResult.QueueSearchResultPanel.Contains(this))
                {
                    try
                    {
                        SearchResult.QueueSearchResultPanel.Remove(this);
                    }
                    catch (Exception exception)//exception before, still dont know why
                    {
                        Console.WriteLine(exception);
                    }
                }
                return;
            }

            _checkLocationTimer.Enabled = false;
            if ((Location.Y + Height) < 0 || Location.Y > Parent.Height)
            {
                if (SearchResult.QueueSearchResultPanel.Contains(this))
                {
                    try
                    {
                        SearchResult.QueueSearchResultPanel.Remove(this);
                    }
                    catch (Exception exception)//exception before, still dont know why
                    {
                        Console.WriteLine(exception);
                    }
                }
                return;
            }

            IsLoad = true;
            SearchResult.QueueLoadSnapshot(this);
        }

        public void CheckVisible()
        {
            if (IsLoad) return;

            _checkLocationTimer.Enabled = false;
            _checkLocationTimer.Enabled = true;
        }

        private Boolean _isReset;
        public Boolean IsLoadingImage;
        public Boolean IsLoad;

        private static readonly Image _playImage = Resources.GetResources(Properties.Resources.play, Properties.Resources.IMGPlay);
        private static readonly Image _noImage = Resources.GetResources(Properties.Resources.no_image, Properties.Resources.IMGNo_image);
        private static readonly Image _snapshotBg = Resources.GetResources(Properties.Resources.image, Properties.Resources.IMGImage);
        //private const String CgiSnapshotWithTimecode = @"cgi-bin/snapshot?channel=channel%1&timestamp={TIMECODE}";
        private readonly Size _defaultSnapshotSize = new Size(320, 240);
        public void LoadSnapshot()
        {
            if (Camera == null || Timecode == 0)
            {
                _snapshotLoadFailure = true;
                Tag = "";
                return;
            }

            _isReset = false;
            IsLoadingImage = true;
            LocationChanged -= SearchResultPanelLocationChanged;

            //SnapshotUrl = CgiSnapshotWithTimecode.Replace("%1", Camera.Id.ToString()).Replace("{TIMECODE}", Timecode.ToString());

            _snapshotImage = Camera.GetSnapshot(Timecode, _defaultSnapshotSize);// as Bitmap;
            UInt32 retry = 1;
            while (_snapshotImage == null && retry > 0 && !_isReset)
            {
                Application.RaiseIdle(null);
                retry--;
                _snapshotImage = Camera.GetSnapshot(Timecode, _defaultSnapshotSize);// as Bitmap;
            }

            IsLoadingImage = false;
            if (_isReset)
            {
                return;
            }

            if (_snapshotImage == null)
            {
                _snapshotLoadFailure = true;
                Tag = "";
            }
            else
            {
                Tag = _snapshotImage.Tag;
            }

            Invalidate();
        }

        protected static readonly Image ManualRecord = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
        protected static readonly Image IVS = Resources.GetResources(Properties.Resources.ivs, Properties.Resources.IMGIvs);
        protected static readonly Image Motion = Resources.GetResources(Properties.Resources.motion, Properties.Resources.IMGMotion);
        protected static readonly Image DigitalInput = Resources.GetResources(Properties.Resources.di, Properties.Resources.IMGDi);
        protected static readonly Image DigitalOutput = Resources.GetResources(Properties.Resources._do, Properties.Resources.IMGDo);
        protected static readonly Image NetworkLoss = Resources.GetResources(Properties.Resources.networkloss, Properties.Resources.IMGNetworkloss);
        protected static readonly Image NetworkRecovery = Resources.GetResources(Properties.Resources.networkrecovery, Properties.Resources.IMGNetworkrecovery);
        protected static readonly Image VideoLoss = Resources.GetResources(Properties.Resources.videoloss, Properties.Resources.IMGVideoloss);
        protected static readonly Image VideoRecovery = Resources.GetResources(Properties.Resources.videorecovery, Properties.Resources.IMGVideorecovery);
        protected static readonly Image Panic = Resources.GetResources(Properties.Resources.panic, Properties.Resources.IMGPanic);
        protected static readonly Image CrossLine = Resources.GetResources(Properties.Resources.crossline, Properties.Resources.IMGCrossLine);
        protected static readonly Image IntrusionDetection = Resources.GetResources(Properties.Resources.intrusionDetection, Properties.Resources.IMGIntrusionDetection);
        protected static readonly Image LoiteringDetection = Resources.GetResources(Properties.Resources.loiteringDetection, Properties.Resources.IMGLoiteringDetection);
        protected static readonly Image ObjectCountingIn = Resources.GetResources(Properties.Resources.objectCountingIn, Properties.Resources.IMGObjectCountingIn);
        protected static readonly Image ObjectCountingOut = Resources.GetResources(Properties.Resources.objectCountingOut, Properties.Resources.IMGObjectCountingOut);
        protected static readonly Image AudioDetection = Resources.GetResources(Properties.Resources.audioDetection, Properties.Resources.IMGAudioDetection);
        protected static readonly Image TamperDetection = Resources.GetResources(Properties.Resources.temperingDetection, Properties.Resources.IMGTemperingDetection);
        protected static readonly Image Userdefined = Resources.GetResources(Properties.Resources.userdefine, Properties.Resources.IMGUserDefine);

        public void Reset()
        {
            Timecode = 0;
            Camera = null;
            IsLoad = false;
            _snapshotLoadFailure = false;
            _snapshotImage = null;
            Tag = "";
            _checkLocationTimer.Enabled = false;

            LocationChanged -= SearchResultPanelLocationChanged;
            LocationChanged += SearchResultPanelLocationChanged;

            _isReset = true;
        }

        private String _size;
        public void SmallSize()
        {
            if (_size == "QQVGA") return;

            _size = "QQVGA";
            Size = new Size(160, 155);
        }

        public void MediumSize()
        {
            if (_size == "HQVGA") return;

            _size = "HQVGA";
            Size = new Size(240, 195);
        }

        public void LargeSize()
        {
            if (_size == "QVGA") return;

            _size = "QVGA";
            Size = new Size(320, 275);
        }
    }
}
