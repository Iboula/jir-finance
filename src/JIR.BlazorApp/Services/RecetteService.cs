using System.Net.Http.Json;
using System.Web;
using JIR.BlazorApp.Models;

namespace JIR.BlazorApp.Services;

public class RecetteService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RecetteService> _logger;

    public RecetteService(HttpClient httpClient, ILogger<RecetteService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<RecetteDto>> GetAllAsync(
        Guid? sectionId = null,
        string? source = null,
        DateTime? dateDebut = null,
        DateTime? dateFin = null)
    {
        try
        {
            var queryParams = new List<string>();
            
            if (sectionId.HasValue)
                queryParams.Add($"sectionId={sectionId.Value}");
            
            if (!string.IsNullOrEmpty(source))
                queryParams.Add($"source={HttpUtility.UrlEncode(source)}");
            
            if (dateDebut.HasValue)
                queryParams.Add($"dateDebut={dateDebut.Value:yyyy-MM-dd}");
            
            if (dateFin.HasValue)
                queryParams.Add($"dateFin={dateFin.Value:yyyy-MM-dd}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var url = $"/api/recettes{query}";
            
            _logger.LogInformation("Fetching recettes from {Url}", url);
            var response = await _httpClient.GetFromJsonAsync<List<RecetteDto>>(url);
            return response ?? new List<RecetteDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching recettes");
            throw;
        }
    }

    public async Task<RecetteDto?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<RecetteDto>($"/api/recettes/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching recette {Id}", id);
            throw;
        }
    }

    public async Task<Guid> CreateAsync(CreateRecetteDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/recettes", dto);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Guid>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating recette");
            throw;
        }
    }

    public async Task UpdateAsync(Guid id, UpdateRecetteDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/recettes/{id}", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating recette {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/recettes/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting recette {Id}", id);
            throw;
        }
    }
}
