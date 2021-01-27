using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoogleSheetsManager;
using StrollStatusBot.Web.Models;

namespace StrollStatusBot.Web
{
    internal static class Utils
    {
        public static void LogException(Exception ex)
        {
            File.AppendAllText(ExceptionsLogPath, $"{ex}{Environment.NewLine}");
        }

        public static Dictionary<int, User> GetUsers(Provider googleSheetsProvider, string googleRange)
        {
            IList<User> users = DataManager.GetValues<User>(googleSheetsProvider, googleRange);
            return users?.ToDictionary(u => u.Id, u => u) ?? new Dictionary<int, User>();
        }

        internal static string GetHyperlink(Uri uri, string text) => string.Format(HyperlinkFormat, uri, text);

        private const string HyperlinkFormat = "=HYPERLINK(\"{0}\";\"{1}\")";

        private const string ExceptionsLogPath = "errors.txt";
    }
}
