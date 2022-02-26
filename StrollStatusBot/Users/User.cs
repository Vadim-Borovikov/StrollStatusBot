using System;
using System.Collections.Generic;
using GoogleSheetsManager;
using GryphonUtilities;

namespace StrollStatusBot.Users;

internal sealed class User : ISavable
{
    IList<string> ISavable.Titles => Titles;

    public readonly long Id;

    public string Status { get; internal set; }
    public DateTime Timestamp { get; internal set; }

    public User(Telegram.Bot.Types.User from, string status, DateTime timestamp)
        : this(from.Id, from.Username, GetName(from), status, timestamp) { }

    private User(long id, string? username, string? name, string status, DateTime timestamp)
    {
        Id = id;
        _username = username;
        _name = name;
        Status = status;
        Timestamp = timestamp;
    }

    public static User Load(IDictionary<string, object?> valueSet)
    {
        long id = valueSet[IdTitle].ToLong().GetValue("Empty Id");

        string? status = valueSet[StatusTitle]?.ToString();
        string statusValue = status.GetValue($"Empty status in {id}");

        DateTime timestamp = valueSet[TimestampTitle].ToDateTime().GetValue($"Empty datetime in {id}");

        string? name = valueSet[NameTitle]?.ToString();

        string? username = valueSet[UsernameTitle]?.ToString()?.Remove(0, 1);

        return new User(id, username, name, statusValue, timestamp);
    }

    public void UpdateName(Telegram.Bot.Types.User from) => _name = GetName(from);

    public IDictionary<string, object?> Convert()
    {
        Uri uri = new(string.Format(UriFormat, _username));
        string login = string.Format(LoginFormat, _username);

        return new Dictionary<string, object?>
        {
            { IdTitle, Id },
            { NameTitle, _name },
            { UsernameTitle, $"{Utils.GetHyperlink(uri, login)}" },
            { StatusTitle, Status },
            { TimestampTitle, $"{Timestamp:d MMMM yyyy HH:mm:ss}" }
        };
    }

    private static string GetName(Telegram.Bot.Types.User from) => $"{from.FirstName} {from.LastName}";

    private readonly string? _username;

    private string? _name;

    private static readonly List<string> Titles = new()
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