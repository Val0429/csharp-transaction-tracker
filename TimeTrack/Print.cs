using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;
using Interface;

namespace TimeTrack
{
    public partial class Print : UserControl, IControl
    {
        public event EventHandler OnPrintImage;
        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;
        private readonly ToolTip _toolTip = new ToolTip();

        public Print()
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

            _toolTip.SetToolTip(printButton, Localization["Print_Button"]);
            printButton.Image = Resources.GetResources(Properties.Resources.print_image, Properties.Resources.IMGPrint_image);
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
    }
}