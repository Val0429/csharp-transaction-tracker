using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace PrintImageForm
{
    public partial class PrintEvidence : Form
    {
        public IServer NVR;
        public List<ICamera> PrintDevices = new List<ICamera>();
        public List<String> PrintComments = new List<String>();
        public Dictionary<UInt16, ICamera> PrintDevicesQueue = new Dictionary<UInt16, ICamera>();
        public Dictionary<ICamera, Image> PrintImages = new Dictionary<ICamera, Image>();
        public DateTime DateTime;

        public Dictionary<String, String> Localization;
        private static readonly Image _noImage = Resources.GetResources(Properties.Resources.no_image, Properties.Resources.IMGNoImage);

        public String Transaction
        {
            get { return transactionRichTextBox.Text; }
            set { transactionRichTextBox.Text = value; }
        }

        public String Keyword { get; set; }

        public PrintEvidence()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Common_Cancel", "Cancel"},
								   {"Common_Done", "Done"},
								   
								   {"MessageBox_Error", "Error"},
								   {"MessageBox_Information", "Information"},

								   {"PrintImage_Title", "Print Image"},
								   {"PrintImage_Loading", "Loading..."},
								   {"PrintImage_Include", "Include"},
								   {"PrintImage_WriteComment", "Enter Comment Here."},
								   {"PrintImage_PrintInformation", "Print Device's Information and Comment"},
								   {"PrintImage_OverlayInformation", "Overlay Device's Information On Image"},
								   {"PrintImage_Print", "Print"},
								   {"PrintImage_PrintText", "User : %1, Server : %2, Print Time : %3"},

								   {"PrintImage_ErrorCccurred", "An error cccurred while printing."},
							   };
            Localizations.Update(Localization);

            InitializeComponent();

            BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.IMGControllerBG);

            doneButton.Location = cancelButton.Location;
            doneButton.Visible = false;

            printImageLabel1.BackgroundImageLayout = ImageLayout.Stretch;
            Text = Localization["PrintImage_Title"];
            printImageLabel1.Text = Localization["PrintImage_Loading"];
            commentTextBox.Text = Localization["PrintImage_WriteComment"];
            printButton.Text = Localization["PrintImage_Print"];
            cancelButton.Text = Localization["Common_Cancel"];
            doneButton.Text = Localization["Common_Done"];
        }

        private Int32 _currentlyPage = 1;
        public void Initialize()
        {
            PrintImages.Clear();
            PrintComments.Clear();
            PrintDevicesQueue.Clear();

            printButton.Enabled = false;

            printButton.Visible = cancelButton.Visible = true;
            doneButton.Visible = false;

            printImageLabel1.Text = Localization["PrintImage_Loading"];
            //nameLabel.Text = PrintDevices[0].ToString();

            _currentlyPage = 1;
            printImageLabel1.BackgroundImage = null;

            datetimeLabel.Text = DateTime.ToString("MM-dd-yyyy  HH:mm:ss", CultureInfo.InvariantCulture);

            foreach (ICamera camera in PrintDevices)
            {
                PrintComments.Add("");
                PrintImages.Add(camera, null);
            }
        }

        private int _loadImageCount;
        public void LoadImage()
        {
            UInt16 index = 0;
            _loadImageCount = 0;

            UInt64 timecode = DateTimes.ToUtc(DateTime, NVR.Server.TimeZone);
            foreach (KeyValuePair<ICamera, Image> obj in PrintImages)
            {
                if (PrintDevicesQueue.ContainsValue(obj.Key)) continue;

                PrintDevicesQueue.Add(index++, obj.Key);

                LoadImageByDeviceDelegate loadImageByDeviceDelegate = LoadImageByDevice;
                loadImageByDeviceDelegate.BeginInvoke(obj.Key, timecode, LoadImageCallback, loadImageByDeviceDelegate);

                _loadImageCount++;
            }
        }

        private void LoadImageByDevice(ICamera camera, UInt64 timecode)
        {
            if (PrintImages[camera] != null) return;

            Image image = camera.GetSnapshot(timecode);

            if (image == null)
            {
                for (UInt16 index = 0; index < PrintDevicesQueue.Count; index++)
                {
                    if (PrintDevicesQueue[index] == camera)
                        PrintDevicesQueue[index] = null;
                }
            }

            PrintImages[camera] = image ?? _noImage;
        }

        private delegate void LoadImageByDeviceDelegate(ICamera camera, UInt64 timecode);
        private delegate void LoadImageCallbackDelegate(IAsyncResult result);
        private void LoadImageCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                _loadImageCount--;
                if (_loadImageCount != 0) return;

                Invoke(new LoadImageCallbackDelegate(LoadImageCallback), result);
                return;
            }
            ((LoadImageByDeviceDelegate)result.AsyncState).EndInvoke(result);

            if (PrintImages.ContainsValue(null))
                return;

            printButton.Enabled = commentTextBox.Enabled = true;

            SetCurrentPage();
        }

        private void CancelButtonClick(Object sender, EventArgs e)
        {
            Hide();
        }

        private void PrintButtonClick(Object sender, EventArgs e)
        {
            Boolean hasDevice = false;

            foreach (KeyValuePair<UInt16, ICamera> obj in PrintDevicesQueue)
            {
                if (obj.Value != null)
                {
                    hasDevice = true;
                    break;
                }
            }

            if (!hasDevice) return;

            PrintDocument doc = new PrintDocument();

            PrintDialog printDialog = new PrintDialog
            {
                Document = doc
            };

            if (IntPtr.Size == 8)
                printDialog.UseEXDialog = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (KeyValuePair<UInt16, ICamera> obj in PrintDevicesQueue)
                    {
                        if (obj.Value == null) continue;

                        doc.DocumentName = obj.Value.ToString();
                        break;
                    }

                    doc.PrintPage += PrintPage;
                    doc.EndPrint += DocEndPrint;
                    doc.Print();
                }
                catch (Exception)
                {
                    TopMostMessageBox.Show(Localization["PrintImage_ErrorCccurred"], Localization["MessageBox_Error"],
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void DocEndPrint(Object sender, PrintEventArgs e)
        {
            EndPrintDelegate endPrintDelegate = EndPrint;
            endPrintDelegate.BeginInvoke(null, null);
            //EndPrint();
        }

        private delegate void EndPrintDelegate();
        private void EndPrint()
        {
            if (InvokeRequired)
            {
                Invoke(new EndPrintDelegate(EndPrint));
                return;
            }

            printButton.Visible = cancelButton.Visible = false;
            doneButton.Visible = true;

            Focus();
            BringToFront();
        }

        public void ClosingPrintImageForm(Object sender, EventArgs e)
        {
            Hide();
            Dispose();
        }

        private GraphicsUnit _units = GraphicsUnit.Pixel;
        private readonly Font _nameFont = new Font("Arial", 18F, FontStyle.Regular);
        private readonly Font _timeFont = new Font("Arial", 12F, FontStyle.Regular);


        private readonly SolidBrush _whiteBrushes = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
        private readonly SolidBrush _blackBrushes = new SolidBrush(Color.Black);

        private readonly Font _transFont = new Font("Lucida Console", 8F, FontStyle.Regular);
        private readonly SolidBrush _transBrushes = new SolidBrush(Color.FromArgb(128, 231, 237, 249));

        private void PrintPage(Object sender, PrintPageEventArgs e)
        {
            UInt16 index = 0;
            //for (UInt16 i = 0; i < PrintDevicesQueue.Count; i++)
            //{
            //    if (PrintDevicesQueue[i] == null) continue;

            //    camera = PrintDevicesQueue[i];
            //    PrintDevicesQueue[i] = null;
            //    index = i;
            //    break;
            //}

            //if (camera == null) return;

            Graphics g = e.Graphics;
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            var name = "";
            Font title = new Font("Arial", 10F, FontStyle.Bold);
            Font label = new Font("Arial", 10F, FontStyle.Regular);

            var mw = e.MarginBounds.Width;
            float fh = g.MeasureString("agh", title).Height;

            name = "Incident Report";
            String reporter = Localization["PrintImage_PrintText"].Replace("%1", NVR.Credential.UserName)
                        .Replace("%2", NVR.Credential.Domain)
                        .Replace("%3", DateTime.Now.ToString("MM-dd-yyyy  HH:mm:ss", CultureInfo.InvariantCulture));

            SizeF fSize = g.MeasureString(name, new Font("Arial", 32F, FontStyle.Regular));
            SizeF dSize = g.MeasureString(reporter, _timeFont);

            g.DrawString(name, new Font("Arial", 32F, FontStyle.Regular), Brushes.Black, x, y);
            y += fSize.Height;

            g.DrawString(reporter, _timeFont, Brushes.Black, x + e.MarginBounds.Width - dSize.Width, y);
            y += dSize.Height;

            g.FillRectangle(_blackBrushes, x, y, e.MarginBounds.Width, 3);
            y += 13;

            SizeF cSize = g.MeasureString(PrintComments[index], _nameFont);

            RectangleF commentRect = new RectangleF(x, y, e.MarginBounds.Width, Math.Min(300, cSize.Height));
            e.Graphics.DrawString(PrintComments[index], _timeFont, Brushes.Black, commentRect);
            //e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(commentRect));
            y += Math.Min(300, cSize.Height);

            y += 20;

            name = "Transaction Information";
            fSize = g.MeasureString(name, _nameFont);
            g.DrawString(name, _nameFont, Brushes.Black, x, y);
            y += fSize.Height;

            g.FillRectangle(_blackBrushes, x, y, e.MarginBounds.Width, 3);
            y += 13;

            g.DrawString("POS Register : ", title, Brushes.Black, x, y);
            g.DrawString(" 001", label, Brushes.Black, x + 120, y + 1);

            g.DrawString("Transaction Date : ", title, Brushes.Black, x + (mw / 2), y);
            g.DrawString(" 2012/05/30 17:01:32", label, Brushes.Black, x + (mw / 2) + 150, y + 1);
            y += (fh + 1);

            g.DrawString("Casher Number : ", title, Brushes.Black, x, y);
            g.DrawString(" I-0001", label, Brushes.Black, x + 120, y + 1);

            g.DrawString("Transaction Number : ", title, Brushes.Black, x + (mw / 2), y);
            g.DrawString(" $123.23", label, Brushes.Black, x + (mw / 2) + 150, y + 1);

            y += (fh + 20);

            name = "Detail";
            fSize = g.MeasureString(name, _nameFont);

            g.DrawString(name, _nameFont, Brushes.Black, x, y);
            y += fSize.Height;

            g.FillRectangle(_blackBrushes, x, y, e.MarginBounds.Width, 3);
            y += 13;

            fh = g.MeasureString("agh", _transFont).Height;
            var mh = transactionRichTextBox.Lines.Count() * (fh + 1);

            g.FillRectangle(_transBrushes, x, y, 308, mh + 5);

            var y1 = y + 1;
            foreach (var line in transactionRichTextBox.Lines)
            {
                if (line.IndexOf(Keyword) >= 1)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(128, 171, 171, 171)), x, y1, 308, fh + 1);
                }

                g.DrawString(line, _transFont, Brushes.Black, x, y1);
                y1 += (fh + 1);
            }

            var x1 = x + 310;
            mw = e.MarginBounds.Width - 310;

            if (_layout <= 1)
                g.DrawImage(PrintImages[PrintDevicesQueue[0]], x1, y, mw, mh);
            else if (_layout == 2)
            {
                g.DrawImage(PrintImages[PrintDevicesQueue[0]], x1, y, mw, mh / 2);
                g.DrawImage(PrintImages[PrintDevicesQueue[1]], x1, y + (mh / 2), mw, mh / 2);
            }
            else if (_layout == 4)
            {
                g.DrawImage(PrintImages[PrintDevicesQueue[0]], x1, y, mw / 2, mh / 2);
                g.DrawImage(PrintImages[PrintDevicesQueue[1]], x1 + (mw / 2), y, mw / 2, mh / 2);
                g.DrawImage(PrintImages[PrintDevicesQueue[2]], x1, y + (mh / 2), mw / 2, mh / 2);
                g.DrawImage(PrintImages[PrintDevicesQueue[3]], x1 + (mw / 2), y + (mh / 2), mw / 2, mh / 2);
            }


            e.HasMorePages = false;
        }

        private int _layout;
        private void SetCurrentPage()
        {
            _layout = 0;
            for (int i = 0; i < 4; i++)
            {
                if (PrintDevices.Count > i)
                {
                    var label = Controls.Find("printImageLabel" + (i + 1), true)[0];

                    label.Text = "";
                    label.BackgroundImage = PrintImages[PrintDevices[i]];
                    label.BackgroundImageLayout = (label.BackgroundImage == _noImage) ? ImageLayout.Center : ImageLayout.Stretch;

                    _layout++;
                }
            }

            var w = VideoFlowPanel.Size.Width;
            var h = VideoFlowPanel.Size.Height;

            if (_layout <= 1)
                printImageLabel1.Size = VideoFlowPanel.Size;
            else if (_layout == 2)
            {
                printImageLabel1.Size = new Size(w / 2, h);
                printImageLabel2.Size = new Size(w / 2, h);
            }
            else
            {
                printImageLabel1.Size = new Size(w / 2, h / 2);
                printImageLabel2.Size = new Size(w / 2, h / 2);
                printImageLabel3.Size = new Size(w / 2, h / 2);
                printImageLabel4.Size = new Size(w / 2, h / 2);
            }

            commentTextBox.TextChanged -= CommentTextBoxTextChanged;

            commentTextBox.Text = Localization["PrintImage_WriteComment"];

            commentTextBox.TextChanged += CommentTextBoxTextChanged;
        }

        private void CommentTextBoxEnter(Object sender, EventArgs e)
        {
            if (PrintComments[_currentlyPage - 1] == "")
            {
                commentTextBox.TextChanged -= CommentTextBoxTextChanged;

                commentTextBox.Text = "";

                commentTextBox.TextChanged += CommentTextBoxTextChanged;
            }
        }

        private void CommentTextBoxTextChanged(Object sender, EventArgs e)
        {
            PrintComments[_currentlyPage - 1] = commentTextBox.Text;
        }

        private void DoneButtonClick(Object sender, EventArgs e)
        {
            printButton.Visible = cancelButton.Visible = true;
            doneButton.Visible = false;

            Hide();
        }
    }
}
