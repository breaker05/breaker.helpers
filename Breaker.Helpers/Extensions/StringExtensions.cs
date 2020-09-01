using System;
using System.Globalization;
using System.Text;

namespace Breaker.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxChars, string postTruncateString = "...")
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - postTruncateString.Length) + postTruncateString;
        }

        public static string ToUrlFriendly(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Guid.NewGuid().ToString();
            }

            name = name.ToLower(CultureInfo.CurrentCulture);
            name = RemoveDiacritics(name);
            name = name.Replace(" ", "-");
            name = name.Strip(c =>
                c != '-'
                && c != '_'
                && !char.IsLetter(c)
                && !char.IsDigit(c)
                );

            while (name.Contains("--"))
                name = name.Replace("--", "-");

            if (name.Length > 200)
                name = name.Substring(0, 200);

            if (string.IsNullOrWhiteSpace(name))
            {
                return Guid.NewGuid().ToString();
            }

            return name;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string Strip(this string subject, Func<char, bool> predicate)
        {
            var result = new char[subject.Length];

            var cursor = 0;
            for (var i = 0; i < subject.Length; i++)
            {
                char current = subject[i];
                if (!predicate(current))
                {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }
    }
}
