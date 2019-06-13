using System;
using System.Collections.Generic;
using Constant;
using Interface;

namespace Device
{
    public class DeviceLayout : Camera, IDeviceLayout
    {
        public String LiveGUID { get; private set; }
        public String PlaybackGUID { get; private set; }

        public Int32 WindowWidth { get; set; }
        public Int32 WindowHeight { get; set; } 

        public Int32 Width
        {
            get
            {
                return LayoutX * WindowWidth;
            } 
        }
        public Int32 Height
        {
            get
            {
                return LayoutY * WindowHeight;
            }
        }

        public UInt16 LayoutX { get; set; }
        public UInt16 LayoutY { get; set; }
        public List<IDevice> Items { get; set; }
        public List<Boolean> Dewarps { get; set; }
        
        public Dictionary<UInt16, ISubLayout> SubLayouts { get; set; }

		public Boolean isImmerVision { get; set; }
		public Boolean isIncludeDevice { get; set; }
		public String DefineLayout { get; set; }
		public MountingType MountingType { get; set; }
		public LensSetting LensSetting { get; set; }

        public DeviceLayout()
        {
            LiveGUID = Guid.NewGuid().ToString();
            PlaybackGUID = Guid.NewGuid().ToString();

            ReadyState = ReadyState.New;

            LayoutX = 1;
            LayoutY = 1;

            Items = new List<IDevice>();
            Dewarps = new List<Boolean>();

            //5 empty sub-layout
            //SubLayouts = new Dictionary<UInt16, ISubLayout> {{1, null}, {2, null}, {3, null}, {4, null}, {5, null}};
			SubLayouts = new Dictionary<UInt16, ISubLayout>();

			isImmerVision = false;
	        isIncludeDevice = false;
	        DefineLayout = "*";
			MountingType = MountingType.Ceiling;
			LensSetting = LensSetting.PTZ;
        }
    }
}