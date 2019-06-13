using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace POSRegister.Objects
{
    public sealed class FolderControl : Panel
    {
        public DoubleBufferPanel TitlePanel = new DoubleBufferPanel();
        public DoubleBufferPanel GroupControlContainer = new DoubleBufferPanel();

        private static readonly Image _arrowDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
        private static readonly Image _arrowUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);
        public FolderControl()
        {
            Dock = DockStyle.Top;
            DoubleBuffered = true;
            AutoSize = true;

            BackColor = Color.FromArgb(59, 62, 67);//drag label will use it
            ForeColor = Color.White; //drag label will use it

            TitlePanel.Size = new Size(220, 32);
            TitlePanel.Dock = DockStyle.Top;
            TitlePanel.Cursor = Cursors.Hand;

            GroupControlContainer.BackColor = Color.FromArgb(55, 59, 66);
            GroupControlContainer.Dock = DockStyle.Fill;
            GroupControlContainer.AutoSize = true;

            Controls.Add(GroupControlContainer);
            Controls.Add(TitlePanel);

            TitlePanel.MouseUp += TitlePanelMouseUp;

            TitlePanel.Paint += TitlePanelPaint;
        }

        private readonly Font _font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);

        private static RectangleF _nameRectangleF = new RectangleF(22, 8, 165, 19);
        private void TitlePanelPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage((GroupControlContainer.Visible ? _arrowUp : _arrowDown), 192, 13);

            g.DrawString(Name, _font, Brushes.White, _nameRectangleF);
        }

        private void TitlePanelMouseUp(Object sender, MouseEventArgs e)
        {
            GroupControlContainer.Visible = !GroupControlContainer.Visible;
            TitlePanel.Invalidate();
        }
    }
}