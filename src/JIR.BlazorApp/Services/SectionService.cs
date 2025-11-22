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
}
