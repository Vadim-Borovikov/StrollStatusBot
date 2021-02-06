using Newtonsoft.Json;
using StrollStatusBot.Bot;

namespace StrollStatusBot.Web.Models
{
    public sealed class Config : BotConfig
    {
        [JsonProperty]
        public string CultureInfoName { get; set; }

        [JsonProperty]
        public string GoogleCredentialJson { get; set; }
    }
}