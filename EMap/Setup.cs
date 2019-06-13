using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace EMap
{
    public partial class Setup : UserControl, IControl
    {
        public event EventHandler OnSetupMap;
        public String TitleName { get; set; }
        public IServer Server { get; set; }
        public Dictionary<String, String> Localization;

        public Setup()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"EMAP_SetupButton", "Setup eMap"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            setupImageButton.MouseDown += ButtonMouseDown;
            setupImageButton.MouseUp += ButtonMouseUp;

            SharedToolTips.SharedToolTip.SetToolTip(setupImageButton, Localization["EMAP_SetupButton"]);
            setupImageButton.Image = Resources.GetResources(Properties.Resources.setup_map, Properties.Resources.IMGSetupMapIcon);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            setupImageButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            setupImageButton.BackgroundImage = Properties.Resources.buttonBG;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void SaveImageButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSetupMap != null)
                OnSetupMap(this, null);
        }

        public void ChangeSetupMode(Object sender, EventArgs<Boolean> e)
        {
            //Visible = !e.Value;
            if(!e.Value)
            {
                setupImageButton.Image = Resources.GetResources(Properties.Resources.setup_map, Properties.Resources.IMGSetupMapIcon);
            }
            else
            {
                setupImageButton.Image = Resources.GetResources(Properties.Resources.setup_map_activate, Properties.Resources.IMGSetupMapActivateIcon);
            }
        }

        public void CheckUserPermission(Object sender, EventArgs<Boolean> e)
        {
            Visible = e.Value;
        }
    }
}