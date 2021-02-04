using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrollStatusBot.Web.Models.Commands
{
    internal sealed class StartCommand : Command
    {
        protected override string Name => "start";

        public StartCommand(List<string> instructionLines) => _instructionLines = instructionLines;

        internal override Task ExecuteAsync(ChatId chatId, ITelegramBotClient client)
        {
            var builder = new StringBuilder();
            foreach (string line in _instructionLines)
            {
                builder.AppendLine(line);
            }
            return client.SendTextMessageAsync(chatId, builder.ToString(), replyMarkup: Utils.ReplyMarkup);
        }

        private readonly List<string> _instructionLines;
    }
}
