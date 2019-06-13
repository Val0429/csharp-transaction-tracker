using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.ExceptionList
{
	public class ExceptionListParser
	{
		private BindingSource _exceptionListBindingSource;

		public Int32 TimeZone;
		public LocalReport LocalReport;
		public XmlDocument ReportXmlDoc;
        public Dictionary<String, String> POS = new Dictionary<String, String>();
		public Dictionary<String, String> Localization;

		public void ParseExceptionList()
		{
			Log.Write("[Exception]", true, "report.txt");

			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_ID", "ID"},
								   {"PTSReports_POS", "POS"},
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_CashierId", "Cashier Id"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_Exception", "Exception"},
								   {"PTSReports_ExceptionAmount", "Exception Amount"},
								   {"PTSReports_TransactionAmount", "Transaction Amount"},
							   };
			Localizations.Update(Localization);

			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.ExceptionList.ExceptionList.rdlc";

			SetExceptionListDataSource();

			//default saveas filename
			LocalReport.DisplayName = "Exception List";

			ParseExceptionListXml();
		}

		private void SetExceptionListDataSource()
		{
			_exceptionListBindingSource = new BindingSource();
			var exceptionListDataSet = new ExceptionList
			{
				DataSetName = "ExceptionList",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var exceptionListReportDataSource = new ReportDataSource
			{
				Name = "ExceptionList",
				Value = _exceptionListBindingSource
			};
			_exceptionListBindingSource.DataSource = exceptionListDataSet;
			LocalReport.DataSources.Add(exceptionListReportDataSource);
		}

		private void SetExceptionListParameter()
		{
			//add str parameter
			LocalReport.SetParameters(new[]
			{
				new ReportParameter("ID", Localization["PTSReports_ID"]),
				new ReportParameter("POS", Localization["PTSReports_POS"]),
				new ReportParameter("DateTime", Localization["PTSReports_DateTime"]),
				new ReportParameter("Cashier", Localization["PTSReports_Cashier"]),
				new ReportParameter("CashierId", Localization["PTSReports_CashierId"]),
				new ReportParameter("Exception", Localization["PTSReports_Exception"]),
				new ReportParameter("ExceptionAmount", Localization["PTSReports_ExceptionAmount"]),
				new ReportParameter("TransactionAmount", Localization["PTSReports_TransactionAmount"]),
			});
		}

		private void ParseExceptionListXml()
		{
			var root = ReportXmlDoc.FirstChild as XmlElement;
			if (root == null) return;

			SetExceptionListParameter();

			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("ID", typeof(String));
			ds.Tables[0].Columns.Add("POS", typeof(String));
			ds.Tables[0].Columns.Add("DateTime", typeof(String));
			ds.Tables[0].Columns.Add("CashierId", typeof(String));
			ds.Tables[0].Columns.Add("Cashier", typeof(String));
			ds.Tables[0].Columns.Add("Exception", typeof(String));
			ds.Tables[0].Columns.Add("ExceptionAmount", typeof(String));
			ds.Tables[0].Columns.Add("TransactionAmount", typeof(String));

			var results = Report.ReadExceptionByCondition.Parse(ReportXmlDoc, TimeZone);

			var index = (results.PageIndex - 1) * 20 + 1;
			foreach (var result in results.Results)
			{
				var pos =
				(POS.ContainsKey(result.POSId))
					? result.POSId + " " + POS[result.POSId]
					//: result.POSId.ToString().PadLeft(2, '0') + " POS";
					: result.POSId + " POS";

				var amount =
					(!String.IsNullOrEmpty(result.ExceptionAmount))
						? @"$" + result.ExceptionAmount
						: "";

				var total =
					(!String.IsNullOrEmpty(result.TotalTransactionAmount))
						? @"$" + result.TotalTransactionAmount
						: "";

				ds.Tables[0].Rows.Add(index++, pos, result.DateTime.ToString("MM-dd-yyyy HH:mm:ss"), result.CashierId, result.Cashier, POS_Exception.FindExceptionValueByKey(result.Type), amount, total);
			}

			//必須指定到table[數值] 才抓地到table內的資料
			_exceptionListBindingSource.DataSource = ds.Tables[0];

			Log.Write("completed", true, "report.txt");
			Log.Write("---------------------------------------------------------", false, "report.txt");
		}
	}
}
