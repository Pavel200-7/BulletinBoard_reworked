# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/UserService/BulletinBoard.UserService.Hosts/BulletinBoard.UserService.Hosts.csproj", "Src/UserService/BulletinBoard.UserService.Hosts/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure.ComponentRegistrar/BulletinBoard.UserService.Infrastructure.ComponentRegistrar.csproj", "Src/UserService/BulletinBoard.UserService.Infrastructure.ComponentRegistrar/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure.DataAccess/BulletinBoard.UserService.Infrastructure.DataAccess.csproj", "Src/UserService/BulletinBoard.UserService.Infrastructure.DataAccess/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure/BulletinBoard.UserService.Infrastructure.csproj", "Src/UserService/BulletinBoard.UserService.Infrastructure/"]
COPY ["Src/UserService/BulletinBoard.UserService.AppServices/BulletinBoard.UserService.AppServices.csproj", "Src/UserService/BulletinBoard.UserService.AppServices/"]
COPY ["Src/UserService/BulletinBoard.UserService.Generators/BulletinBoard.UserService.Generators.csproj", "Src/UserService/BulletinBoard.UserService.Generators/"]
COPY ["Src/UserService/BulletinBoard.UserService.Domain/BulletinBoard.UserService.Domain.csproj", "Src/UserService/BulletinBoard.UserService.Domain/"]
RUN dotnet restore "./Src/UserService/BulletinBoard.UserService.Hosts/BulletinBoard.UserService.Hosts.csproj"
COPY . .
WORKDIR "/src/Src/UserService/BulletinBoard.UserService.Hosts"
RUN dotnet build "./BulletinBoard.UserService.Hosts.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BulletinBoard.UserService.Hosts.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.UserService.Hosts.dll"]