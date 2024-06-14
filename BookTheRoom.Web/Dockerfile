FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/mssql/server AS sqlserver
ENV SA_PASSWORD=YourStrong!Passw0rd
ENV ACCEPT_EULA=Y

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookTheRoom.WebUI/BookTheRoom.WebUI.csproj", "BookTheRoom.WebUI/"]
COPY ["BookTheRoom.Application/BookTheRoom.Application.csproj", "BookTheRoom.Application/"]
COPY ["BookTheRoom.Domain/BookTheRoom.Domain.csproj", "BookTheRoom.Domain/"]
COPY ["BookTheRoom.Infrastructure/BookTheRoom.Infrastructure.csproj", "BookTheRoom.Infrastructure/"]
RUN dotnet restore "./BookTheRoom.WebUI/./BookTheRoom.WebUI.csproj"
COPY . .
WORKDIR "/src/BookTheRoom.WebUI"
RUN dotnet build "./BookTheRoom.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookTheRoom.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookTheRoom.WebUI.dll"]

COPY --from=sqlserver / .
ENTRYPOINT ["dotnet", "BookTheRoom.WebUI.dll"]