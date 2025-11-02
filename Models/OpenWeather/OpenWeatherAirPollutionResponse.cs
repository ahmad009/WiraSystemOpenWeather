using Newtonsoft.Json;

namespace Models
{
    public class OpenWeatherAirPollutionResponse
    {
        [JsonProperty("list")]
        public AirPollutionItem[]? List { get; set; }
    }
}
