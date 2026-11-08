FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["LootApi/LootApi.csproj", "LootApi/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["Models/Models.csproj", "Models/"]
RUN dotnet restore "LootApi/LootApi.csproj"

COPY . .
WORKDIR /src/LootApi
RUN dotnet publish "LootApi.csproj" \
    --configuration $BUILD_CONFIGURATION \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build --chown=$APP_UID:$APP_UID /app/publish .
ENTRYPOINT ["dotnet", "LootApi.dll"]
