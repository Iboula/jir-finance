using JIR.BlazorApp.Models;
using System.Net.Http.Json;

namespace JIR.BlazorApp.Services;

public class SectionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SectionService> _logger;
    private const string BaseUrl = "/api/sections";

    public SectionService(HttpClient httpClient, ILogger<SectionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<SectionDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<SectionDto>>(BaseUrl);
            return response ?? new List<SectionDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des sections");
            throw;
        }
    }

    public async Task<SectionDto?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<SectionDto>($"{BaseUrl}/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération de la section {Id}", id);
            throw;
        }
    }

    public async Task<Guid> CreateAsync(CreateSectionDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateSectionResponse>();
            return result?.Id ?? Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de la section");
            throw;
        }
    }

    public async Task UpdateAsync(Guid id, UpdateSectionDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour de la section {Id}", id);
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
            _logger.LogError(ex, "Erreur lors de la suppression de la section {Id}", id);
            throw;
        }
    }
}

public class CreateSectionResponse
{
    public Guid Id { get; set; }
}
