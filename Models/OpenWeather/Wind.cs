using Newtonsoft.Json;

namespace Models
{
    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
    }
}
