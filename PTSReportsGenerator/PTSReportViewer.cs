using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;
using PTSReportsGenerator.Exception;
using PTSReportsGenerator.ExceptionList;
using PTSReportsGenerator.Summary;
using PTSReportsGenerator.Transaction;
using PTSReportsGenerator.TransactionList;
using PTSReportsGenerator.Trend;

namespace PTSReportsGenerator
{
	public interface IPTSReportViewer
	{   
		String CreateReport(String reportType, String reportXml, String nvrXml, String fileType, String fileNamePrefix, String fileNameSuffix);
		void Exit();
	}

	public sealed class PTSReportViewer : ReportViewer, IPTSReportViewer
	{
		public String ReportType
		{
			set
			{
                POS_Exception POSException = new POS_Exception();
			    try
			    {
                    switch (value)
                    {
                        //i really dont remember who will use this report? maybe it's schedule report?
                        case "Exception":
                            var exceptionParser = new ExceptionParser
                            {
                                LocalReport = LocalReport,
                                ReportXmlDoc = ReportXmlDoc,
                                POS = POS
                            };
                            exceptionParser.ParseException();
                            break;

                        case "ExceptionChart":
                            var exceptionChartParser = new ExceptionChartParser
                            {
                                LocalReport = LocalReport,
                                ReportXmlDoc = ReportXmlDoc,
                                POS = POS
                            };
                            exceptionChartParser.ParseExceptionChart();
                            break;

                        case "ExceptionList":
                            var exceptionListParser = new ExceptionListParser
                            {
                                LocalReport = LocalReport,
                                ReportXmlDoc = ReportXmlDoc,
                                POS = POS,
                                TimeZone = TimeZone,
                            };
                            exceptionListParser.ParseExceptionList();
                            break;

                        case "Summary":
                            var summaryParser = new SummaryParser
                            {
                                LocalReport = LocalReport,
                                ReportXmlDoc = ReportXmlDoc,
                            };
                            summaryParser.ParseSummary();
                            break;

                        case "Transaction":
                            var transactionParser = new TransactionParser
                            {
                                LocalReport = LocalReport,
                                ReportXmlDoc = ReportXmlDoc,
                                NVRXmlDoc = NVRXmlDoc,
                                Snapshots = Snapshots
                            };
                            transactionParser.ParseTransaction();
                            break;

                        case "Trend":
                            var trendParser = new TrendParser
                            {
                                LocalReport = LocalReport,
                                ReportXmlDoc = ReportXmlDoc,
                            };
                            trendParser.ParseTrend();
                            break;

                        case "TransactionList":
                            var transactionListParser = new TransactionListParser
                            {
                                LocalReport = LocalReport,
                                ReportXmlDoc = ReportXmlDoc,
                            };
                            transactionListParser.ParseTransactionList();
                            break;
                    }

                    Log.Write("Success!!");
                    Log.Write("Type:" + value);
                    Log.Write("ReportXmlDoc:" + ReportXmlDoc.OuterXml);
                    
			    }
			    catch (System.Exception exception)
			    {
                    Log.Write("Error:" + exception.Message);
                    Log.Write("Type:" + value);
                    Log.Write("ReportXmlDoc:" + ReportXmlDoc.OuterXml);
			    }
				
			}
		}

        public Dictionary<String, String> POS = new Dictionary<String, String>();
		public Int32 TimeZone;
		public XmlDocument ReportXmlDoc;
		public XmlDocument NVRXmlDoc;
		public Image[] Snapshots;

		private String _prefix = "";
		private String _suffix = "";
		public PTSReportViewer()
		{
			Dock = DockStyle.Fill;
			Location = new Point(0, 0);
			Size = new Size(579, 545);//as xml-> file, default size, if show on form, it's dock.fill to parent
		}

		//call from outer (server) convert xml -> report
		public String CreateReport(String reportType, String reportXml, String nvrXml = "", String fileType = "PDF", String fileNamePrefix = "", String fileNameSuffix = "")
		{
            Log.Write("TYPE: " + reportType, false);
            Log.Write("REPORT: " + reportXml, false);
            Log.Write("NVR: " + nvrXml, false);
            Log.Write("FILE TYPE: " + fileType, false);

			_prefix = fileNamePrefix ?? "";
			_suffix = fileNameSuffix ?? "";

			try
			{
				ReportXmlDoc = new XmlDocument();
				if (reportXml.Substring(reportXml.Length - 4).ToUpper() == ".XML")
				{
					if (!File.Exists(reportXml))
						return "";

					ReportXmlDoc.Load(reportXml);
				}
				else
				{
					ReportXmlDoc.LoadXml(reportXml);
				}
			}
			catch (System.Exception exception)
			{
				Log.Write("Parse Report Exception: " + exception, false);
				Log.Write("-----------------------------------------");
				return "";
			}
            Log.Write("Parse Report Done");

			try
			{
				NVRXmlDoc = new XmlDocument();
				if (!String.IsNullOrEmpty(nvrXml))
				{
					if (nvrXml.Substring(nvrXml.Length - 4).ToUpper() == ".XML")
					{
						if (!File.Exists(nvrXml))
							return "";

						NVRXmlDoc.Load(nvrXml);
					}
					else
					{
						NVRXmlDoc.LoadXml(nvrXml);
					}
				}
			}
			catch (System.Exception exception)
			{
				Log.Write("Parse NVR Exception: " + exception, false);
				Log.Write("-----------------------------------------");
				return "";
			}
            Log.Write("Parse NVR Done");

			try
			{
				ReportType = reportType;
			}
			catch (System.Exception exception)
			{
				Log.Write("Convert File Exception: " + exception, false);
				Log.Write("-----------------------------------------");
				return "";
			}
            Log.Write("Convert File Done");

			String fileName;
			try
			{
				var path = Application.StartupPath + "\\" + "reports";
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				switch (fileType.ToUpper())
				{
					case "PDF":
						fileName = SaveAsPDF(path);
						break;

					case "WORD":
						fileName = SaveAsDoc(path);
						break;

					case "EXCEL":
						fileName = SaveAsExcel(path);
						break;

					default:
						fileName = SaveAsPDF(path);
						break;
				}
			}
			catch (System.Exception exception)
			{
				Log.Write("Save File Exception: " + exception, false);
				Log.Write("-----------------------------------------");
				return "";
			}
            Log.Write("Save File Done");
            Log.Write("-----------------------------------------");

			return fileName;
		}

		public void Exit()
		{
			Application.Exit();
		}

		//------------------------------------------------------------------------------------------------------

		public String SaveAsPDF(String path)
		{
			return SaveAs(path, "PDF");
		}

		public String SaveAsDoc(String path)
		{
			return SaveAs(path, "Word");
		}

		public String SaveAsExcel(String path)
		{
			return SaveAs(path, "Excel");
		}

		public String SaveAs(String path, String type) //D:\\
		{
			if ((path[path.Length - 1] != '\\'))
				path += "\\";

			Warning[] warnings;
			String[] streamids;
			String mimeType;
			String encoding;
			String extension;
			var bytes = LocalReport.Render(type, null, out mimeType, out encoding, out extension, out streamids, out warnings);

			var filaName = path + (_prefix ?? "") + (Tag ?? Name) + @"_" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss.fff") + (_suffix ?? "");
			switch (type)
			{
				case "PDF":
					filaName += @".pdf";
					break;

				case "Word":
					filaName += @".doc";
					break;

				case "Excel":
					filaName += @".xls";
					break;
			}
			var fs = new FileStream(filaName, FileMode.Create);

			fs.Write(bytes, 0, bytes.Length);
			fs.Close();

			return filaName;
		}

		public Byte[] SnapshotImage()
		{
			Warning[] warnings;
			String[] streamids;
			String mimeType;
			String encoding;
			String extension;
			var bytes = LocalReport.Render("Image", null, out mimeType, out encoding, out extension, out streamids, out warnings);


			return bytes;
		}
		
		public static Byte[] ImageToBuffer(Image image)
		{
			if (image == null) { return null; }

			var memoryStream = new MemoryStream();
			var bitmap = new Bitmap(image);

			bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

			memoryStream.Position = 0;

			var data = new byte[memoryStream.Length];

			memoryStream.Read(data, 0, Convert.ToInt32(memoryStream.Length));
			memoryStream.Flush();

			return data;
		}

		public Boolean UpdateParameter(String name, String value)
		{
			//if parameter is the same, no need to set
			var paramaters = LocalReport.GetParameters();
			foreach (var paramater in paramaters)
			{
				if (paramater.Name == name)
				{
					if (paramater.Values.Count > 0)
					{
						if (paramater.Values[0] == value)
						{
							return false;
						}
					}
					break;
				}
			}

			LocalReport.SetParameters(new[]
			{
				new ReportParameter(name, value)
			});

			return true;
		}
	}
}