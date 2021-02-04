using Newtonsoft.Json;

namespace StrollStatusBot.Web.Models
{
    public sealed class Config : Bot.Config
    {
        [JsonProperty]
        public string GoogleCredentialJson { get; set; }
    }
}