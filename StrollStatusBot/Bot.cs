using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AbstractBot.Bots;
using GoogleSheetsManager.Documents;
using StrollStatusBot.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace StrollStatusBot;

public sealed class Bot : BotWithSheets<Config>
{
    public Bot(Config config) : base(config)
    {
        GoogleSheetsManager.Documents.Document document = DocumentsManager.GetOrAdd(config.GoogleSheetId);

        Dictionary<Type, Func<object?, object?>> additionalConverters = new();
        additionalConverters[typeof(long)] = additionalConverters[typeof(long?)] = o => o.ToLong();

        Sheet sheet = document.GetOrAddSheet(config.GoogleTitle, additionalConverters);

        _usersManager = new Manager(this, sheet);
        Operations.Add(new UpdateStatusOperation(this, _usersManager));
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _usersManager.LoadUsersAsync();
        await base.StartAsync(cancellationToken);
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

    protected override IReplyMarkup GetDefaultKeyboard(Chat _) => Keyboard;

    private static readonly ReplyKeyboardMarkup Keyboard = SetupKeyboard();

    private readonly Manager _usersManager;
}