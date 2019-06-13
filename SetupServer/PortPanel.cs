using System;
using System.Drawing;
using System.Windows.Forms;
using Interface;
using PanelBase;

namespace SetupServer
{
    public sealed class PortPanel : Panel
    {
        public IServer Server { get; set; }

        public PortPanel()
        {
            DoubleBuffered = true;
            Anchor = ((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Height = 40;
            Cursor = Cursors.Hand;

            Paint += PortPanelPaint;
        }

        private void PortPanelPaint(Object sender, PaintEventArgs e)
        {
            if (Parent == null) return;

            Graphics g = e.Graphics;

            if (Parent.Controls[0] == this)
            {
                Manager.PaintBottom(g, this);
            }
            else
            {
                Manager.PaintMiddle(g, this);
            }

            if (Width <= 100) return;
            if (Tag.ToString() == Server.Server.Port.ToString())
            {
                Manager.PaintText(g, Tag.ToString(), Manager.SelectedTextColor);
                Manager.PaintSelected(g);
            }
            else
                Manager.PaintText(g, Tag.ToString());
        }
    }
}