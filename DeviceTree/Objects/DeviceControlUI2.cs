using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace DeviceTree.Objects
{
    public class DeviceControlUI2 : Label, IDeviceControl
    {
        private readonly ILogger _logger = LoggerManager.Instance.GetLogger();

        public event MouseEventHandler OnDeviceMouseDrag;
        public event MouseEventHandler OnDeviceMouseDown;
        public event MouseEventHandler OnDeviceMouseDoubleClick;
        public Boolean isShowNVRName = true;
        public IPTS PTS;
        protected IDevice _device;
        public IDevice Device
        {
            get { return _device; }
            set
            {
                _device = value;

                UpdateToolTips();
            }
        }

        private static readonly Image _record = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
        private static readonly Image _normal = Resources.GetResources(Properties.Resources.normal, Properties.Resources.IMGNormal);
        private static readonly Image _online = Resources.GetResources(Properties.Resources.online, Properties.Resources.IMGOnLine);
        private static readonly Image _new = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
        private static readonly Image _modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
        private static readonly Image _bg = Resources.GetResources(Properties.Resources.devicePanelBG, Properties.Resources.IMGDevicePanelBG);
        private static readonly Image GPS = Properties.Resources.gps_device;


        public DeviceControlUI2()
        {
            Dock = DockStyle.Top;
            DoubleBuffered = true;
            Height = 31;

            BackColor = Color.FromArgb(46, 49, 55);//drag label will use it
            ForeColor = Color.DarkGray; //drag label will use it

            BackgroundImage = _bg;

            BackgroundImageLayout = ImageLayout.Center;
            Cursor = Cursors.Hand;

            MouseDoubleClick += DeviceControlMouseDoubleClick;
            MouseUp += DeviceControlMouseUp;
            MouseDown += DeviceControlMouseDown;

            Paint += DeviceControlPaint;
        }

        protected readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        protected virtual void DeviceControlPaint(Object sender, PaintEventArgs e)
        {
            if (_device == null) return;

            Graphics g = e.Graphics;

            if (_device.ReadyState == ReadyState.New)
            {
                g.DrawImage(_new, 4, 7);
            }
            else
            {
                if (_device.ReadyState == ReadyState.Modify)
                    g.DrawImage(_modify, 4, 7);

                ICamera camera = null;
                if (_device is ICamera)
                {
                    camera = _device as ICamera;
                }
                //----------------------------------------------------------------------
                //layout and crop have to record status;
                if (_device is ISubLayout)
                {
                    var subLayout = _device as ISubLayout;

                    var id = SubLayoutUtility.CheckSubLayoutRelativeCamera(subLayout);
                    if (id > 0 && _device.Server.Device.Devices.ContainsKey(Convert.ToUInt16(id)))
                    {
                        camera = _device.Server.Device.Devices[Convert.ToUInt16(id)] as ICamera;
                    }
                }
                //----------------------------------------------------------------------
                if (_device is IDeviceLayout)
                {
                    var layout = _device as IDeviceLayout;
                    foreach (var device in layout.Items.Where(device => device != null))
                    {
                        camera = device as ICamera;
                        break;
                    }
                }
                //----------------------------------------------------------------------
                if (_device.DeviceType == DeviceType.GPS)
                {
                    g.DrawImage(GPS, 24, 7);
                }
                else if (camera != null && camera.ReadyState != ReadyState.New)
                {
                    switch (camera.Status)
                    {
                        case CameraStatus.Recording:
                            g.DrawImage(_record, 24, 7);
                            break;

                        case CameraStatus.Streaming:
                            g.DrawImage(_online, 24, 7);
                            break;

                        default:
                            g.DrawImage(_normal, 24, 7);
                            break;
                    }
                }
            }

            var label = GetLabel(_device);

            g.DrawString(label, _font, Brushes.DarkGray, 45, 8);
        }

        protected virtual string GetLabel(IDevice device)
        {
            var label = device.ToString();

            //check if CMS, add nvr name behine
            var nvrDevice = device as ICamera;
            if (nvrDevice != null && nvrDevice.CMS != null && isShowNVRName)
                label += String.Format(" ({0})", nvrDevice.Server);

            return label;
        }

        private Point _position;
        protected void DeviceControlMouseDown(Object sender, MouseEventArgs e)
        {
            _position = e.Location;
            MouseMove -= DeviceControlMouseMove;
            MouseMove += DeviceControlMouseMove;
        }

        protected void DeviceControlMouseUp(Object sender, MouseEventArgs e)
        {
            MouseMove -= DeviceControlMouseMove;
            if (OnDeviceMouseDown != null)
                OnDeviceMouseDown(this, e);
        }

        private void DeviceControlMouseMove(Object sender, MouseEventArgs e)
        {
            if (e.X == _position.X && e.Y == _position.Y) return;

            if (OnDeviceMouseDrag != null)
            {
                MouseMove -= DeviceControlMouseMove;
                OnDeviceMouseDrag(this, e);
            }
        }

        protected void DeviceControlMouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (OnDeviceMouseDoubleClick != null)
                OnDeviceMouseDoubleClick(this, e);
        }

        public void UpdateToolTips()
        {
            String toolTips = String.Empty;

            if (_device != null)
            {
                var camera = _device as ICamera;
                if (camera != null && camera.DeviceType != DeviceType.GPS)
                {
                    try
                    {
                        toolTips = _device + ((_device.Server.Id != 0) ? @" (" + _device.Server + @")" : "");

                        if (!String.IsNullOrEmpty(camera.Profile.NetworkAddress))
                        {
                            toolTips += Environment.NewLine + camera.Profile.NetworkAddress;
                        }

                        if (camera.StreamConfig != null)
                        {
                            if (camera.StreamConfig.Compression != Compression.Off)
                            {
                                toolTips += Environment.NewLine + Compressions.ToDisplayString(camera.StreamConfig.Compression);
                                switch (camera.StreamConfig.Compression)
                                {
                                    case Compression.Mjpeg:
                                        toolTips += ", " + camera.StreamConfig.VideoQuality;
                                        break;

                                    case Compression.H264:
                                    case Compression.Mpeg4:
                                        toolTips += ", " + Bitrates.ToDisplayString(camera.StreamConfig.Bitrate);
                                        break;
                                }

                                if (camera.StreamConfig.Resolution != Resolution.NA)
                                    toolTips += Environment.NewLine + Resolutions.ToString(camera.StreamConfig.Resolution);
                            }
                        }

                        if (_device.Server != null && _device.Server.Server != null)
                        {
                            if (camera.Model.Manufacture != "UNKNOWN")
                                toolTips += Environment.NewLine + _device.Server.Server.DisplayManufactures(camera.Model.Manufacture);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);

                        toolTips = _device.ToString();
                    }
                }
                else
                {
                    toolTips = _device.ToString();
                }
            }

            SharedToolTips.SharedToolTip.SetToolTip(this, toolTips);
        }
    }
}
