using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Device;
using DeviceConstant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
	public sealed partial class ListPanel : UserControl
	{
		public event EventHandler<EventArgs<IDevice>> OnDeviceEdit;
		public event EventHandler OnDeviceAdd;
		public event EventHandler OnDeviceSearch;

		public IServer Server;
		private INVR _nvr;
		public Dictionary<String, String> Localization;
		public ListPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   
								   {"SetupDevice_MaximumLicense", "Reached maximum license limit \"%1\""},
								   {"SetupDevice_AllManufacturer", "Manufacturers"},
								   {"SetupDevice_AddedDevice", "Added device"},
								   {"SetupDevice_SearchDevice", "Add device by search..."},
								   {"SetupDevice_AddNewDevice", "Add new device manually..."},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			BackgroundImage = Manager.BackgroundNoBorder;
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			addedDeviceLabel.Text = Localization["SetupDevice_AddedDevice"];
		}

		public void Initialize()
		{
			_nvr = Server as INVR;
			addNewDoubleBufferPanel.Paint += InputPanelPaint;
			addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
			searchDeviceDoubleBufferPanel.Paint += InputPanelPaint;
			searchDeviceDoubleBufferPanel.MouseClick += SearchDeviceDoubleBufferPanelMouseClick;
			
			foreach (var cameraManufactureFile in Server.Device.Manufacture)
			{
				//isap, customization can't use search
				if (String.Equals(cameraManufactureFile.Key, "iSapSolution") || String.Equals(cameraManufactureFile.Key, "Customization")) continue;

				manufactureComboBox.Items.Add(Server.Server.DisplayManufactures(cameraManufactureFile.Key));
			}

			if (manufactureComboBox.Items.Count > 1)
			{
				manufactureComboBox.Items.Insert(0, Localization["SetupDevice_AllManufacturer"]);
				Manager.DropDownWidth(manufactureComboBox);
				manufactureComboBox.SelectedIndex = 0;
				manufactureComboBox.SelectedIndexChanged += ManufactureComboBoxSelectedIndexChanged;
			}
			else if(manufactureComboBox.Items.Count == 1)
			{
				manufactureComboBox.SelectedIndex = 0;
				manufactureComboBox.Visible = false;
			}
		}

		public String SelectedManufacturer
		{
			get
			{
				if (manufactureComboBox.SelectedIndex == 0)
					return "ALL";

				var brand = manufactureComboBox.SelectedItem.ToString();

				return Server.Server.FormalManufactures(brand);
			}
		}

		private void InputPanelPaint(Object sender, PaintEventArgs e)
		{
			var control = sender as Control;
			if (control == null) return;

			Graphics g = e.Graphics;

			Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
			Manager.PaintEdit(g, addNewDoubleBufferPanel);

			if (Localization.ContainsKey("SetupDevice_" + control.Tag))
				Manager.PaintText(g, Localization["SetupDevice_" + control.Tag]);
			else
				Manager.PaintText(g, control.Tag.ToString());
		}

		private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();

		//private String _sortBy = "Id";
		//private String _sortOrder = "asc";
		private Point _previousScrollPosition = new Point(); 
		public void GenerateViewModel()
		{
			_previousScrollPosition = containerPanel.AutoScrollPosition;
			_previousScrollPosition.Y *= -1;
			ClearViewModel();
			label1.Visible = addNewDoubleBufferPanel.Visible =
				searchDeviceDoubleBufferPanel.Visible = true;

			if (Server.Device.Groups == null) return;

			var sortResult = new List<IDevice>(Server.Device.Devices.Values);
			//reverse
			sortResult.Sort((x, y) => (y.Id - x.Id));
			/*switch(_sortBy)
			{
				case "Name":
					if (_sortOrder == "asc")
						sortResult.Sort((x, y) => (String.Compare(y.Name, x.Name)));
					else
						sortResult.Sort((x, y) => (String.Compare(x.Name, y.Name)));
					break;

				case "Address":
					if (_sortOrder == "asc")
						sortResult.Sort((x, y) => ());
					else
						sortResult.Sort((x, y) => (String.Compare(x.Name, y.Name)));
					break;

				default:
					if(_sortOrder == "asc")
						sortResult.Sort((x, y) => (y.Id - x.Id));
					else
						sortResult.Sort((x, y) => (x.Id - y.Id));
					break;
			}*/

			if (sortResult.Count == 0)
			{
				addedDeviceLabel.Visible = false;
				return;
			}
			
			addedDeviceLabel.Visible = true;
			containerPanel.Visible = false;
			foreach (IDevice device in sortResult)
			{
				if (device != null && device is ICamera)
				{
					var devicePanel = GetDevicePanel();

					devicePanel.Device = device;

					containerPanel.Controls.Add(devicePanel);
				}
				//else if device is IPos or something else
			}

			var deviceTitlePanel = GetDevicePanel();
			deviceTitlePanel.IsTitle = true;
			deviceTitlePanel.Cursor = Cursors.Default;
			deviceTitlePanel.EditVisible = false;
			deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
			deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
			containerPanel.Controls.Add(deviceTitlePanel);
			containerPanel.Visible = true;

			containerPanel.AutoScroll = false;
			containerPanel.Focus();
			containerPanel.AutoScroll = true;
			containerPanel.AutoScrollPosition = _previousScrollPosition;
		}

		private DevicePanel GetDevicePanel()
		{
			if (_recycleDevice.Count > 0)
			{
				return _recycleDevice.Dequeue();
			}

			var devicePanel = new DevicePanel
			{
				SelectedColor = Manager.DeleteTextColor,
				EditVisible = true,
				SelectionVisible = false,
				Server = Server,
			};
			devicePanel.OnSelectChange += DevicePanelOnSelectChange;
			devicePanel.OnDeviceEditClick += DevicePanelOnDeviceEditClick;

			return devicePanel;
		}

		public Brush SelectedColor{
			set
			{
				foreach (DevicePanel control in containerPanel.Controls)
					control.SelectedColor = value;
			}
		}

		public void ShowCheckBox()
		{
			label1.Visible = addNewDoubleBufferPanel.Visible =
				addedDeviceLabel.Visible = searchDeviceDoubleBufferPanel.Visible = false;

			foreach (DevicePanel control in containerPanel.Controls)
			{
				control.SelectionVisible = true;
				control.EditVisible = false;
			}

			containerPanel.AutoScroll = false;
			containerPanel.Focus();
			containerPanel.AutoScroll = true;
		}

		public void RemoveSelectedDevices()
		{
			foreach (DevicePanel control in containerPanel.Controls)
			{
				if (!control.Checked || control.Device == null) continue;

				control.Device.ReadyState = ReadyState.Delete;
				Server.DeviceModify(control.Device);
				Server.Device.Devices.Remove(control.Device.Id);

				Server.WriteOperationLog("[%1] have been removed".Replace("%1", control.Device.Id.ToString()));
			}
		}

		public void CloneSelectedDevices()
		{
			var controls = new List<DevicePanel>();
			foreach (Control control in containerPanel.Controls)
			{
				if (!(control is DevicePanel)) continue;
				controls.Add(((DevicePanel)control));
			}
			
			controls.Reverse();

			foreach (DevicePanel control in controls)
			{
				if (!control.Checked || !(control.Device is ICamera)) continue;

				var copyFrom = control.Device as ICamera;

				var camera = new Camera
				{
					Server = copyFrom.Server,
					Id = Server.Device.GetNewDeviceId()
				};

				if (camera.Id == 0)
				{
					TopMostMessageBox.Show(Localization["SetupDevice_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}

				camera.Name = copyFrom.Name;
				camera.Mode = copyFrom.Mode;
				camera.ReadyState = ReadyState.JustAdd;

				camera.Profile = new CameraProfile
				{
					NetworkAddress = copyFrom.Profile.NetworkAddress,
					ConnectionTimeout = copyFrom.Profile.ConnectionTimeout,
					ReceiveTimeout = copyFrom.Profile.ReceiveTimeout,
					StreamId = copyFrom.Profile.StreamId,
					RecordStreamId = copyFrom.Profile.RecordStreamId,
					Authentication =
					{
						UserName = copyFrom.Profile.Authentication.UserName,
						Password = copyFrom.Profile.Authentication.Password,
						Encryption = copyFrom.Profile.Authentication.Encryption
					},

					HttpPort = copyFrom.Profile.HttpPort,
					TvStandard = copyFrom.Profile.TvStandard,
					SensorMode = copyFrom.Profile.SensorMode,
					PowerFrequency = copyFrom.Profile.PowerFrequency,
					AspectRatio = copyFrom.Profile.AspectRatio,
					AspectRatioCorrection = copyFrom.Profile.AspectRatioCorrection,
					DewarpType = copyFrom.Profile.DewarpType,
                    DeviceMountType = copyFrom.Profile.DeviceMountType,
                    RemoteRecovery = copyFrom.Profile.RemoteRecovery,
                    HighProfile = copyFrom.Profile.HighProfile,
                    MediumProfile = copyFrom.Profile.MediumProfile,
                    LowProfile = copyFrom.Profile.LowProfile
				};

				camera.Model = copyFrom.Model;

				if (camera.Model.Type == "CaptureCard" && copyFrom.Profile.CaptureCardConfig != null)
				{
					camera.Profile.CaptureCardConfig = new CaptureCardConfig
					{
						Brightness = copyFrom.Profile.CaptureCardConfig.Brightness,
						Contrast = copyFrom.Profile.CaptureCardConfig.Contrast,
						Hue = copyFrom.Profile.CaptureCardConfig.Hue,
						Saturation = copyFrom.Profile.CaptureCardConfig.Saturation,
						Sharpness = copyFrom.Profile.CaptureCardConfig.Sharpness,
						Gamma = copyFrom.Profile.CaptureCardConfig.Gamma,
						ColorEnable = copyFrom.Profile.CaptureCardConfig.ColorEnable,
						WhiteBalance = copyFrom.Profile.CaptureCardConfig.WhiteBalance,
						BacklightCompensation = copyFrom.Profile.CaptureCardConfig.BacklightCompensation,
						Gain = copyFrom.Profile.CaptureCardConfig.Gain,
						TemporalSensitivity = copyFrom.Profile.CaptureCardConfig.TemporalSensitivity,
						SpatialSensitivity = copyFrom.Profile.CaptureCardConfig.SpatialSensitivity,
						LevelSensitivity = copyFrom.Profile.CaptureCardConfig.LevelSensitivity,
						Speed = copyFrom.Profile.CaptureCardConfig.Speed
					};
				}

				camera.IOPort.Clear();
				foreach (var ioPort in copyFrom.IOPort)
					camera.IOPort.Add(ioPort.Key, ioPort.Value);

				camera.Profile.StreamConfigs.Clear();
				var keys = copyFrom.Profile.StreamConfigs.Keys;
				foreach (var key in keys)
				{
					camera.Profile.StreamConfigs.Add(key, StreamConfigs.Clone(copyFrom.Profile.StreamConfigs[key]));
				}

				camera.RecordSchedule = new Schedule();
				camera.RecordSchedule.SetDefaultSchedule(ScheduleType.Continuous);
				camera.RecordSchedule.Description = ScheduleMode.FullTimeRecording;

				camera.EventSchedule = new Schedule();
				camera.EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
				camera.EventSchedule.Description = ScheduleMode.FullTimeEventHandling;

				camera.EventHandling = new EventHandling();
				camera.EventHandling.SetDefaultEventHandling(camera.Model);

                if(copyFrom.Model.Manufacture == "Customization")
                {
                    camera.Profile.LiveCheckURI = copyFrom.Profile.LiveCheckURI;
                    camera.Profile.LiveCheckInterval = copyFrom.Profile.LiveCheckInterval;
                    camera.Profile.LiveCheckRetryCount = copyFrom.Profile.LiveCheckRetryCount;

                    camera.Profile.PtzCommand.Up = copyFrom.Profile.PtzCommand.Up;
                    camera.Profile.PtzCommand.Down = copyFrom.Profile.PtzCommand.Down;
                    camera.Profile.PtzCommand.Left = copyFrom.Profile.PtzCommand.Left;
                    camera.Profile.PtzCommand.Right = copyFrom.Profile.PtzCommand.Right;
                    camera.Profile.PtzCommand.UpLeft = copyFrom.Profile.PtzCommand.UpLeft;
                    camera.Profile.PtzCommand.DownLeft = copyFrom.Profile.PtzCommand.DownLeft;
                    camera.Profile.PtzCommand.UpRight = copyFrom.Profile.PtzCommand.UpRight;
                    camera.Profile.PtzCommand.DownRight = copyFrom.Profile.PtzCommand.DownRight;
                    camera.Profile.PtzCommand.Stop = copyFrom.Profile.PtzCommand.Stop;

                    camera.Profile.PtzCommand.ZoomIn = copyFrom.Profile.PtzCommand.ZoomIn;
                    camera.Profile.PtzCommand.ZoomOut = copyFrom.Profile.PtzCommand.ZoomOut;
                    camera.Profile.PtzCommand.ZoomStop = copyFrom.Profile.PtzCommand.ZoomStop;
                    camera.Profile.PtzCommand.FocusIn = copyFrom.Profile.PtzCommand.FocusIn;
                    camera.Profile.PtzCommand.FocusOut = copyFrom.Profile.PtzCommand.FocusOut;
                    camera.Profile.PtzCommand.FocusStop = copyFrom.Profile.PtzCommand.FocusStop;

                    foreach (KeyValuePair<ushort, PtzCommandCgi> point in copyFrom.Profile.PtzCommand.PresetPoints)
                    {
                        if (camera.Profile.PtzCommand.PresetPoints.ContainsKey(point.Key))
                            camera.Profile.PtzCommand.PresetPoints[point.Key] = point.Value;
                        else
                            camera.Profile.PtzCommand.PresetPoints.Add(point.Key, point.Value);
                    }

                    foreach (KeyValuePair<ushort, PtzCommandCgi> point in copyFrom.Profile.PtzCommand.GotoPresetPoints)
                    {
                        if (camera.Profile.PtzCommand.GotoPresetPoints.ContainsKey(point.Key))
                            camera.Profile.PtzCommand.GotoPresetPoints[point.Key] = point.Value;
                        else
                            camera.Profile.PtzCommand.GotoPresetPoints.Add(point.Key, point.Value);
                    }

                    foreach (KeyValuePair<ushort, PtzCommandCgi> point in copyFrom.Profile.PtzCommand.DeletePresetPoints)
                    {
                        if (camera.Profile.PtzCommand.DeletePresetPoints.ContainsKey(point.Key))
                            camera.Profile.PtzCommand.DeletePresetPoints[point.Key] = point.Value;
                        else
                            camera.Profile.PtzCommand.DeletePresetPoints.Add(point.Key, point.Value);
                    }
                }

				Server.Device.Devices.Add(camera.Id, camera);

				Server.WriteOperationLog("Clone Device %1 from %2".Replace("%1", camera.Id.ToString()).Replace("%2", copyFrom.Id.ToString()));

				Server.DeviceModify(camera);

				var allDeviceGroup = Server.Device.Groups.Values.First();
				if (allDeviceGroup == null) continue;

				if (!allDeviceGroup.Items.Contains(camera))
				{
					allDeviceGroup.Items.Add(camera);
					allDeviceGroup.Items.Sort((x, y) => (x.Id - y.Id));
				}

				if (!allDeviceGroup.View.Contains(camera))
				{
					allDeviceGroup.View.Add(camera);
					allDeviceGroup.View.Sort((x, y) => (x.Id - y.Id));
				}

				Server.GroupModify(allDeviceGroup);
			}
		}

		private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnDeviceAdd != null)
				OnDeviceAdd(this, e);
		}

		private void SearchDeviceDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnDeviceSearch!= null)
				OnDeviceSearch(this, e);
		}
		
		private void DevicePanelOnDeviceEditClick(Object sender, EventArgs e)
		{
			if (((DevicePanel)sender).Device != null)
			{
				if (OnDeviceEdit != null)
					OnDeviceEdit(this, new EventArgs<IDevice>(((DevicePanel)sender).Device));
			}
		}

		private void DevicePanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as DevicePanel;
			if (panel == null) return;

			var selectAll = false;
			if (panel.Checked)
			{
				selectAll = true;
				foreach (DevicePanel control in containerPanel.Controls)
				{
					if (control.IsTitle) continue;
					if (!control.Checked && control.Enabled)
					{
						selectAll = false;
						break;
					}
				}
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as DevicePanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= DevicePanelOnSelectAll;
				title.OnSelectNone -= DevicePanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += DevicePanelOnSelectAll;
				title.OnSelectNone += DevicePanelOnSelectNone;
			}
		}

		private void DevicePanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (DevicePanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void DevicePanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (DevicePanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}

		private void ClearViewModel()
		{
			foreach (DevicePanel devicePanel in containerPanel.Controls)
			{
				devicePanel.SelectionVisible = false;

				devicePanel.OnSelectChange -= DevicePanelOnSelectChange;
				devicePanel.Checked = false;
				devicePanel.Device = null;
				devicePanel.Cursor = Cursors.Hand;
				devicePanel.EditVisible = true;
				devicePanel.OnSelectChange += DevicePanelOnSelectChange;

				if (devicePanel.IsTitle)
				{
					devicePanel.OnSelectAll -= DevicePanelOnSelectAll;
					devicePanel.OnSelectNone -= DevicePanelOnSelectNone;
					devicePanel.IsTitle = false;
				}

				if (!_recycleDevice.Contains(devicePanel))
					_recycleDevice.Enqueue(devicePanel);
			}
			containerPanel.Controls.Clear();
		}

		private void ManufactureComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{

			if (OnDeviceSearch != null)
				OnDeviceSearch(this, e);
		}
	} 
}