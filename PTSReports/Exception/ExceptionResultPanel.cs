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

namespace PTSReports.Exception
{
	public class ExceptionResultPanel : Panel
	{
		public event EventHandler OnPlayback;

		public IApp App;
		public IPTS PTS;
		public SearchPanel SearchPanel;

		private POS_Exception.ExceptionDetail _exceptionDetail;
		public POS_Exception.ExceptionDetail ExceptionDetail
		{
			get { return _exceptionDetail; }
			set
			{
				_exceptionDetail = value;
				if (value != null)
				{
					_transactionDetail.Visible = true;

					_timecode = DateTimes.ToUtc(value.DateTime, PTS.Server.TimeZone);
					_datetime = value.DateTime.ToString("MM-dd-yyyy HH:mm:ss");
					_pos = null;
					_camera = null;
					var pos = PTS.POS.FindPOSById(value.POSId);
					if (pos != null)
					{
						_pos = pos;
						if (_pos.Items.Count > 0)
						{
                            //_camera = _pos.Items.First() as ICamera;
                            foreach (IDevice device in _pos.Items)
						    {
                                if (device as ICamera != null)
						        {
                                    _camera = device as ICamera;
						            break;
						        }
						    }
						}
							
					}
					//else
					//{
					//    _exceptionDetail = null;
					//}
				}
			}
		}

		private String _datetime = "";
		private IPOS _pos;
		private ICamera _camera;
		private UInt64 _timecode;
		public Dictionary<String, String> Localization;
		public Int32 Id;

		public ICamera Camera
		{
			get { return _camera; }
		}

		private readonly PictureBox _snapshot;
		private readonly TransactionDetail _transactionDetail;

		public Boolean IsTitle;

		public ExceptionResultPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"ExceptionResultPanel_ID", "ID"},
                                   {"ExceptionResultPanel_NO", "No."},
								   {"ExceptionResultPanel_POS", "POS"},
								   {"ExceptionResultPanel_DateTime", "DateTime"},
								   {"ExceptionResultPanel_Exception", "Exception"},
								   {"ExceptionResultPanel_Cashier", "Cashier"},
								   {"ExceptionResultPanel_CashierId", "Cashier Id"},
								   {"ExceptionResultPanel_ExceptionAmount", "Exception Amount"},
								   {"ExceptionResultPanel_TransactionAmount", "Transaction Amount"},
								   {"ExceptionResultPanel_POSDeleted", "(POS Deleted)"},
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

		private Boolean TryNextCameraForSnapshotIfAvailable()
		{
			//try next camera if available 
			if (_pos.Items.Count > 0)
			{
				var index = _pos.Items.IndexOf(_camera);
				if (index != -1 && index + 1 < _pos.Items.Count)
				{
					_camera = _pos.Items[index + 1] as ICamera;
					SearchPanel.QueueLoadSnapshot(this);
					return true;
				}
			}

			return false;
		}

		private static readonly Image _noImage = Resources.GetResources(Properties.Resources.no_image, Properties.Resources.IMGNo_image);
		private static readonly Image _snapshotBg = Resources.GetResources(Properties.Resources.image, Properties.Resources.IMGImage);
		public void LoadSnapshot()
		{
			if (_camera == null || _timecode == 0)
			{
				_snapshot.Image = _noImage;
				_snapshot.Tag = "";
				return;
			}

			_isReset = false;
			IsLoadingImage = true;

			var bitmap = _camera.GetSnapshot(_timecode, new Size(320, 240)) as Bitmap;
			//var bitmap = _camera.GetSnapshot(new Size(320, 240)) as Bitmap;
			UInt32 retry = 1;
			while (bitmap == null && retry > 0 && !_isReset)
			{
				Application.RaiseIdle(null);
				retry--;
				bitmap = _camera.GetSnapshot(_timecode, new Size(320, 240)) as Bitmap;
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
				_snapshot.Tag = bitmap.Tag;
			}
		}

		private IViewer _viewer;
		public void LoadSalientSnapshot()
		{
			if (_camera == null || _timecode == 0)
			{
				_snapshot.Image = _noImage;
				_snapshot.Tag = "";
				return;
			}

			_isReset = false;
			IsLoadingImage = true;

			_viewer = App.RegistViewer();
			_viewer.Camera = _camera;
			_viewer.Connect();
			_viewer.OnConnect += SalientViewerOnConnect;
		}

		private System.Timers.Timer _timer;
		private void SalientViewerOnConnect(Object sender, EventArgs<int> e)
		{
			IsLoadingImage = false;

			_viewer.OnConnect -= SalientViewerOnConnect;

			//connect failure
			if (e.Value == 0)
			{
				App.UnregistViewer(_viewer);
				_viewer = null;

				if(TryNextCameraForSnapshotIfAvailable())
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
			_timer.Enabled = true;
		}

		private void GetSalientSnapshot(Object sender, EventArgs e)
		{
			_timer.Enabled = false;
			if (_viewer == null)
			{
				_snapshot.Image = _noImage;
				return;
			}

			//it not goto current timecode, diff > 3 secs
			var timecode = _viewer.Timecode;
			var diff = (timecode > _timecode)
						   ? timecode - _timecode
						   : _timecode - timecode;

			Image image = null;
			if (diff < 3000)
			{
				Clipboard.Clear();
				_viewer.Snapshot("", false);
				image = Clipboard.GetImage();
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

		public void Reset()
		{
			_pos = null;
			_camera = null;
			_exceptionDetail = null;
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
		private static RectangleF _exceptionRectangleF = new RectangleF(650, 13, 145, 17);
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
				
				if (Width <= 510) return;
                g.DrawString(Localization["ExceptionResultPanel_CashierId"], Manager.Font, Manager.TitleTextColor, 405, 13);
				
				if (Width <= 610) return;
                g.DrawString(Localization["ExceptionResultPanel_Cashier"], Manager.Font, Manager.TitleTextColor, 500, 13);
				
				if (Width <= 800) return;
                g.DrawString(Localization["ExceptionResultPanel_Exception"], Manager.Font, Manager.TitleTextColor, 650, 13);

				if (Width <= 920) return;
                g.DrawString(Localization["ExceptionResultPanel_ExceptionAmount"], Manager.Font, Manager.TitleTextColor, 800, 13);

				if (Width <= 1005) return;
                g.DrawString(Localization["ExceptionResultPanel_TransactionAmount"], Manager.Font, Manager.TitleTextColor, 920, 13);
				return;
			}
            Manager.Paint(g, this);

			if (_exceptionDetail == null) return;

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

			if (Width <= 510) return;
			g.DrawString(_exceptionDetail.CashierId, Manager.Font, Brushes.Black, 405, 13);

			if (Width <= 610) return;
			g.DrawString(_exceptionDetail.Cashier, Manager.Font, Brushes.Black, 500, 13);

			if (Width <= 800) return;
			g.DrawString(_exceptionDetail.Type, Manager.Font, Brushes.Black, _exceptionRectangleF);
			
			if (Width <= 920) return;
			if (!String.IsNullOrEmpty(_exceptionDetail.ExceptionAmount))
				g.DrawString(@"$" + _exceptionDetail.ExceptionAmount, Manager.Font, Brushes.Black, 800, 13);

			if (Width <= 1005) return;
			if (!String.IsNullOrEmpty(_exceptionDetail.TotalTransactionAmount))
				g.DrawString(@"$" + _exceptionDetail.TotalTransactionAmount, Manager.Font, Brushes.Black, 920, 13);
		}

		private Boolean _isExpand;
		private Boolean _isLoading;
		private void ExceptionResultPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (IsTitle || ExceptionDetail == null) return;

			_isExpand = !_isExpand;
			if (_isExpand)
			{
				Height = 300;
				Cursor = Cursors.PanNorth;
				if (!IsLoad)
				{
					IsLoad = true;
					if (_camera != null)
						SearchPanel.QueueLoadSnapshot(this);
					else
					{
						IsLoadingImage = false;
						_snapshot.Image = _noImage;
					}
					LoadTransaction();
					_transactionDetail.Focus();
					Invalidate();
				}
			}
			else
			{
				Height = 40;
				Cursor = Cursors.PanSouth;
				Invalidate();

				//refresh scroll bar
				var panel = Parent as Panel;
				if (panel != null)
				{
					panel.Visible = false;
					//var items = new List<ExceptionResultPanel>();
					//foreach (ExceptionResultPanel control in panel.Controls)
					//{
					//    items.Add(control);
					//}

					var scrollPosition = panel.AutoScrollPosition;
					scrollPosition.Y *= -1;
					panel.AutoScroll = false;
					//panel.Controls.Clear();
					panel.AutoScroll = true;

					//foreach (var exceptionResultPanel in items)
					//{
					//    panel.Controls.Add(exceptionResultPanel);
					//}
					panel.AutoScrollPosition = scrollPosition;
					panel.Visible = true;
					panel.Invalidate();
					//panel.Height++;
					//panel.Invalidate();
				}
			}
		}

		private void LoadTransaction()
		{
			if (_exceptionDetail == null) return;

			_isLoading = true;
			if (_transactionDetail.Server == null)
				_transactionDetail.Server = PTS;
			_transactionDetail.OnLoadCompleted += TtransactionDetailOnLoadCompleted;
			_transactionDetail.UpdateSearchResult(new[] { _exceptionDetail.Type }, _exceptionDetail.TransactionId);
		}

		private void TtransactionDetailOnLoadCompleted(Object sender, EventArgs e)
		{
			_transactionDetail.OnLoadCompleted -= TtransactionDetailOnLoadCompleted;
			_isLoading = false;
		}

		private void SnapshotMouseClick(Object sender, MouseEventArgs e)
		{
			if (!_isLoading && OnPlayback != null)
				OnPlayback(this, null);
		}
	}
}
