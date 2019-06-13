using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public void SetSubLayoutRegion(List<ISubLayout> subLayouts)
        {
            foreach (var videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null) continue;

                videoWindow.Viewer.SetSubLayoutRegion(subLayouts);
            }
        }

        public string UpdateSubLayoutRegion()
        {
            var str = "";
            foreach (var videoWindow in VideoWindows)
            {
                if (!(videoWindow.Camera is IDeviceLayout)) continue;

                str = videoWindow.Viewer.UpdateSubLayoutRegion();
            }

            return str;
        }
    }
}
