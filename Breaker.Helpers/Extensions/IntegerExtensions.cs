using System;

namespace Breaker.Helpers.Extensions
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// Returns a FileSize in bytes to the proper display in Mb
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string BytesToString(this int val)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (val == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(val);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(val) * num).ToString() + suf[place];
        }

        public static bool IsInRange(this int checkVal, int value1, int value2)
        {
            // First check to see if the passed in values are in order. If so, then check to see if checkVal is between them
            if (value1 <= value2)
                return checkVal >= value1 && checkVal <= value2;

            // Otherwise invert them and check the checkVal to see if it is between them
            return checkVal >= value2 && checkVal <= value1;
        }
    }
}
