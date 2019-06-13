using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Constant
{
    public static class WindowLayouts
    {
        public static Boolean LayoutCompare(WindowLayout layout, WindowLayout layout2)
        {
            return (String.Equals(String.Format(_enus.NumberFormat, "{0:0.##}", layout.X), String.Format(_enus.NumberFormat, "{0:0.##}", layout2.X)) &&
                String.Equals(String.Format(_enus.NumberFormat, "{0:0.##}", layout.Y), String.Format(_enus.NumberFormat, "{0:0.##}", layout2.Y)) &&
                String.Equals(String.Format(_enus.NumberFormat, "{0:0.##}", layout.Width), String.Format(_enus.NumberFormat, "{0:0.##}", layout2.Width)) &&
                String.Equals(String.Format(_enus.NumberFormat, "{0:0.##}", layout.Height), String.Format(_enus.NumberFormat, "{0:0.##}", layout2.Height)));
        }

        public static Boolean LayoutCompare(List<WindowLayout> layout, List<WindowLayout> layout2)
        {
            if (layout == null || layout.Count != layout2.Count)
                return false;
            
            for (Int32 index = 0; index < layout.Count; index++ )
            {
                if (!LayoutCompare(layout[index], layout2[index]))
                    return false;
            }

            return true;
        }

        //LayoutGenerate(4);
        public static List<WindowLayout> LayoutGenerate(UInt16 quantity)
        {
            //for 2 and 3, has special layout
            if(quantity == 2)
                return LayoutGenerate("**");
            if (quantity == 3)
                return LayoutGenerate("11,**");

            var average = Convert.ToUInt16(Math.Ceiling(Math.Sqrt(quantity)));

            return LayoutGenerate(average, average);
        }

        //LayoutGenerate(2, 2);
        public static List<WindowLayout> LayoutGenerate(UInt16 x, UInt16 y)
        {
            String xTemp = new String('*', x);

            String[] stringArray = new String[y];

            for (Int32 i = 0; i < y; i++)
            {
                stringArray[i] = xTemp;
            }

            return LayoutGenerate(stringArray);
        }

        //LayoutGenerate("11*,11*,***");
        public static List<WindowLayout> LayoutGenerate(String value)
        {
            return LayoutGenerate(value.Split(','));
        }

        //LayoutGenerate(new String[]{"11*", "11*", "***"});
        public static List<WindowLayout> LayoutGenerate(String[] array)
        {
            Int32 layoutXGrid = array[0].Length;
            Int32 layoutYGrid = array.Length;
            var layoutArray = new List<Double[]>();

            for (Int32 _iY = 0; _iY < layoutYGrid; _iY++)
            {
                for (Int32 _iX = 0; _iX < layoutXGrid; _iX++)
                {

                    if (array[_iY][_iX] == ' ') continue;
                    if (array[_iY][_iX] == '*')
                    {
                        layoutArray.Add(new[] {
                            100.0 * _iX / layoutXGrid,
                            100.0 * _iY / layoutYGrid,
                            100.0 / layoutXGrid,
                            100.0 / layoutYGrid
                        });
                    }
                    else
                    {
                        Int32 xsize = 0;
                        Int32 ysize = 0;

                        char keyChar = array[_iY][_iX];

                        for (Int32 iXtemp = _iX; iXtemp < layoutXGrid; iXtemp++)
                        {
                            if (array[_iY][iXtemp] == keyChar)
                                xsize++;
                            else
                                break;
                        }

                        for (Int32 iYtemp = _iY; iYtemp < layoutYGrid; iYtemp++)
                        {
                            if (array[iYtemp][_iX] == keyChar)
                                ysize++;
                            else
                                break;
                        }

                        for (Int32 iYtemp = _iY; iYtemp < layoutYGrid; iYtemp++)
                        {
                            array[iYtemp] = array[iYtemp].Replace(keyChar, ' ');
                        }

                        layoutArray.Add(new[] {
                            100.0 * _iX / layoutXGrid,
                            100.0 * _iY / layoutYGrid,
                            100.0 * xsize / layoutXGrid,
                            100.0 * ysize / layoutYGrid
                        });

                    }
                }
            }

            return LayoutGenerate(layoutArray);
        }

        //List<Double[]> numbers = new List<Double[]>();
        //numbers.Add(new Double[] { 50, 50, 0, 0});
        //numbers.Add(new Double[] { 50, 50, 50, 50 });
        //LayoutGenerate(numbers);
        public static List<WindowLayout> LayoutGenerate(List<Double[]> array)
        {
            var layout = new List<WindowLayout>();

            foreach (Double[] arr in array)
            {
                layout.Add(new WindowLayout
                {
                    X = arr[0],
                    Y = arr[1],
                    Width = arr[2],
                    Height = arr[3]
                });
            }

            return layout;
        }

        private static readonly CultureInfo _enus = new CultureInfo("en-US");
        public static String LayoutToString(List<WindowLayout> layout)
        {
            if (layout == null) return "";

            var str = new List<String>();
            foreach (WindowLayout windowLayout in layout)
            {
                str.Add("[" + String.Format(_enus.NumberFormat, "{0:0.##}", windowLayout.X) + "," + String.Format(_enus.NumberFormat, "{0:0.##}", windowLayout.Y) + ","
                    + String.Format(_enus.NumberFormat, "{0:0.##}", windowLayout.Width) + "," + String.Format(_enus.NumberFormat, "{0:0.##}", windowLayout.Height) + "]");
            }

            return String.Join(",", str.ToArray());
        }
    }

    public class WindowLayout
    {
        public Double X { get; set; }
        public Double Y { get; set; }
        public Double Width { get; set; }
        public Double Height { get; set; }
    }

    public class WindowMountLayout
    {
        public XmlElement RegionXML { get; set; }
        public Int16 MountType { get; set; } //mountType:     0: WALL, 1: CELLING, 2: GROUND
        public Boolean DewarpEnable { get; set; }
    }

    public class WindowPTZRegionLayout
    {
        public XmlElement RegionXML { get; set; }
        public Int16 Id { get; set; } 
        public String Name { get; set; }

        public override String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + " " + Name;
        }
    }
}
