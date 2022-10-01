using AbstractBot;
using System;

namespace StrollStatusBot;

public sealed class Config : ConfigGoogleSheets
{
    internal readonly string GoogleRange;

    public Config(string token, string systemTimeZoneId, string dontUnderstandStickerFileId,
        string forbiddenStickerFileId, TimeSpan sendMessagePeriodPrivate, TimeSpan sendMessagePeriodGroup,
        TimeSpan sendMessagePeriodGlobal, string googleCredentialJson, string applicationName, string googleSheetId,
        string googleRange)
        : base(token, systemTimeZoneId, dontUnderstandStickerFileId, forbiddenStickerFileId, sendMessagePeriodPrivate,
            sendMessagePeriodGroup, sendMessagePeriodGlobal, googleCredentialJson, applicationName, googleSheetId)
    {
        GoogleRange = googleRange;
    }
}