using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.C2Report
{
	public class C2ReportParser
	{
		public LocalReport LocalReport;
		private BindingSource _c2ReportBindingSource;

		public void ParseC2Report()
		{
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.C2Report.C2Report.rdlc";

			SetC2ReportDataSource();

			//default saveas filename
			LocalReport.DisplayName = "C2 Report";
		}

		private void SetC2ReportDataSource()
		{
			_c2ReportBindingSource = new BindingSource();
			var c2ReportDataSet = new C2Report
			{
				DataSetName = "C2Report",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var c2ReportReportDataSource = new ReportDataSource
			{
				Name = "C2Report",
				Value = _c2ReportBindingSource
			};
			_c2ReportBindingSource.DataSource = c2ReportDataSet;
			LocalReport.DataSources.Add(c2ReportReportDataSource);
		}

		public void SetC2Report(List<List<String>> rows)
		{
			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Rank", typeof(String));
			ds.Tables[0].Columns.Add("Cashier", typeof(String));
			ds.Tables[0].Columns.Add("Exception1", typeof(String));
			ds.Tables[0].Columns.Add("Percentage1", typeof(String));
			ds.Tables[0].Columns.Add("Exception2", typeof(String));
			ds.Tables[0].Columns.Add("Percentage2", typeof(String));
			ds.Tables[0].Columns.Add("Exception3", typeof(String));
			ds.Tables[0].Columns.Add("Percentage3", typeof(String));
			ds.Tables[0].Columns.Add("Summary", typeof(String));
			ds.Tables[0].Columns.Add("Percentage", typeof(String));

			foreach (var row in rows)
			{
				ds.Tables[0].Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9]);
			}

			_c2ReportBindingSource.DataSource = ds.Tables[0];
		}
	}
}
