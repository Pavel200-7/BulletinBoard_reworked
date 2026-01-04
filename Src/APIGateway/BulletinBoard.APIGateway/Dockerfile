FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/APIGateway/BulletinBoard.APIGateway/BulletinBoard.APIGateway.csproj", "Src/APIGateway/BulletinBoard.APIGateway/"]
RUN dotnet restore "./Src/APIGateway/BulletinBoard.APIGateway/BulletinBoard.APIGateway.csproj"
COPY . .
WORKDIR "/src/Src/APIGateway/BulletinBoard.APIGateway"
RUN dotnet build "./BulletinBoard.APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BulletinBoard.APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.APIGateway.dll"]