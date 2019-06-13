using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using Interface;
using PanelBase;

namespace PrintImageForm
{
    public partial class PrintImage : Form
    {
        private static readonly Image _noImage = Resources.GetResources(Properties.Resources.no_image, Properties.Resources.IMGNoImage);

        private static readonly Image _nextPage = Resources.GetResources(Properties.Resources.nextPage, Properties.Resources.IMGNextPage);

        private static readonly Image _previousPage = Resources.GetResources(Properties.Resources.previousPage, Properties.Resources.IMGPreviousPage);

        protected readonly SolidBrush _blackBrushes = new SolidBrush(Color.Black);
        protected readonly Font _nameFont = new Font("Arial", 24F, FontStyle.Regular);
        private readonly Brush _nameLabelBrush = new SolidBrush(Color.FromArgb(214, 214, 214));
        private readonly Font _nameLabelFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        protected readonly Font _timeFont = new Font("Arial", 12F, FontStyle.Regular);
        protected readonly SolidBrush _whiteBrushes = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
        public DateTime DateTime { get; set; }

        public Dictionary<String, String> Localization { get; set; }
        public List<String> PrintComments = new List<String>();
        public List<ICamera> PrintDevices = new List<ICamera>();
        public Dictionary<UInt16, ICamera> PrintDevicesQueue = new Dictionary<UInt16, ICamera>();
        public Dictionary<ICamera, Image> PrintImages = new Dictionary<ICamera, Image>();
        public IApp App { get; set; }
        public IServer Server { get; set; }

        private Int32 _currentlyPage = 1;
        private String _nameLabelText;
        protected GraphicsUnit _units = GraphicsUnit.Pixel;


        // Constructor
        public PrintImage()
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
            TopMost = true;

            printButton.BackgroundImage = Resources.GetResources(Properties.Resources.printButton,
                Properties.Resources.IMGPrintButton);
            doneButton.BackgroundImage = Resources.GetResources(Properties.Resources.printButton,
                Properties.Resources.IMGPrintButton);
            cancelButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn,
                Properties.Resources.IMGCancelButotn);

            toolPanel.BackgroundImage = Resources.GetResources(Properties.Resources.toolBG,
                Properties.Resources.IMGToolBG);
            BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG,
                Properties.Resources.IMGControllerBG);

            doneButton.Location = printButton.Location;
            doneButton.Visible = false;

            printImageLabel.BackgroundImageLayout = ImageLayout.Stretch; //can't set on design (no property)

            nameLabel.Text = ""; //use paint no set text
            Text = Localization["PrintImage_Title"];
            printImageLabel.Text = Localization["PrintImage_Loading"];
            includeCheckBox.Text = Localization["PrintImage_Include"];
            commentTextBox.Text = Localization["PrintImage_WriteComment"];
            informationCheckBox.Text = Localization["PrintImage_PrintInformation"];
            overlayCheckBox.Text = Localization["PrintImage_OverlayInformation"];
            printButton.Text = Localization["PrintImage_Print"];
            cancelButton.Text = Localization["Common_Cancel"];
            doneButton.Text = Localization["Common_Done"];

            nameLabel.Paint += NameLabelPaint;
            Application.AddMessageFilter(_globalMouseHandler);
            _globalMouseHandler.TheMouseMoved += GlobalMouseHandlerTheMouseMoved;
        }

        private GlobalMouseHandler _globalMouseHandler = new GlobalMouseHandler();

        private Point _currentMousePoint;
        private void GlobalMouseHandlerTheMouseMoved()
        {
            if (_currentMousePoint != Cursor.Position)
            {
                App.IdleTimer = 0;
            }
            _currentMousePoint = Cursor.Position;
        }

        public new event EventHandler OnClosed; //it's default event on form, but it's also protected event, use new and create as public event

        public virtual void Initialize()
        {
            PrintImages.Clear();
            PrintComments.Clear();
            PrintDevicesQueue.Clear();

            printButton.Enabled =
                informationCheckBox.Enabled = overlayCheckBox.Enabled = commentTextBox.Enabled =
                    includeCheckBox.Enabled = false;

            nextPageButton.BackgroundImage = previousPageButton.BackgroundImage = null;

            printButton.Visible = cancelButton.Visible = true;
            doneButton.Visible = false;

            printImageLabel.Text = Localization["PrintImage_Loading"];
            pageLabel.Text = 1 + @" / " + PrintDevices.Count;
            _nameLabelText = PrintDevices[0].ToString();
            nameLabel.Invalidate();

            _currentlyPage = 1;
            printImageLabel.BackgroundImage = null;

            datetimeLabel.Text = DateTime.ToDateTimeString();

            foreach (ICamera camera in PrintDevices)
            {
                if (PrintImages.ContainsKey(camera)) continue;

                PrintComments.Add("");
                PrintImages.Add(camera, null);
            }

            includeCheckBox.Visible = (PrintDevices.Count > 1);

            FormClosing += PrintImageFormClosing;
        }

        private void NameLabelPaint(Object sender, PaintEventArgs e)
        {
            if (String.IsNullOrEmpty(_nameLabelText)) return;
            Graphics g = e.Graphics;
            int maxWidth = nameLabel.Width - 16; //left,right space 8
            if (maxWidth <= 0) return;

            string text = _nameLabelText;

            SizeF fSize = g.MeasureString(_nameLabelText, _nameLabelFont);
            //trim text if too long
            while (fSize.Width > maxWidth)
            {
                if (text.Length <= 1) break;

                text = text.Substring(0, text.Length - 1);
                fSize = g.MeasureString(text, _nameLabelFont);
            }
            if (text != _nameLabelText)
            {
                text += "...";
                fSize = g.MeasureString(text, _nameLabelFont);
            }

            g.DrawString(text, _nameLabelFont, _nameLabelBrush, Convert.ToInt32((nameLabel.Width - fSize.Width)/2), 7);
        }

        private void PrintImageFormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnClosed != null)
                OnClosed(this, null);
        }


        public void LoadImage()
        {
            UInt16 index = 0;

            UInt64 timecode = DateTimes.ToUtc(DateTime, Server.Server.TimeZone);

            var devices = PrintImages.Keys.ToList();
            foreach (var device in devices)
            {
                if (PrintDevicesQueue.ContainsValue(device)) continue;

                PrintDevicesQueue.Add(index++, device);

                LoadImageByDeviceDelegate loadImageByDeviceDelegate = LoadImageByDevice;
                loadImageByDeviceDelegate.BeginInvoke(device, timecode, LoadImageCallback, loadImageByDeviceDelegate);
            }
        }

        private void LoadImageByDevice(ICamera camera, UInt64 timecode)
        {
            if (PrintImages[camera] != null) return;

            if (camera.Server.Server.TimeZone != Server.Server.TimeZone)
            {
                Int64 time = Convert.ToInt64(timecode);
                time += (Server.Server.TimeZone*1000);
                time -= (camera.Server.Server.TimeZone*1000);
                timecode = Convert.ToUInt64(time);
            }

            Image image = camera.GetSnapshot(timecode);

            if (image == null)
            {
                for (UInt16 index = 0; index < PrintDevicesQueue.Count; index++)
                {
                    if (PrintDevicesQueue[index] == camera && PrintDevicesQueue.Count > 1)
                        PrintDevicesQueue[index] = null;
                }
            }

            PrintImages[camera] = image ?? _noImage;
        }

        private void LoadImageCallback(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new LoadImageCallbackDelegate(LoadImageCallback), result);
                return;
            }

            ((LoadImageByDeviceDelegate) result.AsyncState).EndInvoke(result);

            if (PrintImages.ContainsValue(null))
                return;

            printButton.Enabled =
                informationCheckBox.Enabled = overlayCheckBox.Enabled = commentTextBox.Enabled =
                    includeCheckBox.Enabled = true;

            Boolean hasDevice = false;

            foreach (var obj in PrintDevicesQueue)
            {
                if (obj.Value != null)
                {
                    hasDevice = true;
                    break;
                }
            }
            printButton.Enabled = (hasDevice || PrintDevices.Count == 1);
                // if only print 1 device, can't see include check box, so button is always enable

            if ((PrintDevices.Count > 0))
            {
                nextPageButton.BackgroundImage = _nextPage;
                previousPageButton.BackgroundImage = _previousPage;
            }
            else
            {
                nextPageButton.BackgroundImage = previousPageButton.BackgroundImage = null;
            }

            SetCurrentPage();
        }

        private void CancelButtonClick(Object sender, EventArgs e)
        {
            Hide();

            if (OnClosed != null)
                OnClosed(this, null);
        }

        private void PrintButtonClick(Object sender, EventArgs e)
        {
            Boolean hasImage = false;

            foreach (var obj in PrintDevicesQueue)
            {
                if (obj.Value != null)
                {
                    hasImage = true;
                    break;
                }
            }

            if (!hasImage) return;

            var devices = PrintDevicesQueue.Values.Where(kvp => kvp != null).Select(kvp => kvp.ToString()).ToArray();
            string log = "Print Image Devices %1 Datetime : %2".Replace("%1", String.Join(",", devices)).Replace("%2", DateTime.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));

            Server.WriteOperationLog(log);

            var doc = new PrintDocument();

            var printDialog = new PrintDialog
            {
                Document = doc
            };

            if (IntPtr.Size == 8)
                printDialog.UseEXDialog = true;
            /*doc.PrintPage += PrintPage;
			doc.EndPrint += DocEndPrint;
			PrintPreviewDialog previewDialog = new PrintPreviewDialog();            
			previewDialog.Document = doc;
			previewDialog.ShowDialog();*/

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (var obj in PrintDevicesQueue)
                    {
                        if (obj.Value == null) continue;

                        doc.DocumentName = obj.Value.ToString();
                        break;
                    }
                    //doc.DocumentName = PrintDevices[_currentlyPage - 1].ToString();
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
            OnPrintCompleted();

            EndPrint();
        }

        protected virtual void OnPrintCompleted()
        {
            
        }

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

        protected virtual void PrintPage(Object sender, PrintPageEventArgs e)
        {
            ICamera camera = null;
            UInt16 index = 0;
            for (UInt16 i = 0; i < PrintDevicesQueue.Count; i++)
            {
                if (PrintDevicesQueue[i] == null) continue;

                camera = PrintDevicesQueue[i];
                PrintDevicesQueue[i] = null;
                index = i;
                break;
            }

            if (camera == null) return;

            Graphics g = e.Graphics;
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            String name = camera.ToString();
            SizeF nameSize = g.MeasureString(name, _nameFont);

            String datetime = ((camera.Profile != null) ? (camera.Profile.NetworkAddress + "  ") : "") +
                              DateTime.ToDateTimeString();
            SizeF datetimeSize = g.MeasureString(datetime, _timeFont);

            RectangleF rectangle = PrintImages[camera].GetBounds(ref _units);

            if (informationCheckBox.Checked)
            {
                string titleName = name;

                int maxWidth = e.MarginBounds.Width;
                while (nameSize.Width > maxWidth)
                {
                    if (titleName.Length <= 1) break;

                    titleName = titleName.Substring(0, titleName.Length - 1);
                    nameSize = g.MeasureString(titleName + "...", _nameFont);
                }
                if (titleName != name)
                {
                    titleName += "...";
                    //nameSize = g.MeasureString(titleName, _nameFont);
                }

                g.DrawString(titleName, _nameFont, Brushes.Black, x, y);
                g.DrawString(datetime, _timeFont, Brushes.Black, x + e.MarginBounds.Width - datetimeSize.Width,
                    y + nameSize.Height);
                y += (nameSize.Height + datetimeSize.Height);

                g.FillRectangle(_blackBrushes, x, y, e.MarginBounds.Width, 3);

                y += 15;
            }

            float percent = rectangle.Width/e.MarginBounds.Width;

            //over height
            if (rectangle.Height/percent > (e.MarginBounds.Height - 160))
            {
                percent = rectangle.Height/(e.MarginBounds.Height - 160);
                g.DrawImage(PrintImages[camera], x, y, rectangle.Width/percent, rectangle.Height/percent);
            }
            else
            {
                g.DrawImage(PrintImages[camera], x, y, e.MarginBounds.Width, rectangle.Height/percent);
            }

            if (overlayCheckBox.Checked)
            {
                //g.FillRectangle(_whiteBrushes, x, y, fSize.Width, fSize.Height);
                //g.DrawString(name, _nameFont, Brushes.Black, x, y);

                datetimeSize = g.MeasureString(", " + datetime, _timeFont);

                nameSize = g.MeasureString(name, _timeFont);
                string osdName = name;
                float maxWidth = e.MarginBounds.Width - datetimeSize.Width;

                while (nameSize.Width > maxWidth)
                {
                    if (osdName.Length <= 1) break;

                    osdName = osdName.Substring(0, osdName.Length - 1);
                    nameSize = g.MeasureString(osdName + "...", _timeFont);
                }
                if (osdName != name)
                {
                    osdName += "...";
                }

                SizeF nameAndDatetimeSize = g.MeasureString(osdName + ", " + datetime, _timeFont);
                g.FillRectangle(_whiteBrushes, x, y + rectangle.Height/percent - nameAndDatetimeSize.Height,
                    nameAndDatetimeSize.Width, nameAndDatetimeSize.Height);

                g.DrawString(osdName + ", " + datetime, _timeFont, Brushes.Black, x,
                    y + rectangle.Height/percent - nameAndDatetimeSize.Height);
            }

            if (informationCheckBox.Checked)
            {
                if (PrintComments[index] != "")
                {
                    y += rectangle.Height/percent;
                    g.FillRectangle(_blackBrushes, x, y + 5, e.MarginBounds.Width, 3);
                    y += 15;

                    SizeF commentSize = g.MeasureString(PrintComments[index], _timeFont, e.MarginBounds.Width);

                    var commentRect = new RectangleF(x, y, e.MarginBounds.Width, Math.Min(350, commentSize.Height));
                    e.Graphics.DrawString(PrintComments[index], _timeFont, Brushes.Black, commentRect);
                    e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(commentRect));
                }

                g.DrawString(Localization["PrintImage_PrintText"].Replace("%1", Server.Credential.UserName)
                    .Replace("%2", Server.Credential.Domain)
                    .Replace("%3", DateTime.Now.ToDateTimeString()),
                    _timeFont, Brushes.Black, x, e.MarginBounds.Bottom - datetimeSize.Height);
            }

            foreach (var obj in PrintDevicesQueue)
            {
                if (obj.Value == null) continue;

                e.HasMorePages = true;
                return;
            }

            e.HasMorePages = false;
        }

        private void SetCurrentPage()
        {
            if (_currentlyPage > PrintDevices.Count)
                _currentlyPage = 1;
            else if (_currentlyPage < 1)
                _currentlyPage = PrintDevices.Count;

            printImageLabel.Text = "";
            _nameLabelText = PrintDevices[_currentlyPage - 1].ToString();
            nameLabel.Invalidate();

            printImageLabel.BackgroundImage = PrintImages[PrintDevices[_currentlyPage - 1]];
            printImageLabel.BackgroundImageLayout = (printImageLabel.BackgroundImage == _noImage)
                ? ImageLayout.Center
                : ImageLayout.Stretch;

            pageLabel.Text = _currentlyPage + @" / " + PrintDevices.Count;

            commentTextBox.TextChanged -= CommentTextBoxTextChanged;

            if (PrintComments[_currentlyPage - 1] == "")
            {
                commentTextBox.Text = Localization["PrintImage_WriteComment"];
            }
            else
            {
                commentTextBox.Text = PrintComments[_currentlyPage - 1];
            }

            commentTextBox.TextChanged += CommentTextBoxTextChanged;

            includeCheckBox.Checked = (PrintDevicesQueue[(UInt16) (_currentlyPage - 1)] != null);
        }

        private void NextPageButtonMouseClick(Object sender, MouseEventArgs e)
        {
            _currentlyPage++;
            SetCurrentPage();
        }

        private void PreviousPageButtonMouseClick(Object sender, MouseEventArgs e)
        {
            _currentlyPage--;
            SetCurrentPage();
        }

        private void IncludeCheckBoxClick(Object sender, EventArgs e)
        {
            PrintDevicesQueue[(UInt16) (_currentlyPage - 1)] = (includeCheckBox.Checked)
                ? PrintDevices[_currentlyPage - 1]
                : null;

            foreach (var obj in PrintDevicesQueue)
            {
                if (obj.Value != null)
                {
                    printButton.Enabled = true;
                    return;
                }
            }

            printButton.Enabled = false;
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
            App.IdleTimer = 0;
        }

        private void DoneButtonClick(Object sender, EventArgs e)
        {
            printButton.Visible = cancelButton.Visible = true;
            doneButton.Visible = false;

            Hide();

            if (OnClosed != null)
                OnClosed(this, null);
        }

        private delegate void EndPrintDelegate();

        private delegate void LoadImageByDeviceDelegate(ICamera camera, UInt64 timecode);

        private delegate void LoadImageCallbackDelegate(IAsyncResult result);
    }
}