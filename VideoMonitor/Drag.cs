using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using DeviceConstant;
using Interface;
using PanelBase;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public virtual Boolean CheckDragDataType(Object dragObj)
        {
            return (dragObj is INVR || dragObj is IDeviceGroup || dragObj is IDevice);
        }

        public virtual void DragStop(Point point, EventArgs<Object> e)
        {
            if (!Drag.IsDrop(this, point)) return;

            //in playback, when drag, stoop 1x interval
            if (PlayMode == PlayMode.GotoTimestamp)
            {
                GetTimecodeTimer.Enabled = false;

                if (Server is ICMS)
                {
                    if (Server.Server.Storage.Count <= 0)
                        TopMostMessageBox.Show(Localization["VideoMonitor_SetupStorageFirst"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (e.Value is IDeviceLayout)
            {
                var layout = (IDeviceLayout)e.Value;
                if (layout.isImmerVision)
                    ShowGroup((IDeviceLayout)e.Value);
                else
                    DropDevice((IDevice)e.Value, point, e);
            }
            else if (e.Value is IDevice)
            {
                DropDevice((IDevice)e.Value, point, e);
            }
            else if (e.Value is IDeviceGroup)
            {
                ShowGroup((IDeviceGroup)e.Value);
            }
            else if (e.Value is INVR)
            {
                DropNVR((INVR)e.Value);
            }
        }

        protected virtual bool CanDrop(IDevice device)
        {
            return Devices.ContainsValue(device) || AllowDuplicate;
        }

        private void DropDevice(IDevice device, Point point, EventArgs<Object> e)
        {
            //if (device.DeviceType != DeviceType.GPS)
            if (!AllowDuplicate && Devices.ContainsValue(device)) return;

            IVideoWindow previousActivateWindow = ActivateVideoWindow;
            ActivateVideoWindow = null;

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                //Find Drop, Break loop
                if (ActivateVideoWindow != null)
                    break;

                //Check visible window for drop
                if (!videoWindow.Visible) continue;

                if (Drag.IsDrop(videoWindow as Control, point))
                {
                    var id = VideoWindows.IndexOf(videoWindow) + 1;
                    if (id > MaxConnection) return;

                    if ((App.PageActivated != null) && (App.PageActivated.Name == "Map Tracker"))
                    {

                    }
                    else
                    {
                        if (videoWindow.PlayMode != PlayMode)
                        {
                            videoWindow.PlayMode = PlayMode;
                            videoWindow.Reset();
                            videoWindow.PlayMode = PlayMode;
                        }
                    }

                    videoWindow.SwitchVideoStream(0);
                    videoWindow.DragStop(point, e);
                }
            }

            if (ActivateVideoWindow != null)
            {
                if (previousActivateWindow != null)
                    previousActivateWindow.Active = false;

                if (!Devices.ContainsKey(VideoWindows.IndexOf(ActivateVideoWindow)))
                    Devices.Add(VideoWindows.IndexOf(ActivateVideoWindow), device);
                else
                    Devices[VideoWindows.IndexOf(ActivateVideoWindow)] = device;

                var deviceCount = Devices == null ? 0 : Devices.Count;
                var layoutCount = WindowLayout == null ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);

                //change stream id (if ness) before call ActivateVideoWindow.Activate

                AutoChangeProfileMode();

                ActivateVideoWindow.Activate();
                //it's just drag a device, dont need call set page
                //SetCurrentPage();
                RefreshPageLabel();

                var ptzMode = ActivateVideoWindow.Viewer != null ? ActivateVideoWindow.Viewer.PtzMode : PTZMode.Disable;
                RaiseOnSelectionChange(new EventArgs<ICamera, PTZMode>(ActivateVideoWindow.Camera, ptzMode));

                RaiseOnContentChange(ToArray());

                if (OnViewingDeviceNumberChange != null)
                {
                    OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
                }

                if (OnViewingDeviceListChange != null)
                {
                    OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
                }

                if (Server.Configure.EnableAutoSwitchDecodeIFrame)
                {
                    //When Layout.Count >= 16 , Auto Switch to Decode-I Frame
                    if (count >= Server.Configure.AutoSwitchDecodeIFrameCount)
                        DecodeIframe();
                    else //When Layout.Count < 16 , Auto Switch to autoDrop Frame
                        AutoDropFrame();
                }
                //keep current decode setting
                //else
                //{
                //    AutoDropFrame();
                //}

                if (count > 0)
                {
                    UpdateBitrateTimer.Enabled = true;
                }
                else
                {
                    UpdateBitrateTimer.Enabled = false;

                    if (OnBitrateUsageChange != null)
                    {
                        OnBitrateUsageChange(this, new EventArgs<String>("N/A"));
                    }
                }

                FocusGroup = null;
                ActivateVideoWindow.Active = true;

                _nameLabelText = device.ToString();
                if ((CMS != null || PTS != null) && device.Server != null)
                {
                    _nameLabelText += @" (" + device.Server.Name + @" " + device.Server.Credential.Domain + @")";
                }

                nameLabel.Invalidate();
            }
            else//drop not in windows, nothing changed.
            {
                ActivateVideoWindow = previousActivateWindow;
            }
        }

        protected virtual void DropNVR(INVR nvr)
        {
            //if (nvr.ReadyState == ReadyState.Ready)
            //{
            if (nvr.Device.Groups.Count > 0)
            {
                foreach (KeyValuePair<UInt16, IDeviceGroup> obj in nvr.Device.Groups)
                {
                    if (obj.Value.Items.Count > 0)
                    {
                        ShowGroup(obj.Value);
                        return;
                    }
                }
            }

            if (Server is IPTS) return;
            if (nvr.Device.Devices.Count > 0)
            {
                var sortResult = GetSortedDevice(nvr);
                foreach (IDevice device in sortResult)
                {
                    if (!IsCoolDown)
                    {
                        System.Threading.Thread.Sleep(500);
                        IsCoolDown = true;
                    }

                    if (nvr.Manufacture == "iSAP Failover Server")
                    {
                        if (nvr.FailoverDeviceList != null)
                        {
                            if (nvr.FailoverDeviceList.Contains(device))
                                AppendDevice(device);
                        }
                    }
                    else
                    {
                        AppendDevice(device);
                    }

                }
            }

            //}
        }

        public void DragMove(MouseEventArgs e)
        {
        }
    }
}
