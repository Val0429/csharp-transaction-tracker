using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.Exception
{
	public class ExceptionChartParser
	{
		private BindingSource _chartBindingSource;

		public LocalReport LocalReport;
		public XmlDocument ReportXmlDoc;
        public Dictionary<String, String> POS = new Dictionary<String, String>();

		public void ParseExceptionChart()
		{
			//set table
			//SetReport("PTSReportsGenerator.Exception.WeeklyExceptionChartReport.rdlc");
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.Exception.WeeklyExceptionChartReport.rdlc";
			
			SetExceptionChartDataSource();

			//default saveas filename
			LocalReport.DisplayName = "Exception Chart";

			ParseExceptionChartXml();
		}

		private void SetExceptionChartDataSource()
		{
			_chartBindingSource = new BindingSource();
			var exceptionChartDataSet = new ExceptionCount
			{
				DataSetName = "ExceptionCount",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};
			var exceptionChartDataSource = new ReportDataSource
			{
				Name = "ExceptionCount",
				Value = _chartBindingSource
			};
			_chartBindingSource.DataSource = exceptionChartDataSet;
			LocalReport.DataSources.Add(exceptionChartDataSource);
		}

		private void ParseExceptionChartXml()
		{
			var root = ReportXmlDoc.FirstChild as XmlElement;
			if (root == null) return;

			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var chartds = new DataSet();
			chartds.Tables.Add();
			chartds.Tables[0].Columns.Add("POS", typeof(String));
			chartds.Tables[0].Columns.Add("Exception", typeof(String));
			chartds.Tables[0].Columns.Add("Count", typeof(Int32));

			var results = Report.ReadExceptionCalculationByDateGroupByRegister.Parse(ReportXmlDoc);

			var totalException = new List<POS_Exception.ExceptionCount>();

			foreach (var result in results)
			{
				if (result.Value.Count == 0) continue;

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

					var pos =
					(POS.ContainsKey(result.Key))
						? result.Key + " " + POS[result.Key]
						//: result.POSId.ToString().PadLeft(2, '0') + " POS";
						: result.Key + " Store";

					chartds.Tables[0].Rows.Add(pos, POS_Exception.FindExceptionValueByKey(exceptionCount.Exception), exceptionCount.Count);
				}
			}

			//必須指定到table[數值] 才抓地到table內的資料
			_chartBindingSource.DataSource = chartds.Tables[0];

			Log.Write("completed", true, "report.txt");
			Log.Write("---------------------------------------------------------", false, "report.txt");
		}
	}
}
