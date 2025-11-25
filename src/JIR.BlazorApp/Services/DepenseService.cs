using JIR.BlazorApp.Models;
using System.Net.Http.Json;

namespace JIR.BlazorApp.Services;

public class DepenseService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DepenseService> _logger;
    private const string BaseUrl = "/api/depenses";

    public DepenseService(HttpClient httpClient, ILogger<DepenseService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<DepenseDto>> GetAllAsync(Guid? sectionId = null, string? statut = null, 
        DateTime? dateDebut = null, DateTime? dateFin = null)
    {
        try
        {
            var queryParams = new List<string>();
            if (sectionId.HasValue)
                queryParams.Add($"sectionId={sectionId.Value}");
            if (!string.IsNullOrEmpty(statut))
                queryParams.Add($"statut={statut}");
            if (dateDebut.HasValue)
                queryParams.Add($"dateDepenseDebut={dateDebut.Value:yyyy-MM-dd}");
            if (dateFin.HasValue)
                queryParams.Add($"dateDepenseFin={dateFin.Value:yyyy-MM-dd}");

            var url = queryParams.Count > 0 ? $"{BaseUrl}?{string.Join("&", queryParams)}" : BaseUrl;
            var response = await _httpClient.GetFromJsonAsync<List<DepenseDto>>(url);
            return response ?? new List<DepenseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des dépenses");
            throw;
        }
    }

    public async Task<DepenseDto?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DepenseDto>($"{BaseUrl}/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération de la dépense {Id}", id);
            throw;
        }
    }

    public async Task<Guid> CreateAsync(CreateDepenseDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<CreateDepenseResponse>();
            return result?.Id ?? Guid.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de la dépense");
            throw;
        }
    }

    public async Task UpdateAsync(Guid id, UpdateDepenseDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour de la dépense {Id}", id);
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
            _logger.LogError(ex, "Erreur lors de la suppression de la dépense {Id}", id);
            throw;
        }
    }

    public async Task ValiderAsync(Guid id, string validePar, string? observations = null)
    {
        try
        {
            var dto = new ValiderDepenseDto
            {
                Id = id,
                ValidePar = validePar,
                Observations = observations
            };
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}/valider", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la validation de la dépense {Id}", id);
            throw;
        }
    }

    public async Task RejeterAsync(Guid id, string motifRejet)
    {
        try
        {
            var dto = new RejeterDepenseDto
            {
                Id = id,
                MotifRejet = motifRejet
            };
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}/rejeter", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du rejet de la dépense {Id}", id);
            throw;
        }
    }

    public async Task PayerAsync(Guid id, string modePaiement, DateTime? datePaiement = null)
    {
        try
        {
            var dto = new PayerDepenseDto
            {
                Id = id,
                ModePaiement = modePaiement,
                DatePaiement = datePaiement ?? DateTime.Now
            };
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}/payer", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du paiement de la dépense {Id}", id);
            throw;
        }
    }
}

public class CreateDepenseResponse
{
    public Guid Id { get; set; }
}
