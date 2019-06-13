using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace ExportVideoForm
{
    public partial class ExportVideo : Form
    {
        private readonly GlobalMouseHandler _globalMouseHandler = new GlobalMouseHandler();
        public new event EventHandler OnClosed;
        public IApp App { get; set; }
        public IServer Server { get; set; }
        private Boolean _isChangePath;
        public Boolean IsExporting { get; set; }
        public Queue<ICamera> ExportDevices = new Queue<ICamera>();
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        protected UInt64 StartTimecode;
        protected UInt64 EndTimecode;
        public String ExportInformation;

        public Dictionary<String, String> Localization;

        private Int32 MiniHeight;
        private Int32 TotalHeight;


        // Constructor
        public ExportVideo()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"Common_Cancel", "Cancel"},
								   {"Common_Done", "Done"},
								   
								   {"MessageBox_Information", "Information"},
								   {"MessageBox_Error", "Error"},

								   {"ExportVideo_Start", "Start"},
								   {"ExportVideo_End", "End"},
								   {"ExportVideo_Path", "Path"},
								   {"ExportVideo_Audio", "Audio"},
								   {"ExportVideo_AudioIn", "Audio In"},
								   {"ExportVideo_AudioOut", "Audio Out"},
								   {"ExportVideo_Encode", "Encode"},
								   {"ExportVideo_OverlayOSD", "Overlay OSD On Video"},
								   {"ExportVideo_Export", "Export"},
								   {"ExportVideo_Title", "Export Video"},
								   {"ExportVideo_SelectFilePath", "Select file path"},
								   {"ExportVideo_ExportOriginalVideo", "Export original video"},
								   
								   {"ExportVideo_ConvertVideoToRAW", "RAW"},
								   {"ExportVideo_ConvertVideoToMJPEG", "Convert to MJPEG"},

									{"ExportVideo_Quality", "Quality"},
									{"ExportVideo_Scale", "Scale"},
                                    {"ExportVideo_MountType", "Mount Type"},
									{"ExportVideo_ExportMaxiFileSize", "File split size"},

								   {"ExportVideo_ExportFailed", "Export video \"%1\" failed."},
								   {"ExportVideo_ExportCancelled", "Export video cancelled."},
								   {"ExportVideo_ExportCompleted", "Elapsed time: %1"},
								   {"ExportVideo_PathNotAvailable", "Export path not available.Continue export video to desktop?"},
								   
								   {"ExportVideo_Video", "Video"},
									
								   {"SetupGeneral_Desktop", "Desktop"},
								   {"SetupGeneral_Document", "My Documents"},
								   {"SetupGeneral_Picture", "My Pictures"},

								   {"Application_CantCreateFolder", "Can't create folder %1"},

                                   {"MountType_Ceiling", "Ceiling"},
			                       {"MountType_Ground", "Ground"},
			                       {"MountType_Wall", "Wall"},
                                   {"MountType_Original", "Original"},
                                   {"ExportVideo_FisheyeOnly", "(For Fisheye)"},
							   };
            Localizations.Update(Localization);

            InitializeComponent();
            BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.IMGControllerBG);
            browserButton.BackgroundImage = Resources.GetResources(Properties.Resources.SelectFolder, Properties.Resources.IMGSelectFolder);
            DoubleBuffered = true;
            TopMost = true;

            //var screenRectangle = RectangleToScreen(this.ClientRectangle);

            TotalHeight = 550;// Height; //can't get CORRECT height from form, will lose title and border width
            MiniHeight = TotalHeight - 85;

            Height = MiniHeight;

            exportButton.BackgroundImage = Resources.GetResources(Properties.Resources.exportButton, Properties.Resources.IMGExportButton);
            doneButton.BackgroundImage = Resources.GetResources(Properties.Resources.exportButton, Properties.Resources.IMGExportButton);
            cancelButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn, Properties.Resources.IMGCancelButotn);

            doneButton.Location = exportButton.Location;
            doneButton.Visible = false;

            Text = Localization["ExportVideo_Title"];
            exportButton.Text = Localization["ExportVideo_Export"];
            cancelButton.Text = Localization["Common_Cancel"];
            doneButton.Text = Localization["Common_Done"];

            startLabel.Text = Localization["ExportVideo_Start"];
            endLabel.Text = Localization["ExportVideo_End"];
            pathLabel.Text = Localization["ExportVideo_Path"];
            audioLabel.Text = Localization["ExportVideo_Audio"];
            audioInCheckBox.Text = Localization["ExportVideo_AudioIn"];
            audioOutCheckBox.Text = Localization["ExportVideo_AudioOut"];
            encodeLabel.Text = Localization["ExportVideo_Encode"];
            overlayCheckBox.Text = Localization["ExportVideo_OverlayOSD"];
            orangeRadioButton.Text = Localization["ExportVideo_ExportOriginalVideo"];
            encodeAVIRadioButton.Text = Localization["ExportVideo_ConvertVideoToMJPEG"];

            qualityLabel.Text = Localization["ExportVideo_Quality"];
            scaleLabel.Text = Localization["ExportVideo_Scale"];
            dewarpLabel.Text = Localization["ExportVideo_MountType"];
            fisheyeOnlyLabel.Text = Localization["ExportVideo_FisheyeOnly"];
            exportMaxiSizeLabel.Text = Localization["ExportVideo_ExportMaxiFileSize"];

            endoceComboBox.Items.Add("RAW");
            endoceComboBox.Items.Add("AVI");
            endoceComboBox.SelectedIndexChanged -= EndoceComboBoxSelectedIndexChanged;
            endoceComboBox.SelectedIndexChanged += EndoceComboBoxSelectedIndexChanged;

            scaleComboBox.Items.Add("1:1");
            scaleComboBox.Items.Add("1:4");
            scaleComboBox.Items.Add("1:8");
            scaleComboBox.Items.Add("1:16");

            var item = new ComboxItem(Localization["MountType_Original"], "Original");
            dewarpComboBox.Items.Add(item);

            item = new ComboxItem(Localization["MountType_Ceiling"], "Ceiling");
            dewarpComboBox.Items.Add(item);

            item = new ComboxItem(Localization["MountType_Ground"], "Ground");
            dewarpComboBox.Items.Add(item);

            item = new ComboxItem(Localization["MountType_Wall"], "Wall");
            dewarpComboBox.Items.Add(item);

            KeyPreview = true;
            FormClosing += ExportVideoFormClosing;
            KeyDown += ExportVideoKeyDown;

            Application.AddMessageFilter(_globalMouseHandler);
            _globalMouseHandler.TheMouseMoved += GlobalMouseHandlerTheMouseMoved;
        }
        

        private Point _currentMousePoint;
        private void GlobalMouseHandlerTheMouseMoved()
        {
            if (_currentMousePoint != Cursor.Position && App != null)
            {
                App.IdleTimer = 0;
            }
            _currentMousePoint = Cursor.Position;
        }

        private void EndoceComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (endoceComboBox.SelectedItem.ToString())
            {
                case "RAW":
                    exportMaxiSizeLabel.Visible = MaxiNumericUpDown.Visible = mbLabel.Visible =
                    orangeRadioButton.Visible = encodeAVIRadioButton.Visible = overlayCheckBox.Visible =
                    qualityLabel.Visible = qualityNumericUpDown.Visible =
                    scaleLabel.Visible = scaleComboBox.Visible =
                    dewarpLabel.Visible = dewarpComboBox.Visible = fisheyeOnlyLabel.Visible = false;
                    break;

                case "AVI":
                    exportMaxiSizeLabel.Visible = MaxiNumericUpDown.Visible = mbLabel.Visible =
                    orangeRadioButton.Visible = encodeAVIRadioButton.Visible = true;

                    if (encodeAVIRadioButton.Checked)
                    {
                        qualityLabel.Visible = qualityNumericUpDown.Visible =
                        scaleLabel.Visible = scaleComboBox.Visible =
                        dewarpLabel.Visible = dewarpComboBox.Visible = fisheyeOnlyLabel.Visible =
                        overlayCheckBox.Visible = true;
                    }
                    break;
            }
        }

        //stop ALT + F4 to close window
        private void ExportVideoFormClosing(Object sender, FormClosingEventArgs e)
        {
            if (_altF4Pressed)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                    e.Cancel = true;
                _altF4Pressed = false;
            }
        }

        private bool _altF4Pressed = false;
        private void ExportVideoKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
                _altF4Pressed = true;
            //else
            //    _altF4Pressed = false;
        }

        //disable dbl-click on form-title to maximum form
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0xA3://WM_NCLBUTTONDBLCLK
                    return;
            }
            base.WndProc(ref m);
        }

        private UInt16 _completedCount;
        public void Initialize()
        {
            ExportFileName.Clear();
            pathTextBox.TextChanged -= PathTextBoxTextChanged;

            switch (Server.Configure.ExportVideoPath)
            {
                case "Desktop":
                    pathTextBox.Text = Localization["SetupGeneral_Desktop"];
                    break;

                case "Document":
                    pathTextBox.Text = Localization["SetupGeneral_Document"];
                    break;

                case "Picture":
                    pathTextBox.Text = Localization["SetupGeneral_Picture"];
                    break;

                default:
                    pathTextBox.Text = Server.Configure.ExportVideoPath;
                    break;
            }
            _isChangePath = false;

            pathTextBox.TextChanged += PathTextBoxTextChanged;

            _completedCount = 0;
            exportStatusLabel.Text = "";
            completedLabel.Text = "";
            exportProgressBar.Visible = false;
            //overlayCheckBox.Checked = true;

            exportButton.Visible = cancelButton.Visible = true;
            doneButton.Visible = false;

            cancelButton.Enabled = audioInCheckBox.Enabled = audioOutCheckBox.Enabled =
            startDatePicker.Enabled = startTimePicker.Enabled =
            endDatePicker.Enabled = endTimePicker.Enabled =
            pathTextBox.Enabled = browserButton.Enabled =
            exportButton.Enabled = overlayCheckBox.Enabled =
            orangeRadioButton.Enabled = encodeAVIRadioButton.Enabled =
            endoceComboBox.Enabled = mbLabel.Enabled =
            MaxiNumericUpDown.Enabled = exportMaxiSizeLabel.Enabled =
            qualityNumericUpDown.Enabled = qualityLabel.Enabled =
            scaleComboBox.Enabled = scaleLabel.Enabled =
            dewarpComboBox.Enabled = dewarpLabel.Enabled = fisheyeOnlyLabel.Enabled = true;

            qualityLabel.Visible = qualityNumericUpDown.Visible =
            scaleLabel.Visible = scaleComboBox.Visible =
            dewarpComboBox.Visible = dewarpLabel.Visible = fisheyeOnlyLabel.Visible =
            overlayCheckBox.Visible = encodeAVIRadioButton.Checked;

            startDatePicker.Value = startTimePicker.Value = StartDateTime;
            endDatePicker.Value = endTimePicker.Value = EndDateTime;

            exportProgressBar.Value = 0;
            exportProgressBar.Maximum = 100 * ExportDevices.Count;

            endoceComboBox.SelectedItem = "AVI";
            scaleComboBox.SelectedItem = "1:1";
            dewarpComboBox.SelectedIndex = 0;
            MaxiNumericUpDown.Value = 1024;
            qualityNumericUpDown.Value = 50;

            SharedToolTips.SharedToolTip.SetToolTip(browserButton, Localization["ExportVideo_SelectFilePath"]);

            if (Server is ICMS)
            {
                UseDewarp(false);

                TotalHeight = 500;// Height; //can't get CORRECT height from form, will lose title and border width
                Height = encodeAVIRadioButton.Checked ? TotalHeight : MiniHeight = TotalHeight - 85;
                return;
            }

            var hasFisheyeDevice = false;
            foreach (ICamera exportDevice in ExportDevices)
            {
                if (exportDevice != null && exportDevice.Profile != null && (!String.IsNullOrEmpty(exportDevice.Profile.DewarpType) || exportDevice.Model.Type == "fisheye"))
                {
                    hasFisheyeDevice = true;
                    break;
                }
            }

            UseDewarp(hasFisheyeDevice);
        }

        private void UseDewarp(Boolean use)
        {
            if (use)
            {
                panel2.Controls.Add(dewarpLabel);
                panel2.Controls.Add(dewarpComboBox);
                panel2.Controls.Add(fisheyeOnlyLabel);
            }
            else
            {
                panel2.Controls.Remove(dewarpLabel);
                panel2.Controls.Remove(dewarpComboBox);
                panel2.Controls.Remove(fisheyeOnlyLabel);
            }

        }

        public void StopExport()
        {
            if (IsExporting && _exportDevice != null)
                _exportDevice.StopExportVideo();
        }

        private readonly Stopwatch _watch = new Stopwatch();
        protected String ExportPath;

        private void StartExport()
        {
            StartTimecode = DateTimes.ToUtc(new DateTime(
                startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day,
                startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second), Server.Server.TimeZone);

            EndTimecode = DateTimes.ToUtc(new DateTime(
                endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day,
                endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second), Server.Server.TimeZone);

            _watch.Reset();
            _watch.Start();

            if (ExportDevices.Count > 0)
            {

                var log = String.Format("Export video Device {0} Start Time {1} End Time {2}", String.Join(",", (from exportDevice in ExportDevices where exportDevice != null select exportDevice.ToString()).ToArray()),
                     DateTimes.ToDateTime(StartTimecode, Server.Server.TimeZone).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                    DateTimes.ToDateTime(EndTimecode, Server.Server.TimeZone).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));

                //.Replace("%1", String.Join(",", (from exportDevice in ExportDevices where exportDevice != null select exportDevice.ToString()).ToArray()))
                //.Replace("%2", StartTimecode.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)).Replace("%3", EndTimecode.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
                Server.WriteOperationLog(log);

                ExportDevice(ExportDevices.Dequeue());
            }
        }

        protected ICamera _exportDevice;
        protected readonly Dictionary<ICamera, String> ExportFileName = new Dictionary<ICamera, String>();

        protected virtual void ExportDevice(ICamera camera)
        {
            if (!CheckPath(camera)) return;
            Boolean audioIn = audioInCheckBox.Checked;
            Boolean audioOut = audioOutCheckBox.Checked;
            Boolean displayOsd = overlayCheckBox.Checked;
            //Boolean encode = encodeAVIRadioButton.Checked;
            //Boolean raw = rawRadioButton.Checked;

            _exportDevice = camera;
            if (_exportDevice != null)
            {
                IsExporting = true;

                camera.OnExportVideoProgress -= UtilityOnExportVideoProgress;
                camera.OnExportVideoProgress += UtilityOnExportVideoProgress;
                camera.OnExportVideoComplete -= UtilityOnExportVideoComplete;
                camera.OnExportVideoComplete += UtilityOnExportVideoComplete;

                var startTimecode = StartTimecode;
                var endTimecode = EndTimecode;
                if (camera.Server.Server.TimeZone != Server.Server.TimeZone)
                {
                    Int64 time = Convert.ToInt64(startTimecode);
                    time += (Server.Server.TimeZone * 1000);
                    time -= (camera.Server.Server.TimeZone * 1000);
                    startTimecode = Convert.ToUInt64(time);

                    time = Convert.ToInt64(endTimecode);
                    time += (Server.Server.TimeZone * 1000);
                    time -= (camera.Server.Server.TimeZone * 1000);
                    endTimecode = Convert.ToUInt64(time);
                }

                var type = 0;//type 0:raw, 1:original, 2:mjpeg
                if (endoceComboBox.SelectedItem.ToString() == "AVI")
                {
                    type = encodeAVIRadioButton.Checked ? 2 : 1;
                }

                //scale  0:original 1:1/2,  2:1/4,  3:1/8  4:1/16
                var scale = 0;
                switch (scaleComboBox.SelectedItem.ToString())
                {
                    case "1:2":
                        scale = 1;
                        break;

                    case "1:4":
                        scale = 2;
                        break;

                    case "1:8":
                        scale = 3;
                        break;

                    case "1:16":
                        scale = 4;
                        break;
                }

                //fisheye dewarp
                var dewarp = ((ComboxItem)dewarpComboBox.SelectedItem).Value != "Original";
                var mountType = 0;
                switch (((ComboxItem)dewarpComboBox.SelectedItem).Value)
                {
                    case "Wall":
                        mountType = 0;
                        break;

                    case "Ceiling":
                        mountType = 1;
                        break;

                    case "Ground":
                        mountType = 2;
                        break;
                }
                if (camera.Profile != null && (!String.IsNullOrEmpty(camera.Profile.DewarpType) || camera.Model.Type == "fisheye"))
                    camera.InitFisheyeLibrary(camera, dewarp, (short)mountType);

                //LONG bOSD_Watermark 0:disable 1:enable osd, 2:enable watermark, 3:enable osd and watermark
                var osdWatermark = overlayCheckBox.Checked ? 1 : 0;

                camera.Server.Configure.ExportVideoMaxiFileSize = (ushort)MaxiNumericUpDown.Value;

                var fileName = camera.ExportVideo(startTimecode, endTimecode, displayOsd, audioIn, audioOut, (ushort)type, ExportPath, (ushort)qualityNumericUpDown.Value, (ushort)scale, (ushort)osdWatermark);

                if (!ExportFileName.ContainsKey(camera))
                    ExportFileName.Add(camera, fileName);
                else
                    ExportFileName[camera] = fileName;
            }
        }

        private void UtilityOnExportVideoProgress(Object sender, EventArgs<UInt16, ExportVideoStatus> e)
        {
            if (_exportDevice == null)
            {
                Hide();
                return;
            }

            if (e.Value2 == ExportVideoStatus.ExportFailed)
            {
                _watch.Stop();
                TopMostMessageBox.Show(Localization["ExportVideo_ExportFailed"].Replace("%1", _exportDevice.ToString()), Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (ExportNextIfAvailable()) return;
                Hide();
                return;
            }

            if (!(Server is ICMS) && _exportDevice.DeviceType == DeviceType.Device && !_exportDevice.Server.Device.Devices.ContainsValue(_exportDevice))
            {
                _exportDevice.StopExportVideo();
                _watch.Stop();
                TopMostMessageBox.Show(Localization["ExportVideo_ExportFailed"].Replace("%1", _exportDevice.ToString()), Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (ExportNextIfAvailable()) return;

                if (_completedCount == 0)
                    Hide();
                else
                    ExportVideoComplete();
                return;
            }

            exportProgressBar.Value = (_completedCount * 100) + e.Value1;
            //exportStatusLabel.Text = e.Value2.ToString();

            var percent = (Int32)((exportProgressBar.Value / (double)exportProgressBar.Maximum) * 100);

            exportStatusLabel.Text = percent + @"%";

            cancelButton.Enabled = (percent != 100);
        }

        private Boolean ExportNextIfAvailable()
        {
            IsExporting = false;

            _exportDevice.OnExportVideoComplete -= UtilityOnExportVideoComplete;
            _exportDevice.OnExportVideoProgress -= UtilityOnExportVideoProgress;

            if (ExportDevices.Count > 0)
            {
                var device = ExportDevices.Dequeue();
                while (!device.Server.Device.Devices.ContainsValue(device) && device.DeviceType == DeviceType.Device && ExportDevices.Count > 0 && Server is ICMS == false)
                {
                    TopMostMessageBox.Show(Localization["ExportVideo_ExportFailed"].Replace("%1", device.ToString()), Localization["MessageBox_Information"],
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    device = ExportDevices.Dequeue();
                }

                if (Server is ICMS || device.Server.Device.Devices.ContainsValue(device) || device.DeviceType != DeviceType.Device)
                {
                    ExportDevice(device);
                    return true;
                }

                TopMostMessageBox.Show(Localization["ExportVideo_ExportFailed"].Replace("%1", device.ToString()), Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            _exportDevice = null;

            return false;
        }

        protected virtual void UtilityOnExportVideoComplete(Object sender, EventArgs e)
        {
            _completedCount++;
            ExportVideoComplete();
        }

        protected virtual void ExportVideoComplete()
        {
            if (ExportNextIfAvailable()) return;

            var assembly = Assembly.GetExecutingAssembly();
            var location = assembly.Location;
            var upperPath = new DirectoryInfo(Path.GetDirectoryName(location)).Parent;
            var playerFile = Path.Combine(upperPath.FullName, "Tools\\RawPlayer.exe");

            var exportPath = Path.Combine(new DirectoryInfo(ExportPath).Parent.Parent.Parent.FullName, "RawPlayer.exe");
            if (File.Exists(playerFile))
            {
                File.Copy(playerFile, exportPath, true);
            }
            else
            {
                Logger.Current.WarnFormat("RawPlayer doesn't exists. RawPlayer location: {0}", playerFile);
            }

            exportButton.Visible = cancelButton.Visible = false;
            doneButton.Visible = true;
            _exportPath = String.Empty;
            _watch.Stop();
            var timeStr = String.Format("{0:00}:{1:00}:{2:00}", _watch.Elapsed.Hours, _watch.Elapsed.Minutes, _watch.Elapsed.Seconds);
            completedLabel.Text = Localization["ExportVideo_ExportCompleted"].Replace("%1", timeStr);
            Focus();
            BringToFront();
        }

        protected virtual void ExportButtonClick(Object sender, EventArgs e)
        {
            if (!CheckPath(null)) return;

            exportProgressBar.Visible = true;
            _exportPath = String.Empty;
            audioInCheckBox.Enabled = audioOutCheckBox.Enabled =
            startDatePicker.Enabled = startTimePicker.Enabled =
            endDatePicker.Enabled = endTimePicker.Enabled =
            pathTextBox.Enabled = browserButton.Enabled =
            exportButton.Enabled = overlayCheckBox.Enabled =
            orangeRadioButton.Enabled = encodeAVIRadioButton.Enabled =
            endoceComboBox.Enabled = mbLabel.Enabled =
            MaxiNumericUpDown.Enabled = exportMaxiSizeLabel.Enabled =
            qualityNumericUpDown.Enabled = qualityLabel.Enabled =
            scaleComboBox.Enabled = scaleLabel.Enabled =
            dewarpLabel.Enabled = dewarpComboBox.Enabled = fisheyeOnlyLabel.Enabled = false;

            StartExport();
        }

        private readonly String _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly String _documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly String _picturePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        protected String _exportPath;

        protected virtual Boolean CheckPath(ICamera camera, String defaultExportPath = "")
        {
            String rootPath;

            if (_isChangePath)
            {
                if (!System.IO.Directory.Exists(pathTextBox.Text))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(pathTextBox.Text);
                    }
                    catch (Exception)
                    {
                        DialogResult result = TopMostMessageBox.Show(Localization["ExportVideo_PathNotAvailable"], Localization["MessageBox_Information"],
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                        Focus();
                        BringToFront();

                        if (result == DialogResult.Cancel)
                            return false;
                    }
                }

                rootPath = (System.IO.Directory.Exists(pathTextBox.Text))
                                ? pathTextBox.Text
                                : _desktopPath;
            }
            else
            {
                switch (Server.Configure.ExportVideoPath)
                {
                    case "Desktop":
                        rootPath = _desktopPath;
                        break;

                    case "Document":
                        rootPath = _documentPath;
                        break;

                    case "Picture":
                        rootPath = _picturePath;
                        break;

                    default:
                        if (!System.IO.Directory.Exists(Server.Configure.ExportVideoPath))
                        {
                            try
                            {
                                System.IO.Directory.CreateDirectory(Server.Configure.ExportVideoPath);
                            }
                            catch (Exception)
                            {
                                DialogResult result = TopMostMessageBox.Show(Localization["ExportVideo_PathNotAvailable"], Localization["MessageBox_Information"],
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                                Focus();
                                BringToFront();

                                if (result == DialogResult.Cancel)
                                    return false;
                            }
                        }

                        rootPath = (System.IO.Directory.Exists(Server.Configure.ExportVideoPath))
                                        ? Server.Configure.ExportVideoPath
                                        : _desktopPath;
                        break;
                }
            }

            if (!System.IO.Directory.Exists(rootPath))
            {
                DialogResult result = TopMostMessageBox.Show(Localization["ExportVideo_PathNotAvailable"], Localization["MessageBox_Information"],
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                Focus();
                BringToFront();

                if (result == DialogResult.Cancel)
                    return false;

                rootPath = _desktopPath;
            }

            String folderName = GetFolderPath(rootPath, defaultExportPath, camera);

            Boolean pathIsExists = System.IO.Directory.Exists(rootPath + folderName);
            if (!pathIsExists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(rootPath + folderName);
                }
                catch (Exception)
                {
                    TopMostMessageBox.Show(
                        Localization["Application_CantCreateFolder"].Replace("%1", rootPath + folderName),
                        Localization["MessageBox_Error"], MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }

            ExportPath = rootPath + folderName.Trim();

            if (ExportPath[ExportPath.Length - 1] != '\\')
                ExportPath += "\\";

            return true;
        }

        protected String _openFolderPath;
        protected virtual string GetFolderPath(string rootPath, string defaultExportPath, ICamera camera)
        {
            String folderName;

            String folderType = GetFolderType();
            //folderType += (" " + Localization["ExportVideo_Video"]);
            folderType += " Video";

            var slash = (rootPath[rootPath.Length - 1] != '\\') ? "\\" : "";

            if (String.IsNullOrEmpty(defaultExportPath))
            {
                if (String.IsNullOrEmpty(_exportPath))
                {
                    folderName = string.Format(@"{0}{1} ({2})\{3}\{4}", slash, folderType, Server.Credential.Domain,
                        Server.Server.DateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Server.Server.DateTime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture));

                    _exportPath = folderName;
                }
                else
                {
                    folderName = _exportPath;
                }

                _openFolderPath = rootPath + folderName;
                if (camera != null) folderName += "\\" + camera;
            }
            else
            {
                folderName = slash + folderType + " (" + Server.Credential.Domain + ")" + defaultExportPath;
            }

            return folderName;
        }

        protected virtual string GetFolderType()
        {
            return Server is ICMS ? "CMS" : "NVR";
        }

        private void PathTextBoxTextChanged(Object sender, EventArgs e)
        {
            _isChangePath = true;
        }

        private void CancelButtonClick(Object sender, EventArgs e)
        {
            if (_exportDevice != null)
            {
                _exportDevice.OnExportVideoComplete -= UtilityOnExportVideoComplete;
                _exportDevice.OnExportVideoProgress -= UtilityOnExportVideoProgress;

                _exportDevice.StopExportVideo();
            }

            if (IsExporting)
                TopMostMessageBox.Show(Localization["ExportVideo_ExportCancelled"], Localization["MessageBox_Information"],
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            _exportPath = String.Empty;
            IsExporting = false;
            _exportDevice = null;
            _watch.Stop();
            Hide();

            if (OnClosed != null)
                OnClosed(this, null);
        }

        public void ClosingExportVideoForm(Object sender, EventArgs e)
        {
            Hide();

            if (_exportDevice != null)
            {
                _exportDevice.OnExportVideoComplete -= UtilityOnExportVideoComplete;
                _exportDevice.OnExportVideoProgress -= UtilityOnExportVideoProgress;

                _exportDevice.StopExportVideo();

                IsExporting = false;
                _exportDevice = null;
            }

            Dispose();
        }

        private void BrowserButtonMouseClick(Object sender, EventArgs e)
        {
            DialogResult resault = exportFolderBrowserDialog.ShowDialog();

            if (resault == DialogResult.OK)
            {
                _isChangePath = true;
                pathTextBox.Text = exportFolderBrowserDialog.SelectedPath;
            }
        }

        private void DoneButtonClick(Object sender, EventArgs e)
        {
            exportButton.Visible = cancelButton.Visible = true;
            doneButton.Visible = false;

            try
            {
                Process.Start("explorer.exe", _openFolderPath);
            }
            catch (Exception)
            {
            }

            _openFolderPath = String.Empty;
            Hide();

            if (OnClosed != null)
                OnClosed(this, null);
        }

        private void OrangeRadioButtonCheckedChanged(Object sender, EventArgs e)
        {
            if (orangeRadioButton.Checked)
            {
                qualityLabel.Visible = qualityNumericUpDown.Visible =
                scaleLabel.Visible = scaleComboBox.Visible =
                dewarpLabel.Visible = dewarpComboBox.Visible = fisheyeOnlyLabel.Visible =
                overlayCheckBox.Visible = false;
                Height = MiniHeight;
            }
        }

        private void EncodeRadioButtonCheckedChanged(Object sender, EventArgs e)
        {
            if (encodeAVIRadioButton.Checked)
            {
                qualityLabel.Visible = qualityNumericUpDown.Visible =
                scaleLabel.Visible = scaleComboBox.Visible =
                dewarpLabel.Visible = dewarpComboBox.Visible = fisheyeOnlyLabel.Visible =
                overlayCheckBox.Visible = true;
                Height = TotalHeight;
            }
        }
    }
}
