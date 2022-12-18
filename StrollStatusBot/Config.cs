using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using AbstractBot.Configs;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace StrollStatusBot;

[PublicAPI]
public class Config : ConfigGoogleSheets
{
    [Required]
    [MinLength(1)]
    public string GoogleSheetId { get; init; } = null!;

    [Required]
    [MinLength(1)]
    public string GoogleTitle { get; init; } = null!;

    [Required]
    [MinLength(1)]
    public string GoogleRange { get; init; } = null!;
}