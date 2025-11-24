using JIR.Application;
using JIR.Infrastructure;
using JIR.Infrastructure.Data;
using JIR.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Convert Railway DATABASE_URL to Npgsql connection string if present
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    var databaseUri = new Uri(databaseUrl);
    var userInfo = databaseUri.UserInfo.Split(':');
    
    var connectionString = $"Host={databaseUri.Host};" +
                          $"Port={databaseUri.Port};" +
                          $"Database={databaseUri.LocalPath.TrimStart('/')};" +
                          $"Username={userInfo[0]};" +
                          $"Password={userInfo[1]};" +
                          "SSL Mode=Require;" +
                          "Trust Server Certificate=true";
    
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

// Configure Kestrel to listen on Railway's dynamic port or default 8080
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// Add services to the container
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { 
        Title = "JIR Financial Management API", 
        Version = "v1",
        Description = "API pour la gestion financière JIR"
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7001", 
                "http://localhost:5001",
                "https://jir-blazor.fly.dev",
                "http://jir-blazor.fly.dev")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure JWT Authentication (will be implemented later)
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options => { ... });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Always enable Swagger in all environments for Railway
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "JIR API v1");
    c.RoutePrefix = "swagger"; // Access at /swagger
});

// Vérifier la connexion à la base de données au démarrage (sans bloquer)
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var canConnect = await context.Database.CanConnectAsync();
        
        if (canConnect)
        {
            logger.LogInformation("Database connection successful");
            logger.LogInformation("Use POST /api/seed/initialize to apply migrations and seed data");
        }
        else
        {
            logger.LogWarning("Cannot connect to database. Check your connection string.");
        }
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Database connection check failed. Use /api/seed/status to check database status.");
    }
}

// Don't use HTTPS redirection in container (Azure Container Apps handles HTTPS)
// app.UseHttpsRedirection();
app.UseCors("AllowBlazor");

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
