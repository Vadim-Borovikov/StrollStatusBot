using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrollStatusBot.Web.Models.Commands
{
    public abstract class Command
    {
        protected abstract string Name { get; }

        internal bool IsInvokingBy(Message message) => message.Text == $"/{Name}";

        internal abstract Task ExecuteAsync(ChatId chatId, ITelegramBotClient client);
    }
}
