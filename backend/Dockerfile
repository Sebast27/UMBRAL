FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY Umbral.Domain/Umbral.Domain.csproj Umbral.Domain/
COPY Umbral.Application/Umbral.Application.csproj Umbral.Application/
COPY Umbral.Infrastructure/Umbral.Infrastructure.csproj Umbral.Infrastructure/
COPY Umbral.Adapters.API/Umbral.Adapters.API.csproj Umbral.Adapters.API/

# Restaurar paquetes
RUN dotnet restore Umbral.Adapters.API/Umbral.Adapters.API.csproj

# Copiar todo el código
COPY . .

# Publicar la aplicación
WORKDIR /src/Umbral.Adapters.API
RUN dotnet publish -c Release -o /app/publish

# Imagen de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Establecer entrypoint
ENTRYPOINT ["dotnet", "Umbral.Adapters.API.dll"]