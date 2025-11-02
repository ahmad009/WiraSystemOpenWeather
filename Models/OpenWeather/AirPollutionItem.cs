using Newtonsoft.Json;

namespace Models
{
    public class AirPollutionItem
    {
        [JsonProperty("main")]
        public AirPollutionMain? Main { get; set; }

        [JsonProperty("components")]
        public Components? Components { get; set; }

        [JsonProperty("dt")]
        public long Dt { get; set; }
    }
}
