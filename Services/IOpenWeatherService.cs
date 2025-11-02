using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;

namespace Services
{
    public interface IOpenWeatherService
    {
        Task<WeatherResult?> GetCityWeatherAsync(string city);
    }

    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public OpenWeatherService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<WeatherResult?> GetCityWeatherAsync(string city)
        {
            var apiKey = _config["OpenWeather:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("OpenWeather API key is not configured.");

            var client = _httpClientFactory.CreateClient("openweather");

            // Step 1: Weather data
            var weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";
            var weatherResponse = await client.GetAsync(weatherUrl);
            if (!weatherResponse.IsSuccessStatusCode)
                return null;

            var weatherJson = await weatherResponse.Content.ReadAsStringAsync();
            var weatherData = JsonConvert.DeserializeObject<OpenWeatherWeatherResponse>(weatherJson);

            if (weatherData == null || weatherData.Coord == null)
                return null;

            // Step 2: Air pollution data
            var airUrl = $"https://api.openweathermap.org/data/2.5/air_pollution?lat={weatherData.Coord.Lat}&lon={weatherData.Coord.Lon}&appid={apiKey}";
            var airResponse = await client.GetAsync(airUrl);
            if (!airResponse.IsSuccessStatusCode)
                return null;

            var airJson = await airResponse.Content.ReadAsStringAsync();
            var airData = JsonConvert.DeserializeObject<OpenWeatherAirPollutionResponse>(airJson);

            var airItem = airData?.List != null && airData.List.Length > 0 ? airData.List[0] : null;

            // Step 3: Combine results
            var result = new WeatherResult
            {
                City = weatherData.Name,
                TemperatureC = weatherData.Main.Temp,
                HumidityPercent = weatherData.Main.Humidity,
                WindSpeedMeterPerSec = weatherData.Wind?.Speed ?? 0,
                AQI = airItem?.Main?.Aqi ?? 0,
                MajorPollutants = airItem?.Components,
                Latitude = weatherData.Coord.Lat,
                Longitude = weatherData.Coord.Lon,
                AQIDescription = GetAqiDescription(airItem?.Main?.Aqi ?? 0)
            };

            return result;
        }

        private static string GetAqiDescription(int aqi)
        {
            return aqi switch
            {
                1 => "Good",
                2 => "Fair",
                3 => "Moderate",
                4 => "Poor",
                5 => "Very Poor",
                _ => "Unknown"
            };
        }
    }
}
