using JIR.Infrastructure.Data;
using JIR.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JIR.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SeedController> _logger;

    public SeedController(ApplicationDbContext context, ILogger<SeedController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Applique les migrations et seed la base de données
    /// </summary>
    /// <returns>Résultat de l'opération</returns>
    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize()
    {
        try
        {
            _logger.LogInformation("Starting database initialization...");

            // Vérifier la connexion
            var canConnect = await _context.Database.CanConnectAsync();
            if (!canConnect)
            {
                _logger.LogError("Cannot connect to database");
                return StatusCode(500, new { error = "Cannot connect to database" });
            }

            // Appliquer les migrations
            _logger.LogInformation("Applying database migrations...");
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                _logger.LogInformation($"Found {pendingMigrations.Count()} pending migrations");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Database migrations applied successfully");
            }
            else
            {
                _logger.LogInformation("No pending migrations found");
            }

            // Seed les données
            _logger.LogInformation("Seeding database...");
            await DatabaseSeeder.SeedAsync(_context);
            _logger.LogInformation("Database seeding completed successfully");

            return Ok(new
            {
                success = true,
                message = "Database initialized successfully",
                migrationsApplied = pendingMigrations.Count(),
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    /// <summary>
    /// Vérifie l'état de la base de données
    /// </summary>
    /// <returns>État de la base de données</returns>
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                return Ok(new
                {
                    connected = false,
                    message = "Cannot connect to database"
                });
            }

            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();

            // Vérifier si les données sont seedées
            var usersCount = await _context.Users.CountAsync();
            var sectionsCount = await _context.Sections.CountAsync();
            var cotisationsCount = await _context.Cotisations.CountAsync();
            var depensesCount = await _context.Depenses.CountAsync();
            var recettesCount = await _context.Recettes.CountAsync();

            return Ok(new
            {
                connected = true,
                pendingMigrations = pendingMigrations.ToList(),
                appliedMigrations = appliedMigrations.ToList(),
                pendingMigrationsCount = pendingMigrations.Count(),
                appliedMigrationsCount = appliedMigrations.Count(),
                usersCount,
                sectionsCount,
                cotisationsCount,
                depensesCount,
                recettesCount,
                isSeeded = usersCount > 0 || sectionsCount > 0,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking database status");
            return StatusCode(500, new
            {
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Endpoint de santé pour les health checks
    /// </summary>
    [HttpGet("/health")]
    public async Task<IActionResult> Health()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (canConnect)
            {
                return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
            }
            
            return StatusCode(503, new { status = "Unhealthy", reason = "Cannot connect to database" });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { status = "Unhealthy", error = ex.Message });
        }
    }
}
