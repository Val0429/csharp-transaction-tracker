using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;

namespace VideoMonitor
{
    public partial class VideoMonitor
    {
        public void PrintImage(Object sender, EventArgs e)
        {
            var devices = new List<ICamera>();
            var printImages = new Dictionary<ICamera, Image>();

            foreach (IVideoWindow videoWindow in VideoWindows)
            {
                if (videoWindow.Camera == null) continue;
                if (printImages.ContainsKey(videoWindow.Camera)) continue;

                devices.Add(videoWindow.Camera);

                if (!videoWindow.Visible || !videoWindow.Viewer.Visible) continue;

                //remember call clear before getdata, else will get previous data if clipboard didn't update
                Clipboard.Clear();
                videoWindow.Snapshot(false);//Server.Configure.ImageWithTimestamp, this is use for save/snapshot image, not print
                IDataObject data = Clipboard.GetDataObject();
                if (data != null && data.GetDataPresent(DataFormats.Bitmap))
                {
                    Image image = (Image)data.GetData(DataFormats.Bitmap, true);
                    if (image != null)
                    {
                        printImages.Add(videoWindow.Camera, image);
                    }
                }
            }

            App.PrintImage(devices, printImages, DateTimes.ToDateTime(Timecode, Server.Server.TimeZone));
        }
    }
}
