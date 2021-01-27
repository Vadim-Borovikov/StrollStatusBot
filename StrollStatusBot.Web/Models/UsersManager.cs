using System;
using System.Collections.Generic;
using GoogleSheetsManager;

namespace StrollStatusBot.Web.Models
{
    public sealed class UsersManager
    {
        internal UsersManager(Provider googleSheetsProvider, string googleRange)
        {
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

        internal void AddStatus(Telegram.Bot.Types.User from, string text)
        {
            lock (_locker)
            {
                if (!_users.ContainsKey(from.Id))
                {
                    _users[from.Id] = new User(from);
                }
                _users[from.Id].Status = text;
                _users[from.Id].Timestamp = DateTime.Now;
                DataManager.UpdateValues(_googleSheetsProvider, _googleRange, _users.Values);
            }
        }

        private readonly object _locker;
        private readonly Provider _googleSheetsProvider;
        private readonly string _googleRange;
        private Dictionary<int, User> _users;
    }
}
