FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Hosts/BulletinBoard.NotificationService.Hosts.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Hosts/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar/BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Infrastructure/BulletinBoard.NotificationService.Infrastructure.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Infrastructure/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.AppServices/BulletinBoard.NotificationService.AppServices.csproj", "Src/NotificationService/BulletinBoard.NotificationService.AppServices/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Domain/BulletinBoard.NotificationService.Domain.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Domain/"]
RUN dotnet restore "./Src/NotificationService/BulletinBoard.NotificationService.Hosts/BulletinBoard.NotificationService.Hosts.csproj"
COPY . .
WORKDIR "/src/Src/NotificationService/BulletinBoard.NotificationService.Hosts"
RUN dotnet build "./BulletinBoard.NotificationService.Hosts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./BulletinBoard.NotificationService.Hosts.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.NotificationService.Hosts.dll"]