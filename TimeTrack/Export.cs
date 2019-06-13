using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;

namespace TimeTrack
{
    public partial class Export : UserControl, IControl
    {
        public event EventHandler OnExportVideo;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;
        private readonly ToolTip _toolTip = new ToolTip();

        public Export()
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

            _toolTip.SetToolTip(exportButton, Localization["Export_Button"]);
            exportButton.Image = Resources.GetResources(Properties.Resources.export_video, Properties.Resources.IMGExport_video);
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
    }
}