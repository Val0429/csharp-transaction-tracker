using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        private Boolean _isZoom;
        private Boolean _isMove;
        private int _lastx = 0, _lasty = 0, _lastz = 0;

        protected List<int> _keypadCommand = new List<int>();

        #region Axis Event Helper
        protected virtual void UtilityOnAxisKeypadClickButton(Object sender, EventArgs<AxisKeyPadButton> e)
        {
            AxisKeyPadButton button = e.Value;

            switch (button)
            {
                case AxisKeyPadButton.Num0:
                case AxisKeyPadButton.Num1:
                case AxisKeyPadButton.Num2:
                case AxisKeyPadButton.Num3:
                case AxisKeyPadButton.Num4:
                case AxisKeyPadButton.Num5:
                case AxisKeyPadButton.Num6:
                case AxisKeyPadButton.Num7:
                case AxisKeyPadButton.Num8:
                case AxisKeyPadButton.Num9:
                    if (_keypadCommand.Count > 10) return;
                    _keypadCommand.Add((int)button);
                    break;
                case AxisKeyPadButton.Alt:
                    IDevice camera = null;
                    IVideoWindow view = null;

                    do
                    {
                        if (Server is ICMS)
                        {
                            // CMS: Currently 6 digits
                            if (_keypadCommand.Count != 6) break;

                            var nvrid = _keypadCommand[0] * 10 + _keypadCommand[1];
                            var cameraid = _keypadCommand[2] * 10 + _keypadCommand[3];
                            var viewid = _keypadCommand[4] * 10 + _keypadCommand[5];
                            // Check NVR. Try pattern.
                            if (!CMS.NVRManager.NVRs.ContainsKey(Convert.ToUInt16(nvrid))) break;
                            var nvr = CMS.NVRManager.NVRs[Convert.ToUInt16(nvrid)];
                            // Check Camera
                            if (!nvr.Device.Devices.ContainsKey(Convert.ToUInt16(cameraid))) break;
                            camera = nvr.Device.Devices[Convert.ToUInt16(cameraid)];
                            // Check View
                            // 1) copy VideoWindows & sort
                            var clone = VideoWindows.ToList()
                                .Where(v => v.Visible == true)
                                .OrderBy(v => v.Location.Y)
                                .ThenBy(v => v.Location.X)
                                .ToList();
                            // 2) Check view count
                            if (clone.Count < viewid) break;
                            view = clone[viewid - 1];

                        }
                        else if (Server is INVR)
                        {
                            // NVR: Currently 4 digits
                            if (_keypadCommand.Count != 4) break;

                            var cameraid = _keypadCommand[0] * 10 + _keypadCommand[1];
                            var viewid = _keypadCommand[2] * 10 + _keypadCommand[3];
                            // Check Camera
                            if (!Server.Device.Devices.ContainsKey(Convert.ToUInt16(cameraid))) break;
                            camera = Server.Device.Devices[Convert.ToUInt16(cameraid)];
                            // Check View
                            // 1) copy VideoWindows & sort
                            var clone = VideoWindows.ToList()
                                .Where(v => v.Visible == true)
                                .OrderBy(v => v.Location.Y)
                                .ThenBy(v => v.Location.X)
                                .ToList();
                            // 2) Check view count
                            if (clone.Count < viewid) break;
                            view = clone[viewid - 1];
                        }

                    } while (false);

                    if (camera != null && view != null)
                    {
                        Invoke((MethodInvoker)delegate()
                        {
                            var pt = ((Control)view).PointToScreen(new Point(10, 10));
                            DropDevice(camera, pt, new EventArgs<object>(camera));
                        });
                    }

                    _keypadCommand.Clear();
                    break;
            }
        }

        private void UtilityOnAxisJogDialClickButton(Object sender, EventArgs<AxisJogDialButton> e)
        {
            AxisJogDialButton button = e.Value;

            switch (button)
            {
                case AxisJogDialButton.L:
                case AxisJogDialButton.R:
                    var camera = ActivateVideoWindow.Camera;
                    if (camera == null) break;

                    var server = camera.Server;
                    var nvrid = server.Id;
                    var cameraid = Convert.ToUInt16(camera.Id);

                    var min_id = server.Device.Devices.Keys.Min();
                    var max_id = server.Device.Devices.Keys.Max();

                    IDevice newCamera = null;
                    if (button == AxisJogDialButton.L || button == AxisJogDialButton.R)
                    {
                        for (UInt16 i = button == AxisJogDialButton.L ? --cameraid : ++cameraid;
                            i >= min_id && i <= max_id && newCamera == null; )
                        {
                            var devices = Server is ICMS
                                ? CMS.NVRManager.NVRs[nvrid].Device.Devices
                                : NVR.Device.Devices;
                            newCamera = devices.ContainsKey(i) ? devices[i] : null;

                            // move iterator here
                            if (button == AxisJogDialButton.L) --i; else ++i;
                        }
                    }
                    if (newCamera == null) break;

                    Invoke((MethodInvoker)delegate()
                    {
                        var pt = ((Control)ActivateVideoWindow).PointToScreen(new Point(10, 10));
                        DropDevice(newCamera, pt, new EventArgs<object>(newCamera));
                    });
                    break;
            }
        }

        private void UtilityOnAxisClickButton(Object sender, EventArgs<AxisJoystickButton> e)
        {
            // match same behavior between axis and normal
            AxisJoystickButton button = e.Value;
            switch (button)
            {
                case AxisJoystickButton.J1:
                    ActivateVideoWindow.Camera.PresetPointGo = 1;
                    break;
                case AxisJoystickButton.J2:
                    ActivateVideoWindow.Camera.PresetPointGo = 2;
                    break;
                case AxisJoystickButton.J3:
                    ActivateVideoWindow.Camera.PresetPointGo = 3;
                    break;
                case AxisJoystickButton.J4:
                    ActivateVideoWindow.Camera.PresetPointGo = 4;
                    break;
                case AxisJoystickButton.L:
                case AxisJoystickButton.R:
                    var camera = ActivateVideoWindow.Camera;
                    if (camera == null) break;

                    var server = camera.Server;
                    var nvrid = server.Id;
                    var cameraid = Convert.ToUInt16(camera.Id);

                    var min_id = server.Device.Devices.Keys.Min();
                    var max_id = server.Device.Devices.Keys.Max();

                    IDevice newCamera = null;
                    if (button == AxisJoystickButton.L || button == AxisJoystickButton.R)
                    {
                        for (UInt16 i = button == AxisJoystickButton.L ? --cameraid : ++cameraid;
                            i >= min_id && i <= max_id && newCamera == null; )
                        {
                            var devices = Server is ICMS
                                ? CMS.NVRManager.NVRs[nvrid].Device.Devices
                                : NVR.Device.Devices;
                            newCamera = devices.ContainsKey(i) ? devices[i] : null;

                            // move iterator here
                            if (button == AxisJoystickButton.L) --i; else ++i;
                        }
                    }
                    if (newCamera == null) break;

                    Invoke((MethodInvoker)delegate()
                    {
                        var pt = ((Control)ActivateVideoWindow).PointToScreen(new Point(10, 10));
                        DropDevice(newCamera, pt, new EventArgs<object>(newCamera));
                    });
                    break;
            }
        }

        private void UtilityOnAxisMoveAxis(Object sender, EventArgs<AxisJoystickEvent> e)
        {
            // match same behavior between axis and normal
            if (ActivateVideoWindow == null || !ActivateVideoWindow.Visible || !ActivateVideoWindow.Viewer.Visible) return;
            if (ActivateVideoWindow.Camera == null || ActivateVideoWindow.Camera.Model == null || !ActivateVideoWindow.Camera.Model.IsSupportPTZ) return;
            if (!ActivateVideoWindow.Camera.CheckPermission(Permission.OpticalPTZ)) return;
            if (_server == null || !_server.Configure.EnableAxisJoystick) return;

            AxisJoystickEvent @event = e.Value;

            //Console.WriteLine(@"x: {0}, y: {1}, z: {2}", @event.X, @event.Y, @event.Z);

            int x = @event.X;
            int y = @event.Y;
            int z = @event.Z;

            HelperAxisMoveTranslate(ref x, ref y, ref z);

            string[] cmds = HelperAxisCommandFromXYZ(x, y, z);

            foreach (var cmd in cmds)
            {
                ActivateVideoWindow.SendPTZCommand(cmd);
            }
            Console.WriteLine(@"{0}", string.Join(",", cmds));

            //Console.WriteLine(@"{0},{1},{2}", x, y, z);
        }
        private void HelperAxisMoveTranslate(ref int x, ref int y, ref int z)
        {
            const int min_x = -975, max_x = 967;
            const int min_y = -968, max_y = 975;
            const int min_z = -975, max_z = 959;

            var filter = new Func<int, int, int, int>(delegate(int num, int min, int max)
            {
                bool positive = num > 0;
                var target = Math.Abs(num);
                var reference = Math.Abs(positive ? max : min);

                int rtn;
                if (target < reference * 0.15) { rtn = 0; }
                else if (target < reference * 0.50) { rtn = 1; }
                else if (target < reference * 0.70) { rtn = 2; }
                else if (target < reference * 0.85) { rtn = 3; }
                else if (target < reference * 0.95) { rtn = 4; }
                else { rtn = 5; }

                return rtn * (positive ? 1 : -1);
            });

            x = filter(x, min_x, max_x);
            y = filter(y, min_y, max_y);
            z = filter(z, min_z, max_z);
        }

        private string[] HelperAxisCommandFromXYZ(int x, int y, int z)
        {
            string[] pt_cmds = {
                "cmdMoveDownRight={0:#;#;0},{1:#;#;0}", "cmdMoveUpRight={0:#;#;0},{1:#;#;0}", "cmdMoveDownLeft={0:#;#;0},{1:#;#;0}", "cmdMoveUpleft={0:#;#;0},{1:#;#;0}",
                "cmdMoveUp={0:#;#;0},{1:#;#;0}", "cmdMoveDown={0:#;#;0},{1:#;#;0}", "cmdMoveLeft={0:#;#;0},{1:#;#;0}", "cmdMoveRight={0:#;#;0},{1:#;#;0}"
            };
            string[] z_cmds = { "cmdZoom=TELE,{0:#;#;0}", "cmdZoom=WIDE,{0:#;#;0}" };
            string[] s_cmd = { "cmdMoveStop", "cmdZoom=STOP" };

            var result = new List<string>();

            // Detect PT
            if (_lastx == x && _lasty == y)
            {
                // Don't handle redundant command
            }
            else
            {
                if (x == 0 && y == 0)
                {
                    if (_isMove) result.Add(s_cmd[0]);
                    _isMove = false;
                }
                else
                {
                    _isMove = true;
                }
                if (x > 0 && y > 0) result.Add(string.Format(pt_cmds[0], x, y));
                else if (x > 0 && y < 0) result.Add(string.Format(pt_cmds[1], x, y));
                else if (x < 0 && y > 0) result.Add(string.Format(pt_cmds[2], x, y));
                else if (x < 0 && y < 0) result.Add(string.Format(pt_cmds[3], x, y));
                else if (x == 0 && y < 0) result.Add(string.Format(pt_cmds[4], x, y));
                else if (x == 0 && y > 0) result.Add(string.Format(pt_cmds[5], x, y));
                else if (x < 0 && y == 0) result.Add(string.Format(pt_cmds[6], x, y));
                else if (x > 0 && y == 0) result.Add(string.Format(pt_cmds[7], x, y));

                _lastx = x;
                _lasty = y;
            }

            // Detect Z
            if (_lastz == z)
            {
                // Don't handle redundant z
            }
            else
            {
                if (z > 0)
                {
                    _isZoom = true;
                    result.Add(string.Format(z_cmds[0], z));
                }
                else if (z < 0)
                {
                    _isZoom = true;
                    result.Add(string.Format(z_cmds[1], z));
                }
                else
                {
                    if (_isZoom)
                        result.Add(s_cmd[1]);
                    _isZoom = false;
                }
                _lastz = z;
            }

            return result.ToArray();
        }
        #endregion Axis Event Helper

        private void UtilityOnMoveAxis(Object sender, EventArgs<JoystickEvent> e)
        {
            //Console.WriteLine(String.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}",
            //    e.Value.Up, e.Value.Down, e.Value.Left, e.Value.Right, e.Value.RotateLeft, e.Value.RotateRight, e.Value.Direction));

            if (ActivateVideoWindow == null || !ActivateVideoWindow.Visible || !ActivateVideoWindow.Viewer.Visible) return;
            if (ActivateVideoWindow.Camera == null || ActivateVideoWindow.Camera.Model == null || !ActivateVideoWindow.Camera.Model.IsSupportPTZ) return;
            if (!ActivateVideoWindow.Camera.CheckPermission(Permission.OpticalPTZ)) return;
            if (_server == null || !_server.Configure.EnableJoystick) return;

            JoystickEvent joystickEvent = e.Value;

            switch (joystickEvent.Direction)
            {
                case Direction.Up:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveUp=" + 0 + "," + (UInt16)joystickEvent.Up);
                    break;

                case Direction.Down:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveDown=" + 0 + "," + (UInt16)joystickEvent.Down);
                    break;

                case Direction.Right:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveRight=" + (UInt16)joystickEvent.Right + "," + 0);
                    break;

                case Direction.RightUp:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveUpRight=" + (UInt16)joystickEvent.Right + "," + (UInt16)joystickEvent.Up);
                    break;

                case Direction.RightDown:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveDownRight=" + (UInt16)joystickEvent.Right + "," + (UInt16)joystickEvent.Down);
                    break;

                case Direction.Left:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveLeft=" + (UInt16)joystickEvent.Left + "," + 0);
                    break;

                case Direction.LeftUp:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveUpLeft=" + (UInt16)joystickEvent.Left + "," + (UInt16)joystickEvent.Up);
                    break;

                case Direction.LeftDown:
                    ActivateVideoWindow.SendPTZCommand("cmdMoveDownLeft=" + (UInt16)joystickEvent.Left + "," + (UInt16)joystickEvent.Down);
                    break;

                case Direction.None:
                    if (joystickEvent.RotateRight != JoystickSpeed.Stopped)
                    {
                        ActivateVideoWindow.SendPTZCommand("cmdZoom=TELE,3");
                        _isZoom = true;
                        break;
                    }
                    if (joystickEvent.RotateLeft != JoystickSpeed.Stopped)
                    {
                        ActivateVideoWindow.SendPTZCommand("cmdZoom=WIDE,3");
                        _isZoom = true;
                        break;
                    }
                    if (_isZoom)
                    {
                        ActivateVideoWindow.SendPTZCommand("cmdZoom=STOP");
                        _isZoom = false;
                        break;
                    }

                    ActivateVideoWindow.SendPTZCommand("cmdMoveStop");
                    break;
            }
        }

        private void UtilityOnClickButton(Object sender, EventArgs<Dictionary<UInt16, Boolean>> e)
        {
            if (ActivateVideoWindow == null || !ActivateVideoWindow.Visible || !ActivateVideoWindow.Viewer.Visible) return;
            if (ActivateVideoWindow.Camera == null || ActivateVideoWindow.Camera.Model == null || !ActivateVideoWindow.Camera.Model.IsSupportPTZ) return;
            if (!ActivateVideoWindow.Camera.CheckPermission(Permission.OpticalPTZ)) return;
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
                    ActivateVideoWindow.Viewer.PtzMode = PTZMode.Optical;
                    break;
                }

                if (button.Key == 2 && button.Value)
                {
                    ActivateVideoWindow.Viewer.PtzMode = PTZMode.Digital;
                    break;
                }
            }
        }
    }
}
