using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace EMap
{
    public partial class Save : UserControl, IControl
    {
        public event EventHandler OnSaveClick;

        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public Save()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Save_Button", "Save"},
                               };
            Localizations.Update(Localization);

            Visible = false;
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            saveImageButton.MouseDown += ButtonMouseDown;
            saveImageButton.MouseUp += ButtonMouseUp;

            saveImageButton.Image = Resources.GetResources(Properties.Resources.save_imageButton, Properties.Resources.IMGSaveButton);

            SharedToolTips.SharedToolTip.SetToolTip(saveImageButton, Localization["Save_Button"]);
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            saveImageButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            saveImageButton.BackgroundImage = Properties.Resources.buttonBG;
        }


        private void ButtonSaveClick(object sender, EventArgs e)
        {
            if (OnSaveClick!=null)
            {
                OnSaveClick(this, null);
            }
        }

        public void ChangeSetupMode(Object sender, EventArgs<Boolean> e)
        {
            Visible = e.Value;
        }

    }
}
