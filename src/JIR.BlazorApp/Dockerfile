# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build

# Désactiver les configurations NuGet locales de Visual Studio
ENV DOTNET_CLI_HOME=/tmp/dotnet
ENV NUGET_PACKAGES=/root/.nuget/packages
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_NOLOGO=1

# Copier les fichiers de projet et restaurer les dépendances
COPY ["JIR.BlazorApp.csproj", "./"]
RUN dotnet restore "JIR.BlazorApp.csproj" --disable-parallel

# Copier tout le code source et compiler
COPY . .
RUN dotnet build "JIR.BlazorApp.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
WORKDIR /build
RUN dotnet publish "JIR.BlazorApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# Expose only the app port
EXPOSE 8080

# Copier les fichiers publiés depuis l'étape publish
COPY --from=publish /app/publish .

# Point d'entrée
ENTRYPOINT ["dotnet", "JIR.BlazorApp.dll"]
