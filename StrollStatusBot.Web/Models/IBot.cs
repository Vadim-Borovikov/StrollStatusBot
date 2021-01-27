using System.Collections.Generic;
using StrollStatusBot.Web.Models.Commands;
using Telegram.Bot;

namespace StrollStatusBot.Web.Models
{
    public interface IBot
    {
        TelegramBotClient Client { get; }
        IEnumerable<Command> Commands { get; }
        Config.Config Config { get; }
        UsersManager UsersManager { get; }

        void Initialize(UsersManager usersManager);
    }
}