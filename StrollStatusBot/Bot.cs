using System.Threading;
using System.Threading.Tasks;
using AbstractBot;
using AbstractBot.Commands;
using StrollStatusBot.Users;
using Telegram.Bot.Types;

namespace StrollStatusBot;

public sealed class Bot : BotBaseGoogleSheets<Bot, Config>
{
    public Bot(Config config) : base(config)
    {
        AdditionalConverters[typeof(long)] = AdditionalConverters[typeof(long?)] = o => o?.ToLong();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        Utils.SetupReplyMarkup();
        await UsersManager.LoadUsersAsync();
        await base.StartAsync(cancellationToken);
    }

    protected override Task UpdateAsync(Message message, bool fromChat, CommandBase? command = null,
        string? payload = null)
    {
        return command is null
            ? UsersManager.AddStatus(message, message.Text ?? "")
            : command.ExecuteAsync(message, fromChat, payload);
    }

    private Manager UsersManager => _usersManager ??= new Manager(this);
    private Manager? _usersManager;
}