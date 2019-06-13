using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Constant.Utility;
using SetupBase;

namespace SetupLog
{
    public sealed class LogPanel: Panel
    {
        private static readonly Image _action = Resources.GetResources(Properties.Resources.action_icon, Properties.Resources.IMGActionIcon);
        private static readonly Image _server = Resources.GetResources(Properties.Resources.service_icon, Properties.Resources.IMGServiceIcon);

        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private RectangleF _nameRectangleF = new RectangleF(180, 13, 130, 15);
        private RectangleF _userRectangleF = new RectangleF(300, 13, 120, 15);

        public Log Log;
        public Int32 Index;


        public LogPanel()
        {
            Height = 40;
            Dock = DockStyle.Top;
            DoubleBuffered = true;
            //BackColor = Color.White;

            Paint += LogPanelPaint;
        }

        private void LogPanelPaint(Object sender, PaintEventArgs e)
        {
            if(Log == null) return;

            Graphics g = e.Graphics;

            Manager.Paint(g, this);

            const int y = 0;
            g.DrawString(Index.ToString(), _font, Brushes.Black, 50, y + 13);

            if (Log.Type == LogType.Server)
                g.DrawImage(_server, 28, y + 14);
            else if (Log.Type == LogType.Action)
                g.DrawImage(_action, 28, y + 14);
            else if (Log.Type == LogType.Operation)
                g.DrawImage(_action, 28, y + 14);

            g.DrawString(Log.DateTime.ToTimeString(), _font, Brushes.Black, 90, 0 + 13);

            if (Width <= 380) return;

            _userRectangleF.Y = _nameRectangleF.Y = y + 13;

            if (Log.Type == LogType.Server)
            {
                g.DrawString(Log.User, _font, Brushes.Black, _nameRectangleF);
            }
            else
            {
                if (Log.FullDescription)
                {
                    if (Width <= 420) return;
                    g.DrawString(Log.User, _font, Brushes.Black, _userRectangleF);
                }
                else
                {
                    if (Width <= 310) return;
                    g.DrawString(Log.User, _font, Brushes.Black, _nameRectangleF);
                }
            }

            if (Log.FullDescription)
            {
                if (Width <= 550) return;
                g.DrawString(Log.Description, _font, Brushes.Black, 440, y + 13);
            }
            else
            {
                if (Width <= 430) return;
                g.DrawString(Log.Description, _font, Brushes.Black, 320, y + 13);
            }
        }
    }
}