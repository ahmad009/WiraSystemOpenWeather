using Microsoft.Extensions.Configuration;
using Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Services;
using System.Net;

namespace WiraWeatherTest
{
    public class OpenWeatherServiceTests
    {
        [Fact]
        public async Task GetCityWeatherAsync_ReturnsValidWeatherResult()
        {
            //var configuration = new ConfigurationBuilder().AddJsonFile("../OpenWeather.Api/appsettings.json").Build();

            // Arrange: mock weather response
            var weatherMockResponse = new OpenWeatherWeatherResponse
            {
                Name = "Tehran",
                Coord = new Coord { Lat = 35.6892, Lon = 51.3890 },
                Main = new Main { Temp = 25.5, Humidity = 50 },
                Wind = new Wind { Speed = 3.2 }
            };

            // Arrange: mock air pollution response
            var airMockResponse = new OpenWeatherAirPollutionResponse
            {
                List = new[]
                {
                    new AirPollutionItem
                    {
                        Main = new AirPollutionMain { Aqi = 2 },
                        Components = new Components
                        {
                            CO = 0.3,
                            NO = 0.02,
                            NO2 = 0.01,
                            O3 = 0.05,
                            SO2 = 0.004,
                            PM2_5 = 12,
                            PM10 = 20,
                            NH3 = 0.1
                        }
                    }
                }
            };

            // Mock HttpClient
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsoluteUri.Contains("weather")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(weatherMockResponse))
                });

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsoluteUri.Contains("air_pollution")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(airMockResponse))
                });

            var httpClient = new HttpClient(handlerMock.Object);

            // Mock IHttpClientFactory
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // In-memory configuration
            var inMemorySettings = new Dictionary<string, string>
            {
                {"OpenWeather:ApiKey", "FAKE_API_KEY"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var service = new OpenWeatherService(httpClientFactoryMock.Object, configuration);

            // Act
            var result = await service.GetCityWeatherAsync("Tehran");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tehran", result!.City);
            Assert.Equal(25.5, result.TemperatureC);
            Assert.Equal(50, result.HumidityPercent);
            Assert.Equal(3.2, result.WindSpeedMeterPerSec);
            Assert.Equal(2, result.AQI);
            Assert.NotNull(result.MajorPollutants);
            Assert.Equal(0.3, result.MajorPollutants!.CO);
            Assert.Equal(35.6892, result.Latitude);
            Assert.Equal(51.3890, result.Longitude);
            Assert.Equal("Fair", result.AQIDescription);
        }

        [Fact]
        public async Task GetCityWeatherAsync_InvalidCity_ReturnsNull()
        {
            // Arrange: HttpClient که 404 برمی‌گرداند
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var inMemorySettings = new Dictionary<string, string> { { "OpenWeather:ApiKey", "FAKE_API_KEY" } };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            var service = new OpenWeatherService(httpClientFactoryMock.Object, configuration);

            // Act
            var result = await service.GetCityWeatherAsync("InvalidCity");

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task GetCityWeatherAsync_NoAirData_ReturnsResultWithZeroAQI()
        {
            // Arrange: weather response صحیح
            var weatherMockResponse = new OpenWeatherWeatherResponse
            {
                Name = "Tehran",
                Coord = new Coord { Lat = 35.6892, Lon = 51.3890 },
                Main = new Main { Temp = 25.5, Humidity = 50 },
                Wind = new Wind { Speed = 3.2 }
            };

            // Air pollution response خالی
            var airMockResponse = new OpenWeatherAirPollutionResponse
            {
                List = Array.Empty<AirPollutionItem>()
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsoluteUri.Contains("weather")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(weatherMockResponse))
                });

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsoluteUri.Contains("air_pollution")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(airMockResponse))
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var inMemorySettings = new Dictionary<string, string> { { "OpenWeather:ApiKey", "FAKE_API_KEY" } };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            var service = new OpenWeatherService(httpClientFactoryMock.Object, configuration);

            // Act
            var result = await service.GetCityWeatherAsync("Tehran");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result!.AQI);
            Assert.Null(result.MajorPollutants);
        }


        [Fact]
        public async Task GetCityWeatherAsync_WeatherApiError_ReturnsNull()
        {
            // Arrange: HttpClient که خطای 500 برمی‌گرداند
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsoluteUri.Contains("weather")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var inMemorySettings = new Dictionary<string, string> { { "OpenWeather:ApiKey", "FAKE_API_KEY" } };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            var service = new OpenWeatherService(httpClientFactoryMock.Object, configuration);

            // Act
            var result = await service.GetCityWeatherAsync("Tehran");

            // Assert
            Assert.Null(result);
        }

    }
}