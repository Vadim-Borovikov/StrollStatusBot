using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetsManager;
using Telegram.Bot;

namespace StrollStatusBot.Users
{
    internal sealed class Manager
    {
        internal Manager(ITelegramBotClient client, Provider googleSheetsProvider, string googleRange)
        {
            _client = client;
            _googleSheetsProvider = googleSheetsProvider;
            _googleRange = googleRange;
            _locker = new object();
        }

        internal void LoadUsers()
        {
            lock (_locker)
            {
                _users = Utils.GetUsers(_googleSheetsProvider, _googleRange);
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
                _users[from.Id].Timestamp = Utils.Now();
                DataManager.UpdateValues(_googleSheetsProvider, _googleRange, _users.Values);
            }

            return _client.SendTextMessageAsync(from.Id, "✅", replyMarkup: Utils.ReplyMarkup);
        }

        private readonly object _locker;
        private readonly ITelegramBotClient _client;
        private readonly Provider _googleSheetsProvider;
        private readonly string _googleRange;
        private Dictionary<int, User> _users;
    }
}
