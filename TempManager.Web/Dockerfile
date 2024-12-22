# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TempManager.Web/TempManager.Web.csproj", "TempManager.Web/"]
COPY ["TempManager.BL/TempManager.BL.csproj", "TempManager.BL/"]
COPY ["TempManager.Shibboleth/TempManager.Shibboleth.csproj", "TempManager.Shibboleth/"]
COPY ["TempManager.Common/TempManager.Common.csproj", "TempManager.Common/"]
COPY ["TempManager.DL/TempManager.DL.csproj", "TempManager.DL/"]
COPY ["TempManager.KNX.Api/TempManager.KNX.Api.csproj", "TempManager.KNX.Api/"]
RUN dotnet restore "./TempManager.Web/TempManager.Web.csproj"
COPY . .
WORKDIR "/src/TempManager.Web"
RUN dotnet build "./TempManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TempManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TempManager.Web.dll"]