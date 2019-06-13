using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace TimeTrack
{
    public partial class ExportVideo : UserControl, IControl
    {
        public event EventHandler OnExportVideo;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public ExportVideo()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Export_Button", "Export Video"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            exportButton.MouseDown += ButtonMouseDown;
            exportButton.MouseUp += ButtonMouseUp;

            SharedToolTips.SharedToolTip.SetToolTip(exportButton, Localization["Export_Button"]);
            exportButton.Image = Resources.GetResources(Properties.Resources.export_video, Properties.Resources.IMGExport_video);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            exportButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            exportButton.BackgroundImage = Properties.Resources.button;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void ExportButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnExportVideo != null)
                OnExportVideo(this, null);
        }

        public void VisibleChange(Object sender, EventArgs e)
        {
            if (sender is IMinimize)
            {
                Visible = (((IMinimize)sender).IsMinimize) ? false : true;
            }
        }
    }
}