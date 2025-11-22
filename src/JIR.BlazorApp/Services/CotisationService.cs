using JIR.BlazorApp.Models;
using System.Net.Http.Json;

namespace JIR.BlazorApp.Services;

public class CotisationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CotisationService> _logger;
    private const string BaseUrl = "/api/cotisations";

    public CotisationService(HttpClient httpClient, ILogger<CotisationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<CotisationDto>> GetAllAsync(
        Guid? sectionId = null,
        string? periode = null,
        DateTime? datePaiementDebut = null,
        DateTime? datePaiementFin = null,
        int pageNumber = 1,
        int pageSize = 50)
    {
        try
        {
            var queryParams = new List<string>();
            
            if (sectionId.HasValue)
                queryParams.Add($"sectionId={sectionId.Value}");
            
            if (!string.IsNullOrEmpty(periode))
                queryParams.Add($"periode={periode}");
            
            if (datePaiementDebut.HasValue)
                queryParams.Add($"datePaiementDebut={datePaiementDebut.Value:yyyy-MM-dd}");
            
            if (datePaiementFin.HasValue)
                queryParams.Add($"datePaiementFin={datePaiementFin.Value:yyyy-MM-dd}");
            
            queryParams.Add($"pageNumber={pageNumber}");
            queryParams.Add($"pageSize={pageSize}");

            var url = $"{BaseUrl}?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetFromJsonAsync<List<CotisationDto>>(url);
            return response ?? new List<CotisationDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des cotisations");
            throw;
        }
    }

    public async Task<CotisationDetailDto?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CotisationDetailDto>($"{BaseUrl}/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération de la cotisation {Id}", id);
            throw;
        }
    }

    public async Task<Guid> CreateAsync(CreateCotisationDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
            response.EnsureSuccessStatusCode();
            var id = await response.Content.ReadFromJsonAsync<Guid>();
            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de la cotisation");
            throw;
        }
    }

    public async Task UpdateAsync(Guid id, UpdateCotisationDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour de la cotisation {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la suppression de la cotisation {Id}", id);
            throw;
        }
    }
}
