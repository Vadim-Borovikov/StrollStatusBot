using System.Threading;
using System.Threading.Tasks;
using AbstractBot;
using AbstractBot.Commands;
using AbstractBot.GoogleSheets;
using StrollStatusBot.Users;
using Telegram.Bot.Types;

namespace StrollStatusBot;

public sealed class Bot : BotBaseCustom<Config>, IBotGoogleSheets
{
    public GoogleSheetsComponent GoogleSheetsComponent { get; init; }

    public Bot(Config config) : base(config)
    {
        GoogleSheetsComponent =
            new GoogleSheetsComponent(config, JsonSerializerOptionsProvider.PascalCaseOptions, TimeManager);
        GoogleSheetsComponent.AdditionalConverters[typeof(long)] =
            GoogleSheetsComponent.AdditionalConverters[typeof(long?)] = o => o?.ToLong();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        Utils.SetupReplyMarkup();
        await UsersManager.LoadUsersAsync();
        await base.StartAsync(cancellationToken);
    }

    public void Dispose() => GoogleSheetsComponent.Dispose();

    protected override Task UpdateAsync(Message message, Chat senderChat, CommandBase? command = null,
        string? payload = null)
    {
        return command is null
            ? UsersManager.AddStatus(message, message.Text ?? "")
            : command.ExecuteAsync(message, message.Chat, payload);
    }

    private Manager UsersManager => _usersManager ??= new Manager(this);
    private Manager? _usersManager;
}