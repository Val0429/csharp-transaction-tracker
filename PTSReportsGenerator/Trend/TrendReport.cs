using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.Trend
{
	public class TrendParser
	{
		private BindingSource _trendBindingSource;

		public LocalReport LocalReport;
		public XmlDocument ReportXmlDoc;

		public void ParseTrend()
		{
			Log.Write("[Trend]", true, "report.txt");

			//SetReport("PTSReportsGenerator.Summary.SummaryExceptionCountReport.rdlc");
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.Trend.ExceptionTrendReport.rdlc";

			SetTrendDataSource();

			//default saveas filename
			LocalReport.DisplayName = "Trend";

			ParseTrendXml();
		}

		private void SetTrendDataSource()
		{
			_trendBindingSource = new BindingSource();
			var exceptionTrendDataSet = new ExceptionTrend
			{
				DataSetName = "ExceptionTrend",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var trendReportDataSource = new ReportDataSource
			{
				Name = "ExceptionTrend",
				Value = _trendBindingSource
			};
			_trendBindingSource.DataSource = exceptionTrendDataSet;
			LocalReport.DataSources.Add(trendReportDataSource);
		}

		private void SetTrendParameter(XmlElement root)
		{
			//add str parameter
			var cashier = root.GetAttribute("cashier");

			LocalReport.SetParameters(new[]
			{
				new ReportParameter("Cashier", cashier),
			});
		}

		private void ParseTrendXml()
		{
			var root = ReportXmlDoc.FirstChild as XmlElement;
			if (root == null) return;

			SetTrendParameter(root);

			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var chartds = new DataSet();
			chartds.Tables.Add();

			chartds.Tables[0].Columns.Add("DateTimeRange", typeof(String));
			chartds.Tables[0].Columns.Add("Count", typeof(Int32));

			var results = Report.CustomReport.ParseTrendXmlToExceptionCountList(ReportXmlDoc);

			foreach (var result in results)
			{
				//if (result.Count == 0) continue;

				chartds.Tables[0].Rows.Add(result.DateTime, result.Count);
			}

			//必須指定到table[數值] 才抓地到table內的資料
			_trendBindingSource.DataSource = chartds.Tables[0];

			Log.Write("completed", true, "report.txt");
			Log.Write("---------------------------------------------------------", false, "report.txt");
		}
	}
}
