using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.C3Report
{
	public class C3ReportParser
	{
		public LocalReport LocalReport;
		private BindingSource _c3ReportBindingSource;

		public void ParseC3Report()
		{
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.C3Report.C3Report.rdlc";

			SetC3ReportDataSource();

			//default saveas filename
			LocalReport.DisplayName = "C3 Report";
		}

		private void SetC3ReportDataSource()
		{
			_c3ReportBindingSource = new BindingSource();
			var c3ReportDataSet = new C3Report
			{
				DataSetName = "C3Report",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var c3ReportReportDataSource = new ReportDataSource
			{
				Name = "C3Report",
				Value = _c3ReportBindingSource
			};
			_c3ReportBindingSource.DataSource = c3ReportDataSet;
			LocalReport.DataSources.Add(c3ReportReportDataSource);
		}

		public void SetC3Report(List<Image> snapshots)
		{
			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("Snapshot", typeof(Byte[]));

			foreach (var snapshot in snapshots)
			{
				var bytes = PTSReportViewer.ImageToBuffer(snapshot);
				if (bytes != null)
					ds.Tables[0].Rows.Add(bytes);
			}

			_c3ReportBindingSource.DataSource = ds.Tables[0];
		}
	}
}
