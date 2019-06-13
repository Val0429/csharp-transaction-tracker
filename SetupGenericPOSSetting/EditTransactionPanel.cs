using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;
using POSException;
using Manager = SetupBase.Manager;

namespace SetupGenericPOSSetting
{
    public sealed partial class EditTransactionPanel : UserControl
    {
        public IPOSConnection GenericPosSetting;
        public POS_Exception POSSettingException;
        public POS_Exception POSException = new POS_Exception();
        public IPTS PTS;
        public Dictionary<String, String> Localization;

        public Boolean IsEditing;

        public EditTransactionPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   {"POSException_ExportCompleted", "Export setting successfully."},
                                   {"POSException_Name", "Name"},
                                   {"POSException_Separator", "Separator"},
                                   {"POSException_Import", "Import Transaction"},

                                   {"POS_Exception", "Exception"},
                                   {"POS_Segment", "Segment"},
                                   {"POS_Tag", "Tag"},
                                   {"POS_AddNewException", "Add New Exception"},
                                   {"POS_AddNewTag", "Add New Tag"},
                                   {"SetupServer_NoSelectedFile", "No selected file"},
                                   {"EditPOSExceptionPanel_Information", "Information"},
                                   {"POSException_NameIsEmpty", "Please input the name."},
                                   {"POSException_SeparatorIsEmpty", "Please input the separator."},

                                    {"POSConnection_Transaction", "Transaction"},
                                    {"POSConnection_ExportCompleted", "Export transactions successfully."},
                                    {"POSConnection_ImportTransaction", "Import Transaction"},
                                   {"POSConnection_Name", "Name"},
                                   {"POSConnection_Protocol", "Protocol"},
                                   {"POSConnection_NetworkAddress", "Network Address"},
                                   {"POSConnection_Port", "Port"},
                                   {"GroupPanel_Import", "Import"},

                                   {"POSException_CaptureStart", "Start"},
                                   {"POSException_CaptureStop", "Stop"},
                                   {"POSException_CaptureClear", "Clear"},
                                   {"POSException_CaptureExport", "Export"},

                                   {"POSException_AddNewException", "Add New Exception"},
                                   {"POSException_AddNewTag", "Add New Tag"},

                                   {"EditPOSConnectionPanel_AcceptPortCantEmpty", "Accept port can't be empty."},
                                   {"EditPOSConnectionPanel_AcceptPortOutOfRange", "Accept port value must be 1 - 65535."},
                                   {"EditPOSConnectionPanel_AcceptPortCantTheSame", "Accept port can't be the same. %1 is used by %2."},
                                   
                                   {"EditPOSConnectionPanel_ConnectPortCantEmpty", "Connect port can't be empty."},
                                   {"EditPOSConnectionPanel_ConnectPortOutOfRange", "Connect port value must be 1 - 65535."},
                                   {"EditPOSConnectionPanel_ConnectPortCantTheSame", "Connect port can't be the same. %1 is used by %2."},

                                   {"POS_ExceptionLoadFail", "Invalid format - %1. Please check the file."},
                               };
            Localizations.Update(Localization);
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            informationLabel.Text = Localization["EditPOSExceptionPanel_Information"];
            exceptionLabel.Text = Localization["POS_Exception"];
            segmentLabel.Text = Localization["POSConnection_Transaction"];
            tagLabel.Text = Localization["POS_Tag"];

            //nameTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            BackgroundImage = Manager.BackgroundNoBorder;
         
        }

        public void Initialize()
        {
            importPanel.Paint += PaintInputTop;
            namePanel.Paint += PaintInputMiddle;
            separatorDoubleBufferPanel.Paint += PaintInputBottom;
            addNewExceptionDoubleBufferPanel.Paint += InputPanelPaint;
            addNewTagDoubleBufferPanel.Paint += InputPanelPaint;
            nameTextBox.TextChanged += NameTextBoxTextChanged;
            separatorTextBox.TextChanged += SeparatorTextBoxTextChanged;
            separatorTextBox.LostFocus += SeparatorTextBoxLostFocus;
            FileName = Localization["SetupServer_NoSelectedFile"];
            importButton.Text = Localization["GroupPanel_Import"];
            importTransactionButton.Text = Localization["GroupPanel_Import"];
            _importFile = Localization["SetupServer_NoSelectedFile"];

            startButton.Text = Localization["POSException_CaptureStart"];
            stopButton.Text = Localization["POSException_CaptureStop"];
            clearButton.Text = Localization["POSException_CaptureClear"];
            exportButton.Text = Localization["POSException_CaptureExport"];

            addNewExceptionDoubleBufferPanel.Click += AddNewExceptionDoubleBufferPanelClick;
            addNewTagDoubleBufferPanel.Click += AddNewTagDoubleBufferPanelClick;
            POSException = new POS_Exception();
            POSException.Manufacture = "Generic";
            POS_Exception.SetDefaultExceptions(POSException);
            POS_Exception.SetDefaultSegments(POSException);
            POS_Exception.SetDefaultTags(POSException);

            infoLabel.Text = Localization["POSConnection_Transaction"];
            importTransactionPanel.Paint += PaintImportInput;
            protocolPanel.Paint += PaintInput;
            networkAddressPanel.Paint += PaintInput;
            portPanel.Paint += PaintInput;
            acceptPortTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            protocolComboBox.Items.Add("UDP");
            protocolComboBox.Items.Add("TCP");
            protocolComboBox.SelectedIndex = 0;

            ipAddressTextBox.TextChanged += IPAddressTextBoxTextChanged;
            acceptPortTextBox.LostFocus += AcceptPortTextBoxLostFocus;
            protocolComboBox.SelectedIndexChanged += ProtocolComboBoxSelectedIndexChanged;

            if (PTS != null)
            {
                PTS.POS.OnSaveComplete -= POSOnSaveComplete;
                PTS.POS.OnSaveComplete += POSOnSaveComplete;
                PTS.OnPOSEventReceive -= POSEventReceive;
                PTS.OnPOSEventReceive += POSEventReceive;
            }

            ClearFolder();
        }

        private static String FilePath = String.Format(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("SetupGenericPOSSetting.dll", "")) + "\\" + "collection";
        private void ClearFolder()
        {
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(FilePath))
                {
                    // Try to create the directory.
                    Directory.CreateDirectory(FilePath);
                }
                else
                {
                    foreach (FileInfo file in new DirectoryInfo(FilePath).GetFiles())
                    {
                        file.Delete();
                    }
                    Directory.Delete(FilePath);
                    Directory.CreateDirectory(FilePath);
                }
            }
            catch (Exception)
            {
            }
        }

        public void POSConnectionIsModify()
        {
            if (GenericPosSetting == null) return;
            if (GenericPosSetting.ReadyState == ReadyState.Ready)
                GenericPosSetting.ReadyState = ReadyState.Modify;
        }

        private void IPAddressTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            if (GenericPosSetting == null) return;
            GenericPosSetting.Authentication.Domain = ipAddressTextBox.Text;
            POSConnectionIsModify();
        }

        private void AcceptPortTextBoxLostFocus(Object sender, EventArgs e)
        {
            if (GenericPosSetting == null) return;
            AcceptPortTextBoxLostFocus();
        }

        private Boolean AcceptPortTextBoxLostFocus()
        {
            //if (!IsEditing) return ;
            if (GenericPosSetting == null) return false;
            bool pass = true;
            UInt32 port = 0;
            if (String.IsNullOrEmpty(acceptPortTextBox.Text))
            {
                TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_AcceptPortCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                pass = false;
            }
            else
            {
                port = Convert.ToUInt32(acceptPortTextBox.Text);

                if (port < 1 || port > 65535)
                {
                    TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_AcceptPortOutOfRange"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pass = false;
                }
                else
                {
                    foreach (var obj in PTS.POS.Connections)
                    {
                        var connection = obj.Value;
                        if (connection == GenericPosSetting) continue;
                        if (connection.AcceptPort != port) continue;

                        TopMostMessageBox.Show(Localization["EditPOSConnectionPanel_AcceptPortCantTheSame"].
                            Replace("%1", connection.AcceptPort.ToString()).Replace("%2", connection.ToString()),
                            Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                        pass = false;
                        break;
                    }
                }
            }

            if (pass)
            {
                acceptPortTextBox.BackColor = Color.White;
                GenericPosSetting.AcceptPort = Convert.ToUInt16(Math.Min(Math.Max(port, 1), 65535));
                POSConnectionIsModify();
            }
            else
            {
                if (GenericPosSetting.AcceptPort == 0)
                    acceptPortTextBox.Text = "";
                else
                    acceptPortTextBox.Text = GenericPosSetting.AcceptPort.ToString();
                acceptPortTextBox.BackColor = Color.FromArgb(223, 173, 183);
                acceptPortTextBox.Focus();
            }

            return pass;
        }

        private void ProtocolComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;
            if (GenericPosSetting == null) return;
            GenericPosSetting.Protocol = protocolComboBox.SelectedItem.ToString();

            POSConnectionIsModify();
        }

        private void AddNewTagDoubleBufferPanelClick(object sender, EventArgs e)
        {
            TagPanel tagPanel = GetTagPanel();

            tagPanel.Name = String.Empty;
            tagPanel.Tag = new POS_Exception.Tag();
            tagPanel.IsTitle = false;
            tagPanel.OnSelectChange += TagPanelOnSelectChange;
            tagContainerPanel.Controls.Add(tagPanel);
            tagPanel.BringToFront();
        }

        private void AddNewExceptionDoubleBufferPanelClick(object sender, EventArgs e)
        {
            ExceptionPanel exceptionPanel = GetExceptionPanel();

            exceptionPanel.Name = String.Empty;
            exceptionPanel.Exception = new POS_Exception.Exception();
            exceptionPanel.IsTitle = false;
            exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;
            exceptionContainerPanel.Controls.Add(exceptionPanel);
            exceptionPanel.BringToFront();
        }

        private String FileName;
        public void PaintInputTop(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, importPanel);

            Manager.PaintText(g, FileName);
        }

        public void PaintInputMiddle(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintMiddle(g, namePanel);

            Manager.PaintText(g, Localization["POSException_Name"]);
        }

        public void PaintInputBottom(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintBottom(g, namePanel);

            Manager.PaintText(g, Localization["POSException_Separator"]);
        }

        private void InputPanelPaint(Object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;
            var panel = sender as DoubleBufferPanel;
            Manager.PaintHighLightInput(g, panel);
            Manager.PaintEdit(g, panel);

            if (Localization.ContainsKey("POSException_" + control.Tag))
                Manager.PaintText(g, Localization["POSException_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        private String _importFile;
        public void PaintImportInput(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, importTransactionPanel);

            Manager.PaintText(g, Localization["POSConnection_ImportTransaction"]);
        }

        public void PaintInput(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.Paint(g, (Control)sender);

            if (Localization.ContainsKey("POSConnection_" + ((Control)sender).Tag))
                Manager.PaintText(g, Localization["POSConnection_" + ((Control)sender).Tag]);
            else
                Manager.PaintText(g, ((Control)sender).Tag.ToString());
        }

        public void ParsePOSConnection()
        {
            if (GenericPosSetting == null) return;

            IsEditing = false;

            ipAddressTextBox.Text = GenericPosSetting.Authentication.Domain;

            if (GenericPosSetting.AcceptPort == 0)
                acceptPortTextBox.Text = "";
            else
                acceptPortTextBox.Text = GenericPosSetting.AcceptPort.ToString();

            protocolComboBox.SelectedItem = GenericPosSetting.Protocol;

            IsEditing = true;
        }

        public void ParsePOSException(Boolean isImport = false)
        {
            IsEditing = false;
            ParsePOSConnection();
            _separator = String.Empty;
            if (!isImport)
            {
                FileName = Localization["SetupServer_NoSelectedFile"];
            }
            
            nameTextBox.Text = POSException.Name;
            separatorTextBox.Text = POSException.Separator;
            Visible = false;

            containerPanel.AutoScroll = false;
            ClearViewModel();
            
            //--------------------------------------------------------------------------------
            foreach (var exception in POSException.Exceptions)
            {
                ExceptionPanel exceptionPanel = GetExceptionPanel();
                exception.Editable = true;
                exceptionPanel.Name = exception.Key;
                exceptionPanel.Exception = exception;
                exceptionPanel.Checked = true;
                exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;
                exceptionContainerPanel.Controls.Add(exceptionPanel);
            }

            exceptionLabel.Visible = POSException.Exceptions.Count > 0;
            ExceptionPanel exceptionTitlePanel = GetExceptionPanel();
            exceptionTitlePanel.IsTitle = true;
            exceptionTitlePanel.Cursor = Cursors.Default;
            exceptionTitlePanel.EditVisible = false;
            exceptionTitlePanel.OnSelectAll += ExceptionPanelOnSelectAll;
            exceptionTitlePanel.OnSelectNone += ExceptionPanelOnSelectNone;
            exceptionContainerPanel.Controls.Add(exceptionTitlePanel);
            //--------------------------------------------------------------------------------
            if (POSException.Segments.Count == 0)
            {
                segmentLabel.Visible = segmentContainerPanel.Visible = false;
            }
            else
            {
                segmentLabel.Visible = segmentContainerPanel.Visible = true;
            }

            POSException.Segments.Sort((x, y) => (y.Key.CompareTo(x.Key)));
            foreach (POS_Exception.Segment segment in POSException.Segments)
            {
                SegmentPanel segmentPanel = GetSegmentPanel();
                segment.Editable = true;
                segmentPanel.Name = segment.Key;
                segmentPanel.Segment = segment;
                segmentPanel.Checked = true;
                segmentPanel.OnSelectChange += TagPanelOnSelectChange;
                segmentContainerPanel.Controls.Add(segmentPanel);
            }

            SegmentPanel segmentsTitlePanel = GetSegmentPanel();
            segmentsTitlePanel.IsTitle = true;
            segmentsTitlePanel.Cursor = Cursors.Default;
            segmentsTitlePanel.EditVisible = false;
            //segmentsTitlePanel.OnSelectAll += SegmentPanelOnSelectAll;
            //segmentsTitlePanel.OnSelectNone += SegmentPanelOnSelectNone;
            segmentContainerPanel.Controls.Add(segmentsTitlePanel);
            //--------------------------------------------------------------------------------
            foreach (var tag in POSException.Tags)
            {
                TagPanel tagPanel = GetTagPanel();
                tag.Editable = true;
                tagPanel.Name = tag.Key;
                tagPanel.Tag = tag;
                tagPanel.Checked = true;
                tagPanel.OnSelectChange += TagPanelOnSelectChange;
                tagContainerPanel.Controls.Add(tagPanel);
            }

            TagPanel tagTitlePanel = GetTagPanel();
            tagTitlePanel.IsTitle = true;
            tagTitlePanel.Cursor = Cursors.Default;
            tagTitlePanel.EditVisible = false;
            tagTitlePanel.OnSelectAll += TagPanelOnSelectAll;
            tagTitlePanel.OnSelectNone += TagPanelOnSelectNone;
            tagContainerPanel.Controls.Add(tagTitlePanel);
            //--------------------------------------------------------------------------------

            containerPanel.AutoScroll = true;
            Visible = true;
            containerPanel.AutoScrollPosition = new Point(0, 0);
            containerPanel.Select();

            ReplaceTransactionToNewLine();
            IsEditing = true;
        }

        private void ExceptionPanelOnSelectChange(object sender, EventArgs e)
        {
            var panel = sender as ExceptionPanel;
            if (panel == null) return;

            if(panel.Checked)
            {
                if(!POSException.Exceptions.Contains(panel.Exception))
                {
                    POSException.Exceptions.Add(panel.Exception);
                }
            }
            else
            {
                if (POSException.Exceptions.Contains(panel.Exception))
                {
                    POSException.Exceptions.Remove(panel.Exception);
                }
            }
        }

        private void ExceptionPanelOnSelectNone(object sender, EventArgs e)
        {
            foreach (ExceptionPanel exceptionPanel in exceptionContainerPanel.Controls)
            {
                if (exceptionPanel.IsTitle)
                {
                    continue;
                }

                exceptionPanel.Checked = false;
                ExceptionPanelOnSelectChange(exceptionPanel, null);
            }
        }

        private void ExceptionPanelOnSelectAll(object sender, EventArgs e)
        {
            foreach (ExceptionPanel exceptionPanel in exceptionContainerPanel.Controls)
            {
                if (exceptionPanel.IsTitle)
                {
                    continue;
                }

                exceptionPanel.Checked = true;
                ExceptionPanelOnSelectChange(exceptionPanel, null);
            }
        }

        private void TagPanelOnSelectChange(object sender, EventArgs e)
        {
            var panel = sender as TagPanel;
            if (panel == null) return;

            if (panel.Checked)
            {
                if (!POSException.Tags.Contains(panel.Tag))
                {
                    POSException.Tags.Add(panel.Tag);
                }
            }
            else
            {
                if (POSException.Tags.Contains(panel.Tag))
                {
                    POSException.Tags.Remove(panel.Tag);
                }
            }
        }

        private void TagPanelOnSelectNone(object sender, EventArgs e)
        {
            foreach (TagPanel tagPanel in tagContainerPanel.Controls)
            {
                if (tagPanel.IsTitle)
                {
                    continue;
                }

                tagPanel.Checked = false;
                TagPanelOnSelectChange(tagPanel, null);
            }
        }

        private void TagPanelOnSelectAll(object sender, EventArgs e)
        {
            foreach (TagPanel tagPanel in tagContainerPanel.Controls)
            {
                if (tagPanel.IsTitle)
                {
                    continue;
                }

                tagPanel.Checked = true;
                TagPanelOnSelectChange(tagPanel, null);
            }
        }

        private readonly Queue<ExceptionPanel> _recycleException = new Queue<ExceptionPanel>();
        private ExceptionPanel GetExceptionPanel()
        {
            if (_recycleException.Count > 0)
            {
                return _recycleException.Dequeue();
            }

            var exceptionPanel = new ExceptionPanel
            {
                SelectedColor = Manager.DeleteTextColor,
            };

            //exceptionPanel.OnExceptionEditClick += ExceptionPanelOnExceptionEditClick;
            //exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;

            return exceptionPanel;
        }

        private readonly Queue<SegmentPanel> _recycleSegment = new Queue<SegmentPanel>();
        private SegmentPanel GetSegmentPanel()
        {
            if (_recycleSegment.Count > 0)
            {
                return _recycleSegment.Dequeue();
            }

            var segmentPanel = new SegmentPanel
            {
                SelectedColor = Manager.DeleteTextColor,
            };

            //exceptionPanel.OnExceptionEditClick += ExceptionPanelOnExceptionEditClick;
            //exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;

            return segmentPanel;
        }

        private readonly Queue<TagPanel> _recycleTag = new Queue<TagPanel>();
        private TagPanel GetTagPanel()
        {
            if (_recycleTag.Count > 0)
            {
                return _recycleTag.Dequeue();
            }

            var tagPanel = new TagPanel
            {
                SelectedColor = Manager.DeleteTextColor,
            };

            //exceptionPanel.OnExceptionEditClick += ExceptionPanelOnExceptionEditClick;
            //exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;

            return tagPanel;
        }

        private void ClearViewModel()
        {
            foreach (ExceptionPanel exceptionPanel in exceptionContainerPanel.Controls)
            {
                exceptionPanel.Exception = null;
                //exceptionPanel.Cursor = Cursors.Hand;
                exceptionPanel.Checked = false;
                if (exceptionPanel.IsTitle)
                {
                    exceptionPanel.OnSelectAll -= ExceptionPanelOnSelectAll;
                    exceptionPanel.OnSelectNone -= ExceptionPanelOnSelectNone;
                    exceptionPanel.IsTitle = false;
                }

                exceptionPanel.OnSelectChange -= ExceptionPanelOnSelectChange;

                if (!_recycleException.Contains(exceptionPanel))
                {
                    _recycleException.Enqueue(exceptionPanel);
                }
            }
            exceptionContainerPanel.Controls.Clear();

            foreach (SegmentPanel segmentPanel in segmentContainerPanel.Controls)
            {
                segmentPanel.Tag = null;
                //tagPanel.Cursor = Cursors.Hand;

                if (segmentPanel.IsTitle)
                {
                    //tagPanel.OnSelectAll -= POSExceptionPanelOnSelectAll;
                    //tagPanel.OnSelectNone -= POSExceptionPanelOnSelectNone;
                    segmentPanel.OnSelectAll -= TagPanelOnSelectAll;
                    segmentPanel.OnSelectNone -= TagPanelOnSelectNone;
                    segmentPanel.IsTitle = false;
                }

                segmentPanel.OnSelectChange -= TagPanelOnSelectChange;

                if (!_recycleSegment.Contains(segmentPanel))
                {
                    _recycleSegment.Enqueue(segmentPanel);
                }
            }
            segmentContainerPanel.Controls.Clear();

            foreach (TagPanel tagPanel in tagContainerPanel.Controls)
            {
                tagPanel.Tag = null;
                //tagPanel.Cursor = Cursors.Hand;
                tagPanel.Checked = false;
                if (tagPanel.IsTitle)
                {
                    tagPanel.OnSelectAll -= TagPanelOnSelectAll;
                    tagPanel.OnSelectNone -= TagPanelOnSelectNone;
                    tagPanel.IsTitle = false;
                }

                tagPanel.OnSelectChange -= TagPanelOnSelectChange;

                if (!_recycleTag.Contains(tagPanel))
                {
                    _recycleTag.Enqueue(tagPanel);
                }
            }
            tagContainerPanel.Controls.Clear();
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSException.Name = nameTextBox.Text;

            POSExceptionModify();
        }

        private void SeparatorTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSException.Separator = separatorTextBox.Text;

            POSExceptionModify();
        }

        private String _separator;
        private void SeparatorTextBoxLostFocus(object sender, EventArgs e)
        {
            ReplaceTransactionToNewLine();
        }

        private void ReplaceTransactionToNewLine()
        {
            
            try {
               
                if (String.IsNullOrEmpty(transactionTextBox.Text)) return;
            /*
            if (_separator.IndexOf(",") > -1)
            {
                string[] separatorArray = _separator.Split(',');
                for (int i = 0; i <= separatorArray.Length-1; i++) {
                    transactionTextBox.Text = transactionTextBox.Text.Replace("\r\n", separatorArray[i]);
                }
            }
            else {
                transactionTextBox.Text = transactionTextBox.Text.Replace("\r\n", _separator);
            }
            */

            _separator = separatorTextBox.Text;

            if (String.IsNullOrEmpty(_separator)) return;
            if (_separator.IndexOf(",") > -1)
            {
                    string[] separatorArray = _separator.Split(',');
                    for (int i = 0; i <= separatorArray.Length-1; i++)
                    {
                        if (separatorArray[i] == "") break;
                        transactionTextBox.Text = transactionTextBox.Text.Replace(separatorArray[i], "\r\n");
                    }
            }
            else {
                   
                    switch (_separator)
                    {
                        case "\r":
                            transactionTextBox.Text = transactionTextBox.Text.Replace(_separator, "");
                            break;
                        case "\n":
                            transactionTextBox.Text = transactionTextBox.Text.Replace(_separator, "\r\n");
                            break;
                        case "\r\n":
                            transactionTextBox.Text = transactionTextBox.Text.Replace(_separator, "\r\n");
                            break;
                        case "/s":
                            transactionTextBox.Text = transactionTextBox.Text.Replace(_separator, " ");

                            break;
                        default:
                            transactionTextBox.Text = transactionTextBox.Text.Replace(_separator, "\r\n");
                            break;
                    }
                

            }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
        }

        private void ManufactureComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            //POSException.Manufacture = POS_Exception.ToIndex(manufactureComboBox.SelectedItem.ToString());

            POS_Exception.SetDefaultExceptions(POSException);
            POS_Exception.SetDefaultSegments(POSException);
            POS_Exception.SetDefaultTags(POSException);

            foreach (var pos in PTS.POS.POSServer)
            {
                if (pos.Exception != POSException.Id) continue;
                if (pos.Manufacture == POSException.Manufacture) continue;

                pos.Exception = 0;
                if (pos.ReadyState == ReadyState.Ready)
                    pos.ReadyState = ReadyState.Modify;
            }
            ParsePOSException();
            POSExceptionModify();
        }

        private void POSExceptionModify()
        {
            if (POSException.ReadyState != ReadyState.New)
                POSException.ReadyState = ReadyState.Modify;
        }

        public void SaveSetting()
        {
            if(POSException == null) return;
            if(String.IsNullOrEmpty(POSException.Name))
            {
                TopMostMessageBox.Show(Localization["POSException_NameIsEmpty"],
                        Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);

                nameTextBox.Focus();
                return;
            }

            if (String.IsNullOrEmpty(POSException.Separator))
            {
                TopMostMessageBox.Show(Localization["POSException_SeparatorIsEmpty"],
                        Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                separatorTextBox.Focus();
                return;
            }

            saveFileDialog.FileName = String.Format("{0}.xml", POSException.Name);
            saveFileDialog.DefaultExt = ".text"; // Default file extension
            saveFileDialog.Filter = @"XML documents (.xml)|*.xml"; // Filter files by extension
            DialogResult resault = saveFileDialog.ShowDialog();
            
            if (resault == DialogResult.OK)
            {
                XmlDocument xmlDoc = PTS.POS.ParseExceptionToXml(POSException);
                
               
                //XmlWriterSettings xws = new XmlWriterSettings();
                //xws.NewLineHandling = NewLineHandling.Entitize;
                //using (StringWriter sw = new StringWriter())
                //{
                //    using (XmlWriter xw = XmlWriter.Create(sw, xws))
                //    {
                //        xmlDoc.WriteTo(xw);
                //    }
                //    Console.WriteLine(sw.ToString());
                //}

                //xmlDoc.Save(Path.Combine(folderBrowserDialog.SelectedPath, String.Format("{0}.xml", POSException.Name)));

                //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { NewLineHandling = NewLineHandling.None, Indent = true };
                //using (XmlWriter xmlWriter = XmlWriter.Create(Path.Combine(folderBrowserDialog.SelectedPath, String.Format("{0}.xml", POSException.Name)), xmlWriterSettings))
                //{
                //    xmlDoc.Save(xmlWriter);
                //    xmlWriter.Flush();
                //}
                //xmlDoc.Save()

                //File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, String.Format("{0}.xml", POSException.Name)), xmlDoc.OuterXml);
                File.WriteAllText(saveFileDialog.FileName , xmlDoc.OuterXml);
                TopMostMessageBox.Show(Localization["POSException_ExportCompleted"],
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportButtonClick(object sender, EventArgs e)
        {
            DialogResult resault = openFileDialog.ShowDialog();

            if (resault == DialogResult.OK)
            {
                var size = -1;
                String file = openFileDialog.FileName;
                try
                {
                    String text = File.ReadAllText(file);
                    //_importFile = file;
                    Invalidate();
                    var lines = text.Replace("\r", "\\r").Replace("\n", "\\n").Replace(" ", "/s").Replace("\t", "/t").Split(new[] { "</br>" }, StringSplitOptions.None);
                    //transactionListBox.Items.Clear();
                    transactionTextBox.Text = String.Empty;
                    foreach (String line in lines)
                    {
                        //transactionListBox.Items.Add(line);
                        transactionTextBox.Text += line + Environment.NewLine;
                    }

                    ReplaceTransactionToNewLine();
                }
                catch (IOException)
                {
                }
            }
        }
        private Boolean IsPauseReceiveEvent;
        private void POSEventReceive(Object sender, EventArgs<POS_Exception.TransactionItem> e)
        {
            if (IsPauseReceiveEvent) return;
            if (!IsRunning) return;

            AddEvent(e.Value);
        }

        private UInt64 _currentLine = 0;
        private const UInt16 MaximumAmount = 5000;
        private delegate void AddEventDelegate(POS_Exception.TransactionItem transactionItem);
        private void AddEvent(POS_Exception.TransactionItem transactionItem)
        {
            //if (_selectedPOSId != "0" && _selectedPOSId != transactionItem.POS) return;
            if (transactionItem.POS != "PTSDemo") return;
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new AddEventDelegate(AddEvent), transactionItem);
                }
                catch (Exception)
                {
                }
                return;
            }

            var item = new TransactionItemControl2
            {
                POS = PTS.POS.FindPOSById(transactionItem.POS),
                TransactionItem = transactionItem,
                DisplayPOS = true,
            };

            //transactionTextBox.Text += item.ToFileString().Replace("\r", "\\r").Replace("\n", "\\n").Replace(" ", "/s").Replace("\t", "/t") +Environment.NewLine;
            
            if (String.IsNullOrEmpty(_separator))
            {
                //transactionTextBox.AppendText(item.ToFileString().Replace("\r", "\\r").Replace("\n", "\\n").Replace(" ", "/s").Replace("\t", "/t"));
                transactionTextBox.Text +=
                    item.ToFileString().Replace("\r", "\\r").Replace("\n", "\\n").Replace(" ", "/s").Replace("\t", "/t");
            }
            else
            {
                //transactionTextBox.AppendText(item.ToFileString().Replace("\r", "\\r").Replace("\n", "\\n").Replace(" ", "/s").Replace("\t", "/t").Replace(_separator, "\r\n"));
                transactionTextBox.Text +=
                    item.ToFileString().Replace("\r", "\\r").Replace("\n", "\\n").Replace(" ", "/s").Replace("\t", "/t")
                        .Replace(_separator, "\r\n");
            }
            
            WriteLine(item.ToFileString());
            ReplaceTransactionToNewLine();
        }

        private void WriteLine(String line)
        {
            try
            {
                _currentLine++;

                StreamWriter streamWriter = File.AppendText(String.Format("{0}/Collection{1}.txt", FilePath, (_currentLine / MaximumAmount) + 1));
                streamWriter.Write(line);
                streamWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        public Boolean IsRunning;
        private void StartButtonClick(object sender, EventArgs e)
        {
            var check = AcceptPortTextBoxLostFocus();
            if (!check) return;
            //if (IsRunning) return;
            IsRunning = true;
            if (!PTS.POS.Connections.ContainsKey(GenericPosSetting.Id))
            {
                GenericPosSetting.ReadyState = ReadyState.Modify;
                PTS.POS.Connections.Add(GenericPosSetting.Id, GenericPosSetting);
            }

            if (!PTS.POS.Exceptions.ContainsKey(POSSettingException.Id))
            {
                POSSettingException.ReadyState = ReadyState.Modify;
                PTS.POS.Exceptions.Add(POSSettingException.Id, POSSettingException);
            }
            PTS.POS.Save("Generic");
            ApplicationForms.ShowLoadingIcon(PTS.Form);
        }


        private void POSOnSaveComplete(object sender, EventArgs e)
        {
            ApplicationForms.HideLoadingIcon();
        }

        private void PauseButtonClick(object sender, EventArgs e)
        {
            IsPauseReceiveEvent = !IsPauseReceiveEvent;

            if (IsPauseReceiveEvent)
            {
                stopButton.Text = @"Play";
                //_pauseEvent.BackgroundImage = _pauseeventactivate;
                //SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["POSException_ResumeEvent"]);
            }
            else
            {
                stopButton.Text = @"Stop";
                //_pauseEvent.BackgroundImage = _pauseevent;
                //SharedToolTips.SharedToolTip.SetToolTip(_pauseEvent, Localization["POSException_PauseEvent"]);
            }
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            _currentLine = 0;
            //Stop();
            transactionTextBox.Clear();
            ClearFolder();
        }

        private void ExportButtonClick(object sender, EventArgs e)
        {
            DialogResult resault = folderBrowserDialog.ShowDialog();

            if (resault == DialogResult.OK)
            {
                var path = folderBrowserDialog.SelectedPath;
                var folderPath = String.Format(@"{0}\Transaction from {1} port {2} ({3})", path, GenericPosSetting.Authentication.Domain, GenericPosSetting.AcceptPort, DateTime.Now.ToString("MM-dd-yyyy  HH-mm-ss", CultureInfo.InvariantCulture));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (FileInfo file in new DirectoryInfo(FilePath).GetFiles())
                {
                    File.Copy(Path.Combine(FilePath, file.Name), Path.Combine(folderPath, file.Name), true);
                }

                TopMostMessageBox.Show(Localization["POSConnection_ExportCompleted"],
                    Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            if (!IsRunning) return;
            IsRunning = false;
            PTS.POS.Exceptions.Remove(POSSettingException.Id);
            PTS.POS.Connections.Remove(GenericPosSetting.Id);
            PTS.POS.Save("Generic");
            ApplicationForms.ShowLoadingIcon(PTS.Form);
        }

        public void Stop()
        {
            StopButtonClick(this, null);
        }

        public void EnableButtons(Boolean enable)
        {
            ipAddressTextBox.Enabled=
            acceptPortTextBox.Enabled =
            protocolComboBox.Enabled =
            startButton.Enabled =
            clearButton.Enabled =
            exportButton.Enabled =
            stopButton.Enabled = enable;
        }

        private void AddExceptionByFileDoubleBufferPanelMouseClick(object sender, EventArgs e)
        {
            DialogResult resault = openFileDialog.ShowDialog();
            if (resault == DialogResult.OK)
            {
                String file = openFileDialog.FileName;
                String text = String.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream ms = null;
                try
                {
                    FileName = file;
                    text = File.ReadAllText(file);
                    text = text.Replace("\r", "\\r").Replace("\n", "\\n");
                }
                catch (IOException)
                {
                }

                try
                {
                    xmlDoc.LoadXml(text);

                    var node = xmlDoc.GetElementsByTagName("ExceptionConfiguration");
                    if (node.Count > 0)
                    {
                        var newException = PTS.POS.ParserXmlToException(node[0] as XmlElement);
                        newException.Id = PTS.POS.GetNewExceptionId();
                        newException.TransactionType = 1;
                        POSException = newException;

                        ParsePOSException(true);
                    }
                    else
                    {
                        TopMostMessageBox.Show(Localization["POS_ExceptionLoadFail"].Replace("%1", "1"),
                            Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception exception)
                {
                    TopMostMessageBox.Show(Localization["POS_ExceptionLoadFail"].Replace("%1", exception.Message),
                                Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void networkAddressPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
