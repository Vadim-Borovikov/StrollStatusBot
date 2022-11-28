using System.Collections.Generic;
using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using AbstractBot.GoogleSheets;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace StrollStatusBot;

[PublicAPI]
public class Config : AbstractBot.Config, IConfigGoogleSheets
{
    public Dictionary<string, string>? GoogleCredential { get; init; }
    public string? GoogleCredentialJson { get; init; }

    [Required]
    [MinLength(1)]
    public string ApplicationName { get; init; } = null!;

    [Required]
    [MinLength(1)]
    public string GoogleSheetId { get; init; } = null!;

    [Required]
    [MinLength(1)]
    public string GoogleRange { get; init; } = null!;
}