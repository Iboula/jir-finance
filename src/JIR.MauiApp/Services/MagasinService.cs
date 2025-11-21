using System.Net.Http.Json;
using JIR.MauiApp.Models;

namespace JIR.MauiApp.Services;

public class MagasinService
{
    private readonly HttpClient _httpClient;
    private const string BaseEndpoint = "magasins";

    public MagasinService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MagasinsListResult?> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
    {
        var query = $"{BaseEndpoint}?pageNumber={pageNumber}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(searchTerm))
            query += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";

        return await _httpClient.GetFromJsonAsync<MagasinsListResult>(query);
    }

    public async Task<MagasinDto?> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<MagasinDto>($"{BaseEndpoint}/{id}");
    }

    public async Task<Guid> CreateAsync(CreateMagasinRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseEndpoint, request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task UpdateAsync(Guid id, UpdateMagasinRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseEndpoint}/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseEndpoint}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
