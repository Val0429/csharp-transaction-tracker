using System;
using System.Collections.Generic;
using Constant;
using DeviceConstant;
using Interface;

namespace EMap
{
    partial class MapControl
    {
        private IVideoWindow _activateVideoWindow;
        private Boolean _isZoom;

        private void UtilityOnMoveAxis(Object sender, EventArgs<JoystickEvent> e)
        {
            //Console.WriteLine(String.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}",
            //    e.Value.Up, e.Value.Down, e.Value.Left, e.Value.Right, e.Value.RotateLeft, e.Value.RotateRight, e.Value.Direction));

            if (_activateVideoWindow == null || !_activateVideoWindow.Visible || !_activateVideoWindow.Viewer.Visible) return;
            if (_activateVideoWindow.Camera == null || _activateVideoWindow.Camera.Model == null || !_activateVideoWindow.Camera.Model.IsSupportPTZ) return;
            if (!_activateVideoWindow.Camera.CheckPermission(Permission.OpticalPTZ)) return;
            if (_server == null || !_server.Configure.EnableJoystick) return;

            JoystickEvent joystickEvent = e.Value;
            
            switch (joystickEvent.Direction)
            {
                case Direction.Up:
                    _activateVideoWindow.SendPTZCommand("cmdMoveUp=" + 0 + "," + (UInt16)joystickEvent.Up);
                    break;

                case Direction.Down:
                    _activateVideoWindow.SendPTZCommand("cmdMoveDown=" + 0 + "," + (UInt16)joystickEvent.Down);
                    break;

                case Direction.Right:
                    _activateVideoWindow.SendPTZCommand("cmdMoveRight=" + (UInt16)joystickEvent.Right + "," + 0);
                    break;

                case Direction.RightUp:
                    _activateVideoWindow.SendPTZCommand("cmdMoveUpRight=" + (UInt16)joystickEvent.Right + "," + (UInt16)joystickEvent.Up);
                    break;

                case Direction.RightDown:
                    _activateVideoWindow.SendPTZCommand("cmdMoveDownRight=" + (UInt16)joystickEvent.Right + "," + (UInt16)joystickEvent.Down);
                    break;

                case Direction.Left:
                    _activateVideoWindow.SendPTZCommand("cmdMoveLeft=" + (UInt16)joystickEvent.Left + "," + 0);
                    break;

                case Direction.LeftUp:
                    _activateVideoWindow.SendPTZCommand("cmdMoveUpLeft=" + (UInt16)joystickEvent.Left + "," + (UInt16)joystickEvent.Up);
                    break;

                case Direction.LeftDown:
                    _activateVideoWindow.SendPTZCommand("cmdMoveDownLeft=" + (UInt16)joystickEvent.Left + "," + (UInt16)joystickEvent.Down);
                    break;

                case Direction.None:
                    if(joystickEvent.RotateRight != JoystickSpeed.Stopped)
                    {
                        _activateVideoWindow.SendPTZCommand("cmdZoom=TELE,3");
                        _isZoom = true;
                        break;
                    }
                    if(joystickEvent.RotateLeft != JoystickSpeed.Stopped)
                    {
                        _activateVideoWindow.SendPTZCommand("cmdZoom=WIDE,3");
                        _isZoom = true;
                        break;
                    }
                    if(_isZoom)
                    {
                        _activateVideoWindow.SendPTZCommand("cmdZoom=STOP");
                        _isZoom = false;
                        break;
                    }

                    _activateVideoWindow.SendPTZCommand("cmdMoveStop");
                    break;
            }
        }

        private void UtilityOnClickButton(Object sender, EventArgs<Dictionary<UInt16, Boolean>> e)
        {
            if (_activateVideoWindow == null || !_activateVideoWindow.Visible || !_activateVideoWindow.Viewer.Visible) return;
            if (_activateVideoWindow.Camera == null || _activateVideoWindow.Camera.Model == null || !_activateVideoWindow.Camera.Model.IsSupportPTZ) return;
            if (!_activateVideoWindow.Camera.CheckPermission(Permission.OpticalPTZ)) return;
            if (_server == null || !_server.Configure.EnableJoystick) return;

            Dictionary<UInt16, Boolean> buttons = e.Value;
            //foreach (var obj in buttons)
            //{
            //    if (obj.Value == false) continue;
            //    Console.WriteLine(obj.Key + " " + obj.Value);
            //}

            foreach (KeyValuePair<UInt16, Boolean> button in buttons)
            {
                if (button.Key == 1 && button.Value)
                {
                    _activateVideoWindow.Viewer.PtzMode = PTZMode.Optical;
                    break;
                }

                if (button.Key == 2 && button.Value)
                {
                    _activateVideoWindow.Viewer.PtzMode = PTZMode.Digital;
                    break;
                }
            }
        }
    }
}
