using System.Collections.Generic;
using Newtonsoft.Json;

namespace StrollStatusBot.Bot
{
    public class Config
    {
        [JsonProperty]
        public string Token { get; set; }

        [JsonProperty]
        public string Host { get; set; }

        [JsonProperty]
        public int Port { get; set; }

        internal string Url => $"{Host}:{Port}/{Token}";

        [JsonProperty]
        public Dictionary<string, string> GoogleCredential { get; set; }

        [JsonProperty]
        public string GoogleSheetId { get; set; }

        [JsonProperty]
        public string GoogleRange { get; set; }

        [JsonProperty]
        public string SystemTimeZoneId { get; set; }

        [JsonProperty]
        public string CultureInfoName { get; set; }

        [JsonProperty]
        public List<string> InstructionLines { get; set; }
    }
}