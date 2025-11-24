namespace JIR.BlazorApp.Models;

public class SectionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ResponsableId { get; set; }
    public decimal BudgetAnnuel { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSectionDto
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ResponsableId { get; set; }
    public decimal BudgetAnnuel { get; set; }
}

public class UpdateSectionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ResponsableId { get; set; }
    public decimal BudgetAnnuel { get; set; }
}
