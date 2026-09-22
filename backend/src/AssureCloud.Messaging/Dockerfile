# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["global.json", "."]
COPY ["AssureCloud.sln", "."]
COPY ["src/AssureCloud.Domain/AssureCloud.Domain.csproj", "src/AssureCloud.Domain/"]
COPY ["src/AssureCloud.Application/AssureCloud.Application.csproj", "src/AssureCloud.Application/"]
COPY ["src/AssureCloud.Infrastructure/AssureCloud.Infrastructure.csproj", "src/AssureCloud.Infrastructure/"]
COPY ["src/AssureCloud.Messaging/AssureCloud.Messaging.csproj", "src/AssureCloud.Messaging/"]
COPY ["src/AssureCloud.ServiceDefaults/AssureCloud.ServiceDefaults.csproj", "src/AssureCloud.ServiceDefaults/"]

# Restore dependencies
RUN dotnet restore "AssureCloud.sln"

# Copy source code
COPY ["src/AssureCloud.Domain/", "src/AssureCloud.Domain/"]
COPY ["src/AssureCloud.Application/", "src/AssureCloud.Application/"]
COPY ["src/AssureCloud.Infrastructure/", "src/AssureCloud.Infrastructure/"]
COPY ["src/AssureCloud.Messaging/", "src/AssureCloud.Messaging/"]
COPY ["src/AssureCloud.ServiceDefaults/", "src/AssureCloud.ServiceDefaults/"]

# Build
WORKDIR "/src/src/AssureCloud.Messaging"
RUN dotnet build "AssureCloud.Messaging.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "AssureCloud.Messaging.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssureCloud.Messaging.dll"]