using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AbstractBot;
using AbstractBot.Commands;
using GoogleSheetsManager.Providers;
using StrollStatusBot.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace StrollStatusBot;

public sealed class Bot : BotBaseCustom<Config>, IDisposable
{
    internal readonly SheetsProvider GoogleSheetsProvider;
    internal readonly Dictionary<Type, Func<object?, object?>> AdditionalConverters;

    public Bot(Config config) : base(config)
    {
        GoogleSheetsProvider = new SheetsProvider(config, config.GoogleSheetId);
        AdditionalConverters = new Dictionary<Type, Func<object?, object?>>();
        AdditionalConverters[typeof(long)] = AdditionalConverters[typeof(long?)] = o => o?.ToLong();
        _usersManager = new Manager(this);
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _usersManager.LoadUsersAsync();
        await base.StartAsync(cancellationToken);
    }

    public void Dispose() => GoogleSheetsProvider.Dispose();

    protected override Task UpdateAsync(Message message, Chat senderChat, CommandBase? command = null,
        string? payload = null)
    {
        return command is null
            ? _usersManager.AddStatus(message, message.Text ?? "")
            : command.ExecuteAsync(message, payload);
    }

    protected override IReplyMarkup GetDefaultKeyboard(Chat _) => Utils.Keyboard;

    private readonly Manager _usersManager;
}