using System;
using System.Collections.Generic;
using GoogleSheetsManager;

namespace StrollStatusBot.Users
{
    internal sealed class User : ILoadable, ISavable
    {
        public int Id;
        public string Status;
        public DateTime? Timestamp;

        public User() { }

        public User(Telegram.Bot.Types.User from)
        {
            Id = from.Id;
            _name = $"{from.FirstName} {from.LastName}";
            _username = from.Username;
        }

        public void Load(IList<object> values)
        {
            Id = values.ToInt(0) ?? throw new ArgumentNullException();

            _name = values.ToString(1);

            _username = values.ToString(2).Remove(0, 1);

            Status = values.ToString(3);

            Timestamp = values.ToDateTime(4);
        }

        public IList<object> Save()
        {
            var uri = new Uri(string.Format(UriFormat, _username));
            string login = string.Format(LoginFormat, _username);

            return new List<object>
            {
                Id,
                _name,
                $"{Utils.GetHyperlink(uri, login)}",
                Status,
                $"{Timestamp:d MMMM yyyy HH:mm:ss}"
            };
        }

        private string _name;
        private string _username;

        private const string UriFormat = "https://t.me/{0}";
        private const string LoginFormat = "@{0}";
    }
}