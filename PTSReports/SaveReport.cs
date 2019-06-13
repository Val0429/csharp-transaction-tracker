using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace PTSReports
{
    public partial class SaveReport : UserControl, IControl, IAppUse
    {
        public event EventHandler OnSaveReport;
        
        public String TitleName { get; set; }
        public IApp App { get; set; }

        public Dictionary<String, String> Localization;
        
        public SaveReport()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SaveReport_Button", "Save Report"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;
            saveButton.Image = Resources.GetResources(Properties.Resources.report, Properties.Resources.IMGReport);

            SharedToolTips.SharedToolTip.SetToolTip(saveButton, Localization["SaveReport_Button"]);

            saveButton.MouseDown += ButtonMouseDown;
            saveButton.MouseUp += ButtonMouseUp;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            saveButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            saveButton.BackgroundImage = Properties.Resources.button;
        }

        private void SaveButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSaveReport != null)
                OnSaveReport(this, null);
        }
    }
}