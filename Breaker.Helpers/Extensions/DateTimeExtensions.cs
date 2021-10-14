using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Breaker.Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly SortedList<double, Func<TimeSpan, string>> offsets = new SortedList<double, Func<TimeSpan, string>>
       {
            { 0.75, _ => "less than a minute"},
            { 1.5, _ => "a minute"},
            { 45, x => $"{x.TotalMinutes:F0} minutes"},
            { 90, x => "an hour"},
            { 1440, x => $"{x.TotalHours:F0} hours"},
            { 2880, x => "a day"},
            { 43200, x => $"{x.TotalDays:F0} days"},
            { 86400, x => "a month"},
            { 525600, x => $"{x.TotalDays / 30:F0} months"},
            { 1051200, x => "a year"},
            { double.MaxValue, x => $"{x.TotalDays / 365:F0} years"}
       };

        /// <summary>
        /// Returns the distance in time for the date in a clear relative way.
        /// 2 minutes ago
        /// 2 minutes from now
        /// 2 hours ago
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToRelativeDate(this System.DateTime input)
        {
            var x = DateTime.Now - input;
            string Suffix = x.TotalMinutes > 0 ? " ago" : " from now";
            x = new TimeSpan(Math.Abs(x.Ticks));
            return offsets.First(n => x.TotalMinutes < n.Key).Value(x) + Suffix;
        }

        /// <summary>
        /// Calculates the age in years of the current System.DateTime object today.
        /// </summary>
        /// <param name="birthDate">The date of birth</param>
        /// <returns>Age in years today. 0 is returned for a future date of birth.</returns>
        public static int Age(this System.DateTime birthDate)
        {
            return Age(birthDate, DateTime.Today);
        }

        /// <summary>
        /// Calculates the age in years of the current System.DateTime object on a later date.
        /// </summary>
        /// <param name="birthDate">The date of birth</param>
        /// <param name="laterDate">The date on which to calculate the age.</param>
        /// <returns>Age in years on a later day. 0 is returned as minimum.</returns>
        public static int Age(this System.DateTime birthDate, System.DateTime laterDate)
        {
            int age;
            age = laterDate.Year - birthDate.Year;
            if (age > 0)
            {
                age -= Convert.ToInt32(laterDate.Date < birthDate.Date.AddYears(age));
            }
            else
            {
                age = 0;
            }
            return age;
        }

        /// <summary>
        /// Returns whether the DateTime is on a Weekend.
        /// </summary>
        /// <param name="dt">Required. The DateTime to evaluate.</param>
        /// <returns>Returns whether the DateTime is on a Weekend.</returns>
        public static bool IsWeekend(this System.DateTime dt)
        {
            return (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns whether the DateTime is on a Week Day.
        /// </summary>
        /// <param name="dt">Required. The DateTime to evaluate.</param>
        /// <returns>Returns whether the DateTime is on a Week Day.</returns>
        public static bool IsWeekDay(this System.DateTime dt)
        {
            return !dt.IsWeekend();
        }

        /// <summary>
        /// Returns the Date portion of the specified DateTime with the Time set to "23:59:59". The last moment of the day.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.DateTime EndOfDay(this System.DateTime dt)
        {
            //return new System.DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return dt.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Returns the Date portion of the specified DateTime with the Time set to "00:00:00". The first moment of the day.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.DateTime StartOfDay(this System.DateTime dt)
        {
            //return new System.DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            return dt.Date;
        }

        /// <summary>
        /// Returns the Date of the first day in the same Month as the specified DateTime, with the Time set to "00:00:00"
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.DateTime StartOfMonth(this System.DateTime dt)
        {
            return new System.DateTime(dt.Year, dt.Month, 1).StartOfDay();
        }

        /// <summary>
        /// Returns the Date of the last day in the same Month as the specified DateTime, with the Time set to "23:59:59"
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.DateTime EndOfMonth(this System.DateTime dt)
        {
            return dt.AddMonths(1).StartOfMonth().AddDays(-1).EndOfDay();
        }

        /// <summary>
        /// Returns the Date of the first day in the same year as the specified DateTime, with the Time set to "00:00:00"
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1).StartOfDay();
        }

        /// <summary>
        /// Returns the Date of the last day in the same year as the specified DateTime, with the Time set to "23:59:59"
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31).EndOfDay();
        }

        /// <summary>
        /// Returns the start of day for the first day in the week of the specified DateTime.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startDayOfWeek">Optional. The DayOfWeek that is the first day of the week. Default is Sunday.</param>
        /// <returns></returns>
        public static System.DateTime StartOfWeek(this System.DateTime dt, DayOfWeek startDayOfWeek = DayOfWeek.Sunday)
        {
            var start = dt.StartOfDay();

            if (start.DayOfWeek != startDayOfWeek)
            {
                var d = startDayOfWeek - start.DayOfWeek;
                if (startDayOfWeek <= start.DayOfWeek)
                {
                    return start.AddDays(d);
                }
                else
                {
                    return start.AddDays(-7 + d);
                }
            }

            return start;
        }

        /// <summary>
        /// Returns the start of day for the last day in the week of the specified DateTime.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startDayOfWeek">Optional. The DayOfWeek that is the first day of the week. Default is Sunday.</param>
        /// <returns></returns>
        public static System.DateTime EndOfWeek(this System.DateTime dt, DayOfWeek startDayOfWeek = DayOfWeek.Sunday)
        {
            var end = dt.StartOfDay();
            var endDayOfWeek = startDayOfWeek - 1;
            if (endDayOfWeek < 0)
            {
                endDayOfWeek = DayOfWeek.Saturday;
            }

            if (end.DayOfWeek != endDayOfWeek)
            {
                if (endDayOfWeek < end.DayOfWeek)
                {
                    return end.AddDays(7 - (end.DayOfWeek - endDayOfWeek));
                }
                else
                {
                    return end.AddDays(endDayOfWeek - end.DayOfWeek);
                }
            }

            return end;
        }

        /// <summary>
        /// Returns the number of milliseconds from January 1st, 1970 until the specified DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double MillisecondsSince1970(this DateTime dt)
        {
            return dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        /// <summary>
        /// Returns the DateTime of the next week day after the given DateTime.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.DateTime NextWeekDay(this System.DateTime dt)
        {
            var dayOfWeek = dt.DayOfWeek;
            double daysToAdd = 1;

            if (dayOfWeek == DayOfWeek.Friday)
            {
                daysToAdd = 3;
            }
            else if (dayOfWeek == DayOfWeek.Saturday)
            {
                daysToAdd = 2;
            }

            return dt.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns the DateTime of the previous week day before the given DateTime.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.DateTime PreviousWeekDay(this System.DateTime dt)
        {
            var dayOfWeek = dt.DayOfWeek;
            double daysToAdd = -1;

            if (dayOfWeek == DayOfWeek.Monday)
            {
                daysToAdd = -3;
            }
            else if (dayOfWeek == DayOfWeek.Sunday)
            {
                daysToAdd = -2;
            }

            return dt.AddDays(daysToAdd);
        }

        public static string GetCurrentMonthName(this System.DateTime dt)
        {
            return GetMonthName(dt.Month);
        }

        public static string GetMonthName(int month)
        {
            return CultureInfo.CurrentCulture.
                DateTimeFormat.GetMonthName(month);
        }

        /// <summary>
        /// Returns a DateTime from a DateTimeOffset value
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else return dateTime.DateTime;
        }

        /// <summary>
        /// Convert a DateTime to a unix timestamp
        /// </summary>
        /// <param name="MyDateTime">The DateTime object to convert into a Unix Time</param>
        /// <returns></returns>
        public static long DateTimeToUnix(this DateTime MyDateTime)
        {
            var timeSpan = MyDateTime - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// Returns a UTC date as Eastern Standard Time
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToEasternTime(this System.DateTime input)
        {
            TimeZoneInfo zone = TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == "Eastern Standard Time") ?
             TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time") :
             TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

            return TimeZoneInfo.ConvertTimeFromUtc(input, zone);
        }

        public static DateTime ToCentralTime(this System.DateTime input)
        {
            TimeZoneInfo zone = TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == "Central Standard Time") ?
             TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time") :
             TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");

            return TimeZoneInfo.ConvertTimeFromUtc(input, zone);
        }

        public static DateTime ToMountainTime(this System.DateTime input)
        {
            TimeZoneInfo zone = TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == "Mountain Standard Time") ?
             TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time") :
             TimeZoneInfo.FindSystemTimeZoneById("America/Denver");

            return TimeZoneInfo.ConvertTimeFromUtc(input, zone);
        }

        public static DateTime ToPacificTime(this System.DateTime input)
        {
            TimeZoneInfo zone = TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == "Pacific Standard Time") ?
             TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time") :
             TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");

            return TimeZoneInfo.ConvertTimeFromUtc(input, zone);
        }
    }
}
