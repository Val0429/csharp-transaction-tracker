using System;
using System.Drawing;
using Constant;

namespace DeviceConstant
{
    public class Record
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        private EventType _type;
        public EventType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                //event color is different for all event

                if (_type != EventType.VideoRecord)
                {
                    SelectedBrushes = RecordBrushes = UnSelectedBrushes = GetEventColor();
                }
            }
        }

        protected virtual SolidBrush GetEventColor()
        {
            var brush = new SolidBrush(CameraEventSearchCriteria.GetEventTypeDefaultColor(_type));

            return brush;
        }

        protected SolidBrush RecordBrushes = new SolidBrush(Color.FromArgb(66, 68, 77));//Color.FromArgb(200, 161, 160, 141)
        protected SolidBrush UnSelectedBrushes = new SolidBrush(Color.FromArgb(66, 68, 77)); //Color.FromArgb(100, Color.Gray)
        protected SolidBrush SelectedBrushes = new SolidBrush(Color.FromArgb(89, 92, 107)); //Color.FromArgb(100, Color.Gray)


        // Constructor
        public Record()
        {
        }

        public Record(DateTime startTime, DateTime endTime, EventType eventType)
        {
            StartDateTime = startTime;
            EndDateTime = endTime;
            Type = eventType;
        }


        public void Paint(Graphics graphics, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            if (width <= 0) return;

            graphics.FillRectangle(RecordBrushes, x, y, width, height);
        }

        public void PaintSelected(Graphics graphics, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            if (width <= 0) return;

            graphics.FillRectangle(SelectedBrushes, x, y, width, height);
        }

        public void PaintUnSelected(Graphics graphics, Int32 x, Int32 y, Int32 width, Int32 height)
        {
            if (width <= 0) return;

            graphics.FillRectangle(UnSelectedBrushes, x, y, width, height);
        }
    }
}
