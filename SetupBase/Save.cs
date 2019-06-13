using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace SetupBase
{
    public partial class Save : UserControl, IControl, IAppUse
    {
        public String TitleName { get; set; }
        public IApp App { get; set; }

        public Dictionary<String, String> Localization;
        
        public Save()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Save_Button", "Save"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;
            saveButton.Image = Resources.GetResources(Properties.Resources.save, Properties.Resources.IMGSave);

            SharedToolTips.SharedToolTip.SetToolTip(saveButton, Localization["Save_Button"]);
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void SaveButtonMouseClick(Object sender, MouseEventArgs e)
        {
            App.Save();
        }
    }
}