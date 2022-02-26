using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheetsManager;
using GoogleSheetsManager.Providers;
using StrollStatusBot.Users;
using Telegram.Bot.Types.ReplyMarkups;

namespace StrollStatusBot;

internal static class Utils
{
    public static void SetupReplyMarkup()
    {
        KeyboardButton buttonHome = new("Дома");
        KeyboardButton buttonStroll = new("Гуляю");
        KeyboardButton buttonForAStroll = new("Еду на прогулку");
        KeyboardButton buttonFromAStroll = new("Еду с прогулки");

        KeyboardButton[] raw1 = { buttonStroll, buttonForAStroll };
        KeyboardButton[] raw2 = { buttonHome, buttonFromAStroll };
        KeyboardButton[][] raws = { raw1, raw2 };

        ReplyMarkup = new ReplyKeyboardMarkup(raws);
    }

    public static async Task<Dictionary<long, User>> GetUsersAsync(SheetsProvider googleSheetsProvider,
        string googleRange)
    {
        IList<User> users = await DataManager.GetValuesAsync(googleSheetsProvider, User.Load, googleRange);
        return users.ToDictionary(u => u.Id, u => u);
    }

    public static string GetHyperlink(Uri uri, string text) => string.Format(HyperlinkFormat, uri, text);

    private const string HyperlinkFormat = "=HYPERLINK(\"{0}\";\"{1}\")";

    public static IReplyMarkup? ReplyMarkup { get; private set; }
}