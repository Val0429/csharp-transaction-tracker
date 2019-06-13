using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace DownloadCaseForm
{
	public sealed partial class DownloadCaseQueue : Form
	{
		public IServer Server;
		public List<DownloadCaseConfig> DownloadCaseConfigs = new List<DownloadCaseConfig>();
		public List<DownloadCaseConfig> DownloadCaseCompletedConfigs = new List<DownloadCaseConfig>();

		public Dictionary<String, String> Localization;

		private readonly System.Timers.Timer _startDownloadTimer = new System.Timers.Timer();
		private BackgroundWorker _downloadCaseBackgroundWorker;

		public DownloadCaseQueue()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Error", "Error"},

								   {"Common_Close", "Close"},
								   
								   {"ExportVideo_Start", "Start"},
								   {"ExportVideo_End", "End"},
								   
								   {"DownloadCase_CantCopyFraudInvestigation", "Can't copy 'Fraud Investigation.exe'."},

								   {"DownloadCaseQueue_Delete", "Delete"},
								   {"DownloadCaseQueue_DeleteSelectedItem", "Delete selected item"},
								   
								   {"DownloadCaseQueue_Title", "Export Case Queue"},
								   {"DownloadCaseQueue_EnableScheduleDownload", "Enable schedule export"},
							   };
			Localizations.Update(Localization);
			DoubleBuffered = true;

			InitializeComponent();
			BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.IMGControllerBG);

			_startDownloadTimer.Elapsed += StartDownload;
			_startDownloadTimer.SynchronizingObject = this; 

			Text = Localization["DownloadCaseQueue_Title"];
			closeButton.Text = Localization["Common_Close"];
			deleteButton.Text = Localization["DownloadCaseQueue_Delete"];
			deleteSelectedButton.Text = Localization["DownloadCaseQueue_DeleteSelectedItem"];
			
			enableScheduleCheckBox.Text = Localization["DownloadCaseQueue_EnableScheduleDownload"];
			startLabel.Text = Localization["ExportVideo_Start"];
			endLabel.Text = Localization["ExportVideo_End"];

			startTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
			endTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
			enableScheduleCheckBox.CheckedChanged += EnableScheduleCheckBoxCheckedChanged;

			deleteSelectedButton.Location = new Point(deleteSelectedButton.Location.X, deleteButton.Location.Y);

			addNewDoubleBufferPanel.Paint += AddNewDoubleBufferPanelPaint;
		}

		private void StartDownload(Object sender, EventArgs e)
		{
			_startDownloadTimer.Enabled = false;

			if (_downloadCaseBackgroundWorker == null)
			{
				_downloadCaseBackgroundWorker = new BackgroundWorker();
				_downloadCaseBackgroundWorker.DoWork += DownloadCase;
			}

			if (!_downloadCaseBackgroundWorker.IsBusy)
				_downloadCaseBackgroundWorker.RunWorkerAsync();
		}

		private Boolean _isExporting;
		private void DownloadCase(Object sender, DoWorkEventArgs e)
		{
			if (_isExporting) return;

			_startDownloadTimer.Enabled = false;
			_exportFileName.Clear();
			_completedCount = 0;

			DownloadCase();
		}

		private DownloadCaseConfig _config;
		private void DownloadCase()
		{
			if (DownloadCaseConfigs.Count == 0) return;
			_config = DownloadCaseConfigs.First();

			if (_config.ExportDevices.Count > 0)
			{
				_isExporting = true;
				_config.Status = QueueStatus.Downloading;
				ExportDevice(_config.ExportDevices.Dequeue());
			}
			else
			{
				DownloadCaseCompletedConfigs.Add(_config);
				DownloadCaseConfigs.Remove(_config);
				_config.Status = QueueStatus.Completed;
			}

			containerPanel.Invalidate();
		}

		private ICamera _exportDevice;
		private readonly Dictionary<ICamera, String> _exportFileName = new Dictionary<ICamera, String>();
		private void ExportDevice(ICamera camera)
		{
			_exportDevice = camera;
			if (_exportDevice != null)
			{
				camera.OnExportVideoProgress -= UtilityOnExportVideoProgress;
				camera.OnExportVideoProgress += UtilityOnExportVideoProgress;
				camera.OnExportVideoComplete -= UtilityOnExportVideoComplete;
				camera.OnExportVideoComplete += UtilityOnExportVideoComplete;

				var startTimecode = DateTimes.ToUtc(_config.StartDateTime, camera.Server.Server.TimeZone);
				var endTimecode = DateTimes.ToUtc(_config.EndDateTime, camera.Server.Server.TimeZone);

                var fileName = camera.ExportVideo(startTimecode, endTimecode, _config.OverlayOSD, _config.AudioIn, _config.AudioOut, (ushort)((_config.EncodeAVI) ? 2 : 1), _config.DownloadPath, 60, 0, 1);//type 0:raw, 1:original, 2:mjpeg
				if (!_exportFileName.ContainsKey(camera))
					_exportFileName.Add(camera, fileName);
				else
					_exportFileName[camera] = fileName;
			}
		}

		private UInt16 _completedCount;
		private void UtilityOnExportVideoComplete(Object sender, EventArgs e)
		{
			_completedCount++;

			if (ExportNextIfAvailable()) return;

			if (_completedCount > 0)
			{
				DownloadCaseForm.DownloadCase.SaveAttachXmlDoc(_config.AttachXmlDoc, Server, _config.StartDateTime, _config.EndDateTime, _config.DownloadPath, _config.Comments, _exportFileName);
				CopyPlayer();
			}
			
			DownloadCaseCompletedConfigs.Add(_config);
			DownloadCaseConfigs.Remove(_config);
			if (_completedCount > 0)
				_config.Status = QueueStatus.Completed;
			else
				_config.Status = QueueStatus.Failed;
			
			_isExporting = false;
			containerPanel.Invalidate();

			if(DownloadCaseConfigs.Count == 0) return;

			//more to download

			//count down
			var now = (DateTime.Now.Hour * 60 + DateTime.Now.Minute); //secs
			var start = (startTimePicker.Value.Hour * 60 + startTimePicker.Value.Minute);
			var end = (endTimePicker.Value.Hour * 60 + endTimePicker.Value.Minute);
			
			//normal
			var doNext = false;
			if (end >= start)
			{
				if (now >= start && now < end)
					doNext = true;
			}
			else //cross day
			{
				if (now >= start || now < end)
					doNext = true;
			}

			if (doNext)
			{
				_downloadCaseBackgroundWorker = new BackgroundWorker();
				_downloadCaseBackgroundWorker.DoWork += DownloadCase;
				_downloadCaseBackgroundWorker.RunWorkerAsync();
			}
		}

		private void CopyPlayer()
		{
			//no xml no player
			if (_config.AttachXmlDoc == null) return;
			//no orange player
			if (!File.Exists("Fraud Investigation.exe")) return;
			//already exist player in export forder
			if (File.Exists(_config.DownloadPath + "Fraud Investigation.exe")) return;
			//maybe player is runing or something...

			try
			{
				File.Copy("Fraud Investigation.exe", Path.Combine(_config.DownloadPath, "Fraud Investigation.exe"), true);
			}
			catch (Exception exception)
			{
				TopMostMessageBox.Show(Localization["DownloadCase_CantCopyFraudInvestigation"] + Environment.NewLine + exception,
					Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void UtilityOnExportVideoProgress(Object sender, EventArgs<UInt16, ExportVideoStatus> e)
		{
			if (_exportDevice == null)
				return;

			//check if failure
			if (e.Value2 == ExportVideoStatus.ExportFailed)
			{
				_completedCount--; //it will be ++ in completed
				UtilityOnExportVideoComplete(sender, null);
				return;
			}

			//if (!_exportDevice.Server.Device.Devices.ContainsValue(_exportDevice))
			//{
			//    _exportDevice.StopExportVideo();

			//    if (ExportNextIfAvailable()) return;
			//}
		}

		private Boolean ExportNextIfAvailable()
		{
			_exportDevice.OnExportVideoComplete -= UtilityOnExportVideoComplete;
			_exportDevice.OnExportVideoProgress -= UtilityOnExportVideoProgress;

			if (_config.ExportDevices.Count > 0)
			{
				var device = _config.ExportDevices.Dequeue();
				while (!device.Server.Device.Devices.ContainsValue(device) && _config.ExportDevices.Count > 0)
				{
					device = _config.ExportDevices.Dequeue();
				}

				if (device.Server.Device.Devices.ContainsValue(device))
				{
					ExportDevice(device);
					return true;
				}
			}

			_exportDevice = null;

			return false;
		}

		private void AddNewDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
		{
			Manager.PaintSingleInput(e.Graphics, addNewDoubleBufferPanel);
		}

		private readonly Queue<DownloadCasePanel> _recycleDownloadCase = new Queue<DownloadCasePanel>();

		public void RefreshQueue()
		{
			ClearViewModel();

			var configs = new List<DownloadCaseConfig>();

			configs.AddRange(DownloadCaseCompletedConfigs);
			configs.AddRange(DownloadCaseConfigs);

			configs.Reverse();
			var count = configs.Count;
			foreach (var config in configs)
			{
				config.Id = count--;

				var downloadCasePanel = GetDownloadCasePanel();

				downloadCasePanel.Config = config;
				
				containerPanel.Controls.Add(downloadCasePanel);
			}

			var configTitlePanel = GetDownloadCasePanel();
			configTitlePanel.IsTitle = true;
			configTitlePanel.OnSelectAll += DownloadCasePanelOnSelectAll;
			configTitlePanel.OnSelectNone += DownloadCasePanelOnSelectNone;
			configTitlePanel.Cursor = Cursors.Default;
			containerPanel.Controls.Add(configTitlePanel);

			if(enableScheduleCheckBox.Checked)
				CheckIfDownloadCase();
		}

		private DownloadCasePanel GetDownloadCasePanel()
		{
			if (_recycleDownloadCase.Count > 0)
			{
				return _recycleDownloadCase.Dequeue();
			}

			var downloadCasePanel = new DownloadCasePanel();
			downloadCasePanel.OnSelectChange += DownloadCasePanelOnSelectChange;

			return downloadCasePanel;
		}

		private void ClearViewModel()
		{
			foreach (DownloadCasePanel downloadCasePanel in containerPanel.Controls)
			{
				downloadCasePanel.Checked = false;
				downloadCasePanel.SelectionVisible = false;
				if (downloadCasePanel.IsTitle)
				{
					downloadCasePanel.IsTitle = false;

					downloadCasePanel.OnSelectAll -= DownloadCasePanelOnSelectAll;
					downloadCasePanel.OnSelectNone -= DownloadCasePanelOnSelectNone;
				}
				downloadCasePanel.Config = null;

				if (!_recycleDownloadCase.Contains(downloadCasePanel))
					_recycleDownloadCase.Enqueue(downloadCasePanel);
			}

			containerPanel.Controls.Clear();
		}

		private void DownloadCasePanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as DownloadCasePanel;
			if (panel == null) return;

			var selectAll = false;
			if (panel.Checked)
			{
				selectAll = true;
				foreach (DownloadCasePanel control in containerPanel.Controls)
				{
					if (control.IsTitle) continue;
					if (!control.Checked && control.Enabled)
					{
						selectAll = false;
						break;
					}
				}
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as DownloadCasePanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= DownloadCasePanelOnSelectAll;
				title.OnSelectNone -= DownloadCasePanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += DownloadCasePanelOnSelectAll;
				title.OnSelectNone += DownloadCasePanelOnSelectNone;
			}
		}

		private void DownloadCasePanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (DownloadCasePanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void DownloadCasePanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (DownloadCasePanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}

		private void EnableScheduleCheckBoxCheckedChanged(Object sender, EventArgs e)
		{
			if (enableScheduleCheckBox.Checked)
			{
				startTimePicker.Enabled = endTimePicker.Enabled = false;
				CheckIfDownloadCase();
			}
			else
			{
				foreach (var downloadCaseConfig in DownloadCaseConfigs)
				{
					if (downloadCaseConfig.Status == QueueStatus.Queue)
						downloadCaseConfig.Status = QueueStatus.Stop;
				}

				containerPanel.Invalidate();
				startTimePicker.Enabled = endTimePicker.Enabled = true;
				_startDownloadTimer.Enabled = false;
			}
		}

		private void CheckIfDownloadCase()
		{
			//already enable
			if(_startDownloadTimer.Enabled) return;

			foreach (var downloadCaseConfig in DownloadCaseConfigs)
			{
				if (downloadCaseConfig.Status == QueueStatus.Stop)
					downloadCaseConfig.Status = QueueStatus.Queue;
			}
			containerPanel.Invalidate();

			//count down
			var now = (DateTime.Now.Hour * 60 + DateTime.Now.Minute); //secs
			var start = (startTimePicker.Value.Hour * 60 + startTimePicker.Value.Minute);
			if (now >= start) //already start
			{
				if (_downloadCaseBackgroundWorker == null)
				{
					_downloadCaseBackgroundWorker = new BackgroundWorker();
					_downloadCaseBackgroundWorker.DoWork += DownloadCase;
				}

				if (!_downloadCaseBackgroundWorker.IsBusy)
					_downloadCaseBackgroundWorker.RunWorkerAsync();
			}
			else
			{
				_startDownloadTimer.Interval = (start - now) * 60000;
				_startDownloadTimer.Enabled = true;
			}
		}

		private void CloseButtonClick(Object sender, EventArgs e)
		{
			deleteSelectedButton.Visible = false;
			deleteButton.Visible = true;

			foreach (DownloadCasePanel control in containerPanel.Controls)
			{
				control.SelectionVisible = false;
			}

			Hide();
		}

		private void DeleteButtonClick(Object sender, EventArgs e)
		{
			if (containerPanel.Controls.Count <= 1) return;

			var showCheckBox = false;
			DownloadCasePanel titlePanel = null;
			foreach (DownloadCasePanel control in containerPanel.Controls)
			{
				if (control.IsTitle)
				{
					titlePanel = control;
					continue;
				}

				if (control.Config != null && control.Config.Status == QueueStatus.Downloading) continue;

				control.SelectionVisible = true;
				showCheckBox = true;
			}

			if (showCheckBox && titlePanel != null)
			{
				deleteSelectedButton.Visible = true;
				deleteButton.Visible = false;

				titlePanel.SelectionVisible = true;
			}
		}

		private void DeleteSelectedButtonClick(Object sender, EventArgs e)
		{
			deleteSelectedButton.Visible = false;
			deleteButton.Visible = true;

			var isChange = false;
			foreach (DownloadCasePanel control in containerPanel.Controls)
			{
				control.SelectionVisible = false;

				if (control.IsTitle) continue;
				if (!control.Checked || !control.Enabled || control.Config == null) continue;
				//can't delete "Downloading" config
				if (control.Config.Status == QueueStatus.Downloading) continue;

				DownloadCaseConfigs.Remove(control.Config);
				DownloadCaseCompletedConfigs.Remove(control.Config);

				isChange = true;
			}

			if (isChange)
				RefreshQueue();
		}
	}
}
