using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;

namespace SetupBase
{
    public partial class Undo : UserControl, IControl, IAppUse
    {
        public String TitleName { get; set; }
        public IApp App { get; set; }

        public Dictionary<String, String> Localization;

        public Undo()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Undo_Button", "Undo"},
                                   {"Undo_Warning", "Do you want to undo all settings you changed?"},
                                   {"MessageBox_Information","Information"}
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;
            undoButton.Image = Resources.GetResources(Properties.Resources.undo, Properties.Resources.IMGUndo);

            SharedToolTips.SharedToolTip.SetToolTip(undoButton, Localization["Undo_Button"]);
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void UndoButtonMouseClick(Object sender, MouseEventArgs e)
        {
            ApplicationForms.ShowProgressBar(App.Form);
            Application.RaiseIdle(null);

            var result = TopMostMessageBox.Show(Localization["Undo_Warning"], Localization["MessageBox_Information"],
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
                App.Undo();
            else
            {
                ApplicationForms.HideProgressBar();
            }
        }
    }
}