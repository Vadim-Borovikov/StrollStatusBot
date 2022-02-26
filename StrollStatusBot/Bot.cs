using System.Threading;
using System.Threading.Tasks;
using AbstractBot;
using GryphonUtilities;
using StrollStatusBot.Users;
using Telegram.Bot.Types;

namespace StrollStatusBot;

public sealed class Bot : BotBaseGoogleSheets<Bot, Config>
{
    public Bot(Config config) : base(config) { }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        Commands.Add(new StartCommand(this));
        Utils.SetupReplyMarkup();
        await UsersManager.LoadUsersAsync();
        await base.StartAsync(cancellationToken);
    }

    protected override Task UpdateAsync(Message message, bool fromChat, CommandBase<Bot, Config>? command = null,
        string? payload = null)
    {
        Telegram.Bot.Types.User user = message.From.GetValue(nameof(message.From));
        return command is null
            ? UsersManager.AddStatus(user, message.Text ?? "")
            : command.ExecuteAsync(message, fromChat, payload);
    }

    private Manager UsersManager => _usersManager ??= new Manager(this);
    private Manager? _usersManager;
}