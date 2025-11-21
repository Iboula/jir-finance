using System.Net.Http.Json;
using JIR.MauiApp.Models;

namespace JIR.MauiApp.Services;

public class MouvementStockService
{
    private readonly HttpClient _httpClient;
    private const string BaseEndpoint = "mouvementsstock";

    public MouvementStockService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MouvementsStockListResult?> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        Guid? articleId = null,
        Guid? magasinId = null,
        TypeMouvement? typeMouvement = null,
        DateTime? dateDebut = null,
        DateTime? dateFin = null)
    {
        var query = $"{BaseEndpoint}?pageNumber={pageNumber}&pageSize={pageSize}";
        if (articleId.HasValue)
            query += $"&articleId={articleId.Value}";
        if (magasinId.HasValue)
            query += $"&magasinId={magasinId.Value}";
        if (typeMouvement.HasValue)
            query += $"&typeMouvement={typeMouvement.Value}";
        if (dateDebut.HasValue)
            query += $"&dateDebut={dateDebut.Value:yyyy-MM-dd}";
        if (dateFin.HasValue)
            query += $"&dateFin={dateFin.Value:yyyy-MM-dd}";

        return await _httpClient.GetFromJsonAsync<MouvementsStockListResult>(query);
    }

    public async Task<MouvementStockDto?> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<MouvementStockDto>($"{BaseEndpoint}/{id}");
    }

    public async Task<Guid> CreateAsync(CreateMouvementStockRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseEndpoint, request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseEndpoint}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
