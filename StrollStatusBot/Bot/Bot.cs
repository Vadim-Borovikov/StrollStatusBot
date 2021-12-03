using System.Threading;
using System.Threading.Tasks;
using AbstractBot;
using StrollStatusBot.Users;
using Telegram.Bot.Types;

namespace StrollStatusBot.Bot
{
    public sealed class Bot : BotBaseGoogleSheets<Bot, BotConfig>
    {
        public Bot(BotConfig config) : base(config)
        {
            Commands.Add(new StartCommand(this));

            Utils.SetupReplyMarkup();

            _usersManager = new Manager(this);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _usersManager.LoadUsersAsync();
            await base.StartAsync(cancellationToken);
        }

        protected override Task UpdateAsync(Message message, CommandBase<Bot, BotConfig> command,
            bool fromChat = false)
        {
            return command == null
                ? _usersManager.AddStatus(message.From, message.Text)
                : command.ExecuteAsync(message);
        }

        private readonly Manager _usersManager;
    }
}