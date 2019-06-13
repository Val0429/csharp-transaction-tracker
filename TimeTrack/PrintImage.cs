using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace TimeTrack
{
    public partial class PrintImage : UserControl, IControl
    {
        public event EventHandler OnPrintImage;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        public PrintImage()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"Print_Button", "Print Image"},
                               };
            Localizations.Update(Localization);
        }

        public virtual void Initialize()
        {
            InitializeComponent();
            Dock = DockStyle.Bottom;

            printButton.MouseDown += ButtonMouseDown;
            printButton.MouseUp += ButtonMouseUp;

            SharedToolTips.SharedToolTip.SetToolTip(printButton, Localization["Print_Button"]);
            printButton.Image = Resources.GetResources(Properties.Resources.print_image, Properties.Resources.IMGPrintImage);
        }

        private void ButtonMouseDown(Object sender, MouseEventArgs e)
        {
            printButton.BackgroundImage = Properties.Resources.button_click;
        }

        private void ButtonMouseUp(Object sender, MouseEventArgs e)
        {
            printButton.BackgroundImage = Properties.Resources.button;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        private void PrintButtonMouseClick(Object sender, MouseEventArgs e)
        {
            if (OnPrintImage != null)
                OnPrintImage(this, null);
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