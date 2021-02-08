using System.Threading.Tasks;
using AbstractBot;
using Telegram.Bot.Types;

namespace StrollStatusBot.Bot
{
    internal sealed class StartCommand : CommandBase<BotConfig>
    {
        protected override string Name => "start";
        protected override string Description => "инструкция";

        public StartCommand(Bot bot) : base(bot) { }

        public override Task ExecuteAsync(Message message, bool fromChat = false)
        {
            return Bot.Client.SendTextMessageAsync(message.Chat, Bot.GetDescription(), replyMarkup: Utils.ReplyMarkup);
        }
    }
}
