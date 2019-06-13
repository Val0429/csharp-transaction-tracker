using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class ManualRecord : UserControl, IControl
    {
        public event EventHandler OnManualRecord;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public ManualRecord()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"ManualRecord_Button", "Manual Record All"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            manualRecordButton.MouseDown += ButtonMouseDown;
            manualRecordButton.MouseUp += ButtonMouseUp;

            SharedToolTips.SharedToolTip.SetToolTip(manualRecordButton, Localization["ManualRecord_Button"]);
            manualRecordButton.Image = Resources.GetResources(Properties.Resources.recordButton, Properties.Resources.IMGRecordButton);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            manualRecordButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            manualRecordButton.BackgroundImage = Properties.Resources.button;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void ManualRecordButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnManualRecord != null)
                OnManualRecord(this, null);
        }
    }
}