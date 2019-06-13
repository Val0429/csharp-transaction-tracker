using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using PanelBase;

namespace POSException
{
    public sealed partial class SearchResult : UserControl, IControl, IAppUse, IServerUse, IMinimize, IBlockPanelUse
	{
		public event EventHandler OnMinimizeChange;
		public event EventHandler<EventArgs<Int32, String>> OnPageChange;
		public event EventHandler<EventArgs<String>> OnTimecodeChange;
        public event EventHandler<EventArgs<String[], String, POS_Exception.AdvancedSearchCriteria>> OnTransactionSelectionChange;
		public event EventHandler<EventArgs<Object>> OnSelectionChange;
		
		public Dictionary<String, String> Localization;
		public IApp App { get; set; }
		public String TitleName { get; set; }

		public Button Icon { get; private set; }
		private static readonly Image _icon = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
        private static readonly Image _iconActivate = Resources.GetResources(Properties.Resources.icon, Properties.Resources.IMGIcon);
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
            get { return 0; }
        }
		public Boolean IsMinimize { get; private set; }
		private readonly System.Timers.Timer _setCurrentPageTimer = new System.Timers.Timer();

		//private readonly Font _defaultFont = new Font("Lucida Console", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
		//private readonly Font _emptyFont = new Font("Bottom", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

		public SearchResult()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_POSExceptionSearchResult", "Search Result"},

								   {"Common_UsedSeconds", "(%1 seconds)"},
								   
								   {"PageSelector_Prev", "Previous Page"},
								   {"PageSelector_Next", "Next Page"},
								   {"PageSelector_First", "First Page"},
								   {"PageSelector_Last", "Last Page"},

								   {"POSException_NoTransactionFound", " : No Transaction Found"},
								   {"POSException_SearchTransactionFound", " : %1 Transaction"},
								   {"POSException_SearchTransactionsFound", " : %1 Transactions"},

								   {"POSException_PageQuantity", "%1 of %2 "},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			Dock = DockStyle.Fill;

			_setCurrentPageTimer.Elapsed += SetCurrentPage;
			_setCurrentPageTimer.Interval = 500;
			//---------------------------
			Icon = new ControlIconButton { Image = _icon };
			//Icon.Click += DockIconClick;
		    Icon.Visible = false;
		    //---------------------------
		}

		public Int32 PageIndex = 1;
		public Int32 Pages = 1;

		public void Initialize()
		{
            //if (Parent is IControlPanel)
            //    BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_setCurrentPageTimer.SynchronizingObject = Server.Form;
            PanelTitleBarUI2.Text = TitleName = Localization["Control_POSExceptionSearchResult"];
            titlePanel.Controls.Add(PanelTitleBarUI2);

			SharedToolTips.SharedToolTip.SetToolTip(Icon, TitleName);

			pageLabel.Text = "";

			resultListBox.SelectedIndexChanged += ResultListBoxSelectedIndexChanged;
            //resultListBox.Click += ResultListBoxSelectedIndexChanged;

			SharedToolTips.SharedToolTip.SetToolTip(firstButton, Localization["PageSelector_First"]);
			SharedToolTips.SharedToolTip.SetToolTip(lastButton, Localization["PageSelector_Last"]);
			SharedToolTips.SharedToolTip.SetToolTip(nextPageButton, Localization["PageSelector_Next"]);
			SharedToolTips.SharedToolTip.SetToolTip(previousPageButton, Localization["PageSelector_Prev"]);

			//App.OnSwitchPage += ImportExceptionList;
		}

		private String[] _keywords;
		public void ImportExceptionList(Object sender, EventArgs<String, Object> e)
		{
			if (!String.Equals(e.Value1, "Playback")) return;
			var transactionListParameter = e.Value2 as TransactionListParameter;
			if (transactionListParameter == null) return;

			if (transactionListParameter.Exceptions != null)
				_keywords = transactionListParameter.Exceptions.ToArray();
			else
				_keywords = new String[0];

			UpdateSearchResult(transactionListParameter.TransactionList, transactionListParameter.Transaction, transactionListParameter.SearchCriteria);
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		private POS_Exception.TransactionList _result;
		public void UpdateSearchResult(Object sender, EventArgs<POS_Exception.TransactionList, String[]> e)
		{
			if (e.Value2 != null)
				_keywords = (String[])e.Value2.Clone();
			else
				_keywords = null;

			UpdateSearchResult(e.Value1);
		}

		public void ClearSearchResult(Object sender, EventArgs  e)
		{
			pageLabel.Text = "";
            PanelTitleBarUI2.Text = TitleName;
			resultListBox.Items.Clear();
			firstButton.Visible = lastButton.Visible =
			nextPageButton.Visible = previousPageButton.Visible = false;
		}

        private POS_Exception.AdvancedSearchCriteria _searchCriteria;
        private void UpdateSearchResult(POS_Exception.TransactionList exceptionList, POS_Exception.Transaction detail = null, POS_Exception.AdvancedSearchCriteria searchCriteria = null)
		{
			//Maximize();
			resultListBox.Items.Clear();
            _searchCriteria = searchCriteria;

			resultListBox.Enabled = nextPageButton.Enabled = previousPageButton.Enabled = true;
			//noResultLabel.Visible = false;
			_result = exceptionList;

            PanelTitleBarUI2.Text = TitleName + @" (" + _result.Count + @")";
								  //@" " + Localization["Common_UsedSeconds"].Replace("%1", _result.Elapsed.TotalSeconds.ToString("0.00"));

			if (OnTransactionSelectionChange != null)
                OnTransactionSelectionChange(this, new EventArgs<String[], String, POS_Exception.AdvancedSearchCriteria>(null, null, searchCriteria));

			if (_result.Count == 0)
			{
				//resultListBox.Font = _emptyFont;
                PanelTitleBarUI2.Text = TitleName + Localization["POSException_NoTransactionFound"];
				pageLabel.Text = "";
				pagePanel.BackColor = Color.FromArgb(49, 50, 55);
				return;
			}

            PanelTitleBarUI2.Text = TitleName + (((_result.Count <= 1)
					? Localization["POSException_SearchTransactionFound"]
					: Localization["POSException_SearchTransactionsFound"]).Replace("%1", _result.Count.ToString()));

            pagePanel.BackColor = Color.FromArgb(49, 50, 55); //Color.White;
			//resultListBox.Font = _defaultFont; 
			PageIndex = _result.PageIndex;
			Pages = _result.Pages;
			pageLabel.Text = Localization["POSException_PageQuantity"].Replace("%1", PageIndex.ToString()).Replace("%2", Pages.ToString());

			nextPageButton.Visible = (PageIndex < Pages);
			previousPageButton.Visible = (PageIndex > 1);
			firstButton.Visible = (PageIndex > 2);
			lastButton.Visible = (PageIndex + 1 < Pages);
			//String date;
			TransactionControl selectItem = null;
			foreach (var result in _result.Results)
			{
				//if (date != result.DateTime.ToString("yyyy-MM-dd"))
				//{
				//    date = result.DateTime.ToString("yyyy-MM-dd");

				//    resultListBox.Items.Add(new TransactionControl
				//    {
				//        ExceptionDetail = new POS_Exception.ExceptionDetail
				//        {
				//            TransactionId = "@TITLE",
				//            DateTime = result.DateTime,
				//        }
				//    });
				//}
				
				var item = new TransactionControl
					{
						Transaction = result,
                        Server = Server
					};
				resultListBox.Items.Add(item);

				if (detail == result)
					selectItem = item;
			}
			
			if (selectItem != null)
				resultListBox.SelectedItem = selectItem;
		}

        public void ClearAll()
        {
            _previousSelectItem = null;
        }

		private TransactionControl _previousSelectItem;
		private void ResultListBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if(_previousSelectItem == resultListBox.SelectedItem)
				return;
			_previousSelectItem = resultListBox.SelectedItem as TransactionControl;

			if (_previousSelectItem == null)// || _previousSelectItem.ExceptionDetail.TransactionId == "@TITLE"
				return;

			DateTimeSelectionChange(_previousSelectItem.Transaction.DateTime);

			if(OnTransactionSelectionChange != null)
                OnTransactionSelectionChange(this, new EventArgs<String[], String, POS_Exception.AdvancedSearchCriteria>(_keywords, _previousSelectItem.Transaction.Id, _searchCriteria));

			IPOS pos = _pts.POS.FindPOSById(_previousSelectItem.Transaction.POSId);

			if (pos != null)
			{
				if (pos.Items.Count > 0)
				{
					if (OnSelectionChange != null)
						OnSelectionChange(this, new EventArgs<Object>(pos));
				}
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

		private void PreviousPageButtonMouseClick(Object sender, MouseEventArgs e)
		{
			PageIndex--;
			if (PageIndex < 1)
				PageIndex = 1;
			pageLabel.Text = Localization["POSException_PageQuantity"].Replace("%1", PageIndex.ToString()).Replace("%2", Pages.ToString());

			_setCurrentPageTimer.Enabled = false;
			_setCurrentPageTimer.Enabled = true;
			//SetCurrentPage();
		}

		private void NextPageButtonMouseClick(Object sender, MouseEventArgs e)
		{
			PageIndex++;
			if (PageIndex > Pages)
				PageIndex = Pages;
			pageLabel.Text = Localization["POSException_PageQuantity"].Replace("%1", PageIndex.ToString()).Replace("%2", Pages.ToString());

			_setCurrentPageTimer.Enabled = false;
			_setCurrentPageTimer.Enabled = true;
			//SetCurrentPage();
		}

		private void SetCurrentPage(Object sender, EventArgs e)
		{
			_setCurrentPageTimer.Enabled = false;
			nextPageButton.Enabled = previousPageButton.Enabled = false;
			SetCurrentPage();
		}

		private void FirstButtonMouseClick(Object sender, MouseEventArgs e)
		{
			PageIndex = 1;
			pageLabel.Text = Localization["POSException_PageQuantity"].Replace("%1", PageIndex.ToString()).Replace("%2", Pages.ToString());

			_setCurrentPageTimer.Enabled = false;
			_setCurrentPageTimer.Enabled = true;
		}

		private void LastButtonMouseClick(Object sender, MouseEventArgs e)
		{
			PageIndex = Pages;
			pageLabel.Text = Localization["POSException_PageQuantity"].Replace("%1", PageIndex.ToString()).Replace("%2", Pages.ToString());

			_setCurrentPageTimer.Enabled = false;
			_setCurrentPageTimer.Enabled = true;
		}

		private void SetCurrentPage()
		{
			if (PageIndex > Pages)
				PageIndex = 1;
			else if (PageIndex < 1)
				PageIndex = Pages;

			//pageLabel.Text = "";
			//nextPageButton.Visible = previousPageButton.Visible = false;
			//resultListBox.Items.Clear();
			//_panelTitleBar.Text = TitleName;

			resultListBox.Enabled = false;

			if(OnPageChange != null)
				OnPageChange(this, new EventArgs<Int32, String>(PageIndex, _result.SearchCondition));
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
                //BlockPanel.HideThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = true;
                    App.StartupOption.SaveSetting();
                }
            }

            //Icon.Image = _icon;
            //Icon.BackgroundImage = null;

            //Icon.Invalidate();

            IsMinimize = true;

            //Visible = false;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);

		}

		public void Maximize()
		{
            if (BlockPanel.LayoutManager.Page.Version == "2.0")
            {
                //BlockPanel.ShowThisControlPanel(this);

                if (!App.StartupOption.Loading)
                {
                    App.StartupOption.HidePanel = false;
                    App.StartupOption.SaveSetting();
                }
            }

            //Icon.Image = _iconActivate;
            //Icon.BackgroundImage = ControlIconButton.IconBgActivate;

            IsMinimize = false;

		    //Visible = true;
            if (OnMinimizeChange != null)
                OnMinimizeChange(this, null);
		}

        public void MaximizeChange(Object sender, EventArgs<Boolean> e)
        {
            if (e.Value)
                Maximize();
            else
                Minimize();
        }
	}

	public  class TransactionControl
	{
		//public String TransactionId;
		//public String Amount;
		//public String ExceptionAmount;
		//public DateTime DateTime;
	    public IServer Server;
		private String _dateTimeStr;
		private String _amount;
		private String _exceptionAmount;
		private POS_Exception.Transaction _transaction;
		public POS_Exception.Transaction Transaction
		{
			get
			{
				return _transaction;
			}
			set
			{
				_transaction = value;
				_dateTimeStr = _transaction.DateTime.ToString(" MM-dd-yyyy HH:mm:ss");
				//_amount = String.IsNullOrEmpty(_transaction.Amount) ? "" : "$" + _transaction.Amount;
				_exceptionAmount = String.IsNullOrEmpty(_transaction.ExceptionAmount) ? "" : "$" + _transaction.ExceptionAmount;
			}
		}


		//public TransactionControl()
		//{
		//    Paint += TransactionControlPaint;
		//}

		//private void TransactionControlPaint(object sender, PaintEventArgs e)
		//{
		//    Graphics g = e.Graphics;
		//    g.FillRectangle(new SolidBrush(Color.Red), 0, 0, 10, 10);
		//}

		public override string ToString()
		{
			//if (_exceptionDetail.TransactionId == "@TITLE")
			//    return "-----" + _exceptionDetail.DateTime.ToString("yyyy-MM-dd") + "-----";
            if (Server.Server.HideExceptionAmount)
                return _dateTimeStr;

			return _dateTimeStr + _exceptionAmount.PadLeft(13, ' ');
		}
	}
}
