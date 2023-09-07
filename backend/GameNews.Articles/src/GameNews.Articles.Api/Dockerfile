FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/GameNews.Articles.Api/GameNews.Articles.Api.csproj", "src/GameNews.Articles.Api/"]
RUN dotnet restore "src/GameNews.Articles.Api/GameNews.Articles.Api.csproj"
COPY . .
WORKDIR "/src/src/GameNews.Articles.Api"
RUN dotnet build "GameNews.Articles.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "GameNews.Articles.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameNews.Articles.Api.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameNews.Articles.Api.dll"]
