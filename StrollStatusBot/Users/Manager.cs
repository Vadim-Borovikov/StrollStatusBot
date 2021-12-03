using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheetsManager;
using Telegram.Bot;

namespace StrollStatusBot.Users
{
    internal sealed class Manager
    {
        internal Manager(Bot.Bot bot)
        {
            _bot = bot;
            _locker = new object();
        }

        internal async Task LoadUsersAsync()
        {
            Dictionary<long, User> users = await Utils.GetUsersAsync(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange);
            lock (_locker)
            {
                _users = users;
            }
        }

        internal async Task AddStatus(Telegram.Bot.Types.User from, string text)
        {
            lock (_locker)
            {
                if (!_users.ContainsKey(from.Id))
                {
                    _users[from.Id] = new User(from);
                }
                _users[from.Id].Status = text;
                _users[from.Id].Timestamp = _bot.TimeManager.Now();
            }
            await DataManager.UpdateValuesAsync(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange, _users.Values.ToList());

            await _bot.Client.SendTextMessageAsync(from.Id, "✅", replyMarkup: Utils.ReplyMarkup);
        }

        private readonly object _locker;
        private readonly Bot.Bot _bot;
        private Dictionary<long, User> _users;
    }
}
