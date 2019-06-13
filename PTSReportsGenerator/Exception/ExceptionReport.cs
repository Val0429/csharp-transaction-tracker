using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.Exception
{
	public class ExceptionParser
	{
		private BindingSource _weeklyBindingSource;
		private BindingSource _countBindingSource;

		public LocalReport LocalReport;
		public XmlDocument ReportXmlDoc;
        public Dictionary<String, String> POS = new Dictionary<String, String>();
		public Dictionary<String, String> Localization;

		public void ParseException()
		{
			Log.Write("[Exception]", true, "report.txt");

			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_Store", "Store"},
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_Count", "Count"},
								   {"PTSReports_Information", "Information"},
								   {"PTSReports_Detail", "Detail"},

								   {"PTSReports_Summary", "Summary"},
								   {"PTSReports_Total", "Total"},
							   };
			Localizations.Update(Localization);

			//set table
			//SetReport("PTSReportsGenerator.Exception.WeeklyExceptionReport.rdlc");
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.Exception.WeeklyExceptionReport.rdlc";
			
			SetExceptionDataSource();

			//default saveas filename
			LocalReport.DisplayName = "Exception";

			ParseExceptionXml();
		}

		private void SetExceptionDataSource()
		{
			_weeklyBindingSource = new BindingSource();
			var weeklyExceptionDataSet = new WeeklyException
			{
				DataSetName = "WeeklyException",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var weeklyExceptionReportDataSource = new ReportDataSource
			{
				Name = "WeeklyException",
				Value = _weeklyBindingSource
			};
			_weeklyBindingSource.DataSource = weeklyExceptionDataSet;
			LocalReport.DataSources.Add(weeklyExceptionReportDataSource);
			//------------------------------------------------------------
			_countBindingSource = new BindingSource();
			var exceptionCountDataSet = new ExceptionCount
			{
				DataSetName = "ExceptionCount",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};
			var exceptionCountDataSource = new ReportDataSource
			{
				Name = "ExceptionCount",
				Value = _countBindingSource
			};
			_countBindingSource.DataSource = exceptionCountDataSet;
			LocalReport.DataSources.Add(exceptionCountDataSource);
		}
		
		private void SetExceptionParameter(XmlElement root)
		{
			//add str parameter
			var timezoneStr = root.GetAttribute("timeZone");

			var timeRange = "";
			if (timezoneStr != "")
			{
				var timezone = Convert.ToInt32(timezoneStr) / 1000;
				var startDate = DateTimes.ToDateTime(Convert.ToUInt64(root.GetAttribute("startUTC")), timezone);
				var endDate = DateTimes.ToDateTime(Convert.ToUInt64(root.GetAttribute("endUTC")), timezone);

				timeRange = startDate.ToString("MM-dd-yyyy HH:mm:ss") + @" ~ " + endDate.ToString("MM-dd-yyyy HH:mm:ss");
			}

			LocalReport.SetParameters(new[]
			{
				new ReportParameter("DateTimeRange", timeRange),
				new ReportParameter("Store", Localization["PTSReports_Store"]),
				new ReportParameter("DateTime", Localization["PTSReports_DateTime"]),
				new ReportParameter("Exception", Localization["PTSReports_Exception"]),
				new ReportParameter("Count", Localization["PTSReports_Count"]),
				new ReportParameter("Information", Localization["PTSReports_Information"]),
				new ReportParameter("Detail", Localization["PTSReports_Detail"]),
			});
		}

		private void ParseExceptionXml()
		{
			var root = ReportXmlDoc.FirstChild as XmlElement;
			if (root == null) return;

			SetExceptionParameter(root);

			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Store", typeof(String));
			ds.Tables[0].Columns.Add("DateTime", typeof(String));
			ds.Tables[0].Columns.Add("Exception", typeof(String));
			ds.Tables[0].Columns.Add("Count", typeof(Int32));

			var chartds = new DataSet();
			chartds.Tables.Add();
			chartds.Tables[0].Columns.Add("Store", typeof(String));
			chartds.Tables[0].Columns.Add("Exception", typeof(String));
			chartds.Tables[0].Columns.Add("Count", typeof(Int32));

			var results = Report.ReadExceptionCalculationByDateGroupByRegister.Parse(ReportXmlDoc);

			var totalException = new List<POS_Exception.ExceptionCount>();

			foreach (var result in results)
			{
				if (result.Value.Count == 0) continue;

				ds.Tables[0].Rows.Add(result.Key);

				foreach (var exceptionCount in result.Value)
				{
					POS_Exception.ExceptionCount totalAmount = null;
					foreach (var count in totalException)
					{
						if (count.Exception == exceptionCount.Exception)
						{
							totalAmount = count;
							break;
						}
					}
					if (totalAmount == null)
					{
						totalAmount = new POS_Exception.ExceptionCount
						{
							Exception = exceptionCount.Exception
						};
						totalException.Add(totalAmount);
					}
					totalAmount.Count += exceptionCount.Count;

					ds.Tables[0].Rows.Add("", exceptionCount.DateTime, POS_Exception.FindExceptionValueByKey(exceptionCount.Exception), exceptionCount.Count);

					var pos =
					(POS.ContainsKey(result.Key))
						? result.Key + " " + POS[result.Key]
						//: result.POSId.ToString().PadLeft(2, '0') + " POS";
						: result.Key + " Store";

					chartds.Tables[0].Rows.Add(pos, POS_Exception.FindExceptionValueByKey(exceptionCount.Exception), exceptionCount.Count);
				}
			}

			if (totalException.Count > 0)
			{
				ds.Tables[0].Rows.Add(Localization["PTSReports_Summary"]);

				totalException.Sort((x, y) => x.Exception.CompareTo(y.Exception));
				var total = 0;
				foreach (var exceptionCount in totalException)
				{
					ds.Tables[0].Rows.Add("", "", POS_Exception.FindExceptionValueByKey(exceptionCount.Exception), exceptionCount.Count);
					total += exceptionCount.Count;
				}
				ds.Tables[0].Rows.Add("");
				ds.Tables[0].Rows.Add("", "", Localization["PTSReports_Total"], total);
			}

			//必須指定到table[數值] 才抓地到table內的資料
			_weeklyBindingSource.DataSource = ds.Tables[0];
			_countBindingSource.DataSource = chartds.Tables[0];

			Log.Write("completed", true, "report.txt");
			Log.Write("---------------------------------------------------------", false, "report.txt");
		}
	}
}
