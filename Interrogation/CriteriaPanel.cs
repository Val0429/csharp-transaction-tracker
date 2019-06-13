using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace Interrogation
{
    public partial class CriteriaPanel : Investigation.EventSearch.CriteriaPanel
    {
        public String CriteriaName
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        public String CriteriaNumberofRecord
        {
            get { return noofRecordTextBox.Text; }
            set { noofRecordTextBox.Text = value; }
        }

        public CriteriaPanel()
        {
            Localization.Add("CriteriaPanel_Name", "Name");
            Localization.Add("CriteriaPanel_NoOfRecord", "No. of Record");
            Localizations.Update(Localization);

            InitializeComponent();
        }

        public override void Initialize()
        {
            base.Initialize();

            nameDoubleBufferPanel.Paint += DoubleBufferPanelPaint;
            noofrecordDoubleBufferPanel.Paint += DoubleBufferPanelPaint;
        }

        private void DoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            Graphics g = e.Graphics;

            Manager.PaintSingleInput(g, control);

            if (Localization.ContainsKey("CriteriaPanel_" + control.Tag))
                Manager.PaintText(g, Localization["CriteriaPanel_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }

        public void ClearCriteria()
        {
            nameTextBox.Text = "";
            noofRecordTextBox.Text = "";
        }
    }
}
