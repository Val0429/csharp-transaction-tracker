using System;
using System.Collections.Generic;
using System.Drawing;

namespace Constant
{
    public class MapAttribute : MapNode
    {
        public String ParentId;
        public Bitmap Image;
        public String OriginalFile;
        public String SystemFile;
        public Int32 Width;
        public Int32 Height;
        public Int32 Scale;
        //public List<CameraAttributes> Cameras;
        public Dictionary<String, CameraAttributes> Cameras;
        //public List<ViaAttributes> Vias;
        public Dictionary<String, ViaAttributes> Vias;
        public Boolean IsDefault;
        //public List<NVRAttributes> NVRs;
        public Dictionary<String, NVRAttributes> NVRs;
        public Double ScaleCenterX;
        public Double ScaleCenterY;
        public Dictionary<String,MapHotZoneAttributes> HotZones;
    }
}
