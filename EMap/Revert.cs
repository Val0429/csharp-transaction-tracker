using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace EMap
{
    public partial class Revert : UserControl, IControl, IServerUse
    {
        public event EventHandler<EventArgs<String>> OnRefreshMap;

        public String TitleName { get; set; }
        protected ICMS CMS;
        private IServer _server;
        public IServer Server
        {
            get { return _server; }
            set
            {
                _server = value;
                if (value is ICMS)
                    CMS = value as ICMS;
            }
        }
        public Dictionary<String, String> Localization;

        public Revert()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Confirm", "Confirm"},
                                   {"EMap_Revert", "Revert Settings"},
                                   {"EMap_RevertConfirm", "Do you want to revert the setting?"},
                               };
            Localizations.Update(Localization);

            Visible = false;
        }

        public virtual void Initialize()
        {
            InitializeComponent();
 
            Resources.ParseResources(this, "./EMap/EMap.xml", "Revert");

            Dock = DockStyle.Bottom;

            buttonRevert.MouseDown += ButtonMouseDown;
            buttonRevert.MouseUp += ButtonMouseUp;

            buttonRevert.Image = Resources.GetResources(Properties.Resources.refresh, Properties.Resources.IMGRefresh);

            SharedToolTips.SharedToolTip.SetToolTip(buttonRevert, Localization["EMap_Revert"]);
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            buttonRevert.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            buttonRevert.BackgroundImage = Properties.Resources.buttonBG;
        }

        private void ButtonRefreshClick(object sender, EventArgs e)
        {
            DialogResult result = TopMostMessageBox.Show(Localization["EMap_RevertConfirm"], Localization["MessageBox_Confirm"],
                                     MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if (OnRefreshMap != null)
                {

                    CMS.NVRManager.LoadMap();

                    OnRefreshMap(this, new EventArgs<String>("<xml><mode>refresh</mode></xml>"));
                }
            }
           
        }

        public void ChangeSetupMode(Object sender, EventArgs<Boolean> e)
        {
            Visible = e.Value;
        }
    }
}
