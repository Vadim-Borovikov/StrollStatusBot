using System;
using System.Collections.Generic;
using GoogleSheetsManager;

namespace StrollStatusBot.Users
{
    internal sealed class User : ILoadable, ISavable
    {
        IList<string> ISavable.Titles => Titles;

        public long Id;
        public string Status;
        public DateTime? Timestamp;

        public User() { }

        public User(Telegram.Bot.Types.User from)
        {
            Id = from.Id;
            _name = $"{from.FirstName} {from.LastName}";
            _username = from.Username;
        }

        public void Load(IDictionary<string, object> valueSet)
        {
            Id = valueSet[IdTitle]?.ToInt() ?? throw new ArgumentNullException();

            _name = valueSet[NameTitle]?.ToString();

            _username = valueSet[UsernameTitle]?.ToString().Remove(0, 1);

            Status = valueSet[StatusTitle]?.ToString();

            Timestamp = valueSet[TimestampTitle]?.ToDateTime();
        }

        public IDictionary<string, object> Save()
        {
            var uri = new Uri(string.Format(UriFormat, _username));
            string login = string.Format(LoginFormat, _username);

            return new Dictionary<string, object>
            {
                { IdTitle, Id },
                { NameTitle, _name },
                { UsernameTitle, $"{Utils.GetHyperlink(uri, login)}" },
                { StatusTitle, Status },
                { TimestampTitle,  $"{Timestamp:d MMMM yyyy HH:mm:ss}" }
            };
        }

        private string _name;
        private string _username;

        private static readonly IList<string> Titles = new List<string>
        {
            IdTitle,
            NameTitle,
            UsernameTitle,
            StatusTitle,
            TimestampTitle
        };

        private const string IdTitle = "Id";
        private const string NameTitle = "Имя";
        private const string UsernameTitle = "Ссылка";
        private const string StatusTitle = "Статус";
        private const string TimestampTitle = "Время";

        private const string UriFormat = "https://t.me/{0}";
        private const string LoginFormat = "@{0}";
    }
}