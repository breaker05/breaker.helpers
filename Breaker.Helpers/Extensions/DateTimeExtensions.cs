using System;

namespace Breaker.Helpers.Extensions
{
    public static class DateTimeExtensions
    {
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
    }
}
