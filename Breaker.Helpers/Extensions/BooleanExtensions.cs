using System;
using System.Collections.Generic;
using System.Text;

namespace Breaker.Helpers.Extensions
{
    public static class BooleanExtensions
    {
        /// <summary>
        /// Returns human readable string of Yes/No for the given boolean
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToHuman(this bool val)
        {
            return val.ToString().Replace("False", "No").Replace("True", "Yes");
        }
    }
}
