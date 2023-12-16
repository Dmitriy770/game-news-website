FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GameNews.Articles.Api/GameNews.Articles.Api.csproj", "src/GameNews.Articles.Api/"]
COPY ["src/GameNews.Articles.Infrastructure/GameNews.Articles.Infrastructure.csproj", "src/GameNews.Articles.Infrastructure/"]
COPY ["src/GameNews.Articles.Domain/GameNews.Articles.Domain.csproj", "src/GameNews.Articles.Domain/"]
RUN dotnet restore "src/GameNews.Articles.Api/GameNews.Articles.Api.csproj"
COPY . .
WORKDIR "/src/src/GameNews.Articles.Api"
RUN dotnet build "GameNews.Articles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GameNews.Articles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameNews.Articles.Api.dll"]
