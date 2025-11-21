namespace JIR.Application.Magasins.Queries.Common;

public class MagasinDetailDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public Guid? ResponsableId { get; set; }
    public string? ResponsableNom { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
