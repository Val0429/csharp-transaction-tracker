using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace SetupBase
{
    public static partial class Manager
    {
        //Background
        public static readonly Image Background = Resources.GetResources(Properties.Resources.bg, Properties.Resources.IMGBg);
        public static readonly Image BackgroundNoBorder = Resources.GetResources(Properties.Resources.bg_noborder, Properties.Resources.IMGBgNoborder);

        //Highlight Input
        private static readonly Image LeftInputHighLight = Resources.GetResources(Properties.Resources.input_left_highlight, Properties.Resources.IMGInputLeftHighlight);
        private static readonly Image RightInputHighLight = Resources.GetResources(Properties.Resources.input_right_highlight, Properties.Resources.IMGInputRightHighlight);
        private static readonly Image BgInputHighLight = Resources.GetResources(Properties.Resources.inputBG_highlight, Properties.Resources.IMGInputBgHighlight);
        private static readonly Image TopRightHighLight = Resources.GetResources(Properties.Resources.tz_top_right_highlight, Properties.Resources.IMGTzTopRightHighlight);
        private static readonly Image TopLeftHighLight = Resources.GetResources(Properties.Resources.tz_top_left_highlight, Properties.Resources.IMGTzTopLeftHighlight);

        //Title Input
        private static readonly Image BgInputTitle = Resources.GetResources(Properties.Resources.inputBG_title, Properties.Resources.IMGInputBgTitle);
        private static readonly Image TopRightTitle = Resources.GetResources(Properties.Resources.tz_top_right_title, Properties.Resources.IMGTzTopRightTitle);
        private static readonly Image TopLeftTitle = Resources.GetResources(Properties.Resources.tz_top_left_title, Properties.Resources.IMGTzTopLeftTitle);

        //Left Input
        private static readonly Image InputTopLeftRound = Resources.GetResources(Properties.Resources.input_tl_round, Properties.Resources.IMGInputTopLeftRound);
        private static readonly Image InputBottomLeftRound = Resources.GetResources(Properties.Resources.input_bl_round, Properties.Resources.IMGInputBottomLeftRound);
        private static readonly Image InputLeftBorder = Resources.GetResources(Properties.Resources.input_l_border, Properties.Resources.IMGInputLeftBorder);
        private static readonly Image InputLeftSplit = Resources.GetResources(Properties.Resources.input_left_split, Properties.Resources.IMGInputLeftSplit);
        //private static readonly TextureBrush InputLeftBorderBrush = new TextureBrush(InputLeftBorder, WrapMode.Tile);

        //Right Input
        private static readonly Image InputTopRightRound = Resources.GetResources(Properties.Resources.input_tr_round, Properties.Resources.IMGInputTopRightRound);
        private static readonly Image InputBottomRightRound = Resources.GetResources(Properties.Resources.input_br_round, Properties.Resources.IMGInputBottomRightRound);
        private static readonly Image InputRightBorder = Resources.GetResources(Properties.Resources.input_r_border, Properties.Resources.IMGInputRightBorder);
        private static readonly Image InputRightSplit = Resources.GetResources(Properties.Resources.input_right_split, Properties.Resources.IMGInputRightSplit);
        //private static readonly TextureBrush InputRightBorderBrush = new TextureBrush(InputRightBorder, WrapMode.Tile);

        // Input Top / Bottom
        private static readonly Image InputTopBorder = Resources.GetResources(Properties.Resources.input_t_border, Properties.Resources.IMGInputTopBorder);
        private static readonly Image InputBottomBorder = Resources.GetResources(Properties.Resources.input_b_border, Properties.Resources.IMGInputBottomBorder);

        //Right Top&Bottom
        private static readonly Image TopBorder = Resources.GetResources(Properties.Resources.input_t_border, Properties.Resources.IMGTopBorder);
        private static readonly Image BottomBorder = Resources.GetResources(Properties.Resources.input_b_border, Properties.Resources.IMGBottomBorder);
        private static readonly Image LeftBorder = Resources.GetResources(Properties.Resources.input_l_border, Properties.Resources.IMGLeftBorder);
        private static readonly Image RightBorder = Resources.GetResources(Properties.Resources.input_r_border, Properties.Resources.IMGRightBorder);
        private static readonly Image TopLeftRound = Resources.GetResources(Properties.Resources.input_tl_round, Properties.Resources.IMGTopLeftRound);
        private static readonly Image TopRightRound = Resources.GetResources(Properties.Resources.input_tr_round, Properties.Resources.IMGTopRightRound);
        private static readonly Image BottomLeftRound = Resources.GetResources(Properties.Resources.input_bl_round, Properties.Resources.IMGBottomLeftRound);
        private static readonly Image BottomRightRound = Resources.GetResources(Properties.Resources.input_br_round, Properties.Resources.IMGBottomRightRound);

        private static readonly SolidBrush InputBrush = StyleResource.InputBrush;
        private static readonly SolidBrush InputBrushe = new SolidBrush(Color.FromArgb(234, 235, 239));

        private static readonly Image Selected = Resources.GetResources(Properties.Resources.selected, Properties.Resources.IMGSelected);
        private static readonly Image Edit = Resources.GetResources(Properties.Resources.edit, Properties.Resources.IMGEdit);
        private static readonly Image Expand = Resources.GetResources(Properties.Resources.expand, Properties.Resources.IMGExpand);
        private static readonly Image Collapse = Resources.GetResources(Properties.Resources.collapse, Properties.Resources.IMGCollapse);
        private static readonly Image New = Resources.GetResources(Properties.Resources._new, Properties.Resources.IMGNew);
        private static readonly Image Modify = Resources.GetResources(Properties.Resources.modify, Properties.Resources.IMGModify);
        private static readonly Image Unavailable = Resources.GetResources(Properties.Resources.unavailable, Properties.Resources.IMGUnavailable);

        public static readonly Brush DeleteTextColor = new SolidBrush(Color.FromArgb(237, 57, 28)); //Brushes.RoyalBlue
        public static readonly Brush SelectedTextColor = new SolidBrush(Color.FromArgb(77, 177, 224)); //Brushes.RoyalBlue
        public static readonly Brush TitleTextColor = Brushes.WhiteSmoke;
        public static readonly Font Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        public static readonly Font BoldFont = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);

        public static void Paint(Graphics graphics, Control control, Boolean hightLight)
        {
            if (control.Parent == null) return;

            var controls = new List<Control>();

            foreach (Control obj in control.Parent.Controls)
            {
                if (!obj.Visible) continue;
                controls.Add(obj);
            }

            if (controls.Count == 0) return;
            if (!controls.Contains(control)) return;

            if (controls.Count == 1)
            {
                if (hightLight)
                    PaintHighLightInput(graphics, control);
                else
                    PaintSingleInput(graphics, control);
                return;
            }

            if (controls[controls.Count - 1] == control)
            {
                if (hightLight)
                    PaintHighLightTopInput(graphics, control);
                else
                    PaintTop(graphics, control);
                return;
            }

            if (controls[0] == control)
            {
                PaintBottom(graphics, control);
                return;
            }

            PaintMiddle(graphics, control);
        }

        public static void Paint(Graphics graphics, Control control)
        {
            Paint(graphics, control, false);
        }

        public static void PaintTop(Graphics graphics, Control control, Int32 height, Int32 width)
        {
            graphics.FillRectangle(InputBrushe, TopLeftRound.Width, TopBorder.Height, width - TopLeftRound.Width - TopRightRound.Width, height);
            graphics.DrawImage(TopBorder, TopLeftRound.Width, 0, width - TopLeftRound.Width - TopRightRound.Width, TopBorder.Height);
            graphics.DrawImage(BottomBorder, InputLeftSplit.Width, height - BottomBorder.Height, width - InputLeftSplit.Width - InputRightSplit.Width, BottomBorder.Height);

            graphics.DrawImage(LeftBorder, 0, TopLeftRound.Height, LeftBorder.Width, height);
            graphics.DrawImage(TopLeftRound, 0, 0);
            graphics.DrawImage(InputLeftSplit, 0, height - InputLeftSplit.Height);

            graphics.DrawImage(RightBorder, width - RightBorder.Width, TopRightRound.Height, RightBorder.Width, height);
            graphics.DrawImage(TopRightRound, width - TopRightRound.Width, 0);
            graphics.DrawImage(InputRightSplit, width - InputRightSplit.Width, height - InputRightSplit.Height);
        }

        public static void PaintTop(Graphics graphics, Control control)
        {
            PaintTop(graphics, control, control.Height, control.Width);
        }

        public static void PaintMiddle(Graphics graphics, Control control, Int32 height, Int32 width, Int32 top)
        {
            graphics.FillRectangle(InputBrushe, InputLeftSplit.Width, top, width - InputLeftSplit.Width - InputRightSplit.Width, height);
            graphics.DrawImage(BottomBorder, InputLeftSplit.Width, top + height - BottomBorder.Height, width - InputLeftSplit.Width - InputRightSplit.Width, BottomBorder.Height);

            graphics.DrawImage(LeftBorder, 0, top, LeftBorder.Width, height);
            graphics.DrawImage(InputLeftSplit, 0, top + height - InputLeftSplit.Height);

            graphics.DrawImage(RightBorder, width - RightBorder.Width, top, RightBorder.Width, height);
            graphics.DrawImage(InputRightSplit, width - InputRightSplit.Width, top + height - InputRightSplit.Height);
        }

        public static void PaintMiddle(Graphics graphics, Control control)
        {
            PaintMiddle(graphics, control, control.Height, control.Width, 0);
        }

        public static void PaintBottom(Graphics graphics, Control control, Int32 height, Int32 width, Int32 top)
        {
            graphics.FillRectangle(InputBrushe, BottomLeftRound.Width, top, width - BottomLeftRound.Width - BottomRightRound.Width, height);
            graphics.DrawImage(BottomBorder, BottomLeftRound.Width, top + height - BottomBorder.Height, width - BottomLeftRound.Width - BottomRightRound.Width, BottomBorder.Height);

            graphics.DrawImage(LeftBorder, 0, top, LeftBorder.Width, height - BottomLeftRound.Height);
            graphics.DrawImage(BottomLeftRound, 0, top + height - BottomLeftRound.Height, BottomLeftRound.Width, BottomLeftRound.Height);

            graphics.DrawImage(RightBorder, width - RightBorder.Width, top, RightBorder.Width, height - BottomRightRound.Height);
            graphics.DrawImage(BottomRightRound, width - BottomRightRound.Width, top + height - BottomRightRound.Height, BottomRightRound.Width, BottomRightRound.Height);
        }

        public static void PaintBottom(Graphics graphics, Control control)
        {
            PaintBottom(graphics, control, control.Height, control.Width, 0);
        }

        public static void PaintSingleInput(Graphics graphics, Control control, Int32 height, Int32 width)
        {
            if (control.Parent == null) return;

            graphics.FillRectangle(InputBrush, InputTopLeftRound.Width, InputTopBorder.Height, width - InputTopLeftRound.Width - InputTopRightRound.Width, height);
            graphics.DrawImage(InputTopBorder, InputTopLeftRound.Width, 0, width - InputTopLeftRound.Width - TopRightRound.Width, InputTopBorder.Height);
            graphics.DrawImage(InputBottomBorder, InputBottomLeftRound.Width, height - InputBottomBorder.Height, width - InputBottomLeftRound.Width - InputBottomRightRound.Width, InputBottomBorder.Height);

            graphics.DrawImage(InputLeftBorder, 0, InputTopLeftRound.Height, InputLeftBorder.Width, height - InputTopLeftRound.Height - InputBottomLeftRound.Height);
            graphics.DrawImage(InputTopLeftRound, 0, 0, InputTopLeftRound.Width, TopLeftRound.Height);
            graphics.DrawImage(InputBottomLeftRound, 0, height - InputBottomLeftRound.Height, InputBottomLeftRound.Width, InputBottomLeftRound.Height);

            graphics.DrawImage(InputRightBorder, width - InputRightBorder.Width, InputTopRightRound.Height, InputRightBorder.Width, height - InputTopRightRound.Height - InputBottomRightRound.Height);
            graphics.DrawImage(InputTopRightRound, width - InputTopRightRound.Width, 0, InputTopRightRound.Width, InputTopRightRound.Height);
            graphics.DrawImage(InputBottomRightRound, width - InputBottomRightRound.Width, height - InputBottomRightRound.Height, InputBottomRightRound.Width, InputBottomRightRound.Height);
        }

        public static void PaintSingleInput(Graphics graphics, Control control)
        {
            PaintSingleInput(graphics, control, control.Height, control.Width);
        }

        public static void PaintHighLightTopInput(Graphics graphics, Control control)
        {
            PaintHighLightTopInput(graphics, control, control.Height, control.Width);
        }

        public static void PaintHighLightTopInput(Graphics graphics, Control control, Int32 height, Int32 width)
        {
            if (control.Parent == null) return;

            graphics.DrawImage(BgInputHighLight, TopLeftHighLight.Width - 2, 0, width - TopLeftHighLight.Width - TopRightHighLight.Width + 4, height);

            graphics.DrawImage(TopLeftHighLight, 0, 0);

            graphics.DrawImage(TopRightHighLight, width - TopRightHighLight.Width, 0);
        }

        public static void PaintTitleTopInput(Graphics graphics, Control control)
        {
            PaintTitleTopInput(graphics, control, control.Height, control.Width);
        }

        public static void PaintTitleTopInput(Graphics graphics, Control control, Int32 height, Int32 width)
        {
            if (control.Parent == null) return;

            graphics.DrawImage(BgInputTitle, TopLeftTitle.Width - 2, 0, width - TopLeftTitle.Width - TopRightTitle.Width + 4, height);

            graphics.DrawImage(TopLeftTitle, 0, 0, TopLeftTitle.Width, TopLeftTitle.Height);

            graphics.DrawImage(TopRightTitle, width - TopRightTitle.Width, 0, TopRightTitle.Width, TopRightTitle.Height);
        }

        public static void PaintTitleMiddleInput(Graphics graphics, Control control)
        {
            PaintTitleMiddleInput(graphics, control, control.Height, control.Width, 0);
        }

        public static void PaintTitleMiddleInput(Graphics graphics, Control control, Int32 height, Int32 width, Int32 top)
        {
            if (control.Parent == null) return;
            graphics.DrawImage(BgInputTitle, 0, 0, width, height);
        }

        public static void PaintHighLightInput(Graphics graphics, Control control, Int32 height, Int32 width)
        {
            if (control.Parent == null) return;

            graphics.DrawImage(BgInputHighLight, LeftInputHighLight.Width - 2, 0, width - LeftInputHighLight.Width - RightInputHighLight.Width + 4, height);

            graphics.DrawImage(LeftInputHighLight, 0, 0, LeftInputHighLight.Width, height);

            graphics.DrawImage(RightInputHighLight, width - RightInputHighLight.Width, 0, RightInputHighLight.Width, height);
        }

        public static void PaintHighLightInput(Graphics graphics, Control control)
        {
            PaintHighLightInput(graphics, control, control.Height, control.Width);
        }

        public static void PaintEdit(Graphics graphics, Control control, Int32 height = 15)
        {
            graphics.DrawImage(Edit, control.Width - Edit.Width - 10, height);
        }

        public static void PaintExpand(Graphics graphics, Control control)
        {
            graphics.DrawImage(Expand, control.Width - Expand.Width - 10, 15);
        }

        public static void PaintCollapse(Graphics graphics, Control control)
        {
            graphics.DrawImage(Collapse, control.Width - Collapse.Width - 10, control.Height - Collapse.Height - 10);
        }

        public static void PaintSelected(Graphics graphics)
        {
            graphics.DrawImage(Selected, 10, 13);
        }

        public static void PaintSelected(Graphics graphics, Int32 x, Int32 y)
        {
            graphics.DrawImage(Selected, x, y);
        }

        public static void PaintStatus(Graphics graphics, ReadyState readyState, Int32 height = 12)
        {
            if (readyState == ReadyState.New)
                graphics.DrawImage(New, 25, height);
            else if (readyState == ReadyState.Modify)
                graphics.DrawImage(Modify, 25, height);
            else if (readyState == ReadyState.Unavailable)
                graphics.DrawImage(Unavailable, 25, height);
        }

        public static void PaintText(Graphics graphics, String text, Brush brushe, Int32 width = 44, Int32 height = 13)
        {
            if (String.IsNullOrEmpty(text)) return;
            graphics.DrawString(text, Font, brushe, width, height);
        }

        public static void PaintTextBold(Graphics graphics, String text, Brush brushe, Int32 width = 44, Int32 height = 13)
        {
            if (String.IsNullOrEmpty(text)) return;
            graphics.DrawString(text, BoldFont, brushe, width, height);
        }

        public static void PaintTitleText(Graphics graphics, String text)
        {
            if (String.IsNullOrEmpty(text)) return;
            PaintText(graphics, text, TitleTextColor);
        }

        public static void PaintText(Graphics graphics, String text)
        {
            if (String.IsNullOrEmpty(text)) return;
            PaintText(graphics, text, Brushes.Black);
        }

        public static void PaintTextBold(Graphics graphics, String text)
        {
            if (String.IsNullOrEmpty(text)) return;
            PaintTextBold(graphics, text, Brushes.Black);
        }

        public static void PaintTextRight(Graphics graphics, Control control, String text, Brush brushe, Int32 height = 13)
        {
            if (String.IsNullOrEmpty(text)) return;
            SizeF fSize = graphics.MeasureString(text, Font);
            graphics.DrawString(text, Font, brushe, control.Width - fSize.Width - Edit.Width - LeftBorder.Width, height);
        }

        public static void PaintTextRight(Graphics graphics, Control control, String text)
        {
            if (String.IsNullOrEmpty(text)) return;
            PaintTextRight(graphics, control, text, Brushes.Black);
        }
    }
}
