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
COPY ["src/AssureCloud.Api/AssureCloud.Api.csproj", "src/AssureCloud.Api/"]
COPY ["src/AssureCloud.ServiceDefaults/AssureCloud.ServiceDefaults.csproj", "src/AssureCloud.ServiceDefaults/"]

# Restore dependencies
RUN dotnet restore "AssureCloud.sln"

# Copy source code
COPY ["src/AssureCloud.Domain/", "src/AssureCloud.Domain/"]
COPY ["src/AssureCloud.Application/", "src/AssureCloud.Application/"]
COPY ["src/AssureCloud.Infrastructure/", "src/AssureCloud.Infrastructure/"]
COPY ["src/AssureCloud.Api/", "src/AssureCloud.Api/"]
COPY ["src/AssureCloud.ServiceDefaults/", "src/AssureCloud.ServiceDefaults/"]

# Build
WORKDIR "/src/src/AssureCloud.Api"
RUN dotnet build "AssureCloud.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "AssureCloud.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Create non-root user
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssureCloud.Api.dll"]