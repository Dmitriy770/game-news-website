FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/GameNews.MediaStorage.Api/GameNews.MediaStorage.Api.csproj", "src/GameNews.MediaStorage.Api/"]
RUN dotnet restore "src/GameNews.MediaStorage.Api/GameNews.MediaStorage.Api.csproj"
COPY . .
WORKDIR "/src/src/GameNews.MediaStorage.Api"
RUN dotnet build "GameNews.MediaStorage.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameNews.MediaStorage.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameNews.MediaStorage.Api.dll"]
