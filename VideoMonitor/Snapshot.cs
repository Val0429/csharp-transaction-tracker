using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ApplicationForms = PanelBase.ApplicationForms;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class Snapshot : UserControl, IControl, IServerUse
    {
        public event EventHandler OnSnapshot;
        public String TitleName { get; set; }

        public IServer Server { get; set; }
        public Dictionary<String, String> Localization;

        public Snapshot()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Snapshot_Button", "Take All Snapshots"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            snapshotButton.MouseDown += ButtonMouseDown;
            snapshotButton.MouseUp += ButtonMouseUp;
            snapshotButton.MouseClick += SnapshotButtonMouseClick;
            SharedToolTips.SharedToolTip.SetToolTip(snapshotButton, Localization["Snapshot_Button"]);
            snapshotButton.Image = Resources.GetResources(Properties.Resources.snapshotButton, Properties.Resources.IMGSnapshotButton);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            snapshotButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            snapshotButton.BackgroundImage = Properties.Resources.button;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void SnapshotButtonMouseClick(Object sender, MouseEventArgs e)
        {
            ApplicationForms.ShowLoadingIcon(Server.Form);
            Application.Idle -= SendOnSnapshot;
            Application.Idle += SendOnSnapshot;
        }

        private void SendOnSnapshot(Object sender, EventArgs e)
        {
            Application.Idle -= SendOnSnapshot;
            if (OnSnapshot != null)
            {
                OnSnapshot(this, null);
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