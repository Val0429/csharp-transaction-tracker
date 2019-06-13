using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class SaveImage : UserControl, IControl, IServerUse
    {
        public event EventHandler OnSaveImage;
        public String TitleName { get; set; }
        public IServer Server { get; set; }
        public Dictionary<String, String> Localization;

        public SaveImage()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SaveImage_Button", "Save All Images"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            saveImageButton.MouseDown += ButtonMouseDown;
            saveImageButton.MouseUp += ButtonMouseUp;

            SharedToolTips.SharedToolTip.SetToolTip(saveImageButton, Localization["SaveImage_Button"]);
            saveImageButton.Image = Resources.GetResources(Properties.Resources.save_imageButton, Properties.Resources.IMGSaveImageButton);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            saveImageButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            saveImageButton.BackgroundImage = Properties.Resources.button;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void SaveImageButtonMouseClick(Object sender, MouseEventArgs e)
        {
            ApplicationForms.ShowLoadingIcon(Server.Form);
            Application.Idle -= SendOnSnapshot;
            Application.Idle += SendOnSnapshot;
        }

        private void SendOnSnapshot(Object sender, EventArgs e)
        {
            Application.Idle -= SendOnSnapshot;
            if (OnSaveImage != null)
            {
                OnSaveImage(this, null);
                Application.Idle -= HideLoading;
                Application.Idle += HideLoading;
            }
        }

        private void HideLoading(Object sender, EventArgs e)
        {
            Application.Idle -= HideLoading;
            ApplicationForms.HideLoadingIcon();
        }
    }
}