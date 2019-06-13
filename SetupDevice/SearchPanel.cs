using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;
using ApplicationForms = App.ApplicationForms;
using Manager = SetupBase.Manager;

namespace SetupDevice
{
	public sealed partial class SearchPanel : UserControl
	{
		public event EventHandler OnSearchStart;
		public event EventHandler OnSearchComplete;
		public event EventHandler OnDeviceModify;

		public IServer Server;
		public Dictionary<String, String> Localization;
		public Queue<String> Manufacturers = new Queue<String>();
		//private readonly Timer _dotTimer = new Timer();

		public SearchPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Data_NotSupport", "Not Support"},

								   {"MessageBox_Information", "Information"},
								   {"Common_UsedSeconds", "(%1 seconds)"},
								   
								   {"SetupDevice_AllManufacturer", "Manufacturers"},
								   {"SetupDevice_MaximumLicense", "Reached maximum license limit \"%1\""},
								   {"SetupDevice_SearchingDevices", "Searching Devices"},
								   {"SetupDevice_SearchResult", "Search Result"},
								   {"SetupDevice_SearchNoResult", "No Device Found"},
								   {"SetupDevice_SearchDeviceFound", "%1 Device Found"},
								   {"SetupDevice_SearchDevicesFound", "%1 Devices Found"},
								   {"SetupDevice_DeviceSelected", "%1 Device Selected"},
								   {"SetupDevice_DevicesSelected", "%1 Devices Selected"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			BackgroundImage = Manager.BackgroundNoBorder;
			DoubleBuffered = true;
			Dock = DockStyle.None;

			manufacturesPanel.Paint += ManufacturesPanelPaint;
		}

		private void ManufacturesPanelPaint(Object sender, PaintEventArgs e)
		{
			var g = e.Graphics;

			g.DrawLine(Pens.DarkGray, 0, manufacturesPanel.Height - 1, manufacturesPanel.Width, manufacturesPanel.Height - 1);
		}

		private readonly List<CheckBox> _manufacturesCheckBox = new List<CheckBox>();
		public void Initialize()
		{
			manufacturesLabel.Text = Localization["SetupDevice_AllManufacturer"];
			var manufactures = Server.Device.Manufacture.Keys;
			foreach (var manufacture in manufactures)
			{
				//isap, customization can't use search
				if (String.Equals(manufacture, "iSapSolution") || String.Equals(manufacture, "Customization")) continue;

				var checkBox = new CheckBox
								   {
									   Text = Server.Server.DisplayManufactures(manufacture),
									   Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
									   MinimumSize = new Size(115, 0),
									   AutoSize = true,
									   Padding = new Padding(0, 4, 0, 0)
								   };
				_manufacturesCheckBox.Add(checkBox);
				manufacturesPanel.Controls.Add(checkBox);
			}
		}

		private readonly Queue<Label> _recycleLabels = new Queue<Label>();
		private  Label GetResultLabel()
		{
			if (_recycleLabels.Count > 0)
			{
				return _recycleLabels.Dequeue();
			}

			return new Label
			{
				Dock = DockStyle.Top,
				Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
				Padding = new Padding(8, 10, 0, 2),
				Size = new Size(456, 30),
				TextAlign = ContentAlignment.BottomLeft,
				ForeColor = SystemColors.ControlDarkDark
			};
		}

		private readonly Queue<DeviceResultPanel> _recyclePanels = new Queue<DeviceResultPanel>();
		private DeviceResultPanel GetResultPanel()
		{
			if (_recyclePanels.Count > 0)
			{
				return _recyclePanels.Dequeue();
			}

			return new DeviceResultPanel
			{
				AutoSize = true,
				Dock = DockStyle.Top,
				MinimumSize = new Size(0, 15),
				Size = new Size(456, 15)
			};
		}

		private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();
		private DevicePanel GetDevicePanel()
		{
			if (_recycleDevice.Count > 0)
			{
				return _recycleDevice.Dequeue();
			}

			var devicePanel = new DevicePanel
			{
				SelectionVisible = true,
				DataType = "SearchResult",
				Server = Server,
			};
			devicePanel.OnSelectChange += DevicePanelOnSelectChange;

			return devicePanel;
		}

		public void ApplyManufactures(String manufacturer)
		{
			foreach (var checkBox in _manufacturesCheckBox)
			{
				checkBox.Checked = (String.Equals(checkBox.Text, Server.Server.DisplayManufactures(manufacturer))); //String.Equals(manufacturer, "ALL")
			}
		}

		private Boolean _isSearching;
		private readonly Queue<Label> _noResultLabels = new Queue<Label>();
		private delegate List<ICamera> SearchDeviceDelegate(String manufacturer);
		private readonly Stopwatch _watch = new Stopwatch();
		private String _searchingManufacture;
		public void SearchDevice()
		{
			if (_isSearching) return;
			
			_isEditing = false;

			if (Server.Device.Groups == null) return;

			Manufacturers.Clear();
			
			foreach (var checkBox in _manufacturesCheckBox)
			{
				if (checkBox.Checked)
				{
					Manufacturers.Enqueue(Server.Server.FormalManufactures(checkBox.Text));
				}
			}

			if (Manufacturers.Count == 0)
			{
				ClearViewModel();
				return;
			}

			ClearViewModel();

			ApplicationForms.ShowLoadingIcon(Server.Form);

			_noResultLabels.Clear();
			_isSearching = true;
			_watch.Reset();
			_watch.Start();
			_searchingManufacture = Manufacturers.Dequeue();
			SearchDeviceDelegate searchDeviceDelegate = Server.Device.Search;
			searchDeviceDelegate.BeginInvoke(_searchingManufacture.Replace("-", ""), SearchDeviceCallback, searchDeviceDelegate);

			if(OnSearchStart != null)
				OnSearchStart(this, null);
		}

		private delegate void SearchDeviceCallbackDelegate(IAsyncResult result);
		private void SearchDeviceCallback(IAsyncResult result)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new SearchDeviceCallbackDelegate(SearchDeviceCallback), result);
				}
				catch (Exception)
				{
					_watch.Stop();
					SearchCompleted();
				}
				return;
			}

			var searchResult = ((SearchDeviceDelegate)result.AsyncState).EndInvoke(result);
			
			_watch.Stop();

			var searchPanel = GetResultPanel();
			searchPanel.ResultLabel = GetResultLabel();

			containerPanel.Controls.Add(searchPanel.ResultLabel);
			searchPanel.ResultLabel.BringToFront();

			if (searchResult.Count == 0)
			{
			    searchPanel.ResultLabel.Text = Server.Server.DisplayManufactures(_searchingManufacture) + @" - " + Localization["SetupDevice_SearchNoResult"];
					 //+ @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));
				_noResultLabels.Enqueue(searchPanel.ResultLabel);
			}
			else
			{
				searchPanel.ResultLabel.Tag = Server.Server.DisplayManufactures(_searchingManufacture) + @" - ";
				searchPanel.ResultLabel.Tag += ((searchResult.Count == 1)
							? Localization["SetupDevice_SearchDeviceFound"]
							: Localization["SetupDevice_SearchDevicesFound"]).Replace("%1", searchResult.Count.ToString());

				//searchPanel.ResultLabel.Tag += @" " + Localization["Common_UsedSeconds"].Replace("%1", _watch.Elapsed.TotalSeconds.ToString("0.00"));

				searchPanel.ResultLabel.Text = searchPanel.ResultLabel.Tag.ToString();

				searchResult.Sort((x, y) => (y.Id - x.Id));

				containerPanel.Controls.Add(searchPanel);
				searchPanel.BringToFront();

				foreach (ICamera camera in searchResult)
				{
					DevicePanel devicePanel = GetDevicePanel();

					devicePanel.Device = camera;

					searchPanel.Controls.Add(devicePanel);
					//else if device is IPos or something else
				}

				var deviceTitlePanel = GetDevicePanel();
				deviceTitlePanel.IsTitle = true;
				deviceTitlePanel.Cursor = Cursors.Default;
				deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
				deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
				searchPanel.Controls.Add(deviceTitlePanel);
			}

			if (Manufacturers.Count > 0)
			{
				_watch.Reset();
				_watch.Start();
				_searchingManufacture = Manufacturers.Dequeue();
				SearchDeviceDelegate searchDeviceDelegate = Server.Device.Search;
                searchDeviceDelegate.BeginInvoke(_searchingManufacture.Replace("-", ""), SearchDeviceCallback, searchDeviceDelegate);
				return;
			}

			//group no-result labels
			foreach (var noResultLabel in _noResultLabels)
			{
				noResultLabel.BringToFront();
			}
			_noResultLabels.Clear();
			SearchCompleted();

		}
		private void SearchCompleted()
		{
			_isEditing = true;
			_isSearching = false;
			if (OnSearchComplete != null)
				OnSearchComplete(this, null);

			ApplicationForms.HideLoadingIcon();
			containerPanel.AutoScroll = false;
			containerPanel.Select();
			containerPanel.AutoScrollPosition = new Point(0, 0);
			containerPanel.AutoScroll = true;
		}

		private void DevicePanelOnSelectAll(Object sender, EventArgs e)
		{
			var titleControl = (sender as Control);
			if(titleControl == null) return;

			//containerPanel.AutoScroll = false;
			var position = containerPanel.AutoScrollPosition;
			position.Y *= -1;
			containerPanel.Enabled = false;
			var controls = new List<DevicePanel>();

			foreach (DevicePanel control in titleControl.Parent.Controls)
			{
				controls.Add(control);
			}
			controls.Reverse();

			foreach (DevicePanel control in controls)
			{
				control.Checked = true;
				if (!control.Checked)
					break;
			}
			containerPanel.AutoScrollPosition = position;
			containerPanel.Enabled = true;
			//containerPanel.AutoScroll = true;
		}

		private void DevicePanelOnSelectNone(Object sender, EventArgs e)
		{
			var titleControl = (sender as Control);
			if (titleControl == null) return;

			//containerPanel.AutoScroll = false;
			var position = containerPanel.AutoScrollPosition;
			position.Y *= -1;
			containerPanel.Enabled = false;
			foreach (DevicePanel control in titleControl.Parent.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScrollPosition = position;
			containerPanel.Enabled = true;
			//containerPanel.AutoScroll = true;
		}

		private Boolean _isEditing;
		private void DevicePanelOnSelectChange(Object sender, EventArgs e)
		{
			if (!_isEditing) return;
			if(!(sender is DevicePanel)) return;

			var panel = sender as DevicePanel;
			if (panel.Device == null) return;
			var resultPanel = panel.Parent as DeviceResultPanel;
			if (resultPanel == null) return;

			UInt16 count = 0;
			foreach (DevicePanel control in resultPanel.Controls)
			{
				if (control.IsTitle || control.Device == null) continue;

				if (control.Checked)
					count++;
			}

			if(count == 0)
				resultPanel.ResultLabel.Text = resultPanel.ResultLabel.Tag.ToString();
			else
				resultPanel.ResultLabel.Text = resultPanel.ResultLabel.Tag + @", " + ((count == 1)
														? Localization["SetupDevice_DeviceSelected"]
														: Localization["SetupDevice_DevicesSelected"]).Replace("%1", count.ToString());

			//Add Device
			var selectAll = false;
			if (panel.Checked)
			{
				panel.Device.Id = Server.Device.GetNewDeviceId();

				if (panel.Device.Id == 0)
				{
					panel.Checked = false;
					TopMostMessageBox.Show(Localization["SetupDevice_MaximumLicense"].Replace("%1", Server.License.Amount.ToString()), Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					AddDevice(panel.Device as ICamera);
					selectAll = true;
					foreach (DevicePanel control in resultPanel.Controls)
					{
						if (control.IsTitle) continue;
						if (!control.Checked && control.Enabled)
						{
							selectAll = false;
							break;
						}
					}
				}
			}
			else
			{
				RemoveDevice(panel.Device as ICamera);
			}

			var title = resultPanel.Controls[resultPanel.Controls.Count - 1] as DevicePanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= DevicePanelOnSelectAll;
				title.OnSelectNone -= DevicePanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += DevicePanelOnSelectAll;
				title.OnSelectNone += DevicePanelOnSelectNone;
			}
		}

		private void AddDevice(ICamera camera)
		{
			if (camera == null) return;

            camera.ReadyState = ReadyState.JustAdd;

			if (camera.Model.Model == Localization["Data_NotSupport"])
			{
				if (Server.Device.Manufacture.ContainsKey(camera.Model.Manufacture.Replace("*", "")))
				{
					var list = Server.Device.Manufacture[camera.Model.Manufacture.Replace("*", "")];
					camera.Model = list[0];
                    camera.Mode = camera.Model.CameraMode[0];
					Server.Device.SetDeviceCapabilityByModel(camera);
				}
				else
				{
					return;
				}
			}
            else
			{
                camera.Mode = camera.Model.CameraMode[0];

			    switch (camera.Model.Manufacture)
			    {
                    case "Axis":
                        if (camera.Mode == CameraMode.Quad)
                        {
                            if (!camera.Profile.StreamConfigs.ContainsKey(2))
                                camera.Profile.StreamConfigs.Add(2, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));

                            if (!camera.Profile.StreamConfigs.ContainsKey(3))
                                camera.Profile.StreamConfigs.Add(3, StreamConfigs.Clone(camera.Profile.StreamConfigs[1]));
                        }
			            break;
			    }
			}

			if (camera.RecordSchedule == null)
			{
				camera.RecordSchedule = new Schedule();
				camera.RecordSchedule.SetDefaultSchedule(ScheduleType.Continuous);
				camera.RecordSchedule.Description = ScheduleMode.FullTimeRecording;
			}

			if (camera.EventSchedule == null)
			{
				camera.EventSchedule = new Schedule();
				camera.EventSchedule.SetDefaultSchedule(ScheduleType.EventHandlering);
				camera.EventSchedule.Description = ScheduleMode.FullTimeEventHandling;
			}

			if (camera.EventHandling == null)
			{
				camera.EventHandling = new EventHandling();
				camera.EventHandling.SetDefaultEventHandling(camera.Model);
			}

			Server.Device.Devices.Add(camera.Id, camera);
			Server.DeviceModify(camera);

			var allDeviceGroup = Server.Device.Groups.Values.First();
			if (allDeviceGroup == null) return;

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

			if (OnDeviceModify != null)
				OnDeviceModify(this, null);
		}

		private void RemoveDevice(ICamera camera)
		{
			if (camera == null) return;

			camera.ReadyState = ReadyState.NotInUse;

			UInt16 key = 0;

			foreach (KeyValuePair<UInt16, IDevice> obj in Server.Device.Devices)
			{
				//if (!(obj.Value is ICamera)) continue;

				if (obj.Value != camera) continue;

				key = obj.Key;
				Server.Device.Devices.Remove(obj.Key);

				if (Server.Device.Groups.Values.First() == null) return;

				Server.Device.Groups.Values.First().Items.Remove(obj.Value);
				Server.Device.Groups.Values.First().View.Remove(obj.Value);
				Server.GroupModify(Server.Device.Groups.Values.First());

				break;
			}

			Boolean changeOrder = true;

			while (changeOrder && key > 0)
			{
				changeOrder = false;
				IDevice changeDevice = null;
				UInt16 changeKey = key;
				foreach (KeyValuePair<UInt16, IDevice> obj in Server.Device.Devices)
				{
					if (obj.Value.ReadyState != ReadyState.New) continue;
					if (obj.Key > changeKey)
					{
						changeDevice = obj.Value;
						break;
					}
				}

				if (changeDevice == null) continue;

				Server.Device.Devices.Add(changeKey, changeDevice);

				Server.Device.Devices.Remove(changeDevice.Id);

				key = changeDevice.Id;
				changeDevice.Id = changeKey;
				changeOrder = true;
			}

			if (OnDeviceModify != null)
				OnDeviceModify(this, null);
		}

		public void ClearViewModel()
		{
			_isEditing = false;
			if (containerPanel.Controls.Count == 0) return;

			foreach (Control control in containerPanel.Controls)
			{
				if (control is Label)
				{
					var label = control as Label;
					if (!_recycleLabels.Contains(label))
						_recycleLabels.Enqueue(label);
					continue;
				}

				if (control is DeviceResultPanel)
				{
					var panel = control as DeviceResultPanel;

					foreach (DevicePanel devicePanel in panel.Controls)
					{
						devicePanel.OnSelectChange -= DevicePanelOnSelectChange;
						devicePanel.SelectionVisible = true;
						devicePanel.Checked = false;
						devicePanel.Device = null;
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
					panel.Controls.Clear();

					if (!_recyclePanels.Contains(panel))
						_recyclePanels.Enqueue(panel);
				}
			}

			containerPanel.Controls.Clear();
		}
	}

	public sealed class DeviceResultPanel : Panel
	{
		public Label ResultLabel;
		public DeviceResultPanel()
		{
			DoubleBuffered = true;
		}
	}
}
