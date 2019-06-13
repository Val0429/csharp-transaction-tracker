using System;
using System.Collections.Generic;
using System.Globalization;

namespace Constant
{
    public static class DateTimes
    {
        private static DateTime _datetime1970 = new DateTime(1970, 1, 1, 0, 0, 0);
        public static UInt64 UtcNow
        {
            get
            {
                return ((UInt64)(DateTime.UtcNow - _datetime1970).TotalSeconds) * 1000;
            }
        }
        public static UInt64 ToUtc(DateTime dateTime, Int32 timeZone)
        {
            try
            {
                return (UInt64)(dateTime.AddSeconds(-1 * timeZone) - _datetime1970).TotalMilliseconds;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static String ToUtcString(this DateTime dateTime, Int32 timeZone)
        {
            return ToUtc(dateTime, timeZone).ToString(CultureInfo.InvariantCulture);
        }

        //private const Int32 OneWeekSec = 7*24*60*60;
        public static UInt32 ToScheduleTime(DateTime dateTime)
        {
            var sec = 0;

            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    sec = 0;
                    break;

                case DayOfWeek.Tuesday:
                    sec = 1 * 24 * 60 * 60;
                    break;

                case DayOfWeek.Wednesday:
                    sec = 2 * 24 * 60 * 60;
                    break;

                case DayOfWeek.Thursday:
                    sec = 3 * 24 * 60 * 60;
                    break;

                case DayOfWeek.Friday:
                    sec = 4 * 24 * 60 * 60;
                    break;

                case DayOfWeek.Saturday:
                    sec = 5 * 24 * 60 * 60;
                    break;

                case DayOfWeek.Sunday:
                    sec = 6 * 24 * 60 * 60;
                    break;
            }

            sec += dateTime.Hour * 60 * 60;
            sec += dateTime.Minute * 60;
            sec += dateTime.Second;

            //sec -= timezone;

            //if (sec < 0)
            //    return Convert.ToUInt32(OneWeekSec + sec);
            //if (sec > OneWeekSec)
            //    return Convert.ToUInt32(sec - OneWeekSec);

            return Convert.ToUInt32(sec);
        }

        public static DateTime ToDateTime(UInt64 timestamp, Int32 timeZone)
        {
            return _datetime1970.AddMilliseconds(timestamp).AddSeconds(timeZone);
        }

        public static String ToDateTimeString(UInt64 timestamp, Int32 timeZone)
        {
            return ToDateTime(timestamp, timeZone).ToString(CultureInfo.CurrentCulture);
        }

        public static String ToDateTimeString(UInt64 timestamp, Int32 timeZone, String format)
        {
            return ToDateTime(timestamp, timeZone).ToString(format, CultureInfo.CurrentCulture);
        }

        public static List<UInt64[]> SplitUtcDayByDay(UInt64 start, UInt64 end, Int32 timeZone)
        {
            var result = new List<UInt64[]>();
            var gap = start + (86399000 - ((start + (UInt64)timeZone * 1000) % 86400000));

            while (start < end)
            {
                if (gap > end)
                {
                    break;
                }

                result.Add(new[] { start, gap });
                start = gap + 1000;
                gap += 86400000;
            }

            if (result.Count > 0)
            {
                var last = result[result.Count - 1][1];
                if (end > last)
                    result.Add(new[] { last + 1000, end });
            }
            else
            {
                result.Add(new[] { start, end });
            }

            return result;
        }

        public static UInt64[] UpdateStartAndEndDateTime(DateTime today, Int32 timezone, DateTimeSet dateTimeSet)
        {
            var interval = dateTimeSet.ToDateTimeSetInterval(today);

            return new[] { ToUtc(interval[0], timezone), ToUtc(interval[1], timezone) };
        }

        public static DateTime[] ToDateTimeSetInterval(this DateTimeSet set, DateTime today)
        {
            DateTime start = today;
            DateTime end = today;
            switch (set)
            {
                case DateTimeSet.Today:
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24).AddMilliseconds(-1);//AddSeconds(-1);
                    break;

                case DateTimeSet.Yesterday:
                    today = today.AddHours(-24);
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24).AddMilliseconds(-1);//AddSeconds(-1);
                    break;

                case DateTimeSet.DayBeforeYesterday:
                    today = today.AddHours(-48);
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24).AddMilliseconds(-1);//AddSeconds(-1);
                    break;

                case DateTimeSet.ThisWeek:
                    var dayofWeek = today.DayOfWeek;
                    var days = 0;
                    switch (dayofWeek)
                    {
                        case DayOfWeek.Monday:
                            days = 1;
                            break;

                        case DayOfWeek.Tuesday:
                            days = 2;
                            break;

                        case DayOfWeek.Wednesday:
                            days = 3;
                            break;

                        case DayOfWeek.Thursday:
                            days = 4;
                            break;

                        case DayOfWeek.Friday:
                            days = 5;
                            break;

                        case DayOfWeek.Saturday:
                            days = 6;
                            break;
                    }

                    today = today.AddHours(-24 * days);
                    start = new DateTime(today.Year, today.Month, today.Day);
                    end = start.AddHours(24 * 7).AddMilliseconds(-1);//AddSeconds(-1);
                    break;

                case DateTimeSet.ThisMonth:
                    start = new DateTime(today.Year, today.Month, 1);
                    end = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month), 23, 59, 59, 999);
                    break;

                case DateTimeSet.LastMonth:
                    today = today.AddMonths(-1);
                    start = new DateTime(today.Year, today.Month, 1);
                    end = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month), 23, 59, 59, 999);
                    break;

                case DateTimeSet.TheMonthBeforeLast:
                    today = today.AddMonths(-2);
                    start = new DateTime(today.Year, today.Month, 1);
                    end = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month), 23, 59, 59, 999);
                    break;
            }

            return new[] { start, end };
        }

        public static List<UInt64[]> GetTimeDailyPeriods(DateTime now, Int32 timeZone)
        {
            var periods = new List<UInt64[]>();

            var end = now;
            var start = new DateTime(end.Year, end.Month, end.Day);

            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });

            start = start.AddDays(-1);
            end = start.AddDays(1).AddMilliseconds(-1);//AddSeconds(-1);
            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });

            start = start.AddDays(-1);
            end = start.AddDays(1).AddMilliseconds(-1);//AddSeconds(-1);
            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });
            periods.Reverse();

            return periods;
        }

        public static List<UInt64[]> GetTimeWeeklyPeriods(DateTime now, Int32 timeZone)
        {
            var periods = new List<UInt64[]>();

            var end = now;
            //end = end.AddDays(-37);

            var offset = 0;
            switch (end.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    offset = 0;
                    break;

                case DayOfWeek.Monday:
                    offset = -1;
                    break;

                case DayOfWeek.Tuesday:
                    offset = -2;
                    break;

                case DayOfWeek.Wednesday:
                    offset = -3;
                    break;

                case DayOfWeek.Thursday:
                    offset = -4;
                    break;

                case DayOfWeek.Friday:
                    offset = -5;
                    break;

                case DayOfWeek.Saturday:
                    offset = -6;
                    break;
            }
            var start = new DateTime(end.Year, end.Month, end.Day);
            start = start.AddDays(offset);

            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });

            start = start.AddDays(-7);
            end = start.AddDays(7).AddMilliseconds(-1);//AddSeconds(-1);
            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });

            start = start.AddDays(-7);
            end = start.AddDays(7).AddMilliseconds(-1);//AddSeconds(-1);
            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });
            periods.Reverse();

            return periods;
        }

        public static List<UInt64[]> GetTimeMonthlyPeriods(DateTime now, Int32 timeZone)
        {
            var periods = new List<UInt64[]>();

            var end = now;
            var start = new DateTime(end.Year, end.Month, 1);

            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });

            start = start.AddMonths(-1);
            end = start.AddMonths(1).AddMilliseconds(-1);//AddSeconds(-1);
            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });

            start = start.AddMonths(-1);
            end = start.AddMonths(1).AddMilliseconds(-1);//AddSeconds(-1);
            periods.Add(new[] { ToUtc(start, timeZone), ToUtc(end, timeZone) });
            periods.Reverse();

            return periods;
        }
    }

    public class TimeZone
    {
        public String Name;
        public Int32 Value;
    }
}