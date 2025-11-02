using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Web.Configuration;

namespace Web.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public WeatherController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        [HttpGet]
        [Route("Weather/GetWeather")]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City is required.");

            var client = _httpClientFactory.CreateClient();
            var apiUrl = $"{_apiSettings.BaseUrl}/weather?city={Uri.EscapeDataString(city)}";

            try
            {
                var result = await client.GetFromJsonAsync<WeatherResult>(apiUrl);
                if (result == null) return NotFound();
                return Ok(result); // JSON
            }
            catch
            {
                return StatusCode(502, "Upstream API error");
            }
        }
    }
}
