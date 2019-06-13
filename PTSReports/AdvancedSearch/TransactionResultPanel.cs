using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using POSException;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed class TransactionResultPanel : Panel
	{
		public event EventHandler OnPlayback;

		public IPTS PTS;
		public SearchPanel SearchPanel;

		private POS_Exception.Transaction _transaction;
		public POS_Exception.Transaction Transaction
		{
			get { return _transaction; }
			set
			{
				_transaction = value;
				if (value != null)
				{
					_transactionDetail.Visible = true;

					_timecode = DateTimes.ToUtc(value.DateTime, PTS.Server.TimeZone);
					_datetime = value.DateTime.ToString("MM-dd-yyyy HH:mm:ss");
					_pos = null;
					Camera = null;
					var pos = PTS.POS.FindPOSById(value.POSId);
					if (pos != null)
					{
						_pos = pos;
						if (_pos.Items.Count > 0)
						{
                            //Camera = _pos.Items.First() as ICamera;
                            foreach (IDevice device in _pos.Items)
                            {
                                if (device as ICamera != null)
                                {
                                    Camera = device as ICamera;
                                    break;
                                }
                            }
						}
							
					}
					//else
					//{
					//    _transaction = null;
					//}
				}
			}
		}

		private String _datetime = "";
		private IPOS _pos;
		private UInt64 _timecode;
		public Dictionary<String, String> Localization;
		public IApp App;
		public Int32 Id;

		public ICamera Camera { get; private set; }

		private readonly PictureBox _snapshot;
		private readonly TransactionDetail _transactionDetail;

		public Boolean IsTitle;

		public TransactionResultPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"ExceptionResultPanel_ID", "ID"},
                                   {"ExceptionResultPanel_NO", "No."},
								   {"ExceptionResultPanel_POS", "POS"},
								   {"ExceptionResultPanel_DateTime", "Date / Time"},
								   {"ExceptionResultPanel_Cashier", "Cashier"},
								   {"ExceptionResultPanel_CashierId", "Cashier Id"},
								   {"ExceptionResultPanel_TotalExceptionAmount", "Total Exception Amount"},
								   {"ExceptionResultPanel_Total", "Total"},
								   {"ExceptionResultPanel_POSDeleted", "(POS Deleted)"},
                                   {"ExceptionResultPanel_CountingDiscrepancies", "Counting Discrepancies"},
                                   {"ExceptionResultPanel_CountingDiscrepanciesCompare", "%1 (POS: %2, Counting: %3)"},
							   };
			Localizations.Update(Localization);

			DoubleBuffered = true;
			Dock = DockStyle.Top;
			Cursor = Cursors.PanSouth;// Cursors.Hand;
			Size = new Size(1035, 40);

			_snapshot = new PictureBox
			{
				Location = new Point(14, 43),
				BackgroundImageLayout = ImageLayout.Stretch,
				SizeMode = PictureBoxSizeMode.CenterImage,
				Size = new Size(320, 234),
				Image = _snapshotBg,
				Cursor = Cursors.Hand,
			};

			_transactionDetail = new TransactionDetail
			{
				Dock = DockStyle.None,
				Location = new Point(340, 43),
				Size = new Size(715, 234),
				FontColor = Brushes.Black,
				BackgroundColor = Color.White,
				DisplayDateTime = false,
				BorderStyle = BorderStyle.FixedSingle,
				//MinimumSize = new Size(240, 240),
				//Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
			};
			_transactionDetail.RemoveTitlePanel();

			Paint += ExceptionResultPanelPaint;
			_snapshot.Paint += SnapshotPaint;

			_snapshot.MouseClick += SnapshotMouseClick;

			Controls.Add(_snapshot);
			Controls.Add(_transactionDetail);

			MouseClick += ExceptionResultPanelMouseClick;
		}

		private Boolean _isReset;
		public Boolean IsLoadingImage;
		public Boolean IsLoad;

		private static readonly Image _noImage = Resources.GetResources(Properties.Resources.no_image, Properties.Resources.IMGNo_image);
		private static readonly Image _snapshotBg = Resources.GetResources(Properties.Resources.image, Properties.Resources.IMGImage);
		public void LoadSnapshot()
		{
			if (Camera == null || _timecode == 0)
			{
				_snapshot.Image = _noImage;
				_snapshot.Tag = "";
				return;
			}

			_isReset = false;
			IsLoadingImage = true;

			var bitmap = Camera.GetSnapshot(_timecode, new Size(320, 240)) as Bitmap;

			//var bitmap = _camera.GetSnapshot(new Size(320, 240)) as Bitmap;
			UInt32 retry = 1;
			while (bitmap == null && retry > 0 && !_isReset)
			{
				Application.RaiseIdle(null);
				retry--;
				bitmap = Camera.GetSnapshot(_timecode, new Size(320, 240)) as Bitmap;
				//bitmap = _camera.GetSnapshot(new Size(320, 240)) as Bitmap;
			}

			IsLoadingImage = false;
			if (_isReset)
				return;

			if (bitmap == null)
			{
				if (TryNextCameraForSnapshotIfAvailable())
					return;

				_snapshot.Image = _noImage;
				_snapshot.Tag = "";
			}
			else
			{
				_snapshot.Image = null;
				_snapshot.BackgroundImage = bitmap;
				_snapshot.Tag = bitmap.Tag; //url
			}
		}

		private IViewer _viewer;
		public void LoadSalientSnapshot()
		{
			if (Camera == null || _timecode == 0)
			{
				_snapshot.Image = _noImage;
				_snapshot.Tag = "";
				return;
			}

			_isReset = false;
			IsLoadingImage = true;

			_viewer = App.RegistViewer();
			_viewer.Camera = Camera;
			_viewer.OnConnect += SalientViewerOnConnect;
			_viewer.Connect();
		}

		private System.Timers.Timer _timer;
		private void SalientViewerOnConnect(Object sender, EventArgs<Int32> e)
		{
			IsLoadingImage = false;

			_viewer.OnConnect -= SalientViewerOnConnect;

			//connect failure)
			if (e.Value == 0)
			{
				App.UnregistViewer(_viewer);
				_viewer = null;

				if (TryNextCameraForSnapshotIfAvailable())
					return;

				_snapshot.Image = _noImage;
				return;
			}

			_viewer.GoTo(_timecode);

			//wait 1sec to get snapshot, //salient's limitation
			if (_timer == null)
			{
				_timer = new System.Timers.Timer(1000);
				_timer.Elapsed += GetSalientSnapshot;
				_timer.SynchronizingObject = this;
			}
			_retrySnapshotTimes = 5;
			_timer.Enabled = true;
		}

		private UInt16 _retrySnapshotTimes = 5;
		private void GetSalientSnapshot(Object sender, EventArgs e)
		{
			try
			{
				_timer.Enabled = false;
				if (_viewer == null) return;

				//it not goto current timecode, diff > 3 secs
				var timecode = _viewer.Timecode;

				var diff = (timecode > _timecode)
							   ? timecode - _timecode
							   : _timecode - timecode;

				//MessageBox.Show(_timecode + " " + timecode + " " + diff);

				Image image = null;
				if (diff < 3000)
				{
					Clipboard.Clear();
					_viewer.Snapshot("", false);
					image = Clipboard.GetImage();
				}
				else if (_retrySnapshotTimes > 0)//retry 5 times wait for snapshot
				{
					_retrySnapshotTimes--;
					_timer.Enabled = true;
					return;
				}

				App.UnregistViewer(_viewer);
				_viewer = null;

				if (_isReset)
					return;

				_snapshot.Tag = "";
				if (image == null)
				{
					if (TryNextCameraForSnapshotIfAvailable())
						return;

					_snapshot.Image = _noImage;
				}
				else
				{
					_snapshot.Image = null;
					_snapshot.BackgroundImage = image;
				}
			}
			catch (System.Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		public void Reset()
		{
			_pos = null;
			Camera = null;
			_transaction = null;
			_transactionDetail.Visible = false;
			IsTitle = _isExpand = IsLoad = false;
			_snapshot.Image = _snapshotBg;
			_snapshot.BackgroundImage = null;
			Height = 40;
			Cursor = Cursors.PanSouth;

			if (_timer != null)
				_timer.Enabled = false;
			if (_viewer != null)
				App.UnregistViewer(_viewer);

			_isReset = true;
		}

		private readonly Point _pointA = new Point(145, 102);
		private readonly Point _pointB = new Point(145, 132);
		private readonly Point _pointC = new Point(175, 117);

		private readonly Point _point1 = new Point(125, 89);
		private readonly Point _point2 = new Point(195, 89);
		private readonly Point _point3 = new Point(200, 94);
		private readonly Point _point4 = new Point(200, 140);
		private readonly Point _point5 = new Point(195, 145);
		private readonly Point _point6 = new Point(125, 145);
		private readonly Point _point7 = new Point(120, 140);
		private readonly Point _point8 = new Point(120, 94);
		private void SnapshotPaint(Object sender, PaintEventArgs e)
		{
			if (_snapshot.BackgroundImage == null) return;

			Graphics g = e.Graphics;

			Point[] rectPoints = {
				_point1, 
				_point2, 
				_point3,
				_point4,
				_point5,
				_point6,
				_point7,
				_point8,
			};
			var brush = new SolidBrush(Color.FromArgb(170, 0, 0, 0));
			g.FillPolygon(brush, rectPoints, FillMode.Alternate);

			Point[] curvePoints = {
				_pointA, 
				_pointB, 
				_pointC
			};
			g.FillPolygon(new SolidBrush(Color.White), curvePoints, FillMode.Alternate);

			//var drawingPen = new Pen(Color.FromArgb(150, 0, 0, 0), 3);
			//g.DrawLine(drawingPen, _pointA, _pointB);
			//g.DrawLine(drawingPen, _pointA, _pointC);
			//g.DrawLine(drawingPen, _pointB, _pointC);  
		}

		private static RectangleF _nameRectangleF = new RectangleF(90, 13, 126, 17);
		private void ExceptionResultPanelPaint(Object sender, PaintEventArgs e)
		{
			if (Parent == null) return;

			Graphics g = e.Graphics;

			if (IsTitle)
			{
                Manager.PaintTitleTopInput(g, this);

				if (Width <= 200) return;
                Manager.PaintTitleText(g, Localization["ExceptionResultPanel_NO"]);
                g.DrawString(Localization["ExceptionResultPanel_POS"], Manager.Font, Manager.TitleTextColor, 90, 13);

				if (Width <= 400) return;
                g.DrawString(Localization["ExceptionResultPanel_DateTime"], Manager.Font, Manager.TitleTextColor, 230, 13);

				if (Width <= 500) return;
                g.DrawString(Localization["ExceptionResultPanel_CashierId"], Manager.Font, Manager.TitleTextColor, 405, 13);

				if (Width <= 600) return;
                g.DrawString(Localization["ExceptionResultPanel_Cashier"], Manager.Font, Manager.TitleTextColor, 500, 13);

				if (Width <= 735) return;
                g.DrawString(Localization["ExceptionResultPanel_TotalExceptionAmount"], Manager.Font, Manager.TitleTextColor, 650, 13);

				if (Width <= 885) return;
                g.DrawString(Localization["ExceptionResultPanel_Total"], Manager.Font, Manager.TitleTextColor, 800, 13);

                if (Width <= 955) return;
                if (SearchPanel.SearchCriteria.CountingCriteria.Enable)
                    g.DrawString(Localization["ExceptionResultPanel_CountingDiscrepancies"], Manager.Font, Manager.TitleTextColor, 900, 13);

				return;
			}

            Manager.Paint(g, this);

			if (_transaction == null) return;

			if (Height == 40)
				Manager.PaintExpand(g, this);
			else
				Manager.PaintCollapse(g, this);

			if (Width <= 200) return;

			Manager.PaintText(g, Id.ToString());
			if ((_pos == null))
				g.DrawString(Localization["ExceptionResultPanel_POSDeleted"], Manager.Font, Manager.DeleteTextColor, _nameRectangleF);
			else
				g.DrawString(_pos.ToString(), Manager.Font, Brushes.Black, _nameRectangleF);

			if (Width <= 400) return;
			g.DrawString(_datetime, Manager.Font, Brushes.Black, 230, 13);

			if (Width <= 500) return;
			g.DrawString(_transaction.CashierId, Manager.Font, Brushes.Black, 405, 13);

			if (Width <= 600) return;
			g.DrawString(_transaction.Cashier, Manager.Font, Brushes.Black, 500, 13);

			if (Width <= 730) return;
			if (!String.IsNullOrEmpty(_transaction.ExceptionAmount))
				g.DrawString(@"$" + _transaction.ExceptionAmount, Manager.Font, Brushes.Black, 650, 13);

			if (Width <= 880) return;
			if (!String.IsNullOrEmpty(_transaction.Total))
				g.DrawString(@"$" + _transaction.Total, Manager.Font, Brushes.Black, 800, 13);

            if (Width <= 930) return;
            if (SearchPanel.SearchCriteria.CountingCriteria.Enable)
                g.DrawString(Localization["ExceptionResultPanel_CountingDiscrepanciesCompare"].Replace("%1", (_transaction.ItemsCount - _transaction.Counting).ToString()).Replace("%2", _transaction.ItemsCount.ToString()).Replace("%3", _transaction.Counting.ToString()), Manager.Font, Brushes.Black, 900, 13);
		}

		private Boolean _isExpand;

		private void ExceptionResultPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (IsTitle || Transaction == null) return;

			_isExpand = !_isExpand;
			if (_isExpand)
			{
				Height = 300;
				Cursor = Cursors.PanNorth;
				if (!IsLoad)
				{
					IsLoad = true;
					SearchPanel.QueueLoadSnapshot(this);
					LoadTransaction();
					//public void UpdateSearchResult(String[] keywords, List<POS_Exception.TransactionItemDetail> transactionList)
				}
			}
			else
			{
				Height = 40;
				Cursor = Cursors.PanSouth;
			}

			Invalidate();
		}

		private Boolean TryNextCameraForSnapshotIfAvailable()
		{
			//try next camera if available 
			if (_pos.Items.Count > 0)
			{
				var index = _pos.Items.IndexOf(Camera);
				if (index != -1 && index + 1 < _pos.Items.Count)
				{
					Camera = _pos.Items[index + 1] as ICamera;
					SearchPanel.QueueLoadSnapshot(this);
					return true;
				}
			}

			return false;
		}

		private void LoadTransaction()
		{
			if (_transaction == null) return;

			if (_transactionDetail.Server == null)
				_transactionDetail.Server = PTS;
			var keyword = new List<String>();
			foreach (var cashierCriteria in SearchPanel.SearchCriteria.CashierCriterias)
			{
				if (keyword.Contains(cashierCriteria.Cashier)) continue;
				keyword.Add(cashierCriteria.Cashier);
			}
			foreach (var exceptionAmountCriteria in SearchPanel.SearchCriteria.ExceptionAmountCriterias)
			{
				if (keyword.Contains(exceptionAmountCriteria.Exception)) continue;
				keyword.Add(exceptionAmountCriteria.Exception);
			}
			//foreach (var exceptionCriteria in SearchPanel.SearchCriteria.ExceptionCriterias)
			//{
			//    if (keyword.Contains(exceptionCriteria.Exception)) continue;
			//    keyword.Add(exceptionCriteria.Exception);
			//}
			foreach (var tagCriteria in SearchPanel.SearchCriteria.TagCriterias)
			{
				if (keyword.Contains(tagCriteria.TagName)) continue;
				keyword.Add(tagCriteria.TagName);
			}
			foreach (var keywordCriteria in SearchPanel.SearchCriteria.KeywordCriterias)
			{
				if (keyword.Contains(keywordCriteria.Keyword)) continue;
				keyword.Add(keywordCriteria.Keyword);
			}

		    _transactionDetail.SearchCriteria = SearchPanel.SearchCriteria;
			_transactionDetail.UpdateSearchResult(keyword.ToArray(), _transaction.Id);
		}

		private void SnapshotMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPlayback != null)
				OnPlayback(this, null);
		}
	}
}
