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
    }
}
