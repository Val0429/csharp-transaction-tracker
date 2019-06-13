using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupLicense
{
    public sealed partial class InfoControl : UserControl
    {
        public Dictionary<String, String> Localization;
        public Adaptor Adaptor;

        public InfoControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"LicensePanel_Description", "Description"},
                                   {"LicensePanel_MAC", "MAC"}
                               };
            Localizations.Update(Localization);

            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Top;

            macPanel.Paint += MacPaintInput;
            descriptionPanel.Paint += DescriptionPaintInput;
        }

        public void ParseInfo()
        {
            if (Adaptor.LicenseInfo.Count <= 0) return;

            foreach (var info in Adaptor.LicenseInfo)
            {
                Controls.Add(new InfoPanel { Info = info });
            }
            
            Controls.Add(macPanel);
            Controls.Add(descriptionPanel);
        }

        public void MacPaintInput(Object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;
            PaintInput(control, g);

            if (Width < 420) return;
            Manager.PaintTextRight(g, control, Adaptor.Mac);
        }

        public void DescriptionPaintInput(Object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;
            PaintInput(control, g);

            if (Width < 420) return;
            Manager.PaintTextRight(g, control, Adaptor.Description);
        }
        
        public void PaintInput(Control control, Graphics g)
        {
            Manager.Paint(g, control);

            if (Localization.ContainsKey("LicensePanel_" + control.Tag))
                Manager.PaintText(g, Localization["LicensePanel_" + control.Tag]);
            else
                Manager.PaintText(g, control.Tag.ToString());
        }
    }

    public class InfoPanel: Panel
    {
        public Dictionary<String, String> Localization;
        public LicenseInfo Info;// = null;
        public InfoPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"SetupLicense_LicenseKey", "License Key"},
                                   {"SetupLicense_LicenseQuantity", "License Quantity"},
                                   {"LicensePanel_Expired", "Expired"},
                                   {"LicensePanel_Trial", "Trial"},
                                   {"LicensePanel_ExpiresDate", "Expires on GMT %1"},
                               };
            Localizations.Update(Localization);

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Size = new Size(408, 40);

            Paint += InfoPanelPaint;
        }

        private void InfoPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Info == null) return;
            var control = (Control)sender;
            if (control == null || control.Parent == null) return;

            Graphics g = e.Graphics;
            Manager.Paint(g, control);
            Manager.PaintText(g, Localization["SetupLicense_LicenseKey"]);

            if(Width < 490) return;

            var info = Info.Serial + ", " + Localization["SetupLicense_LicenseQuantity"] + ": " + Info.Quantity;
            if (Info.Expired)
            {
                if (Width > 550)
                    info = "(" + Localization["LicensePanel_Expired"] + ") "  + info;

                Manager.PaintTextRight(g, control, info, Manager.DeleteTextColor);
            }
            else if (Info.Trial)
            {
                if (Width > 700)
                {
                    info = "(" + Localization["LicensePanel_Trial"] +
                        ((String.IsNullOrEmpty(Info.ExpiresDate))
                            ? ""
                            : (": " + Localization["LicensePanel_ExpiresDate"].Replace("%1", Info.ExpiresDate))) + ") " + info;
                }
                else if (Width > 550)
                    info = "(" + Localization["LicensePanel_Trial"] + ") " + info;

                Manager.PaintTextRight(g, control, info, Manager.SelectedTextColor);
            }
            else
            {
                Manager.PaintTextRight(g, control, info);
            }
        }
    }
}
