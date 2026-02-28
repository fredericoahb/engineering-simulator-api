# ── Build stage ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files for layer caching
COPY EngineeringSimulator.sln ./
COPY src/EngineeringSimulator.Domain/EngineeringSimulator.Domain.csproj src/EngineeringSimulator.Domain/
COPY src/EngineeringSimulator.Application/EngineeringSimulator.Application.csproj src/EngineeringSimulator.Application/
COPY src/EngineeringSimulator.Infrastructure/EngineeringSimulator.Infrastructure.csproj src/EngineeringSimulator.Infrastructure/
COPY src/EngineeringSimulator.API/EngineeringSimulator.API.csproj src/EngineeringSimulator.API/
COPY tests/EngineeringSimulator.Domain.Tests/EngineeringSimulator.Domain.Tests.csproj tests/EngineeringSimulator.Domain.Tests/
COPY tests/EngineeringSimulator.API.Tests/EngineeringSimulator.API.Tests.csproj tests/EngineeringSimulator.API.Tests/

RUN dotnet restore

# Copy all source and build
COPY . .
RUN dotnet publish src/EngineeringSimulator.API/EngineeringSimulator.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Test stage (optional, for CI) ─────────────────────────
FROM build AS test
RUN dotnet test --no-restore --verbosity normal

# ── Runtime stage ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "EngineeringSimulator.API.dll"]
