using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheetsManager;
using GryphonUtilities;
using Telegram.Bot.Types;

namespace StrollStatusBot.Users;

internal sealed class Manager
{
    internal Manager(Bot bot) => _bot = bot;

    internal async Task LoadUsersAsync()
    {
        SheetData<User> data = await DataManager<User>.LoadAsync(_bot.GoogleSheetsComponent.GoogleSheetsProvider,
            _bot.Config.GoogleRange, additionalConverters: _bot.GoogleSheetsComponent.AdditionalConverters);
        lock (_locker)
        {
            _titles = data.Titles;
            _users = data.Instances.ToDictionary(u => u.Id, u => u);
        }
    }

    internal Task AddStatus(Message message, string text) => AddStatus(message, message.Chat, text);

    private async Task AddStatus(Message message, Chat chat, string text)
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
        await DataManager<User>.SaveAsync(_bot.GoogleSheetsComponent.GoogleSheetsProvider, _bot.Config.GoogleRange,
            data);

        await _bot.SendTextMessageAsync(message.Chat, "✅", replyMarkup: Utils.ReplyMarkup);
    }

    private readonly Bot _bot;
    private IList<string> _titles = Array.Empty<string>();
    private Dictionary<long, User> _users = new();
    private readonly object _locker = new();
}