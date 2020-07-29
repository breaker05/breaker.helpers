namespace Breaker.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxChars, string postTruncateString = "...")
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - postTruncateString.Length) + postTruncateString;
        }
    }
}
