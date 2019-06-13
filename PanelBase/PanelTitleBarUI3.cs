using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;

namespace PanelBase
{
    public sealed partial class PanelTitleBarUI3 : UserControl
    {
        private IMinimize _panel;
        public IMinimize Panel
        {
            set
            {
                _panel = value;
                _panel.OnMinimizeChange += ObjectOnMinimizeChange;
            }
        }

        private static readonly Image _mini = Resources.GetResources(Properties.Resources.ui2_mini, Properties.Resources.IMGUI2Mini);
        private static readonly Image _mini2 = Resources.GetResources(Properties.Resources.ui2_mini2, Properties.Resources.IMGUI2Mini2);
        private static readonly Image _banner = Resources.GetResources(Properties.Resources.banner2, Properties.Resources.IMGBanner2);

        private void ObjectOnMinimizeChange(Object sender, EventArgs e)
        {
            minimizePictureBox.BackgroundImage = (_panel.IsMinimize) ? _mini : _mini2;
        }

        public PanelTitleBarUI3()
        {
            InitializeComponent();

            BackgroundImage = _banner;
            minimizePictureBox.BackgroundImage = _mini2;

            Dock = DockStyle.Fill;
            DoubleBuffered = true;

            Paint += PanelTitleBarPaint;
            TextChanged += PanelTitleBarTextChanged;
        }

        public void HideMinimize()
        {
            Controls.Remove(minimizePictureBox);
            
            if (_panel != null)
                _panel.OnMinimizeChange -= ObjectOnMinimizeChange;
        }

        private void PanelTitleBarTextChanged(Object sender, EventArgs e)
        {
            Invalidate();
        }

        private readonly Font _font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private void PanelTitleBarPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(Text, _font, Brushes.White, 22, 10);
        }

        private void MinimizePictureBoxClick(Object sender, MouseEventArgs e)
        {
            if (_panel == null) return;

            if (_panel.IsMinimize)
                _panel.Maximize();
            else
                _panel.Minimize();
        }
    }
}
