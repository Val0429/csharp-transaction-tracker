using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Microsoft.Reporting.WinForms;

namespace PTSReportsGenerator.TransactionList
{
	public class TransactionListParser
	{
		private BindingSource _transactionListBindingSource;

		public LocalReport LocalReport;
		public XmlDocument ReportXmlDoc;
        public Dictionary<String, String> POS = new Dictionary<String, String>();
		public Int32 TimeZone;
		public Dictionary<String, String> Localization;

		public void ParseTransactionList()
		{
			Log.Write("[Exception]", true, "report.txt");

			Localization = new Dictionary<String, String>
							   {
								   {"PTSReports_ID", "ID"},
								   {"PTSReports_POS", "POS"},
								   {"PTSReports_DateTime", "Date / Time"},
								   {"PTSReports_CashierId", "Cashier Id"},
								   {"PTSReports_Cashier", "Cashier"},
								   {"PTSReports_TotalExceptionAmount", "Total Exception Amount"},
								   {"PTSReports_Total", "Total"},
							   };
			Localizations.Update(Localization);
			
			LocalReport.ReportEmbeddedResource = "PTSReportsGenerator.TransactionList.TransactionList.rdlc";

			SetTransactionListDataSource();

			//default saveas filename
			LocalReport.DisplayName = "Transaction List";

			ParseTransactionListXml();
		}

		private void SetTransactionListDataSource()
		{
			_transactionListBindingSource = new BindingSource();
			var transactionListDataSet = new TransactionList
			{
				DataSetName = "TransactionList",
				SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
			};

			var transactionListReportDataSource = new ReportDataSource
			{
				Name = "TransactionList",
				Value = _transactionListBindingSource
			};
			_transactionListBindingSource.DataSource = transactionListDataSet;
			LocalReport.DataSources.Add(transactionListReportDataSource);
		}

		private void SetTransactionListParameter()
		{
			//add str parameter
			LocalReport.SetParameters(new[]
			{
				new ReportParameter("ID", Localization["PTSReports_ID"]),
				new ReportParameter("POS", Localization["PTSReports_POS"]),
				new ReportParameter("DateTime", Localization["PTSReports_DateTime"]),
				new ReportParameter("Cashier", Localization["PTSReports_Cashier"]),
				new ReportParameter("CashierId", Localization["PTSReports_CashierId"]),
				new ReportParameter("ExceptionAmount", Localization["PTSReports_TotalExceptionAmount"]),
				new ReportParameter("Total", Localization["PTSReports_Total"]),
			});
		}

		private void ParseTransactionListXml()
		{
			var root = ReportXmlDoc.FirstChild as XmlElement;
			if (root == null) return;

			SetTransactionListParameter();

			//Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
			var ds = new DataSet();
			ds.Tables.Add();
			ds.Tables[0].Columns.Add("ID", typeof(String));
			ds.Tables[0].Columns.Add("POS", typeof(String));
			ds.Tables[0].Columns.Add("DateTime", typeof(String));
			ds.Tables[0].Columns.Add("CashierId", typeof(String));
			ds.Tables[0].Columns.Add("Cashier", typeof(String));
			ds.Tables[0].Columns.Add("ExceptionAmount", typeof(String));
			ds.Tables[0].Columns.Add("Total", typeof(String));

			var results = Report.ReadTransactionByCondition.Parse(ReportXmlDoc, new XmlDocument(), TimeZone);

			var index = (results.PageIndex - 1) * 20 + 1;
			foreach (var result in results.Results)
			{
				var pos =
				(POS.ContainsKey(result.POSId))
					? result.POSId + " " + POS[result.POSId]
					//: result.POSId.ToString().PadLeft(2, '0') + " POS";
					: result.POSId + " POS";

				var exceptionAmount =
					(!String.IsNullOrEmpty(result.ExceptionAmount))
						? @"$" + result.ExceptionAmount
						: "";

				var total =
					(!String.IsNullOrEmpty(result.Total))
						? @"$" + result.Total
						: "";

				ds.Tables[0].Rows.Add(index++, pos, result.DateTime.ToString("MM-dd-yyyy HH:mm:ss"), result.CashierId, result.Cashier, exceptionAmount, total);
			}

			//必須指定到table[數值] 才抓地到table內的資料
			_transactionListBindingSource.DataSource = ds.Tables[0];

			Log.Write("completed", true, "report.txt");
			Log.Write("---------------------------------------------------------", false, "report.txt");
		}
	}
}
