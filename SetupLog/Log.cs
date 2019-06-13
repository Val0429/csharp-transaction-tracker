using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupLog
{
    public partial class Setup
    {
        //private void LogDoubleBufferPanelPaint(Object sender, PaintEventArgs e)
        //{
        //    if (_logs.Count == 0)
        //        return;

        //    Graphics g = e.Graphics;
        //    List<Log> drawLog = _logs.GetRange(_logIndex, Math.Min((logDoubleBufferPanel.Height / 40), _logs.Count - _logIndex));

        //    Int16 index = 0;
        //    foreach (Log log in drawLog)
        //    {
        //        Int32 top = index * 40;

        //        if (_logs.IndexOf(log) == (_logs.Count - 1))
        //            Manager.PaintBottom(g, logDoubleBufferPanel, 40, logDoubleBufferPanel.Width, top);
        //        else
        //            Manager.PaintMiddle(g, logDoubleBufferPanel, 40, logDoubleBufferPanel.Width, top);

        //        if (logDoubleBufferPanel.Width <= 200) continue;

        //        log.Paint(g, _logIndex + index + 1, top, logDoubleBufferPanel.Width);

        //        index++;
        //    }
        //}
    }

    public sealed class LogPanel: Panel
    {
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
            Log.Paint(g, Index, 0, Width);
        }
    }
}
