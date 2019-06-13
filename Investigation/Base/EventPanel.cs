using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace Investigation.Base
{
    public sealed partial class EventPanel : UserControl
    {
        public event EventHandler OnSelectChange;

        public INVR NVR;
        public CameraEventSearchCriteria SearchCriteria;
        public Boolean ShowEventColor;

        private readonly List<EventType> _allEventList;

        private static readonly Image _motion = Resources.GetResources(Properties.Resources.motion, Properties.Resources.IMGMotion);
        private static readonly Image _di = Resources.GetResources(Properties.Resources.di, Properties.Resources.IMGDI);
        private static readonly Image _do = Resources.GetResources(Properties.Resources._do, Properties.Resources.IMGDO);
        private static readonly Image _networkLoss = Resources.GetResources(Properties.Resources.networkloss, Properties.Resources.IMGNetworkLoss);
        private static readonly Image _networkRecovery = Resources.GetResources(Properties.Resources.networkrecovery, Properties.Resources.IMGNetworkRecovery);
        private static readonly Image _videoLoss = Resources.GetResources(Properties.Resources.videoloss, Properties.Resources.IMGVideoLoss);
        private static readonly Image _videoRecovery = Resources.GetResources(Properties.Resources.videorecovery, Properties.Resources.IMGVideoRecovery);
        private static readonly Image _manualRecord = Resources.GetResources(Properties.Resources.manualrecord, Properties.Resources.IMGManualRecord);
        private static readonly Image _panic = Resources.GetResources(Properties.Resources.panic, Properties.Resources.IMGPanic);
        private static readonly Image _crossLine = Resources.GetResources(Properties.Resources.crossline, Properties.Resources.IMGCrossLine);
        private static readonly Image _intrusionDetection = Resources.GetResources(Properties.Resources.intrusionDetection, Properties.Resources.IMGIntrusionDetection);
        private static readonly Image _loiteringDetection = Resources.GetResources(Properties.Resources.loiteringDetection, Properties.Resources.IMGLoiteringDetection);
        private static readonly Image _objectCountingIn = Resources.GetResources(Properties.Resources.objectCountingIn, Properties.Resources.IMGObjectCountingIn);
        private static readonly Image _objectCountingOut = Resources.GetResources(Properties.Resources.objectCountingOut, Properties.Resources.IMGObjectCountingOut);
        private static readonly Image _audioDetection = Resources.GetResources(Properties.Resources.audioDetection, Properties.Resources.IMGAudioDetection);
        private static readonly Image _temperingDetection = Resources.GetResources(Properties.Resources.temperingDetection, Properties.Resources.IMGTemperingDetection);
        private static readonly Image _userdefined = Resources.GetResources(Properties.Resources.userdefine, Properties.Resources.IMGUserdefined);

        public EventPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.None;
            Name = "Event";

            BackgroundImage = Manager.BackgroundNoBorder;

            _allEventList = new List<EventType>
								{
									EventType.DigitalInput,
									EventType.ManualRecord,
									EventType.Motion,
									EventType.Panic,
									EventType.VideoRecovery,
									EventType.VideoLoss,
									EventType.DigitalOutput,
									EventType.NetworkRecovery,
									EventType.NetworkLoss,
									EventType.CrossLine,
                                    EventType.IntrusionDetection,
                                    EventType.LoiteringDetection,
                                    EventType.ObjectCountingIn,
                                    EventType.ObjectCountingOut,
                                    EventType.AudioDetection,
                                    EventType.TamperDetection,
                                    EventType.UserDefine
								};
            _allEventList.Reverse();
        }

        private Boolean _isEditing;
        public void ParseSetting()
        {
            _isEditing = false;

            ClearViewModel();

            var selectAll = true;
            var count = 0;

            foreach (EventType eventType in _allEventList)
            {
                var stringPanel = GetStringPanel();

                switch (eventType)
                {
                    case EventType.Motion:
                        stringPanel.EventImage = _motion;
                        break;

                    case EventType.DigitalInput:
                        stringPanel.EventImage = _di;
                        break;

                    case EventType.DigitalOutput:
                        stringPanel.EventImage = _do;
                        break;

                    case EventType.NetworkLoss:
                        stringPanel.EventImage = _networkLoss;
                        break;

                    case EventType.NetworkRecovery:
                        stringPanel.EventImage = _networkRecovery;
                        break;

                    case EventType.VideoLoss:
                        stringPanel.EventImage = _videoLoss;
                        break;

                    case EventType.VideoRecovery:
                        stringPanel.EventImage = _videoRecovery;
                        break;

                    case EventType.ManualRecord:
                        stringPanel.EventImage = _manualRecord;
                        break;

                    case EventType.Panic:
                        stringPanel.EventImage = _panic;
                        break;

                    case EventType.CrossLine:
                        stringPanel.EventImage = _crossLine;
                        break;

                    case EventType.IntrusionDetection:
                        stringPanel.EventImage = _intrusionDetection;
                        break;

                    case EventType.LoiteringDetection:
                        stringPanel.EventImage = _loiteringDetection;
                        break;

                    case EventType.ObjectCountingIn:
                        stringPanel.EventImage = _objectCountingIn;
                        break;

                    case EventType.ObjectCountingOut:
                        stringPanel.EventImage = _objectCountingOut;
                        break;

                    case EventType.AudioDetection:
                        stringPanel.EventImage = _audioDetection;
                        break;

                    case EventType.TamperDetection:
                        stringPanel.EventImage = _temperingDetection;
                        break;
                    case EventType.UserDefine:
                        stringPanel.EventImage = _userdefined;
                        break;
                }
                stringPanel.EventType = eventType;
                stringPanel.Event = CameraEventSearchCriteria.EventTypeToLocalizationString(eventType);

                if (ShowEventColor)
                {
                    stringPanel.ShowEventColor = true;
                    stringPanel.EventColor = CameraEventSearchCriteria.GetEventTypeDefaultColor(eventType);
                    stringPanel.RefreshColor();
                }
                if (SearchCriteria.Event.Contains(eventType))
                {
                    count++;
                    stringPanel.Checked = true;
                }
                else
                    selectAll = false;

                containerPanel.Controls.Add(stringPanel);
            }
            if (count == 0 && selectAll)
                selectAll = false;

            var stringTitlePanel = GetStringPanel();
            stringTitlePanel.IsTitle = true;
            stringTitlePanel.ShowEventColor = ShowEventColor;
            stringTitlePanel.Cursor = Cursors.Default;
            stringTitlePanel.Checked = selectAll;
            stringTitlePanel.OnSelectAll += StringPanelOnSelectAll;
            stringTitlePanel.OnSelectNone += StringPanelOnSelectNone;
            containerPanel.Controls.Add(stringTitlePanel);

            containerPanel.Select();
            containerPanel.AutoScrollPosition = new Point(0, 0);

            _isEditing = true;
        }

        private readonly Queue<StringPanel> _recycleString = new Queue<StringPanel>();
        private StringPanel GetStringPanel()
        {
            if (_recycleString.Count > 0)
            {
                return _recycleString.Dequeue();
            }

            var stringPanel = new StringPanel
            {
                SelectionVisible = true,
            };

            stringPanel.OnSelectChange += StringPanelOnSelectChange;

            return stringPanel;
        }

        private void StringPanelOnSelectChange(Object sender, EventArgs e)
        {
            if (!_isEditing) return;

            var panel = sender as StringPanel;
            if (panel == null) return;
            if (panel.IsTitle) return;

            var selectAll = false;
            if (panel.Checked)
            {
                if (!SearchCriteria.Event.Contains(panel.EventType))
                {
                    SearchCriteria.Event.Add(panel.EventType);
                    SearchCriteria.Event.Sort();
                }

                selectAll = true;
                foreach (StringPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }
            else
            {
                SearchCriteria.Event.Remove(panel.EventType);
            }

            if (OnSelectChange != null)
                OnSelectChange(null, null);

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as StringPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= StringPanelOnSelectAll;
                title.OnSelectNone -= StringPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += StringPanelOnSelectAll;
                title.OnSelectNone += StringPanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (StringPanel stringPanel in containerPanel.Controls)
            {
                stringPanel.SelectionVisible = false;
                stringPanel.Visible = true;

                stringPanel.Checked = false;
                stringPanel.Event = "";
                stringPanel.Cursor = Cursors.Hand;
                stringPanel.SelectionVisible = true;

                if (stringPanel.IsTitle)
                {
                    stringPanel.OnSelectAll -= StringPanelOnSelectAll;
                    stringPanel.OnSelectNone -= StringPanelOnSelectNone;
                    stringPanel.IsTitle = false;
                }

                if (!_recycleString.Contains(stringPanel))
                {
                    _recycleString.Enqueue(stringPanel);
                }
            }
            containerPanel.Controls.Clear();
        }

        private void StringPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (StringPanel control in containerPanel.Controls)
            {
                if (control.IsTitle) continue;

                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void StringPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (StringPanel control in containerPanel.Controls)
            {
                if (control.IsTitle) continue;

                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        public void ScrollTop()
        {
            containerPanel.Select();
            containerPanel.AutoScrollPosition = new Point(0, 0);
        }

        public Panel ContainerPanel
        {
            get { return containerPanel; }
        }
    }

    public sealed class StringPanel : Panel
    {
        public event EventHandler OnSelectAll;
        public event EventHandler OnSelectNone;
        public event EventHandler OnSelectChange;

        public Dictionary<String, String> Localization;

        private readonly CheckBox _checkBox = new CheckBox();
        private readonly Panel _colorPanel = new Panel();

        public Boolean IsTitle;
        public String Event;
        public Image EventImage;
        public EventType EventType;
        public Color EventColor;
        private Boolean _showEventColor;
        public Boolean ShowEventColor
        {
            get { return _showEventColor; }
            set
            {
                _showEventColor = value;
                if (value && !IsTitle)
                {
                    Controls.Add(_colorPanel);
                }
                else
                {
                    Controls.Remove(_colorPanel);
                }
            }
        }
        public StringPanel()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Investigation_Event", "Event"},
								   {"Investigation_EventColor", "Event Color"},
							   };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Height = 40;

            BackColor = Color.Transparent;

            _checkBox.Padding = new Padding(10, 0, 0, 0);
            _checkBox.Dock = DockStyle.Left;
            _checkBox.Width = 25;

            _colorPanel.Padding = new Padding(0, 0, 0, 0);
            _colorPanel.Margin = new Padding(0, 0, 0, 0);
            _colorPanel.Location = new Point(250, 8);
            _colorPanel.Dock = DockStyle.None;
            _colorPanel.Size = new Size(40, 21);
            _colorPanel.Cursor = Cursors.Hand;

            Controls.Add(_checkBox);

            _checkBox.CheckedChanged += CheckBoxCheckedChanged;

            MouseClick += StringPanelMouseClick;
            Paint += StringPanelPaint;
        }

        public void RefreshColor()
        {
            _colorPanel.BackColor = EventColor;
        }

        private void PaintTitle(Graphics g)
        {
            if (Width <= 250) return;
            Manager.PaintTitleText(g, Localization["Investigation_Event"]);

            if (Width <= 300 || !ShowEventColor) return;
            g.DrawString(Localization["Investigation_EventColor"], Manager.Font, Manager.TitleTextColor, 250, 13);
        }

        private void StringPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);
                PaintTitle(g);
                return;
            }

            Manager.Paint(g, (Control)sender);

            Brush fontBrush = Brushes.Black;

            if (_checkBox.Visible && Checked)
            {
                fontBrush = SelectedColor;
            }

            if (Width <= 200) return;

            Manager.PaintText(g, Event, fontBrush, 80, 13);

            if (EventImage != null)
                g.DrawImage(EventImage, 40, 8, 32, 23);
        }

        private void StringPanelMouseClick(Object sender, MouseEventArgs e)
        {
                if (_checkBox.Visible)
                {
                    _checkBox.Checked = !_checkBox.Checked;
            }
        }

        private void CheckBoxCheckedChanged(Object sender, EventArgs e)
        {
            Invalidate();

            if (IsTitle)
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
            set { _checkBox.Visible = value; }
        }
    }
}
