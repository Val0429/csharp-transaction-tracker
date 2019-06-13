using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.Summary
{
	public class SummaryParser
	{
		private BindingSource _summaryBindingSource;

		public LocalReport LocalReport;
		public XmlDocument ReportXmlDoc;

		public void ParseSummary()
		{
			Log.Write("[Summary]", true, "report.txt");

			//SetReport("PTSReportsGenerator.Summary.SummaryExceptionCountReport.rdlc");
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.Summary.SummaryExceptionCountReport.rdlc";

			SetSummaryDataSource();

			//default saveas filename
			LocalReport.DisplayName = "Summary";

			ParseSummaryXml();
		}

		private void SetSummaryDataSource()
		{
			_summaryBindingSource = new BindingSource();
			var summaryExceptionDataSet = new ExceptionCount
			{
				DataSetName = "PTSReportsGeneratorSummary",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var summaryReportDataSource = new ReportDataSource
			{
				Name = "PTSReportsGeneratorSummary",
				Value = _summaryBindingSource
			};
			_summaryBindingSource.DataSource = summaryExceptionDataSet;
			LocalReport.DataSources.Add(summaryReportDataSource);
		}

		private void ParseSummaryXml()
		{
			var root = ReportXmlDoc.FirstChild as XmlElement;
			if (root == null) return;

			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var chartds = new DataSet();
			chartds.Tables.Add();

			chartds.Tables[0].Columns.Add("DateTimeRange", typeof(String));
			chartds.Tables[0].Columns.Add("Exception", typeof(String));
			chartds.Tables[0].Columns.Add("Count", typeof(Int32));

			var results = Report.CustomReport.ParseSummaryXmlToExceptionCountList(ReportXmlDoc);

			foreach (var result in results)
			{
				//if (result.Count == 0) continue;

				chartds.Tables[0].Rows.Add(result.DateTime, POS_Exception.FindExceptionValueByKey(result.Exception), result.Count);
			}

			//必須指定到table[數值] 才抓地到table內的資料
			_summaryBindingSource.DataSource = chartds.Tables[0];

			Log.Write("completed", true, "report.txt");
			Log.Write("---------------------------------------------------------", false, "report.txt");
		}
	}
}
