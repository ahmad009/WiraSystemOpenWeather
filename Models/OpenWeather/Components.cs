using Newtonsoft.Json;

namespace Models
{
    public class Components
    {
        [JsonProperty("co")]
        public double CO { get; set; }

        [JsonProperty("no")]
        public double NO { get; set; }

        [JsonProperty("no2")]
        public double NO2 { get; set; }

        [JsonProperty("o3")]
        public double O3 { get; set; }

        [JsonProperty("so2")]
        public double SO2 { get; set; }

        [JsonProperty("pm2_5")]
        public double PM2_5 { get; set; }

        [JsonProperty("pm10")]
        public double PM10 { get; set; }

        [JsonProperty("nh3")]
        public double NH3 { get; set; }
    }
}
