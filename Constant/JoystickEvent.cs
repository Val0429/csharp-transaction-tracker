
namespace Constant
{
    public class JoystickEvent
    {
        public Direction Direction;
        public JoystickSpeed Up;
        public JoystickSpeed Down;
        public JoystickSpeed Left;
        public JoystickSpeed Right;
        public JoystickSpeed RotateRight;
        public JoystickSpeed RotateLeft;
    }

    public enum JoystickSpeed : ushort
    {
        Stopped,
        Slowest,
        Slow,
        Medium,
        Fast,
        Fastest
    }
}
