
using System.Drawing;
using System.Windows.Forms;

namespace PanelBase
{
    public sealed class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
            Dock = DockStyle.None;
            DoubleBuffered = true;
            Location = new Point(0, 0);
            //Cursor = Cursors.WaitCursor;
            BackgroundImageLayout = ImageLayout.Center;
            //Paint += TransparentPanelPaint;
        }

        //private readonly SolidBrush _grayBrushes = new SolidBrush(Color.FromArgb(100, Color.Gray));
        //private void TransparentPanelPaint(Object sender, PaintEventArgs e)
        //{
        //    Graphics graphics = e.Graphics;
        //    graphics.FillRectangle(_grayBrushes, 0, 0, Width, Height);
        //}

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams createParams = base.CreateParams;
        //        createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
        //        return createParams;
        //    }
        //}

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
            // Do not paint background.
        //}
    }
}
