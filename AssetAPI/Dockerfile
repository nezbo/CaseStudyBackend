FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["AssetAPI/AssetAPI.csproj", "AssetAPI/"]
COPY ["Microservice.SourceGeneration/Microservice.SourceGeneration.csproj", "Microservice.SourceGeneration/"]
COPY ["Microservice.SourceGeneration.Attributes/Microservice.SourceGeneration.Attributes.csproj", "Microservice.SourceGeneration.Attributes/"]
COPY ["Microservice.Common/Microservice.Common.csproj", "Microservice.Common/"]
RUN dotnet restore "./AssetAPI/AssetAPI.csproj"
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