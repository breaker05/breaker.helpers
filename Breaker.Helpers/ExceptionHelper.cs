using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Breaker.Helpers
{
    public static class ExceptionHelper
    {
        private class ExceptionNotification
        {
            readonly string header = "<html><body><div style='width: 100%;'><table cellspacing='1' cellpadding='5'>";
            readonly string footer = "</div></body></html>";
            readonly string rowHeaderTemplate = "<tr><td style='font-family: Consolas, Monospace; font-size: 13px; font-weight: bold; background-color: #666; color: #fff;' colspan='2'>{0}</td></tr>";
            readonly string rowTemplate = "<tr><td valign='top' style='font-family: Consolas, Monospace; font-size: 12px; font-weight: bold; background-color: #ccc; white-space: nowrap; border-top: 1px solid #fff;'>{0}</td><td style='font-family: Consolas, Monospace; font-size: 12px; background-color: #efefef; border-top: 1px solid #fff;'>{1}</td></tr>";
            readonly StringBuilder builder = new StringBuilder();

            public ExceptionNotification()
            {
            }

            public void AddRow(string label, object value)
            {
                if (value == null)
                {
                    builder.AppendFormat(rowTemplate, label, "NULL");
                    return;
                }
                builder.AppendFormat(rowTemplate, label, prepValue(value.ToString()));
            }
            public void AddHeader(string name)
            {
                builder.AppendFormat(rowHeaderTemplate, name);
            }

            public override string ToString()
            {
                return string.Format("{0}{1}{2}",
                    header,
                    builder.ToString(),
                    footer
                );
            }

            private string prepValue(string input)
            {
                if (input == null) return null;
                string pattern = @"(\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$])";
                return Regex.Replace(input, pattern, "<a href='$1'>$1</a>");
            }
        }

        public static string GetDetails(Exception ex, string note)
        {
            var message = new ExceptionNotification();
            message.AddHeader(string.Format("{0} from {1} at {2}", ex.GetType().Name, note, DateTime.Now.ToString()));
            message.AddRow("Message", ex.Message);
            message.AddRow("Stack trace", ex.StackTrace);

            if (ex.InnerException != null)
            {
                message.AddHeader("Inner exception");
                message.AddRow("Inner exception type", ex.InnerException.GetType().Name);
                message.AddRow("Message", ex.InnerException.Message);
                message.AddRow("Stack trace", ex.InnerException.StackTrace);
            }

            return message.ToString();
        }

        //private static string GetDetails(Exception ex, string note)
        //{
        //    HttpContext context = HttpContext.Current;
        //    ExceptionNotification message = new ExceptionNotification();
        //    message.AddHeader(string.Format("{0} from {1} at {2}", ex.GetType().Name, note, DateTime.Now.ToString()));
        //    message.AddRow("Message", ex.Message);
        //    if (context != null) message.AddRow("Requested URL", context.Request.Url.AbsoluteUri);
        //    message.AddRow("Stack trace", ex.StackTrace);

        //    if (context != null && context.Request != null)
        //    {
        //        message.AddHeader("Request Details");

        //        message.AddRow("Handler", context.Request.Url.AbsolutePath);
        //        if (context.Request.UrlReferrer != null) message.AddRow("Referring URL", context.Request.UrlReferrer);
        //        message.AddRow("Client IP", context.Request.UserHostAddress);

        //        if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) message.AddRow("Forwarded IP", context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString());

        //        var vars = context.Request.ServerVariables;
        //        message.AddRow("Host", vars["HTTP_HOST"]);
        //        message.AddRow("Server Name", context.Server.MachineName != null ? context.Server.MachineName : "");
        //        message.AddRow("User", vars["AUTH_USER"]);
        //        message.AddRow("Browser", vars["HTTP_USER_AGENT"]);

        //        var headers = context.Request.Headers;
        //        foreach (string s in headers.Keys)
        //        {
        //            try
        //            {
        //                message.AddRow(s, headers[s]);
        //            }
        //            catch { }
        //        }

        //        var cookies = vars["HTTP_COOKIE"];
        //        if (!string.IsNullOrWhiteSpace(cookies))
        //        {
        //            message.AddHeader("Cookies");
        //            List<string> ignoredCookies = new List<string>() { "__utm", "mp_" };

        //            foreach (var cookie in cookies.Split(';'))
        //            {
        //                if (cookie.Contains("="))
        //                {
        //                    var vals = cookie.Split('=').Select(s => s.Trim()).ToList();
        //                    if (ignoredCookies.Contains(vals[0]) || ignoredCookies.Any(c => vals[0].StartsWith(c))) continue;
        //                    message.AddRow(vals[0], vals[1]);
        //                }
        //                else
        //                {
        //                    message.AddRow("(unnamed)", cookie);
        //                }
        //            }
        //        }


        //    }

        //    if (ex.InnerException != null)
        //    {
        //        message.AddHeader("Inner exception");
        //        message.AddRow("Inner exception type", ex.InnerException.GetType().Name);
        //        message.AddRow("Message", ex.InnerException.Message);
        //        message.AddRow("Stack trace", ex.InnerException.StackTrace);
        //    }

        //    return message.ToString();
        //}

    }
}
