using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Constant.Utility
{
    public enum DateTimeUnit { Second, MilliSecond }

    public static class DateTimeConverter
    {
        // Field
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public static bool SystemDateTimeFormatEnabled { get; set; }

        public static ulong ToUtcTimestamp(this DateTime dateTime, DateTimeUnit unit)
        {
            switch (dateTime.Kind)
            {
                case DateTimeKind.Utc:
                    return (ulong)ConvertToTimestamp(dateTime, unit);
                case DateTimeKind.Local:
                case DateTimeKind.Unspecified:
                default:
                    return (ulong)ConvertToTimestamp(dateTime.ToUniversalTime(), unit);
            }
        }
        /// <summary>
        /// Convert DateTime to UNIX timestamp, DateTimeKind specific. (Unit: millisecond)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static ulong ToUtcTimestamp(this DateTime dateTime)
        {
            var timestamp = dateTime.ToUtcTimestamp(DateTimeUnit.MilliSecond);

            return timestamp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <param name="timeZone">Time zone in second.</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static ulong ToUtcTimestamp(this DateTime localDateTime, int timeZone, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            return localDateTime.ToUtcTimestamp(TimeSpan.FromSeconds(timeZone), unit);
        }

        public static ulong ToUtcTimestamp(this DateTime localDateTime, TimeSpan timeZoneOffset, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            var timestamp = ConvertToTimestamp(localDateTime - timeZoneOffset, unit);
            if (timestamp < 0)
            {
                throw new ArgumentOutOfRangeException("localDateTime", localDateTime, "Local time value is before epoch.");
            }
            return (ulong)timestamp;
        }

        private static double ConvertToTimestamp(DateTime utcDateTime, DateTimeUnit unit)
        {
            var timeSpan = (utcDateTime - Epoch);
            return unit == DateTimeUnit.Second ? timeSpan.TotalSeconds : timeSpan.TotalMilliseconds;
        }


        public static DateTime ToUtcTime(this ulong timestamp, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            var dateTime = timestamp.ToDateTime(0, unit);
            var utcTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return utcTime;
        }

        public static DateTime ToLocalTime(this long timestamp, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            if (timestamp < 0) throw new ArgumentOutOfRangeException("timestamp", timestamp, "Timestamp must be greater than 0.");

            return ((ulong)timestamp).ToLocalTime(unit);
        }

        public static DateTime ToLocalTime(this ulong timestamp, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            var utcTime = timestamp.ToUtcTime(unit);

            var localTime = utcTime.ToLocalTime();

            return localTime;
        }

        public static TimeSpan GetTime(this DateTime dateTime)
        {
            var timeSpan = new TimeSpan(0, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);

            return timeSpan;
        }

        public static bool HasSameTime(this DateTime t1, DateTime t2)
        {
            return t1.Year == t2.Year && t1.Month == t2.Month && t1.Day == t2.Day &&
                   t1.Hour == t2.Hour && t1.Minute == t2.Minute && t1.Second == t2.Second;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp">millisecond or second</param>
        /// <param name="timeZone">second</param>
        /// <param name="unit">Specified the date time is millisecond or second.</param>
        /// <returns></returns>
        public static DateTime ToLocalTime(this ulong timestamp, int timeZone, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            var dateTime = timestamp.ToDateTime(timeZone, unit);
            var localTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

            return localTime;
        }

        private static DateTime ToDateTime(this ulong timestamp, int timeZone, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            var timespan = ConvertToTimeSpan(timestamp, unit);
            var dateTime = Epoch.Add(timespan).AddSeconds(timeZone);

            return dateTime;
        }

        private static TimeSpan ConvertToTimeSpan(ulong timestamp, DateTimeUnit unit)
        {
            var timeSpan = unit == DateTimeUnit.MilliSecond
                ? TimeSpan.FromMilliseconds(timestamp)
                : TimeSpan.FromSeconds(timestamp);

            return timeSpan;
        }

        public static string ToLocalTimeString(this long timestamp, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            return timestamp.ToLocalTime(unit).ToDateTimeString();
        }

        public static string ToDateTimeString(this DateTime localTime)
        {
            return SystemDateTimeFormatEnabled ? localTime.ToString(CultureInfo.CurrentCulture) : localTime.ToString("yyyy-MM-dd  HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return SystemDateTimeFormatEnabled ? dateTime.ToShortDateString() : dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return SystemDateTimeFormatEnabled ? dateTime.ToLongTimeString() : dateTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string ToFileDateTimeString(this DateTime localTime)
        {
            if (!SystemDateTimeFormatEnabled)
            {
                return localTime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
            }

            var currentDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            var shortDatePattern = currentDateTimeFormat.ShortDatePattern;
            var dateSeparator = currentDateTimeFormat.DateSeparator[0];
            if (Path.GetInvalidFileNameChars().Contains(dateSeparator))
            {
                shortDatePattern = shortDatePattern.Replace(currentDateTimeFormat.DateSeparator, "-");
            }

            var longTimePattern = currentDateTimeFormat.LongTimePattern.Replace(currentDateTimeFormat.TimeSeparator, "-");

            var fileDateTimePattern = string.Format("{0}-{1}", shortDatePattern, longTimePattern);

            return localTime.ToString(fileDateTimePattern, CultureInfo.InvariantCulture);
        }

        public static string ToFileDateString(this DateTime localTime)
        {
            if (!SystemDateTimeFormatEnabled)
            {
                return localTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            var dateSeparator = CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator[0];
            if (Path.GetInvalidFileNameChars().Contains(dateSeparator))
            {
                shortDatePattern = shortDatePattern.Replace(CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator, "-");
            }

            return localTime.ToString(shortDatePattern, CultureInfo.InvariantCulture);
        }

        public static string ToFileTimeString(this DateTime localTime)
        {
            var longTimeString = localTime.ToLongTimeString();

            return longTimeString.Replace(CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator, "-");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="timezone">timezone in seconds</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string ToLongTimeString(this ulong timestamp, int timezone = 0, DateTimeUnit unit = DateTimeUnit.MilliSecond)
        {
            var localTime = timestamp.ToLocalTime(timezone, unit);

            return SystemDateTimeFormatEnabled ? localTime.ToLongTimeString() : localTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string ToYearMonthString(this DateTime localTime)
        {
            var pattern = GetShortYearMonthPattern();

            return localTime.ToString(pattern, CultureInfo.InvariantCulture);
        }

        public static string GetDatePattern()
        {
            return SystemDateTimeFormatEnabled ? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern : "yyyy-MM-dd";
        }

        public static string GetTimePattern()
        {
            return "HH:mm:ss";
            //return SystemDateTimeFormatEnabled ? CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern : "HH:mm:ss";
        }

        public static string GetLongYearMonthPattern()
        {
            return SystemDateTimeFormatEnabled ? CultureInfo.CurrentCulture.DateTimeFormat.YearMonthPattern : "yyyy-MM";
        }

        public static string GetShortYearMonthPattern()
        {
            if (!SystemDateTimeFormatEnabled) return "yyyy-MM";

            var dateSeparator = CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator;
            var tmp = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Split(new[] { dateSeparator }, StringSplitOptions.RemoveEmptyEntries);
            var pattern = tmp.Where(t => !t.Contains("d")).Join(t => t, dateSeparator);
            return pattern;
        }

        public static Size GetDateTimeStringSize(Font font)
        {
            var dateTime = DateTime.Today.AddHours(10).AddMinutes(10).AddSeconds(10); //new DateTime(2015, 12, 12, 10, 10, 10);
            var dateTimeString = dateTime.ToDateTimeString();
            var textSize = TextRenderer.MeasureText(dateTimeString, font);

            return textSize;
        }

        public static TimeSpan GetTimeZone()
        {
            var now = DateTime.Now;
            var utcNow = now.ToUniversalTime();

            return now - utcNow;
        }
    }
}
