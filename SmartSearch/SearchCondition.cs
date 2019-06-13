using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Constant.Utility;
using Device;
using DeviceConstant;
using Interface;
using PanelBase;

namespace SmartSearch
{
    public sealed partial class SearchCondition : UserControl, IControl, IAppUse, IServerUse
    {
        public event EventHandler OnSearchStart;

        public event EventHandler<EventArgs<String>> OnSearchStartDateTimeChange;
        public event EventHandler<EventArgs<String>> OnSearchEndDateTimeChange;

        public event EventHandler<EventArgs<String>> OnSmartSearchResult;
        public event EventHandler OnSmartSearchComplete;

        public SmartSearch SmartSearchControl;
        public IVideoWindow VideoWindow;
        public Dictionary<String, String> Localization;

        public String TitleName { get; set; }

        public IApp App { get; set; }
        public IServer Server { get; set; }

        private readonly List<UInt32> _periodList = new List<UInt32> { 1, 5, 10, 30, 
																	   60, 300, 600, 1800,
																	   3600, 86400};

        public SearchCondition()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Control_SmartSearch", "Smart Search"},
								   
								   {"Common_Sec", "Sec"},
								   {"Common_Min", "Min"},
								   {"Common_Hr", "Hr"},
								   {"Common_Day", "Day"},

								   
								   {"MessageBox_Information", "Information"},
								   {"SmartSearch_NVR", "NVR"},
								   {"SmartSearch_Device", "Device"},
								   {"SmartSearch_Start", "Start"},
								   {"SmartSearch_End", "End"},
								   {"SmartSearch_Search", "Search"},
								   {"SmartSearch_Stop", "Stop"},
								   {"SmartSearch_NoCriteria", "Please select search criteria."},
								   {"SmartSearch_Period", "Period"},
								   {"SmartSearch_IVS", "IVS"},
								   {"SmartSearch_IVSMotionDetection", "Motion Detection"},
								   {"SmartSearch_Event", "Event"},
								   {"SmartSearch_SetupAnalysisRange", "Setup analysis range"},

								   {"Event_Motion", "Motion"},
								   {"Event_DigitalInput", "Digital Input"},
								   {"Event_DigitalOutput", "Digital Output"},
								   {"Event_NetworkLoss", "Network Lost"},
								   {"Event_NetworkRecovery", "Network Recovery"},
								   {"Event_VideoLoss", "Video Lost"},
								   {"Event_VideoRecovery", "Video Recovery"},
								   {"Event_Panic", "Panic Button"},
								   {"Event_ManualRecord", "Manual Record"},
								   {"Event_CrossLine", "Cross Line"},
                                   {"Event_IntrusionDetection", "Intrusion Detection"},
								   {"Event_LoiteringDetection", "Loitering Detection"},
                                   {"Event_ObjectCountingIn", "Object Counting In"},
								   {"Event_ObjectCountingOut", "Object Counting Out"},
                                   {"Event_AudioDetection", "Audio Detection"},
								   {"Event_TamperDetection", "Tamper Detection"},
                                   {"Event_UserDefined", "User Defined"}
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            stopButton.Location = searchButton.Location;

            nvrLabel.Text = Localization["SmartSearch_NVR"];
            deviceLabel.Text = Localization["SmartSearch_Device"];
            startLabel.Text = Localization["SmartSearch_Start"];
            endLabel.Text = Localization["SmartSearch_End"];
            searchButton.Text = Localization["SmartSearch_Search"];
            stopButton.Text = Localization["SmartSearch_Stop"];
            periodLabel.Text = Localization["SmartSearch_Period"];
            ivsLabel.Text = Localization["SmartSearch_IVS"];
            ivsMotionCheckBox.Text = Localization["SmartSearch_IVSMotionDetection"];
            eventLabel.Text = Localization["SmartSearch_Event"];

            motionCheckBox.Text = Localization["Event_Motion"];
            diCheckBox.Text = Localization["Event_DigitalInput"];
            doCheckBox.Text = Localization["Event_DigitalOutput"];
            networkLossCheckBox.Text = Localization["Event_NetworkLoss"];
            networkRecoveryCheckBox.Text = Localization["Event_NetworkRecovery"];
            videoLossCheckBox.Text = Localization["Event_VideoLoss"];
            videoRecoveryCheckBox.Text = Localization["Event_VideoRecovery"];
            panicCheckBox.Text = Localization["Event_Panic"];
            manualRecordCheckBox.Text = Localization["Event_ManualRecord"];
            crossLineCheckBox.Text = Localization["Event_CrossLine"];
            intrusionDetectionCheckBox.Text = Localization["Event_IntrusionDetection"];
            loiteringDetectionCheckBox.Text = Localization["Event_LoiteringDetection"];
            objectCountingInCheckBox.Text = Localization["Event_ObjectCountingIn"];
            objectCountingOutCheckBox.Text = Localization["Event_ObjectCountingOut"];
            audioDetectionCheckBox.Text = Localization["Event_AudioDetection"];
            temperingDetectionCheckBox.Text = Localization["Event_TamperDetection"];
            userDefineCheckBox.Text = Localization["Event_UserDefined"];

            SharedToolTips.SharedToolTip.SetToolTip(ivsMotionCheckBox, Localization["SmartSearch_IVSMotionDetection"]);
            SharedToolTips.SharedToolTip.SetToolTip(motionCheckBox, Localization["Event_Motion"]);
            SharedToolTips.SharedToolTip.SetToolTip(diCheckBox, Localization["Event_DigitalInput"]);
            SharedToolTips.SharedToolTip.SetToolTip(doCheckBox, Localization["Event_DigitalOutput"]);
            SharedToolTips.SharedToolTip.SetToolTip(networkLossCheckBox, Localization["Event_NetworkLoss"]);
            SharedToolTips.SharedToolTip.SetToolTip(networkRecoveryCheckBox, Localization["Event_NetworkRecovery"]);
            SharedToolTips.SharedToolTip.SetToolTip(videoLossCheckBox, Localization["Event_VideoLoss"]);
            SharedToolTips.SharedToolTip.SetToolTip(videoRecoveryCheckBox, Localization["Event_VideoRecovery"]);
            SharedToolTips.SharedToolTip.SetToolTip(panicCheckBox, Localization["Event_Panic"]);
            SharedToolTips.SharedToolTip.SetToolTip(manualRecordCheckBox, Localization["Event_ManualRecord"]);
            SharedToolTips.SharedToolTip.SetToolTip(crossLineCheckBox, Localization["Event_CrossLine"]);
            SharedToolTips.SharedToolTip.SetToolTip(intrusionDetectionCheckBox, Localization["Event_IntrusionDetection"]);
            SharedToolTips.SharedToolTip.SetToolTip(loiteringDetectionCheckBox, Localization["Event_LoiteringDetection"]);
            SharedToolTips.SharedToolTip.SetToolTip(objectCountingInCheckBox, Localization["Event_ObjectCountingIn"]);
            SharedToolTips.SharedToolTip.SetToolTip(objectCountingOutCheckBox, Localization["Event_ObjectCountingOut"]);
            SharedToolTips.SharedToolTip.SetToolTip(audioDetectionCheckBox, Localization["Event_AudioDetection"]);
            SharedToolTips.SharedToolTip.SetToolTip(temperingDetectionCheckBox, Localization["Event_TamperDetection"]);
            SharedToolTips.SharedToolTip.SetToolTip(userDefineCheckBox, Localization["Event_UserDefined"]);

            ivsMotionCheckBox.Image = Resources.GetResources(Properties.Resources.ivs, Properties.Resources.IMGIvs);
            manualRecordCheckBox.Image = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
            motionCheckBox.Image = Resources.GetResources(Properties.Resources.motion, Properties.Resources.IMGMotion);
            diCheckBox.Image = Resources.GetResources(Properties.Resources.di, Properties.Resources.IMGDi);
            doCheckBox.Image = Resources.GetResources(Properties.Resources._do, Properties.Resources.IMGDo);
            networkLossCheckBox.Image = Resources.GetResources(Properties.Resources.networkloss, Properties.Resources.IMGNetworkloss);
            networkRecoveryCheckBox.Image = Resources.GetResources(Properties.Resources.networkrecovery, Properties.Resources.IMGNetworkrecovery);
            videoLossCheckBox.Image = Resources.GetResources(Properties.Resources.videoloss, Properties.Resources.IMGVideoloss);
            videoRecoveryCheckBox.Image = Resources.GetResources(Properties.Resources.videorecovery, Properties.Resources.IMGVideorecovery);
            panicCheckBox.Image = Resources.GetResources(Properties.Resources.panic, Properties.Resources.IMGPanic);
            crossLineCheckBox.Image = Resources.GetResources(Properties.Resources.crossline, Properties.Resources.IMGCrossLine);
            intrusionDetectionCheckBox.Image = Resources.GetResources(Properties.Resources.intrusionDetection, Properties.Resources.IMGIntrusionDetection);
            loiteringDetectionCheckBox.Image = Resources.GetResources(Properties.Resources.loiteringDetection, Properties.Resources.IMGLoiteringDetection);
            objectCountingInCheckBox.Image = Resources.GetResources(Properties.Resources.objectCountingIn, Properties.Resources.IMGObjectCountingIn);
            objectCountingOutCheckBox.Image = Resources.GetResources(Properties.Resources.objectCountingOut, Properties.Resources.IMGObjectCountingOut);
            audioDetectionCheckBox.Image = Resources.GetResources(Properties.Resources.audioDetection, Properties.Resources.IMGAudioDetection);
            temperingDetectionCheckBox.Image = Resources.GetResources(Properties.Resources.temperingDetection, Properties.Resources.IMGTemperingDetection);
            userDefineCheckBox.Image = Resources.GetResources(Properties.Resources.userdefine, Properties.Resources.IMGUserDefine);

            setupIVSMotionPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.ive_setup, Properties.Resources.IMGIve_setup);
            minusPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.minus_scale, Properties.Resources.IMGMinus_scale);
            plusPictureBox.BackgroundImage = Resources.GetResources(Properties.Resources.plus_scale, Properties.Resources.IMGPlus_scale);
            periodPanel.BackgroundImage = Resources.GetResources(Properties.Resources.scaleBar, Properties.Resources.IMGScaleBar);

            searchButton.MouseClick += SearchButtonMouseClick;
            stopButton.MouseClick += StopButtonMouseClick;
            setupIVSMotionPictureBox.MouseClick += SetupIvsMotionPictureBoxMouseClick;
            ivsMotionCheckBox.CheckedChanged += IvsMotionCheckBoxCheckedChanged;
            periodPointPictureBox.MouseDown += PeriodPointPictureBoxMouseDown;
            minusPictureBox.MouseClick += MinusPictureBoxMouseClick;
            plusPictureBox.MouseClick += PlusPictureBoxMouseClick;

            periodPointPictureBox.LocationChanged += PeriodPointPictureBoxLocationChanged;

            this.startDatePicker.CustomFormat = DateTimeConverter.GetDatePattern();
            this.startTimePicker.CustomFormat = DateTimeConverter.GetTimePattern();
            this.endDatePicker.CustomFormat = DateTimeConverter.GetDatePattern();
            this.endTimePicker.CustomFormat = DateTimeConverter.GetTimePattern();
        }

        public Boolean IsSearch;
        public UInt32 CurrentlyPeriod = 1;

        public void Initialize()
        {
            SetPeriodPosition();
            SharedToolTips.SharedToolTip.SetToolTip(setupIVSMotionPictureBox, Localization["SmartSearch_SetupAnalysisRange"]);

            SetMaxAndMinDate();

            startDatePicker.ValueChanged += StartDatePickerValueChanged;
            startTimePicker.ValueChanged += StartDatePickerValueChanged;
            endDatePicker.ValueChanged += EndDatePickerValueChanged;
            endTimePicker.ValueChanged += EndDatePickerValueChanged;

            if (Server is ICMS)
            {
                nvrComboBox.SelectedIndexChanged += NVRComboBoxSelectedIndexChanged;
                // Disable IVS
                //panel4.Visible = ivsLabel.Visible = ivsMotionCheckBox.Visible = false;
                //MoveEventConditions();
            }
            else if (!Server.Server.CheckProductNoToSupport("snapshot"))
            {
                nvrLabel.Visible = nvrComboBox.Visible = false;
                panel4.Visible = ivsLabel.Visible = ivsMotionCheckBox.Visible = false;
                MoveConditions();
                MoveEventConditions();
            }
            else
            {
                nvrLabel.Visible = nvrComboBox.Visible = false;
                MoveConditions();
            }
            userDefineCheckBox.Visible = Server.Configure.EnableUserDefine;
        }

        private void MoveEventConditions()
        {
            eventLabel.Location = new Point(eventLabel.Location.X, eventLabel.Location.Y - 56);
            foreach (var control in conditionPanel.Controls)
            {
                var checkbox = control as CheckBox;
                if (checkbox != null)
                {
                    if (checkbox.Tag != null)
                    {
                        if (checkbox.Tag.ToString() == "Period") continue;
                    }

                    checkbox.Location = new Point(checkbox.Location.X, checkbox.Location.Y - 56);
                    continue;
                }
            }
        }

        private void MoveConditions()
        {
            foreach (var control in conditionPanel.Controls)
            {
                var label = control as Label;
                if (label != null)
                {
                    label.Location = new Point(label.Location.X, label.Location.Y - 56);
                    continue;
                }

                var combobox = control as ComboBox;
                if (combobox != null)
                {
                    combobox.Location = new Point(combobox.Location.X, combobox.Location.Y - 56);
                    continue;
                }

                var checkbox = control as CheckBox;
                if (checkbox != null)
                {
                    checkbox.Location = new Point(checkbox.Location.X, checkbox.Location.Y - 56);
                    continue;
                }

                var doubleBufferPanel = control as DoubleBufferPanel;
                if (doubleBufferPanel != null)
                {
                    doubleBufferPanel.Location = new Point(doubleBufferPanel.Location.X, doubleBufferPanel.Location.Y - 56);
                    continue;
                }

                var panel = control as Panel;
                if (panel != null)
                {
                    panel.Location = new Point(panel.Location.X, panel.Location.Y - 56);
                    continue;
                }

                var dateTimePicker = control as DateTimePicker;
                if (dateTimePicker != null)
                {
                    //dateTimePicker
                    dateTimePicker.Location = new Point(dateTimePicker.Location.X, dateTimePicker.Location.Y - 56);
                    continue;
                }

                var pictureBox = control as PictureBox;
                if (pictureBox != null)
                {
                    pictureBox.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y - 56);
                    continue;
                }
            }
        }

        public void UpdateDevice()
        {
            deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;

            deviceComboBox.Items.Clear();

            if (Server is ICMS)
            {
                nvrComboBox.SelectedIndexChanged -= NVRComboBoxSelectedIndexChanged;
                nvrComboBox.Items.Clear();
                var server = Server as ICMS;
                var list = new List<INVR>();

                foreach (KeyValuePair<UInt16, INVR> nvr in server.NVRManager.NVRs)
                {
                    if (nvr.Value.ReadyState != ReadyState.Ready) continue;
                    list.Add(nvr.Value);
                }

                list.Sort((x, y) => (x.Id - y.Id));
                foreach (INVR nvr in list)
                {
                    nvrComboBox.Items.Add(nvr);
                }

                nvrComboBox.SelectedIndexChanged += NVRComboBoxSelectedIndexChanged;
            }
            else
            {
                var list = new List<IDevice>(Server.Device.Devices.Values);
                list.Sort((x, y) => (x.Id - y.Id));
                foreach (var device in list)
                {
                    deviceComboBox.Items.Add(device);
                }
            }

            deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
        }

        private void SetMaxAndMinDate()
        {
            startDatePicker.MaxDate = new DateTime(2038, 12, 31, 23, 59, 59);
            startDatePicker.MinDate = new DateTime(1970, 1, 1, 0, 0, 0);

            endDatePicker.MaxDate = new DateTime(2038, 12, 31, 23, 59, 59);
            endDatePicker.MinDate = new DateTime(1970, 1, 1, 0, 0, 0);
        }

        public void ExportStartDateTimeChange(Object sender, EventArgs<String> e)
        {
            String value = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "StartTime");

            if (value != "")
            {
                _fireStartDateEvent = false;

                startDatePicker.ValueChanged -= StartDatePickerValueChanged;
                startTimePicker.ValueChanged -= StartDatePickerValueChanged;

                var startDate = DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                //SetMaxAndMinDate(startDate, endDatePicker.Value);

                startTimePicker.Value = startDatePicker.Value = startDate;

                StartDatePickerValueChanged();

                startDatePicker.ValueChanged += StartDatePickerValueChanged;
                startTimePicker.ValueChanged += StartDatePickerValueChanged;

                _fireStartDateEvent = true;

                CheckMinimumPeriod();
            }
        }

        public void ExportEndDateTimeChange(Object sender, EventArgs<String> e)
        {
            String value = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "EndTime");

            if (value != "")
            {
                _fireEndDateEvent = false;

                endDatePicker.ValueChanged -= EndDatePickerValueChanged;
                endTimePicker.ValueChanged -= EndDatePickerValueChanged;

                var endDate = DateTimes.ToDateTime(Convert.ToUInt64(value), Server.Server.TimeZone);
                //SetMaxAndMinDate(startDatePicker.Value, endDate);

                endTimePicker.Value = endDatePicker.Value = endDate;

                EndDatePickerValueChanged();

                endDatePicker.ValueChanged += EndDatePickerValueChanged;
                endTimePicker.ValueChanged += EndDatePickerValueChanged;

                _fireEndDateEvent = true;

                CheckMinimumPeriod();
            }
        }

        public TimeUnit Timeunit = TimeUnit.Unit10Senonds;
        public void TimeUnitChange(Object sender, EventArgs<TimeUnit, UInt64[]> e)
        {
            Timeunit = e.Value1;

            //check if selection is available
            if (e.Value2[2] == 0 && e.Value2[0] != 0)
            {
                _fireStartDateEvent = false;
                var startDate = DateTimes.ToDateTime(Convert.ToUInt64(e.Value2[0]), Server.Server.TimeZone);
                //SetMaxAndMinDate(startDate, endDatePicker.Value);
                startTimePicker.Value = startDatePicker.Value = startDate;
                _fireStartDateEvent = true;
            }
            if (e.Value2[3] == 0 && e.Value2[1] != 0)
            {
                _fireEndDateEvent = false;
                var endDate = DateTimes.ToDateTime(Convert.ToUInt64(e.Value2[1]), Server.Server.TimeZone);
                //SetMaxAndMinDate(startDatePicker.Value, endDate);
                endTimePicker.Value = endDatePicker.Value = endDate;
                _fireEndDateEvent = true;
            }
            CheckMinimumPeriod();
        }

        public Boolean IsIVSMotionChecked
        {
            get
            {
                return ivsMotionCheckBox.Checked;
            }
        }

        private static String ExportStartChangeXml(String startTime)
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartTime", startTime));

            return xmlDoc.InnerXml;
        }

        private static String ExportEndChangeXml(String endTime)
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("XML");
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndTime", endTime));

            return xmlDoc.InnerXml;
        }

        private Boolean _fireStartDateEvent;
        private Boolean _fireEndDateEvent;

        private void StartDatePickerValueChanged(Object sender, EventArgs e)
        {
            StartDatePickerValueChanged();
        }

        private void StartDatePickerValueChanged()
        {
            var startDate = new DateTime(
                startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day,
                startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second);

            var endDate = new DateTime(
                endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day,
                endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second);

            //if (_isEndChange)
            //    endDatePicker.MinDate = endTimePicker.MinDate = startDate;

            if (_fireStartDateEvent && _fireEndDateEvent)
                CheckMinimumPeriod();

            if (startDate > endDate)
            {
                if (_fireStartDateEvent && OnSearchEndDateTimeChange != null)
                {
                    //_isEndChange = true;
                    OnSearchEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(DateTimes.ToUtcString(startDate, Server.Server.TimeZone))));
                }

                if (_fireStartDateEvent && OnSearchStartDateTimeChange != null)
                {
                    //_isStartChange = true;
                    OnSearchStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(DateTimes.ToUtcString(endDate, Server.Server.TimeZone))));
                }
                _isRevert = true;
            }
            else
            {
                if (_fireStartDateEvent && OnSearchStartDateTimeChange != null)
                {
                    //_isStartChange = true;
                    OnSearchStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(DateTimes.ToUtcString(startDate, Server.Server.TimeZone))));
                }

                if (_isRevert && _fireStartDateEvent && OnSearchEndDateTimeChange != null)
                {
                    //_isEndChange = true;
                    OnSearchEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(DateTimes.ToUtcString(endDate, Server.Server.TimeZone))));
                    _isRevert = false;
                }
            }
        }

        private void EndDatePickerValueChanged(Object sender, EventArgs e)
        {
            EndDatePickerValueChanged();
        }

        private Boolean _isRevert;
        private void EndDatePickerValueChanged()
        {
            var startDate = new DateTime(
                startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day,
                startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second);

            var endDate = new DateTime(
                endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day,
                endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second);

            //if (_isStartChange)
            //    startDatePicker.MaxDate = startTimePicker.MaxDate = endDate;);

            if (_fireStartDateEvent && _fireEndDateEvent)
                CheckMinimumPeriod();

            if (endDate < startDate)
            {
                if (_fireEndDateEvent && OnSearchStartDateTimeChange != null)
                {
                    //_isEndChange = true;
                    OnSearchStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(DateTimes.ToUtcString(endDate, Server.Server.TimeZone))));
                }

                if (_fireEndDateEvent && OnSearchEndDateTimeChange != null)
                {
                    //_isEndChange = true;
                    OnSearchEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(DateTimes.ToUtcString(startDate, Server.Server.TimeZone))));
                }

                _isRevert = true;
            }
            else
            {
                if (_fireEndDateEvent && OnSearchEndDateTimeChange != null)
                {
                    //_isEndChange = true;
                    OnSearchEndDateTimeChange(this, new EventArgs<String>(ExportEndChangeXml(DateTimes.ToUtcString(endDate, Server.Server.TimeZone))));
                }

                if (_isRevert && _fireEndDateEvent && OnSearchStartDateTimeChange != null)
                {
                    //_isEndChange = true;
                    OnSearchStartDateTimeChange(this, new EventArgs<String>(ExportStartChangeXml(DateTimes.ToUtcString(startDate, Server.Server.TimeZone))));
                    _isRevert = false;
                }
            }
        }

        private UInt32 _minimumPeriod = 1;
        private void CheckMinimumPeriod()
        {
            //always 1 sec
            _minimumPeriod = 1;
            //Int64 diff = Math.Abs((end.Ticks - start.Ticks) / 10000000);

            //if(diff > 1728000) //20 Day
            //    _minimumPeriod = 86400;
            //else if(diff > 864000) // 10 Day
            //    _minimumPeriod = 3600;
            //else if(diff > 259200) // 3 Day
            //    _minimumPeriod = 1800;
            //else if(diff > 86400) // 1 Day
            //    _minimumPeriod = 600;
            //else if(diff > 28800) // 8 Hr
            //    _minimumPeriod = 300;
            //else if(diff > 10800) // 3 Hr
            //    _minimumPeriod = 60;
            //else if(diff > 3600) // 1 Hr
            //    _minimumPeriod = 30;
            //else if(diff > 600) // 10 Min
            //    _minimumPeriod = 10;
            //else
            //    _minimumPeriod = 1;

            periodPanel.Invalidate();
            if (CurrentlyPeriod < _minimumPeriod)
            {
                CurrentlyPeriod = _minimumPeriod;
                SetPeriodPosition();
            }
        }

        public UInt64 KeyFrame;
        public UInt64 Start;
        public UInt64 End;

        public void GoTo(Object sender, EventArgs<String> e)
        {
            var xmlDoc = Xml.LoadXml(e.Value);
            var value = Xml.GetFirstElementValueByTagName(xmlDoc, "Timestamp");
            var startTime = Xml.GetFirstElementValueByTagName(xmlDoc, "StartTime");
            var endTime = Xml.GetFirstElementValueByTagName(xmlDoc, "EndTime");
            var rangeStartTime = Xml.GetFirstElementValueByTagName(xmlDoc, "RangeStartTime");
            var rangeEndTime = Xml.GetFirstElementValueByTagName(xmlDoc, "RangeEndTime");

            if (value != "")
            {
                KeyFrame = Convert.ToUInt64(value);

                _isRevert = _fireStartDateEvent = _fireEndDateEvent = false;

                startDatePicker.ValueChanged -= StartDatePickerValueChanged;
                startTimePicker.ValueChanged -= StartDatePickerValueChanged;

                var startDate = (rangeStartTime != "")
                    ? DateTimes.ToDateTime(Convert.ToUInt64(rangeStartTime), Server.Server.TimeZone)
                    : DateTimes.ToDateTime(Convert.ToUInt64(startTime), Server.Server.TimeZone);

                //SetMaxAndMinDate(startDate, endDatePicker.Value);
                startTimePicker.Value = startDatePicker.Value = startDate;

                StartDatePickerValueChanged();

                startDatePicker.ValueChanged += StartDatePickerValueChanged;
                startTimePicker.ValueChanged += StartDatePickerValueChanged;

                endDatePicker.ValueChanged -= EndDatePickerValueChanged;
                endTimePicker.ValueChanged -= EndDatePickerValueChanged;

                var endDate = (rangeEndTime != "")
                    ? DateTimes.ToDateTime(Convert.ToUInt64(rangeEndTime), Server.Server.TimeZone)
                    : DateTimes.ToDateTime(Convert.ToUInt64(endTime), Server.Server.TimeZone);

                //SetMaxAndMinDate(startDatePicker.Value, endDate);
                endTimePicker.Value = endDatePicker.Value = endDate;

                EndDatePickerValueChanged();

                endDatePicker.ValueChanged += EndDatePickerValueChanged;
                endTimePicker.ValueChanged += EndDatePickerValueChanged;

                _fireStartDateEvent = _fireEndDateEvent = true;

                CheckMinimumPeriod();
            }
        }

        public Boolean StopButtonEnabled
        {
            set
            {
                stopButton.Enabled = value;
            }
        }

        public void Activate()
        {
            if (startDatePicker.Value == startDatePicker.MinDate)
            {
                startDatePicker.ValueChanged -= StartDatePickerValueChanged;
                startTimePicker.ValueChanged -= StartDatePickerValueChanged;

                startDatePicker.Value = startTimePicker.Value = Server.Server.DateTime;

                startDatePicker.ValueChanged += StartDatePickerValueChanged;
                startTimePicker.ValueChanged += StartDatePickerValueChanged;
            }

            if (endDatePicker.Value == endDatePicker.MaxDate)
            {
                endDatePicker.ValueChanged -= EndDatePickerValueChanged;
                endTimePicker.ValueChanged -= EndDatePickerValueChanged;

                endDatePicker.Value = endTimePicker.Value = Server.Server.DateTime;

                endDatePicker.ValueChanged += EndDatePickerValueChanged;
                endTimePicker.ValueChanged += EndDatePickerValueChanged;
            }
        }

        public void Deactivate()
        {
            if (!IsSearch) return;

            IsSearch = false;
            stopButton.Enabled = false;

            if (SearchingCamera != null)
                SearchingCamera.StopSearch();
        }

        private void NVRComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var server = Server as ICMS;
            if (server == null) return;
            if (nvrComboBox.SelectedItem as INVR == null) return;
            var nvr = nvrComboBox.SelectedItem as INVR;
            if (!server.NVRManager.NVRs.ContainsKey(nvr.Id)) return;

            UpdateDeviceComboBoxContentByNVR(nvr);
        }

        private void UpdateDeviceComboBoxContentByNVR(INVR nvr)
        {
            deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;
            deviceComboBox.Items.Clear();

            var list = new List<IDevice>(nvr.Device.Devices.Values);
            list.Sort((x, y) => (x.Id - y.Id));
            foreach (var device in list)
            {
                deviceComboBox.Items.Add(device);
            }

            deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
        }

        private void DeviceComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            var device = deviceComboBox.SelectedItem as IDevice;

            if (device == null) return;

            SmartSearchControl.AppendDevice(device);
        }

        #region PeriodPosition

        private const Int16 PeriodPosition1 = 18;
        private const Int16 PeriodPosition2 = 31;
        private const Int16 PeriodPosition3 = 44;
        private const Int16 PeriodPosition4 = 57;
        private const Int16 PeriodPosition5 = 70;
        private const Int16 PeriodPosition6 = 83;
        private const Int16 PeriodPosition7 = 96;
        private const Int16 PeriodPosition8 = 109;
        private const Int16 PeriodPosition9 = 122;
        private const Int16 PeriodPosition10 = 135;
        private Int32 _lastMovePosition;
        private const UInt16 PointWidth = 11;
        private const UInt16 PeriodDiff = 7;
        private void PeriodPanelMouseMove(Object sender, MouseEventArgs e)
        {
            if (_lastMovePosition == e.X) return;

            Int32 newX = Math.Min(Math.Max(PeriodPosition1, e.X - PointWidth), PeriodPosition10);

            if (newX <= PeriodPosition1 + PeriodDiff)
                periodPointPictureBox.Location = new Point(PeriodPosition1, 0);
            else if (newX > PeriodPosition2 - PeriodDiff && newX <= PeriodPosition2 + PeriodDiff && _minimumPeriod <= _periodList[1])
                periodPointPictureBox.Location = new Point(PeriodPosition2, 0);
            else if (newX > PeriodPosition3 - PeriodDiff && newX <= PeriodPosition3 + PeriodDiff && _minimumPeriod <= _periodList[2])
                periodPointPictureBox.Location = new Point(PeriodPosition3, 0);
            else if (newX > PeriodPosition4 - PeriodDiff && newX <= PeriodPosition4 + PeriodDiff && _minimumPeriod <= _periodList[3])
                periodPointPictureBox.Location = new Point(PeriodPosition4, 0);
            else if (newX > PeriodPosition5 - PeriodDiff && newX <= PeriodPosition5 + PeriodDiff && _minimumPeriod <= _periodList[4])
                periodPointPictureBox.Location = new Point(PeriodPosition5, 0);
            else if (newX > PeriodPosition6 - PeriodDiff && newX <= PeriodPosition6 + PeriodDiff && _minimumPeriod <= _periodList[5])
                periodPointPictureBox.Location = new Point(PeriodPosition6, 0);
            else if (newX > PeriodPosition7 - PeriodDiff && newX <= PeriodPosition7 + PeriodDiff && _minimumPeriod <= _periodList[6])
                periodPointPictureBox.Location = new Point(PeriodPosition7, 0);
            else if (newX > PeriodPosition8 - PeriodDiff && newX <= PeriodPosition8 + PeriodDiff && _minimumPeriod <= _periodList[7])
                periodPointPictureBox.Location = new Point(PeriodPosition8, 0);
            else if (newX > PeriodPosition9 - PeriodDiff && newX <= PeriodPosition9 + PeriodDiff && _minimumPeriod <= _periodList[8])
                periodPointPictureBox.Location = new Point(PeriodPosition9, 0);
            else if (newX > PeriodPosition10 - PeriodDiff)
                periodPointPictureBox.Location = new Point(PeriodPosition10, 0);

            _lastMovePosition = e.X;
        }

        private static readonly Image _scalePoint = Resources.GetResources(Properties.Resources.scalePoint, Properties.Resources.IMGScalePoint);
        private static readonly Image _scalePointselect = Resources.GetResources(Properties.Resources.scalePoint_select, Properties.Resources.IMGScalePoint_select);
        private void PeriodPanelMouseUp(Object sender, MouseEventArgs e)
        {
            periodPanel.MouseMove -= PeriodPanelMouseMove;
            periodPanel.MouseUp -= PeriodPanelMouseUp;
            periodPointPictureBox.BackgroundImage = _scalePoint;

            switch (periodPointPictureBox.Location.X)
            {
                case PeriodPosition1:
                    CurrentlyPeriod = _periodList[0];
                    break;

                case PeriodPosition2:
                    CurrentlyPeriod = _periodList[1];
                    break;

                case PeriodPosition3:
                    CurrentlyPeriod = _periodList[2];
                    break;

                case PeriodPosition4:
                    CurrentlyPeriod = _periodList[3];
                    break;

                case PeriodPosition5:
                    CurrentlyPeriod = _periodList[4];
                    break;

                case PeriodPosition6:
                    CurrentlyPeriod = _periodList[5];
                    break;

                case PeriodPosition7:
                    CurrentlyPeriod = _periodList[6];
                    break;

                case PeriodPosition8:
                    CurrentlyPeriod = _periodList[7];
                    break;

                case PeriodPosition9:
                    CurrentlyPeriod = _periodList[8];
                    break;

                case PeriodPosition10:
                    CurrentlyPeriod = _periodList[9];
                    break;
            }
        }

        private void PeriodPointPictureBoxMouseDown(Object sender, MouseEventArgs e)
        {
            periodPointPictureBox.BackgroundImage = _scalePointselect;

            periodPanel.MouseMove -= PeriodPanelMouseMove;
            periodPanel.MouseMove += PeriodPanelMouseMove;

            periodPanel.MouseUp -= PeriodPanelMouseUp;
            periodPanel.MouseUp += PeriodPanelMouseUp;

            periodPanel.Capture = true;
        }

        private void PlusPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (_periodList.IndexOf(CurrentlyPeriod) + 1 < _periodList.Count)
                CurrentlyPeriod = _periodList[_periodList.IndexOf(CurrentlyPeriod) + 1];

            SetPeriodPosition();
        }

        private void MinusPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            if (_periodList.IndexOf(CurrentlyPeriod) > 0)
                CurrentlyPeriod = _periodList[_periodList.IndexOf(CurrentlyPeriod) - 1];

            CurrentlyPeriod = Math.Max(CurrentlyPeriod, _minimumPeriod);

            SetPeriodPosition();
        }

        private void PeriodPointPictureBoxLocationChanged(Object sender, EventArgs e)
        {
            var period = "";
            switch (periodPointPictureBox.Location.X)
            {
                case PeriodPosition1:
                    period = 1 + @" " + Localization["Common_Sec"];
                    break;

                case PeriodPosition2:
                    period = 5 + @" " + Localization["Common_Sec"];
                    break;

                case PeriodPosition3:
                    period = 10 + @" " + Localization["Common_Sec"];
                    break;

                case PeriodPosition4:
                    period = 30 + @" " + Localization["Common_Sec"];
                    break;

                case PeriodPosition5:
                    period = 1 + @" " + Localization["Common_Min"];
                    break;

                case PeriodPosition6:
                    period = 5 + @" " + Localization["Common_Min"];
                    break;

                case PeriodPosition7:
                    period = 10 + @" " + Localization["Common_Min"];
                    break;

                case PeriodPosition8:
                    period = 30 + @" " + Localization["Common_Min"];
                    break;

                case PeriodPosition9:
                    period = 1 + @" " + Localization["Common_Hr"];
                    break;

                case PeriodPosition10:
                    period = 1 + @" " + Localization["Common_Day"];
                    break;
            }

            periodLabel.Text = Localization["SmartSearch_Period"] + @" : " + period;
        }

        private void SetPeriodPosition()
        {
            switch (CurrentlyPeriod)
            {
                case 1:
                    periodPointPictureBox.Location = new Point(PeriodPosition1, 0);
                    break;

                case 5:
                    periodPointPictureBox.Location = new Point(PeriodPosition2, 0);
                    break;

                case 10:
                    periodPointPictureBox.Location = new Point(PeriodPosition3, 0);
                    break;

                case 30:
                    periodPointPictureBox.Location = new Point(PeriodPosition4, 0);
                    break;

                case 60:
                    periodPointPictureBox.Location = new Point(PeriodPosition5, 0);
                    break;

                case 300:
                    periodPointPictureBox.Location = new Point(PeriodPosition6, 0);
                    break;

                case 600:
                    periodPointPictureBox.Location = new Point(PeriodPosition7, 0);
                    break;

                case 1800:
                    periodPointPictureBox.Location = new Point(PeriodPosition8, 0);
                    break;

                case 3600:
                    periodPointPictureBox.Location = new Point(PeriodPosition9, 0);
                    break;

                case 86400:
                    periodPointPictureBox.Location = new Point(PeriodPosition10, 0);
                    break;
            }
        }
        #endregion

        public readonly List<EventType> QueueToSearchEvent = new List<EventType>();
        public ICamera SearchingCamera;
        public readonly List<Rectangle> MotionAreas = new List<Rectangle>();
        public void SearchStart()
        {
            var start = new DateTime(startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day, startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second);
            Start = DateTimes.ToUtc(start, VideoWindow.Camera.Server.Server.TimeZone);

            var end = new DateTime(endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day, endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second);
            End = DateTimes.ToUtc(end, VideoWindow.Camera.Server.Server.TimeZone);

            var log = "Search Device %1 Start Time %2 End Time %3"
                .Replace("%1", VideoWindow.Camera.Id.ToString())
                .Replace("%2", start.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture))
                .Replace("%3", end.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));

            Server.WriteOperationLog(log);

            List<EventType> events = GetEventSelection();
            SearchingCamera = VideoWindow.Camera;

            SearchingCamera.OnSmartSearchResult -= SmartSearchResult;
            SearchingCamera.OnSmartSearchResult += SmartSearchResult;
            SearchingCamera.OnSmartSearchComplete -= SmartSearchComplete;
            SearchingCamera.OnSmartSearchComplete += SmartSearchComplete;

            if (periodTimeCheckBox.Checked)
            {
                if (ivsMotionCheckBox.Checked)
                {
                    String str = VideoWindow.Viewer.GetMotionRegion();

                    //remove previous region , dont keep it
                    MotionAreas.Clear();

                    if (str != null)
                    {
                        XmlDocument xmlDoc = Xml.LoadXml(str);

                        XmlNodeList motionAreas = xmlDoc.GetElementsByTagName("MotionArea");

                        //if(motionAreas.Count > 0)
                        //    _motionAreas.Clear();

                        foreach (XmlElement area in motionAreas)
                        {
                            Int32[] values = Array.ConvertAll(area.InnerText.Split(','), Convert.ToInt32);
                            MotionAreas.Add(new Rectangle(values[0], values[1], values[2], values[3]));
                        }
                    }

                    var keyFrame = KeyFrame;

                    if (Server is INVR)
                    {
                        if (Server.Server.TimeZone != VideoWindow.Camera.Server.Server.TimeZone)
                        {
                            Int64 time = Convert.ToInt64(keyFrame);
                            time -= (VideoWindow.Camera.Server.Server.TimeZone * 1000);
                            time += (Server.Server.TimeZone * 1000);
                            keyFrame = Convert.ToUInt64(time);
                        }
                    }

                    SmartSearchDelegate smartSearchDelegate = VideoWindow.Camera.SmartSearch;
                    smartSearchDelegate.BeginInvoke(CurrentlyPeriod, keyFrame, Start, End, MotionAreas.ToArray(), null, null);

                    QueueToSearchEvent.Clear();
                    if (events.Count > 0)
                    {
                        QueueToSearchEvent.AddRange(events);
                    }
                }
                else if (events.Count > 0)
                {
                    List<UInt64> period = null;

                    if (CurrentlyPeriod != 1)
                    {
                        period = new List<UInt64>();
                        UInt64 startTime = Start;
                        while (startTime <= End)
                        {
                            period.Add(startTime);
                            startTime += CurrentlyPeriod * 1000;
                        }
                    }

                    EventSearchDelegate eventSearchDelegate = VideoWindow.Camera.EventSearch;
                    eventSearchDelegate.BeginInvoke(Start, End, events, period, null, null);
                }
                else
                {
                    TimePeriodSearchDelegate timePeriodSearchDelegate = VideoWindow.Camera.TimePeriodSearch;
                    timePeriodSearchDelegate.BeginInvoke(CurrentlyPeriod, Start, End, null, null);
                }
            }
            else
            {
                EventSearchDelegate eventSearchDelegate = VideoWindow.Camera.EventSearch;
                eventSearchDelegate.BeginInvoke(Start, End, events, null, null, null);
            }

            //VideoWindow.Viewer.PtzMode = PtzMode.Digital;

            if (OnSearchStart != null)
                OnSearchStart(this, null);
        }

        private delegate void SmartSearchDelegate(UInt32 period, UInt64 keyFrame, UInt64 startTime, UInt64 endTime, Rectangle[] areas);
        private delegate void TimePeriodSearchDelegate(UInt32 period, UInt64 startTime, UInt64 endTime);
        private delegate void EventSearchDelegate(UInt64 startTime, UInt64 endTime, List<EventType> events, List<UInt64> period);

        private List<EventType> GetEventSelection()
        {
            var events = new List<EventType>();

            if (motionCheckBox.Checked)
                events.Add(EventType.Motion);

            if (manualRecordCheckBox.Checked)
                events.Add(EventType.ManualRecord);

            if (networkLossCheckBox.Checked)
                events.Add(EventType.NetworkLoss);

            if (networkRecoveryCheckBox.Checked)
                events.Add(EventType.NetworkRecovery);

            if (videoLossCheckBox.Checked)
                events.Add(EventType.VideoLoss);

            if (videoRecoveryCheckBox.Checked)
                events.Add(EventType.VideoRecovery);

            if (diCheckBox.Checked)
                events.Add(EventType.DigitalInput);

            if (doCheckBox.Checked)
                events.Add(EventType.DigitalOutput);

            if (panicCheckBox.Checked)
                events.Add(EventType.Panic);

            if (crossLineCheckBox.Checked)
                events.Add(EventType.CrossLine);

            if (intrusionDetectionCheckBox.Checked)
                events.Add(EventType.IntrusionDetection);

            if (loiteringDetectionCheckBox.Checked)
                events.Add(EventType.LoiteringDetection);

            if (objectCountingInCheckBox.Checked)
                events.Add(EventType.ObjectCountingIn);

            if (objectCountingOutCheckBox.Checked)
                events.Add(EventType.ObjectCountingOut);

            if (audioDetectionCheckBox.Checked)
                events.Add(EventType.AudioDetection);

            if (temperingDetectionCheckBox.Checked)
                events.Add(EventType.TamperDetection);

            if (userDefineCheckBox.Checked)
                events.Add(EventType.UserDefine);

            return events;
        }

        public void ApplyPlaybackParameter(CameraEvents cameraEvents, UInt64 start, UInt64 end)
        {
            startTimePicker.Value = startDatePicker.Value = DateTimes.ToDateTime(start, Server.Server.TimeZone);
            endTimePicker.Value = endDatePicker.Value = DateTimes.ToDateTime(end, Server.Server.TimeZone);

            if (Server is ICMS)
            {
                nvrComboBox.SelectedIndexChanged -= NVRComboBoxSelectedIndexChanged;
                nvrComboBox.SelectedItem = cameraEvents.Device.Server;
                nvrComboBox.SelectedIndexChanged += NVRComboBoxSelectedIndexChanged;

                UpdateDeviceComboBoxContentByNVR(cameraEvents.Device.Server as INVR);
            }
            deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;
            deviceComboBox.SelectedItem = cameraEvents.Device;
            deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;

            motionCheckBox.Checked = false;
            manualRecordCheckBox.Checked = false;
            networkLossCheckBox.Checked = false;
            networkRecoveryCheckBox.Checked = false;
            videoLossCheckBox.Checked = false;
            videoRecoveryCheckBox.Checked = false;
            diCheckBox.Checked = false;
            doCheckBox.Checked = false;
            panicCheckBox.Checked = false;
            crossLineCheckBox.Checked = false;
            intrusionDetectionCheckBox.Checked = false;
            loiteringDetectionCheckBox.Checked = false;
            objectCountingInCheckBox.Checked = false;
            objectCountingOutCheckBox.Checked = false;
            audioDetectionCheckBox.Checked = false;
            temperingDetectionCheckBox.Checked = false;
            userDefineCheckBox.Checked = false;

            switch (cameraEvents.Type)
            {
                case EventType.Motion:
                    motionCheckBox.Checked = true;
                    break;

                case EventType.ManualRecord:
                    manualRecordCheckBox.Checked = true;
                    break;

                case EventType.NetworkLoss:
                    networkLossCheckBox.Checked = true;
                    break;

                case EventType.NetworkRecovery:
                    networkRecoveryCheckBox.Checked = true;
                    break;

                case EventType.VideoLoss:
                    videoLossCheckBox.Checked = true;
                    break;

                case EventType.VideoRecovery:
                    videoRecoveryCheckBox.Checked = true;
                    break;

                case EventType.DigitalInput:
                    diCheckBox.Checked = true;
                    break;

                case EventType.DigitalOutput:
                    doCheckBox.Checked = true;
                    break;

                case EventType.Panic:
                    panicCheckBox.Checked = true;
                    break;

                case EventType.CrossLine:
                    crossLineCheckBox.Checked = true;
                    break;

                case EventType.IntrusionDetection:
                    intrusionDetectionCheckBox.Checked = true;
                    break;

                case EventType.LoiteringDetection:
                    loiteringDetectionCheckBox.Checked = true;
                    break;

                case EventType.ObjectCountingIn:
                    objectCountingInCheckBox.Checked = true;
                    break;

                case EventType.ObjectCountingOut:
                    objectCountingOutCheckBox.Checked = true;
                    break;

                case EventType.AudioDetection:
                    audioDetectionCheckBox.Checked = true;
                    break;

                case EventType.TamperDetection:
                    temperingDetectionCheckBox.Checked = true;
                    break;

                case EventType.UserDefine:
                    userDefineCheckBox.Checked = true;
                    break;
            }
        }

        private void IvsMotionCheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            setupIVSMotionPictureBox.Visible = (ivsMotionCheckBox.Checked);

            if (ivsMotionCheckBox.Checked)
            {
                VideoWindow.Viewer.EnableMotionDetection(true);
                VideoWindow.Viewer.SetupMotionStart();
                periodTimeCheckBox.Checked = true;
                periodTimeCheckBox.Enabled = false;
            }
            else
            {
                VideoWindow.Viewer.EnableMotionDetection(false);
                VideoWindow.Viewer.PtzMode = PTZMode.Digital;
                periodTimeCheckBox.Enabled = true;
            }
        }

        private void SetupIvsMotionPictureBoxMouseClick(Object sender, MouseEventArgs e)
        {
            VideoWindow.Viewer.EnableMotionDetection(true);
            VideoWindow.Viewer.SetupMotionStart();
        }

        private void SearchButtonMouseClick(Object sender, MouseEventArgs e)
        {
            _found = 0;

            if (VideoWindow.Camera == null || KeyFrame == 0) return;

            if (!ivsMotionCheckBox.Checked && !periodTimeCheckBox.Checked && GetEventSelection().Count == 0)
            {
                TopMostMessageBox.Show(Localization["SmartSearch_NoCriteria"], Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IsSearch = true;
            stopButton.Visible = true;
            searchButton.Visible = false;

            SearchStart();
        }

        private void StopButtonMouseClick(Object sender, MouseEventArgs e)
        {
            stopButton.Enabled = false;
            IsSearch = false;

            if (SearchingCamera != null)
                SearchingCamera.StopSearch();
        }

        private Int32 _found;

        private delegate void SmartSearchResultDelegate(Object sender, EventArgs<String> e);
        public void SmartSearchResult(Object sender, EventArgs<String> e)
        {
            if (!IsSearch) return;

            if (InvokeRequired)
            {
                Invoke(new SmartSearchResultDelegate(SmartSearchResult), sender, e);
                return;
            }

            _found += Xml.LoadXml(e.Value).GetElementsByTagName("Time").Count;

            if (OnSmartSearchResult != null)
                OnSmartSearchResult(this, e);
        }

        private delegate void SmartSearchCompleteDelegate(Object sender, EventArgs e);
        public void SmartSearchComplete(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new SmartSearchCompleteDelegate(SmartSearchComplete), sender, e);
                return;
            }
            if (SearchingCamera == null) return;

            //after search ivs, now search event
            if (IsSearch && (SearchingCamera.MaximumSearchResult == 0 || _found < SearchingCamera.MaximumSearchResult)
                && QueueToSearchEvent.Count > 0 && VideoWindow.Camera != null)
            {
                var events = new List<EventType>();
                events.AddRange(QueueToSearchEvent);

                List<UInt64> period = null;

                if (CurrentlyPeriod != 1)
                {
                    UInt64 startTime = Start;
                    period = new List<UInt64>();
                    while (startTime <= End)
                    {
                        period.Add(startTime);
                        startTime += CurrentlyPeriod * 1000;
                    }
                }

                EventSearchDelegate eventSearchDelegate = VideoWindow.Camera.EventSearch;
                eventSearchDelegate.BeginInvoke(Start, End, events, period, null, null);

                QueueToSearchEvent.Clear();
                return;
            }

            QueueToSearchEvent.Clear();

            stopButton.Enabled = true;
            stopButton.Visible = false;
            searchButton.Visible = true;

            SearchingCamera.OnSmartSearchResult -= SmartSearchResult;
            SearchingCamera.OnSmartSearchComplete -= SmartSearchComplete;
            SearchingCamera = null;

            if (OnSmartSearchComplete != null)
                OnSmartSearchComplete(this, e);

            IsSearch = false;
        }

        public void Disconnect(Object sender, EventArgs e)
        {
            Disconnect();
        }

        public void Disconnect()
        {
            if (Server is ICMS)
            {
                nvrComboBox.SelectedIndexChanged -= NVRComboBoxSelectedIndexChanged;
                nvrComboBox.SelectedIndex = -1;
                nvrComboBox.SelectedIndexChanged += NVRComboBoxSelectedIndexChanged;

                deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;
                deviceComboBox.Items.Clear();
                deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            }
            else
            {
                deviceComboBox.SelectedIndexChanged -= DeviceComboBoxSelectedIndexChanged;
                deviceComboBox.SelectedIndex = -1;
                deviceComboBox.SelectedIndexChanged += DeviceComboBoxSelectedIndexChanged;
            }

            if (SearchingCamera != null)
                SearchingCamera.StopSearch();

            stopButton.Enabled = false;
            IsSearch = false;
            MotionAreas.Clear();
        }
    }
}
