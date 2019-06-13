using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace SetupNVR
{
    public sealed class DevicePanel : Panel
    {
        public event EventHandler OnDeviceEditClick;
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        private static readonly Dictionary<String, String> Localization;
        //public event EventHandler<EventArgs<String>> OnSortChange;

        private readonly  CheckBox _checkBox = new CheckBox();

        public IServer Server;
        public ICMS CMS;
        public String DataType = "Information";
        public Boolean IsTitle;
        public IDevice Device;


        static DevicePanel()
        {
            Localization = new Dictionary<String, String>
            {
                {"Common_Sec", "Sec"},
                {"Common_Min", "Min"},
                {"Common_Hr", "Hr"},

                {"Control_PeopleCounting", "PeopleCounting"},

                {"DevicePanel_ID", "ID"},
                {"DevicePanel_Name", "Name"},
                {"DevicePanel_NetworkAddress", "Network Address"},
                {"DevicePanel_Compression", "Compression"},
                {"DevicePanel_Resolution", "Resolution"},
                {"DevicePanel_FPS", "FPS"},
                {"DevicePanel_Protocol", "Protocol"},
                {"DevicePanel_LiveStream", "Live Stream"},
                {"DevicePanel_Manufacturer", "Manufacturer"},
                {"DevicePanel_Model", "Model"},
                {"DevicePanel_InUse", "(In use)"},

                {"DevicePanel_VideoRecording", "Video Recording"},
                {"DevicePanel_EventHandling", "Event Handling"},
                {"DevicePanel_PreEventRecording", "Pre-event Recording"},
                {"DevicePanel_PostEventRecording", "Post-event Recording"},

                {"DeviceManager_NoHandler", "No handler"},

                {"Schedule_CustomSchedule", "Custom schedule"},
                {"Schedule_FullTime", "Full Time"},
                {"Schedule_WorkingTime", "Working Hours"},
                {"Schedule_WorkingDay", "Working Day"},
                {"Schedule_NonWorkingTime", "Non-working Hours"},
                {"Schedule_NonWorkingDay", "Non-working Day"},
                {"Schedule_NoSchedule", "No schedule"},
                {"Schedule_FullTimeEventRecording", "Full time event recording"},
                {"Schedule_WithEventRecording", "(with event recording)"},
            };
            Localizations.Update(Localization);
        }

        public DevicePanel()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;
            
            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += DevicePanelMouseClick;
            Paint += DevicePanelPaint;
        }

        private static RectangleF _nameRectangleF = new RectangleF(74, 13, 126, 17);
        private static RectangleF _nameRectangleF2 = new RectangleF(44, 13, 126, 17);
        private static RectangleF _addressRectangleF = new RectangleF(200, 13, 160, 17);

        private void DevicePanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;
            
            Graphics g = e.Graphics;

            if(IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                return;
            }

            Manager.Paint(g, this);

            if (_editVisible)
                Manager.PaintEdit(g, this);

            if (Device == null) return;

            Manager.PaintStatus(g, Device.ReadyState);

            Brush fontBrush = Brushes.Black;
            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 200) return;
            
            if (DataType == "SearchResult")
            {
                Manager.PaintText(g, (Parent.Controls.Count - Parent.Controls.IndexOf(this) - 1).ToString().PadLeft(2, '0'));
                g.DrawString(Device.Name, Manager.Font, fontBrush, _nameRectangleF);
            }
            else
            {
                if ((Server.Configure != null) ? Server.Configure.DisplayDeviceId : true)
                {
                    Manager.PaintText(g, Device.Id.ToString().PadLeft(2, '0'));
                    g.DrawString(Device.Name, Manager.Font, fontBrush, _nameRectangleF);
                }
                else
                    g.DrawString(Device.Name, Manager.Font, fontBrush, _nameRectangleF2);
            }

            if (Width <= 320) return;

            var camera = Device as ICamera;
            if (camera == null) return;

            switch (DataType)
            {
                case "Information":
                case "SearchResult":
                    if (camera.CMS != null)
                    {
                        PaintCMSDevice(g, camera, fontBrush);
                    }
                    else
                    {
                        PaintInformation(g, camera, fontBrush);
                    }
                    break;

                case "Schedule":
                    PaintSchedule(g, camera, fontBrush);
                    break;

                case "EventHandle":
                    PaintEventHandle(g, camera);
                    break;
            }
        }

        private void PaintInformation(Graphics g, ICamera camera, Brush fontBrush)
        {
            if (camera.Model == null) return;
            String address = camera.Profile.NetworkAddress;

            if (camera.IsInUse)
                address += Localization["DevicePanel_InUse"];

            g.DrawString(((!String.IsNullOrEmpty(address) ? address : "N/A")), Manager.Font, fontBrush, _addressRectangleF);

            if (Width <= 420) return;
            if (camera.StreamConfig != null && camera.StreamConfig.Compression != Compression.Off)
            {
                var compression = Compressions.ToDisplayString(camera.StreamConfig.Compression);
                switch (camera.StreamConfig.Compression)
                {
                    case Compression.Mjpeg:
                        compression += "," + camera.StreamConfig.VideoQuality;
                        break;

                    case Compression.H264:
                    case Compression.Mpeg4:
                        compression += ", " + Bitrates.ToDisplayString(camera.StreamConfig.Bitrate);
                        break;

                    default:
                        compression = "N/A";
                        break;
                }

                g.DrawString(compression, Manager.Font, fontBrush, 350, 13);

                if (Width <= 540) return;
                g.DrawString(Resolutions.ToString(camera.StreamConfig.Resolution), Manager.Font, fontBrush, 460, 13);

                if (Width <= 640) return;
                var framerate = camera.StreamConfig.Framerate == 0 ? "N/A" : camera.StreamConfig.Framerate.ToString();
                g.DrawString(framerate, Manager.Font, fontBrush, 570, 13);

                if (Width <= 750) return;
                var protocol = ConnectionProtocols.ToDisplayString(camera.StreamConfig.ConnectionProtocol);
                g.DrawString((!String.IsNullOrEmpty(protocol) ? protocol : "N/A"), Manager.Font, fontBrush, 650, 13);

                if (Width <= 855) return;
                g.DrawString(camera.Profile.StreamId.ToString(), Manager.Font, fontBrush, 750, 13);
            }
            else
            {
                g.DrawString("N/A", Manager.Font, fontBrush, 350, 13);

                if (Width <= 540) return;
                g.DrawString("N/A", Manager.Font, fontBrush, 460, 13);

                if (Width <= 640) return;
                g.DrawString("N/A", Manager.Font, fontBrush, 570, 13);

                if (Width <= 750) return;
                if (camera.StreamConfig != null)
                {
                    var protocol = ConnectionProtocols.ToDisplayString(camera.StreamConfig.ConnectionProtocol);
                    g.DrawString((!String.IsNullOrEmpty(protocol) ? protocol : "N/A"), Manager.Font, fontBrush, 650, 13);
                }
                else
                    g.DrawString("N/A", Manager.Font, fontBrush, 650, 13);

                if (Width <= 855) return;
                //isap cloud camera have no streamId, set as 0
                if (camera.Model.Type == "CloudCamera")
                    g.DrawString("0", Manager.Font, fontBrush, 750, 13);
                else
                    g.DrawString(camera.Profile.StreamId.ToString(), Manager.Font, fontBrush, 750, 13);
            }

            if (Width <= 960) return;
            g.DrawString(Server.Server.DisplayManufactures(camera.Model.Manufacture), Manager.Font, fontBrush, 860, 13);

            if (Width <= 1080) return;
            g.DrawString(camera.Model.Model, Manager.Font, fontBrush, 970, 13);
        }

        private void PaintCMSDevice(Graphics g, ICamera camera, Brush fontBrush)
        {
            if (camera.Model == null) return;
            String address = camera.Profile.NetworkAddress;

            if (camera.IsInUse)
                address += Localization["DevicePanel_InUse"];

            g.DrawString(((!String.IsNullOrEmpty(address) ? address : "N/A")), Manager.Font, fontBrush, _addressRectangleF);

            if (Width <= 420) return;
            if (camera.StreamConfig != null && camera.StreamConfig.Compression != Compression.Off)
            {
                g.DrawString(camera.Profile.StreamId.ToString(), Manager.Font, fontBrush, 350, 13);
            }
            else
            {
                //isap cloud camera have no streamId, set as 0
                if (camera.Model.Type == "CloudCamera")
                    g.DrawString("0", Manager.Font, fontBrush, 750, 13);
                else
                    g.DrawString(camera.Profile.StreamId.ToString(), Manager.Font, fontBrush, 350, 13);
            }

            if (Width <= 570) return;
            g.DrawString(Server.Server.DisplayManufactures(camera.Model.Manufacture), Manager.Font, fontBrush, 460, 13);

            if (Width <= 690) return;
            g.DrawString(camera.Model.Model, Manager.Font, fontBrush, 570, 13);
        }

        private void PaintSchedule(Graphics g, ICamera camera, Brush fontBrush)
        {
            if (Server is IVAS)
            {
                if (camera.EventSchedule != null)
                    g.DrawString(RecordScheduleToLocalizationString(camera.EventSchedule.Description), Manager.Font, fontBrush, 200, 13);
            }
            else
            {
                if (camera.RecordSchedule != null)
                    g.DrawString(RecordScheduleToLocalizationString(camera.RecordSchedule.Description), Manager.Font, fontBrush, 200, 13);

                if (Width <= 610) return;
                if (camera.EventSchedule != null)
                    g.DrawString(RecordScheduleToLocalizationString(camera.EventSchedule.Description), Manager.Font, fontBrush, 460, 13);

                if (Width <= 710) return;
                g.DrawString(DurationToText(camera.PreRecord / 1000), Manager.Font, Brushes.Black, 610, 13);

                if (Width <= 850) return;
                g.DrawString(DurationToText(camera.PostRecord / 1000), Manager.Font, Brushes.Black, 750, 13);
            }
        }

        private void PaintEventHandle(Graphics g, ICamera camera)
        {
            g.DrawString(EventHandlerToString(camera.EventHandling), Manager.Font, Brushes.Black, 200, 13);
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 200) return;

            if ((Server.Configure != null) ? Server.Configure.DisplayDeviceId : true)
            {
                Manager.PaintTitleText(g, Localization["DevicePanel_ID"]);

                g.DrawString(Localization["DevicePanel_Name"], Manager.Font, Manager.TitleTextColor, 74, 13);
            }
            else
                Manager.PaintTitleText(g, Localization["DevicePanel_Name"]);

            if (Width <= 320) return;

            switch (DataType)
            {
                case "Information":
                case "SearchResult":
                    if (CMS != null)
                    {
                        g.DrawString(Localization["DevicePanel_NetworkAddress"], Manager.Font, Manager.TitleTextColor, 200, 13);

                        if (Width <= 420) return;
                        g.DrawString(Localization["DevicePanel_LiveStream"], Manager.Font, Manager.TitleTextColor, 350, 13);

                        if (Width <= 540) return;
                        g.DrawString(Localization["DevicePanel_Manufacturer"], Manager.Font, Manager.TitleTextColor, 460, 13);

                        if (Width <= 640) return;
                        g.DrawString(Localization["DevicePanel_Model"], Manager.Font, Manager.TitleTextColor, 570, 13);
                    }
                    else
                    {
                        g.DrawString(Localization["DevicePanel_NetworkAddress"], Manager.Font, Manager.TitleTextColor, 200, 13);

                        if (Width <= 420) return;
                        g.DrawString(Localization["DevicePanel_Compression"], Manager.Font, Manager.TitleTextColor, 350, 13);

                        if (Width <= 540) return;
                        g.DrawString(Localization["DevicePanel_Resolution"], Manager.Font, Manager.TitleTextColor, 460, 13);

                        if (Width <= 640) return;
                        g.DrawString(Localization["DevicePanel_FPS"], Manager.Font, Manager.TitleTextColor, 570, 13);

                        if (Width <= 750) return;
                        g.DrawString(Localization["DevicePanel_Protocol"], Manager.Font, Manager.TitleTextColor, 650, 13);

                        if (Width <= 855) return;
                        g.DrawString(Localization["DevicePanel_LiveStream"], Manager.Font, Manager.TitleTextColor, 750, 13);

                        if (Width <= 960) return;
                        g.DrawString(Localization["DevicePanel_Manufacturer"], Manager.Font, Manager.TitleTextColor, 860, 13);

                        if (Width <= 1080) return;
                        g.DrawString(Localization["DevicePanel_Model"], Manager.Font, Manager.TitleTextColor, 970, 13);
                    }
                    
                    break;

                case "Schedule":
                    if (Server is IVAS)
                    {
                        g.DrawString(Localization["Control_PeopleCounting"], Manager.Font, Manager.TitleTextColor, 200, 13);
                    }
                    else
                    {
                        g.DrawString(Localization["DevicePanel_VideoRecording"], Manager.Font, Manager.TitleTextColor, 200, 13);

                        if (Width <= 610) return;
                        g.DrawString(Localization["DevicePanel_EventHandling"], Manager.Font, Manager.TitleTextColor, 460, 13);

                        if (Width <= 710) return;
                        g.DrawString(Localization["DevicePanel_PreEventRecording"], Manager.Font, Manager.TitleTextColor, 610, 13);

                        if (Width <= 850) return;
                        g.DrawString(Localization["DevicePanel_PostEventRecording"], Manager.Font, Manager.TitleTextColor, 750, 13);
                    }
                    break;

                case "EventHandle":
                    g.DrawString(Localization["DevicePanel_EventHandling"], Manager.Font, Manager.TitleTextColor, 200, 13);
                    break;
            }
        }

        public String RecordScheduleToLocalizationString(ScheduleMode mode)
        {
            switch (mode)
            {
                case ScheduleMode.CustomSchedule:
                    return Localization["Schedule_CustomSchedule"];

                case ScheduleMode.FullTimeEventHandling:
                case ScheduleMode.FullTimeRecording:
                    return Localization["Schedule_FullTime"];

                case ScheduleMode.NoSchedule:
                    return Localization["Schedule_NoSchedule"];

                case ScheduleMode.WorkingTimeEventHandling:
                case ScheduleMode.WorkingTimeRecording:
                    return Localization["Schedule_WorkingTime"];

                case ScheduleMode.WorkingDayEventHandling:
                case ScheduleMode.WorkingDayRecording:
                    return Localization["Schedule_WorkingDay"];

                case ScheduleMode.NonWorkingTimeEventHandling:
                case ScheduleMode.NonWorkingTimeRecording:
                    return Localization["Schedule_NonWorkingTime"];

                case ScheduleMode.NonWorkingDayEventHandling:
                case ScheduleMode.NonWorkingDayRecording:
                    return Localization["Schedule_NonWorkingDay"];

                case ScheduleMode.FullTimeEventRecording:
                    return Localization["Schedule_FullTimeEventRecording"];

                case ScheduleMode.WorkingTimeRecordingWithEventRecording:
                    return Localization["Schedule_WorkingTime"] + Localization["Schedule_WithEventRecording"];

                case ScheduleMode.WorkingDayRecordingWithEventRecording:
                    return Localization["Schedule_WorkingDay"] + Localization["Schedule_WithEventRecording"];

                case ScheduleMode.NonWorkingTimeRecordingWithEventRecording:
                    return Localization["Schedule_NonWorkingTime"] + Localization["Schedule_WithEventRecording"];

                case ScheduleMode.NonWorkingDayRecordingWithEventRecording:
                    return Localization["Schedule_NonWorkingDay"] + Localization["Schedule_WithEventRecording"];
            }

            return Localization["Schedule_CustomSchedule"];
        }

        public String EventHandlerToString(EventHandling eventHandling)
        {
            var str = new List<String>();

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.Motion)
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.DigitalInput)
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.VideoLoss)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.VideoRecovery)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.NetworkLoss)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.NetworkRecovery)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.UserDefine)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.Panic)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.CrossLine)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.IntrusionDetection)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.LoiteringDetection)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.ObjectCountingIn)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.ObjectCountingOut)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.AudioDetection)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            foreach (KeyValuePair<EventCondition, List<EventHandle>> obj in eventHandling)
            {
                if (obj.Value.Count == 0) continue;

                if (obj.Key.CameraEvent.Type == EventType.TamperDetection)
                {
                    str.Add(obj.Key.CameraEvent.ToLocalizationString());
                    break;
                }
            }

            if (str.Count > 0)
            {
                return String.Join(", ", str.ToArray());
            }

            return Localization["DeviceManager_NoHandler"];
        }

        private String DurationToText(UInt32 duration)
        {
            if (duration < 60)
                return SecToStr(duration);
            if (duration < 3600)
                return MinToStr(duration);
            
            return HrToStr(duration);
        }

        private String SecToStr(UInt32 sec)
        {
            return sec + Localization["Common_Sec"];
        }

        private String MinToStr(UInt32 min)
        {
            String str = "";
            if (min >= 60)
                str = (min / 60) + Localization["Common_Min"];

            if (min % 60 != 0)
                str += " " + SecToStr(min % 60);

            return str;
        }

        private String HrToStr(UInt32 hr)
        {
            String str = (hr / 3600) + Localization["Common_Hr"];
            if (hr % 3600 != 0)
                str += " " + MinToStr(hr % 3600);

            return str;
        }

        //private String _sortItem;
        //private Boolean _sortAsc;

        //private Rectangle _nameRectangle = new Rectangle(44, 0, 136, 40);
        //private Rectangle _addressRectangle = new Rectangle(180, 0, 140, 40);
        //private const String OnSortChangeXml = "<XML><Order>{ORDER}</Order><Item>{ITEM}</Item></XML>";
        private void DevicePanelMouseClick(Object sender, MouseEventArgs e)
        {
            if (IsTitle)
            {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
                    return;
                }
                /*String item = "";
                if (_nameRectangle.Contains(e.X, e.Y))
                {
                    item = "Name";
                }
                else if (_addressRectangle.Contains(e.X, e.Y))
                {
                    item = "Address";
                }

                if(item == "") return;

                if (_sortItem == item)
                    _sortAsc = !_sortAsc;
                else
                    _sortAsc = true;

                _sortItem = item;
                if(OnSortChange!=null)
                    OnSortChange(this, new EventArgs<String>(OnSortChangeXml.Replace("{ORDER}", (_sortAsc) ? "asc" : "desc").Replace("{ITEM}", item)));*/
            }
            else
            {
                //if (_checkBox.Visible)
                //{
                //    _checkBox.Checked = !_checkBox.Checked;
                //    return;
                //}
                if (OnDeviceEditClick != null)
                    OnDeviceEditClick(this, e);
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();

            if(IsTitle)
            {
                if (Checked && OnSelectAll != null)
                    OnSelectAll(this, null);
                else if (!Checked && OnSelectNone != null)
                    OnSelectNone(this, null);

                return;
            }
            
            _checkBox.Focus();
            if (OnSelectChange != null)
                OnSelectChange(this, null);
        }

        public Brush SelectedColor = Manager.SelectedTextColor;

        public Boolean Checked
        {
            get
            {
                return _checkBox.Checked;
            }
            set
            {
                _checkBox.Checked = value;
            }
        }

        public Boolean SelectionVisible
        {
            set{ _checkBox.Visible = value; }
        }

        private Boolean _editVisible;
        public Boolean EditVisible { 
            set
            {
                _editVisible = value;
                Invalidate();
            }
        }
    }
}
