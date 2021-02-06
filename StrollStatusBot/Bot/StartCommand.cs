using System.Threading.Tasks;
using AbstractBot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace StrollStatusBot.Bot
{
    internal sealed class StartCommand : CommandBase
    {
        protected override string Name => "start";
        protected override string Description => "инструкция";

        public StartCommand(IDescriptionProvider descriptionProvider) => _descriptionProvider = descriptionProvider;

        public override Task ExecuteAsync(ChatId chatId, ITelegramBotClient client, int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null)
        {
            return client.SendTextMessageAsync(chatId, _descriptionProvider.GetDescription(),
                replyToMessageId: replyToMessageId, replyMarkup: Utils.ReplyMarkup);
        }

        private readonly IDescriptionProvider _descriptionProvider;
    }
}
