using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Constant;
using Device;
using DeviceConstant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        protected IDeviceGroup FocusGroup;

        public virtual void ShowGroup(Object sender, EventArgs<String> e)
        {
            String value = Xml.GetFirstElementValueByTagName(Xml.LoadXml(e.Value), "GROUP");
            var id = Convert.ToUInt16(value);

            if (Server.User.Current.DeviceGroups.ContainsKey(id))
                ShowGroup(Server.User.Current.DeviceGroups[id]);
        }

        public virtual void ShowGroup(Object sender, EventArgs<IDeviceGroup> e)
        {
            if (e.Value == null) return;

            ShowGroup(e.Value);
        }

        public virtual void ShowGroup(IDeviceGroup group)
        {
            if (group == null) return;

            //show group EVEN it have no device inside (or how to delete those group that is invisible?)
            //if (group.Items.Count <= 0) return;

            //append same group, if have muilt page, go to page 1( patrol to page 1)
            if (group == FocusGroup && PageCount >= 1)
            {
                if (PageIndex != 1)
                {
                    PageIndex = 1;
                    SetCurrentPage();
                }
                return;
            }

            if (ActivateVideoWindow != null)
                ActivateVideoWindow.Active = false;

            ActivateVideoWindow = null;

            Devices.Clear();
            for (Int32 i = 0; i < group.View.Count; i++)
            {
                if (group.View[i] == null)
                    continue;

                if (!AllowDuplicate && Devices.ContainsValue(group.View[i]))
                    continue;

                Devices.Add(i, group.View[i]);
                if (Devices.Count >= MaxConnection && PlayMode == PlayMode.GotoTimestamp)
                    break;
            }

            FocusGroup = group;

            ChangeNameLabelText();

            windowsPanel.Visible = false;
            ClearLayout();

            var layout = WindowLayouts.LayoutGenerate(1);
            if (group.Layout != null && group.Layout.Count > 0 && group.Layout.Count <= MaxConnection)
            {
                layout = group.Layout;
            }
            else if (group.Items.Count > 0)
            {
                layout = WindowLayouts.LayoutGenerate((UInt16)Math.Min(group.Items.Count, MaxConnection));
            }
            SetLayout(layout);

            windowsPanel.Visible = true;

            if (OnSelectionChange != null)
            {
                EventArgs<ICamera, PTZMode> eventArgs;
                if (ActivateVideoWindow != null && ActivateVideoWindow.Camera != null)
                {
                    eventArgs = new EventArgs<ICamera, PTZMode>(ActivateVideoWindow.Camera, (ActivateVideoWindow.Viewer != null) ? ActivateVideoWindow.Viewer.PtzMode : PTZMode.Disable);
                }
                else
                {
                    eventArgs = new EventArgs<ICamera, PTZMode>(null, PTZMode.Disable);
                }
                RaiseOnSelectionChange(eventArgs);
            }

            if (group.Regions != null)
            {
                for (Int32 i = 0; i < group.Regions.Count; i++)
                {
                    if (group.Regions[i] == null)
                        continue;

                    if (i > VideoWindows.Count - 1)
                        continue;

                    if (VideoWindows[i] == null)
                        continue;

                    var videoWindow = VideoWindows[i];
                    if (videoWindow.Camera == null)
                        continue;

                    if (videoWindow.Viewer == null)
                        continue;

                    if (group.Regions[i].ParentNode != null)
                    {
                        if (i > group.DewarpEnable.Count - 1)
                            continue;
                        var dewarpEnable = group.DewarpEnable[i];
                        if (dewarpEnable)
                        {
                            var mountType = (short)group.MountType[i];
                            videoWindow.Viewer.InitFisheyeLibrary(true, mountType);
                            videoWindow.Viewer.Dewarp = true;
                            videoWindow.Viewer.DewarpType = mountType;
                            videoWindow.Viewer.SetDigitalPtzRegion(group.Regions[i].ParentNode.InnerXml);
                        }
                    }

                    videoWindow.Viewer.ShowRIPWindow(ActivateVideoWindow == videoWindow);
                }
            }

            RaiseOnContentChange(ToArray());

            if (OnViewingDeviceNumberChange != null)
            {
                var deviceCount = (Devices == null) ? 0 : Devices.Count;
                var layoutCount = (WindowLayout == null) ? 0 : WindowLayout.Count;

                foreach (var popupVideoMonitor in UsingPopupVideoMonitor)
                {
                    deviceCount += popupVideoMonitor.DeviceCount;
                    layoutCount += popupVideoMonitor.LayoutCount;
                }

                var count = Math.Min(layoutCount, deviceCount);
                OnViewingDeviceNumberChange(this, new EventArgs<Int32>(count));
            }

            if (OnViewingDeviceListChange != null)
            {
                OnViewingDeviceListChange(this, new EventArgs<List<IDevice>>(ReadViewDeviceList()));
            }
        }

        public virtual void ShowGroup(Object sender, EventArgs<IDeviceLayout> e)
        {
            if (e.Value == null) return;

            ShowGroup(e.Value);
        }

        public void ShowGroup(IDeviceLayout layout)
        {
            if (layout == null || layout.Items.Count <= 0) return;

            if (ActivateVideoWindow != null)
                ActivateVideoWindow.Active = false;

            ActivateVideoWindow = null;

            Devices.Clear();

            var idx = 0;
            foreach (KeyValuePair<ushort, ISubLayout> keyValuePair in layout.SubLayouts)
            {
                if (keyValuePair.Key == 99)
                {
                    if ((App.PageActivated.Name == "Playback") && (layout.DefineLayout == "11**,11**"))
                        idx--;
                    else
                        Devices.Add(idx, layout.Items[0]);
                }
                else
                    Devices.Add(idx, keyValuePair.Value);
                idx++;
                if (Devices.Count >= MaxConnection && PlayMode == PlayMode.GotoTimestamp)
                    break;
            }

            FocusGroup = new DeviceGroup { Name = String.Format("{0} {1}", layout.Id.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), layout.Name) };

            ChangeNameLabelText();

            windowsPanel.Visible = false;
            ClearLayout();

            if (layout.DefineLayout == "")
            {
                SetLayout(WindowLayouts.LayoutGenerate((UInt16)Math.Min(layout.SubLayouts.Count, MaxConnection)));
            }
            else
            {
                if ((App.PageActivated.Name == "Playback") && (layout.DefineLayout == "11**,11**"))
                    SetLayout(WindowLayouts.LayoutGenerate("**,**"));
                else
                    SetLayout(WindowLayouts.LayoutGenerate(layout.DefineLayout));
            }

            windowsPanel.Visible = true;

            if (ActivateVideoWindow != null)
            {
                if (OnSelectionChange != null)
                    OnSelectionChange(this, new EventArgs<ICamera, PTZMode>(ActivateVideoWindow.Camera, ((ActivateVideoWindow.Viewer != null) ? ActivateVideoWindow.Viewer.PtzMode : PTZMode.Disable)));
            }
            else
            {
                if (OnSelectionChange != null)
                    OnSelectionChange(this, new EventArgs<ICamera, PTZMode>(null, PTZMode.Disable));
            }

            //windowsPanel.Visible = true;    
            RaiseOnContentChange(ToArray());
        }
    }
}
