# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers de projet et restaurer les dépendances
COPY ["JIR.WebAPI/JIR.WebAPI.csproj", "JIR.WebAPI/"]
COPY ["JIR.Application/JIR.Application.csproj", "JIR.Application/"]
COPY ["JIR.Domain/JIR.Domain.csproj", "JIR.Domain/"]
COPY ["JIR.Infrastructure/JIR.Infrastructure.csproj", "JIR.Infrastructure/"]
RUN dotnet restore "JIR.WebAPI/JIR.WebAPI.csproj"

# Copier tout le code source et compiler
COPY . .
WORKDIR "/src/JIR.WebAPI"
RUN dotnet build "JIR.WebAPI.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "JIR.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copier les fichiers publiés depuis l'étape publish
COPY --from=publish /app/publish .

# Point d'entrée
ENTRYPOINT ["dotnet", "JIR.WebAPI.dll"]
