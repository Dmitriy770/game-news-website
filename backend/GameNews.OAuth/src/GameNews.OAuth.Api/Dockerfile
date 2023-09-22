FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/GameNews.OAuth.Api/GameNews.OAuth.Api.csproj", "src/GameNews.OAuth.Api/"]
RUN dotnet restore "src/GameNews.OAuth.Api/GameNews.OAuth.Api.csproj"
COPY . .
WORKDIR "/src/src/GameNews.OAuth.Api"
RUN dotnet build "GameNews.OAuth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameNews.OAuth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameNews.OAuth.Api.dll"]
