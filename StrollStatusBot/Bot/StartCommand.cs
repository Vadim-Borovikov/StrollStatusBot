using System.Threading.Tasks;
using AbstractBot;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrollStatusBot.Bot
{
    internal sealed class StartCommand : CommandBase<Bot, BotConfig>
    {
        protected override string Name => "start";
        protected override string Description => "инструкция";

        public StartCommand(Bot bot) : base(bot) { }

        public override Task ExecuteAsync(Message message, bool fromChat = false)
        {
            return Bot.Client.SendTextMessageAsync(message.Chat, Bot.GetDescriptionFor(message.From.Id),
                replyMarkup: Utils.ReplyMarkup);
        }
    }
}
