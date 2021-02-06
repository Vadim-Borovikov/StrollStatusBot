using Newtonsoft.Json;
using AbstractBot;

namespace StrollStatusBot.Bot
{
    public class BotConfig : ConfigGoogleSheets
    {
        [JsonProperty]
        public string GoogleRange { get; set; }
    }
}