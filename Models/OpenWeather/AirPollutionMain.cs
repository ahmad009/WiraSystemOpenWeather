using Newtonsoft.Json;

namespace Models
{
    public class AirPollutionMain
    {
        // OpenWeatherMap returns AQI as integer 1..5
        [JsonProperty("aqi")]
        public int Aqi { get; set; }
    }
}
