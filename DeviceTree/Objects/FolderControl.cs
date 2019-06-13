using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace DeviceTree.Objects
{
    public sealed class FolderControl : Panel
    {
        public DoubleBufferPanel TitlePanel = new DoubleBufferPanel();
        public DoubleBufferPanel GroupControlContainer = new DoubleBufferPanel();
        public Dictionary<String, String> Localization;

        private static readonly Image _arrowDown = Resources.GetResources(Properties.Resources.arrow_down, Properties.Resources.IMGArrowDown);
        private static readonly Image _arrowUp = Resources.GetResources(Properties.Resources.arrow_up, Properties.Resources.IMGArrowUp);
        private static readonly Image _titlePanelBg = Resources.GetResources(Properties.Resources.folderPanelBG, Properties.Resources.IMGFolderPanelBG);
        public FolderControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"FolderControl_NumView", "(%1 View)"},
                                   {"FolderControl_NumViews", "(%1 Views)"},
                               };
            Localizations.Update(Localization);

            Dock = DockStyle.Top;
            DoubleBuffered = true;
            AutoSize = true;

            BackColor = Color.FromArgb(59, 62, 67);//drag label will use it
            ForeColor = Color.White; //drag label will use it

            TitlePanel.Size = new Size(220, 32);
            TitlePanel.Dock = DockStyle.Top;
            TitlePanel.BackgroundImage = _titlePanelBg;
            TitlePanel.BackgroundImageLayout = ImageLayout.Center;
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

            var name = Name + "   " +
                       ((GroupControlContainer.Controls.Count <= 1) ? Localization["FolderControl_NumView"] : Localization["FolderControl_NumViews"]).
                           Replace("%1", GroupControlContainer.Controls.Count.ToString());

            g.DrawString(name, _font, Brushes.White, _nameRectangleF);
        }

        private void TitlePanelMouseUp(Object sender, MouseEventArgs e)
        {
            GroupControlContainer.Visible = !GroupControlContainer.Visible;
            TitlePanel.Invalidate();
        }

        public void UpdateRecordingStatus()
        {
            foreach (GroupControlUI2 groupControl in GroupControlContainer.Controls)
            {
                groupControl.UpdateRecordingStatus();
            }
        }
    }
}