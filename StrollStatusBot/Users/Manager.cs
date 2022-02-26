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
        Dictionary<long, User> users = await Utils.GetUsersAsync(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange);
        lock (_locker)
        {
            _users = users;
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
        await DataManager.UpdateValuesAsync(_bot.GoogleSheetsProvider, _bot.Config.GoogleRange,
            _users.Values.ToList());

        await _bot.Client.SendTextMessageAsync(from.Id, "✅", replyMarkup: Utils.ReplyMarkup);
    }

    private readonly Bot _bot;
    private Dictionary<long, User> _users = new();
    private readonly object _locker = new();
}