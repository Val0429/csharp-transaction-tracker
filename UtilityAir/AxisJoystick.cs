using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxisJoystickModuleLib;
using Interface;
using Constant;

namespace UtilityAir
{
    partial class UtilityAir
    {
        #region Events
        public event EventHandler<EventArgs<AxisJoystickEvent>> OnAxisJoystickRotate;
        public event EventHandler<EventArgs<AxisJoystickButton>> OnAxisJoystickButtonDown;

        public event EventHandler<EventArgs<AxisJogDialEvent>> OnAxisJogDialRotate;
        public event EventHandler<EventArgs<AxisJogDialEvent>> OnAxisJogDialShuttle;
        public event EventHandler<EventArgs<AxisJogDialButton>> OnAxisJogDialButtonDown;

        public event EventHandler<EventArgs<AxisKeyPadButton>> OnAxisKeyPadButtonDown;
        #endregion Events

        private static readonly AxisJoystickHandlerClass _axisJoystickHandler = new AxisJoystickHandlerClass();
        private static readonly AxisJogDialHandlerClass _axisJogDialHandler = new AxisJogDialHandlerClass();
        private static readonly AxisKeyPadHandlerClass _axisKeyPadHandler = new AxisKeyPadHandlerClass();

        private AxisJoystick _axisJoystick;
        private AxisJogDial _axisJogDial;
        private AxisKeyPad _axisKeyPad;

        #region Initializer
        public void InitializeAxisJoystick()
        {
            // Joystick
            _noEx(() =>
            {
                var joys = _axisJoystickHandler.EnumerateJoysticks();
                if (joys > 0)
                {
                    _axisJoystick = _axisJoystickHandler.GetJoystick(0);
                    if (_axisJoystick != null)
                    {
                        _axisJoystick.ButtonDown += HandlerJoystickButtonDown;
                        _axisJoystick.JoystickMove += HandlerJoystickMove;
                    }
                }
            });

            // JogDial
            _noEx(() =>
            {
                var dials = _axisJogDialHandler.EnumerateJogDials();
                if (dials > 0)
                {
                    _axisJogDial = _axisJogDialHandler.GetJogDial(0);
                    if (_axisJogDial != null)
                    {
                        _axisJogDial.ButtonDown += HandlerJogDialButtonDown;
                        _axisJogDial.JogMove += HandlerJogDialMove;
                        _axisJogDial.ShuttleMove += HandlerJogDialShuttleMove;
                    }
                }
            });

            // KeyPad
            _noEx(() =>
            {
                var pads = _axisKeyPadHandler.EnumerateKeyPads();
                if (pads > 0)
                {
                    _axisKeyPad = _axisKeyPadHandler.GetKeyPad(0);
                    if (_axisKeyPad != null)
                    {
                        _axisKeyPad.ButtonDown += HandlerKeyPadButtonDown;
                    }
                }
            });

            Activate();
        }

        private void Activate()
        {
            _noEx(() => _axisJoystick.Activate());
            _noEx(() => _axisJogDial.Activate());
            _noEx(() => _axisKeyPad.Activate());
        }

        private void DeActivate()
        {
            _noEx(() =>
            {
                _axisJoystick.ButtonDown -= HandlerJoystickButtonDown;
                _axisJoystick.JoystickMove -= HandlerJoystickMove;
                _axisJoystick.DeActivate();
                _axisJoystick.Close();
            });

            _noEx(() =>
            {
                _axisJogDial.ButtonDown -= HandlerJogDialButtonDown;
                _axisJogDial.JogMove -= HandlerJogDialMove;
                _axisJogDial.ShuttleMove -= HandlerJogDialShuttleMove;
                _axisJogDial.DeActivate();
                _axisJogDial.Close();
            });

            _noEx(() =>
            {
                _axisKeyPad.ButtonDown -= HandlerKeyPadButtonDown;
                _axisKeyPad.DeActivate();
                _axisKeyPad.Close();
            });

            _axisJoystick = null;
            _axisJogDial = null;
            _axisKeyPad = null;

            GC.Collect();
        }
        #endregion Initializer


        #region Event Handler
        void HandlerJoystickButtonDown(int joystickId, int buttonIndex)
        {
            //DateTime now = DateTime.Now;
            //Console.WriteLine(@"[{0}] {1},{2}", now.ToString("mm:ss.fff"), joystickId, buttonIndex);
            OnAxisJoystickButtonDown(this, new EventArgs<AxisJoystickButton>((AxisJoystickButton)buttonIndex));
        }

        void HandlerJoystickMove(int joystickId, int xValue, int yValue, int zValue)
        {
            //DateTime now = DateTime.Now;
            //Console.WriteLine(@"[{0}] {1},{2},{3}", now.ToString("mm:ss.fff"), xValue, yValue, zValue);
            OnAxisJoystickRotate(this, new EventArgs<AxisJoystickEvent>(new AxisJoystickEvent(xValue, yValue, zValue)));
        }

        void HandlerJogDialButtonDown(int jogDialId, int buttonIndex)
        {
            //DateTime now = DateTime.Now;
            //Console.WriteLine(@"[{0}] {1},{2}", now.ToString("mm:ss.fff"), jogDialId, buttonIndex);
            OnAxisJogDialButtonDown(this, new EventArgs<AxisJogDialButton>((AxisJogDialButton)buttonIndex));
        }
        void HandlerJogDialMove(int jogDialId, int joggState)
        {
            //DateTime now = DateTime.Now;
            //Console.WriteLine(@"[{0}] {1},{2}", now.ToString("mm:ss.fff"), jogDialId, joggState);
            OnAxisJogDialRotate(this, new EventArgs<AxisJogDialEvent>(new AxisJogDialEvent(joggState)));
        }
        void HandlerJogDialShuttleMove(int jogDialId, int dialState)
        {
            //DateTime now = DateTime.Now;
            //Console.WriteLine(@"[{0}] {1},{2}", now.ToString("mm:ss.fff"), jogDialId, dialState);
            OnAxisJogDialShuttle(this, new EventArgs<AxisJogDialEvent>(new AxisJogDialEvent(dialState)));
        }

        void HandlerKeyPadButtonDown(int keyPadId, int buttonIndex)
        {
            //DateTime now = DateTime.Now;
            //Console.WriteLine(@"[{0}] {1},{2}", now.ToString("mm:ss.fff"), keyPadId, buttonIndex);
            OnAxisKeyPadButtonDown(this, new EventArgs<AxisKeyPadButton>((AxisKeyPadButton)buttonIndex));
        }
        #endregion Event Handler


        #region Utility
        private void _noEx(Action act)
        {
            try
            {
                act.Invoke();
            }
            catch { }
        }
        #endregion Utility
    }

}
