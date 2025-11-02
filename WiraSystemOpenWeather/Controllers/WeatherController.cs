using Microsoft.AspNetCore.Mvc;
using Services;

namespace WiraSystemOpenWeather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IOpenWeatherService _weatherService;

        public WeatherController(IOpenWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Get current weather and air quality data for a city
        /// </summary>
        /// <param name="city">City name (e.g., Tehran)</param>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            var result = await _weatherService.GetCityWeatherAsync(city);

            if (result == null)
                return NotFound($"No data found for city '{city}'.");

            return Ok(result);
        }
    }
}
