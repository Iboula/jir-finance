using System.Net.Http.Json;
using JIR.BlazorApp.Models;

namespace JIR.BlazorApp.Services;

public class ArticleMagasinService
{
    private readonly HttpClient _httpClient;
    private const string BaseEndpoint = "articlesmagasin";

    public ArticleMagasinService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ArticlesMagasinListResult?> GetAllAsync(
        int pageNumber = 1, 
        int pageSize = 10, 
        Guid? magasinId = null,
        string? searchTerm = null)
    {
        var query = $"{BaseEndpoint}?pageNumber={pageNumber}&pageSize={pageSize}";
        if (magasinId.HasValue)
            query += $"&magasinId={magasinId.Value}";
        if (!string.IsNullOrEmpty(searchTerm))
            query += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";

        return await _httpClient.GetFromJsonAsync<ArticlesMagasinListResult>(query);
    }

    public async Task<ArticleMagasinDto?> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<ArticleMagasinDto>($"{BaseEndpoint}/{id}");
    }

    public async Task<Guid> CreateAsync(CreateArticleMagasinRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseEndpoint, request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task UpdateAsync(Guid id, UpdateArticleMagasinRequest request)
    {
        var command = new 
        {
            Id = id,
            CodeArticle = request.CodeArticle,
            Designation = request.Designation,
            Unite = request.Unite,
            SeuilAlerte = request.SeuilAlerte,
            PrixUnitaire = request.PrixUnitaire,
            Observations = request.Observations
        };
        var response = await _httpClient.PutAsJsonAsync($"{BaseEndpoint}/{id}", command);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseEndpoint}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
