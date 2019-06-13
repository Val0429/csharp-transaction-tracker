using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class Broadcast : UserControl, IControl, IAppUse
    {
        public IApp App { get; set; }
        public event EventHandler OnBroadcastStart;
        public event EventHandler OnBroadcastStop;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public Broadcast()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Broadcast_Button", "Broadcast"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            broadcastButton.MouseDown += ButtonMouseDown;
            broadcastButton.MouseUp += ButtonMouseUp;

            SharedToolTips.SharedToolTip.SetToolTip(broadcastButton, Localization["Broadcast_Button"]);
            broadcastButton.Image = Resources.GetResources(Properties.Resources.broadcast, Properties.Resources.IMGBroadcast);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            App.IdleTimer = -1;
            broadcastButton.BackgroundImage = Properties.Resources.button_click;
            if (OnBroadcastStart != null)
                OnBroadcastStart(this, null);
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            App.IdleTimer = 0;
            broadcastButton.BackgroundImage = Properties.Resources.button;
            if (OnBroadcastStop != null)
                OnBroadcastStop(this, null);
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void BroadcastButtonMouseClick(Object sender, MouseEventArgs e)
        {
            
        }
    }
}