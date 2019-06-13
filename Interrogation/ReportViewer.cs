using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Constant;
using Device;

namespace Interrogation
{
    public partial class ReportViewer : Investigation.SaveReport.InvestigationReportViewer
    {
        public ReportViewer()
        {
            Localization.Add("EventResultPanel_Name", "Name");
            Localization.Add("EventResultPanel_NoOfRecord", "No. of Record");
            Localizations.Update(Localization);

            InitializeComponent();
        }

        public override void ParseEventList(List<CameraEvents> resultList)
        {
            //LocalReport.ReportEmbeddedResource = "Investigation.SaveReport.EventReport.rdlc";
            LocalReport.ReportEmbeddedResource = "Interrogation.EventReport.rdlc";

            SetEventReportDataSource();
            SetEventReportParameter();

            //default saveas filename
            LocalReport.DisplayName = "Event List";

            //Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
            var ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("NoOfRecord", typeof(String));
            ds.Tables[0].Columns.Add("Name", typeof(String));
            ds.Tables[0].Columns.Add("DateTime", typeof(String));
            ds.Tables[0].Columns.Add("Device", typeof(String));

            foreach (var result in resultList)
            {
                var xmlDoc = Xml.LoadXml(result.Status);

                var name = Xml.GetFirstElementValueByTagName(xmlDoc, "Name");
                var record = Xml.GetFirstElementValueByTagName(xmlDoc, "NoOfRecord");

                ds.Tables[0].Rows.Add(record, name, result.DateTime.ToString("MM-dd-yyyy HH:mm:ss"), result.Device);
            }

            //必須指定到table[數值] 才抓地到table內的資料
            _eventListBindingSource.DataSource = ds.Tables[0];

            Refresh();
        }

        protected override void SetEventReportParameter()
        {
            var imagePath = String.Empty;
            if (!File.Exists(Application.StartupPath + "\\images\\logo.png"))
            {
                imagePath = new Uri(Application.StartupPath + "\\images\\logo.png").AbsoluteUri;
            }
            LocalReport.EnableExternalImages = true;

            //add str parameter
            LocalReport.SetParameters(new[]
			{
				new Microsoft.Reporting.WinForms.ReportParameter("NoOfRecord", Localization["EventResultPanel_NoOfRecord"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Name", Localization["EventResultPanel_Name"]),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime", Localization["EventResultPanel_DateTime"]),
                new Microsoft.Reporting.WinForms.ReportParameter("Device", Localization["EventResultPanel_Device"]),
                new Microsoft.Reporting.WinForms.ReportParameter("ImagePath", imagePath),
			});
        }
    }
}
