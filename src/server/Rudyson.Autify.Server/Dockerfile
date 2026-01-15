# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/server/Rudyson.Autify.Server/Rudyson.Autify.Server.csproj", "src/server/Rudyson.Autify.Server/"]
COPY ["src/server/Rudyson.Autify.Application/Rudyson.Autify.Application.csproj", "src/server/Rudyson.Autify.Application/"]
COPY ["src/server/Rudyson.Autify.Domain/Rudyson.Autify.Domain.csproj", "src/server/Rudyson.Autify.Domain/"]
COPY ["src/server/Rudyson.Autify.Infrastructure/Rudyson.Autify.Infrastructure.csproj", "src/server/Rudyson.Autify.Infrastructure/"]
RUN dotnet restore "./src/server/Rudyson.Autify.Server/Rudyson.Autify.Server.csproj"
COPY . .
WORKDIR "/src/src/server/Rudyson.Autify.Server"
RUN dotnet build "./Rudyson.Autify.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rudyson.Autify.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rudyson.Autify.Server.dll"]