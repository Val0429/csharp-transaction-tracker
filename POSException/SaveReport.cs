using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using PTSReportsGenerator;
using ApplicationForms = PanelBase.ApplicationForms;
namespace POSException
{
	public sealed partial class TransactionDetail
	{
		private const String CgiLoadNVR = @"cgi-bin/sysconfig?action=loadnvr";
		public void SaveReport(Object sender, EventArgs e)
		{
			//ApplicationForms.ShowLoadingIcon(_pts.Form);
			SaveTransactionReport();
			//SaveReportDelegate saveDelegate = SaveTransactionReport;
			//saveDelegate.BeginInvoke(SaveReportCallback, saveDelegate);
		}

		private readonly Size _defaultSnapshotSize = new Size(400, 340);
		private List<Image> _snapshots = new List<Image>();
		private XmlDocument _resultXml;
		private void SaveTransactionReport()
		{
			POS_Exception.TransactionItem transaction = null;
			if (_previousSelectItem == null)
			{
				if (transactionListBox.Items.Count > 0)
				{
					var control = transactionListBox.Items[0] as TransactionItemControl;
					if (control != null)
						transaction = control.TransactionItem;
				}
			}
			else
			{
				transaction = _previousSelectItem.TransactionItem;
			}

			if (transaction == null)
			{
				TopMostMessageBox.Show(Localization["POSException_SelectTransactionBeforeExport"], Localization["MessageBox_Information"],
								   MessageBoxButtons.OK, MessageBoxIcon.Information);

				return;
			}

			_resultXml = new XmlDocument();
			var xmlRoot = _resultXml.CreateElement("Result");
			_resultXml.AppendChild(xmlRoot);

			IPOS pos = _pts.POS.FindPOSById(transaction.POS);

			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "Server", Server.Credential.Domain));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "User", Server.User.Current.Credential.UserName));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "PrintTime", DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "Description", ""));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "StoreId", _pts.Configure.Store.Id));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "Store", _pts.Configure.Store.Name));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "POSId", (pos != null) ? pos.Id.ToString() : ""));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "POS", (pos != null) ? pos.Name : ""));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "CashierId", transaction.CashierId));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "Cashier", transaction.Cashier));
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "TransactionTime", transaction.TransactionTime));

			var transactions = "";
			foreach (var item in _transactionItemList.TransactionItems)
			{
				//item.DateTime.ToString(" yyyy-MM-dd HH:mm:ss   ");
				transactions += (item.Content + Environment.NewLine);
			}
			xmlRoot.AppendChild(Xml.CreateXmlElementWithText(_resultXml, "TransactionDetail", transactions));

			var snapshotRoot = _resultXml.CreateElement("Snapshots");
			xmlRoot.AppendChild(snapshotRoot);

			_queueSnapshotProperty.Clear();
			_snapshots.Clear();
			//<Snapshots>
			//    <Snapshot width="400" height="340" nvrId="1" channelId="1" timestamp="1360139673000" />
			//    <Snapshot width="400" height="340" nvrId="1" channelId="2" timestamp="1360139673000" />
			//</Snapshots>
			
			//var snapshots = new List<Image>();
			//Image snapshot = null;
			var waitSnapshot = false;

			if (pos != null && pos.Items.Count > 0)
			{
				foreach (IDevice device in pos.Items)
				{
					if (device == null || device.Server == null) continue;

					var timestamp = _timestamp;
					if (device.Server.Server.TimeZone != Server.Server.TimeZone)
					{
						Int64 time = Convert.ToInt64(timestamp);
						time += (Server.Server.TimeZone * 1000);
						time -= (device.Server.Server.TimeZone * 1000);
						timestamp = Convert.ToUInt64(time).ToString();
					}

					switch (device.Server.Manufacture)
					{
						case "Salient":
							waitSnapshot = true;
							LoadSalientSnapshot((ICamera)device, Convert.ToUInt64(timestamp), _defaultSnapshotSize.Width, _defaultSnapshotSize.Height);
							break;

						default:
							var snapshotNode = _resultXml.CreateElement("Snapshot");
							snapshotNode.SetAttribute("width", _defaultSnapshotSize.Width.ToString());
							snapshotNode.SetAttribute("height", _defaultSnapshotSize.Height.ToString());
							snapshotNode.SetAttribute("nvrId", device.Server.Id.ToString());
							snapshotNode.SetAttribute("channelId", device.Id.ToString());
							snapshotNode.SetAttribute("timestamp", timestamp);
							if (_pts.Configure.ImageWithTimestamp)
							{
								var camera = device as ICamera;
								if(camera != null && camera.Profile.StreamConfigs.ContainsKey(camera.Profile.RecordStreamId))
								{
									var config = camera.Profile.StreamConfigs[camera.Profile.RecordStreamId];
									var watermark = device + "  FPS:" + config.Framerate + "  " +
													Compressions.ToDisplayString(config.Compression) +
													"  " + Resolutions.ToString(config.Resolution) + "  " +
													DateTimes.ToDateTimeString(Convert.ToUInt64(timestamp), camera.Server.Server.TimeZone, "yyyy-MM-dd HH:mm:ss");
									snapshotNode.SetAttribute("watermark", watermark);
								}
								else
								{
								snapshotNode.SetAttribute("watermark", "");
								}
							}
							else
							{
								snapshotNode.SetAttribute("watermark", "");
							}
							
							snapshotRoot.AppendChild(snapshotNode);
							break;
					}
				}
			}
			//-----------------------------------------------------------------------
			if (!waitSnapshot)
				ShowSaveReportForm();
			else
				ApplicationForms.ShowLoadingIcon(Server.Form);
		}

		private void ShowSaveReportForm()
		{
			var saveReportForm = new SaveReportForm
			{
				Icon = Server.Form.Icon,
				Text = Localization["PTSReports_SaveReport"],
			};

			var posDic = _pts.POS.POSServer.ToDictionary(pos => pos.Id, pos => pos.Name);

			var reportViewer = new PTSReportViewer
			{
				POS = posDic,
				TimeZone = Server.Server.TimeZone,
				Snapshots = _snapshots.ToArray(),
				ReportXmlDoc = _resultXml,
				NVRXmlDoc = Xml.LoadXmlFromHttp(CgiLoadNVR, Server.Credential),
				ReportType = "Transaction",
				ShowZoomControl = false
			};
			saveReportForm.AddReport(reportViewer);

			//要呼叫 RefreshReport 才會繪製報表
			reportViewer.RefreshReport();

			saveReportForm.Show();
			saveReportForm.BringToFront();
		}

		private IViewer _viewer;

		private UInt64 _timecode;
		private Int32 _width;
		private Int32 _height;
		private readonly Queue<SnapshotProperty> _queueSnapshotProperty = new Queue<SnapshotProperty>();
		public void LoadSalientSnapshot(ICamera camera, UInt64 timecode, Int32 width, Int32 height)
		{
			if (camera == null || timecode == 0)
				return;

			//busy
			if (_viewer != null)
			{
				_queueSnapshotProperty.Enqueue(new SnapshotProperty
												   {
													   Camera = camera,
													   Timecode = timecode,
													   Width = width,
													   Height = height,
												   });
				return;
			}
			
			_timecode = timecode;
			_width = width;
			_height = height;
			_viewer = App.RegistViewer();
			_viewer.Camera = camera;
			_viewer.OnConnect += SalientViewerOnConnect;
			_viewer.Connect();
		}

		private System.Timers.Timer _getSnapshotTimer;
		private void SalientViewerOnConnect(Object sender, EventArgs<Int32> e)
		{
			_viewer.OnConnect -= SalientViewerOnConnect;

			//connect failure
			if (e.Value == 0)
			{
				CheckNextSnapshot();
				return;
			}

			_viewer.GoTo(_timecode);

			//wait 1sec to get snapshot, //salient's limitation
			if (_getSnapshotTimer == null)
			{
				_getSnapshotTimer = new System.Timers.Timer(1000);
				_getSnapshotTimer.Elapsed += GetSalientSnapshot;
				_getSnapshotTimer.SynchronizingObject = this;
			}
			_getSnapshotTimer.Enabled = true;
		}

		private void GetSalientSnapshot(Object sender, EventArgs e)
		{
			_getSnapshotTimer.Enabled = false;

			Clipboard.Clear();
			//it not goto current timecode, diff > 3 secs
			var timecode = _viewer.Timecode;
			var diff = (timecode > _timecode)
						   ? timecode - _timecode
						   : _timecode - timecode;
			
			if (diff > 3000)
			{
				CheckNextSnapshot();
				return;
			}

			_viewer.Snapshot("", false);
			var image = Clipboard.GetImage();
			if (image != null)
			{
				var b = new Bitmap(_width, _height);
				Graphics g = Graphics.FromImage(b);

				g.DrawImage(image, 0, 0, _width, _height);
				g.Dispose();

			_snapshots.Add(b);
			}
		    _viewer.Stop();
            CheckNextSnapshot();
		}

		private void CheckNextSnapshot()
		{
			App.UnregistViewer(_viewer);
			_viewer = null;

			if (_queueSnapshotProperty.Count > 0)
			{
				var property = _queueSnapshotProperty.Dequeue();
				LoadSalientSnapshot(property.Camera, property.Timecode, property.Width, property.Height);
			}
			else
			{
				ApplicationForms.HideLoadingIcon();
				ShowSaveReportForm();
			}
		}

		public class SnapshotProperty
		{
			public ICamera Camera;
			public UInt64 Timecode;
			public Int32 Width;
			public Int32 Height;
		}
	}
}
