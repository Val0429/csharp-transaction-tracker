using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace PTSReports.AdvancedSearch
{
	public sealed partial class CountingPanel : UserControl
	{
		public IPTS PTS;
        public POS_Exception.CountingCriteria CountingCriteria;

		public Dictionary<String, String> Localization;

        public CountingPanel()
		{
			Localization = new Dictionary<String, String>
							   {
                                   {"Common_Pcs", "Piece"},
								   {"PTSReports_CountingDiscrepancies", "Counting Discrepancies"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.None;
            Name = "CountingDiscrepancies";
            BackgroundImage = Manager.BackgroundNoBorder;

            pieceLabel.Text = Localization["Common_Pcs"];
			enablePanel.Paint += EnablePanelPaint;
            enableCheckBox.Click += EnableCheckBoxCheckedChanged;
            pieceTextBox.KeyPress += KeyAccept.AcceptNumberOnly;
            pieceTextBox.TextChanged += SecTextBoxTextChanged;
            pieceTextBox.Enabled = enableCheckBox.Checked;
		}

        private void SecTextBoxTextChanged(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(pieceTextBox.Text))
            {
                CountingCriteria.Piece = 0;
                return;
            }

            CountingCriteria.Piece = Convert.ToUInt16(pieceTextBox.Text);
        }

        private void EnableCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            CountingCriteria.Enable = enableCheckBox.Checked;
            if (enableCheckBox.Checked && String.IsNullOrEmpty(pieceTextBox.Text))
            {
                CountingCriteria.Piece = 1;
                pieceTextBox.Text = CountingCriteria.Piece.ToString();
            }
            pieceTextBox.Enabled = enableCheckBox.Checked;
        }

		public void Initialize()
		{
			
		}

		public void ParseSetting()
		{
            pieceTextBox.TextChanged -= SecTextBoxTextChanged;
            pieceTextBox.Text = CountingCriteria.Piece.ToString();
            pieceTextBox.TextChanged += SecTextBoxTextChanged;

            enableCheckBox.CheckedChanged -= EnableCheckBoxCheckedChanged;
            enableCheckBox.Checked = CountingCriteria.Enable;
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

            Manager.PaintText(g, Localization["PTSReports_CountingDiscrepancies"]);
		}

	}

}
