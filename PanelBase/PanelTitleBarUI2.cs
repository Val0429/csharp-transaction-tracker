using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Constant;

namespace PanelBase
{
    public sealed partial class PanelTitleBarUI2 : UserControl
    {
        private static readonly Image TitleBarBg = Resources.GetResources(Properties.Resources.titleBarBG, Properties.Resources.IMGTitleBarBG);

        private readonly Font _font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);


        public PanelTitleBarUI2()
        {
            InitializeComponent();

            BackgroundImage = TitleBarBg;

            Dock = DockStyle.Fill;
            DoubleBuffered = true;

            Paint += PanelTitleBarPaint;
            TextChanged += PanelTitleBarTextChanged;
        }


        // Properties
        [Browsable(true)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public MenuStrip MenuStrip { get; set; }

        public ToolStripMenuItem ToolStripMenuItem { get; set; }



        private void PanelTitleBarTextChanged(Object sender, EventArgs e)
        {
            Invalidate();
        }

        public void InitializeToolStripMenuItem()
        {
            MenuStrip = new MenuStrip
            {
                Dock = DockStyle.None,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                ShowItemToolTips = true,
                Padding = new Padding(0),
                Margin = new Padding(0),
                Size = new Size(42, 42),
                AutoSize = false,
            };

            ToolStripMenuItem = new ToolStripMenuItem
            {
                Alignment = ToolStripItemAlignment.Right,
                Size = new Size(42, 42),
                Image = Resources.GetResources(Properties.Resources.toolList, Properties.Resources.IMGToolList),
                ImageAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                DropDown = { BackColor = Color.FromArgb(39, 41, 44) },
            };

            MenuStrip.Items.Add(ToolStripMenuItem);

            Controls.Add(MenuStrip);
            MenuStrip.Location = new Point(Width - MenuStrip.Width, 0);
        }

        private void PanelTitleBarPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(Text, _font, Brushes.White, 18, 13);
        }
    }
}