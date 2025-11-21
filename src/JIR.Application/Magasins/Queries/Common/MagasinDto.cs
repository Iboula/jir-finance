namespace JIR.Application.Magasins.Queries.Common;

public class MagasinDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public string? ResponsableNom { get; set; }
}
