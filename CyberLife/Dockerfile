FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8090

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CyberLife/CyberLife.csproj", "CyberLife/"]
RUN dotnet restore "CyberLife/CyberLife.csproj"
COPY . .
WORKDIR "/src/CyberLife"
RUN dotnet build "CyberLife.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CyberLife.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:8090
ENTRYPOINT ["dotnet", "CyberLife.dll"]

# docker run -it --rm -p 5000:8090 --name 2c9045ce2071 d95a6af28319
