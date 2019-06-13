using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
	public partial class VideoMenu
	{
		private Button _opticalPTZButton;
		protected Button _manualRecordButton;
		private Button _panicButton;

		private static readonly Image _opticalptz = Resources.GetResources(Properties.Resources.opticalptz, Properties.Resources.IMGOpticalptz);
		private static readonly Image _opticalptzactivate = Resources.GetResources(Properties.Resources.opticalptz_activate, Properties.Resources.IMGOpticalptz_activate);
		private static readonly Image _record = Resources.GetResources(Properties.Resources.record, Properties.Resources.IMGRecord);
		private static readonly Image _recordactivate = Resources.GetResources(Properties.Resources.record_activate, Properties.Resources.IMGRecord_activate);
		//private static readonly Image _panic = Resources.GetResources(Properties.Resources.panic, Properties.Resources.IMGPanic);
		private static readonly Image _panicActive = Resources.GetResources(Properties.Resources.panic_activate, Properties.Resources.IMGPanic_activate);

		private void OpticalPTZButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if ((String.Equals(_opticalPTZButton.Tag.ToString(), "Inactivate")))
			{
				_opticalPTZButton.Tag = "Activate";
				_opticalPTZButton.BackgroundImage = _opticalptzactivate;
				if (VideoWindow != null)
					VideoWindow.PtzMode = PTZMode.Optical;

				if (OnButtonClick != null)
					OnButtonClick(this, new EventArgs<String>(SelectionChangeXml(_opticalPTZButton.Name, _opticalPTZButton.Tag.ToString())));

				if (VideoWindow != null)
				{
					VideoWindow.Viewer.Focus();

					Server.WriteOperationLog("Device %1 Enable PTZ".Replace("%1", VideoWindow.Camera.Id.ToString()));
				}
			}
			else
			{
				_opticalPTZButton.Tag = "Inactivate";
				_opticalPTZButton.BackgroundImage = _opticalptz;
				if (VideoWindow != null)
				{
					VideoWindow.PtzMode = PTZMode.Digital;

					Server.WriteOperationLog("Device %1 Disable PTZ".Replace("%1", VideoWindow.Camera.Id.ToString()));
				}

				if (OnButtonClick != null)
					OnButtonClick(this, new EventArgs<String>(SelectionChangeXml(_opticalPTZButton.Name, _opticalPTZButton.Tag.ToString())));
			}
		}

		private delegate void ManualRecordDelegate();
		protected virtual void ManualRecordButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if (VideoWindow != null && VideoWindow.Camera != null)
			{
				if (VideoWindow.Camera is IDeviceLayout)
				{
					var deviceLayout = VideoWindow.Camera as IDeviceLayout;
					
					foreach (var device in deviceLayout.Items)
					{
						var camera = device as ICamera;
						if (camera == null) continue;
						if (camera.Status == CameraStatus.Nosignal) continue;
						if (!camera.CheckPermission(Permission.ManualRecord)) continue;

						camera.IsManualRecord = true;

						//it call CGI to activate manual record, when server busy, CGI response will slow -> cause AP freeze.
						ManualRecordDelegate manualRecordDelegate = camera.ManualRecord;
						manualRecordDelegate.BeginInvoke(null, null);

						Server.WriteOperationLog("Device %1 Start Manual Record".Replace("%1", camera.Id.ToString()));
					}
				}
				else
				{
					VideoWindow.Camera.IsManualRecord = true;

					//it call CGI to activate manual record, when server busy, CGI response will slow -> cause AP freeze.
					ManualRecordDelegate manualRecordDelegate = VideoWindow.Camera.ManualRecord;
					manualRecordDelegate.BeginInvoke(null, null);

					Server.WriteOperationLog("Device %1 Start Manual Record".Replace("%1", VideoWindow.Camera.Id.ToString()));
				}

				//VideoWindow.Device.ManualRecord();

				//_manualRecordButton.BackgroundImage = (VideoWindow.Camera.IsManualRecord) ? _recordactivate : _record;
				var getRecordStatusTimer = new Timer { Interval = 50 };
				var count = 10;
				getRecordStatusTimer.Tick += (sender2, e2) =>
				{
					//after 0.5 sec still not get record event
					count--;
					if (count < 0)
					{
						getRecordStatusTimer.Enabled = false;
						getRecordStatusTimer = null;
					}
					
					var camera = VideoWindow.Camera;

					if (VideoWindow.Camera is IDeviceLayout)
					{
						foreach (var device in ((IDeviceLayout)VideoWindow.Camera).Items.Where(device => device != null))
						{
							camera = device as ICamera;
							break;
						}
					}
					//after timer, window's camera could be null
					if (camera == null || !camera.IsManualRecord) return;

					_manualRecordButton.BackgroundImage = _recordactivate;
					getRecordStatusTimer.Enabled = false;
					getRecordStatusTimer = null;
				};
				getRecordStatusTimer.Enabled = true;
			}
		}

		private delegate void PanicDelegate();
		private void PanicButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if (VideoWindow == null || VideoWindow.Camera == null) return;
			VideoWindow.Camera.Panic();
			
			//PanicDelegate panicDelegate = VideoWindow.Camera.Panic;
			//panicDelegate.BeginInvoke(null, null);
		}

		protected virtual void UpdateManualRecordButton()
		{
			if (_manualRecordButton == null || VideoWindow.Camera == null) return;

			var showManualRecordBtn = (VideoWindow.Viewer.Visible && VideoWindow.PlayMode == PlayMode.LiveStreaming);
			 
			if (showManualRecordBtn)
			{
				if (VideoWindow.Camera is IDeviceLayout)
				{
					showManualRecordBtn = true;
				}
				else if (VideoWindow.Camera is ISubLayout)
				{
					showManualRecordBtn = false;
				}
				else
				{
					showManualRecordBtn = VideoWindow.Camera.CheckPermission(Permission.ManualRecord);

					//check if status is not nosingle,if no single dont show manual record btn
					if (showManualRecordBtn && VideoWindow.Camera.Status == CameraStatus.Nosignal)
						showManualRecordBtn = false;
				}		   
			}

			if (showManualRecordBtn)
			{
				var camera = VideoWindow.Camera;

				if (VideoWindow.Camera is IDeviceLayout)
				{
					camera = null;
					foreach (var device in ((IDeviceLayout)VideoWindow.Camera).Items.Where(device => device != null))
					{
						camera = device as ICamera;
						break;
					}
				}

				if (camera == null)
				{
					Controls.Remove(_manualRecordButton);
					return;
				}

				_manualRecordButton.BackgroundImage = (camera.IsManualRecord) ? _recordactivate : _record;

				SetButtonPosition(_manualRecordButton);
				Controls.Add(_manualRecordButton);
				_count++;
			}
			else
			{
				Controls.Remove(_manualRecordButton);
			}
		}

		protected void UpdateOpticalPTZButton()
		{
			if (_opticalPTZButton == null) return;
			if (VideoWindow.Camera == null || VideoWindow.Camera.Model == null)
			{
				Controls.Remove(_opticalPTZButton);
				return;
			}

			if (VideoWindow.Viewer.Visible && VideoWindow.Track == null && VideoWindow.Camera.Model.IsSupportPTZ
				&& VideoWindow.Camera.CheckPermission(Permission.OpticalPTZ))
			{
				ChangeOpticalPTZButton();
				SetButtonPosition(_opticalPTZButton);
				Controls.Add(_opticalPTZButton);
				_count++;
			}
			else
			{
				Controls.Remove(_opticalPTZButton);
				//_opticalPTZButton.BackgroundImage = _opticalptz;
				//_opticalPTZButton.Tag = "Inactivate";
			}
		}

		private void ChangeOpticalPTZButton()
		{
			if (_opticalPTZButton == null || VideoWindow.Camera == null) return;

			if (VideoWindow.PtzMode == PTZMode.Optical)
			{
				_opticalPTZButton.BackgroundImage = _opticalptzactivate;
				_opticalPTZButton.Tag = "Activate";
			}
			else
			{
				_opticalPTZButton.BackgroundImage = _opticalptz;
				_opticalPTZButton.Tag = "Inactivate";
			}
		}

		protected void UpdatePanicButton()
		{
			if (_panicButton == null) return;

			if (VideoWindow.Camera == null || VideoWindow.Camera.EventHandling == null)
			{
				Controls.Remove(_panicButton);
				return;
			}

			var panicHandel = VideoWindow.Camera.EventHandling.GetEventHandleViaCameraEvent(EventType.Panic, 1, true);
			if (VideoWindow.Viewer.Visible && panicHandel != null && panicHandel.Count > 0)
			{
				SetButtonPosition(_panicButton);
				Controls.Add(_panicButton);
				_count++;
			}
			else
			{
				Controls.Remove(_panicButton);
			}
		}
	}
}