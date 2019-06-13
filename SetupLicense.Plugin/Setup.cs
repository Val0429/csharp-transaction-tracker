using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupBase;

namespace SetupLicense.Plugin
{
	public partial class Setup
	{
		public override event EventHandler<EventArgs<String>> OnSelectionChange;

		public Setup()
		{
			Localization.Add("Control_Plug-inLicense", "Plug-in License");
			Localization.Add("SetupLicense_PluginLicenseQuantity", "Plug-in License Quantity");

			Localizations.Update(Localization);

			Name = "Plug-in License";
			TitleName = Localization["Control_Plug-inLicense"];

			InitializeComponent();
		}

		protected override void AmountInputPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, amountDoubleBufferPanel);
			Manager.PaintText(g, Localization["SetupLicense_PluginLicenseQuantity"]);
			//Manager.PaintTextRight(g, amountDoubleBufferPanel, Server.License.PluginLicnese[PluginPackage.Google].ToString());
		}

		protected override void LicenseInputPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, licenseKeyBufferPanel);
			Manager.PaintText(g, Localization["SetupLicense_LicenseKey"]);
		}

		protected delegate void ShowPluginLicenseAmountDelegate();
		protected override void ShowLicenseAmount()
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke(new ShowPluginLicenseAmountDelegate(ShowLicenseAmount));
				}
				catch (Exception)
				{
				}
				return;
			}

			amountDoubleBufferPanel.Visible = true;
			ethernetCardPanel.Visible = licenseKeyBufferPanel.Visible = false;

			infoContainerPanel.Controls.Clear();

			var temp = new List<Adaptor>(Server.License.Adaptor);
			temp.Reverse();
			foreach (var adaptor in temp)
			{
				var infoPanel = new InfoControl { Adaptor  = adaptor};
				infoPanel.ParseInfo();
				infoContainerPanel.Controls.Add(infoPanel);
			}

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName, "", "OnlineRegistration,OfflineRegistration" )));
		}

		protected override void OnlineRegistration()
		{
			infoContainerPanel.Controls.Clear();

			ethernetCardPanel.Visible = licenseKeyBufferPanel.Visible = true;
			amountDoubleBufferPanel.Visible = false;
			//upload .key(Xml) file to server

			key1TextBox.TextChanged -= Key1TextBoxTextChanged;
			key2TextBox.TextChanged -= Key2TextBoxTextChanged;
			key3TextBox.TextChanged -= Key3TextBoxTextChanged;
			key4TextBox.TextChanged -= Key4TextBoxTextChanged;
			
			ethernetComboBox.SelectedIndexChanged -= EthernetComboBoxSelectedIndexChanged;
			ethernetComboBox.Items.Clear();
			
			foreach (var adaptor in Server.License.Adaptor)
				ethernetComboBox.Items.Add(adaptor.Description);

			if (ethernetComboBox.Items.Count > 0)
			{
				ethernetComboBox.Enabled = true;
				ethernetComboBox.SelectedIndexChanged += EthernetComboBoxSelectedIndexChanged;
				ethernetComboBox.SelectedIndex = 0;
			}
			else
				ethernetComboBox.Enabled = false;

			Manager.DropDownWidth(ethernetComboBox);

			key1TextBox.Text = key2TextBox.Text = key3TextBox.Text = key4TextBox.Text = key5TextBox.Text = "";

			key1TextBox.TextChanged += Key1TextBoxTextChanged;
			key2TextBox.TextChanged += Key2TextBoxTextChanged;
			key3TextBox.TextChanged += Key3TextBoxTextChanged;
			key4TextBox.TextChanged += Key4TextBoxTextChanged;

			key1TextBox.Focus();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, Localization["SetupLicense_OnlineRegistration"],
					"Back", "Confirm")));
		}

		protected override void SubmitOnlineRegistration()
		{
			if (key1TextBox.Text.Length != 5 || key2TextBox.Text.Length != 5 || key3TextBox.Text.Length != 5
				|| key4TextBox.Text.Length != 5 || key5TextBox.Text.Length != 5)
			{
				TopMostMessageBox.Show(Localization["SetupLicense_CheckLicenseLength"], Localization["MessageBox_Error"],
				   MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			String[] key = new[]
			{
				key1TextBox.Text,
				key2TextBox.Text,
				key3TextBox.Text,
				key4TextBox.Text,
				key5TextBox.Text,
			};

			DialogResult result = MessageBox.Show(Localization["SetupLicense_KeyConfirm"].Replace("%1", String.Join("-", key)), Localization["MessageBox_Information"],
				MessageBoxButtons.YesNo, MessageBoxIcon.Information);

			if (result != DialogResult.Yes) return;

			//wait 0.2sec to close dialog, avoid capture dialog as background image
			Thread.Sleep(200);

			Server.License.OnPluginSaveComplete -= OnRegistrationComplete;
			Server.License.OnPluginSaveComplete += OnRegistrationComplete;
			Server.License.SavePlugin(String.Join("-", key));
		}

		protected override void SubmitOfflineRegistration(String key)
		{
			Server.License.OnPluginSaveComplete -= OnRegistrationComplete;
			Server.License.OnPluginSaveComplete += OnRegistrationComplete;

			Server.License.SavePlugin(key);
		}
	}
}
