using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using ServerProfile;

namespace DeviceTree
{
	public sealed partial class GroupNameInputForm : Form
	{
		public Dictionary<String, String> Localization;

		public IDeviceGroup DeviceGroup { get; set; }

		private IServer _server;
		private INVR _nvr;
	    public  ICMS CMS;

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
								   {"MessageBox_Information", "Information"},
								   {"Common_Cancel", "Cancel"},
								   {"SetupTitle_Save", "Save"},
								   {"UserDefineGroupForm_UserDefineGroup", "Save View"},
								   {"UserDefineGroupForm_SetToShared", "Set this View as a shared one, so everyone can find it under Shared."},
								   {"DevicePanel_Name", "Name" },
								   {"SetupDeviceGroup_NewView", "New View"},
								   
								   {"SetupDeviceGroup_MaximumAmount", "Reached maximum amount limit \"%1\""},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			Text = Localization["UserDefineGroupForm_UserDefineGroup"];
			unlockLabel.Text = Localization["DevicePanel_Name"];
			publishCheckBox.Text = Localization["UserDefineGroupForm_SetToShared"];
			saveButton.Text = Localization["SetupTitle_Save"];
			cancelButton.Text = Localization["Common_Cancel"];
			TopMost = true;

			saveButton.BackgroundImage = Resources.GetResources(Properties.Resources.saveButton, Properties.Resources.IMGSaveButton);
			cancelButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn, Properties.Resources.IMGCancelButotn);
			BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.BGControllerBG);
		}

		private String _previousName;
		private void GroupNameInputFormShown(Object sender, EventArgs e)
		{
			publishCheckBox.Enabled = false;

            if(CMS!=null)
            {
                if (CMS.User.Current.Group.IsFullAccessToDevices)
                    publishCheckBox.Enabled = true;
            }
            else
            {
                if (_nvr.User.Current.Group.IsFullAccessToDevices)
                    publishCheckBox.Enabled = true;
            }

			publishCheckBox.Checked = false;
			groupnameTextBox.Text = _previousName = DeviceGroup.Name;
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
				var maximum = (IsPublish) 
					? DeviceManager.MaximumDeviceGroupsAmount
					: User.MaximumDeviceGroupsAmount;

				TopMostMessageBox.Show(Localization["SetupDeviceGroup_MaximumAmount"].Replace("%1", maximum.ToString()),
					Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			DeviceGroup.Name = groupnameTextBox.Text;
			DialogResult = DialogResult.OK;
		}

		private void CancelButtonClick(Object sender, EventArgs e)
		{
			groupnameTextBox.Text = _previousName = "";

			DialogResult = DialogResult.Cancel;
		}

		public Boolean IsPublish
		{
			get { return publishCheckBox.Checked; }
		}

		private void PublishCheckBoxMouseClick(Object sender, MouseEventArgs e)
		{
            if(CMS != null)
            {
                DeviceGroup.Id = (IsPublish)
               ? CMS.Device.GetNewGroupId()
               : CMS.User.Current.GetNewGroupId();
            }
            else
            {
                DeviceGroup.Id = (IsPublish)
               ? _nvr.Device.GetNewGroupId()
               : _nvr.User.Current.GetNewGroupId();
            }
		   
			
			//if user already changed group's name, dont auto change it.
			if (DeviceGroup.Name.Contains(Localization["SetupDeviceGroup_NewView"]))
			{
				//user didn't change the default name given by AP.
				if (_previousName == groupnameTextBox.Text)
				{
					groupnameTextBox.Text = DeviceGroup.Name = _previousName = Localization["SetupDeviceGroup_NewView"] + @" " + DeviceGroup.Id;
				}
			}
		}
	}
}
