using Newtonsoft.Json;

namespace Models
{
    public class OpenWeatherWeatherResponse
    {
        [JsonProperty("coord")]
        public Coord Coord { get; set; } = new Coord();

        [JsonProperty("weather")]
        public object[]? Weather { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; } = new Main();

        [JsonProperty("wind")]
        public Wind? Wind { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
