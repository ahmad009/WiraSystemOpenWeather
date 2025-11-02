using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configuration (appsettings.json + env vars)
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

// Add logging (default)
builder.Services.AddLogging();

// Swagger / OpenAPI for easy testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "OpenWeather Assessment API",
        Version = "v1",
        Description = "Returns weather + air quality data via OpenWeatherMap"
    });
});

// HttpClient for calling OpenWeather APIs
builder.Services.AddHttpClient("openweather", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("OpenWeatherAssessment/1.0");
});

// Register application services
builder.Services.AddScoped<IOpenWeatherService, OpenWeatherService>();

// Optional: allow CORS for local testing (adjust in production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Validate required configuration (fail fast if missing)
var apiKey = app.Configuration.GetValue<string>("OpenWeather:ApiKey");
if (string.IsNullOrWhiteSpace(apiKey))
{
    app.Logger.LogWarning("OpenWeather:ApiKey is not configured. The API will return errors when calling upstream.");
    // We *don't* exit; we let the app run so developer can still view Swagger and local errors.
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenWeatherAssessment v1"));
}
else
{
    // In production: minimal error page and HSTS could be added
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors("LocalDev");

//app.UseAuthorization();

app.MapControllers();

app.Run();
