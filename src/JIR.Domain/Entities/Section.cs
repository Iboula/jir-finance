using JIR.Domain.Common;

namespace JIR.Domain.Entities;

public class Section : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ResponsableId { get; set; }
    public decimal BudgetAnnuel { get; set; }

    // Navigation properties
    public User? Responsable { get; set; }
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<Cotisation> Cotisations { get; set; } = new List<Cotisation>();
    public ICollection<Depense> Depenses { get; set; } = new List<Depense>();
    public ICollection<Recette> Recettes { get; set; } = new List<Recette>();
}
