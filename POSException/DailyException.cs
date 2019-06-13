using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using App;
using Constant;
using Interface;
using PanelBase;
using App_POSTransactionServer;
using System.Xml;
using static Constant.POS_Exception;

namespace POSException
{
    public sealed partial class DailyException : UserControl, IControl, IAppUse, IServerUse, IMinimize, IMouseHandler, IBlockPanelUse
    {
        public event EventHandler OnMinimizeChange;

        public Dictionary<String, String> Localization;

        public Dictionary<UInt16, POS_Exception> Exceptions { get;  set; }


        public String TitleName { get; set; }

        public Button Icon { get; private set; }
        private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
        public IApp App { get; set; }
        private string[] _exceptipKeyArray =new string[] { } ;
        private const String CgiLoadAllException = @"cgi-bin/posconfig?action=loadallexception";

        public IPOSManager POS { get; private set; }
        private IPTS _pts;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is IPTS)
                    _pts = value as IPTS;
            }
        }

        public IBlockPanel BlockPanel { get; set; }
        private readonly System.Timers.Timer _refreshTimer = new System.Timers.Timer();
        protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();
        private ToolStripMenuItemUI2 _reflashMenuItem;

        public UInt16 MinimizeHeight
        {
            get { return (UInt16)titlePanel.Size.Height; }
        }
        public Boolean IsMinimize { get; private set; }

        public DailyException()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Control_DailyException", "Daily Exception"},

                                   {"POSException_Summary", "Summary"},
                                   {"POSException_Refresh", "Refresh"},
                                   {"POSException_NoExceptionFound", " : Not Found"},
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            Dock = DockStyle.Fill;
            //---------------------------
            Icon = new ControlIconButton { Image = _iconActivate, BackgroundImage = ControlIconButton.IconBgActivate };
            Icon.Click += DockIconClick;
            //---------------------------
        }

        public void Initialize()
        {
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            PanelTitleBarUI2.Text = TitleName = Localization["Control_DailyException"];
            titlePanel.Controls.Add(PanelTitleBarUI2);
            PanelTitleBarUI2.InitializeToolStripMenuItem();

            SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

            if (_pts != null)
            {
                _pts.OnPOSModify += POSModify;
            }

            _reflashMenuItem = new ToolStripMenuItemUI2
            {
                Text = Localization["POSException_Refresh"],
            };
            _reflashMenuItem.Click += ReflashMenuItemClick;
            PanelTitleBarUI2.ToolStripMenuItem.DropDownItems.Add(_reflashMenuItem);

            _refreshTimer.Elapsed += RefreshExceptionStatus;
            _refreshTimer.Interval = 60000;//1 min
            _refreshTimer.SynchronizingObject = Server.Form;

            LoadException();
        }

        private void ReflashMenuItemClick(object sender, EventArgs e)
        {
            //reset timer
            _refreshTimer.Enabled = false;
            _refreshTimer.Enabled = true;

            //update right now
            RefreshExceptionStatus();
        }

        private void RefreshExceptionStatus(Object sender, EventArgs e)
        {
            RefreshExceptionStatus();
        }

        public void Activate()
        {
            if (_reloadTree)
            {
                if (_pts != null)
                    UpdatePOSList();
            }

            RefreshResult();
            _refreshTimer.Enabled = true;
            _reloadTree = false;
        }

        public void Deactivate()
        {
            _refreshTimer.Enabled = false;
        }

        public void UpdatePOSList()
        {
            posComboBox.SelectedIndexChanged -= POSComboBoxSelectedIndexChanged;

            posComboBox.Items.Clear();
            
            if (_pts.POS.POSServer.Count > 0)
            {
                posComboBox.Enabled = true;
                posComboBox.Items.Add(Localization["POSException_Summary"]);
                
                var sortResult = new List<IPOS>(_pts.POS.POSServer);
                sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));
                _exceptipKeyArray =new string [sortResult.Count];
                int _key = 0;
                foreach (var pos in sortResult)
                {
                    posComboBox.Items.Add(pos.ToString());
                    _exceptipKeyArray[_key] = pos.Exception.ToString();
                    _key++;
                }

                posComboBox.SelectedIndexChanged += POSComboBoxSelectedIndexChanged;

                posComboBox.SelectedIndex = 0;
            }
            else
            {
                posComboBox.Enabled = false;
            }
        }

        private readonly Queue<ExceptionPanel> _recyclePanel = new Queue<ExceptionPanel>();

        public ExceptionPanel GetExceptionPanel()
        {
            if (_recyclePanel.Count > 0)
            {
                return _recyclePanel.Dequeue();
            }

            var panel = new ExceptionPanel();

            panel.OnExceptionSelected += OnExceptionPanelSelected;

            return panel;
        }

        private void OnExceptionPanelSelected(Object sender, EventArgs e)
        {
            var item = sender as ExceptionPanel;
            if (item == null || item.ExceptionCount == null || item.ExceptionCount.Count == 0) return;

            var posList = item.POSIds.Where(id => _pts.POS.FindPOSById(id) != null).ToList();

            if (posList.Count == 0) return;

            App.SwitchPage("Report", new ExceptionReportParameter
            {
                POS = posList,
                DateTimeSet = DateTimeSet.Today,
                Exceptions = new List<String> { item.Exception }
            });

            //App.SwitchPage("Report", new ExceptionReportParameter
            //{
            //    POS = posList,
            //    StartDateTime = item.StartDateTime,
            //    EndDateTime = item.EndDateTime,
            //    DateTimeSet = DateTimeSet.None,
            //    Exceptions = new List<String> { item.Exception }
            //});
        }

        private void POSComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            RefreshExceptionStatus();
        }

        private void RefreshExceptionStatus()
        {
            if (posComboBox.Items.Count == 0) return;

            //total
            var ids = new List<String>();
            if (posComboBox.SelectedIndex > 0)
            {
                //PTS.POS.ReadDailyReportByStationGroupByException(0);
                ids.Add(posComboBox.SelectedItem.ToString().Split(' ')[0]);
            }
            else
            {
                ids.AddRange(_pts.POS.POSServer.Select(pos => pos.Id));
            }
            Enabled = false;

            //today, selected pos, all exception
            var today = Server.Server.DateTime;
            //var today = new DateTime(2012, 12, 14);
            var start = new DateTime(today.Year, today.Month, today.Day);
            var end = start.AddHours(24).AddMilliseconds(-1);//AddSeconds(-1);
            ReadDailyReportByStationGroupByExceptionDelegate loadReportDelegate = _pts.POS.ReadDailyReportByStationGroupByException;
            loadReportDelegate.BeginInvoke(ids.ToArray(), DateTimes.ToUtc(start, Server.Server.TimeZone), DateTimes.ToUtc(end, Server.Server.TimeZone), new String[] { }, LoadReportCallback, loadReportDelegate);
        }

        private POS_Exception.ExceptionCountList _reports;
        private delegate POS_Exception.ExceptionCountList ReadDailyReportByStationGroupByExceptionDelegate(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions);
        private delegate void LoadReportCallbackDelegate(IAsyncResult result);
        private void LoadReportCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new LoadReportCallbackDelegate(LoadReportCallback), result);
                return;
            }

            //exceptionPanel.Paint += ExceptionPanelPaint;
            Enabled = true;
            _reports = ((ReadDailyReportByStationGroupByExceptionDelegate)result.AsyncState).EndInvoke(result);

            if (_reports != null && _reports.ExceptionList.Count > 1)
                _reports.ExceptionList.Sort((x, y) => (x.Count - y.Count));

            RefreshResult();
        }

      

        public void LoadException()
        {
            
            var result = Xml.LoadTextFromHttp(CgiLoadAllException, Server.Credential);

            XmlDocument xmlDoc = new XmlDocument(); //= Xml.LoadXmlFromHttp(CgiLoadAllException, Server.Credential);
            xmlDoc.LoadXml(result.Replace("\r", "\\r").Replace("\n", "\\n"));

            if (xmlDoc == null) return;
            Exceptions = new Dictionary<ushort, POS_Exception>();
            Exceptions.Clear();
            var configurationList = xmlDoc.GetElementsByTagName("ExceptionConfiguration");
            foreach (XmlElement configurationNode in configurationList)
            {
                var exception = ParserXmlToException(configurationNode);
                Exceptions.Add(exception.Id, exception);
            }
        }

        public POS_Exception ParserXmlToException(XmlElement configurationNode)
        {
            var exception = new POS_Exception
            {
                ReadyState = ReadyState.Ready,
                Id = Convert.ToUInt16(configurationNode.GetAttribute("id")),
                Name = Xml.GetFirstElementValueByTagName(configurationNode, "Name"),
                Manufacture = Xml.GetFirstElementValueByTagName(configurationNode, "Manufacture"),
                Worker = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "Worker")),
                Buffer = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "Buffer")),
                Separator = Xml.GetFirstElementValueByTagName(configurationNode, "Separator").Replace("\r", "\\r").Replace("\n", "\\n"),
                TransactionType = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "TransactionType")),
                IsCapture = Xml.GetFirstElementValueByTagName(configurationNode, "IsCapture") == "true",
                IsSupportPOSId = Xml.GetFirstElementValueByTagName(configurationNode, "IsSupportPOSId") == "true"
            };


            Boolean editable = true;
            switch (exception.Manufacture)
            {
                case "Retalix":
                case "BHG":
                case "Radiant":
                case "Micros":
                case "ActiveMQ":
                case "Oracle":
                case "Oracle Demo":
                case "everrich":
                case "Generic":
                    editable = false;
                    break;
            }
            //-------------------------------------------------------------------------
            XmlNode exceptionsNode = configurationNode.SelectSingleNode("Exceptions");
            if (exceptionsNode != null)
            {
                foreach (XmlElement exceptionNode in exceptionsNode.ChildNodes)
                {
                    var editableValue = exceptionNode.GetAttribute("editable");
                    var valueType = exceptionNode.GetAttribute("valuetype");
                    exception.Exceptions.Add(new POS_Exception.Exception
                    {
                        Key = exceptionNode.GetAttribute("tag"),
                        Dir = exceptionNode.GetAttribute("dir"),
                        Value = exception.Manufacture != "Generic" ? exceptionNode.InnerText : exceptionNode.InnerText.Replace(" ", "/s").Replace(Environment.NewLine, exception.Separator).Replace("\t", "/t"),
                        Editable = String.IsNullOrEmpty(editableValue) ? editable : (editableValue == "true"),
                        ValueType = String.IsNullOrEmpty(valueType) ? "" : valueType,
                        TagEnd = exception.Manufacture != "Generic" ? exceptionNode.GetAttribute("tagend") : exceptionNode.GetAttribute("tagend").Replace(" ", "/s").Replace(Environment.NewLine, exception.Separator).Replace("\t", "/t")
                    });
                }

                exception.Exceptions.Sort((x, y) => (y.Key.CompareTo(x.Key)));
            }
            //-------------------------------------------------------------------------
            XmlNode segmentsNode = configurationNode.SelectSingleNode("Segments");
            if (segmentsNode != null)
            {
                foreach (XmlElement segmentNode in segmentsNode.ChildNodes)
                {
                    exception.Segments.Add(new POS_Exception.Segment
                    {
                        Key = segmentNode.GetAttribute("tag"),
                        Value = exception.Manufacture != "Generic" ? segmentNode.InnerText : segmentNode.InnerText.Replace(" ", "/s").Replace(Environment.NewLine, exception.Separator).Replace("\t", "/t"),
                        Editable = editable,
                        TagEnd = exception.Manufacture != "Generic" ? segmentNode.GetAttribute("tagend") : segmentNode.GetAttribute("tagend").Replace(" ", "/s").Replace(Environment.NewLine, exception.Separator).Replace("\t", "/t")
                    });
                }
            }
            //-------------------------------------------------------------------------
            XmlNode tagsNode = configurationNode.SelectSingleNode("Tags");
            if (tagsNode != null)
            {
                foreach (XmlElement tagNode in tagsNode)
                {
                    var dir = tagNode.GetAttribute("dir");
                    var valueType = tagNode.GetAttribute("valuetype");
                    exception.Tags.Add(new POS_Exception.Tag
                    {
                        Key = tagNode.GetAttribute("tag"),
                        Value = exception.Manufacture != "Generic" ? tagNode.InnerText : tagNode.InnerText.Replace(" ", "/s").Replace(Environment.NewLine, exception.Separator).Replace("\t", "/t"),
                        Editable = editable,
                        Dir = String.IsNullOrEmpty(dir) ? "" : dir,
                        ValueType = String.IsNullOrEmpty(valueType) ? "" : valueType,
                        TagEnd = exception.Manufacture != "Generic" ? tagNode.GetAttribute("tagend") : tagNode.GetAttribute("tagend").Replace(" ", "/s").Replace(Environment.NewLine, exception.Separator).Replace("\t", "/t")
                    });
                }

                exception.Tags.Sort((x, y) => (y.Key.CompareTo(x.Key)));
            }

            return exception;
        }


        private void RefreshResult()
		{
            //containerPanel.Visible = false;

        
             var result = Xml.LoadTextFromHttp(CgiLoadAllException, _server.Credential);

            foreach (ExceptionPanel panel in exceptionPanel.Controls)
			{
				panel.ExceptionThreshold = null;
				if (!_recyclePanel.Contains(panel))
					_recyclePanel.Enqueue(panel);
			}
			exceptionPanel.Controls.Clear();




            if (_reports == null || (_reports.ExceptionList.Count == 0))
			{
                PanelTitleBarUI2.Text = TitleName + Localization["POSException_NoExceptionFound"];
				return;
			}

            PanelTitleBarUI2.Text = TitleName;

            /*
            foreach (KeyValuePair<UInt16, POS_Exception> posException in Exceptions)
            {
                foreach (var exception in posException.Value.Exceptions)
                {
                    var panel = GetExceptionPanel();
                    panel.Exception = exception.Key;
                    panel.POSIds = _reports.POSIds;
                    panel.StartDateTime = _reports.StartDateTime;
                    panel.EndDateTime = _reports.EndDateTime;
                    ExceptionCount excep = new ExceptionCount();
                    excep.Exception = exception.Key;
                    excep.Count = 123123;
                    panel.ExceptionCount = excep;
                    foreach (var threshold in _pts.POS.ExceptionThreshold)
                    {
                        if (threshold.Key == panel.Exception)
                        {
                            panel.ExceptionThreshold = threshold.Value;
                            break;
                        }
                    }

                    if (panel.ExceptionThreshold == null)
                    {
                        _recyclePanel.Enqueue(panel);
                        continue;
                    }

                    panel.Initialize();
                    exceptionPanel.Controls.Add(panel);
                }


            }
            */

            foreach (var report in _reports.ExceptionList)
			{
				var panel = GetExceptionPanel();
				panel.Exception = report.Exception;
				panel.POSIds = _reports.POSIds;
				panel.StartDateTime = _reports.StartDateTime;
				panel.EndDateTime = _reports.EndDateTime;
				panel.ExceptionCount = report;

				foreach (var threshold in _pts.POS.ExceptionThreshold)
				{
					if (threshold.Key == panel.Exception)
					{
						panel.ExceptionThreshold = threshold.Value;
						break;
					}
				}
				if (panel.ExceptionThreshold == null)
				{
					_recyclePanel.Enqueue(panel);
					continue;
				}

				panel.Initialize();
				exceptionPanel.Controls.Add(panel);
			}

			//containerPanel.Visible = true;
		}

		private Boolean _reloadTree = true;
		public void POSModify(Object sender, EventArgs<IPOS> e)
		{
			_reloadTree = true;
		}

		public void GlobalMouseHandler()
		{
			if (Drag.IsDrop(containerPanel))
			{
				if (!exceptionPanel.AutoScroll)
				{
					exceptionPanel.AutoScroll = true;
					//viewModelPanel.AutoScrollPosition = _previousScrollPosition;
				}
				//viewModelPanel.Focus();

				return;
			}
			//viewModelPanel.AutoScroll = false;
			if (exceptionPanel.AutoScroll)
				HideScrollBar();
		}

		private Point _previousScrollPosition;
		private void HideScrollBar()
		{
			_previousScrollPosition = exceptionPanel.AutoScrollPosition;
			_previousScrollPosition.Y *= -1;
			exceptionPanel.AutoScroll = false;

			//force refresh to hide scroll bar
			exceptionPanel.Height++;
			exceptionPanel.AutoScrollPosition = _previousScrollPosition;
		}

		private void DockIconClick(Object sender, EventArgs e)
		{
			if (IsMinimize)
				Maximize();
			else
				Minimize();
		}

		public void Minimize()
		{
            if (BlockPanel.LayoutManager.Page.Version == "2.0" && !IsMinimize)
            {
                BlockPanel.HideThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = true;
                    App.StartupOption.SaveSetting();
                }
            }

            Icon.Image = _icon;
            Icon.BackgroundImage = null;

            Icon.Invalidate();

            IsMinimize = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
		}

		public void Maximize()
		{
            if (BlockPanel.LayoutManager.Page.Version == "2.0")
            {
                BlockPanel.ShowThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = false;
                    App.StartupOption.SaveSetting();
                }
            }

            Icon.Image = _iconActivate;
            Icon.BackgroundImage = ControlIconButton.IconBgActivate;

            IsMinimize = false;

            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
		}
	}

	public sealed class ExceptionPanel : Panel
	{
		public event EventHandler OnExceptionSelected;

		public String[] POSIds;
		public UInt64 StartDateTime;
		public UInt64 EndDateTime;

		public String Exception;
		public POS_Exception.ExceptionThreshold ExceptionThreshold;
		public POS_Exception.ExceptionCount ExceptionCount;

		private readonly Label _countLabel = new Label();
		private readonly Label _threshold1Label = new Label();
		private readonly Label _threshold2Label = new Label();
		private readonly Label _thresholdLabel = new Label();
		private readonly Button _exceptionButton = new Button();

		public Dictionary<String, String> Localization;
		public Boolean IsTitle;
		public ExceptionPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"POS_Threshold", "Threshold"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.Default;
			Height = 30;

			BackColor = Color.Transparent;
			Padding = new Padding(0, 3, 0, 3);

			_exceptionButton.Dock = DockStyle.Left;
			_exceptionButton.Size = new Size(132, 22);
			_exceptionButton.Font = _dafaultFont;
			_exceptionButton.TextAlign = ContentAlignment.MiddleLeft;

			_countLabel.Dock = DockStyle.Left;
			_countLabel.Size = new Size(25, 20);
			_countLabel.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			_countLabel.TextAlign = ContentAlignment.MiddleRight;
			_countLabel.ForeColor = Color.White;
			_countLabel.BackColor = Color.Transparent;

			_threshold1Label.Dock = DockStyle.Left;
			_threshold1Label.Size = new Size(25, 20);
			_threshold1Label.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			_threshold1Label.TextAlign = ContentAlignment.MiddleRight;
			_threshold1Label.ForeColor = Color.White;
			_threshold1Label.BackColor = Color.Transparent;

			_threshold2Label.Dock = DockStyle.Left;
			_threshold2Label.Size = new Size(25, 20);
			_threshold2Label.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			_threshold2Label.TextAlign = ContentAlignment.MiddleRight;
			_threshold2Label.ForeColor = Color.White;
			_threshold2Label.BackColor = Color.Transparent;

			_thresholdLabel.Dock = DockStyle.Left;
			_thresholdLabel.Size = new Size(65, 20);
			_thresholdLabel.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			_thresholdLabel.TextAlign = ContentAlignment.MiddleRight;
			_thresholdLabel.ForeColor = Color.White;
			_thresholdLabel.BackColor = Color.Transparent;
			_thresholdLabel.Text = Localization["POS_Threshold"];// +@" : ";

			var gap1 = new Label
			{
				Dock = DockStyle.Left,
				Size = new Size(2, 20),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				ForeColor = Color.White,
				BackColor = Color.Transparent
			};

			var gap2 = new Label
			{
				Dock = DockStyle.Left,
				Size = new Size(2, 20),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				ForeColor = Color.White,
				BackColor = Color.Transparent
			};

			Controls.Add(_threshold2Label);
			Controls.Add(gap1);
			Controls.Add(_threshold1Label);
			Controls.Add(_thresholdLabel);
			Controls.Add(_countLabel);
			Controls.Add(gap2);
			Controls.Add(_exceptionButton);

			//Paint += ExceptionPanelPaint;
			//_countLabel.Paint += CountLabelPaint;
			_threshold1Label.Paint += Threshold1LabelPaint;
			_threshold2Label.Paint += Threshold2LabelPaint;

			_exceptionButton.Click += ExceptionButtonClick;
			//_countLabel.Click += ExceptionPanelClick;
		}

		private readonly Font _dafaultFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
		private readonly Font _middleFont = new Font("Arial", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
		private readonly Font _smallFont = new Font("Arial", 6.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
		private readonly Font _tinyFont = new Font("Arial", 6F, FontStyle.Regular, GraphicsUnit.Point, 0);
		public void Initialize()
		{
			if (ExceptionThreshold == null)
			{
				return;
			}

            SharedToolTips.SharedToolTip.SetToolTip(_exceptionButton, POS_Exception.FindExceptionValueByKey(ExceptionCount.Exception));
            _exceptionButton.Text = POS_Exception.FindExceptionValueByKey(ExceptionCount.Exception);
			_exceptionButton.Font = _dafaultFont;

			_countLabel.Text = ExceptionCount.Count.ToString();
			_threshold1Label.Text = ExceptionThreshold.ThresholdValue1.ToString();
			_threshold2Label.Text = ExceptionThreshold.ThresholdValue2.ToString();

			if (ExceptionCount.Count < 100) //##
			{
				_countLabel.Font = _dafaultFont;
			}
			else if (ExceptionCount.Count < 1000) //###
			{
				_countLabel.Font = _middleFont;
			}
			else if (ExceptionCount.Count < 10000) //####
			{
				_countLabel.Font = _smallFont;
			}
			else//#####
			{
				_countLabel.Font = _tinyFont;
			}
			
			if (ExceptionThreshold.ThresholdValue1 < 100) //##
			{
				_threshold1Label.Font = _dafaultFont;
			}
			else if (ExceptionThreshold.ThresholdValue1 < 1000) //###
			{
				_threshold1Label.Font = _middleFont;
			}
			else if (ExceptionThreshold.ThresholdValue1 < 10000) //####
			{
				_threshold1Label.Font = _smallFont;
			}
			else//#####
			{
				_threshold1Label.Font = _tinyFont;
			}

			if (ExceptionThreshold.ThresholdValue2 < 100) //##
			{
				_threshold2Label.Font = _dafaultFont;
			}
			else if (ExceptionThreshold.ThresholdValue2 < 1000) //###
			{
				_threshold2Label.Font = _middleFont;
			}
			else if (ExceptionThreshold.ThresholdValue2 < 10000) //####
			{
				_threshold2Label.Font = _smallFont;
			}
			else//#####
			{
				_threshold2Label.Font = _tinyFont;
			}

			//red alert)
			if (ExceptionCount.Count >= ExceptionThreshold.ThresholdValue2)
			{
				_threshold1Label.BackColor = YellowColor;
				_threshold2Label.BackColor = RedColor;

				_threshold1Label.ForeColor = SystemColors.ControlText;
				_threshold2Label.ForeColor = SystemColors.ControlText;

				_countLabel.BackColor = _threshold2Label.BackColor;
				_countLabel.ForeColor = _threshold2Label.ForeColor;
			}
			else if (ExceptionCount.Count >= ExceptionThreshold.ThresholdValue1)
			{
				_threshold1Label.BackColor = YellowColor;
				_threshold2Label.BackColor = Color.Transparent;

				_threshold1Label.ForeColor = SystemColors.ControlText;
				_threshold2Label.ForeColor = Color.White;

				_countLabel.BackColor = _threshold1Label.BackColor;
				_countLabel.ForeColor = _threshold1Label.ForeColor;
			}
			else
			{
				_threshold1Label.BackColor = Color.Transparent;
				_threshold2Label.BackColor = Color.Transparent;

				_threshold1Label.ForeColor = Color.White;
				_threshold2Label.ForeColor = Color.White;

				_countLabel.BackColor = GreenColor;
				_countLabel.ForeColor = Color.White;
			}

			Invalidate();
		}

		private void ExceptionButtonClick(Object sender, EventArgs e)
		{
			if (ExceptionThreshold == null) return;

			if (OnExceptionSelected != null)
				OnExceptionSelected(this, null);
		}

		private static readonly Color RedColor = ColorTranslator.FromHtml("#f17d7d");
		private static readonly Color YellowColor = ColorTranslator.FromHtml("#d5d578");
		private static readonly Color GreenColor = ColorTranslator.FromHtml("#008000");
		//private readonly Pen _whitePen = new Pen(Color.White);
		private readonly Pen _yellowPen = new Pen(YellowColor);
		private readonly Pen _redPen = new Pen(RedColor);
		//private void CountLabelPaint(Object sender, PaintEventArgs e)
		//{
		//    Graphics g = e.Graphics;

		//    g.DrawRectangle(_whitePen, 0, 0, _countLabel.Width - 1, _countLabel.Height - 1);
		//}
		
		private void Threshold1LabelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.DrawRectangle(_yellowPen, 0, 0, _threshold1Label.Width - 1, _threshold1Label.Height - 1);
		}

		private void Threshold2LabelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.DrawRectangle(_redPen, 0, 0, _threshold2Label.Width - 1, _threshold2Label.Height - 1);
		}
	}
}
