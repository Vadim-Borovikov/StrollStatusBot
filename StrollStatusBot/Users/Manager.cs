using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheetsManager;
using Telegram.Bot;

namespace StrollStatusBot.Users;

internal sealed class Manager
{
    internal Manager(Bot bot) => _bot = bot;

    internal async Task LoadUsersAsync()
    {
        SheetData<User> data =
            await DataManager.GetValuesAsync<User>(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange);
        lock (_locker)
        {
            _titles = data.Titles;
            _users = data.Instances.ToDictionary(u => u.Id, u => u);
        }
    }

    internal async Task AddStatus(Telegram.Bot.Types.User from, string text)
    {
        DateTime timestamp = _bot.TimeManager.Now();
        lock (_locker)
        {
            if (_users.ContainsKey(from.Id))
            {
                _users[from.Id].UpdateName(from);
                _users[from.Id].Status = text;
                _users[from.Id].Timestamp = timestamp;
            }
            else
            {
                _users[from.Id] = new User(from, text, timestamp);
            }
        }

        SheetData<User> data = new(_users.Values.ToList(), _titles);
        await DataManager.UpdateValuesAsync(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange, data);

        await _bot.Client.SendTextMessageAsync(from.Id, "✅", replyMarkup: Utils.ReplyMarkup);
    }

    private readonly Bot _bot;
    private IList<string> _titles = Array.Empty<string>();
    private Dictionary<long, User> _users = new();
    private readonly object _locker = new();
}