# Создает среду для выгрузки исходного кода
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /Src

ARG SERVICE_PATH=/Src/UserService
ARG LAYER_PREFIX=BulletinBoard.UserService
COPY ["Src/UserService/BulletinBoard.UserService.Domain/BulletinBoard.UserService.Domain.csproj", "./UserService/BulletinBoard.UserService.Domain/"]
COPY ["Src/UserService/BulletinBoard.UserService.AppServices/BulletinBoard.UserService.AppServices.csproj", "./UserService/BulletinBoard.UserService.AppServices/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure/BulletinBoard.UserService.Infrastructure.csproj", "./UserService/BulletinBoard.UserService.Infrastructure/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure.DataAccess/BulletinBoard.UserService.Infrastructure.DataAccess.csproj", "./UserService/BulletinBoard.UserService.Infrastructure.DataAccess/"]
COPY ["Src/UserService/BulletinBoard.UserService.Infrastructure.ComponentRegistrar/BulletinBoard.UserService.Infrastructure.ComponentRegistrar.csproj", "./UserService/BulletinBoard.Infrastructure.ComponentRegistrar/"]
COPY ["Src/UserService/BulletinBoard.UserService.Hosts/BulletinBoard.UserService.Hosts.csproj", "./UserService/BulletinBoard.UserService.Hosts/"]
RUN dotnet restore "./UserService/BulletinBoard.UserService.Hosts/BulletinBoard.UserService.Hosts.csproj"

COPY /Src .
WORKDIR /Src/UserService/BulletinBoard.UserService.Hosts
RUN dotnet build "./BulletinBoard.UserService.Hosts.csproj" -c release -o /app/build

FROM base AS publish
RUN dotnet publish "./BulletinBoard.UserService.Hosts.csproj" -c release -o /app/publish --no-restore 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "BulletinBoard.UserService.Hosts.dll"]

