using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrollStatusBot.Bot.Commands
{
    internal abstract class Command
    {
        protected abstract string Name { get; }

        internal bool IsInvokingBy(Message message) => message.Text == $"/{Name}";

        internal abstract Task ExecuteAsync(ChatId chatId, ITelegramBotClient client);
    }
}
