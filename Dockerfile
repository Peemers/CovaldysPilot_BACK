# build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copie les fichiers projet
COPY CovaldysPilot.Domain/CovaldysPilot.Domain.csproj CovaldysPilot.Domain/
COPY CovaldysPilot.Application/CovaldysPilot.Application.csproj CovaldysPilot.Application/
COPY CovaldysPilot.Infrastructure/CovaldysPilot.Infrastructure.csproj CovaldysPilot.Infrastructure/
COPY CovaldysPilot.API/CovaldysPilot.API.csproj CovaldysPilot.API/
COPY CovaldysPilot.Tests/CovaldysPilot.API.csproj CovaldysPilot.Tests/

# Restaure les dépendances
RUN dotnet restore CovaldysPilot.API/CovaldysPilot.API.csproj

# Copie tout le reste
COPY . .

# Build et publie
RUN dotnet publish CovaldysPilot.API/CovaldysPilot.API.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "CovaldysPilot.API.dll"]
