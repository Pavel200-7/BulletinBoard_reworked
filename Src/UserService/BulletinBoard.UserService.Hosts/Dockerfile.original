FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Src/UserService/BulletinBoard.UserService.Hosts/BulletinBoard.UserService.Hosts.csproj", "Src/UserService/BulletinBoard.UserService.Hosts/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure.ComponentRegistrar/BulletinBoard.UserService.Infrastructure.ComponentRegistrar.csproj", "Src/UserService/BulletinBoard.UserService.Infrastructure.ComponentRegistrar/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure/BulletinBoard.UserService.Infrastructure.csproj", "Src/UserService/BulletinBoard.UserService.Infrastructure/"]
COPY ["Src/UserService/BulletinBoard.UserService.AppServices/BulletinBoard.UserService.AppServices.csproj", "Src/UserService/BulletinBoard.UserService.AppServices/"]
RUN dotnet restore "./Src/UserService/BulletinBoard.UserService.Hosts/BulletinBoard.UserService.Hosts.csproj"
COPY . .
WORKDIR "/src/Src/UserService/BulletinBoard.UserService.Hosts"
RUN dotnet build "./BulletinBoard.UserService.Hosts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./BulletinBoard.UserService.Hosts.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.UserService.Hosts.dll"]