using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace POSException
{
	public sealed partial class TransactionDetail : UserControl, IControl, IAppUse, IServerUse, IMinimize
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<String>> OnTimecodeChange;
		public event EventHandler<EventArgs<IDeviceGroup>> OnTransactionDoubleClick;
		public event EventHandler<EventArgs<String>> OnExportDateTime;
		public event EventHandler OnLoadCompleted;
		public Dictionary<String, String> Localization;

		public String TitleName { get; set; }

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);

		private const UInt16 MaximumAmount = 100;
		
		public IApp App { get; set; }
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

		private readonly PanelTitleBarUI3 _panelTitleBar = new PanelTitleBarUI3();
        private readonly ToolTip _toolTip = new ToolTip();
		private PictureBox _pauseEvent;
		private PictureBox _resetButton;
		private ComboBox _posComboBox;
		public UInt16 MinimizeHeight
		{
			get { return (UInt16)titlePanel.Size.Height; }
		}
		public Boolean IsMinimize { get; private set; }
        
		private static readonly Image _reset = Resources.GetResources(Properties.Resources.reset, Properties.Resources.IMGReset);
		private static readonly Image _pauseevent= Resources.GetResources(Properties.Resources.pause_event, Properties.Resources.IMGPauseEvent);
		private static readonly Image _pauseeventactivate= Resources.GetResources(Properties.Resources.pause_event_activate, Properties.Resources.IMGPauseEventActivate);
		
		public TransactionDetail()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},

								   {"Control_POSExceptionTransactionDetail", "Transaction Details"},
								   {"Control_TransactionInRealTime", "Transaction in real-time"},
								   
								   {"PTSReports_SaveReport", "Save Report"},
								   {"POSException_All", "All"},
								   
								   {"POSException_NoTransactionFound", " : No Transaction Found"},
								   {"POSException_Searching", "Searching"},

								   {"POSException_ClearTransaction", "Clear transaction"},
								   {"POSException_PauseEvent", "Suspended receive transactions"},
								   {"POSException_ResumeEvent", "Resuming receive transactions"},
								   
								   {"POSException_SelectTransactionBeforeExport", "Select transaction before save report."},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			Dock = DockStyle.Fill;
			DoubleBuffered = true;

			transactionListBox.Cursor = Cursors.Default;
			transactionListBox.DrawMode = DrawMode.OwnerDrawFixed;
			transactionListBox.DrawItem += TransactionListBoxDrawItem;
			//transactionListBox.MouseMove += TransactionListBoxMouseMove;
			transactionListBox.SelectedIndexChanged += TransactionListBoxSelectedIndexChanged;
			//---------------------------
			Icon = new ControlIconButton { Image = _icon };
			Icon.Click += DockIconClick;
			//---------------------------

            transactionListBox.BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.BGControllerBG);
		}

		public void Initialize()
		{
			_panelTitleBar.Text = TitleName = Localization["Control_POSExceptionTransactionDetail"];
			_panelTitleBar.Panel = this;
			titlePanel.Controls.Add(_panelTitleBar);

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

			if (_pts != null)
			{
				_pts.OnPOSModify += POSModify;
			}
		}

		//private String _prevTooltips;
		//private void TransactionListBoxMouseMove(Object sender, MouseEventArgs e)
		//{
		//    String strTip = "";

		//    int nIdx = transactionListBox.IndexFromPoint(e.Location);
		//    if ((nIdx >= 0) && (nIdx < transactionListBox.Items.Count))
		//        strTip = transactionListBox.Items[nIdx].ToString();
			
		//    if (_prevTooltips == strTip) return;

		//    SharedToolTips.SharedToolTip.SetToolTip(transactionListBox, strTip);
		//    _prevTooltips = strTip;
		//}

		public void RemoveTitlePanel()
		{
			Controls.Remove(titlePanel);
			Padding = new Padding(0);
		}

		public Color BackgroundColor
		{
			set
			{
				transactionListBox.BackColor = value;
			}
		}

		public Brush FontColor
		{
			set
			{
				_fontColor = value;
			}
		}

		public Boolean DisplayDateTime = true;

		public void SetLiveProperty()
		{
			_panelTitleBar.Text = TitleName = Localization["Control_TransactionInRealTime"];

			transactionListBox.MouseDoubleClick += TransactionListBoxMouseDoubleClick;
			if (_pts != null)
			{
				_pts.OnPOSEventReceive -= POSEventReceive;
				_pts.OnPOSEventReceive += POSEventReceive;
			}

			transactionListBox.DrawMode = DrawMode.Normal;
			transactionListBox.DrawItem -= TransactionListBoxDrawItem;
			AddIconSets();
		}

		public void AddIconSets()
		{
			_posComboBox = new ComboBox
			{
				Dock = DockStyle.Right,
				Size = new Size(120, 23),
				Anchor = AnchorStyles.Top | AnchorStyles.Right,
				Location = new Point(4, 6),
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				FlatStyle = FlatStyle.System,
				DropDownStyle = ComboBoxStyle.DropDownList,
			};
			_posComboBox.SelectedIndexChanged += POSComboBoxSelectedIndexChanged;
			_panelTitleBar.Controls.Add(_posComboBox);

			_resetButton = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _reset,
				BackgroundImageLayout = ImageLayout.Center
			};

			SharedToolTips.SharedToolTip.SetToolTip(_resetButton, Localization["POSException_ClearTransaction"]);

			_resetButton.MouseClick += ResetButtonMouseClick;
			_panelTitleBar.Controls.Add(_resetButton);

			_pauseEvent = new PictureBox
			{
				Dock = DockStyle.Right,
				Cursor = Cursors.Hand,
				Size = new Size(25, 25),
				BackColor = Color.Transparent,
				BackgroundImage = _pauseevent,
				BackgroundImageLayout = ImageLayout.Center
			};
			_pauseEvent.MouseClick += PauseEvent;
			_panelTitleBar.Controls.Add(_pauseEvent);
			SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["POSException_PauseEvent"]);
		}

		private String _selectedPOSId;
		private void POSComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			//total
			if (_posComboBox.SelectedIndex != 0)
			{
				//PTS.POS.ReadDailyReportByStationGroupByException(0);
				_selectedPOSId = _posComboBox.SelectedItem.ToString().Split(' ')[0];
			}
			else
			{
				_selectedPOSId = "0";
			}
		}

		private Boolean _isActivate;
		public void Activate()
		{
			if (_reloadTree)
			{
				if (_pts != null)
					UpdatePOSList();
			}

			_reloadTree = false;
			_isActivate = true;
		}

		public void Deactivate()
		{
			_isActivate = false;
		}

		public void UpdatePOSList()
		{
			if (_posComboBox == null) return;

			_posComboBox.SelectedIndexChanged -= POSComboBoxSelectedIndexChanged;

			_posComboBox.Items.Clear();

			if(_pts.POS.POSServer.Count > 0)
			{
				_posComboBox.Enabled = true;
				_posComboBox.Items.Add(Localization["POSException_All"]);

				var sortResult = new List<IPOS>(_pts.POS.POSServer);
				sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));

				foreach (var pos in sortResult)
				{
					_posComboBox.Items.Add(pos.ToString());
				}

				_posComboBox.SelectedIndexChanged += POSComboBoxSelectedIndexChanged;

				_posComboBox.SelectedIndex = 0;
			}
			else
			{
				_posComboBox.Enabled = false;
			}
		}

		private Boolean _reloadTree = true;
		public void POSModify(Object sender, EventArgs<IPOS> e)
		{
			_reloadTree = true;
		}

		private void ResetButtonMouseClick(Object sender, MouseEventArgs e)
		{
			SharedToolTips.SharedToolTip.SetToolTip(transactionListBox, "");
			transactionListBox.Items.Clear();
		}

		public Boolean IsPauseReceiveEvent;
		private void PauseEvent(Object sender, MouseEventArgs e)
		{
			IsPauseReceiveEvent = !IsPauseReceiveEvent;

			if (IsPauseReceiveEvent)
			{
				_pauseEvent.BackgroundImage = _pauseeventactivate;
				SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["POSException_ResumeEvent"]);
			}
			else
			{
				_pauseEvent.BackgroundImage = _pauseevent;
				SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["POSException_PauseEvent"]);
			}
		}

        public void UpdateSearchResult(Object sender, EventArgs<String[], String, POS_Exception.AdvancedSearchCriteria> e)
        {
            SearchCriteria = e.Value3;
			UpdateSearchResult(e.Value1, e.Value2);
		}

		private POS_Exception.TransactionItemList _transactionItemList;
        public POS_Exception.AdvancedSearchCriteria SearchCriteria;
		private String _transactionId;
		public void UpdateSearchResult(String[] keywords, String transactionId)
		{
			_keywords = (keywords != null)
							? keywords.Select(keyword => keyword.Trim()).ToArray()
							: keywords;
			_transactionId = transactionId;

			Maximize();

			//SharedToolTips.SharedToolTip.SetToolTip(transactionListBox, "");
            _toolTip.SetToolTip(transactionListBox, "");
			transactionListBox.Items.Clear();
			//transactionListBox.Font = _emptyFont;

			if (String.IsNullOrEmpty(transactionId))
			{
				_panelTitleBar.Text = TitleName + Localization["POSException_NoTransactionFound"];
				return;
			}
			_panelTitleBar.Text = TitleName + @" " + Localization["POSException_Searching"] + @"...";
			
			ReadTransactionByIdDelegate loadReportDelegate = _pts.POS.ReadTransactionById;
			loadReportDelegate.BeginInvoke(transactionId, LoadReportCallback, loadReportDelegate);
		}

		private delegate POS_Exception.TransactionItemList ReadTransactionByIdDelegate(String transactionposId);
		private delegate void LoadReportCallbackDelegate(IAsyncResult result);
		private void LoadReportCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new LoadReportCallbackDelegate(LoadReportCallback), result);
				}
				catch (Exception)
				{
					if (OnLoadCompleted != null)
						OnLoadCompleted(this, null);
				}
				return;
			}

			_transactionItemList = ((ReadTransactionByIdDelegate)result.AsyncState).EndInvoke(result);

			if (OnExportDateTime != null)
				OnExportDateTime(this, new EventArgs<String>(ExportDateTimeChangeXml()));

			//SharedToolTips.SharedToolTip.SetToolTip(transactionListBox, "");
            _toolTip.SetToolTip(transactionListBox, "");
			transactionListBox.Items.Clear();
			if (_transactionItemList.TransactionItems.Count == 0)
			{
				_panelTitleBar.Text = TitleName + Localization["POSException_NoTransactionFound"];
				return;
			}

			_panelTitleBar.Text = TitleName;
			//transactionListBox.Font = _defaultFont;

		    var needHighLightTimeInterval = false;
            if (SearchCriteria != null)
            {
                if (SearchCriteria.TimeIntervalCriteria != null)
                {
                    if (SearchCriteria.TimeIntervalCriteria.Enable)
                    {
                        needHighLightTimeInterval = true;
                    }
                }
            }

            TransactionItemControl tempTransactionItemControl = null;
			foreach (var transactionItemDetail in _transactionItemList.TransactionItems)
			{
			    var transactionItemControl = new TransactionItemControl
			                                     {
			                                         DisplayDateTime = DisplayDateTime,
			                                         POS = _pts.POS.FindPOSById(transactionItemDetail.POS),
			                                         TransactionItem = transactionItemDetail,
			                                     };
				transactionListBox.Items.Add(transactionItemControl);

                if (needHighLightTimeInterval)
                {
                    if (tempTransactionItemControl != null && transactionItemDetail.ValueType == "$")
                    {
                        if(transactionItemDetail.UTC - tempTransactionItemControl.TransactionItem.UTC > (ulong) (SearchCriteria.TimeIntervalCriteria.Sec * 1000))
                        {
                            transactionItemControl.HeightVerified = transactionItemControl.NeedHeight = true;
                            transactionItemControl.HighLightRange.Add(new HightLightRange
                            {
                                Start = 0,
                                End = transactionItemControl.TransactionItem.Content.Length * 10
                            });

                            tempTransactionItemControl.HeightVerified = tempTransactionItemControl.NeedHeight = true;
                            tempTransactionItemControl.HighLightRange.Add(new HightLightRange
                            {
                                Start = 0,
                                End = tempTransactionItemControl.TransactionItem.Content.Length * 10
                            });
                        }
                    }

                    if (transactionItemDetail.ValueType == "$")
                        tempTransactionItemControl = transactionItemControl;
                }
                
			}
			if (OnLoadCompleted != null)
				OnLoadCompleted(this, null);

			AutoScrollTransactionItems();
		}

		private String ExportDateTimeChangeXml()
		{
			var xmlDoc = new XmlDocument();

			var xmlRoot = xmlDoc.CreateElement("XML");
			xmlDoc.AppendChild(xmlRoot);

			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartTime", _transactionItemList.StartDateTime));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndTime", Math.Max(_transactionItemList.EndDateTime, _transactionItemList.StartDateTime + 2000)));

			return xmlDoc.InnerXml;
		}

		private TransactionItemControl _previousSelectItem;
		private Boolean _ignore;
		private void TransactionListBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if(_ignore) return;
			if (_previousSelectItem == transactionListBox.SelectedItem)
				return;

			_previousSelectItem = transactionListBox.SelectedItem as TransactionItemControl;

			if (_previousSelectItem == null)
				return;

			//SharedToolTips.SharedToolTip.SetToolTip(transactionListBox, _previousSelectItem.ToString());
            _toolTip.SetToolTip(transactionListBox, _previousSelectItem.ToString());

			DateTimeSelectionChange(_previousSelectItem.TransactionItem.DateTime);
		}

		private void TransactionListBoxMouseDoubleClick(Object sender, MouseEventArgs e)
		{
			var doubleClickItem = transactionListBox.SelectedItem as TransactionItemControl;

			if (doubleClickItem == null)
				return;

			if (doubleClickItem.TransactionItem == null) return;

			IPOS pos = _pts.POS.FindPOSById(doubleClickItem.TransactionItem.POS);
			if (pos != null)
			{
				if (OnTransactionDoubleClick != null)
					OnTransactionDoubleClick(this, new EventArgs<IDeviceGroup>(pos));
			}
		}

		private void DateTimeSelectionChange(DateTime dateTime)
		{
			if (OnTimecodeChange != null)
			{
				String timecode = DateTimes.ToUtcString(dateTime, Server.Server.TimeZone);
				OnTimecodeChange(this, new EventArgs<String>(TimecodeChangeXml(timecode)));
			}
		}

		private static String TimecodeChangeXml(String timestamp)
		{
			var xmlDoc = new XmlDocument();

			XmlElement xmlRoot = xmlDoc.CreateElement("XML");
			xmlDoc.AppendChild(xmlRoot);

			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Timestamp", timestamp));

			return xmlDoc.InnerXml;
		}

		private void POSEventReceive(Object sender, EventArgs<POS_Exception.TransactionItem> e)
		{
			if (IsPauseReceiveEvent) return;
			if (!_isActivate) return;

			AddEvent(e.Value);
		}

		private delegate void AddEventDelegate(POS_Exception.TransactionItem transactionItem);
		private void AddEvent(POS_Exception.TransactionItem transactionItem)
		{   
			if (_selectedPOSId != "0" && _selectedPOSId != transactionItem.POS) return;
            if (transactionItem.POS == "PTSDemo") return;
			if (InvokeRequired)
			{
				try
				{
					Invoke(new AddEventDelegate(AddEvent), transactionItem);
				}
				catch (Exception)
				{
				}
				return;
			}

			if (transactionListBox.Items.Count > MaximumAmount)
			{
				//transactionListBox.Items.RemoveAt(0);
				//recycle
				var item = transactionListBox.Items[0] as TransactionItemControl;
				if (item != null)
				{
					item.TransactionItem = transactionItem;
					item.HeightVerified = item.NeedHeight = false;
					item.POS = _pts.POS.FindPOSById(transactionItem.POS);
					transactionListBox.Items.RemoveAt(0);
					transactionListBox.Items.Add(item);
				}
			}
			else
			{
				transactionListBox.Items.Add(new TransactionItemControl
				{
					POS = _pts.POS.FindPOSById(transactionItem.POS),
					TransactionItem = transactionItem,
					DisplayPOS = true,
				});

				//transactionListBox.Items.Add(transactionItemDetail.DateTime.ToString("  yyyy-MM-dd  HH:mm:ss       ") + transactionItemDetail.Content);
			}
			//scroll down
			_ignore = true;
			transactionListBox.SelectedIndex = transactionListBox.Items.Count - 1;
			//select none
			transactionListBox.SelectedIndex = -1;
			_ignore = false;
		}

		//private readonly SolidBrush _textBg = new SolidBrush(Color.FromArgb(98, 98, 98));
		private readonly SolidBrush _highlightTextBrush = new SolidBrush(Color.FromArgb(51, 153, 255));
		//private readonly SolidBrush _highlightBg = new SolidBrush(SystemColors.Highlight);
		private float _fontwidth;// = 0;//(float)7.45; //Lucida Console, 9pt
		private float _fontSpace;// = 0;//(float)7.45; //Lucida Console, 9pt
		private readonly Font _defaultFont = new Font("Lucida Console", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
		//private readonly Font _emptyFont = new Font("Bottom", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
		private Brush _fontColor = Brushes.White;

		private void TransactionListBoxDrawItem(Object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			
			Boolean selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

			Int32 index = e.Index;

			if (index < 0 || index >= transactionListBox.Items.Count) return;

			String text = transactionListBox.Items[index].ToString();
			Graphics graphics = e.Graphics;

			if (_fontwidth == 0)
			{
				var size1 = graphics.MeasureString("1", _defaultFont);
				var size2 = graphics.MeasureString("11", _defaultFont);
				_fontwidth = size2.Width - size1.Width;
				_fontSpace = size1.Width - _fontwidth;
			}

			//var bg = (selected) ? _highlightBg : _textBg;
			//graphics.FillRectangle(bg, e.Bounds);
			//if(selected)
			//    graphics.FillRectangle(_highlightBg, e.Bounds);
			//var xxx = sender as TransactionItemControl;
				
			var rectangle = transactionListBox.GetItemRectangle(index);//.Location;
			if (!selected)
			{
				var transactionItemControl = transactionListBox.Items[index] as TransactionItemControl;
				if (transactionItemControl != null)
				{
					PaintHightLightText(text, graphics, rectangle, transactionItemControl);
				}
				//SizeF fSize = graphics.MeasureString(text, detailListBox.Font);
			}
			// Print text
			graphics.DrawString(text, e.Font, _fontColor, rectangle.Location);

			//e.DrawFocusRectangle();
		}

		private String[] _keywords = new String[0];
		private readonly CultureInfo _enus = new CultureInfo("en-US");
		private void PaintHightLightText(String text, Graphics graphics, Rectangle rectangle, TransactionItemControl transactionItemControl)
		{
			if (transactionItemControl.HeightVerified)
			{
				if (transactionItemControl.NeedHeight)
				{
					foreach (var point in transactionItemControl.HighLightRange)
					{
						graphics.FillRectangle(_highlightTextBrush, rectangle.X + point.Start,
							rectangle.Y, point.End, rectangle.Height);
					}
				}
				
				return;
			}

			transactionItemControl.HeightVerified = true;
			transactionItemControl.HighLightRange.Clear();
			if (_keywords == null || String.IsNullOrEmpty(text) || graphics == null) return;
			
			var keywords = new List<String>(_keywords);

			//add exception's value (ex: V.O.I.D or -V) to highlight list
			if (transactionItemControl.POS != null && _pts.POS.Exceptions.ContainsKey(transactionItemControl.POS.Exception))
			{
				var excep = _pts.POS.Exceptions[transactionItemControl.POS.Exception];

				foreach (var keyword in _keywords)
				{
					foreach (var exception in excep.Exceptions)
					{
						if (exception.Key == keyword)
						{
							if (!keywords.Contains(exception.Value))
								keywords.Add(exception.Value);
							break;
						}
					}
				}
			}

			foreach (var keyword in keywords)
			{
				if(String.IsNullOrEmpty(keyword)) continue;

				var start = text.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase);
				if (start < 0) continue;

				var end = Math.Min(keyword.Length, text.Length - start);
				var target = text.Substring(start, end).Trim();

				if (target.ToUpper(_enus) != keyword.ToUpper(_enus))
					continue;

				transactionItemControl.NeedHeight = true;

                var startString = start == 0 ? text.Substring(start, end).Replace(" ", "1") : text.Substring(0, start).Replace(" ", "1");
                var startSize1 = graphics.MeasureString(startString, _defaultFont);
			    var doubleString = startString + "" + startString;
                var startSize2 = graphics.MeasureString(doubleString, _defaultFont);
                var startFontWidth = start == 0 ? 0 : (startSize2.Width - startSize1.Width) / startString.Length;
                
                var size1 = graphics.MeasureString(keyword, _defaultFont);
                var size2 = graphics.MeasureString(keyword + keyword, _defaultFont);
                var fontwidth = (size2.Width - size1.Width) / keyword.Length;
               
				transactionItemControl.HighLightRange.Add(new HightLightRange
															  {
																  //Start = start*_fontwidth + _fontSpace - 3,
                                                                  Start = startFontWidth * startString.Length,
                                                                  End = keyword.Length * fontwidth + 2
															  });
			}

			foreach (var point in transactionItemControl.HighLightRange)
			{
				graphics.FillRectangle(_highlightTextBrush, rectangle.X + point.Start,
					rectangle.Y, point.End, rectangle.Height);
			}
		}
		
		private String _timestamp;
		public void GoTo(Object sender, EventArgs<String> e)
		{
			_timestamp = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "Timestamp");
			AutoScrollTransactionItems();
		}

		public void AutoScrollTransactionItems()
		{
			//hight transaction item
			if(String.IsNullOrEmpty(_timestamp)) return;

			if(transactionListBox.Items.Count == 0) return;

			var utc =Convert.ToUInt64(_timestamp);
			if(utc < (_transactionItemList.StartDateTime - 1000))
			{
				transactionListBox.SelectedIndex = -1;
				return;
			}
			if (utc > (_transactionItemList.EndDateTime + 1000))
			{
				transactionListBox.SelectedIndex = -1;
				return;
			}

			//same timecode
			if (transactionListBox.SelectedItem != null && ((TransactionItemControl)transactionListBox.SelectedItem).TransactionItem.UTC == utc)
				return;

			TransactionItemControl target = null;
			var diff = 999999999999999;
			foreach (TransactionItemControl transactionItemControl in transactionListBox.Items)
			{
				//if (utc == transactionItemControl.TransactionItem.UTC)
				//{
				//    target = transactionItemControl;
				//    break;
				//}

				var temp = Convert.ToInt32((utc > transactionItemControl.TransactionItem.UTC)
					? (utc - transactionItemControl.TransactionItem.UTC)
					: (transactionItemControl.TransactionItem.UTC - utc));

				//find the close and last item
				if (Math.Abs(temp) <= diff)
				{
					diff = temp;
					target = transactionItemControl;
				}
				else
				{
					break;
				}
				//if (utc > transactionItemControl.TransactionItem.UTC)
				//{
				//    target = transactionItemControl;
				//    continue;
				//}

				//if(utc < transactionItemControl.TransactionItem.UTC)
				//    break;
			}

			if (target != null)
			{
				_ignore = true;
				transactionListBox.SelectedItem = target;
				_ignore = false;
			}
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
			IsMinimize = true;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}

		public void Maximize()
		{
			IsMinimize = false;
			if (OnMinimizeChange != null)
				OnMinimizeChange(this, null);
		}
	}

	public class HightLightRange
	{
		public float Start;
		public float End;
	}

	public class TransactionItemControl
	{
		//public String Content;
		//public DateTime DateTime;
		public Boolean DisplayDateTime = true;
		public Boolean DisplayPOS;
		public IPOS POS;

		private String _dateTimeStr;
		private POS_Exception.TransactionItem _transactionItem;
		public POS_Exception.TransactionItem TransactionItem
		{
			get
			{
				return _transactionItem;
			}
			set {
				_transactionItem = value;
				_dateTimeStr = _transactionItem.DateTime.ToString(" HH:mm:ss   ");//yyyy-MM-dd 
			}
		}

		//---HeightLight
		public Boolean HeightVerified;
		public Boolean NeedHeight;
		public List<HightLightRange> HighLightRange = new List<HightLightRange>();

		public override string ToString()
		{
			var header = "";
			if(DisplayPOS && POS != null)
				header = POS.ToString().PadRight(21, ' ') + " -";

			if (DisplayDateTime)
				header += _dateTimeStr;

			return header + _transactionItem.Content;
		}
	}

    public class TransactionItemControl2
    {
        //public String Content;
        //public DateTime DateTime;
        public Boolean DisplayDateTime = true;
        public Boolean DisplayPOS;
        public IPOS POS;

        private String _dateTimeStr;
        private POS_Exception.TransactionItem _transactionItem;
        public POS_Exception.TransactionItem TransactionItem
        {
            get
            {
                return _transactionItem;
            }
            set
            {
                _transactionItem = value;
                _dateTimeStr = _transactionItem.DateTime.ToString(" HH:mm:ss   ");//yyyy-MM-dd 
            }
        }

        //---HeightLight
        public Boolean HeightVerified;
        public Boolean NeedHeight;
        public List<HightLightRange> HighLightRange = new List<HightLightRange>();

        public override string ToString()
        {
            var header = "";
            if (DisplayPOS && POS != null)
                header = POS.ToString().PadRight(21, ' ') + " -";

            if (DisplayDateTime)
                header += _dateTimeStr;

            return header + _transactionItem.Content.Replace("\r", "\\r").Replace("\n", "\\n");
        }

        public string ToFileString()
        {
            var header = "";
            if (DisplayPOS && POS != null)
                header = POS.ToString().PadRight(21, ' ') + " -";

            if (DisplayDateTime)
                header += _dateTimeStr;

            return header + _transactionItem.Content;
        }
    }
}
