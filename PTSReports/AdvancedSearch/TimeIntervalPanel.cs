using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class TimeIntervalPanel : UserControl
	{
		public IPTS PTS;
        public POS_Exception.TimeIntervalCriteria TimeIntervalCriteria;

		public Dictionary<String, String> Localization;

        public TimeIntervalPanel()
		{
			Localization = new Dictionary<String, String>
							   {
                                   {"Common_Sec", "Sec"},
								   {"PTSReports_TimeIntervel", "Time Interval"}
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
            Name = "TimeIntervel";
            BackgroundImage = Manager.BackgroundNoBorder;

            secLabel.Text = Localization["Common_Sec"];
			enablePanel.Paint += EnablePanelPaint;
            enableCheckBox.Click += EnableCheckBoxCheckedChanged;
            secTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            secTextBox.TextChanged += SecTextBoxTextChanged;
            secTextBox.Enabled = enableCheckBox.Checked;
		}

        private void SecTextBoxTextChanged(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(secTextBox.Text))
            {
                TimeIntervalCriteria.Sec = 0;
                return;
            }

            TimeIntervalCriteria.Sec = Convert.ToUInt16(secTextBox.Text);
        }

        private void EnableCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            TimeIntervalCriteria.Enable = enableCheckBox.Checked;
            if (enableCheckBox.Checked && String.IsNullOrEmpty(secTextBox.Text))
            {
                TimeIntervalCriteria.Sec = 2;
                secTextBox.Text = TimeIntervalCriteria.Sec.ToString();
            }
            secTextBox.Enabled = enableCheckBox.Checked;
        }

		public void Initialize()
		{
			
		}

		public void ParseSetting()
		{
            secTextBox.TextChanged -= SecTextBoxTextChanged;
		    secTextBox.Text = TimeIntervalCriteria.Sec.ToString();
            secTextBox.TextChanged += SecTextBoxTextChanged;

            enableCheckBox.CheckedChanged -= EnableCheckBoxCheckedChanged;
		    enableCheckBox.Checked = TimeIntervalCriteria.Enable;
            enableCheckBox.CheckedChanged += EnableCheckBoxCheckedChanged;
		}

		public void ScrollTop()
		{

		}

		private void EnablePanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintTop(g, enablePanel);

			if (enablePanel.Width <= 150) return;

            Manager.PaintText(g, Localization["PTSReports_TimeIntervel"]);
		}

	}

}
