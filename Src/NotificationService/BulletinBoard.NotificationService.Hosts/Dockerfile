# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Hosts/BulletinBoard.NotificationService.Hosts.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Hosts/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar/BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Infrastructure/BulletinBoard.NotificationService.Infrastructure.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Infrastructure/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.AppServices/BulletinBoard.NotificationService.AppServices.csproj", "Src/NotificationService/BulletinBoard.NotificationService.AppServices/"]
COPY ["Src/NotificationService/BulletinBoard.NotificationService.Domain/BulletinBoard.NotificationService.Domain.csproj", "Src/NotificationService/BulletinBoard.NotificationService.Domain/"]
RUN dotnet restore "./Src/NotificationService/BulletinBoard.NotificationService.Hosts/BulletinBoard.NotificationService.Hosts.csproj"
COPY . .
WORKDIR "/src/Src/NotificationService/BulletinBoard.NotificationService.Hosts"
RUN dotnet build "./BulletinBoard.NotificationService.Hosts.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BulletinBoard.NotificationService.Hosts.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.NotificationService.Hosts.dll"]