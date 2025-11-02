namespace Models
{
    public class WeatherResult
    {
        public string City { get; set; } = string.Empty;

        // Temperature in Celsius
        public double TemperatureC { get; set; }

        // Humidity in percent
        public int HumidityPercent { get; set; }

        // Wind speed in meter/sec
        public double WindSpeedMeterPerSec { get; set; }

        // AQI (OpenWeather: 1..5), we'll expose the raw integer and also a textual meaning if needed later
        public int AQI { get; set; }

        // Major pollutants (the raw components object from OpenWeather)
        public Components? MajorPollutants { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Optional: human readable description of AQI (derived by service if desired)
        public string? AQIDescription { get; set; }

        // Optional: additional metadata if needed
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
