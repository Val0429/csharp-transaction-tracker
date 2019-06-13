using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;

namespace SmartSearch
{
    public partial class SmartSearch
    {
        public void PrintImage(Object sender, EventArgs e)
        {
            var devices = new List<ICamera>();
            var printImages = new Dictionary<ICamera, Image>();

            if (VideoWindow.Camera != null)
            {
                devices.Add(VideoWindow.Camera);

                if (VideoWindow.Visible && VideoWindow.Viewer.Visible)
                {//remember call clear before getdata, else will get previous data if clipboard didn't update
                    Clipboard.Clear();
                    VideoWindow.Snapshot(false);//Server.Configure.ImageWithTimestamp, this is use for save/snapshot image, not print
                    IDataObject data = Clipboard.GetDataObject();
                    if (data != null && data.GetDataPresent(DataFormats.Bitmap))
                    {
                        Image image = (Image)data.GetData(DataFormats.Bitmap, true);
                        if (image != null)
                        {
                            printImages.Add(VideoWindow.Camera, image);
                        }
                    }
                }
            }

            App.PrintImage(devices, printImages, DateTimes.ToDateTime(_timecode, Server.Server.TimeZone));
        }
    }
}
