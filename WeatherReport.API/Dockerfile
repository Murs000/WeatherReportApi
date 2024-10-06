FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5109

ENV ASPNETCORE_URLS=http://+:5109

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["WeatherReport.API/WeatherReport.API.csproj", "WeatherReport.API/"]
RUN dotnet restore "WeatherReport.API/WeatherReport.API.csproj"
COPY . .
WORKDIR "/src/WeatherReport.API"
RUN dotnet build "WeatherReport.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "WeatherReport.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherReport.API.dll"]
