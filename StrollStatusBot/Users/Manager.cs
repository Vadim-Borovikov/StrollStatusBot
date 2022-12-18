using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheetsManager;
using GoogleSheetsManager.Documents;
using GryphonUtilities;
using Telegram.Bot.Types;

namespace StrollStatusBot.Users;

internal sealed class Manager
{
    internal Manager(Bot bot, Sheet sheet)
    {
        _bot = bot;
        _sheet = sheet;
    }

    internal async Task LoadUsersAsync()
    {
        SheetData<User> data = await _sheet.LoadAsync<User>(_bot.Config.GoogleRange);
        lock (_locker)
        {
            _titles = data.Titles;
            _users = data.Instances.ToDictionary(u => u.Id, u => u);
        }
    }

    internal async Task AddStatus(Chat chat, string text)
    {
        DateTimeFull timestamp = _bot.TimeManager.Now();
        lock (_locker)
        {
            if (_users.ContainsKey(chat.Id))
            {
                _users[chat.Id].UpdateName(chat);
                _users[chat.Id].Status = text;
                _users[chat.Id].Timestamp = timestamp;
            }
            else
            {
                _users[chat.Id] = new User(chat, text, timestamp);
            }
        }

        SheetData<User> data = new(_users.Values.ToList(), _titles);
        await _sheet.SaveAsync(_bot.Config.GoogleRange, data);

        await _bot.SendTextMessageAsync(chat, "✅");
    }

    private readonly Bot _bot;
    private readonly Sheet _sheet;
    private IList<string> _titles = Array.Empty<string>();
    private Dictionary<long, User> _users = new();
    private readonly object _locker = new();
}