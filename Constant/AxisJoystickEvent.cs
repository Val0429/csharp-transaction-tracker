using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constant
{
    #region Enumerators

    public class AxisJoystickEvent
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public AxisJoystickEvent(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    };
    public enum AxisJoystickButton
    {
        J1 = 0,
        J2 = 1,
        J3 = 2,
        J4 = 3,
        L = 4,
        R = 5
    };

    public class AxisJogDialEvent
    {
        public int Direction { get; private set; }

        public AxisJogDialEvent(int direction)
        {
            Direction = direction;
        }
    }
    public enum AxisJogDialButton
    {
        L = 1,
        Flag = 2,
        Previous = 3,
        PlayPause = 4,
        Next = 5,
        R = 6
    };

    public enum AxisKeyPadButton
    {
        Num0 = 0,
        Num1 = 1,
        Num2 = 2,
        Num3 = 3,
        Num4 = 4,
        Num5 = 5,
        Num6 = 6,
        Num7 = 7,
        Num8 = 8,
        Num9 = 9,

        Control = 10,
        Alt = 11,

        Layout = 12,
        Record = 13,
        Focus = 14,
        Timer = 15,
        Tool = 16,

        F1 = 17,
        F2 = 18,
        F3 = 19,
        F4 = 20,
        F5 = 21
    };
    #endregion Enumerators
}
