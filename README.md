# WiraWeather Solution

This solution contains five projects related to weather data, API services, and a web dashboard. All projects can be built and run together using the provided `Start.bat` script.

## Projects Overview

1. **Models**  
   Contains data models and DTOs for weather information, cities, and countries.

2. **Services**  
   Implements core business logic such as fetching weather data from OpenWeather API, caching, and data processing.

3. **Web**  
   ASP.NET Core web application providing a dashboard for users to search cities, view current weather, air quality, and historical trends.

4. **WiraSystemOpenWeather**  
   RESTful API project exposing endpoints for weather data, supporting city queries, pollutants, and AQI information.

5. **WiraWeatherTest**  
   Unit and integration tests for API and services to ensure correctness, error handling, and response validation.

## How to Run

1. Open the solution folder in **Windows Explorer**.  
2. Double-click **`Start.bat`**. This will:  
   - Build and run the API (`WiraSystemOpenWeather`) in a separate window.  
   - Wait until the API is ready.  
   - Build and run the Web app (`Web`) in a separate window.  
   - Open the default browser to the API Swagger UI and the Web dashboard.  

> Ensure you have **.NET SDK** installed and that ports `5003`, `6003`, and `5199` are available.
