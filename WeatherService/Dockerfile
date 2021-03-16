#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WeatherService/WeatherService.csproj", "WeatherService/"]
RUN dotnet restore "WeatherService/WeatherService.csproj"
COPY . .
ENV APIKEY 6b34b4b2b9b20b3af3530f12a8a7723e
WORKDIR "/src/WeatherService"
RUN dotnet build "WeatherService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherService.dll"]