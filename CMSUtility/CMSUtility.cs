using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Constant;
using Interface;

namespace CMSUtility
{
    public partial class CMSUtility : UserControl, IUtility
    {
        public CMSUtility()
        {
            InitializeComponent();

            _control.OnExportStatus += ControlOnExportStatus;
            _control.OnAudioOutStatus += ControlOnAudioOutStatus;
            JoystickEnabled = false;
        }

        private IServer _server;
        public IServer Server
        {
            set
            {
                _server = value;
                _control.ServerPort = _server.Credential.Port;
                _control.ServerIp = _server.Credential.Domain;
                _control.ServerUsername = _server.Credential.UserName;
                _control.ServerPassword = _server.Credential.Password;
                _control.ServerSSL = (_server.Credential.SSLEnable ? 1 : 0);
            }
            get { return _server; }
        }

        public String Version
        {
            get
            {
                return _control.Version;
            }
        }

        public String ComponentName
        {
            get
            {
                return "CMSUtility";
            }
        }

        private Boolean _isEventReceive;

        public void Quit()
        {
        }

        public void StartEventReceive()
        {
            if (_control == null) return;
            if (_server == null || String.IsNullOrEmpty(_server.Credential.Domain))
                return;

            if (_isEventReceive)
            {
                UpdateEventRecive();
                return;
            }

            //if (_server.Device.Devices.Count == 0) return;
            _isEventReceive = true;

            _control.OnServerEventReceive += ControlOnServerEventReceive;
            _control.StartEventReceive();
        }

        void ControlOnServerEventReceive(object sender, AxiCMSUtilityLib._IiCMSUtilityEvents_OnServerEventReceiveEvent e)
        {
            if (_server != null && _isEventReceive)
            {
                if (_server is ICMS)
                    ((ICMS)_server).UtilityOnServerEventReceive(e.msg);
            }
        }

        private void ControlOnAudioOutStatus(Object sender, AxiCMSUtilityLib._IiCMSUtilityEvents_OnAudioOutStatusEvent e)
        {
            if (_server == null) return;

            if (_server is ICMS)
                ((ICMS)_server).UtilityOnServerEventReceive(e.status);
        }

        public void StopEventReceive()
        {
            if (_control == null) return;

            _control.OnServerEventReceive -= ControlOnServerEventReceive;

            _control.StopEventReceive();

            _isEventReceive = false;
        }

        public void UpdateEventRecive()
        {
            if (_control == null) return;

            if (!_isEventReceive)
            {
                StartEventReceive();
                return;
            }

            //if (_server.Device.Devices.Count == 0)
            //    _control.UpdateEventRecive("");
            //else
            //{
            //    var list = (from obj in _server.Device.Devices where obj.Value is ICamera select "channel" + obj.Value.Id);
            //    _control.UpdateEventRecive(String.Join(",", list.ToArray()));
            //}
        }

        public void GetAllChannelStatus()
        {
            if (_control == null || !_isEventReceive) return;

            try
            {
                _control.GetAllChannelStatus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void GetAllNVRStatus()
        {
            if (_control == null || !_isEventReceive) return;

            try
            {
                _control.GetAllNVRStatus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void PlaySystemSound(UInt16 times, UInt16 duration, UInt16 interval)
        {
            if (_control == null) return;

            _control.PlaySystemSound(times, duration, interval);
        }

        private Boolean _isAudioOut;
        public Int32 AudioOutChannelCount { get; private set; }
        public Int32 StartAudioTransfer(String channels)
        {
            if (_control == null) return 0;

            if (_isAudioOut)
                StopAudioTransfer();
            AudioOutChannelCount = channels.Split(',').Length;

            _isAudioOut = true;
            Int32 result = _control.StartAudioTransfer(channels);

            //No Microphone Device
            if (result == 0)
                StopAudioTransfer();

            //Console.WriteLine("StartAudioTransfer " + channels + " , result " + result);

            return result;
        }
        public void StopAudioTransfer()
        {
            if (_control == null) return;

            AudioOutChannelCount = 0;
            _control.StopAudioTransfer();

            //Console.WriteLine("StopAudioTransfer");

            _isAudioOut = false;
        }

        public void InitFisheyeLibrary(ICamera camera, bool dewarpEnable, short mountType)
        {

        }

        private ICamera _exportingCamera;
        public String ExportVideo(ICamera camera, UInt64 startTime, UInt64 endTime, Boolean displayOsd, Boolean audioIn, Boolean audioOut, UInt16 encode, String path, UInt16 quality, UInt16 scale, UInt16 osdWatermark) //Boolean encode
        {
            String filename = "";
            if (_control == null) return filename;

            if (endTime < startTime)
            {
                var tmp = endTime;
                endTime = startTime;
                startTime = tmp;
            }

            String format = "video";
            if (audioIn)
                format += ",audio1";
            if (audioOut)
                format += ",audio2";

            _exportingCamera = camera;

            //path "C:\\"
            _control.SetExportAVITimeZone(camera.Server.Server.TimeZone);

            var osd = new List<String>();

            foreach (VideoWindowTitleBarInformation information in camera.Server.Configure.VideoWindowTitleBarInformations)
            {
                switch (information)
                {
                    case VideoWindowTitleBarInformation.CameraName:
                        osd.Add(camera.ToString());
                        break;

                    //case VideoWindowTitleBarInformation.Compression:
                    //    osd.Add("%%type");
                    //    break;

                    //case VideoWindowTitleBarInformation.Resolution:
                    //    osd.Add("%%res");
                    //    break;

                    // playback dont have bitrate
                    //case VideoWindowTitleBarInformation.Bitrate:
                    //        osd.Add("%%bitrate");
                    //    break;

                    //case VideoWindowTitleBarInformation.FPS:
                    //        osd.Add("%%fps");
                    //    break;

                    case VideoWindowTitleBarInformation.DateTime:
                        osd.Add("%Y-%m-%d %H:%M:%S");// %%ms
                        break;
                }
            }

            String name = Regex.Replace(camera.ToString(), "[^a-zA-Z0-9 \\-]", "").Trim();

            osdWatermark = osdWatermark == 1 ? (ushort)3 : (ushort)2;
            int fontSize = Server.Configure.WatermarkFontSize;
            //string color = Server.Configure.WatermarkFontColor.Substring(1, Server.Configure.WatermarkFontColor.Length - 1);
            //string color2 = string.Join("", color.Reverse().Select(c => c.ToString()).ToArray());

            var bghtml = Server.Configure.WatermarkFontColor.Replace("#", "");
            var r = bghtml.Substring(0, 2);
            var g = bghtml.Substring(2, 2);
            var b = bghtml.Substring(4, 2);
            int fontColor = (int) UInt32.Parse(b + g + r, NumberStyles.HexNumber);

            //int fontColor;
            //if (!int.TryParse(ConfigurationManager.AppSettings.Get("FontColor"), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out fontColor))
            //if (!int.TryParse(color2, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out fontColor))
            //{
            //    fontColor = Int32.Parse("FFFFFF", NumberStyles.HexNumber);
            //}

            //var waterMarkText = ConfigurationManager.AppSettings.Get("WaterMarkText");
            var waterMarkText = Server.Configure.WatermarkText;

            _control.ExportFile(
                Convert.ToUInt32(startTime / 1000), //ULONG BeginTime  //Millisecond -> Second
                Convert.ToUInt32(endTime / 1000), //ULONG EndTime      //Millisecond -> Second
                camera.Server.Id,
                camera.PlaybackSessionId,
                camera.Id, //LONG CameraID
                format, //BSTR track
                path.Trim(),//BSTR strSaveLoc
                name, //BSTR prefix
                Convert.ToUInt64(camera.Server.Configure.ExportVideoMaxiFileSize * 1024 * 1024),//BYTE   ULONGLONG split_size
                encode, //((encode) ? 2 : 1), //LONG type 0:raw, 1:original, 2:mjpeg
                quality, //LONG jpeg_quality 1~100
                scale, //LONG resize_type 0:original 1:1/2,  2:1/4,  3:1/8  4:1/16
                osdWatermark, //LONG bOSD_Watermark 0:disable 1:enable osd, 2:enable watermark, 3:enable osd and watermark
                String.Join("   ", osd.ToArray()), //BSTR strOSDText
                Server.Configure.WatermarkFontFamify, //BSTR strWatermarkFont"
                fontSize, //LONG WatermarkFontSize
                fontColor, //LONG WatermarkFontColor
                waterMarkText);

            // NOTE: CMSUtility create the file, AP cannot assign the file name. 
            // CMS Export will append exporting interval at the end of the file name
            filename = string.Format("NVR{0}_{1}_{2}_{3}.avi", camera.Server.Id, name,
                DateTimes.ToDateTime(startTime, camera.Server.Server.TimeZone).ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture),
                DateTimes.ToDateTime(endTime, camera.Server.Server.TimeZone).ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture));

            return filename;
        }

        public void StopExportVideo()
        {
            if (_control == null) return;

            _control.StopExportFile();
            _exportingCamera = null;
        }

        private void ControlOnExportStatus(object sender, AxiCMSUtilityLib._IiCMSUtilityEvents_OnExportStatusEvent e)
        {
            if (_exportingCamera == null) return;

            UInt16 progress = Convert.ToUInt16(e.progress);
            var status = (ExportVideoStatus)Convert.ToUInt16(e.status);

            ICamera camera = _exportingCamera;
            _exportingCamera.ExportVideoProgress(progress, status);

            if (progress == 100 && status == ExportVideoStatus.ExportFinished && _exportingCamera == camera)
                _exportingCamera = null;
        }

        public void UploadPack(String fileName)
        {
        }

        public void StopUploadPack(String fileName)
        {
        }
    }
}
