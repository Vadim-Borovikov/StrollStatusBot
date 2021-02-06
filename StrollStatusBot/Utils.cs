using System;
using System.Collections.Generic;
using System.Linq;
using GoogleSheetsManager;
using StrollStatusBot.Users;
using Telegram.Bot.Types.ReplyMarkups;

namespace StrollStatusBot
{
    internal static class Utils
    {
        public static void SetupReplyMarkup()
        {
            var buttonHome = new KeyboardButton("Дома");
            var buttonStroll = new KeyboardButton("Гуляю");
            var buttonForAStroll = new KeyboardButton("Еду на прогулку");
            var buttonFromAStroll = new KeyboardButton("Еду с прогулки");

            var raw1 = new[] { buttonStroll, buttonForAStroll };
            var raw2 = new[] { buttonHome, buttonFromAStroll };
            var raws = new[] { raw1, raw2 };

            ReplyMarkup = new ReplyKeyboardMarkup(raws, true);
        }

        public static Dictionary<int, User> GetUsers(Provider googleSheetsProvider, string googleRange)
        {
            IList<User> users = DataManager.GetValues<User>(googleSheetsProvider, googleRange);
            return users?.ToDictionary(u => u.Id, u => u) ?? new Dictionary<int, User>();
        }

        public static string GetHyperlink(Uri uri, string text) => string.Format(HyperlinkFormat, uri, text);

        private const string HyperlinkFormat = "=HYPERLINK(\"{0}\";\"{1}\")";

        public static IReplyMarkup ReplyMarkup { get; private set; }
    }
}
