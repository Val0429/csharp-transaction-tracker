using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.Transaction
{
	public class TransactionParser
	{
		public Image[] Snapshots;
		private BindingSource _transactionBindingSource;

		public LocalReport LocalReport;
		public XmlDocument ReportXmlDoc;
		public XmlDocument NVRXmlDoc;
		public Dictionary<String, String> Localization;

		public void ParseTransaction()
		{
			Log.Write("[Transaction]", true, "report.txt");

			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_ExceptionReport", "Exception Report"},
								   {"PTSReports_User", "User"},
								   {"PTSReports_Server", "Server"},
								   {"PTSReports_PrintTime", "Print Time"},
								   {"PTSReports_Description", "Description"},
								   {"PTSReports_StoreInformation", "Store Information"},
								   {"PTSReports_TransactionInformation", "Transaction Information"},
								   {"PTSReports_Detail", "Detail"},
								   {"PTSReports_TransactionTime", "Transaction Time"},
								   {"PTSReports_CashierId", "Cashier Id"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_StoreId", "Store Id"},
								   {"PTSReports_Store", "Store"},
								   {"PTSReports_POSId", "POS Register Id"},
								   {"PTSReports_POS", "POS"},
							   };
			Localizations.Update(Localization);

			//set table
			//SetReport("PTSReportsGenerator.Transaction.Transaction.rdlc");
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.Transaction.Transaction.rdlc";

			SetTransactionDataSource();

			//default saveas filename
			LocalReport.DisplayName = "Transaction";

			ParseTransactionXml();
		}

		private void SetTransactionDataSource()
		{
			_transactionBindingSource = new BindingSource();
			var transactionDataSet = new TransactionDetail
			{
				DataSetName = "TransactionDetail",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var transactionReportDataSource = new ReportDataSource
			{
				Name = "TransactionDetail",
				Value = _transactionBindingSource
			};
			_transactionBindingSource.DataSource = transactionDataSet;
			LocalReport.DataSources.Add(transactionReportDataSource);
		}

		private void SetTransactionParameter()
		{
			//<Result>
			//    <Server>localhost</Server>
			//    <User>Admin</User>
			//    <PrintTime>2012-12-12 12:34:56</PrintTime>
			//    <Description>comments</Description>
			//    <StoreId>1</StoreId>
			//    <Store>somewhere</Store>
			//    <POSId>1</POSId>
			//    <POS>somewhere</POS>
			//    <CashierId>1</CashierId>
			//    <Cashier>someone</Cashier>
			//    <TransactionTime>2012-12-12 12:34:56</TransactionTime>
			//    <TransactionDetail>123</TransactionDetail>
			//    <Snapshots>
			//        <Snapshot width="400" height="340" nvrId="1" channelId="1" timestamp="1360139673000" />
			//       <Snapshot width="400" height="340" nvrId="1" channelId="2" timestamp="1360139673000" />
			//  </Snapshots>
			//</Result>

			var headerInfo = Localization["PTSReports_User"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "User") + ", " +
							 Localization["PTSReports_Server"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "Server") + ", " +
							 Localization["PTSReports_PrintTime"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "PrintTime");

			LocalReport.SetParameters(new[]
			{ 
				new ReportParameter("ExceptionReport", Localization["PTSReports_ExceptionReport"]),
				new ReportParameter("HeaderInfo", headerInfo),
				new ReportParameter("Description", Localization["PTSReports_Description"]),
				new ReportParameter("DescriptionContent", Xml.GetFirstElementValueByTagName(ReportXmlDoc, "Description")),
				new ReportParameter("StoreInformation", Localization["PTSReports_StoreInformation"]),
				new ReportParameter("StoreId", Localization["PTSReports_StoreId"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "StoreId")),
				new ReportParameter("StoreName", Localization["PTSReports_Store"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "Store")),
				new ReportParameter("POSId", Localization["PTSReports_POSId"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "POSId")),
				new ReportParameter("POSName", Localization["PTSReports_POS"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "POS")),
				new ReportParameter("CashierId", Localization["PTSReports_CashierId"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "CashierId")),
				new ReportParameter("CashierName", Localization["PTSReports_Cashier"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "Cashier")),
				new ReportParameter("TransactionInformation",Localization["PTSReports_TransactionInformation"]),
				new ReportParameter("TransactionTime", Localization["PTSReports_TransactionTime"] + " : " + Xml.GetFirstElementValueByTagName(ReportXmlDoc, "TransactionTime")),
				new ReportParameter("Detail",Localization["PTSReports_Detail"]),
				new ReportParameter("DetailContent", Xml.GetFirstElementValueByTagName(ReportXmlDoc, "TransactionDetail")),
			});
		}

		private Font _watermarkfont = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
		private const String CgiSnapshotWithTimecodeAndSize = @"cgi-bin/snapshot?channel=channel%1&timestamp={TIMECODE}&width={WIDTH}&height={HEIGHT}";
		private void ParseTransactionXml()
		{
			SetTransactionParameter();
			ParseNVRXml();

			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Snapshot", typeof(Byte[]));
			
			//<Snapshots>
			//    <Snapshot width="400" height="340" nvrId="1" channelId="1" timestamp="1360139673000" watermark="01 XXX FPS:XX H.264"/>
			//    <Snapshot width="400" height="340" nvrId="1" channelId="2" timestamp="1360139673000" watermark=""/>
			//</Snapshots>
			
			var snapshots = new List<Image>();
			var snapshotsNode = Xml.GetFirstElementByTagName(ReportXmlDoc, "Snapshots");
			if (snapshotsNode != null)
			{
				var snapshotNodeList = snapshotsNode.GetElementsByTagName("Snapshot");
				foreach (XmlElement snapshotNode in snapshotNodeList)
				{
					if (String.IsNullOrEmpty(snapshotNode.GetAttribute("nvrId"))) continue;
					var nvrId = Convert.ToUInt16(snapshotNode.GetAttribute("nvrId"));
					if (!_nvrConnection.ContainsKey(nvrId)) continue;

					var nvr = _nvrConnection[nvrId];

					var snapshot = LoadImageFromHttp(CgiSnapshotWithTimecodeAndSize
																 .Replace("%1", snapshotNode.GetAttribute("channelId"))
																 .Replace("{TIMECODE}", snapshotNode.GetAttribute("timestamp"))
																 .Replace("{WIDTH}", snapshotNode.GetAttribute("width"))
																 .Replace("{HEIGHT}", snapshotNode.GetAttribute("height")), nvr);


					if(snapshot != null)
					{
						//add text(timestamp) on snapshot
						if (!String.IsNullOrEmpty(snapshotNode.GetAttribute("watermark")))
						{
							var g = Graphics.FromImage(snapshot);
							g.DrawString(snapshotNode.GetAttribute("watermark"), _watermarkfont, Brushes.White, 0, 0);
						}

						snapshots.Add(snapshot);
					}
				}

				//pre load snapshot image(salient)
				if (Snapshots != null && Snapshots.Length > 0)
				{
					snapshots.AddRange(Snapshots);
				}
			}

			if (snapshots.Count == 0)
			{
				snapshots.Add(Properties.Resources.no_image);
			}

			foreach (var snapshot in snapshots)
			{
				var bytes = PTSReportViewer.ImageToBuffer(snapshot);
				if (bytes != null)
					ds.Tables[0].Rows.Add(bytes);
			}

			//必須指定到table[數值] 才抓地到table內的資料
			_transactionBindingSource.DataSource = ds.Tables[0];

			Log.Write("completed", true, "report.txt");
			Log.Write("---------------------------------------------------------", false, "report.txt");
		}

		private static Image LoadImageFromHttp(String url, ServerCredential credential)
		{
			Image tmpImage = null;
			try
			{
				var request = Xml.GetHttpRequest(url, credential);
				var webResponse = request.GetResponse();

				var webStream = webResponse.GetResponseStream();

				if (webStream != null)
				{
					tmpImage = Image.FromStream(webStream);
					tmpImage.Tag = url;
					webStream.Close();
				}

				webResponse.Close();
			}
			catch (System.Exception)
			{
				return null;
			}

			return tmpImage;
		}

		//<CMS>
		//    <NVR id="1" name="localhost NVR">
		//        <Domain>172.16.1.16</Domain> 
		//        <Port>80</Port> 
		//        <Account>zQjDgOyQcPU=</Account> 
		//        <Password>JRp9eL+fp18=</Password> 
		//        <IsListenEvent>true</IsListenEvent> 
		//        <IsPatrolInclude>true</IsPatrolInclude> 
		//        <Modified>1360077756005</Modified> 
		//    </NVR>
		//</CMS>

		private readonly Dictionary<UInt16, ServerCredential> _nvrConnection = new Dictionary<UInt16, ServerCredential>();
		private void ParseNVRXml()
		{
			_nvrConnection.Clear();
			var nvrNodes = NVRXmlDoc.GetElementsByTagName("NVR");
			foreach (XmlElement nvrNode in nvrNodes)
			{
				var id = Convert.ToUInt16(nvrNode.GetAttribute("id"));
				if (_nvrConnection.ContainsKey(id)) continue;

				_nvrConnection.Add(id, new ServerCredential
				{
					Domain = Xml.GetFirstElementValueByTagName(nvrNode, "Domain").Trim(),
					Port = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(nvrNode, "Port")),
					UserName = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(nvrNode, "Account")),
					Password = Encryptions.DecryptDES(Xml.GetFirstElementValueByTagName(nvrNode, "Password")),
				});
			}
		}
	}
}
