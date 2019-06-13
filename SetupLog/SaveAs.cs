using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Constant.Utility;
using PanelBase;

namespace SetupLog
{
    public partial class Setup
    {
        private String _defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly SaveFileDialog _saveFileDialog = new SaveFileDialog();
        private void SaveAs()
        {
            _saveFileDialog.InitialDirectory = _defaultPath;
            _saveFileDialog.FileName = DateTime.Now.ToDateString() + " Log";

            _saveFileDialog.Filter = Localization["SetupLog_ExcelFile"] + @"(*.csv) |*.csv|" +
                //Localization["SetupLog_PDFFile"] + @"(*.pdf) |*.pdf|" +
                Localization["SetupLog_HTMLFile"] + @"(*.html) |*.html|" +
                Localization["SetupLog_XmlFile"] + @"(*.xml) |*.xml|" +
                Localization["SetupLog_TextFile"] + @"(*.txt) |*.txt";

            if (_saveFileDialog.ShowDialog() != DialogResult.OK) return;

            var fileInfo = new FileInfo(_saveFileDialog.FileName);
            _defaultPath = fileInfo.DirectoryName;
            //String path = _saveFileDialog.FileName;
            //String[] paths = path.Split('\\');
            //String fileName = paths[paths.Length - 1];
            //String fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
            UInt16 count = 1;

            //if (fileInfo.Extension == ".pdf")
            //{
            //    Document myDoc = new Document(new Rectangle(PageSize.A3));
            //    FileStream fs = new FileStream(_saveFileDialog.FileName, FileMode.Create);
            //    PdfWriter.GetInstance(myDoc, fs);
            //    myDoc.Open();

            //    myDoc.AddTitle(datePicker.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + " Log");//文件標題

            //    if (_showLogTypes.Count == 2)
            //    {
            //        myDoc.Add(new Paragraph(Localization["SetupLog_ID"] + "\t" + Localization["SetupLog_Time"] +
            //            "\t\t" + Localization["SetupLog_System"] + "\t\t\t" + Localization["SetupLog_User"] + "\t\t\t" + Localization["SetupLog_Description"], new Font(iTextSharp.text.Font.FontFamily.COURIER, 12)));

            //        foreach (Log log in _logs)
            //        {
            //            String tab = (log.User.Length < 24) ? new String('\t', Convert.ToUInt16(Math.Ceiling((24 - log.User.Length) / 8.0))) : "\t";

            //            if (log.Type == LogType.Server)
            //                 myDoc.Add(new Paragraph(count++ + "-" + log.DateTime.ToString("HH:mm:ss") + "\t" + log.User + tab + "\t\t\t" + log.Description, new Font(iTextSharp.text.Font.FontFamily.COURIER, 12)));
            //            else
            //                myDoc.Add(new Paragraph(count++ + "-" + log.DateTime.ToString("HH:mm:ss") + "\t\t\t\t" + log.User + tab + log.Description, new Font(iTextSharp.text.Font.FontFamily.COURIER, 12)));

            //        }
            //    }
            //    else
            //    {
            //        myDoc.Add(new Paragraph(Localization["SetupLog_ID"] + "\t" + Localization["SetupLog_Time"] + "\t\t" +
            //            Localization["SetupLog_System"] + "\t\t\t" + Localization["SetupLog_Description"], new Font(iTextSharp.text.Font.FontFamily.COURIER, 12)));

            //        foreach (Log log in _logs)
            //        {
            //            String tab = (log.User.Length < 24) ? new String('\t', Convert.ToUInt16(Math.Ceiling((24 - log.User.Length) / 8.0))) : "\t";
            //            myDoc.Add(new Paragraph(count++ + "-" + log.DateTime.ToString("HH:mm:ss") + "\t" + log.User + tab + log.Description, new Font(iTextSharp.text.Font.FontFamily.COURIER, 12)));
            //        }
            //    }

            //    //myDoc.AddAuthor("einboch");//文件作者
            //    myDoc.Close();
            //    fs.Close();
            //    TopMostMessageBox.Show(Localization["SetupLog_SaveAsComplete"].Replace("%1", fileInfo.Name), Localization["MessageBox_Information"],
            //    MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    return;
            //}

            var sw = new StreamWriter(_saveFileDialog.FileName, false, System.Text.Encoding.UTF8);

            switch (fileInfo.Extension)
            {
                case ".csv":
                    sw.WriteLine(datePicker.Value.ToDateString() + " Log");
                    if (_showLogTypes.Count == 2)
                    {
                        sw.WriteLine(Localization["SetupLog_ID"] + "," + Localization["SetupLog_Time"] + "," + Localization["SetupLog_System"]
                            + "," + Localization["SetupLog_User"] + "," + Localization["SetupLog_Description"]);
                    }
                    else
                    {
                        if (_showLogTypes.Contains(LogType.Server))
                            sw.WriteLine(Localization["SetupLog_ID"] + "," + Localization["SetupLog_Time"] + "," + Localization["SetupLog_System"] + "," + Localization["SetupLog_Description"]);
                        if (_showLogTypes.Contains(LogType.Action))
                            sw.WriteLine(Localization["SetupLog_ID"] + "," + Localization["SetupLog_Time"] + "," + Localization["SetupLog_User"] + "," + Localization["SetupLog_Description"]);
                    }

                    if (_showLogTypes.Count == 2)
                    {
                        foreach (var log in _logs)
                        {
                            if (log.Type == LogType.Server)
                                sw.WriteLine(count++ + "," + log.DateTime.ToTimeString() + "," + log.User + ",," + log.Description);
                            else
                                sw.WriteLine(count++ + "," + log.DateTime.ToTimeString() + ",," + log.User + "," + log.Description);
                        }
                    }
                    else
                    {
                        foreach (var log in _logs)
                        {
                            sw.WriteLine(count++ + "," + log.DateTime.ToTimeString() + "," + log.User + "," + log.Description);
                        }
                    }
                    break;

                case ".html":
                    sw.WriteLine("<table border=1 cellspacing=0 cellpadding=0>");

                    if (_showLogTypes.Count == 2)
                    {
                        sw.WriteLine("<tr><td colspan=5 align=center>" + datePicker.Value.ToDateString() + " Log" + "</td></tr>");
                        sw.WriteLine("<tr><td>" + Localization["SetupLog_ID"] + "</td><td>" + Localization["SetupLog_Time"] + "</td><td>" + Localization["SetupLog_System"]
                            + "</td><td>" + Localization["SetupLog_User"] + "</td><td>" + Localization["SetupLog_Description"] + "</td></tr>");
                    }
                    else
                    {
                        sw.WriteLine("<tr><td colspan=4 align=center>" + datePicker.Value.ToDateString() + " Log" + "</td></tr>");

                        if (_showLogTypes.Contains(LogType.Server))
                            sw.WriteLine("<tr><td>" + Localization["SetupLog_ID"] + "</td><td>" + Localization["SetupLog_Time"] + "</td><td>" + Localization["SetupLog_System"] + "</td><td>" + Localization["SetupLog_Description"] + "</td></tr>");
                        if (_showLogTypes.Contains(LogType.Action))
                            sw.WriteLine("<tr><td>" + Localization["SetupLog_ID"] + "</td><td>" + Localization["SetupLog_Time"] + "</td><td>" + Localization["SetupLog_User"] + "</td><td>" + Localization["SetupLog_Description"] + "</td></tr>");
                    }

                    if (_showLogTypes.Count == 2)
                    {
                        foreach (Log log in _logs)
                        {
                            if (log.Type == LogType.Server)
                                sw.WriteLine("<tr><td>" + count++ + "</td><td>" + log.DateTime.ToTimeString() + "</td><td>" + log.User + "</td><td>&nbsp;</td><td>" + log.Description + "</td></tr>");
                            else
                                sw.WriteLine("<tr><td>" + count++ + "</td><td>" + log.DateTime.ToTimeString() + "</td><td>&nbsp;</td><td>" + log.User + "</td><td>" + log.Description + "</td></tr>");
                        }
                    }
                    else
                    {
                        foreach (Log log in _logs)
                        {
                            sw.WriteLine("<tr><td>" + count++ + "</td><td>" + log.DateTime.ToTimeString() + "</td><td>" + log.User + "</td><td>" + log.Description + "</td></tr>");
                        }
                    }

                    sw.WriteLine("</table>");
                    break;

                case ".xml":
                    var xmldoc = new XmlDocument();
                    var root = xmldoc.CreateElement("Logs");
                    root.SetAttribute("date", datePicker.Value.ToDateString());
                    xmldoc.AppendChild(root);

                    foreach (Log log in _logs)
                    {
                        XmlElement logNode = xmldoc.CreateElement("Log");
                        logNode.SetAttribute("id", count++.ToString());
                        logNode.SetAttribute("time", log.DateTime.ToTimeString());
                        switch (log.Type)
                        {
                            case LogType.Server:
                                logNode.SetAttribute("system", log.User);
                                break;

                            case LogType.Action:
                                logNode.SetAttribute("user", log.User);
                                break;
                        }
                        logNode.InnerText = log.Description;
                        root.AppendChild(logNode);
                    }
                    sw.Write(xmldoc.OuterXml);
                    break;

                default:
                    sw.WriteLine(datePicker.Value.ToDateString() + " Log");

                    if (_showLogTypes.Count == 2)
                    {
                        sw.WriteLine(Localization["SetupLog_ID"] + "\t" + Localization["SetupLog_Time"] +
                            "\t\t" + Localization["SetupLog_System"] + "\t\t\t" + Localization["SetupLog_User"] + "\t\t\t" + Localization["SetupLog_Description"]);
                        foreach (Log log in _logs)
                        {
                            String tab = (log.User.Length < 24) ? new String('\t', Convert.ToUInt16(Math.Ceiling((24 - log.User.Length) / 8.0))) : "\t";

                            if (log.Type == LogType.Server)
                                sw.WriteLine(count++ + "\t" + log.DateTime.ToTimeString() + "\t" + log.User + tab + "\t\t\t" + log.Description);
                            else
                                sw.WriteLine(count++ + "\t" + log.DateTime.ToTimeString() + "\t\t\t\t" + log.User + tab + log.Description);
                        }
                    }
                    else
                    {
                        sw.WriteLine(Localization["SetupLog_ID"] + "\t" + Localization["SetupLog_Time"] + "\t\t" +
                            Localization["SetupLog_System"] + "\t\t\t" + Localization["SetupLog_Description"]);
                        foreach (Log log in _logs)
                        {
                            String tab = (log.User.Length < 24) ? new String('\t', Convert.ToUInt16(Math.Ceiling((24 - log.User.Length) / 8.0))) : "\t";

                            sw.WriteLine(count++ + "\t" + log.DateTime.ToTimeString() + "\t" + log.User + tab + log.Description);
                        }
                    }

                    break;
            }

            sw.Close();

            TopMostMessageBox.Show(Localization["SetupLog_SaveAsComplete"].Replace("%1", fileInfo.Name), Localization["MessageBox_Information"],
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
