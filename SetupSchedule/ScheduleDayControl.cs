using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace SetupSchedule
{
    public sealed class ScheduleDayControl : Panel
    {
        public UInt32 StartTime; //Second
        public UInt32 EndTime; //Second
        public String Day;
        public ScheduleControl ScheduleControl;
        public Boolean IsSelected;

        public ScheduleDayControl()
        {
            Cursor = Cursors.Default;
            DoubleBuffered = true;
            Dock = DockStyle.Top;
            Size = new Size(500, 30);
            MinimumSize = new Size(120, 30);

            Paint += ScheduleDayControlPaint;
            MouseDown += ScheduleDayControlMouseDown;
            MouseUp += ScheduleDayControlMouseUp;
        }

        private Int32 _mouseX;
        private Int32 _mouseY;

        private void ScheduleDayControlMouseUp(Object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            MouseMove -= ScheduleDayControlMouseMove;
        }

        private void ScheduleDayControlMouseDown(Object sender, MouseEventArgs e)
        {
            if (e.X < ScheduleControl.StartPosition || e.X > _maxWidth + ScheduleControl.StartPosition) return;

            Cursor = Cursors.SizeWE;

            _mouseX = e.X;
            _mouseY = e.Y;
            MouseMove -= ScheduleDayControlMouseMove;
            MouseMove += ScheduleDayControlMouseMove;
        }

        private void ScheduleDayControlMouseMove(Object sender, MouseEventArgs e)
        {
            if (_mouseX != e.X)
            {
                MouseMove -= ScheduleDayControlMouseMove;

                ScheduleControl.SelectDay = this;

                ScheduleControl.SelectStartTime = (e.X > _mouseX)
                    ? AsStart(ScheduleTime(e.X))
                    : AsEnd(ScheduleTime(e.X));

                ScheduleControl.SelectStartX = Convert.ToUInt16(e.X);
                ScheduleControl.SelectStartY = Convert.ToUInt16(_mouseY + Location.Y);

                ScheduleControl.Capture = true;
            }
        }

        private static readonly Image _left = Resources.GetResources(Properties.Resources.sh_left, Properties.Resources.BGSh_left);
        private static readonly Image _right = Resources.GetResources(Properties.Resources.sh_right, Properties.Resources.BGSh_right);
        private static readonly Image _bottomLeft = Resources.GetResources(Properties.Resources.sh_bottom_left, Properties.Resources.BGSh_bottom_left);
        private static readonly Image _bottomRight = Resources.GetResources(Properties.Resources.sh_bottom_right, Properties.Resources.BGSh_bottom_right);
        private static readonly Image _bgMidleInput = Resources.GetResources(Properties.Resources.inputBG_middle, Properties.Resources.BGInputBG_middle);
        private readonly Pen _scaledPen = new Pen(Color.FromArgb(145, 145, 145));
        private readonly Pen _scaledPen2 = new Pen(Color.FromArgb(215, 215, 215));

        private readonly SolidBrush _eventBrushes = new SolidBrush(Color.FromArgb(200, 254, 197, 126));
        private readonly SolidBrush _recordBrushes = new SolidBrush(Color.FromArgb(200, 161, 190, 141));
        private readonly SolidBrush _eventhandleBrushes = new SolidBrush(Color.FromArgb(200, 254, 197, 126)); //200, 172, 172, 99
        private readonly SolidBrush _selectionBrushes = new SolidBrush(Color.FromArgb(150, 125, 192, 255));

        private UInt16 _maxWidth;
        private UInt16 _gap;

        private static RectangleF _dayRectangleF = new RectangleF(15, 7, 60, 17);
        private void ScheduleDayControlPaint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Image left = _left;
            Image right = _right;
            if (Parent.Controls[0] == this)
            {
                left = _bottomLeft;
                right = _bottomRight;
            }
            g.DrawImage(_bgMidleInput, left.Width - 2, 0, ScheduleControl.ScheduleWidth - left.Width - right.Width + 4, 30);

            g.DrawImage(left, 0, 0);

            g.DrawImage(right, ScheduleControl.ScheduleWidth - right.Width, 0);

            g.DrawString(Day, Manager.Font, Brushes.Black, _dayRectangleF);

            if (ScheduleControl == null || ScheduleControl.Diff == 0 || ScheduleControl.ScheduleWidth < 400 || ScheduleControl.SchedulePanel == null || ScheduleControl.SchedulePanel.Camera == null) return;

            _gap = Convert.ToUInt16(ScheduleControl.Diff / 3);

            _maxWidth = Convert.ToUInt16(ScheduleControl.Diff * 24);
            Int32 start = ScheduleControl.StartPosition;
            for (UInt16 hour = 0; hour <= 24; hour++)
            {
                g.DrawLine(_scaledPen, start, 0, start, 30);

                if (hour == 24) break;

                if (_gap > 8)
                {
                    g.DrawLine(_scaledPen2, start + _gap, 0, start + _gap, 30);
                    g.DrawLine(_scaledPen2, start + ScheduleControl.Diff - _gap, 0, start + ScheduleControl.Diff - _gap, 30);
                }

                start += ScheduleControl.Diff;
            }

            Schedule schedule = (ScheduleControl.Data == "Record")
                ? ScheduleControl.SchedulePanel.Camera.RecordSchedule
                : ScheduleControl.SchedulePanel.Camera.EventSchedule;

            foreach (ScheduleData scheduleData in schedule)
            {
                if (scheduleData.EndTime < StartTime || scheduleData.StartTime > EndTime) continue;

                UInt32 startTime = Math.Max(scheduleData.StartTime, StartTime);
                UInt32 endTime = Math.Min(scheduleData.EndTime, EndTime);

                switch (scheduleData.Type)
                {
                    case ScheduleType.EventRecording:
                        g.FillRectangle(_eventBrushes, ScheduleX(startTime), 0, ScheduleWidth(startTime, endTime), 30);
                        break;

                    case ScheduleType.Continuous:
                        g.FillRectangle(_recordBrushes, ScheduleX(startTime), 0, ScheduleWidth(startTime, endTime), 30);
                        break;

                    case ScheduleType.EventHandlering:
                        g.FillRectangle(_eventhandleBrushes, ScheduleX(startTime), 0, ScheduleWidth(startTime, endTime), 30);
                        break;
                }
            }

            if (!IsSelected) return;

            UInt32 x = ScheduleX(SelectStart);
            UInt32 width = ScheduleWidth(SelectStart, SelectEnd);
            g.FillRectangle(_selectionBrushes, x, 0, width, 30);

            if (ScheduleControl.SelectDay != this) return;

            String sTime = ((SelectStart - StartTime) / 3600).ToString("0") + ":" + ((SelectStart - StartTime) % 3600 / 60).ToString("00");

            SizeF sSize = g.MeasureString(sTime, Manager.Font);
            if (width > sSize.Width)
            {
                g.DrawString(sTime, Manager.Font, Brushes.Black, x, 7);

                String eTime = ((SelectEnd - StartTime) / 3600).ToString("0") + ":" + ((SelectEnd - StartTime) % 3600 / 60).ToString("00");

                SizeF eSize = g.MeasureString(eTime, Manager.Font);

                if (width > sSize.Width + eSize.Width + 5)
                {
                    g.DrawString(eTime, Manager.Font, Brushes.Black, x + width - eSize.Width, 7);

                    //String dTime = ((SelectEnd - SelectStart) / 3600).ToString("(0") + ":" + ((SelectEnd - SelectStart) % 3600 / 60).ToString("00)");
                    //SizeF dSize = g.MeasureString(dTime, _font);

                    //if (width > sSize.Width + eSize.Width + dSize.Width + 20)
                    //{
                    //    g.DrawString(dTime, _font, Brushes.Black, x + (width - dSize.Width) / 2, 7);
                    //}
                }
            }
        }

        public UInt32 SelectStart;
        public UInt32 SelectEnd;
        public void SelectionRange(UInt32 startTime, UInt32 endTime)
        {
            SelectStart = Math.Min(startTime, endTime) + StartTime;
            SelectEnd = Math.Max(startTime, endTime) + StartTime;

            Invalidate();
        }

        private const UInt16 MiniScale = 1200;//sec
        private const UInt16 MaxScale = 3600;//sec
        public UInt32 ScheduleTime(Int32 x)
        {
            x = Math.Min(Math.Max(x, ScheduleControl.StartPosition), _maxWidth + ScheduleControl.StartPosition);

            return Convert.ToUInt32((x - ScheduleControl.StartPosition) * (86400.0 / _maxWidth));
        }

        public UInt32 AsStart(UInt32 time)
        {
            if (_gap > 8)
            {
                time = (time % MiniScale < 1190)
                    ? Convert.ToUInt32(Math.Floor(time * 1.0 / MiniScale) * MiniScale)
                    : Convert.ToUInt32(Math.Ceiling(time * 1.0 / MiniScale) * MiniScale);
            }
            else
            {
                time = (time % MaxScale < 3550)
                    ? Convert.ToUInt32(Math.Floor(time * 1.0 / MaxScale) * MaxScale)
                    : Convert.ToUInt32(Math.Ceiling(time * 1.0 / MaxScale) * MaxScale);
            }

            return time;
        }

        public UInt32 AsEnd(UInt32 time)
        {
            if (_gap > 8)
            {
                time = (time % MiniScale > 10)
                    ? Convert.ToUInt32(Math.Ceiling(time * 1.0 / MiniScale) * MiniScale)
                    : Convert.ToUInt32(Math.Floor(time * 1.0 / MiniScale) * MiniScale);
            }
            else
            {
                time = (time % MaxScale > 10)
                    ? Convert.ToUInt32(Math.Ceiling(time * 1.0 / MaxScale) * MaxScale)
                    : Convert.ToUInt32(Math.Floor(time * 1.0 / MaxScale) * MaxScale);
            }

            return time;
        }

        private UInt32 ScheduleX(UInt32 startTime)
        {
            return Convert.ToUInt32((startTime - StartTime) * (_maxWidth / 86400.0) + ScheduleControl.StartPosition);
        }

        private UInt32 ScheduleWidth(UInt32 startTime, UInt32 endTime)
        {
            UInt32 width = Convert.ToUInt32((endTime - startTime) * (_maxWidth / 86400.0));
            return (width != 0) ? width + 1 : 0;
        }
    }
}