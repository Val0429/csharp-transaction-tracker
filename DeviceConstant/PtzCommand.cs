using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public class PtzCommand
    {
        public PtzCommandCgi Up = new PtzCommandCgi{Name = "Up"};
        public PtzCommandCgi Down = new PtzCommandCgi { Name = "Down" };
        public PtzCommandCgi Left = new PtzCommandCgi { Name = "Left" };
        public PtzCommandCgi Right = new PtzCommandCgi { Name = "Right" };
        public PtzCommandCgi UpRight = new PtzCommandCgi { Name = "UpRight" };
        public PtzCommandCgi DownRight = new PtzCommandCgi { Name = "DownRight" };
        public PtzCommandCgi UpLeft = new PtzCommandCgi { Name = "UpLeft" };
        public PtzCommandCgi DownLeft = new PtzCommandCgi { Name = "DownLeft" };
        public PtzCommandCgi Stop = new PtzCommandCgi { Name = "Stop" };
        public PtzCommandCgi ZoomIn = new PtzCommandCgi { Name = "ZoomIn" };
        public PtzCommandCgi ZoomOut = new PtzCommandCgi { Name = "ZoomOut" };
        public PtzCommandCgi ZoomStop = new PtzCommandCgi { Name = "ZoomStop" };
        public PtzCommandCgi FocusIn = new PtzCommandCgi { Name = "FocusIn" };
        public PtzCommandCgi FocusOut = new PtzCommandCgi { Name = "FocusOut" };
        public PtzCommandCgi FocusStop = new PtzCommandCgi { Name = "FocusStop" };
        public Dictionary<UInt16, PtzCommandCgi> PresetPoints =new Dictionary<ushort, PtzCommandCgi>
                                                                   {
                                                                       {1, new PtzCommandCgi{Name = "1"}},
                                                                       {2, new PtzCommandCgi{Name = "2"}},
                                                                       {3, new PtzCommandCgi{Name = "3"}},
                                                                       {4, new PtzCommandCgi{Name = "4"}},
                                                                       {5, new PtzCommandCgi{Name = "5"}},
                                                                       {6, new PtzCommandCgi{Name = "6"}},
                                                                       {7, new PtzCommandCgi{Name = "7"}},
                                                                       {8, new PtzCommandCgi{Name = "8"}},
                                                                       {9, new PtzCommandCgi{Name = "9"}},
                                                                       {10, new PtzCommandCgi{Name = "10"}}
                                                                   };

        public Dictionary<UInt16, PtzCommandCgi> GotoPresetPoints = new Dictionary<ushort, PtzCommandCgi>
                                                                   {
                                                                       {1, new PtzCommandCgi{Name = "1"}},
                                                                       {2, new PtzCommandCgi{Name = "2"}},
                                                                       {3, new PtzCommandCgi{Name = "3"}},
                                                                       {4, new PtzCommandCgi{Name = "4"}},
                                                                       {5, new PtzCommandCgi{Name = "5"}},
                                                                       {6, new PtzCommandCgi{Name = "6"}},
                                                                       {7, new PtzCommandCgi{Name = "7"}},
                                                                       {8, new PtzCommandCgi{Name = "8"}},
                                                                       {9, new PtzCommandCgi{Name = "9"}},
                                                                       {10, new PtzCommandCgi{Name = "10"}}
                                                                   };
        public Dictionary<UInt16, PtzCommandCgi> DeletePresetPoints = new Dictionary<ushort, PtzCommandCgi>
                                                                   {
                                                                       {1, new PtzCommandCgi{Name = "1"}},
                                                                       {2, new PtzCommandCgi{Name = "2"}},
                                                                       {3, new PtzCommandCgi{Name = "3"}},
                                                                       {4, new PtzCommandCgi{Name = "4"}},
                                                                       {5, new PtzCommandCgi{Name = "5"}},
                                                                       {6, new PtzCommandCgi{Name = "6"}},
                                                                       {7, new PtzCommandCgi{Name = "7"}},
                                                                       {8, new PtzCommandCgi{Name = "8"}},
                                                                       {9, new PtzCommandCgi{Name = "9"}},
                                                                       {10, new PtzCommandCgi{Name = "10"}}
                                                                   };

        
    }

    public class PtzCommandCgi
    {
        public String Name;
        public PtzCommandMethod Method = PtzCommandMethod.Get;
        public String Cgi;
        public String Parameter;
    }

    public enum PtzCommandMethod : ushort
    {
        Get = 0,
        Post = 1
    }

    public static class PtzCommandMethods
    {
        public static PtzCommandMethod ToIndex(String value)
        {
            foreach (KeyValuePair<PtzCommandMethod, String> keyValuePair in List)
            {
                if (String.Equals(value.ToUpper(), keyValuePair.Value.ToUpper()))
                    return keyValuePair.Key;
            }

            return PtzCommandMethod.Get;
        }

        public static String ToString(PtzCommandMethod index)
        {
            foreach (KeyValuePair<PtzCommandMethod, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<PtzCommandMethod, String> List = new Dictionary<PtzCommandMethod, String>
                                                                               {
                                                                                   {PtzCommandMethod.Get, "get"},
                                                                                   {PtzCommandMethod.Post, "post"}
                                                                               };
    }
}
