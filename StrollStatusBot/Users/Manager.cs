using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetsManager;

namespace StrollStatusBot.Users
{
    internal sealed class Manager
    {
        internal Manager(Bot.Bot bot)
        {
            _bot = bot;
            _locker = new object();
        }

        internal void LoadUsers()
        {
            lock (_locker)
            {
                _users = Utils.GetUsers(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange);
            }
        }

        internal Task AddStatus(Telegram.Bot.Types.User from, string text)
        {
            lock (_locker)
            {
                if (!_users.ContainsKey(from.Id))
                {
                    _users[from.Id] = new User(from);
                }
                _users[from.Id].Status = text;
                _users[from.Id].Timestamp = _bot.TimeManager.Now();
                DataManager.UpdateValues(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange, _users.Values);
            }

            return _bot.Client.SendTextMessageAsync(from.Id, "✅", replyMarkup: Utils.ReplyMarkup);
        }

        private readonly object _locker;
        private readonly Bot.Bot _bot;
        private Dictionary<int, User> _users;
    }
}
