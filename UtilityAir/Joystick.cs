using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Constant;
using Interface;
using SharpDX.DirectInput;

namespace UtilityAir
{
    public partial class UtilityAir
    {
        public event EventHandler<EventArgs<JoystickEvent>> OnMoveAxis;
        public event EventHandler<EventArgs<Dictionary<UInt16, Boolean>>> OnClickButton;

        private readonly AutoResetEvent _joystickEvent = new AutoResetEvent(true);
        private readonly WaitHandle[] _waitFor = new WaitHandle[1];
        private Joystick _joystick;

        private Int32 _originX;
        private Int32 _originY;
        private Int32 _originZ;

        private readonly Dictionary<UInt16, Boolean> _buttons = new Dictionary<UInt16, Boolean>();
        private readonly Dictionary<UInt16, Boolean> _buttonsTemp = new Dictionary<UInt16, Boolean>();

        public Boolean JoystickEnabled { get; private set; }

        private Thread _detectJoystickThread;
        public void InitializeJoystick()
        {
            if (_detectJoystickThread != null)
            {
                _detectJoystickThread.Abort();
            }

            _detectJoystickThread = new Thread(LoadJoystick);
            _detectJoystickThread.Start();
        }

        //private static readonly Version Win8Version = new Version(6, 2, 9200, 0);
        private void LoadJoystick()
        {
            try
            {
                //if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= Win8Version)
                //{
                //    // its win8 or higher.
                //    //dont detecmine directx input, it TAKE LONF LONG TIME!
                //    return;
                //}

                _waitFor[0] = _joystickEvent;

                var directInput = new DirectInput();

                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly))
                {
                    AcquireJoystick(directInput, deviceInstance.InstanceGuid);
                    break;
                }
            }
            catch (Exception)
            {
            }
        }

        private Thread _joystickThread;

        public void StartJoystickTread()
        {
            if (_joystickThread != null)
            {
                _joystickThread.Abort();
            }

            _joystickThread = new Thread(ListenJoystickEvent);
            _joystickThread.Start();
        }

        public void StopJoystickTread()
        {
            if (_detectJoystickThread != null)
            {
                _detectJoystickThread.Abort();
            }

            if (_joystickThread != null)
            {
                _joystickThread.Abort();
            }
        }

        private void AcquireJoystick(DirectInput directInput, Guid joystickGuid)
        {
            try
            {
                _joystick = new Joystick(directInput, joystickGuid);

                _joystick.SetNotification(_joystickEvent);
                //Set joystick axis ranges.
                //foreach (DeviceObjectInstance doi in _joystick.Objects)
                //{
                //    if ((doi.ObjectId & (int)DeviceObjectTypeFlags.Axis) != 0)
                //    {
                //        _joystick.Properties.SetRange(ParameterHow.ById,doi.ObjectId,new InputRange(-50000, 50000));
                //    }
                //}
                //_joystick.Properties.AxisModeAbsolute = true;
                //_joystick.SetCooperativeLevel(null, CooperativeLevelFlags.NonExclusive | CooperativeLevelFlags.Background);
                _joystick.Acquire();

                //joystick setting
                var jss = new JoystickState();
                _joystick.GetCurrentState(ref jss);

                _originX = jss.X;
                _originY = jss.Y;
                _originZ = jss.Z;

                var jsButtons = jss.Buttons;
                UInt16 i = 1;
                foreach (var button in jsButtons)
                {
                    _buttons.Add(i, button);
                    i++;
                }

                StartJoystickTread();
            }
            catch (Exception)
            {
                StopJoystickTread();
            }
        }

        private void ListenJoystickEvent()
        {
            try
            {
                JoystickEvent joystick = null;

                while (true)
                {
                    Console.WriteLine(DateTime.Now);
                    WaitHandle.WaitAll(_waitFor);
                    var jss = new JoystickState();
                    _joystick.GetCurrentState(ref jss);

                    if (OnMoveAxis != null)
                    {
                        var je = ListenAxis(jss, joystick);
                        if (je != null) joystick = je;
                    }

                    if (OnClickButton != null)
                    {
                        ListenButtons(jss);
                    }
                }
            }
            catch (Exception)
            {
                StopJoystickTread();
            }
        }

        private JoystickEvent ListenAxis(JoystickState joystickState, JoystickEvent joystickevent)
        {
            var upSpeed = JoystickSpeed.Stopped;
            var downSpeed = JoystickSpeed.Stopped;
            var leftSpeed = JoystickSpeed.Stopped;
            var rightSpeed = JoystickSpeed.Stopped;
            var rotateLeftSpeed = JoystickSpeed.Stopped;
            var rotateRightSpeed = JoystickSpeed.Stopped;

            var direct = 0;
            //up,down
            if (joystickState.Y < _originY)
            {
                if (joystickState.Y < _originY - 10000)
                {
                    upSpeed = JoystickSpeed.Fastest;
                    direct += 1;
                }
                else if (joystickState.Y < _originY - 8000)
                {
                    upSpeed = JoystickSpeed.Fast;
                    direct += 1;
                }
                else if (joystickState.Y < _originY - 6000)
                {
                    upSpeed = JoystickSpeed.Medium;
                    direct += 1;
                }
                else if (joystickState.Y < _originY - 4000)
                {
                    upSpeed = JoystickSpeed.Slow;
                    direct += 1;
                }
                else if (joystickState.Y < _originY - 1000)
                {
                    upSpeed = JoystickSpeed.Slowest;
                    direct += 1;
                }
            }
            else
            {
                if (joystickState.Y > _originY + 10000)
                {
                    downSpeed = JoystickSpeed.Fastest;
                    direct += 2;
                }
                else if (joystickState.Y > _originY + 8000)
                {
                    downSpeed = JoystickSpeed.Fast;
                    direct += 2;
                }
                else if (joystickState.Y > _originY + 6000)
                {
                    downSpeed = JoystickSpeed.Medium;
                    direct += 2;
                }
                else if (joystickState.Y > _originY + 4000)
                {
                    downSpeed = JoystickSpeed.Slow;
                    direct += 2;
                }
                else if (joystickState.Y > _originY + 1000)
                {
                    downSpeed = JoystickSpeed.Slowest;
                    direct += 2;
                }
            }

            // left , right
            if (joystickState.X < _originX)
            {
                if (joystickState.X < _originX - 10000)
                {
                    leftSpeed = JoystickSpeed.Fastest;
                    direct += 3;
                }
                else if (joystickState.X < _originX - 8000)
                {
                    leftSpeed = JoystickSpeed.Fast;
                    direct += 3;
                }
                else if (joystickState.X < _originX - 6000)
                {
                    leftSpeed = JoystickSpeed.Medium;
                    direct += 3;
                }
                else if (joystickState.X < _originX - 4000)
                {
                    leftSpeed = JoystickSpeed.Slow;
                    direct += 3;
                }
                else if (joystickState.X < _originX - 1000)
                {
                    leftSpeed = JoystickSpeed.Slowest;
                    direct += 3;
                }
            }
            else
            {
                if (joystickState.X > _originX + 10000)
                {
                    rightSpeed = JoystickSpeed.Fastest;
                    direct += 6;
                }
                else if (joystickState.X > _originX + 8000)
                {
                    rightSpeed = JoystickSpeed.Fast;
                    direct += 6;
                }
                else if (joystickState.X > _originX + 6000)
                {
                    rightSpeed = JoystickSpeed.Medium;
                    direct += 6;
                }
                else if (joystickState.X > _originX + 4000)
                {
                    rightSpeed = JoystickSpeed.Slow;
                    direct += 6;
                }
                else if (joystickState.X > _originX + 1000)
                {
                    rightSpeed = JoystickSpeed.Slowest;
                    direct += 6;
                }
            }

            //rotate right , left
            if (joystickState.Z < _originZ)
            {
                if (joystickState.Z < _originZ - 10000)
                {
                    rotateRightSpeed = JoystickSpeed.Fastest;
                }
                else if (joystickState.Z < _originZ - 8000)
                {
                    rotateRightSpeed = JoystickSpeed.Fast;
                }
                else if (joystickState.Z < _originZ - 6000)
                {
                    rotateRightSpeed = JoystickSpeed.Medium;
                }
                else if (joystickState.Z < _originZ - 4000)
                {
                    rotateRightSpeed = JoystickSpeed.Slow;
                }
                else if (joystickState.Z < _originZ - 1000)
                {
                    rotateRightSpeed = JoystickSpeed.Slowest;
                }
            }
            else
            {
                if (joystickState.Z > _originZ + 10000)
                {
                    rotateLeftSpeed = JoystickSpeed.Fastest;
                }
                else if (joystickState.Z > _originZ + 8000)
                {
                    rotateLeftSpeed = JoystickSpeed.Fast;
                }
                else if (joystickState.Z > _originZ + 6000)
                {
                    rotateLeftSpeed = JoystickSpeed.Medium;
                }
                else if (joystickState.Z > _originZ + 4000)
                {
                    rotateLeftSpeed = JoystickSpeed.Slow;
                }
                else if (joystickState.Z > _originZ + 1000)
                {
                    rotateLeftSpeed = JoystickSpeed.Slowest;
                }
            }



            //Console.WriteLine(jss);
            if (joystickevent == null || joystickevent.Up != upSpeed || joystickevent.Down != downSpeed || joystickevent.Left != leftSpeed || joystickevent.Right != rightSpeed || joystickevent.RotateLeft != rotateLeftSpeed || joystickevent.RotateRight != rotateRightSpeed)
            {
                if (OnMoveAxis != null)
                {
                    joystickevent = new JoystickEvent
                    {
                        Down = downSpeed,
                        Up = upSpeed,
                        Right = rightSpeed,
                        Left = leftSpeed,
                        RotateRight = rotateLeftSpeed,
                        RotateLeft = rotateRightSpeed,
                    };

                    switch (direct)
                    {
                        case 0:
                            joystickevent.Direction = Direction.None;
                            break;

                        case 1:
                            joystickevent.Direction = Direction.Up;
                            break;

                        case 2:
                            joystickevent.Direction = Direction.Down;
                            break;

                        case 3:
                            joystickevent.Direction = Direction.Left;
                            break;

                        case 4:
                            joystickevent.Direction = Direction.LeftUp;
                            break;

                        case 5:
                            joystickevent.Direction = Direction.LeftDown;
                            break;

                        case 6:
                            joystickevent.Direction = Direction.Right;
                            break;

                        case 7:
                            joystickevent.Direction = Direction.RightUp;
                            break;

                        case 8:
                            joystickevent.Direction = Direction.RightDown;
                            break;
                    }
                    OnMoveAxis(this, new EventArgs<JoystickEvent>(joystickevent));

                    return joystickevent;
                }
            }

            return null;
        }

        private void ListenButtons(JoystickState joystickState)
        {
            //buttons manager
            if (OnClickButton == null) return;

            Boolean[] jsButtons = joystickState.Buttons;

            UInt16 i = 1;

            _buttonsTemp.Clear();

            foreach (var button in jsButtons)
            {
                if (_buttons.ContainsKey(i))
                {
                    var check = button;
                    if (_buttons[i] != check)
                    {
                        if (check) _buttonsTemp.Add(i, true);
                        _buttons[i] = check;
                    }
                }
                i++;
            }

            if (_buttonsTemp.Count > 0 && _buttonsTemp.Values.Contains(true))
            {
                OnClickButton(this, new EventArgs<Dictionary<UInt16, Boolean>>((from a in _buttonsTemp select a).Where(d => d.Value).ToDictionary(d => (d.Key), d => d.Value)));
            }
        }

    }
}
