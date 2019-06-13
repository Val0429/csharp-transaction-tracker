using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.B2Report
{
	public class B2ReportParser
	{
		public LocalReport LocalReport;
		private BindingSource _b2ReportBindingSource;

		public void ParseB2Report()
		{
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.B2Report.B2Report.rdlc";

			SetB2ReportDataSource();

			//default saveas filename
			LocalReport.DisplayName = "B2 Report";
		}

		private void SetB2ReportDataSource()
		{
			_b2ReportBindingSource = new BindingSource();
			var b2ReportDataSet = new B2Report
			{
				DataSetName = "B2Report",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var b2ReportReportDataSource = new ReportDataSource
			{
				Name = "B2Report",
				Value = _b2ReportBindingSource
			};
			_b2ReportBindingSource.DataSource = b2ReportDataSet;
			LocalReport.DataSources.Add(b2ReportReportDataSource);
		}

		public void SetB2Report(List<List<String>> rows)
		{
			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Rank", typeof(String));
			ds.Tables[0].Columns.Add("Cashier", typeof(String));
			ds.Tables[0].Columns.Add("Exception", typeof(String));
			ds.Tables[0].Columns.Add("Count1", typeof(String));
			ds.Tables[0].Columns.Add("Count2", typeof(String));
			ds.Tables[0].Columns.Add("Count3", typeof(String));
			ds.Tables[0].Columns.Add("Summary", typeof(String));

			var rank = "";
			var cashier = "";
			foreach (var row in rows)
			{
				if (rank != row[0] || cashier != row[1])
				{
					rank = row[0];
					cashier = row[1];
					ds.Tables[0].Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6]);
				}else
					ds.Tables[0].Rows.Add("", "", row[2], row[3], row[4], row[5], row[6]);
			}

			_b2ReportBindingSource.DataSource = ds.Tables[0];
		}
	}
}
