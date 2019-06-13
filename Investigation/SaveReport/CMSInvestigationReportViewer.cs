using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Constant;
using Device;

namespace Investigation.SaveReport
{
	public class CMSInvestigationReportViewer : Microsoft.Reporting.WinForms.ReportViewer
    {
        public Int32 TimeZone;
        public Dictionary<String, String> Localization;

        public CMSInvestigationReportViewer()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"EventResultPanell_ID", "ID"},
								   {"EventResultPanel_NVR", "NVR"},
								   {"EventResultPanel_Device", "Device"},
								   {"EventResultPanel_DateTime", "DateTime"},
								   {"EventResultPanel_Event", "Event"},
							   };
            Localizations.Update(Localization);

            Dock = DockStyle.Fill;
            Location = new System.Drawing.Point(0, 0);
            Size = new System.Drawing.Size(579, 545);//as xml-> file, default size, if show on form, it's dock.fill to parent
        }

		public virtual void ParseEventList(List<CameraEvents> resultList)
        {
            LocalReport.ReportEmbeddedResource = "Investigation.SaveReport.CMSEventReport.rdlc";

            SetEventReportDataSource();
            SetEventReportParameter();

            //default saveas filename
            LocalReport.DisplayName = "Event List";

            //Columns的名稱必須和DataSet所訂立的名稱，資料類型一致，才能將數值套入
            var ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("ID", typeof(String));
            ds.Tables[0].Columns.Add("NVR", typeof(String));
            ds.Tables[0].Columns.Add("Device", typeof(String));
            ds.Tables[0].Columns.Add("DateTime", typeof(String));
            ds.Tables[0].Columns.Add("Event", typeof(String));

            var index = 1;
            foreach (var result in resultList)
            {
                ds.Tables[0].Rows.Add(index++, result.Device.Server.ToString(), result.Device.ToString(), result.DateTime.ToString("MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture), result.ToLocalizationString());
            }

            //必須指定到table[數值] 才抓地到table內的資料
            _eventListBindingSource.DataSource = ds.Tables[0];
        }

	    protected BindingSource _eventListBindingSource;
	    protected void SetEventReportDataSource()
        {
            _eventListBindingSource = new BindingSource();
            var eventListDataSet = new CMSEventReport
            {
                DataSetName = "CMSEventReport",
                SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
            };

            var eventListReportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource
            {
                Name = "CMSEventReport",
                Value = _eventListBindingSource
            };
            _eventListBindingSource.DataSource = eventListDataSet;
            LocalReport.DataSources.Add(eventListReportDataSource);
        }

	    protected void SetEventReportParameter()
        {
            //add str parameter
            LocalReport.SetParameters(new[]
			{
				new Microsoft.Reporting.WinForms.ReportParameter("ID", Localization["EventResultPanell_ID"]),
				new Microsoft.Reporting.WinForms.ReportParameter("NVR", Localization["EventResultPanel_NVR"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Device", Localization["EventResultPanel_Device"]),
				new Microsoft.Reporting.WinForms.ReportParameter("DateTime", Localization["EventResultPanel_DateTime"]),
				new Microsoft.Reporting.WinForms.ReportParameter("Event", Localization["EventResultPanel_Event"]),
			});
        }

        public void Exit()
        {
            Application.Exit();
        }

        //------------------------------------------------------------------------------------------------------

        public String SaveAsPDF(String path)
        {
            return SaveAs(path, "PDF");
        }

        public String SaveAsDoc(String path)
        {
            return SaveAs(path, "Word");
        }

        public String SaveAsExcel(String path)
        {
            return SaveAs(path, "Excel");
        }

        public String SaveAs(String path, String type) //D:\\
        {
            if ((path[path.Length - 1] != '\\'))
                path += "\\";

            Microsoft.Reporting.WinForms.Warning[] warnings;
            String[] streamids;
            String mimeType;
            String encoding;
            String extension;
            var bytes = LocalReport.Render(type, null, out mimeType, out encoding, out extension, out streamids, out warnings);

            var filaName = path + (Tag ?? Name) + @"_" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss.fff", CultureInfo.InvariantCulture);
            switch (type)
            {
                case "PDF":
                    filaName += @".pdf";
                    break;

                case "Word":
                    filaName += @".doc";
                    break;

                case "Excel":
                    filaName += @".xls";
                    break;
            }
            var fs = new FileStream(filaName, FileMode.Create);

            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            return filaName;
        }
    }
}