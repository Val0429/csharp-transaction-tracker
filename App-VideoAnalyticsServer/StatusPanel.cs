using System;
using System.Drawing;
using System.Windows.Forms;
using App;

namespace App_VideoAnalyticsServer
{
    public partial class VideoAnalyticsServer
    {
        private Panel _timePanel;

        protected override void InitializeStatePanel()
        {
            base.InitializeStatePanel();

            _timePanel = ApplicationForms.TimePanel();

            StatePanel.Controls.Add(_timePanel);

            _timePanel.Paint += TimePanelPaint;
        }

        private readonly Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private void TimePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var text = _vas.Server.Location + @" " + _vas.Server.DateTime.ToString("yyyy-MM-dd HH:mm:ss");

            g.DrawString(text, _font, Brushes.Black, 25, 4);
        }
    }
}
