using System.Collections.Generic;
using System.Globalization;
using StrollStatusBot.Web.Models.Commands;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace StrollStatusBot.Web.Models
{
    internal sealed class Bot : IBot
    {
        public TelegramBotClient Client { get; }

        public IEnumerable<Command> Commands => _commands.AsReadOnly();

        public Config.Config Config { get; }
        public UsersManager UsersManager { get; private set; }

        public Bot(IOptions<Config.Config> options)
        {
            Config = options.Value;

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Config.CultureInfoName);

            Utils.SetupTimeZoneInfo(Config.SystemTimeZoneId);
            Utils.SetupReplyMarkup();

            Client = new TelegramBotClient(Config.Token);
        }

        public void Initialize(UsersManager usersManager)
        {
            UsersManager = usersManager;

            var startCommand = new StartCommand(Config.InstructionLines);
            _commands = new List<Command> { startCommand };
        }

        private List<Command> _commands;
    }
}