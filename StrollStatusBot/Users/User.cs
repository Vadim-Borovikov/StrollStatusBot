using GoogleSheetsManager;
using System;
using GryphonUtilities;
using JetBrains.Annotations;
using Telegram.Bot.Types;
using GoogleSheetsManager.Extensions;

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
            if (string.IsNullOrWhiteSpace(_username))
            {
                return null;
            }

            Uri uri = new(string.Format(UriFormat, _username));
            string login = string.Format(LoginFormat, _username);

            return uri.ToHyperlink(login);
        }

        set => _username = string.IsNullOrWhiteSpace(value) ? null : value.Remove(0, 1);
    }

    [SheetField("Статус")]
    [UsedImplicitly]
    public string Status = null!;

    [SheetField("Время", "{0:d MMMM yyyy HH:mm:ss}")]
    [UsedImplicitly]
    public DateTimeFull Timestamp;

    public User() { }

    public User(Chat from, string status, DateTimeFull timestamp)
        : this(from.Id, from.Username, GetName(from), status, timestamp) { }

    private User(long id, string? username, string? name, string status, DateTimeFull timestamp)
    {
        Id = id;
        _username = username;
        Name = name;
        Status = status;
        Timestamp = timestamp;
    }

    public void UpdateName(Chat from) => Name = GetName(from);

    private static string GetName(Chat from) => $"{from.FirstName} {from.LastName}";

    private string? _username;

    private const string UriFormat = "https://t.me/{0}";
    private const string LoginFormat = "@{0}";
}