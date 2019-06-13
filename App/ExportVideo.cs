using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using ExportVideoForm;
using Interface;
using PanelBase;

namespace App
{
	public class ExportVideoForm
	{
		public Dictionary<String, String> Localization;
	    protected ExportVideo _exportVideo;
		public IApp App;
	    
        public String ExportInformation;

		public Boolean IsExporting { get { return _exportVideo.IsExporting; } }

		public ExportVideoForm()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   
								   {"ExportVideo_AddDeviceBeforeExport", "Select device before export video."},
								   {"ExportVideo_ExportVideoInProgress", "Export video already in progress.\r\nPlease complete or cancel previous operation."},
								   {"ExportVideo_NoPermissionToExport", "You don't have permission to export the following devices video."},
								   {"ExportVideo_CantExportDeviceLayout", "Can't export device layout."},
							   };
			Localizations.Update(Localization);

            _exportVideo = new ExportVideo();
		}

		public void StopExport()
		{
			_exportVideo.Hide();
			_exportVideo.StopExport();
		}

		public void ExportVideo(IServer server, IDevice[] usingDevices, DateTime start, DateTime end)
		{
			_exportVideo.OnClosed += ExportVideoOnClosed;
		    _exportVideo.App = App;
			_exportVideo.Server = server;
			_exportVideo.Icon = server.Form.Icon;

			if(_exportVideo.Visible)
			{
				TopMostMessageBox.Show(Localization["ExportVideo_ExportVideoInProgress"], Localization["MessageBox_Information"],
								   MessageBoxButtons.OK, MessageBoxIcon.Information);

				_exportVideo.Show();
				_exportVideo.BringToFront();

				return;
			}

			if (usingDevices == null)
			{
				TopMostMessageBox.Show(Localization["ExportVideo_AddDeviceBeforeExport"], Localization["MessageBox_Information"],
								   MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (!_exportVideo.IsExporting)
			{
				_exportVideo.ExportDevices.Clear();

				var noPermission = new List<String>();
				var deviceLayoutDevice = new List<String>();
				
				foreach (ICamera camera in usingDevices)
				{
					if (camera == null) continue;
					
					if (camera is IDeviceLayout)
					{
						deviceLayoutDevice.Add(camera.ToString());
						continue;
					}

					if (camera is ISubLayout)
					{
						deviceLayoutDevice.Add(camera.ToString());
						continue;
					}

					if (!camera.CheckPermission(Permission.ExportVideo))
					{
						noPermission.Add(camera.ToString());
						continue;
					}

					_exportVideo.ExportDevices.Enqueue(camera);
				}

				if (noPermission.Count > 0)
				{
					DialogResult result = TopMostMessageBox.Show(Localization["ExportVideo_NoPermissionToExport"] +
									Environment.NewLine + @"""" + String.Join(",", noPermission.ToArray()) + @"""", Localization["MessageBox_Information"],
									MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

					if (result == DialogResult.Cancel) return;
					if (_exportVideo.ExportDevices.Count == 0) return;
				}
				
				if (deviceLayoutDevice.Count > 0)
				{
					DialogResult result = TopMostMessageBox.Show(Localization["ExportVideo_CantExportDeviceLayout"] +
									Environment.NewLine + @"""" + String.Join(",", deviceLayoutDevice.ToArray()) + @"""", Localization["MessageBox_Information"],
									MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

					if (result == DialogResult.Cancel) return;
					if (_exportVideo.ExportDevices.Count == 0) return;
				}

				if (_exportVideo.ExportDevices.Count == 0)
				{
					TopMostMessageBox.Show(Localization["ExportVideo_AddDeviceBeforeExport"], Localization["MessageBox_Information"],
									   MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				_exportVideo.StartDateTime = start;
				_exportVideo.EndDateTime = end;

			    _exportVideo.ExportInformation = ExportInformation;

				_exportVideo.Initialize();
			}

			// Add By Tulip for DVR Mode
			if (App.IsLock)
			{
				App.Form.TopMost = false;
				_exportVideo.TopMost = true;
			}
			
			_exportVideo.Show();
			_exportVideo.BringToFront();
		}

	    protected virtual void ExportVideoOnClosed(Object sender, EventArgs e)
		{
			if (!App.IsLock) return;

			// Add By Tulip for DVR Mode
			App.Form.TopMost = true;
			_exportVideo.TopMost = false;
		}
	}
}
