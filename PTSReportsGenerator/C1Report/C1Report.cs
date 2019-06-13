using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.C1Report
{
	public class C1ReportParser
	{
		public LocalReport LocalReport;
		private BindingSource _c1ReportBindingSource;

		public void ParseC1Report()
		{
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.C1Report.C1Report.rdlc";

			SetC1ReportDataSource();

			//default saveas filename
			LocalReport.DisplayName = "C1 Report";
		}

		private void SetC1ReportDataSource()
		{
			_c1ReportBindingSource = new BindingSource();
			var c1ReportDataSet = new C1Report
			{
				DataSetName = "C1Report",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var c1ReportReportDataSource = new ReportDataSource
			{
				Name = "C1Report",
				Value = _c1ReportBindingSource
			};
			_c1ReportBindingSource.DataSource = c1ReportDataSet;
			LocalReport.DataSources.Add(c1ReportReportDataSource);
		}

		public void SetC1Report(List<List<String>> rows)
		{
			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Exception", typeof(String));
			ds.Tables[0].Columns.Add("Cashier1", typeof(String));
			ds.Tables[0].Columns.Add("Cashier2", typeof(String));
			ds.Tables[0].Columns.Add("Cashier3", typeof(String));

			foreach (var row in rows)
			{
				ds.Tables[0].Rows.Add(row[0], row[1], row[2], row[3]);
			}

			_c1ReportBindingSource.DataSource = ds.Tables[0];
		}
	}
}
