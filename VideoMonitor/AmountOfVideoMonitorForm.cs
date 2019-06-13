using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;

namespace VideoMonitor
{
	public sealed partial class AmountOfVideoMonitorForm : Form
	{
		public Dictionary<String, String> Localization;

		public AmountOfVideoMonitorForm()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Confirm", "Information"},
								   {"Common_OK", "OK"},
								   {"AmountOfVideoMonitorForm_AmountOfMonitor", "Amount of monitor"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			Text = Localization["MessageBox_Confirm"];
			amountLabel.Text = Localization["AmountOfVideoMonitorForm_AmountOfMonitor"];
			okButton.Text = Localization["Common_OK"];
			TopMost = true;

			BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.BGControllerBG);
		}

		private void AmountOfVideoMonitorFormShown(Object sender, EventArgs e)
		{
			amountComboBox.Items.Clear();

			for (int i = 1; i <= MaximumAmount; i++ )
			{
				amountComboBox.Items.Add(i);
			}
			if (amountComboBox.Items.Count > 1)
			{
				amountComboBox.SelectedIndex = 1;
				amountComboBox.Enabled = true;
			}
			else
			{
				amountComboBox.SelectedIndex = 0;
				amountComboBox.Enabled = false;
			}

			Shown -= AmountOfVideoMonitorFormShown;
		}

		public UInt16 MaximumAmount = 1;
		public UInt16 SelectAmount;
		private void OKButtonClick(Object sender, EventArgs e)
		{
			SelectAmount = UInt16.Parse(amountComboBox.SelectedItem.ToString());

			DialogResult = DialogResult.OK;
		}
	}
}
