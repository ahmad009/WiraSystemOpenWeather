using Newtonsoft.Json;

namespace Models
{
    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }
}
