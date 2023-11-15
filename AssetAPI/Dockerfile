#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["AssetAPI/AssetAPI.csproj", "AssetAPI/"]
COPY ["Microservice.Common/Microservice.Common.csproj", "Microservice.Common/"]
RUN dotnet restore "./AssetAPI/./AssetAPI.csproj"
COPY . .
WORKDIR "/src/AssetAPI"
RUN dotnet build "./AssetAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AssetAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
RUN chown app:app -R /app
USER app
ENTRYPOINT ["dotnet", "AssetAPI.dll"]