using System;
using System.Collections.Generic;
using System.Linq;
using Interface;

namespace ViewerAir
{
    public partial class VideoPlayer
    {
        private Int16 _dewarpType = -1;
        public Int16 DewarpType
        {
            get { return _dewarpType; }
            set { _dewarpType = value; }
        }

        //public void EnablePlaybackSmoothMode(UInt16 mode)
        //{
        //    if (_control != null)
        //    {
        //        _control.EnablePlaybackSmoothMode(mode);
        //    }
        //}

        public void EnableKeepLastFrame(ushort enable)
        {
            //_control.KeepLiveLastFrame(enable);
        }

        public void InitFisheyeLibrary(bool dewarpEnable, short mountType)
        {
            if (_control != null)
            {
                var vendor = Camera.Server.Device.ReadFisheyeVendorByCamera(Camera);
                var dewarpType = Camera.Server.Device.ReadFisheyeDewarpTypeByCamera(Camera);

                _control.InitFisheyeLibrary(vendor, dewarpType, dewarpEnable ? 1 : 0, mountType);
            }
        }

        public void SetSubLayoutRegion(ISubLayout subLayout)
        {
            if (subLayout == null)
            {
                //_control.EnableRegion(0);
                return;
            }

            var regionStr = "<Regions><Area>" + subLayout.X + "," + subLayout.Y + "," + subLayout.Width + "," + subLayout.Height + "</Area></Regions>";

            //_control.EnableRegion(1);
            //_control.SetupRegionStart();
            //_control.SetRegions(regionStr);
        }

        public void SetSubLayoutRegion(List<ISubLayout> subLayouts)
        {
            //_control.EnableRegion(0);

            var regionStr = "<Regions>" + String.Join("", (from subLayout in subLayouts
                                                           where subLayout != null
                                                           select
                                                               String.Format("<Area>{0},{1},{2},{3}</Area>", subLayout.X, subLayout.Y, subLayout.Width, subLayout.Height)).ToArray()) + "</Regions>";

            //_control.EnableRegion(1);
            //_control.SetRegions(regionStr);
            //_control.SetupRegionStart();
        }

        public void UpdateSubLayoutRegion(ISubLayout subLayout)
        {
            //var region = _control.SetupRegionEnd();
            //_control.EnableRegion(0);
            //region = region.Replace("<Regions>", "").Replace("</Regions>", "");
            //region = region.Replace("<Area>", "").Replace("</Area>", "");

            //if(String.IsNullOrEmpty(region))
            //    return;

            //var values = region.Split(',');

            //if(values.Length > 0)
            //    subLayout.X = Math.Max(Convert.ToInt32(values[0]), 0);

            //if(values.Length > 1)
            //    subLayout.Y = Math.Max(Convert.ToInt32(values[1]), 0);

            //if(values.Length > 2)
            //    subLayout.Width = Math.Max(Convert.ToInt32(values[2]), 0);

            //if(values.Length > 3)
            //    subLayout.Height = Math.Max(Convert.ToInt32(values[3]), 0);

            //_control.EnableRegion(0);
        }

        public String UpdateSubLayoutRegion()
        {
            //var str = _control.SetupRegionEnd();
            //_control.SetupRegionStart();

            return null;
        }

    }
}
