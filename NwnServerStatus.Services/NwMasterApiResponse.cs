using System.Text.Json.Serialization;

namespace NwnServerStatus.Services
{
    public class NwMasterApiResponse
    {
        [JsonPropertyName("first_seen")]
        public int FirstSeen { get; set; }

        [JsonPropertyName("last_advertisement")]
        public int LastAdvertisement { get; set; }

        [JsonPropertyName("session_name")]
        public string SessionName { get; set; }

        [JsonPropertyName("module_name")]
        public string ModuleName { get; set; }

        [JsonPropertyName("passworded")]
        public bool Passworded { get; set; }

        [JsonPropertyName("current_players")]
        public int CurrentPlayers { get; set; }

        [JsonPropertyName("max_players")]
        public int MaxPlayers { get; set; }

        [JsonPropertyName("latency")]
        public int Latency { get; set; }

        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("kx_pk")]
        public string KxPk { get; set; }
    }
}