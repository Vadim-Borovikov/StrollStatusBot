using AbstractBot;

namespace StrollStatusBot;

public sealed class Config : ConfigGoogleSheets
{
    internal readonly string GoogleRange;

    public Config(string token, string systemTimeZoneId, string dontUnderstandStickerFileId,
        string forbiddenStickerFileId, string googleCredentialJson, string applicationName, string googleSheetId,
        string googleRange)
        : base(token, systemTimeZoneId, dontUnderstandStickerFileId, forbiddenStickerFileId, googleCredentialJson,
            applicationName, googleSheetId)
    {
        GoogleRange = googleRange;
    }
}