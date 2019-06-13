using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace Interrogation
{
    public class EventResultPanel : Investigation.EventSearch.EventResultPanel
    {
        public EventResultPanel()
        {
            Localization.Add("EventResultPanel_Name", "Name");
            Localization.Add("EventResultPanel_NoOfRecord", "No. of Record");
            Localizations.Update(Localization);

            //DoubleBuffered = true;
            //Dock = DockStyle.Top;
            //Cursor = Cursors.PanSouth;// Cursors.Hand;
            //Size = new Size(640, 40);
        }

        protected override void EventResultPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;


            if (IsTitle)
            {
                Manager.PaintTitleTopInput(g, this);

                Manager.PaintTitleText(g, Localization["EventResultPanel_NoOfRecord"]);

                if (Width <= 150) return;
                g.DrawString(Localization["EventResultPanel_Name"], Manager.Font, Manager.TitleTextColor, 180, 13);

                if (Width <= 550) return;
                g.DrawString(Localization["EventResultPanel_DateTime"], Manager.Font, Manager.TitleTextColor, 580, 13);

                if (Width <= 750) return;
                g.DrawString(Localization["EventResultPanel_Device"], Manager.Font, Manager.TitleTextColor, 780, 13);
                return;
            }

            Manager.Paint(g, this);
            if (CameraEvent == null) return;

            if (Height == 40)
                Manager.PaintExpand(g, this);
            else
                Manager.PaintCollapse(g, this);

            var xmlDoc = Xml.LoadXml(CameraEvent.Status);

            var name = Xml.GetFirstElementValueByTagName(xmlDoc, "Name");
            var record = Xml.GetFirstElementValueByTagName(xmlDoc, "NoOfRecord");

            Manager.PaintText(g, record);

            if (Width <= 150) return;
            g.DrawString(name, Manager.Font, Brushes.Black, 180, 13);

            if (Width <= 550) return;
            g.DrawString(CameraEvent.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), Manager.Font, Brushes.Black, 580, 13);

            if (Width <= 750) return;
            g.DrawString(CameraEvent.Device.ToString(), Manager.Font, Brushes.Black, 780, 13);
        }
    }
}
