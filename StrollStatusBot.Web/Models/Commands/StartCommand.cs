using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrollStatusBot.Web.Models.Commands
{
    internal sealed class StartCommand : Command
    {
        internal override string Name => "start";
        internal override string Description => "���������� � ������ ������";

        public StartCommand(IReadOnlyCollection<Command> commands) => _commands = commands;

        internal override async Task ExecuteAsync(ChatId chatId, int replyToMessageId, ITelegramBotClient client)
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("�������:");
            foreach (Command command in _commands)
            {
                builder.AppendLine($"/{command.Name} � {command.Description}");
            }

            await client.SendTextMessageAsync(chatId, builder.ToString());
        }

        private readonly IReadOnlyCollection<Command> _commands;
    }
}
