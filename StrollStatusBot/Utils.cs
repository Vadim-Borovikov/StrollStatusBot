using Telegram.Bot.Types.ReplyMarkups;

namespace StrollStatusBot;

internal static class Utils
{
    public static readonly ReplyKeyboardMarkup Keyboard = SetupKeyboard();

    public static long? ToLong(this object? o)
    {
        if (o is long l)
        {
            return l;
        }
        return long.TryParse(o?.ToString(), out l) ? l : null;
    }

    private static ReplyKeyboardMarkup SetupKeyboard()
    {
        KeyboardButton buttonHome = new("Дома");
        KeyboardButton buttonStroll = new("Гуляю");
        KeyboardButton buttonForAStroll = new("Еду на прогулку");
        KeyboardButton buttonFromAStroll = new("Еду с прогулки");

        KeyboardButton[] raw1 = { buttonStroll, buttonForAStroll };
        KeyboardButton[] raw2 = { buttonHome, buttonFromAStroll };
        KeyboardButton[][] raws = { raw1, raw2 };

        return new ReplyKeyboardMarkup(raws);
    }
}