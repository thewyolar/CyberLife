FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CyberLife/CyberLife.csproj", "CyberLife/"]
RUN dotnet restore "CyberLife/CyberLife.csproj"
COPY . .
WORKDIR "/src/CyberLife"
RUN dotnet build "CyberLife.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CyberLife.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CyberLife.dll"]

# docker run -it --rm -p 5000:8090 --name 2c9045ce2071 d95a6af28319
