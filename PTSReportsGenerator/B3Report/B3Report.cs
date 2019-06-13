using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.B3Report
{
	public class B3ReportParser
	{
		public LocalReport LocalReport;
		private BindingSource _b3ReportBindingSource;

		public void ParseB3Report()
		{
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.B3Report.B3Report.rdlc";

			SetB3ReportDataSource();

			//default saveas filename
			LocalReport.DisplayName = "B3 Report";
		}

		private void SetB3ReportDataSource()
		{
			_b3ReportBindingSource = new BindingSource();
			var b3ReportDataSet = new B3Report
			{
				DataSetName = "B3Report",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var b3ReportReportDataSource = new ReportDataSource
			{
				Name = "B3Report",
				Value = _b3ReportBindingSource
			};
			_b3ReportBindingSource.DataSource = b3ReportDataSet;
			LocalReport.DataSources.Add(b3ReportReportDataSource);
		}

		public void SetB3Report(List<List<String>> rows)
		{
			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Rank", typeof(String));
			ds.Tables[0].Columns.Add("Exception", typeof(String));
			ds.Tables[0].Columns.Add("Count1", typeof(String));
			ds.Tables[0].Columns.Add("Threshold1", typeof(String));
			ds.Tables[0].Columns.Add("Difference1", typeof(String));
			ds.Tables[0].Columns.Add("Count2", typeof(String));
			ds.Tables[0].Columns.Add("Threshold2", typeof(String));
			ds.Tables[0].Columns.Add("Difference2", typeof(String));
			ds.Tables[0].Columns.Add("Count3", typeof(String));
			ds.Tables[0].Columns.Add("Threshold3", typeof(String));
			ds.Tables[0].Columns.Add("Difference3", typeof(String));

			foreach (var row in rows)
			{
				ds.Tables[0].Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10]);
			}

			_b3ReportBindingSource.DataSource = ds.Tables[0];
		}
	}
}
