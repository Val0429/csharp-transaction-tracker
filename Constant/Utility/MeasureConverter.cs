using System;

namespace Constant.Utility
{
    public class MeasureConverter
    {
        public static double Km2Mile(double km)
        {
            return km * 0.62137;
        }

        public static double Mile2Km(double mile)
        {
            return mile * 1.6093;
        }
        /// <summary>
        /// Convert knots to km / hour
        /// </summary>
        /// <param name="knot"></param>
        /// <returns></returns>
        public static double Knot2Km(double knot)
        {
            return knot * 1.852;
        }
        /// <summary>
        /// Convert knots to mile / hour
        /// </summary>
        /// <param name="knot"></param>
        /// <returns></returns>
        public static double Knot2Mile(double knot)
        {
            return knot * 1.15077944802;
        }

        public static double Km2Knot(double km)
        {
            return km * 0.539956803455724;
        }

        public static double Mile2Knot(double mile)
        {
            return mile * 0.868976242;
        }

        public static double Celcius2Fahrenheit(double celcius)
        {
            var fahrenheit = celcius * 9 / 5 + 32;

            return fahrenheit;
        }

        public static double Fahrenheit2Celcius(double fahrenheit)
        {
            var celcius = (fahrenheit - 32) * 5 / 9;

            return celcius;
        }


        public static double ConvertToSeconds(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalSeconds;
        }

        public static ulong ConvertToMilliseconds(double seconds)
        {
            return Convert.ToUInt64(TimeSpan.FromSeconds(seconds).TotalMilliseconds);
        }
    }
}
