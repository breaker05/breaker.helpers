using System.Text.RegularExpressions;

namespace Breaker.Helpers
{
    public class PathHelper
    {
        public static string EnsureTrailingSlash(string path, SlashDirection direction = SlashDirection.Back)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;
            string slash = direction == SlashDirection.Back ? "\\" : "/";
            if (path.EndsWith(slash)) return path;
            return string.Format("{0}{1}", path, slash);
        }

        public static string RemoveRoot(string fileWithRoot, string root)
        {
            if (string.IsNullOrWhiteSpace(root)) return fileWithRoot;
            string pattern = string.Format("^{0}", Regex.Escape(root));
            return Regex.Replace(fileWithRoot, pattern, string.Empty);
        }

        public static string SetSlashDirection(string path, SlashDirection direction)
        {
            if (string.IsNullOrWhiteSpace(path)) return path;

            string pattern = null;
            string replacement = null;
            if (direction == SlashDirection.Back)
            {
                pattern = "/";
                replacement = "\\";
            }
            else
            {
                pattern = @"\\";
                replacement = "/";
            }
            return Regex.Replace(path, pattern, replacement);
        }
    }

    public enum SlashDirection
    {
        Forward,
        Back
    }
}
