using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceCab;
using DeviceConstant;
using PanelBase;

namespace SetupDevice
{
	public partial class BrandControl : UserControl
	{
		public event EventHandler OnBrandChange;
		public event EventHandler OnModelChange;

		public Dictionary<String, String> Localization;
		public EditPanel EditPanel;

		public BrandControl()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"DevicePanel_Brand", "Brand"},
								   {"DevicePanel_Name", "Name"},
								   {"DevicePanel_Manufacturer", "Manufacturer"},
								   {"DevicePanel_Model", "Model"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Top;

			Paint += BrandControlPaint;

			manufacturePanel.Paint += PaintInput;
			modelPanel.Paint += PaintInput;
			namePanel.Paint += PaintInput;

			brandComboBox.SelectedIndexChanged += BrandComboBoxSelectedIndexChanged;
			modelComboBox.SelectedIndexChanged += ModelComboBoxSelectedIndexChanged;
			nameTextBox.TextChanged += NameTextBoxTextChanged;
		}

		protected virtual void BrandControlPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

            g.DrawString(Localization["DevicePanel_Brand"], SetupBase.Manager.Font, Brushes.DimGray, 8, 10);
		}

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			var control = (Control)sender;

			if (control == null || control.Parent == null) return;

			var g = e.Graphics;

            SetupBase.Manager.Paint(g, control);

			if (Localization.ContainsKey("DevicePanel_" + control.Tag))
                SetupBase.Manager.PaintText(g, Localization["DevicePanel_" + control.Tag]);
			else
                SetupBase.Manager.PaintText(g, control.Tag.ToString());
		}

		public void InitializeBrand()
		{
			foreach (var cameraManufactureFile in EditPanel.Server.Device.Manufacture)
			{
				brandComboBox.Items.Add(EditPanel.Server.Server.DisplayManufactures(cameraManufactureFile.Key));
			}

			brandComboBox.Enabled = (brandComboBox.Items.Count > 1);
		}

		private CameraModel SelectedModel
		{
			get
			{
				var brand = SelectedBrand;

				if (!EditPanel.Server.Device.Manufacture.ContainsKey(brand)) return null;

				var list = EditPanel.Server.Device.Manufacture[brand];
				foreach (var mode in list)
				{
					if (mode.Model == modelComboBox.SelectedItem.ToString()) return mode;
				}
				return null;
			}
		}

		public String SelectedBrand
		{
			get
			{
				var brand = brandComboBox.SelectedItem.ToString();
				return EditPanel.Server.Server.FormalManufactures(brand);
			}
		}

		private void BrandComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			String brand = SelectedBrand;
			modelComboBox.Items.Clear();

			if (!EditPanel.Server.Device.Manufacture.ContainsKey(brand))  return;

			var list = EditPanel.Server.Device.Manufacture[brand];
			CameraModel model = null;
			foreach (var cameraModel in list)
			{
				if (model == null)
					model = cameraModel;

				modelComboBox.Items.Add(cameraModel.Model);
			}

			modelComboBox.Enabled = (modelComboBox.Items.Count > 1);

			if (!EditPanel.IsEditing) return;

			if (model != null)
				EditPanel.Camera.Model = model;

			ProfileChecker.SetDefaultPort(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultMulticastNetworkAddress(EditPanel.Camera, EditPanel.Camera.Model);

			if (OnBrandChange != null)
				OnBrandChange(this, null);

			if (modelComboBox.Items.Count <= 0) return;
			
			modelComboBox.SelectedIndex = 0;
		}

		private void ModelComboBoxSelectedIndexChanged(Object sender, EventArgs e)
		{
			ChangeModel(SelectedModel);

			if (OnModelChange != null)
				OnModelChange(this, null);
		}

		private void ChangeModel(CameraModel model)
		{
			if (EditPanel.Camera == null) return;

			if (model != null)
			{
				EditPanel.UpdateSettingContentAndSetDefault(model);
				nameTextBox.Text = model.Model;
			}

			if (!EditPanel.IsEditing) return;

			EditPanel.Camera.Model = model;

			ProfileChecker.SetDefaultMode(EditPanel.Camera, EditPanel.Camera.Model);
			EditPanel.Camera.Profile.StreamConfigs.Clear();

			EditPanel.Camera.Profile.StreamConfigs.Add(1, new StreamConfig());
			switch (EditPanel.Camera.Mode)
			{
				case CameraMode.Single:
					break;

				case CameraMode.Dual:
					EditPanel.Camera.Profile.StreamConfigs.Add(2, new StreamConfig());
					break;

				case CameraMode.Triple:
					EditPanel.Camera.Profile.StreamConfigs.Add(2, new StreamConfig());
					EditPanel.Camera.Profile.StreamConfigs.Add(3, new StreamConfig());
					break;

				case CameraMode.Multi:
				case CameraMode.FourVga:
					EditPanel.Camera.Profile.StreamConfigs.Add(2, new StreamConfig());
					EditPanel.Camera.Profile.StreamConfigs.Add(3, new StreamConfig());
					EditPanel.Camera.Profile.StreamConfigs.Add(4, new StreamConfig());
					break;

                case CameraMode.Five:
                    EditPanel.Camera.Profile.StreamConfigs.Add(2, new StreamConfig());
                    EditPanel.Camera.Profile.StreamConfigs.Add(3, new StreamConfig());
                    EditPanel.Camera.Profile.StreamConfigs.Add(4, new StreamConfig());
                    EditPanel.Camera.Profile.StreamConfigs.Add(5, new StreamConfig());
                    break;

				case CameraMode.SixVga:
					EditPanel.Camera.Profile.StreamConfigs.Add(2, new StreamConfig());
					EditPanel.Camera.Profile.StreamConfigs.Add(3, new StreamConfig());
					EditPanel.Camera.Profile.StreamConfigs.Add(4, new StreamConfig());
					EditPanel.Camera.Profile.StreamConfigs.Add(5, new StreamConfig());
					EditPanel.Camera.Profile.StreamConfigs.Add(6, new StreamConfig());
					break;
			}

			ProfileChecker.SetDefaultAccountPassword(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultProtocol(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultPort(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultMulticastNetworkAddress(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultAudioOutPort(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultTvStandard(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultSensorMode(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultPowerFrequency(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultAspectRatio(EditPanel.Camera, EditPanel.Camera.Model);
			ProfileChecker.SetDefaultIOPort(EditPanel.Camera, EditPanel.Camera.Model);

			foreach (var config in EditPanel.Camera.Profile.StreamConfigs)
			{
			    if (EditPanel.Camera.Model != null)
			    {
			        switch (EditPanel.Camera.Model.Manufacture)
			        {
			            case "Axis":
			                if (EditPanel.Camera.Model.Type == "fisheye")
			                {
			                    ProfileChecker.CheckAvailableSetDefaultDewarpMode(EditPanel.Camera, EditPanel.Camera.Model);
			                }
			                break;
			        }
			        ProfileChecker.SetDefaultCompression(EditPanel.Camera, EditPanel.Camera.Model, config.Value, config.Key);
			        ProfileChecker.CheckAvailableSetDefaultResolution(EditPanel.Camera, EditPanel.Camera.Model, config.Value, config.Key);
			        ProfileChecker.CheckAvailableSetDefaultFramerate(EditPanel.Camera, EditPanel.Camera.Model, config.Value, config.Key);
			        ProfileChecker.CheckAvailableSetDefaultBitrate(EditPanel.Camera, EditPanel.Camera.Model, config.Value, config.Key);
			        ProfileChecker.SetDefaultBitrateControl(EditPanel.Camera.Model, config.Value);
			    }
			}

			//apply value to UI
			EditPanel.UpdateSettingContent(EditPanel.Camera.Model);

			EditPanel.CameraIsModify();
		}

		public void ParseDevice()
		{
			modelComboBox.SelectedIndexChanged -= ModelComboBoxSelectedIndexChanged;

			if (EditPanel.Camera.Model != null)
			{
				EditPanel.UpdateSettingContent(EditPanel.Camera.Model);
				brandComboBox.SelectedItem = EditPanel.Server.Server.DisplayManufactures(EditPanel.Camera.Model.Manufacture);
				modelComboBox.SelectedItem = EditPanel.Camera.Model.Model;
				nameTextBox.Text = EditPanel.Camera.Name;
			}
			else
			{
				brandComboBox.SelectedIndex = 0;
				modelComboBox.SelectedIndex = 0;
				nameTextBox.Text = modelComboBox.SelectedItem.ToString();
				CameraModel model = SelectedModel;
				if(model != null)
					EditPanel.UpdateSettingContentAndSetDefault(SelectedModel);
			}

			if (OnBrandChange != null)
				OnBrandChange(this, null);

			modelComboBox.SelectedIndexChanged += ModelComboBoxSelectedIndexChanged;
		}

		private void NameTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!EditPanel.IsEditing) return;

			EditPanel.Camera.Name = nameTextBox.Text;

			EditPanel.CameraIsModify();
		}

		private void OpenWebButtonClick(object sender, EventArgs e)
		{
			Process.Start("IExplore.exe", "http://" + EditPanel.Camera.Profile.NetworkAddress + ":" + EditPanel.Camera.Profile.HttpPort);
		}
	}
}
