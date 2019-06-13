using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public class PresetTours: Dictionary<UInt16, PresetTour>
    {
        public Boolean IsModify;
    }

    public class PresetTour
    {
        public UInt16 Id;
        public String Name;
        public List<TourPoint> Tour = new List<TourPoint>();
        public new String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + " " + Name;
        }

        public String PointToString()
        {
            var tourPoints = new List<String>();
            var index = 0;
            foreach (TourPoint tourPoint in Tour)
            {
                tourPoints.Add(index + ":\"" + tourPoint.Id + "," + tourPoint.Duration + "\"");
                index++;
            }

            return String.Join(",", tourPoints.ToArray());
        }
    }

    public class PresetPoints : Dictionary<UInt16, PresetPoint>
    {
        public Boolean IsModify;
    }

    public class PresetPoint
    {
        public UInt16 Id;
        public String Name;
        public UInt16 Duration;//secend

        public override String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + " " + Name;
        }
    }

    public class TourPoint
    {
        public UInt16 Id;
        public UInt16 Duration;//secend
    }
}
