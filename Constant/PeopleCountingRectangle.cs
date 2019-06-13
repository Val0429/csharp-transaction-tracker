using System;
using System.Drawing;

namespace Constant
{
    public class PeopleCountingRectangle
    {
        public Rectangle Rectangle { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Direction In { get; set; }
        public Direction Out { get; set; }

        public UInt16 PeopleCountingIn { get; set; }
        public UInt16 PeopleCountingOut { get; set; }

        public PeopleCountingRectangle()
        {
            Rectangle = new Rectangle
            {
                Location = new Point(0, 0),
                Size = new Size(100, 100),
            };
            
            StartPoint = new Point(50, 0);
            EndPoint = new Point(50, 100);

            In = Direction.LeftToRight;
            Out = Direction.RightToLeft;

            PeopleCountingIn = 0;
            PeopleCountingOut = 0;
        }
    }
}
