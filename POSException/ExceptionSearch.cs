using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using PanelBase;

namespace POSException
{
    public sealed partial class ExceptionSearch : UserControl, IControl, IAppUse, IServerUse, IMinimize, IBlockPanelUse
	{
		public event EventHandler OnMinimizeChange;
        public event EventHandler<EventArgs<Boolean>> OnMaximizeChange;
		public event EventHandler OnSearchStart;
		public event EventHandler<EventArgs<POS_Exception.TransactionList, String[]>> OnSearchResult;

        public event EventHandler<EventArgs<Int32, String>> OnPageChange;
        public event EventHandler<EventArgs<String>> OnTimecodeChange;
        public event EventHandler<EventArgs<String[], String, POS_Exception.AdvancedSearchCriteria>> OnTransactionSelectionChange;
        public event EventHandler<EventArgs<Object>> OnSelectionChange;

		public Dictionary<String, String> Localization;
		public IApp App { get; set; }
		public String TitleName { get; set; }
        public SearchResult SearchResult;
		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon_activate, Properties.Resources.IMGIconActivate);
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
        protected readonly PanelTitleBarUI2 PanelTitleBarUI2 = new PanelTitleBarUI2();
		public UInt16 MinimizeHeight
		{
			get { return (UInt16)titlePanel.Size.Height; }
		}
		public Boolean IsMinimize { get; private set; }

		public ExceptionSearch()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_POSExceptionSearch", "Exception Search"},
								   {"Control_POS", "POS"},

								   {"POSException_Summary", "Summary"},
								   
								   {"POSException_Start", "Start"},
								   {"POSException_End", "End"},
								   {"POSException_Search", "Search"},
								   {"POSException_Searching", "Searching"},
								   {"POSException_ExceptionAndKeyword", "Exception / Keyword"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			Dock = DockStyle.Fill;

			posLabel.Text = Localization["Control_POS"];
			startLabel.Text = Localization["POSException_Start"];
			endLabel.Text = Localization["POSException_End"];
			exceptionAndKeywordLabel.Text = Localization["POSException_ExceptionAndKeyword"];

			searchButton.Text = Localization["POSException_Search"];
			//---------------------------
			Icon = new ControlIconButton { Image = _icon };
			Icon.Click += DockIconClick;
			//---------------------------

		}

		public void Initialize()
		{
            if (Parent is IControlPanel)
                BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

            PanelTitleBarUI2.Text = TitleName = Localization["Control_POSExceptionSearch"];
            titlePanel.Controls.Add(PanelTitleBarUI2);

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

			posComboBox.SelectedIndexChanged += POSComboBoxSelectedIndexChanged;
			if (_pts != null)
			{
				_pts.OnPOSModify += POSModify;

				var today = _pts.Server.DateTime;
				startDatePicker.Value = new DateTime(today.Year, today.Month, today.Day);
				startTimePicker.Value = new DateTime(today.Year, today.Month, today.Day);

				endDatePicker.Value = new DateTime(today.Year, today.Month, today.Day);
				endTimePicker.Value = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59, 999);
			}

			App.OnSwitchPage += ApplySearchCriteria;

            SearchResult = new SearchResult { Server = Server};
            SearchResult.Initialize();
            panelResult.Controls.Add(SearchResult);

            App.OnSwitchPage += SearchResult.ImportExceptionList;

            SearchResult.OnSelectionChange += SearchResultOnSelectionChange;
            SearchResult.OnPageChange += SearchResultOnPageChange;
            SearchResult.OnTimecodeChange += SearchResultOnTimecodeChange;
            SearchResult.OnTransactionSelectionChange += SearchResultOnTransactionSelectionChange;
		}

        public void ClearAll(Object sender, EventArgs e)
        {
            SearchResult.ClearAll();
        }

        private void SearchResultOnTransactionSelectionChange(object sender, EventArgs<string[], string, POS_Exception.AdvancedSearchCriteria> e)
        {
            if (OnTransactionSelectionChange != null)
                OnTransactionSelectionChange(this, e);
        }

        private void SearchResultOnTimecodeChange(object sender, EventArgs<string> e)
        {
            if (OnTimecodeChange != null)
                OnTimecodeChange(this, e);
        }

        private void SearchResultOnPageChange(object sender, EventArgs<int, string> e)
        {
            SearchByCondition(sender, e);
        }

        private void SearchResultOnSelectionChange(object sender, EventArgs<object> e)
        {
            if (OnSelectionChange != null)
                OnSelectionChange(this, e);
        }

		public void Activate()
		{
			if (_reloadTree)
			{
				if (_pts != null)
					UpdatePOSList();
			}

			_reloadTree = false;
		}

		private Boolean _searchFromAdvanced;
		public void ApplySearchCriteria(Object sender, EventArgs<String, Object> e)
		{
			if (!String.Equals(e.Value1, "Playback")) return;
			var transactionListParameter = e.Value2 as TransactionListParameter;
			if (transactionListParameter == null) return;

			_searchFromAdvanced = true;
			ApplyTransactionListParameter(transactionListParameter);
			Maximize();
		}

		private void ApplyTransactionListParameter(TransactionListParameter parameter)
		{
			if (parameter == null) return;
			if (parameter.POS != null && parameter.POS.Count > 0)
			{
				var allPos = true;
				foreach (IPOS obj in _pts.POS.POSServer)
				{
					if (!parameter.POS.Contains(obj.Id))
					{
						allPos = false;
						break;
					}
				}

				if (allPos)
				{
					posComboBox.SelectedIndex = 0;
				}
				else
				{
					var pos = parameter.POS.First();
					foreach (var item in posComboBox.Items)
					{
						if (item.ToString() == Localization["POSException_Summary"]) continue;

						String id = item.ToString().Split(' ')[0];
						if (id == pos)
						{
							posComboBox.SelectedItem = item;
							break;
						}
					}
				}
			}

			var start = DateTimes.ToDateTime(parameter.StartDateTime, _pts.Server.TimeZone);
			startDatePicker.Value = startTimePicker.Value = start;

			var end = DateTimes.ToDateTime(parameter.EndDateTime, _pts.Server.TimeZone);
			endDatePicker.Value = endTimePicker.Value = end;

			if (parameter.Exceptions != null)
				searchComboBox.Text = String.Join("+", parameter.Exceptions.ToArray());
			else
				searchComboBox.Text = "";
		}

		public void UpdatePOSList()
		{
			posComboBox.Items.Clear();

			if (_pts.POS.POSServer.Count > 0)
			{
				posComboBox.Items.Add(Localization["POSException_Summary"]);
				searchButton.Enabled = posComboBox.Enabled = true;

				var sortResult = new List<IPOS>(_pts.POS.POSServer);
                sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));

				foreach (var pos in sortResult)
				{
					posComboBox.Items.Add(pos.ToString());
				}

				posComboBox.SelectedIndex = 0;
			}
			else
			{
				searchButton.Enabled = posComboBox.Enabled = false;
			}
		}

		private List<String> _defaultExceptions;
		private void POSComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			var value = searchComboBox.Text;
			searchComboBox.Items.Clear();

			var exceptions = new List<String>();
			
			if (posComboBox.SelectedIndex != 0)
			{
				String id = posComboBox.SelectedItem.ToString().Split(' ')[0];
				var exceptionId = _pts.POS.FindPOSById(id).Exception;

				if (!_pts.POS.Exceptions.ContainsKey(exceptionId)) return;
				var target = _pts.POS.Exceptions[exceptionId];
                foreach (var obj in target.Exceptions.OrderBy(x => x.Value))
				{
					exceptions.Add(obj.Key);
				}
				//exceptions.Sort((x, y) => (x.CompareTo(y)));
			}
			else
			{
				if (_defaultExceptions == null)
				{
                    var defaultExceptions = new List<POS_Exception.Exception>();
					_defaultExceptions = new List<String>();
					foreach (var manufacture in POS_Exception.Manufactures)
					{
						var newException = new POS_Exception { Manufacture = manufacture };
						POS_Exception.SetDefaultExceptions(newException);

						foreach (var obj in newException.Exceptions)
						{
						    var add = defaultExceptions.All(defaultException => defaultException.Key != obj.Key);

						    if (add)
                                defaultExceptions.Add(obj);

                            //if (!defaultExceptions.Contains(obj))
                            //    defaultExceptions.Add(obj);

                            //if (!_defaultExceptions.Contains(obj.Key))
                            //    _defaultExceptions.Add(obj.Key);
                        }
					}

                    foreach (var obj in defaultExceptions.OrderBy(x => x.Value))
                    {
                        _defaultExceptions.Add(obj.Key);
                    }

					//_defaultExceptions.Sort((x, y) => (x.CompareTo(y)));
				}
				exceptions = _defaultExceptions;
			}

			foreach (var exception in exceptions)
			{
                searchComboBox.Items.Add(new ComboxItem(POS_Exception.FindExceptionValueByKey(exception), exception));
			}

			if (String.IsNullOrEmpty(value))
				searchComboBox.SelectedIndex = -1;
			else
				searchComboBox.Text = value;
		}

		private Boolean _reloadTree = true;
		public void POSModify(Object sender, EventArgs<IPOS> e)
		{
			_reloadTree = true;
		}

		public void Deactivate()
		{
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

		public UInt16 ResultPerPage = 20;
		public Int32 PageIndex = 1;
		private void SearchButtonMouseClick(Object sender, MouseEventArgs e)
		{
			String id = "0";
			if (posComboBox.SelectedIndex != 0)
			{
				id = posComboBox.SelectedItem.ToString().Split(' ')[0];
				//if (double.IsNaN(id)) return;
			}

			var startDate = new DateTime(startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day,
				startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second, 0);

			var endDate = new DateTime(endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day,
				endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second, 999);

			UInt64 startUtc;
			UInt64 endUtc;

			if (endDate >= startDate)
			{
				startUtc = DateTimes.ToUtc(startDate, Server.Server.TimeZone);
				endUtc = DateTimes.ToUtc(endDate, Server.Server.TimeZone);
			}
			else
			{
				endUtc = DateTimes.ToUtc(startDate, Server.Server.TimeZone);
				startUtc = DateTimes.ToUtc(endDate, Server.Server.TimeZone);
			}

			searchButton.Text = Localization["POSException_Searching"];
			searchButton.Enabled = false;

            //if (OnSearchStart != null)
            //    OnSearchStart(this, null);
            SearchResult.ClearSearchResult(sender, null);

			var keywordList = new List<String>();
		    var comboboxItem = searchComboBox.SelectedItem as ComboxItem;
            var temp = searchComboBox.Text.Split('+');
			foreach (var str in temp)
			{
				if (String.IsNullOrEmpty(str)) continue;
				keywordList.Add(str);
			}

			if (keywordList.Count == 0 && String.Equals(searchComboBox.Text, "+"))
				keywordList.Add("+");

			_keywords = keywordList.ToArray();

			_searchFromAdvanced = false;

		    var searchText = comboboxItem == null ? searchComboBox.Text : comboboxItem.Value;
			ReadTransactionHeadByConditionDelegate loadReportDelegate = _pts.POS.ReadTransactionHeadByCondition;
            loadReportDelegate.BeginInvoke(id, startUtc, endUtc, searchComboBox.Text, PageIndex, ResultPerPage, LoadReportCallback, loadReportDelegate);
		}

		private String[] _keywords;
		private delegate POS_Exception.TransactionList ReadTransactionHeadByConditionDelegate(String posId, UInt64 startutc, UInt64 endutc, String searchText, Int32 pageIndex, UInt16 count);
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
				}
				return;
			}
			searchButton.Text = Localization["POSException_Search"];
			searchButton.Enabled = true;

			var report = ((ReadTransactionHeadByConditionDelegate)result.AsyncState).EndInvoke(result);

            //if (OnSearchResult != null)
            //    OnSearchResult(this, new EventArgs<POS_Exception.TransactionList, String[]>(report, _keywords));

            SearchResult.UpdateSearchResult(this, new EventArgs<POS_Exception.TransactionList, String[]>(report, _keywords));
		}

		private Boolean _loading;
		private XmlDocument _conditionXml;
		public void SearchByCondition(Object sender, EventArgs<Int32, String> e)
		{
			searchButton.Text = Localization["POSException_Searching"] + @"...";
			searchButton.Enabled = false;

			_loading = true;
			//ital's from advanced search or here?'
			if(_searchFromAdvanced)
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(e.Value2);

				var pageNode = Xml.GetFirstElementByTagName(xmlDoc, "Page");
				if (pageNode != null)
					pageNode.SetAttribute("index", e.Value1.ToString());
				_conditionXml = xmlDoc;

				ReadTransactionByConditionByPageConditionDelegate loadReportDelegate2 = _pts.POS.ReadTransactionByCondition;
				loadReportDelegate2.BeginInvoke(xmlDoc, LoadReportPageCallback2, loadReportDelegate2);
			}
			else
			{
				ReadTransactionHeadByConditionByPageConditionDelegate loadReportDelegate = _pts.POS.ReadTransactionHeadByCondition;
				loadReportDelegate.BeginInvoke(e.Value1, e.Value2, LoadReportPageCallback, loadReportDelegate);
			}
		}

		private delegate XmlDocument ReadTransactionByConditionByPageConditionDelegate(XmlDocument conditionXml);
		private delegate void LoadReportPageCallback2Delegate(IAsyncResult result);
		private void LoadReportPageCallback2(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				Invoke(new LoadReportPageCallback2Delegate(LoadReportPageCallback2), result);
				return;
			}
			searchButton.Text = Localization["POSException_Search"];
			searchButton.Enabled = true;

			var reportXml = ((ReadTransactionByConditionByPageConditionDelegate)result.AsyncState).EndInvoke(result);
			var list = Report.ReadTransactionByCondition.Parse(reportXml, _conditionXml, _pts.Server.TimeZone);

            //if (OnSearchResult != null)
            //    OnSearchResult(this, new EventArgs<POS_Exception.TransactionList, String[]>(list, _keywords));
            SearchResult.UpdateSearchResult(this, new EventArgs<POS_Exception.TransactionList, String[]>(list, _keywords));
		}


		private delegate POS_Exception.TransactionList ReadTransactionHeadByConditionByPageConditionDelegate(Int32 pageIndex, String condition);
		private delegate void LoadReportPageCallbackDelegate(IAsyncResult result);
		private void LoadReportPageCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				Invoke(new LoadReportPageCallbackDelegate(LoadReportPageCallback), result);
				return;
			}
			searchButton.Text = Localization["POSException_Search"];
			searchButton.Enabled = true;

			var report = ((ReadTransactionHeadByConditionByPageConditionDelegate)result.AsyncState).EndInvoke(result);

            //if (OnSearchResult != null)
            //    OnSearchResult(this, new EventArgs<POS_Exception.TransactionList, String[]>(report, _keywords));
            SearchResult.UpdateSearchResult(this, new EventArgs<POS_Exception.TransactionList, String[]>(report, _keywords));
		}
	}
}
