namespace JIR.BlazorApp.Models;

public class MagasinDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public string? ResponsableNom { get; set; }
}

public class CreateMagasinRequest
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public Guid? ResponsableId { get; set; }
}

public class UpdateMagasinRequest
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public Guid? ResponsableId { get; set; }
}

public class MagasinsListResult
{
    public List<MagasinDto> Magasins { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
