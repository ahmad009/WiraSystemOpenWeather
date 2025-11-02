@echo off
REM --- Start API in a new window ---
start "API" cmd /k "dotnet run --project WiraSystemOpenWeather"

REM --- Wait until API is responding ---
echo Waiting for API to start...
:check
timeout /t 2 > nul
curl -s http://localhost:5003/swagger > nul 2>&1
if errorlevel 1 (
    goto check
)

REM --- Start Web app in a new window after API is ready ---
start "Web" cmd /k "dotnet run --project Web"

REM --- Open both sites in default browser ---
timeout /t 2 > nul
start "" "https://localhost:6003/swagger/index.html"
start "" "http://localhost:5199"

echo Both API and Web app have been started, and browsers opened.