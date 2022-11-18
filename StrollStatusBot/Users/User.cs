using GoogleSheetsManager;
using System;
using GryphonUtilities;
using JetBrains.Annotations;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace StrollStatusBot.Users;

internal sealed class User
{
    [SheetField]
    [UsedImplicitly]
    public long Id;

    [SheetField("Имя")]
    [UsedImplicitly]
    public string? Name;

    [SheetField("Ссылка")]
    [UsedImplicitly]
    public string? Username
    {
        get
        {
            Uri uri = new(string.Format(UriFormat, _username));
            string login = string.Format(LoginFormat, _username);

            return $"{GoogleSheetsManager.Utils.GetHyperlink(uri, login)}";
        }

        set => _username = value?.Remove(0, 1);
    }

    [SheetField("Статус")]
    [UsedImplicitly]
    public string Status = null!;

    [SheetField("Время", "{0:d MMMM yyyy HH:mm:ss}")]
    [UsedImplicitly]
    public DateTimeFull Timestamp;

    public User() { }

    public User(Telegram.Bot.Types.User from, string status, DateTimeFull timestamp)
        : this(from.Id, from.Username, GetName(from), status, timestamp) { }

    private User(long id, string? username, string? name, string status, DateTimeFull timestamp)
    {
        Id = id;
        _username = username;
        Name = name;
        Status = status;
        Timestamp = timestamp;
    }

    public void UpdateName(Telegram.Bot.Types.User from) => Name = GetName(from);

    private static string GetName(Telegram.Bot.Types.User from) => $"{from.FirstName} {from.LastName}";

    private string? _username;

    private const string UriFormat = "https://t.me/{0}";
    private const string LoginFormat = "@{0}";
}