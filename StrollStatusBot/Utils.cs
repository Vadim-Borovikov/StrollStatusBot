using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheetsManager;
using GoogleSheetsManager.Providers;
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

            ReplyMarkup = new ReplyKeyboardMarkup(raws);
        }

        public static async Task<Dictionary<long, User>> GetUsersAsync(SheetsProvider googleSheetsProvider, string googleRange)
        {
            IList<User> users = await DataManager.GetValuesAsync<User>(googleSheetsProvider, googleRange);
            return users?.ToDictionary(u => u.Id, u => u) ?? new Dictionary<long, User>();
        }

        public static string GetHyperlink(Uri uri, string text) => string.Format(HyperlinkFormat, uri, text);

        private const string HyperlinkFormat = "=HYPERLINK(\"{0}\";\"{1}\")";

        public static IReplyMarkup ReplyMarkup { get; private set; }
    }
}
