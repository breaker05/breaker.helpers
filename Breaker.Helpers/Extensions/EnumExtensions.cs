using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Breaker.Helpers.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the [Display(Name="")] attribute of an enum, otherwise falls back to the enum base value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);

            if (attr != null)
                return attr.Name ?? enu.ToString();

            return enu.ToString();
        }

        /// <summary>
        /// Returns the [Display(Description="")] attribute of an enum, otherwise falls back to the enum base value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            if (attr != null)
                return attr.Description ?? enu.ToString();

            return enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            var type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
