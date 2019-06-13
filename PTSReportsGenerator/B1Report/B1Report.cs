using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.B1Report
{
	public class B1ReportParser
	{
		public LocalReport LocalReport;
		private BindingSource _b1ReportBindingSource;

		public void ParseB1Report()
		{
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.B1Report.B1Report.rdlc";

			SetB1ReportDataSource();

			//default saveas filename
			LocalReport.DisplayName = "B1 Report";
		}

		private void SetB1ReportDataSource()
		{
			_b1ReportBindingSource = new BindingSource();
			var b1ReportDataSet = new B1Report
			{
				DataSetName = "B1Report",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var b1ReportReportDataSource = new ReportDataSource
			{
				Name = "B1Report",
				Value = _b1ReportBindingSource
			};
			_b1ReportBindingSource.DataSource = b1ReportDataSet;
			LocalReport.DataSources.Add(b1ReportReportDataSource);
		}

		public void SetB1Report(List<List<String>> rows, Image summarySnapshot)
		{
			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Rank", typeof(String));
			ds.Tables[0].Columns.Add("Exception", typeof(String));
			ds.Tables[0].Columns.Add("Count1", typeof(String));
			ds.Tables[0].Columns.Add("Count2", typeof(String));
			ds.Tables[0].Columns.Add("Count3", typeof(String));
			ds.Tables[0].Columns.Add("Summary", typeof(String));
			ds.Tables[0].Columns.Add("Snapshot", typeof(Byte[]));

			var snapshot = false;
			foreach (var row in rows)
			{
				if (!snapshot)
				{
					ds.Tables[0].Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5], PTSReportViewer.ImageToBuffer(summarySnapshot));
					snapshot = true;
					continue;
				}

				ds.Tables[0].Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
			}

			_b1ReportBindingSource.DataSource = ds.Tables[0];
		}
	}
}
