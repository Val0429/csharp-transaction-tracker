using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using SetupDevice;
using SetupDeviceGroup;
using Manager = SetupBase.Manager;

namespace SetupPOS
{
	public sealed partial class EditPanel : UserControl
	{
		public IPOS POS;
		public IPTS PTS;
		public Dictionary<String, String> Localization;

		public Boolean IsEditing;

		private readonly Queue<DevicePanel> _recycleDevice = new Queue<DevicePanel>();
		private readonly List<DeviceListPanel> _deviceListPanels = new List<DeviceListPanel>();

		public EditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   
								   {"POS_Name", "Name"},
								   {"POS_Manufacture", "Manufacture"},
								   {"POS_Model", "Model"},
								   {"POS_RegisterId", "Register Id"},
								   {"POS_Exception", "Exception"},
								   {"POS_Keyword", "Keyword"},

								   {"EditPOSPanel_Information", "Information"},
								   {"EditPOSPanel_RegisterIdCantEmpty", "Register Id can't be empty."},
								   {"EditPOSPanel_RegisterIdOutOfRange", "Register Id must be 1 - 65535."},
								   {"EditPOSPanel_RegisterIdCantTheSame", "Register Id can't be the same. %1 is used by %2."},
							   };
			Localizations.Update(Localization);
			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			//nameTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
			registerIDTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;
			BackgroundImage = Manager.BackgroundNoBorder;

			_deviceListPanels.Add(new DeviceListPanel { Padding = new Padding(0, 15, 0, 0) });
		}

		public void Initialize()
		{
			posPanel.Paint += ContainerPanelPaint;

			manufacturePanel.Paint += PaintInput;
			modelPanel.Paint += PaintInput;
			namePanel.Paint += PaintInput;
			registerIDPanel.Paint += PaintInput;
			exceptionPanel.Paint += PaintInput;
			keywordPanel.Paint += PaintInput;

			foreach (String manufacture in POS_Exception.Manufactures)
			{
				manufactureComboBox.Items.Add(POS_Exception.ToDisplay(manufacture));
			}
			manufactureComboBox.SelectedIndex = 0;
			manufactureComboBox.Enabled = (POS_Exception.Manufactures.Length > 1);

			nameTextBox.TextChanged += NameTextBoxTextChanged;
			registerIDTextBox.TextChanged += RegisterIDTextBoxTextChanged;
			registerIDTextBox.GotFocus += RegisterIDTextBoxGotFocus;

			manufactureComboBox.SelectedIndexChanged += ManufactureComboBoxSelectedIndexChanged;
			//modelComboBox.SelectedIndexChanged += ModelComboBoxSelectedIndexChanged;
			exceptionComboBox.SelectedIndexChanged += ExceptionComboBoxSelectedIndexChanged;
			keywordComboBox.SelectedIndexChanged += KeywordComboBoxSelectedIndexChanged;
		}
		
		private void DevicePanelOnSelectAll(Object sender, EventArgs e)
		{
			var devicePanel = sender as DevicePanel;
			if (devicePanel == null || devicePanel.Parent == null) return;

			//Panel panel = null;
			//try
			//{
			//    panel = ((Panel)Parent);
			//}
			//catch (Exception)
			//{
			//}

			var scrollTop = 0;
			scrollTop = containerPanel.AutoScrollPosition.Y * -1;
			containerPanel.AutoScroll = false;

			foreach (DevicePanel control in devicePanel.Parent.Controls)
			{
				control.Checked = true;
			}

			containerPanel.AutoScroll = true;
			containerPanel.AutoScrollPosition = new Point(0, scrollTop);
		}

		private void DevicePanelOnSelectNone(Object sender, EventArgs e)
		{
			var devicePanel = sender as DevicePanel;
			if (devicePanel == null || devicePanel.Parent == null) return;

			var scrollTop = 0;

			scrollTop = containerPanel.AutoScrollPosition.Y * -1;
			containerPanel.AutoScroll = false;

			foreach (DevicePanel control in devicePanel.Parent.Controls)
			{
				control.Checked = false;
			}

			containerPanel.AutoScroll = true;
			containerPanel.AutoScrollPosition = new Point(0, scrollTop);
		}

		private void DeviceControlOnSelectChange(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

			var panel = sender as DevicePanel;
			if (panel == null) return;

			var selectAll = false;
			if (panel.Checked)
			{
				selectAll = true;
				foreach (DevicePanel control in panel.Parent.Controls)
				{
					if (control.IsTitle) continue;
					if (!control.Checked)
					{
						selectAll = false;
						break;
					}
				}
			}

			var title = panel.Parent.Controls[panel.Parent.Controls.Count - 1] as DevicePanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= DevicePanelOnSelectAll;
				title.OnSelectNone -= DevicePanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += DevicePanelOnSelectAll;
				title.OnSelectNone += DevicePanelOnSelectNone;
			}
			
			if (POS.ReadyState == ReadyState.Ready)
				POS.ReadyState = ReadyState.Modify;
			PTS.POSModify(POS);

			if (panel.Checked)
			{
				if (!POS.Items.Contains(panel.Device))
				{
					POS.Items.Add(panel.Device);
					POS.Items.Sort(SortByIdThenNVR);
				}

				POS.View.Clear();
				POS.View.AddRange(POS.Items);
			}
			else
			{
				POS.Items.Remove(panel.Device);
				POS.View.Remove(panel.Device);
			}
		}

		private static Int32 SortByIdThenNVR(IDevice x, IDevice y)
		{
			if (x.Id != y.Id)
				return (x.Id - y.Id);

			return (x.Server.Id - y.Server.Id);
		}

		private void ContainerPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.DrawString(Localization["EditPOSPanel_Information"], Manager.Font, Brushes.DimGray, 8, 0);
		}

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, (Control)sender);

			if (Localization.ContainsKey("POS_" + ((Control)sender).Tag))
				Manager.PaintText(g, Localization["POS_" + ((Control)sender).Tag]);
			else
				Manager.PaintText(g, ((Control)sender).Tag.ToString());
		}

		public void ParsePOS()
		{
			if (POS == null) return;

			IsEditing = false;

			registerIDTextBox.LostFocus -= RegisterIDTextBoxLostFocus;
			nameTextBox.Text = POS.Name;
			//registerIDTextBox.Text = POS.RegisterId.ToString();
			registerIDTextBox.Text = POS.Id.ToString();

			UpdateExceptionList();

			manufactureComboBox.SelectedItem = POS_Exception.ToDisplay(POS.Manufacture);
			modelComboBox.SelectedItem = POS.Model;

			keywordComboBox.SelectedItem = POS.Keyword;

			ClearDeviceListControls();
			CreateDeviceList();

			posPanel.Focus();

			IsEditing = true;
		}

		private void RegisterIDTextBoxGotFocus(Object sender, EventArgs e)
		{
			registerIDTextBox.LostFocus -= RegisterIDTextBoxLostFocus;
			registerIDTextBox.LostFocus += RegisterIDTextBoxLostFocus;
		}

		private void RegisterIDTextBoxLostFocus(Object sender, EventArgs e)
		{
			registerIDTextBox.LostFocus -= RegisterIDTextBoxLostFocus;
			if (String.IsNullOrEmpty(registerIDTextBox.Text))
			{
				TopMostMessageBox.Show(Localization["EditPOSPanel_RegisterIdCantEmpty"], Localization["MessageBox_Information"],
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				registerIDTextBox.Focus();
				return;
			}
			
			//check if ID dulipcate
			var pass = true;
			String id = registerIDTextBox.Text;

            //if (id > 65535)//id < 1 || 
            //{
            //    TopMostMessageBox.Show(Localization["EditPOSPanel_RegisterIdOutOfRange"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    pass = false;
            //}
			foreach (var pos in PTS.POS.POSServer)
			{
				if(pos == POS) continue;
				if (pos.Id == POS.Id)
				{
					pass = false;

					//alert
					TopMostMessageBox.Show(Localization["EditPOSPanel_RegisterIdCantTheSame"].Replace("%1", POS.Id.ToString()).Replace("%2", pos.ToString()),
						Localization["MessageBox_Information"],
						MessageBoxButtons.OK, MessageBoxIcon.Information);

					break;
				}
			}
			if (!pass)
			{
				registerIDTextBox.Focus();
			}
		}

		public void CreateDeviceList()
		{
			if (PTS.NVR.NVRs.Count == 0) return;
			
			var nvrs = new List<INVR>(PTS.NVR.NVRs.Values);
			nvrs.Sort((x, y) => (y.Id - x.Id));

			foreach (INVR nvr in nvrs)
			{
				if (nvr.Device.Devices.Count == 0) continue;

				AppendDeviceListPanel(nvr, new DeviceListPanel { Padding = new Padding(0, 22, 0, 0), NVR = nvr });
			}
		}

		private void ClearDeviceListControls()
		{
			foreach (DeviceListPanel listPanel in _deviceListPanels)
			{
				foreach (DevicePanel control in listPanel.Controls)
				{
					control.Checked = false;
					control.Device = null;
					control.EditVisible = true;

					if (control.IsTitle)
					{
						control.OnSelectAll -= DevicePanelOnSelectAll;
						control.OnSelectNone -= DevicePanelOnSelectNone;
						control.IsTitle = false;
					}

					if (control.Height == 0)
						continue;

					if (!_recycleDevice.Contains(control))
						_recycleDevice.Enqueue(control);
				}
				listPanel.Controls.Clear();
			}
			_deviceListPanels.Clear();
			containerPanel.Controls.Clear();
		}

		private void AppendDeviceListPanel(INVR nvr, DeviceListPanel listPanel)
		{
			var selectAll = true;
			var devices = new List<IDevice>(nvr.Device.Devices.Values);
			var count = 0;
			//reverse
			devices.Sort((x, y) => (y.Id - x.Id));
			if (devices.Count > 0)
			{
				foreach (IDevice device in devices)
				{
					if (!(device is ICamera)) continue;

					var devicePanel = GetDevicePanel();
					devicePanel.SelectionVisible = true;
					devicePanel.Device = device;

					devicePanel.EditVisible = false;

					if (POS.Items.Contains(device))
						{
							count++;
							devicePanel.Checked = true;
						}
					else
						selectAll = false;

					listPanel.Controls.Add(devicePanel);
				}
				if (count == 0 && selectAll)
					selectAll = false;

				var deviceTitlePanel = GetDevicePanel();
				deviceTitlePanel.IsTitle = true;
				deviceTitlePanel.Cursor = Cursors.Default;
				deviceTitlePanel.EditVisible = false;
				deviceTitlePanel.SelectionVisible = true;
				deviceTitlePanel.Checked = selectAll;
				deviceTitlePanel.OnSelectAll += DevicePanelOnSelectAll;
				deviceTitlePanel.OnSelectNone += DevicePanelOnSelectNone;
				listPanel.Controls.Add(deviceTitlePanel);
			}

			containerPanel.Controls.Add(listPanel);
			_deviceListPanels.Add(listPanel);
		}

		private DevicePanel GetDevicePanel()
		{
			if (_recycleDevice.Count > 0)
			{
				return _recycleDevice.Dequeue();
			}

			var devicePanel = new DevicePanel
			{
				EditVisible = false,
				SelectionVisible = true,
				Cursor = Cursors.Default,
				Server = PTS,
			};
			devicePanel.OnSelectChange += DeviceControlOnSelectChange;

			return devicePanel;
		}

		private void NameTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

			POS.Name = nameTextBox.Text;
			POSIsModify();
		}

		private void RegisterIDTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

			if (String.IsNullOrEmpty(registerIDTextBox.Text.Trim()))
			{
				//zero will popup message
				POS.Id = "0";
			}
			else
			{
                //Int32 id = Convert.ToInt32(registerIDTextBox.Text);
                //POS.Id = Convert.ToUInt16(Math.Min(Math.Max(id, 0), 65535));
			    POS.Id = registerIDTextBox.Text;
			}
			POSIsModify();
		}

		private void ManufactureComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

			POS.Manufacture = POS_Exception.ToIndex(manufactureComboBox.SelectedItem.ToString());

			UpdateExceptionList();

			foreach (var posConnection in PTS.POS.Connections)
			{
				if(!posConnection.Value.POS.Contains(POS)) continue;
				if (posConnection.Value.Manufacture == POS.Manufacture) continue;

				posConnection.Value.POS.Remove(POS);

				if (posConnection.Value.ReadyState == ReadyState.Ready)
					posConnection.Value.ReadyState = ReadyState.Modify;
			}

			POSIsModify();
		}

		private void UpdateExceptionList()
		{
			exceptionComboBox.Items.Clear();
			var keys = PTS.POS.Exceptions.Keys.ToList();
			keys.Sort();

			var containes = false;
			UInt16 firstMatch = 0;
			foreach (var key in keys)
			{
				if (PTS.POS.Exceptions[key].Manufacture != POS.Manufacture) continue;
				if(PTS.POS.Exceptions[key].IsCapture) continue;
				if (firstMatch == 0)
					firstMatch = key;

				exceptionComboBox.Items.Add(PTS.POS.Exceptions[key].ToString());
				if (POS.Exception == key)
					containes = true;
			}
			exceptionComboBox.Enabled = exceptionComboBox.Items.Count > 0;

			if (!containes)
			{
				POS.Exception = firstMatch;
				//clear exception report
				if (POS.ExceptionReports.ReadyState == ReadyState.Ready)
					POS.ExceptionReports.ReadyState = ReadyState.Modify;

				POS.ExceptionReports.Clear();

				POSIsModify();
			}
			
			if(POS.Exception != 0)
				exceptionComboBox.SelectedItem = PTS.POS.Exceptions[POS.Exception].ToString();
		}

		//private void ModelComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		//{
		//    if (!IsEditing) return;

		//    POS.Model = modelComboBox.SelectedItem.ToString();

		//    POSIsModify();
		//}

		private void ExceptionComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

			POS.Exception = Convert.ToUInt16(exceptionComboBox.SelectedItem.ToString().Split(' ')[0]);

			//clear exception report
			if (POS.ExceptionReports.ReadyState == ReadyState.Ready)
				POS.ExceptionReports.ReadyState = ReadyState.Modify;

			POS.ExceptionReports.Clear();

			POSIsModify();
		}

		private void KeywordComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

			POS.Keyword = Convert.ToUInt16(keywordComboBox.SelectedItem.ToString().Split(' ')[0]);

			POSIsModify();
		}


		public void POSIsModify()
		{
			if (POS.ReadyState == ReadyState.Ready)
				POS.ReadyState = ReadyState.Modify;

			PTS.POSModify(POS);
		}
	}
}
