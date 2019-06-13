using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace PanelBase
{
    public class ToolStripMenuItemUI2 : ToolStripMenuItem
    {
        private static readonly Image _activate = Resources.GetResources(Properties.Resources.activate, Properties.Resources.IMGActivate);

        public Boolean IsSelected { get; set; }

        private Brush _selectBackColor = new SolidBrush(Color.FromArgb(72, 75, 80));
        private Brush _backColor = new SolidBrush(Color.FromArgb(59, 63, 69));
        private Font _font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        private Brush _fontColor = new SolidBrush(Color.FromArgb(142, 145, 152));
        private Pen _borderColor = new Pen(Color.FromArgb(39, 41, 44));


        public ToolStripMenuItemUI2()
        {
            
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            var text = Text;
            //only support (Ctrl + W) now
            if (ShortcutKeys == (Keys.Control | Keys.W))
            {
                text += @" (Ctrl + W)";
            }
            if (Selected)
            {
                if (Enabled)
                {
                    g.FillRectangle(_selectBackColor, e.ClipRectangle);
                    g.DrawString(text, _font, Brushes.WhiteSmoke, 36, 2);
                }
                else
                {
                    g.FillRectangle(_backColor, e.ClipRectangle);
                    g.DrawString(text, _font, _fontColor, 36, 2);
                }
            }
            else
            {
                g.FillRectangle(_backColor, e.ClipRectangle);
                g.DrawString(text, _font, _fontColor, 36, 2);
            }

            g.DrawLine(_borderColor, 0, Height - 1, Width - 1, Height - 1);

            if (IsSelected)
            {
                g.DrawImage(_activate, 3, 3);
            }
        }

        public void DecodeChange(Object sender, EventArgs<DecodeMode> e)
        {
            if (e.Value == DecodeMode.DecodeI)
            {
                if (Name == "Auto Drop Frame")
                {
                    IsSelected = false;
                }
                else if (Name == "Decode I-frame")
                {
                    IsSelected = true;
                }
            }
            else
            {
                if (Name == "Auto Drop Frame")
                {
                    IsSelected = true;
                }
                else if (Name == "Decode I-frame")
                {
                    IsSelected = false;
                }
            }
        }

        public void TitleBarVisibleChange(Object sender, EventArgs<Boolean> e)
        {
            if (e.Value)
            {
                if (Name == "Video Title Bar")
                    IsSelected = true;
            }
            else
            {
                if (Name == "Video Title Bar")
                    IsSelected = false;
            }
        }

        public void SetupMapModeChange(Object sender, EventArgs<Boolean> e)
        {
            if (e.Value)
            {
                if (Name == "Setup Map")
                    IsSelected = true;
            }
            else
            {
                if (Name == "Setup Map")
                    IsSelected = false;
            }
        }

        //call from xml
        public void InUse()
        {
            IsSelected = true;
        }
    }

    class CutomToolStripMenuRenderer : ToolStripProfessionalRenderer
    {
        public CutomToolStripMenuRenderer()
        {
            
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
                base.OnRenderMenuItemBackground(e);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
                base.OnRenderMenuItemBackground(e);
        }
    }
}
