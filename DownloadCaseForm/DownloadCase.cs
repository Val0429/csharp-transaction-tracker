using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace DownloadCaseForm
{
	public sealed partial class DownloadCase : Form
	{
		public event EventHandler<EventArgs<DownloadCaseConfig>> OnDownloadCaseAddToQueue;

		private IServer _server;
		public IServer Server
		{
			get { return _server; }
			set {
				_server = value;

				if (_server is IPTS)
				{
					var hideUnSupportFunctions = ((IPTS)Server).ReleaseBrand == "Salient";

					if (hideUnSupportFunctions)
					{
						var diff = 117;// commentLabel.Location.Y - audioLabel.Location.Y;
						Controls.Remove(audioLabel);
						Controls.Remove(audioInCheckBox);
						Controls.Remove(audioOutCheckBox);
						Controls.Remove(encodeLabel);
						Controls.Remove(orangeRadioButton);
						Controls.Remove(encodeAVIRadioButton);
						Controls.Remove(overlayCheckBox);

                        commentLabel.Location = new Point(commentLabel.Location.X, commentLabel.Location.Y - diff);
                        commentTextBox.Location = new Point(commentTextBox.Location.X, commentTextBox.Location.Y - diff);
					    commentTextBox.Height = commentTextBox.Height + diff;
					    //exportProgressBar.Location = new Point(exportProgressBar.Location.X, exportProgressBar.Location.Y - diff);
					    //exportStatusLabel.Location = new Point(exportStatusLabel.Location.X, exportStatusLabel.Location.Y - diff);
					    //addedToQueueButton.Location = new Point(addedToQueueButton.Location.X, addedToQueueButton.Location.Y - diff);
					    //downloadButton.Location = new Point(downloadButton.Location.X, downloadButton.Location.Y - diff);
					    //cancelButton.Location = new Point(cancelButton.Location.X, cancelButton.Location.Y - diff);
					    //doneButton.Location = new Point(doneButton.Location.X, doneButton.Location.Y - diff);
					    //completedLabel.Location = new Point(completedLabel.Location.X, completedLabel.Location.Y - diff);
					    //Size = new Size(Width, 126);

					}
				}
			}
		}
		private Boolean _isChangePath;
		public Boolean IsDownloading;
		public Queue<ICamera> ExportDevices = new Queue<ICamera>();
		public DateTime StartDateTime;
		public DateTime EndDateTime;
		private UInt64 _startTimecode;
		private UInt64 _endTimecode;

		public XmlDocument AttachXmlDoc;

		public Dictionary<String, String> Localization;

		public DownloadCase()
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
								   {"ExportVideo_SelectFilePath", "Select file path"},
								   {"ExportVideo_ExportOriginalVideo", "Export original video"},

								   {"ExportVideo_ConvertVideoToRAW", "RAW"},
								   {"ExportVideo_ConvertVideoToMJPEG", "Convert to MJPEG"},

									{"ExportVideo_Quality", "Quality"},
									{"ExportVideo_Scale", "Scale"},
									{"ExportVideo_ExportMaxiFileSize", "File split size"},

								   {"ExportVideo_ExportFailed", "Export video \"%1\" failed."},
								   {"ExportVideo_ExportCancelled", "Export video cancelled."},
								   {"ExportVideo_ExportCompleted", "Elapsed time: %1"},
								   {"ExportVideo_PathNotAvailable", "Export path not available.Continue export video to desktop?"},
								   
								   {"ExportVideo_Video", "Video"},
								   {"DownloadCase_Title", "Export Case"},
								   {"DownloadCase_Comment", "Comment"},
								   {"DownloadCase_WriteComment", "Enter Comment Here."},
								   {"DownloadCase_Download", "Download"},
								   {"DownloadCase_AddedToQueue", "Add to queue"},
								   {"DownloadCase_CantCopyFraudInvestigation", "Can't copy 'Fraud Investigation.exe'."},
									
								   {"SetupGeneral_Desktop", "Desktop"},
								   {"SetupGeneral_Document", "My Documents"},
								   {"SetupGeneral_Picture", "My Pictures"},

								   {"Application_CantCreateFolder", "Can't create folder %1"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.IMGControllerBG);
			browserButton.BackgroundImage = Resources.GetResources(Properties.Resources.SelectFolder, Properties.Resources.IMGSelectFolder);
            addedToQueueButton.BackgroundImage = Resources.GetResources(Properties.Resources.exportButton, Properties.Resources.IMGExportlButton);
            downloadButton.BackgroundImage = Resources.GetResources(Properties.Resources.exportButton, Properties.Resources.IMGExportlButton);
            doneButton.BackgroundImage = Resources.GetResources(Properties.Resources.exportButton, Properties.Resources.IMGExportlButton);
            cancelButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn, Properties.Resources.IMGCancelButton);
			DoubleBuffered = true;

			doneButton.Location = cancelButton.Location;
			doneButton.Visible = false;
			Text = Localization["DownloadCase_Title"];
			downloadButton.Text = Localization["ExportVideo_Export"];//DownloadCase_Download
			cancelButton.Text = Localization["Common_Cancel"];
			doneButton.Text = Localization["Common_Done"];
			addedToQueueButton.Text = Localization["DownloadCase_AddedToQueue"];

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
			exportMaxiSizeLabel.Text = Localization["ExportVideo_ExportMaxiFileSize"];

			endoceComboBox.Items.Add("RAW");
			endoceComboBox.Items.Add("AVI");
			endoceComboBox.SelectedIndexChanged -= EndoceComboBoxSelectedIndexChanged;
			endoceComboBox.SelectedIndexChanged += EndoceComboBoxSelectedIndexChanged;

			scaleComboBox.Items.Add("1:1");
			scaleComboBox.Items.Add("1:4");
			scaleComboBox.Items.Add("1:8");
			scaleComboBox.Items.Add("1:16");

			commentLabel.Text = Localization["DownloadCase_Comment"];

			commentTextBox.Text = Localization["DownloadCase_WriteComment"];

		}

		private void EndoceComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			switch (endoceComboBox.SelectedItem.ToString())
			{
				case "RAW":
					exportMaxiSizeLabel.Visible = MaxiNumericUpDown.Visible = mbLabel.Visible =
					orangeRadioButton.Visible = encodeAVIRadioButton.Visible = overlayCheckBox.Visible =
					qualityLabel.Visible = qualityNumericUpDown.Visible =
					scaleLabel.Visible = scaleComboBox.Visible = false;
					break;

				case "AVI":
					exportMaxiSizeLabel.Visible = MaxiNumericUpDown.Visible = mbLabel.Visible =
					orangeRadioButton.Visible = encodeAVIRadioButton.Visible = true;

					if (encodeAVIRadioButton.Checked)
					{
						qualityLabel.Visible = qualityNumericUpDown.Visible =
						scaleLabel.Visible = scaleComboBox.Visible =
						overlayCheckBox.Visible = true;
					}
					break;
			}
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

		private void StartDatePickerValueChanged(Object sender, EventArgs e)
		{
			return;

			//endDatePicker.MinDate = endTimePicker.MinDate = new DateTime(
			//    startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day,
			//    startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second);
		}

		private void EndDatePickerValueChanged(Object sender, EventArgs e)
		{
			return;

			//startDatePicker.MaxDate = startTimePicker.MaxDate = new DateTime(
			//    endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day,
			//    endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second);
		}

		private UInt16 _completedCount;
	    private Boolean _isFirst = true; 
		public void Initialize()
		{
			_exportFileName.Clear();
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

			startDatePicker.ValueChanged -= StartDatePickerValueChanged;
			startTimePicker.ValueChanged -= StartDatePickerValueChanged;
			endDatePicker.ValueChanged -= EndDatePickerValueChanged;
			endTimePicker.ValueChanged -= EndDatePickerValueChanged;
			
			downloadButton.Visible = cancelButton.Visible = true;
			doneButton.Visible = false;

			cancelButton.Enabled = audioInCheckBox.Enabled = audioOutCheckBox.Enabled =
			startDatePicker.Enabled = startTimePicker.Enabled =
			endDatePicker.Enabled =
			endTimePicker.Enabled =
			pathTextBox.Enabled =
			commentTextBox.Enabled =
			browserButton.Enabled =
			downloadButton.Enabled =
			overlayCheckBox.Enabled =
			orangeRadioButton.Enabled =
			encodeAVIRadioButton.Enabled =
			endoceComboBox.Enabled = mbLabel.Enabled =
		   MaxiNumericUpDown.Enabled = exportMaxiSizeLabel.Enabled =
		   qualityNumericUpDown.Enabled = qualityLabel.Enabled =
		   scaleComboBox.Enabled = scaleLabel.Enabled = true;

			qualityLabel.Visible = qualityNumericUpDown.Visible =
			scaleLabel.Visible = scaleComboBox.Visible =
			overlayCheckBox.Visible = encodeAVIRadioButton.Checked;

			addedToQueueButton.Visible = true;
			//endDatePicker.MinDate = endTimePicker.MinDate = StartDateTime;
			//startDatePicker.MaxDate = startTimePicker.MaxDate = EndDateTime;

			startDatePicker.Value = startTimePicker.Value = StartDateTime;
			endDatePicker.Value = endTimePicker.Value = EndDateTime;

			exportProgressBar.Value = 0;
			exportProgressBar.Maximum = 100 * ExportDevices.Count;

			startDatePicker.ValueChanged += StartDatePickerValueChanged;
			startTimePicker.ValueChanged += StartDatePickerValueChanged;
			endDatePicker.ValueChanged += EndDatePickerValueChanged;
			endTimePicker.ValueChanged += EndDatePickerValueChanged;

			//SharedToolTips.SharedToolTip.SetToolTip(browserButton, Localization["ExportVideo_SelectFilePath"]);

			commentTextBox.TextChanged -= CommentTextBoxTextChanged;
			_comments = "";
			commentTextBox.Text = Localization["DownloadCase_WriteComment"];
			commentTextBox.TextChanged += CommentTextBoxTextChanged;

			endoceComboBox.SelectedItem = "AVI";
			scaleComboBox.SelectedItem = "1:1";
			MaxiNumericUpDown.Value = 1024;
			qualityNumericUpDown.Value = 50;

            if (!_isFirst) return;
            if (Server is IPTS && _isFirst)
            {
                if (((IPTS)Server).ReleaseBrand == "Salient")
                {
                    encodeLabel.Visible =
                        endoceComboBox.Visible =
                        exportMaxiSizeLabel.Visible =
                        MaxiNumericUpDown.Visible =
                        mbLabel.Visible = false;

                    commentLabel.Location = new Point(commentLabel.Location.X, commentLabel.Location.Y - 95);
                    commentTextBox.Location = new Point(commentTextBox.Location.X, commentTextBox.Location.Y - 95);
                    commentTextBox.Size = new Size(commentTextBox.Size.Width, commentTextBox.Size.Height + 95);
                }
            }

		    _isFirst = false;
		}

		public void StopDownload()
		{
			if (IsDownloading && _exportDevice != null)
				_exportDevice.StopExportVideo();
		}

		private readonly Stopwatch _watch = new Stopwatch();
		private String _exportPath;
		private String _rootPath;
		private void StartDownload()
		{
			_startTimecode = DateTimes.ToUtc(new DateTime(
				startDatePicker.Value.Year, startDatePicker.Value.Month, startDatePicker.Value.Day,
				startTimePicker.Value.Hour, startTimePicker.Value.Minute, startTimePicker.Value.Second), Server.Server.TimeZone);

			_endTimecode = DateTimes.ToUtc(new DateTime(
				endDatePicker.Value.Year, endDatePicker.Value.Month, endDatePicker.Value.Day,
				endTimePicker.Value.Hour, endTimePicker.Value.Minute, endTimePicker.Value.Second), Server.Server.TimeZone);

			_watch.Reset();
			_watch.Start();

			if (ExportDevices.Count > 0)
			{
				ExportDevice(ExportDevices.Dequeue());
			}
		}

		private ICamera _exportDevice;
		private readonly Dictionary<ICamera, String> _exportFileName = new Dictionary<ICamera, String>();
		private void ExportDevice(ICamera camera)
		{
			Boolean audioIn = audioInCheckBox.Checked;
			Boolean audioOut = audioOutCheckBox.Checked;
			Boolean displayOsd = overlayCheckBox.Checked;
			//Boolean encode = encodeAVIRadioButton.Checked;

			_exportDevice = camera;
			if (_exportDevice != null)
			{
				IsDownloading = true;

				camera.OnExportVideoProgress -= UtilityOnExportVideoProgress;
				camera.OnExportVideoProgress += UtilityOnExportVideoProgress;
				camera.OnExportVideoComplete -= UtilityOnExportVideoComplete;
				camera.OnExportVideoComplete += UtilityOnExportVideoComplete;

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

				//LONG bOSD_Watermark 0:disable 1:enable osd, 2:enable watermark, 3:enable osd and watermark
				var osdWatermark = overlayCheckBox.Checked ? 1 : 0;

				camera.Server.Configure.ExportVideoMaxiFileSize = (ushort)MaxiNumericUpDown.Value;

				var fileName = camera.ExportVideo(_startTimecode, _endTimecode, displayOsd, audioIn, audioOut, (ushort)type, _exportPath, (ushort)qualityNumericUpDown.Value, (ushort)scale, (ushort)osdWatermark);
				if (!_exportFileName.ContainsKey(camera))
					_exportFileName.Add(camera, fileName);
				else
					_exportFileName[camera] = fileName;
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

				if (_completedCount == 0)
					Hide();
				else
					ExportVideoComplete();
				return;
			}

			if (!_exportDevice.Server.Device.Devices.ContainsValue(_exportDevice))
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
			if (_exportDevice != null)
			{
				_exportDevice.OnExportVideoComplete -= UtilityOnExportVideoComplete;
				_exportDevice.OnExportVideoProgress -= UtilityOnExportVideoProgress;
			}

			IsDownloading = false;
			if (ExportDevices.Count > 0)
			{
				var device = ExportDevices.Dequeue();
				while (!device.Server.Device.Devices.ContainsValue(device) && ExportDevices.Count > 0)
				{
					TopMostMessageBox.Show(Localization["ExportVideo_ExportFailed"].Replace("%1", device.ToString()), Localization["MessageBox_Information"],
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
					device = ExportDevices.Dequeue();
				}

				if (device.Server.Device.Devices.ContainsValue(device))
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

		private void UtilityOnExportVideoComplete(Object sender, EventArgs e)
		{
			_completedCount++;
			ExportVideoComplete();
		}
		
		private void ExportVideoComplete()
		{
			if (ExportNextIfAvailable()) return;

			SaveAttachXmlDoc(AttachXmlDoc, Server, StartDateTime, EndDateTime, _exportPath, _comments, _exportFileName);
			CopyPlayer();

			downloadButton.Visible = cancelButton.Visible = false;
			doneButton.Visible = true;
			_watch.Stop();

			exportProgressBar.Value = exportProgressBar.Maximum;
			exportStatusLabel.Text = @"100%";

			var timeStr = String.Format("{0:00}:{1:00}:{2:00}", _watch.Elapsed.Hours, _watch.Elapsed.Minutes, _watch.Elapsed.Seconds);
			completedLabel.Text = Localization["ExportVideo_ExportCompleted"].Replace("%1", timeStr);
			Focus();
			BringToFront();
		}

		public static void SaveAttachXmlDoc(XmlDocument attachXmlDoc, IServer server, DateTime startDateTime, DateTime endDateTime, String exportPath, String comments, Dictionary<ICamera, String> exportFileName)
		{
			//<Information>
			//    <Server domain="172.16.1.16" port="87"/>
			//    <User name="deray"/>
			//    <Store id="777" name="Taipei" />
			//    <DownloadTime utc="1234567890123" datetime="2013-02-11 12:34:56" />
			//    <ExportStart utc="1234567890123" datetime="2013-02-11 12:34:56" />
			//    <ExportEnd utc="1234567890123" datetime="2013-02-11 12:34:56" />
			//    <Comment></Comment>
			//    <ExportVideos>
			//        <Video cameraId="1" cameraName="Device 1" resolution="640x480">02 P1344_2013-02-06-20-42-57_2013-02-06-20-42-59.avi</Video>
			//    <ExportVideos>
			//<Information>
			if (attachXmlDoc == null) return;

			var xmlDoc = (XmlDocument)attachXmlDoc.Clone();

			var xmlRoot = Xml.GetFirstElementByTagName(xmlDoc, "Result");
			var informationNode = xmlDoc.CreateElement("Information");
			xmlRoot.AppendChild(informationNode);

			var serverNode = xmlDoc.CreateElement("Server");
			if (server != null)
			{
				serverNode.SetAttribute("domain", server.Credential.Domain);
				serverNode.SetAttribute("port", server.Credential.Port.ToString());
				serverNode.SetAttribute("timezone", server.Server.TimeZone.ToString());
			}

			var userNode = xmlDoc.CreateElement("User");
			if (server != null)
				userNode.SetAttribute("name", server.User.Current.Credential.UserName);

			var storeNode = xmlDoc.CreateElement("Store");
			if (server != null)
			{
				storeNode.SetAttribute("id", server.Configure.Store.Id);
				storeNode.SetAttribute("name", server.Configure.Store.Name);
			}

			var downloadTimeNode = xmlDoc.CreateElement("DownloadTime");
			downloadTimeNode.SetAttribute("utc", DateTimes.UtcNow.ToString());
			downloadTimeNode.SetAttribute("datetime", DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss"));

			var exportStartNode = xmlDoc.CreateElement("ExportStart");
			if (server != null)
				exportStartNode.SetAttribute("utc", DateTimes.ToUtc(startDateTime, server.Server.TimeZone).ToString());
			exportStartNode.SetAttribute("datetime", startDateTime.ToString("MM-dd-yyyy HH:mm:ss"));

			var exportEndNode = xmlDoc.CreateElement("ExportEnd");
			if (server != null)
				exportEndNode.SetAttribute("utc", DateTimes.ToUtc(endDateTime, server.Server.TimeZone).ToString());
			exportEndNode.SetAttribute("datetime", endDateTime.ToString("MM-dd-yyyy HH:mm:ss"));

			var commentNode = Xml.CreateXmlElementWithText(xmlDoc, "Comment", comments);

			var exportVideosNode = xmlDoc.CreateElement("ExportVideos");

			//        <Video cameraId="1" cameraName="Device 1" resolution="640x480">02 P1344_2013-02-06-20-42-57_2013-02-06-20-42-59.avi</Video>
			foreach (var obj in exportFileName)
			{
				var videoNode = Xml.CreateXmlElementWithText(xmlDoc, "Video", obj.Value);
				videoNode.SetAttribute("cameraId", obj.Key.Id.ToString());
				videoNode.SetAttribute("cameraName", obj.Key.Name);
				if (obj.Key.StreamConfig != null)
					videoNode.SetAttribute("resolution", Resolutions.ToString(obj.Key.StreamConfig.Resolution));
				exportVideosNode.AppendChild(videoNode);
			}
			informationNode.AppendChild(serverNode);
			informationNode.AppendChild(userNode);
			informationNode.AppendChild(storeNode);
			informationNode.AppendChild(downloadTimeNode);
			informationNode.AppendChild(exportStartNode);
			informationNode.AppendChild(exportEndNode);
			informationNode.AppendChild(commentNode);
			informationNode.AppendChild(exportVideosNode);
			// Save the document to a file and auto-indent the output.

			var reportFileName = "Report_" +
								 startDateTime.ToString("yyyy-MM-dd-HH-mm-ss") + "_" +
								 endDateTime.ToString("yyyy-MM-dd-HH-mm-ss") + ".xml";

			var writer = new XmlTextWriter(exportPath + reportFileName, null) { Formatting = Formatting.Indented };
			xmlDoc.Save(writer);
			writer.Close();
		}

		private void CopyPlayer()
		{
			//no xml no player
			if (AttachXmlDoc == null) return;
			//no orange player
			if(!File.Exists("Fraud Investigation.exe")) return;
			//already exist player in export forder
			if (File.Exists(_exportPath + "Fraud Investigation.exe")) return;
			//maybe player is runing or something...
			
			try
			{
				File.Copy("Fraud Investigation.exe", Path.Combine(_exportPath, "Fraud Investigation.exe"), true);
                File.Copy("Interop.WMPLib.dll", Path.Combine(_exportPath, "Interop.WMPLib.dll"), true);
                File.Copy("AxInterop.WMPLib.dll", Path.Combine(_exportPath, "AxInterop.WMPLib.dll"), true);
            }
			catch (Exception exception)
			{
				TopMostMessageBox.Show(Localization["DownloadCase_CantCopyFraudInvestigation"] + Environment.NewLine + exception,
					Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void DownloadButtonClick(Object sender, EventArgs e)
		{
			if(!CheckPath()) return;

			exportProgressBar.Visible = true;

			audioInCheckBox.Enabled = audioOutCheckBox.Enabled =
			startDatePicker.Enabled = startTimePicker.Enabled =
			endDatePicker.Enabled = endTimePicker.Enabled =
			pathTextBox.Enabled =
			commentTextBox.Enabled =
			browserButton.Enabled =
			downloadButton.Enabled =
			overlayCheckBox.Enabled =
			orangeRadioButton.Enabled =
			encodeAVIRadioButton.Enabled =
			endoceComboBox.Enabled = mbLabel.Enabled =
		   MaxiNumericUpDown.Enabled = exportMaxiSizeLabel.Enabled =
		   qualityNumericUpDown.Enabled = qualityLabel.Enabled =
		   scaleComboBox.Enabled = scaleLabel.Enabled = false;

			addedToQueueButton.Visible = false;
			
			StartDownload();
		}

		private readonly String _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		private readonly String _documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		private readonly String _picturePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
		private Boolean CheckPath()
		{
			//String rootPath;

			if (_isChangePath)
			{
				if (!Directory.Exists(pathTextBox.Text))
				{
					try
					{
						Directory.CreateDirectory(pathTextBox.Text);
					}
					catch (Exception)
					{
						var result = TopMostMessageBox.Show(Localization["ExportVideo_PathNotAvailable"], Localization["MessageBox_Information"],
							MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

						Focus();
						BringToFront();

						if (result == DialogResult.Cancel)
							return false;
					}
				}

				_rootPath = (Directory.Exists(pathTextBox.Text))
								? pathTextBox.Text
								: _desktopPath;
			}
			else
			{
				switch (Server.Configure.ExportVideoPath)
				{
					case "Desktop":
						_rootPath = _desktopPath;
						break;

					case "Document":
						_rootPath = _documentPath;
						break;

					case "Picture":
						_rootPath = _picturePath;
						break;

					default:
						if (!Directory.Exists(Server.Configure.ExportVideoPath))
						{
							try
							{
								Directory.CreateDirectory(Server.Configure.ExportVideoPath);
							}
							catch (Exception)
							{
								var result = TopMostMessageBox.Show(Localization["ExportVideo_PathNotAvailable"], Localization["MessageBox_Information"],
									MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

								Focus();
								BringToFront();

								if (result == DialogResult.Cancel)
									return false;
							}
						}

						_rootPath = (Directory.Exists(Server.Configure.ExportVideoPath))
										? Server.Configure.ExportVideoPath
										: _desktopPath;
						break;
				}
			}

			if (!Directory.Exists(_rootPath))
			{
				var result = TopMostMessageBox.Show(Localization["ExportVideo_PathNotAvailable"], Localization["MessageBox_Information"],
					MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

				Focus();
				BringToFront();

				if (result == DialogResult.Cancel)
					return false;

				_rootPath = _desktopPath;
			}

			String folderType;
			switch (((IPTS)Server).ReleaseBrand)
			{
				case "Salient":
					folderType = "TransactionTracker Video";
					break;

				default:
					folderType = "PTS Video";
					break;
			}

			//folderType += (" " + Localization["DownloadCase_Title"]);

			var slash = (_rootPath[_rootPath.Length - 1] != '\\') ? "\\" : "";

			var folderName = (slash + folderType + " (" + Server.Credential.Domain + ")\\" + Server.Server.DateTime.ToString("yyyy-MM-dd"));

			var pathIsExists = Directory.Exists(_rootPath + folderName);
			if(!pathIsExists)
			{
				try
				{
					Directory.CreateDirectory(_rootPath + folderName);
				}
				catch(Exception)
				{
					TopMostMessageBox.Show(Localization["Application_CantCreateFolder"].Replace("%1", _rootPath + folderName),
									Localization["MessageBox_Error"], MessageBoxButtons.OK,
									MessageBoxIcon.Error);
					return false;
				}
			}
			_exportPath = _rootPath + folderName;

			if (_exportPath[_exportPath.Length - 1] != '\\')
				_exportPath += "\\";

			return true;
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

			if(IsDownloading)
				TopMostMessageBox.Show(Localization["ExportVideo_ExportCancelled"], Localization["MessageBox_Information"],
					MessageBoxButtons.OK, MessageBoxIcon.Information);

			IsDownloading = false;
			_exportDevice = null;
			_watch.Stop();
			Hide();
		}

		public void ClosingExportVideoForm(Object sender, EventArgs e)
		{
			Hide();

			if (_exportDevice != null)
			{
				_exportDevice.OnExportVideoComplete -= UtilityOnExportVideoComplete;
				_exportDevice.OnExportVideoProgress -= UtilityOnExportVideoProgress;

				_exportDevice.StopExportVideo();

				IsDownloading = false;
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
			downloadButton.Visible = cancelButton.Visible = true;
			doneButton.Visible = false;

			try
			{
				Process.Start("explorer.exe", _exportPath);
			}
			catch (Exception)
			{
			}

			Hide();
		}

		private void OrangeRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if(orangeRadioButton.Checked)
			{
				qualityLabel.Visible = qualityNumericUpDown.Visible =
				scaleLabel.Visible = scaleComboBox.Visible =
				overlayCheckBox.Visible = false;
			}
		}

		private void EncodeRadioButtonCheckedChanged(Object sender, EventArgs e)
		{
			if (encodeAVIRadioButton.Checked)
			{
				qualityLabel.Visible = qualityNumericUpDown.Visible =
				scaleLabel.Visible = scaleComboBox.Visible =
				overlayCheckBox.Visible = true;
			}
		}

		private String _comments = "";
		private void CommentTextBoxEnter(Object sender, EventArgs e)
		{
			if (_comments == "")
				commentTextBox.Text = "";
		}

		private void CommentTextBoxLeave(Object sender, EventArgs e)
		{
			if (_comments == "")
			{
				commentTextBox.TextChanged -= CommentTextBoxTextChanged;

				commentTextBox.Text = Localization["DownloadCase_WriteComment"];

				commentTextBox.TextChanged += CommentTextBoxTextChanged;
			}
		}

		private void CommentTextBoxTextChanged(Object sender, EventArgs e)
		{
			_comments = commentTextBox.Text;
		}

		private void AddedToQueueButtonClick(Object sender, EventArgs e)
		{
			if (OnDownloadCaseAddToQueue == null) return;
			if (!CheckPath()) return;
			
			var config = new DownloadCaseConfig
							 {
								 StartDateTime = StartDateTime,
								 EndDateTime = EndDateTime,
								 DownloadPath = _exportPath,
								 AudioIn = audioInCheckBox.Checked,
								 AudioOut = audioOutCheckBox.Checked,
								 EncodeAVI = encodeAVIRadioButton.Checked,
								 OverlayOSD = overlayCheckBox.Checked,
								 AttachXmlDoc = (AttachXmlDoc != null) ? (XmlDocument)AttachXmlDoc.Clone() : null,
								 Comments = _comments,
							 };

			foreach (var camera in ExportDevices)
			{
				config.ExportDevices.Enqueue(camera);
			}

			OnDownloadCaseAddToQueue(this, new EventArgs<DownloadCaseConfig>(config));

			Hide();
		}
	}
}
