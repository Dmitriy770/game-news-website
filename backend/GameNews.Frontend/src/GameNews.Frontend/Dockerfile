FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ["src/GameNews.Frontend/GameNews.Frontend.csproj", "src/GameNews.Frontend/"]
RUN dotnet restore "src/GameNews.Frontend/GameNews.Frontend.csproj"

COPY . .

 CMD ["dotnet", "watch", "run", "-v", "--project", "src/GameNews.Frontend/GameNews.Frontend.csproj", "--launch-profile", "docker", "--non-interactive"]
