# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers de projet et restaurer les dépendances
COPY ["JIR.BlazorApp/JIR.BlazorApp.csproj", "JIR.BlazorApp/"]
COPY ["JIR.BlazorApp/Models/", "JIR.BlazorApp/Models/"]
COPY ["JIR.BlazorApp/Services/", "JIR.BlazorApp/Services/"]
RUN dotnet restore "JIR.BlazorApp/JIR.BlazorApp.csproj"

# Copier tout le code source et compiler
COPY JIR.BlazorApp/ JIR.BlazorApp/
WORKDIR "/src/JIR.BlazorApp"
RUN dotnet build "JIR.BlazorApp.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "JIR.BlazorApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copier les fichiers publiés depuis l'étape publish
COPY --from=publish /app/publish .

# Point d'entrée
ENTRYPOINT ["dotnet", "JIR.BlazorApp.dll"]
