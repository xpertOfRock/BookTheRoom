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
COPY ["BookTheRoom.Web/BookTheRoom.Web.csproj", "BookTheRoom.Web/"]
COPY ["BookTheRoom.Application/BookTheRoom.Application.csproj", "BookTheRoom.Application/"]
COPY ["BookTheRoom.Core/BookTheRoom.Core.csproj", "BookTheRoom.Core/"]
COPY ["BookTheRoom.Infrastructure/BookTheRoom.Infrastructure.csproj", "BookTheRoom.Infrastructure/"]
RUN dotnet restore "./BookTheRoom.Web/./BookTheRoom.Web.csproj"
COPY . .
WORKDIR "/src/BookTheRoom.Web"
RUN dotnet build "./BookTheRoom.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookTheRoom.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookTheRoom.Web.dll"]

COPY --from=sqlserver / .
ENTRYPOINT ["dotnet", "BookTheRoom.Web.dll"]