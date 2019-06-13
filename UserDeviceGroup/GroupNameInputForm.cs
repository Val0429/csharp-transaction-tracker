using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace UserDeviceGroup
{
	public sealed partial class GroupNameInputForm : Form
	{
		public Dictionary<String, String> Localization;

		public IDeviceGroup DeviceGroup { get; set; }

		private IServer _server;
		private INVR _nvr;

		public IServer Server
		{
			get { return _server; }
			set
			{
				_server = value;

				if (_server is INVR)
					_nvr = _server as INVR;
			}
		}

		public GroupNameInputForm()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Common_Cancel", "Cancel"},
								   {"SetupTitle_Save", "Save"},
								   {"UserDefineGroupForm_UserDefineGroup", "User Define Group"},
								   {"UserDefineGroupForm_PublishThisGroup", "Publish This Group"},
								   {"UserDefineGroupForm_PleaseInputGroupName", "Please enter the Device Group name" },
								   {"SetupDeviceGroup_NewGroup", "New Group"},
								   
								   {"SetupDeviceGroup_MaximumAmount", "Reached maximum amount limit \"20\""},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			Text = Localization["UserDefineGroupForm_UserDefineGroup"];
			unlockLabel.Text = Localization["UserDefineGroupForm_PleaseInputGroupName"];
			publishCheckBox.Text = Localization["UserDefineGroupForm_PublishThisGroup"];
			saveButton.Text = Localization["SetupTitle_Save"];
			cancelButton.Text = Localization["Common_Cancel"];

			BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.BGControllerBG);
		}

		private void GroupNameInputFormShown(Object sender, EventArgs e)
		{
			publishCheckBox.Enabled = false;
			if ((_nvr.User.Current.Group.Name == "Administrator") || (_nvr.User.Current.Group.Name == "Superuser"))
				publishCheckBox.Enabled = true;

			publishCheckBox.Checked = DeviceGroup.Publish;
			groupnameTextBox.Text = DeviceGroup.Name;
			groupnameTextBox.Focus();

			Shown -= GroupNameInputFormShown;
		}

		private void GroupnameTextBoxKeyPress(Object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == Convert.ToChar(Keys.Enter))
			{
				saveButton.PerformClick();
			}
		}

		private void SaveButtonClick(Object sender, EventArgs e)
		{
			if (DeviceGroup.Id == 0)
			{
				TopMostMessageBox.Show(Localization["SetupDeviceGroup_MaximumAmount"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			DeviceGroup.Name = groupnameTextBox.Text;
			DialogResult = DialogResult.OK;
		}

		private void CancelButtonClick(Object sender, EventArgs e)
		{
			groupnameTextBox.Text = "";

			DialogResult = DialogResult.Cancel;
		}

		private void PublishCheckBoxMouseClick(Object sender, MouseEventArgs e)
		{
			DeviceGroup.Publish = publishCheckBox.Checked;

			DeviceGroup.Id = (DeviceGroup.Publish)
				? _nvr.Device.GetNewGroupId()
				: _nvr.User.Current.GetNewGroupId();
			
			//if user already changed group's name, dont auto change it.
			if (DeviceGroup.Name.Contains(Localization["SetupDeviceGroup_NewGroup"]))
				groupnameTextBox.Text = DeviceGroup.Name = Localization["SetupDeviceGroup_NewGroup"] + @" " + DeviceGroup.Id;
		}
	}
}
