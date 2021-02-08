using System.Threading.Tasks;
using AbstractBot;
using StrollStatusBot.Users;
using Telegram.Bot.Types;

namespace StrollStatusBot.Bot
{
    public sealed class Bot : BotBaseGoogleSheets<BotConfig>
    {
        public Bot(BotConfig config) : base(config)
        {
            Commands.Add(new StartCommand(this));

            Utils.SetupReplyMarkup();

            _usersManager = new Manager(Client, GoogleSheetsProvider, Config.GoogleRange);
            _usersManager.LoadUsers();
        }

        protected override Task UpdateAsync(Message message, CommandBase<BotConfig> command, bool fromChat = false)
        {
            return command == null
                ? _usersManager.AddStatus(message.From, message.Text)
                : command.ExecuteAsync(message);
        }

        private readonly Manager _usersManager;
    }
}