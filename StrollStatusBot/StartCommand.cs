using System.Threading.Tasks;
using AbstractBot;
using GryphonUtilities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrollStatusBot;

internal sealed class StartCommand : CommandBase<Bot, Config>
{
    protected override string Name => "start";
    protected override string Description => "инструкция";

    public StartCommand(Bot bot) : base(bot) { }

    public override Task ExecuteAsync(Message message, bool fromChat, string? payload)
    {
        User user = message.From.GetValue(nameof(message.From));
        return Bot.Client.SendTextMessageAsync(message.Chat.Id, Bot.GetDescriptionFor(user.Id),
            replyMarkup: Utils.ReplyMarkup);
    }
}