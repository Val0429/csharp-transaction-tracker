using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;
using PanelBase;

namespace Investigation
{
    public partial class SaveResult : UserControl, IControl
    {
        public event EventHandler OnSaveResult;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public SaveResult()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SaveResult_Button", "Save Result"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            saveResultButton.MouseDown += ButtonMouseDown;
            saveResultButton.MouseUp += ButtonMouseUp;

            SharedToolTips.SharedToolTip.SetToolTip(saveResultButton, Localization["SaveResult_Button"]);
            saveResultButton.Image = Resources.GetResources(Properties.Resources.saveReport, Properties.Resources.IMGSaveReport);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            saveResultButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            saveResultButton.BackgroundImage = Properties.Resources.button;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void SaveResultButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnSaveResult != null)
                OnSaveResult(this, null);
        }

        public void VisibleChange(Object sender, EventArgs e)
        {
            if (sender is IMinimize && ((IMinimize)sender).IsMinimize)
            {
                Visible = false;
            }
        }

        public void DisplayButton(Object sender, EventArgs<String> e)
        {
            XmlDocument xmlDoc = Xml.LoadXml(e.Value);

            String buttons = Xml.GetFirstElementValueByTagName(xmlDoc, "Buttons");
            
            //in search result page
            if (buttons == "")
                Visible = true;
            else //in search condition page
                Visible = false; 
        }
    }
}