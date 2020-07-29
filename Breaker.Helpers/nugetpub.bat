@echo off
set /p version="Enter new version: "
set /p apiKey="Enter API Key: "

echo '%version%'
echo '%apiKey%'

dotnet nuget push ./bin/Release/Breaker.Helpers.%version%.nupkg -k %apiKey% -s https://api.nuget.org/v3/index.json